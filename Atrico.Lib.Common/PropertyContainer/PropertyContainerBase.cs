using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Atrico.Lib.Common.PropertyContainer
{
    public abstract class PropertyContainerBase<TProp> : IPropertyContainer
    {
        private readonly object _owner;
        protected readonly IDictionary<string, TProp> Properties = new Dictionary<string, TProp>();

        public event PropertyChangedEventHandler PropertyChanged;

        protected PropertyContainerBase(object owner)
        {
            _owner = owner;
        }

        public T Get<T>([CallerMemberName] string name = null)
        {
            return !Properties.ContainsKey(name) ? default(T) : GetValue<T>(name);
        }

        public void Set<T>(T value, [CallerMemberName] string name = null)
        {
            // ReSharper disable once ExplicitCallerInfoArgument
            var oldvalue = Get<T>(name);
            if (Equals(oldvalue, value))
            {
                return;
            }
            SetValue(name, value);
            OnPropertyChanged(name);
        }

        public IEnumerable<string> Names
        {
            get { return Properties.Keys; }
        }

        protected abstract T GetValue<T>(string name);
        protected abstract void SetValue<T>(string name, T value);

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(_owner, new PropertyChangedEventArgs(propertyName));
            }
        }

        #region Equality

        public override bool Equals(object obj)
        {
            return Equals(obj as IPropertyContainer);
        }

        public override int GetHashCode()
        {
            var hashcode = 17;
            foreach (var entry in Properties.OrderBy(e => e.Key))
            {
                hashcode = (hashcode * 31) ^ entry.Key.GetHashCode();
                hashcode = (hashcode * 31) ^ (ReferenceEquals(entry.Value, null) ? 0 : entry.Value.GetHashCode());
            }
            return hashcode;
        }

        public virtual bool Equals(IPropertyContainer other)
        {
            if (ReferenceEquals(other, null) || GetType() != other.GetType())
            {
                return false;
            }
            var names = Names.Concat(other.Names).Distinct();
            // ReSharper disable ExplicitCallerInfoArgument
            return names.All(name => Equals(Get<object>(name), other.Get<object>(name)));
            // ReSharper restore ExplicitCallerInfoArgument
        }

        #endregion
    }
}