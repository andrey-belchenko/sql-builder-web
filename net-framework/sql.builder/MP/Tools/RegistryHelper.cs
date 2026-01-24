using System;
//using System.Windows.Forms;
using Microsoft.Win32;

namespace sql.builder.MP.Tools
{
    public static class RegistryHelper
    {
        static string _applicationPath;

        static RegistryHelper()
        {
            //_applicationPath = string.Format(@"{0}\{1}", Environment.UserName, Application.ProductName);
        }

        public static string GetStringValue(string itemName, string folderName = null)
        {
            throw new NotImplementedException();
            //string path = _applicationPath;
            //if(folderName != null) path += @"\" + folderName;

            //using(var regKey = Registry.CurrentUser.CreateSubKey(path, RegistryKeyPermissionCheck.ReadSubTree))
            //{
            //    object val = regKey.GetValue(itemName);
            //    return (val != null) ? val.ToString() : null;
            //}
        }

        public static void SetStringValue(string itemName, string value, string folderName = null)
        {
            //string path = _applicationPath;
            //if (folderName != null) path += @"\" + folderName;

            //using (var regKey = Registry.CurrentUser.CreateSubKey(path, RegistryKeyPermissionCheck.ReadWriteSubTree))
            //{
            //    regKey.SetValue(itemName, value);
            //}
        }
    }
}