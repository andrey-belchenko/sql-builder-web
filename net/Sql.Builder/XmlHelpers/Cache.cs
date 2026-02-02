using System;
using System.IO;
using System.Text;
//using System.Windows.Forms;
// Cross-platform: System.Windows.Input (WPF) is Windows-only, commented out
//using System.Windows.Input;
using System.Xml.Linq;
using sql.builder.DataApi;
//using sql.builder.TFS;

namespace sql.builder.XmlHelpers
{
    /// <summary>
    /// кэш для форм
    /// </summary>
    internal static class Cache
    {
        #region Forms
        private static string _forms_folder;
        public static string FormsFolder
        {
            get
            {
                if (_forms_folder == null)
                {
                    _forms_folder = Path.Combine(XmlReports.GetCurrentContentFolder(), XmlReports.FormsCacheFolderName);
                }

                return _forms_folder;
            }
        }

        private static bool _useFormsCache = false;

        // всегда false в режиме отладки с зажатым ctrl+shift
        public static bool UseFormsCache
        {
            get
            {
                if (!XmlReports.IsDeveloperMode())
                {
                    return true;
                }
                if (!XmlReports.IsNative)
                {
                    return true;
                }
                if (!_useFormsCache) return false;
                //Keyboard.Modifiers.HasFlag(ModifierKeys.Control) дает ошибку при вызове не из главного потока
                //if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control) && Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
                //{                   
                //    return false;    
                //}

                return _useFormsCache;
            }
            set { _useFormsCache = value; }
        }

        internal static bool IsActualForm(string form_name, DateTime item_write_date)
        {
            if (!XmlReports.IsDeveloperMode()) return true;
            if (!_useFormsCache) return false;

            var xitem = XmlReports.Environment.GetFormOrQuery(form_name);

            DateTime? item_changed_date = xitem.GetTimeStamp();
            if (item_changed_date == null)
            {
                var filepath = XmlReports.GetCurrentSourceFolder() + "\\" + xitem.Attribute(TextConst.AName.File).Value;
                item_changed_date = File.GetLastWriteTime(filepath);
            }

            return (item_changed_date <= item_write_date);
        }
        internal static void SaveNotActualForm(XElement xform, string form_name)
        {
            if (!XmlReports.IsDeveloperMode() || !XmlReports.IsNative) return;
            string filepath = Path.Combine(FormsFolder, form_name + ".xml");
            FileInfo file = new FileInfo(filepath);
            if (!file.Exists) {
                Cmn.SaveText(xform.ToString(), filepath, Encoding.UTF8);
                //using (var tfs = new TFSServer()) {
                //    tfs.AddFile(filepath);
                //}                
                //VSProjectHelper.AddContentToProject(filepath);
            } else if (!IsActualForm(form_name, file.LastWriteTime)) {
                Cmn.SaveXmlWithCheckOut(xform, filepath);
            }
        }
        internal static XElement GetActualForm(string form_name)
        {
            var filepath = Path.Combine(FormsFolder, form_name + ".xml");
            var file = new FileInfo(filepath);

            // для неактуальных возвращаем null
            if (!file.Exists || !IsActualForm(form_name, file.LastWriteTime)) return null;

            return XElement.Load(filepath);
        }
        #endregion
        #region Qubes
        private static string _qubes_folder;
        public static string QubesFolder
        {
            get
            {
                if (_qubes_folder == null)
                {
                    _qubes_folder = Path.Combine(XmlReports.GetCurrentContentFolder(), XmlReports.QubesCacheFolderName);
                }

                return _qubes_folder;
            }
        }

        internal static DateTime GetLastQubeCacheTime(string query_name)
        {
            string qube_path = Path.Combine(QubesFolder, query_name + ".xml");
            if (!File.Exists(qube_path)) {
                return DateTime.MinValue;
            }
            XElement xqube = XElement.Load(qube_path);
            return DateTime.Parse(xqube.Attribute(AName.timestamp).Value);
        }
        internal static void SaveQubeInfoToCache(XElement xqube, string query_name)
        {
            if (!XmlReports.IsDeveloperMode() || !XmlReports.IsNative) return;
            xqube.SetAttributeValue(AName.timestamp, DateTime.Now);
            string qube_path = Path.Combine(QubesFolder, query_name + ".xml");
            if (!File.Exists(qube_path)) {
                Cmn.SaveText(xqube.ToString(), qube_path, Encoding.UTF8);
                //using (var tfs = new TFSServer()) {
                //    tfs.AddFile(qube_path);
                //}
                //VSProjectHelper.AddContentToProject(qube_path);
            } else {
                Cmn.SaveXmlWithCheckOut(xqube, qube_path);
            }
        }
        #endregion
        #region Queries
        private static string _queries_folder;
        internal static string QueriesFolder {
            get {
                if (_queries_folder == null) {
                    _queries_folder = Path.Combine(XmlReports.GetCurrentContentFolder(), XmlReports.QueriesCacheFolderName);
                }
                return _queries_folder;
            }
        }
        internal static void SaveQueryInfoToCache(XElement xquery, string query_name)
        {
            if (!XmlReports.IsDeveloperMode() || !XmlReports.IsNative) return;
            xquery.SetAttrValue(AName.timestamp, DateTime.Now);
            string query_path = Path.Combine(QueriesFolder, query_name + ".xml");
            if (!File.Exists(query_path)) {
                Cmn.SaveText(xquery.ToString(), query_path, Encoding.UTF8);
                //using (var tfs = new TFSServer()) {
                //    tfs.AddFile(query_path);
                //}
                //VSProjectHelper.AddContentToProject(query_path);
            } else {
                Cmn.SaveXmlWithCheckOut(xquery, query_path);
            }
        }
        internal static XElement GetQueryInfoFromCache(string query_name, DateTime changeTime, bool allowNoCacheInRelease)
        {
            string query_path = Path.Combine(QueriesFolder, query_name + ".xml");
            XElement xquery = null;
            if (!File.Exists(query_path)) {
                if (!XmlReports.IsDeveloperMode()) {
                    if (allowNoCacheInRelease) {
                        return null;
                    } else {
                        // будет ошибка - специально
                        xquery = XElement.Load(query_path);
                    }
                } else {
                    return null;
                }
            } else {
                xquery = XElement.Load(query_path);
            }
            if (!XmlReports.IsDeveloperMode()) {
                return xquery;
            }
            if (changeTime == DateTime.MaxValue) {
                changeTime = XmlReports.Environment.GetLastSchemeAssembleTime();
            }
            DateTime query_time = xquery.AttrOrDefault(AName.timestamp, DateTime.MinValue);
            if (changeTime > query_time) {
                return null;
            } else {
                return xquery;
            }
        }
        #endregion
    }
}