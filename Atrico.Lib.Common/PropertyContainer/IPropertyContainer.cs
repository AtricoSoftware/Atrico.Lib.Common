using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Atrico.Lib.Common.PropertyContainer
{
    /// <summary>
    /// Property container with notifications
    /// </summary>
    public interface IPropertyContainer : INotifyPropertyChanged, IEquatable<IPropertyContainer>
    {
        /// <summary>
        /// Get a property of the specified type
        /// </summary>
        /// <typeparam name="T">Type of property</typeparam>
        /// <param name="name">Name of property</param>
        /// <returns>Property value</returns>
        T Get<T>([CallerMemberName] string name = null);

        /// <summary>
        /// Set a property
        /// </summary>
        /// <typeparam name="T">Type of property</typeparam>
        /// <param name="value">New value for property</param>
        /// <param name="name">Name of property</param>
        void Set<T>(T value, [CallerMemberName] string name = null);

        /// <summary>
        /// Collection of property names
        /// </summary>
        IEnumerable<string> Names { get; }
    }
}