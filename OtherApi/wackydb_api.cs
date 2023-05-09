using BepInEx.Bootstrap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EpicMMOSystem.OtherApi
{
    public class WackyDatabase_API
    {

        private static readonly bool _IsInstalled;
        private static MethodInfo eAddBlacklistClone;


        public static bool IsInstalled() => _IsInstalled;
        public static void AddBlacklistClone(string value)
        {
            eAddBlacklistClone?.Invoke(null, new object[] { value });
        }

        static WackyDatabase_API()
        {

            if (Type.GetType("API.WackyAPI, WackysDatabase") == null)
            //if (Type.GetType("API.WackyAPI") is not { } wackydatabaseAPI)
            {
                _IsInstalled = false;
                return;
            }
            Type wackydatabaseAPI = Type.GetType("API.WackyAPI, WackysDatabase");
            _IsInstalled = true;
            eAddBlacklistClone = wackydatabaseAPI.GetMethod("AddBlacklistClone", BindingFlags.Public | BindingFlags.Static);
        }

    }
}



