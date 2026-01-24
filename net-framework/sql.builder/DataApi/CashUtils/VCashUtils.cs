using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.IO;
////using System.Windows.Forms;
using Devart.Data.Oracle;
using sql.builder.FieldInfo;
using System.Reflection;
using System.Diagnostics;

namespace sql.builder.DataApi
{
    internal static class  VCashUtils
    {

        public static bool CashEnabled 
        {
            get
            {
#if DEBUG

                return true;

#else

               return true;

#endif

            }
        
        }// = false;
        private static Dictionary<int, Dictionary<string, object>> CashLists = new Dictionary<int, Dictionary<string, object>>();

        public static void ClearCash()
        {
            ClearCashNotErrors();
            VSXElement.SetElemntsWithError(null);
        }

        public static void ClearCashNotErrors()
        {
            foreach (var cash in CashLists.Values)
            {
                cash.Clear();
            }
            CashLists.Clear();
        }



        public static void AddCashValue(Dictionary<string, object> cash, object val, string methodName, string parmsInfo)
        {
            if (!CashLists.ContainsKey(cash.GetHashCode()))
            {
                CashLists.Add(cash.GetHashCode(), cash);
            }
            if (!CashEnabled) return;
            //StackTrace stackTrace = new StackTrace();
            //string name = stackTrace.GetFrame(2).GetMethod().Name;
            string name = methodName;
            if (parmsInfo != null)
            {
                name += "|" + parmsInfo;
            }
            
            // Емцов - вылетает из-за неуникальности ключа 
            // release 33984-1 казань сети

            cash[name]= val;
        }

        public static bool IsCashValueExists(Dictionary<string, object> cash, string methodName, string parmsInfo)
        {
            if (!CashEnabled) return false;
            if (cash == null) return false;

            //StackTrace stackTrace = new StackTrace();
            //string name = stackTrace.GetFrame(2).GetMethod().Name;
            string name = methodName;
            if (parmsInfo != null)
            {
                name += "|" + parmsInfo;
            }
            return cash.ContainsKey(name);
        }

        public static object GetCashValue(Dictionary<string, object> cash, string methodName, string parmsInfo)
        {

            //StackTrace stackTrace = new StackTrace();
            //string name = stackTrace.GetFrame(2).GetMethod().Name;
            string name = methodName;
            if (parmsInfo != null)
            {
                name += "|" + parmsInfo;
            }
            return cash[name];

        }


 
    }


    internal partial class VSXElement : VXElement
    {


        private Dictionary<string, object> cash = new Dictionary<string, object>();
        protected void AddCashValue(object val, string methodName, string parmsInfo)
        {
            VCashUtils.AddCashValue(cash, val,methodName, parmsInfo);
        }

        protected bool IsCashValueExists(string methodName, string parmsInfo)
        {
            return VCashUtils.IsCashValueExists(cash,methodName, parmsInfo);
        }

        protected object GetCashValue(string methodName, string parmsInfo)
        {
            return VCashUtils.GetCashValue(cash, methodName, parmsInfo);
        }

        public void ClearCash()
        {
            cash = new Dictionary<string, object>();
            //staticCash = new SortedList<string, object>();
        }


        private static Dictionary<string, object> staticCash = new Dictionary<string, object>();
        protected static void AddStaticCashValue(object val, string methodName, string parmsInfo)
        {
            VCashUtils.AddCashValue(staticCash, val, methodName, parmsInfo);
        }

        protected static bool IsStaticCashValueExists(string methodName, string parmsInfo)
        {
            return VCashUtils.IsCashValueExists(staticCash, methodName, parmsInfo);
        }

        protected static object GetStaticCashValue(string methodName, string parmsInfo)
        {
            return VCashUtils.GetCashValue(staticCash, methodName, parmsInfo);
        }



    }


    internal partial class VEntityType
    {
        private Dictionary<string, object> cash = new Dictionary<string, object>();
        private void AddCashValue(object val, string methodName, string parmsInfo)
        {
            VCashUtils.AddCashValue(cash, val, methodName, parmsInfo);
        }
        private bool IsCashValueExists(string methodName, string parmsInfo)
        {
            return VCashUtils.IsCashValueExists(cash, methodName, parmsInfo);
        }
        private object GetCashValue(string methodName, string parmsInfo)
        {
            return VCashUtils.GetCashValue(cash, methodName, parmsInfo);
        }
    }


    internal partial class VEnvironment
    {


        private Dictionary<string, object> cash = new Dictionary<string, object>();
        protected void AddCashValue(object val, string methodName, string parmsInfo)
        {
            VCashUtils.AddCashValue(cash, val, methodName, parmsInfo);
        }

        protected bool IsCashValueExists(string methodName, string parmsInfo)
        {
            return VCashUtils.IsCashValueExists(cash, methodName, parmsInfo);
        }

        protected object GetCashValue(string methodName, string parmsInfo)
        {
            return VCashUtils.GetCashValue(cash, methodName, parmsInfo);
        }
    }



    
}
