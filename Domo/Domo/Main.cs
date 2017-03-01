using Domo.API;
using Domo.ApiTest;
using Domo.Misc.Debug;
using Domo.Modules;
using Domo.Packaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Domo
{
    public static class Main
    {
        public static void Init()
        {
            //ApiManager.Init();

            PackageManager pm = new PackageManager();
            pm.LoadPackages();

            ModuleManager.Init();
            ModuleManager.LoadAllModules(pm.packages);
            ApiTestSelfHost api = new ApiTestSelfHost(pm.packages);
        }

        public static void ShutDown()
        {
            ApiTestSelfHost.Unload();
            ModuleManager.ShutDown();
            //ApiManager.OnShutdown();
            Log.OnShutDown();
        }
    }
}