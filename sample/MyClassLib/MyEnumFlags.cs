using System;

namespace MyClassLib
{
    /// <summary>
    /// My Enum with Atrib Flags
    /// </summary>
    [Flags]
    public enum MyEnumFlags
    {
        /// <summary>
        /// The default
        /// </summary>
        Default = 0,

        /// <summary>
        /// The first.
        /// </summary>
        First = 1,

        /// <summary>
        /// The second.
        /// </summary>
        Second = 2,

        /// <summary>
        /// The Third.
        /// </summary>
        Third = 4
    }
}
