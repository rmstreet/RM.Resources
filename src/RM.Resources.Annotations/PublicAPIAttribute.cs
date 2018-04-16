
namespace RM.Resources.Annotations
{
    using System;

    [MeansImplicitUse]
    public sealed class PublicAPIAttribute : Attribute
    {
        [NotNull]
        public string Comment
        {
            get;
            private set;
        }

        public PublicAPIAttribute()
        {
        }

        public PublicAPIAttribute([NotNull] string comment)
        {
            this.Comment = comment;
        }
    }
}
