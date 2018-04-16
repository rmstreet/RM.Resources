
namespace RM.Resources.Annotations
{
    using System;

    [BaseTypeRequired(typeof(Attribute)), AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class BaseTypeRequiredAttribute : Attribute
    {
        [NotNull]
        public Type BaseType
        {
            get;
            private set;
        }

        public BaseTypeRequiredAttribute([NotNull] Type baseType)
        {
            this.BaseType = baseType;
        }
    }
}
