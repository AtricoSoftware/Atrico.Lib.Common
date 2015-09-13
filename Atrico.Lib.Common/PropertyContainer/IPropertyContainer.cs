﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Atrico.Lib.Common.PropertyContainer
{
    public interface IPropertyContainer : INotifyPropertyChanged
    {
        T Get<T>([CallerMemberName] string name = null);
        void Set<T>(T value, [CallerMemberName] string name = null);
    }
}