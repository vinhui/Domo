namespace Domo.Packaging
{
    public class PackageManifest
    {
        public readonly string name;
        public readonly string description;
        public readonly string iconPath;
        public readonly string[] permissions;
        public readonly string[] dependencies;
        public readonly string[] executionPriority;
    }
}