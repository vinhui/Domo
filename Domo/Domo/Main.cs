using Domo.API;

namespace Domo
{
    public static class Main
    {
        public static void Init()
        {
            ApiManager.Init();
        }

        public static void ShutDown()
        {
            ApiManager.OnShutdown();
        }
    }
}