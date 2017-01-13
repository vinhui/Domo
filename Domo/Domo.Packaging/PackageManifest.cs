using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domo.Packaging
{
    public class PackageManifest
    {
        public readonly string name;
        public readonly string description;
        public readonly string iconPath;
        public readonly string[] permissions;
        public readonly string[] dependencies;
    }
}
