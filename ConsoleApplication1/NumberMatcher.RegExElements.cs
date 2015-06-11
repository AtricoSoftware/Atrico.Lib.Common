using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atrico.Lib.Common.Collections.Tree;
using Atrico.Lib.Common.NamesByConvention;

namespace ConsoleApplication1
{
    public partial class NumberMatcher
    {
        private abstract class RegExElement
        {
            public static RegExElement Create(Tree<char>.INode node)
            {
                // Root node
                if (node.IsRoot()) return new RegExOr(node.Children.Select(Create));
                var thisDigit = new RegExDigits(new[]{node.Data});
                // Leaf
                if (!node.Children.Any()) return thisDigit;
                // Non terminal
                return new RegExAnd(new RegExElement[] {thisDigit, new RegExOr(node.Children.Select(Create))});
            }

            public virtual RegExElement Simplify()
            {
                return this;
            }

            public Tree<string>.INode ToTree()
            {
                var tree = Tree<string>.Create(true);
                AddNodeToTree(tree);
                return tree;
            }

            protected virtual Tree<string>.INode AddNodeToTree(Tree<string>.INode container)
            {
                return container.Add(ToString());
            }

            private abstract class RegExComposite : RegExElement
            {
                internal readonly IEnumerable<RegExElement> Elements;

                protected RegExComposite(IEnumerable<RegExElement> elements)
                {
                    Elements = elements.Where(el => el != null);
                }

                protected override Tree<string>.INode AddNodeToTree(Tree<string>.INode container)
                {
                    var node = container.Add(new EverythingAfter(GetType().Name, "RegEx").ToString());
                    foreach (var element in Elements)
                    {
                        element.AddNodeToTree(node);
                    }
                    return node;
                }

                public override RegExElement Simplify()
                {
                    // Simplify all children
                    var simplifiedComp = Create(Elements.Select(el => el.Simplify()));
                    // Merge same operator
                    var simplified = MergeSameOperator(simplifiedComp);
                    simplifiedComp = simplified as RegExComposite;
                    if (simplifiedComp == null) return simplified;
                    // Remove composite for single child
                    return simplifiedComp.Elements.Count() == 1 ? simplifiedComp.Elements.First() : simplified;
                }

                private RegExElement MergeSameOperator(RegExComposite simplified)
                {
                    var thisType = simplified.GetType();
                    if (simplified.Elements.Any(el => el.GetType() != thisType)) return simplified;
                    return Create(simplified.Elements.Cast<RegExComposite>().SelectMany(comp=>comp.Elements));
                }

                public override int GetHashCode()
                {
                    return Elements.Aggregate(0, (current, item) => current ^ item.GetHashCode());
                }

                protected abstract RegExComposite Create(IEnumerable<RegExElement> elements);

                protected bool EqualsImpl(RegExComposite other)
                {
                    if (other == null) return false;
                    var thisEn = Elements.GetEnumerator();
                    var otherEn = other.Elements.GetEnumerator();
                    var more = false;
                    do
                    {
                        more = thisEn.MoveNext();
                        if (otherEn.MoveNext() != more) return false;
                        if (!more) continue;
                        if (thisEn.Current != otherEn.Current) return false;
                    } while (more);
                    return true;
                }

                public override string ToString()
                {
                    var text = new StringBuilder();
                    text.Append("(");
                    var first = true;
                    foreach (var el in Elements)
                    {
                        if (!first) text.Append(Separator);
                        else first = false;
                        text.Append(el);
                    }
                    text.Append(")");
                    return text.ToString();
                }

                protected abstract string Separator { get; }
            }

            private class RegExAnd : RegExComposite
            {
                public RegExAnd(IEnumerable<RegExElement> elements)
                    : base(elements)
                {
                }

                protected override RegExComposite Create(IEnumerable<RegExElement> elements)
                {
                    return new RegExAnd(elements);
                }

                public override int GetHashCode()
                {
                    return base.GetHashCode();
                }

                public override bool Equals(object obj)
                {
                    return EqualsImpl(obj as RegExAnd);
                }

                protected override string Separator
                {
                    get { return " & "; }
                }
            }

            private class RegExOr : RegExComposite
            {
                public RegExOr(IEnumerable<RegExElement> elements)
                    : base(elements)
                {
                }

                public override RegExElement Simplify()
                {
                    var simplified = base.Simplify();
                    var or = simplified as RegExOr;
                    if (or == null) return simplified;
                    // Merge digits
                    var digits = or.Elements.Where(dg => dg is RegExDigits).Cast<RegExDigits>().ToArray();
                    if (digits.Count() > 1) return new RegExOr(or.Elements.Where(dg => !(dg is RegExDigits)).Concat(new[] {new RegExDigits(digits.SelectMany(dg => dg.Digits))})).Simplify();
                    // (A & B) | (A & C) => A & (B | C)                   
                    simplified = MergeOrAnds(or);
                    return simplified;
                }

