
namespace RM.Resources.Annotations
{
    using System;

    [AttributeUsage(AttributeTargets.Parameter)]
    public class PathReferenceAttribute : Attribute
    {
        [NotNull]
        public string BasePath
        {
            get;
            private set;
        }

        public PathReferenceAttribute()
        {
        }

        public PathReferenceAttribute([PathReference] string basePath)
        {
            this.BasePath = basePath;
        }
    }
}
