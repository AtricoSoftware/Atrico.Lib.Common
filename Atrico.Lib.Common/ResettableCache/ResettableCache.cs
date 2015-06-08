using System;
using System.Collections.Generic;

namespace Atrico.Lib.Common.ResettableCache
{
    /// <summary>
    ///     Cached value with create/update function
    ///     Equivalent to Lazy generic with function to reset value
    /// </summary>
    /// <typeparam name="T">Type of value</typeparam>
    public class ResettableCache<T> : IResettable
    {
        private readonly Func<T> _evaluator;
        private readonly IEnumerable<IResettable> _dependencies;
        private Lazy<T> _lazy;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="evaluator"></param>
        /// <param name="dependencies"></param>
        public ResettableCache(Func<T> evaluator, params IResettable[] dependencies)
        {
            _evaluator = evaluator;
            _dependencies = dependencies;
            Reset();
        }

        /// <summary>
        ///     Get the cached value
        ///     Value is lazy evaluated
        /// </summary>
        public T Value
        {
            get { return _lazy.Value; }
        }

        /// <summary>
        ///     Is the value currently valid
        /// </summary>
        public bool IsValueCreated
        {
            get { return _lazy.IsValueCreated; }
        }

        /// <summary>
        ///     Reset the value
        ///     This will force the value to be re-evaluated at next access
        ///     All dependencies will also be reset
        /// </summary>
        public void Reset()
        {
            _lazy = new Lazy<T>(_evaluator);
            foreach (var dep in _dependencies) dep.Reset();
        }
    }

}
