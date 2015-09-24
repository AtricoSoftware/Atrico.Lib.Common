using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Atrico.Lib.Common.PropertyContainer
{
    /// <summary>
    /// Property container
    /// Handles all properties for object including change notifications and equality
    /// <example>
    /// Example of usage:
    /// <code>
    /// class MyObject : INotifyPropertyChanged, IEquatable&lt;MyObject&gt;
    /// {
    ///     private readonly PropertyContainer _properties;
    /// 
    ///     public event PropertyChangedEventHandler PropertyChanged
    ///     {
    ///         add { _properties.PropertyChanged += value; }
    ///         remove { _properties.PropertyChanged -= value; }
    ///     }
    /// 
    ///     public MyObject()
    ///     {
    ///         _properties = new PropertyContainer(this);
    ///     }
    /// 
    ///     public int Property1
    ///     {
    ///         get { return _properties.Get&lt;int&gt;(); }
    ///         set { _properties.Set(value); }
    ///     }
    /// 
    ///     public bool Equals(MyObject other)
    ///     {
    ///         return _properties.Equals(other._properties);
    ///     }
    /// }
    /// </code>
    /// </example>
    /// </summary>
    public class PropertyContainer : EquatableObject<PropertyContainer>, INotifyPropertyChanged
    {
        private readonly object _owner;
        private readonly IDictionary<string, object> _properties = new Dictionary<string, object>();

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="owner">Owning object (for notifications)</param>
        public PropertyContainer(object owner)
        {
            _owner = owner;
        }

        /// <summary>
        /// Get a property of the specified type
        /// </summary>
        /// <typeparam name="T">Type of property</typeparam>
        /// <param name="name">Name of property</param>
        /// <returns>Property value</returns>
        public T Get<T>([CallerMemberName] string name = null)
        {
            return _properties.ContainsKey(name) ? (T) _properties[name] : default(T);
        }

        /// <summary>
        /// Set a property
        /// </summary>
        /// <typeparam name="T">Type of property</typeparam>
        /// <param name="value">New value for property</param>
        /// <param name="name">Name of property</param>
        public void Set<T>(T value, [CallerMemberName] string name = null)
        {
            // ReSharper disable once ExplicitCallerInfoArgument
            var oldvalue = Get<T>(name);
            if (Equals(oldvalue, value))
            {
                return;
            }
            _properties[name] = value;
            OnPropertyChanged(name);
        }

        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(_owner, new PropertyChangedEventArgs(propertyName));
            }
        }

        #region Equality

        protected override int GetHashCodeImpl()
        {
            var hashcode = 17;
            foreach (var entry in _properties.OrderBy(e => e.Key))
            {
                hashcode = (hashcode * 31) ^ entry.Key.GetHashCode();
                hashcode = (hashcode * 31) ^ (ReferenceEquals(entry.Value, null) ? 0 : entry.Value.GetHashCode());
            }
            return hashcode;
        }

        protected override bool EqualsImpl(PropertyContainer other)
        {
            if (ReferenceEquals(other, null) || GetType() != other.GetType())
            {
                return false;
            }
            var names = _properties.Keys.Concat(other._properties.Keys).Distinct();
            // ReSharper disable ExplicitCallerInfoArgument
            return names.All(name => Equals(Get<object>(name), other.Get<object>(name)));
            // ReSharper restore ExplicitCallerInfoArgument
        }

        #endregion
    }
}