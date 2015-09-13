using System.Collections.Generic;
using System.ComponentModel;
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

        public T Get<T>([CallerMemberName]string name = null)
        {
            if (!Properties.ContainsKey(name)) Properties[name] = CreateInitialValue<T>();
            return GetValue<T>(name);
        }

        public void Set<T>(T value, [CallerMemberName]string name = null)
        {
            var oldvalue = Get<T>(name);
            if (Equals(oldvalue, value)) return;
            SetValue(name, value);
            OnPropertyChanged(name);
        }

        protected abstract TProp CreateInitialValue<T>();
        protected abstract T GetValue<T>(string name);
        protected abstract void SetValue<T>(string name, T value);


        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(_owner, new PropertyChangedEventArgs(propertyName));
        }
    }
}