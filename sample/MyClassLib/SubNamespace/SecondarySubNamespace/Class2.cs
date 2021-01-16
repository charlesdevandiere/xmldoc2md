using System;
using System.Collections.Generic;
using System.Text;

namespace MyClassLib.SubNamespace.SecondarySubNamespace
{
    /// <summary>
    /// Class3
    /// </summary>
    public class Class2
    {
        private static readonly Lazy<MyClass> staticInstance = new Lazy<MyClass>(() => new MyClass());

        /// <summary>
        /// Represents the static instance of <see cref="MyClass"/>.
        /// </summary>
        public static MyClass ProviderInstance => staticInstance.Value;

        /// <summary>
        /// Gets the value of <see cref="MyClass.MyEnum"/> of <see cref="ProviderInstance"/>.
        /// </summary>
        public MyEnum StaticProvidedClass1 => ProviderInstance.MyEnum;
    }
}
