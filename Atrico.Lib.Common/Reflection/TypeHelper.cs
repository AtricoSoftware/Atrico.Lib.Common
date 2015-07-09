using System;

namespace Atrico.Lib.Common.Reflection
{
    public static class TypeHelper
    {
        /// <summary>
        /// Determines whether this type is nullable
        /// </summary>
        /// <param name="type">The type</param>
        public static bool IsNullable(this Type type)
        {
            Type dummy;
            return IsNullable(type, out dummy);
        }

        /// <summary>
        /// Determines whether this type is nullable
        /// </summary>
        /// <param name="type">The type</param>
        /// <param name="underlyingType">Returns the underlying type</param>
        public static bool IsNullable(this Type type, out Type underlyingType)
        {
            if (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof (Nullable<>))
            {
                underlyingType = null;
                return false;
            }
            underlyingType = type.GetGenericArguments()[0];
            return true;
        }

        /// <summary>
        /// Determines whether this type is an enum
        /// </summary>
        /// <param name="type">The type</param>
        public static bool IsEnum(this Type type)
        {
            return type.BaseType == typeof (Enum);
        }
    }
}
