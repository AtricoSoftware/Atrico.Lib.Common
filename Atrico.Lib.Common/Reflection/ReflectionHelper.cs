using System;
using System.Linq;
using System.Reflection;

namespace Atrico.Lib.Common.Reflection
{
    public static class ReflectionHelper
    {
        public static T GetCustomAttribute<T>(this MemberInfo member, bool inherit = false) where T : Attribute
        {
            return member.GetCustomAttributes(typeof (T), inherit).FirstOrDefault() as T;
        }
    }
}
