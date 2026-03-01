using System.Collections.Generic;
using sql.builder.DataApi;

namespace sql.builder
{
    public static partial class Compiler
    {
        private static Dictionary<string, object> cash = new Dictionary<string, object>();
        private static void AddCashValue(object val, string methodName, string parmsInfo)
        {
            VCashUtils.AddCashValue(cash, val, methodName, parmsInfo);
        }

        private static bool IsCashValueExists(string methodName, string parmsInfo)
        {
            return VCashUtils.IsCashValueExists(cash, methodName, parmsInfo);
        }

        private static object GetCashValue(string methodName, string parmsInfo)
        {
            return VCashUtils.GetCashValue(cash, methodName, parmsInfo);
        }

    }
}
