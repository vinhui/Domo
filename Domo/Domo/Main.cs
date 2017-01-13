using Domo.API;
using Domo.Packaging;

namespace Domo
{
    public static class Main
    {
        public static void Init()
        {
            //ApiManager.Init();

            PackageManager pm = new PackageManager();
            pm.LoadPackages();

        }

        public static void ShutDown()
        {
            ApiManager.OnShutdown();
        }
    }
}