                // (A & B) | (A & C) => A & (B | C)                   
                private static RegExElement MergeOrAnds(RegExComposite or)
                {
                    var elements = or.Elements.ToArray();
                    var changes = false;
                    bool changesLoop;
                    do
                    {
                        changesLoop = false;
                        for (var i = 0;!changesLoop && i < elements.Length; ++i)
                        {
                            var and1 = elements[i] as RegExAnd;
                            if (and1 != null)
                            {
                                var elements1 = and1.Elements.ToArray();
                                for (var j = i + 1; j < elements.Length; ++j)
                                {
                                    var and2 = elements[j] as RegExAnd;
                                    if (and2 != null)
                                    {
                                        var elements2 = and2.Elements.ToArray();
                                        if (elements1.Length == elements2.Length)
                                        {
                                            var compare = elements1.Zip(elements2, (el1, el2) => el1.Equals(el2)).ToArray();
                                            if (compare.Count(v => !v) == 1)
                                            {
                                                changes = changesLoop = true;
                                                var newElements = compare.Select((val, idx) => val ? elements1[idx] : new RegExOr(new[] {elements1[idx], elements2[idx]})).ToArray();
                                                elements[i] = new RegExAnd(newElements);
                                                elements[j] = null;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    } while (changesLoop);

                    return changes ? new RegExOr(elements).Simplify() : or;
                }

                protected override RegExComposite Create(IEnumerable<RegExElement> elements)
                {
                    return new RegExOr(elements);
                }

                public override int GetHashCode()
                {
                    return base.GetHashCode();
                }

                public override bool Equals(object obj)
                {
                    return EqualsImpl(obj as RegExOr);
                }

                protected override string Separator
                {
                    get { return " | "; }
                }
            }

            private class RegExDigits : RegExElement
            {
                public IEnumerable<char> Digits { get; private set; }
                private readonly Lazy<string> _regex;

                public RegExDigits(IEnumerable<char> digits)
                {
                    Digits = digits.Distinct().OrderBy(ch => ch);
                    _regex = new Lazy<string>(CreateRegex);
                }

                private string CreateRegex()
                {
                    return Digits.Count() == 10 ? @"\d" : new Simplifier(Digits).ToString();
                }

                public override int GetHashCode()
                {
                    return Digits.Aggregate(0, (current, ch) => current ^ ch.GetHashCode());
                }

                public override bool Equals(object obj)
                {
                    var other = obj as RegExDigits;
                    if (other == null) return false;
                    var thisEn = Digits.GetEnumerator();
                    var otherEn = other.Digits.GetEnumerator();
                    var more = false;
                    do
                    {
                        more = thisEn.MoveNext();
                        if (otherEn.MoveNext() != more) return false;
                        if (!more) continue;
                        if (thisEn.Current != otherEn.Current) return false;
                    } while (more);
                    return true;
                }

                public override string ToString()
                {
                    return _regex.Value;
                }

                private class Simplifier
                {
                    private enum State
                    {
                        Empty,
                        AddingChar,
                        NoRange,
                        Range,
                        EndingRange
                    }

                    private State _state = State.Empty;
                    private readonly StringBuilder _regex = new StringBuilder();
                    private char _last = '\0';

                    public Simplifier(IEnumerable<char> chars)
                    {
                        foreach (var ch in chars) AddChar(ch);
                    }

                    private void AddChar(char ch)
                    {
                        var done = false;
                        do
                        {
                            switch (_state)
                            {
                                case State.Empty:
                                    _regex.Append('[');
                                    _state = State.AddingChar;
                                    break;
                                case State.AddingChar:
                                    _regex.AppendFormat("{0}", ch);
                                    _state = State.NoRange;
                                    done = true;
                                    break;
                                case State.NoRange:
                                    if (ch == _last + 1)
                                    {
                                        _state = State.Range;
                                        done = true;
                                    }
                                    else
                                    {
                                        _regex.Append(',');
                                        _state = State.AddingChar;
                                    }
                                    break;
                                case State.Range:
                                    if (ch == _last + 1)
                                    {
                                        done = true;
                                    }
                                    else
                                    {
                                        _state = State.EndingRange;
                                    }
                                    break;
                                case State.EndingRange:
                                    _regex.AppendFormat("-{0}", _last);
                                    _state = State.NoRange;
                                    break;
                            }
                        } while (!done);

                        _last = ch;
                    }

                    public override string ToString()
                    {
                        switch (_state)
                        {
                            case State.NoRange:
                                _regex.Append(']');
                                break;
                            case State.Range:
                                _regex.AppendFormat("-{0}]", _last);
                                break;
                        }
                        return _regex.ToString();
                    }
                }
            }

         }
    }
}