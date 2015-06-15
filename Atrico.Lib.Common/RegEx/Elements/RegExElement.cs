namespace Atrico.Lib.Common.RegEx.Elements
{
    public abstract partial class RegExElement
    {
        /// <summary>
        ///     Creates an element from a list of characters
        /// </summary>
        /// <param name="chars">The characters</param>
        /// <returns>New element</returns>
        public static RegExElement Create(params char[] chars)
        {
            return new RegExChars(chars);
        }

        /// <summary>
        /// Creates an OR element
        /// </summary>
        /// <param name="elements">The reg ex elements</param>
        /// <returns>New element</returns>
        public static RegExElement CreateOr(params RegExElement[] elements)
        {
            return RegExAlternation.Create(elements);
        }

        public static RegExElement CreateSequence(params RegExElement[] elements)
        {
            return RegExSequence.Create(elements);
        }
    }
}