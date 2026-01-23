//using System.Collections.Generic;
//using System.Linq;
//using infoenergo.core;


//namespace sql.builder.DataApi
//{
//    internal static class VSecurityUtils 
//    {
//        public static string[] GetUserRoles(bool write)
//        {
//            var access_level = (write) ? 2 : 1;

//            if (Security.Menu == null || Security.Menu.Count==0 /*при запуске из веба получилось 0*/) Security.LoadPermissions(db.Connection.UserId, db.Connection);
//            var roles =  Security.Menu.Where(m => (int) m.Value >= access_level).Select(m => m.Key.ToString("G0")).ToArray();

//            //var list = new List<string>();
//            //list.Add("test1");
//            //list.Add("test2");
//            //list.Add("test3");
//            return roles;
//        }

//        public static void ReloadUserRoles()
//        {
//            if (Security.Menu == null) Security.LoadPermissions(db.Connection.UserId, db.Connection);
//            else Security.RefreshPermissions(db.Connection.UserId, db.Connection);
//        }

//        public static string[] GetNecessaryRoles(string securityId, bool write)
//        {
//            List<VRole> roles = XmlReports.Environment.GetRoles();
//            string[] names = (write) 
//                ? roles.Where(r => r.HasWritePermission(securityId)).Select(r => r.P_Name).ToArray() 
//                : roles.Where(r => r.HasReadPermission(securityId)).Select(r => r.P_Name).ToArray();

//            return names;
//        }


//        public static bool HasWritePermission(string securityId)
//        {
//            foreach (string roleName in GetUserRoles(true))
//            {
//                var role = XmlReports.Environment.GetRole(roleName);
//                if (role == null) continue;

//                if (role.HasWritePermission(securityId))
//                {
//                    return true;
//                }
//            }

//            return false;
//        }

//        public static bool HasReadPermission(string securityId)
//        {
//            foreach (string roleName in GetUserRoles(false))
//            {
//                var role = XmlReports.Environment.GetRole(roleName);
//                if (role == null) continue;

//                if (role.HasReadPermission(securityId))
//                {
//                    return true;
//                }
//            }

//            return false;
//        }
//    }


//}
