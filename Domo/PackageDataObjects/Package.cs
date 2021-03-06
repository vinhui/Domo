﻿using Domo.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domo.Packaging
{
    public class Package
    {
        public PackageManifest manifest { get; private set; }
        public string path { get; private set; }
        public ScriptEngine engine { get; private set; }

        public Package(PackageManifest manifest, string path, ScriptEngine engine)
        {
            this.manifest = manifest;
            this.path = path;
            this.engine = engine;
        }
    }
}
