using System.Collections.Generic;

namespace Atrico.Lib.Common
{
    /// <summary>
    ///     An object which can be displayed on multiple lines of text
    /// </summary>
    public interface IMultilineDisplayable
    {
        /// <summary>
        ///     As ToString but output ismultiline
        /// </summary>
        /// <returns>Lines of text</returns>
        IEnumerable<string> ToMultilineString();
    }
}