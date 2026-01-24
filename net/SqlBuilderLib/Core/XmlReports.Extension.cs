using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
////using System.Windows.Forms;
using System.Xml.Linq;
//using DevExpress.DashboardCommon.Native;
//using infoenergo.Context; // ContextDataModel
//using infoenergo.core.Extensions;
using infoenergo.sys;
using sql.builder.Clean;
using sql.builder.Core;
using sql.builder.DataApi;
//using sql.builder.TFS;
using sql.builder.XmlHelpers;
using sql.builder.Clean.Extensions;

//using DevExpress.XtraRichEdit.API.Word;

namespace sql.builder
{
    internal partial class XmlReports
    {
        /// <summary>
        /// Путь к папке source. Заполняется при вызове из ASP
        /// для веба Application.StartupPath использовать нельзя
        /// </summary>
        /// 
        private static string _sourceFolder = null;
        public static string SourceFolder
        {
            get { return _sourceFolder; }

            set {  
                _sourceFolder= value;
            }
        }
        private static XElement _inputParams;
        public static XElement InputParams {
            get {
                return _inputParams;
            }
            set {
                _inputParams = value;
            }
        }
        internal const string NativeProductName = "sql.builder";
        public static void SetInputParameter(string param_name, string value)
        {
            if (InputParams == null) InputParams = Cmn.GetFakeGlobalParams();
            var par = InputParams.Elements(EName.param).FirstOrDefault(e => e.AttrOrDefault(AName.name, string.Empty) == param_name);
            if (par == null)
            {
                par = new XElement(EName.param, new XAttribute(AName.name, param_name));
                InputParams.Add(par);
            }
            par.Elements().Remove();
            foreach (var val in value.Split(','))
            {
                par.Add(new XElement(EName.@const, val));
            }
        }
        private static SortedDictionary<string, object> globalParsValues = new SortedDictionary<string, object>();
        public static void SetGlobalParValue(string name, object value)
        {
            globalParsValues[name] = value;
        }
        public static bool IsGlobalParExists(string name)
        {
            return globalParsValues.ContainsKey(name);
        }
        public static void LoadXml(string scheme = null, string customer = null, XElement pars = null, bool dontCompile = false, bool force_reload = false, string source_folder = null)
        {
            if (scheme != null) schemeName = scheme;
            else scheme = schemeName;
            if (customer != null) customerId = customer;
            else customer = customerId;
            _environment = new VEnvironment(db.Connection);
            //_environment.Manager.LoadProjectIfNeed("common");

            Printing.templatesFolder = Path.Combine(GetCurrentContentFolder(), "printTemplate");

            ProcessGlobalParams(pars);
        }
        private static void ProcessGlobalParams(XElement pars)
        {
            XElement globpars = Environment.Manager.GetScheme().Elements(EName.globalparams).First();
            if (!IsDeveloperMode()) {
                Cmn.setParams(globpars, pars, true);
            }
            foreach (XElement param in globpars.Elements(EName.param)) {
                string name = param.Attribute(AName.name).Value;
                object val = DBNull.Value;
                if (param.HasElements) {
                    string vals = param.Elements().First().Value;
                    if (!string.IsNullOrEmpty(vals)) {
                        val = Cmn.ToDecimal(vals);
                    }
                }
                SetGlobalParValue(name, val);
            }
        }
        public static void DeleteElementInCompiledScheme(VSXElement newElement)
        {
            if (newElement == null) return;
            XName keyFieldName = newElement.Parent.Attribute(AName.key_name).Value;
            string key = newElement.Attribute(keyFieldName).Value;
            var xparents = Environment.Manager.GetScheme().Elements(newElement.Parent.Name).ToList();
            XElement old = xparents.Elements().FirstOrDefault(e => e.Attribute(keyFieldName).Value == key);
            if (old != null)
            {
                old.Remove();
            }
        }
        public static XElement UpdateElementInCompiledScheme(VSXElement newElement, List<XElement> extensions = null)
        {
            if (newElement == null) return null;
            XName keyFieldName = newElement.Parent.Attribute(AName.key_name).Value;
            string key = newElement.Attribute(keyFieldName).Value;
            var xparents = Environment.Manager.GetScheme().Elements(newElement.Parent.Name).ToList();
            XElement old = xparents.Elements().FirstOrDefault(e => e.Attribute(keyFieldName).Value == key);
            if (old != null)
            {
                old.Remove();
            }
            XElement newel = new XElement(newElement);
            xparents.First().Add(newel);
            if (extensions != null)
            {
                foreach (var xqry in extensions)
                {
                    XmlReports.ExtentdQuery(xqry, newel);
                }
            }
            if (newel.Name == EName.query)
            {
                Compiler.PreCompileQuery(newel, false);
                var newQry = newElement as VQuery;
                var exstQery = newQry.GetMainE();
                if (exstQery != newQry)
                {
                    var exts =
                        Environment.Manager.GetScheme()
                            .Elements(EName.queries)
                            .Elements(EName.query)
                            .Where(e => e.AttrOrDefault(AName.extend, string.Empty) == exstQery.P_Name)
                            .ToList();
                    var xexstQery = UpdateElementInCompiledScheme(exstQery, exts);
                    //foreach (var xqry in Environment.Scheme.Elements(TextConst.EName.Queries).Elements(TextConst.EName.Query).Where(e => Cmn.GetAttrValue(e, TextConst.AName.Extend) == exstQery.P_Name).ToArray())
                    //{
                    //    XmlReports.ExtentdQuery(xqry, xexstQery);
                    //}
                }
                foreach (var hqry in newQry.GetHeirs())
                {
                    UpdateElementInCompiledScheme(hqry);
                }
            }
            else
            {
                Compiler.PreCompileOther(newel, false);
            }
            //сделать обновление редактора
            /*if (XmlReports.Environment != newElement.GetEnvironment())
            {
                xparents = Environment.Manager.GetNativeScheme().Elements(newElement.Parent.Name).ToList();
                old = xparents.Elements().FirstOrDefault(e => e.Attribute(keyFieldName).Value == key);
                if (old != null)
                {
                    old.Remove();
                }
                newel = new XElement(newElement);
                xparents.First().Add(newel);
            }*/
            Environment.UpdateLastSchemeAssembleTime();
            return newel;
        }
        /* private static bool IsSchemeChanged(string sourceFolderPath, string assembledFilePath)
        {
            DateTime d1 = Cmn.GetDirectoryLastChange(sourceFolderPath);
            FileInfo fi = new FileInfo(assembledFilePath);
            return fi.LastWriteTime < d1;
        }*/
        private static bool IsOldSchemeChanged(string scheme)
        {
            string path = GetDefaultContentFolder();
            FileInfo fi = new FileInfo(Path.Combine(path, scheme + ".old.xml"));
            if (!fi.Exists) {
                return false;
            }
            DirectoryInfo di = new DirectoryInfo(Path.Combine(path, "oldsource"));
            if (!di.Exists) {
                return false;
            }
            DateTime source_date;
            using (IEnumerator<FileInfo> enumerator = di.EnumerateFiles("*.xml", SearchOption.AllDirectories).GetEnumerator()) {
                if (!enumerator.MoveNext()) {
                    return false;
                }
                source_date = enumerator.Current.LastWriteTimeUtc;
                while (enumerator.MoveNext()) {
                    Cmn.GetLastDate(ref source_date, enumerator.Current.LastWriteTimeUtc);
                }
            }
            DateTime assemble_date = fi.LastWriteTimeUtc;
            //TimeSpan delta = source_date - assemble_date;
            //bool is_changed = delta > Cmn.HalfOfSecond;
            bool is_changed = source_date > assemble_date;
            return is_changed;
        }
        private static bool IsProjectChanged(XElement xproject)
        {
            string project_path = Cmn.GetProjectPath(xproject);
            DirectoryInfo di = new DirectoryInfo(project_path);
            if (!di.Exists) {
                return false;
            }
            string name = xproject.Attribute(AName.name).Value;
            string file_compiled = Path.Combine(project_path, name + ".xml");
            string file_native = Path.Combine(project_path, name + "native.xml");
            DateTime assemble_date = DateTime.MinValue;
            DateTime source_date = DateTime.MinValue;
            foreach (FileInfo fi in di.EnumerateFiles("*.xml", SearchOption.AllDirectories)) {
                string file = fi.FullName;
                DateTime date = fi.LastWriteTimeUtc; // т.к. NTFS хранит время в формате UTC см. https://learn.microsoft.com/en-us/windows/win32/sysinfo/file-times
                if (string.Equals(file, file_compiled, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(file, file_native, StringComparison.OrdinalIgnoreCase)) {
                    Cmn.GetLastDate(ref assemble_date, date);
                } else {
                    Cmn.GetLastDate(ref source_date, date);
                }
            }
            //TimeSpan delta = source_date - assemble_date;
            //#if DEBUG
            //if (delta > TimeSpan.Zero) {
            //    Debug.WriteLine("Проект " + name + ": лаг " + delta.ToString());
            //}
            //#endif
            //bool is_changed = delta > Cmn.HalfOfSecond;
            bool is_changed = source_date > assemble_date;
            return is_changed;
        }
        /*private static bool IsProjectChanged(XElement xproject)
        {
            var projectPath = Cmn.GetProjectPath(xproject);
            var projectPathInfo = new DirectoryInfo(projectPath);
            var fileInfoCompiled = new FileInfo(Path.Combine(projectPath, xproject.Attribute("name").Value + ".xml"));
            var fileInfoNative = new FileInfo(Path.Combine(projectPath, xproject.Attribute("name").Value + "native.xml"));
            DateTime assembleDateChange = new[] { fileInfoCompiled.LastWriteTime, fileInfoCompiled.LastWriteTime }.Max();
            DateTime sourceDateChange = DateTime.MinValue;
            if (projectPathInfo.Exists)
            {
                var fis=projectPathInfo.EnumerateFiles("*.xml", SearchOption.AllDirectories)
                    .Where(fi => fi.FullName != fileInfoCompiled.FullName && fi.FullName != fileInfoNative.FullName);
                if (fis.Any())
                {
                    sourceDateChange = fis.Max(fi => fi.LastWriteTime);
                }
            }
            return (assembleDateChange < sourceDateChange);
        }*/
        public static DateTime SchemeChangeTime()
        {
            return Environment.GetLastSchemeAssembleTime();
        }
        //public static string GetNativeSchemeRuntimeFile(string schName)
        //{
        //    return Path.Combine(GetRuntimePath(), schName + ".native.xml");
        //}
        public static string GetRuntimePath()
        {
            return Path.Combine((SourceFolder ?? CleanUtils.GetRootPath()));
        }
        public static string GetProductName()
        {
            return "sql.builder";
            //return Application.ProductName;
        }
        static string _projectPath;
        public static string GetVSProjectPath()
        {
            if (_projectPath == null)
            {
                string path = (SourceFolder ?? CleanUtils.GetRootPath());
                var di = new DirectoryInfo(path);
                while (di.Name != "bin" && di.Parent != null)
                {
                    di = di.Parent;
                }
                _projectPath = (di.Parent != null) ? di.Parent.FullName : "";
            }
            return _projectPath;
        }
        static string _rootPath;
        /// <summary>
        /// Папка, в которой лежат все проекты (all)
        /// </summary>
        /// <returns></returns>
        internal static string GetRootPath()
        {
            if (_rootPath == null)
            {
                var vsPath = GetVSProjectPath();
                _rootPath = (vsPath != "") ? Path.GetFullPath(Path.Combine(vsPath, "..")) : "";
            }
            return _rootPath;
        }
        internal static string GetDefaultSourceFolder()
        {
            return Path.Combine(GetDefaultContentFolder(), SourceFolderName);
        }
        internal static string GetRuntimeSourceFolder()
        {
            return Path.Combine(GetRuntimeContentFolder(), SourceFolderName);
        }
        internal static string GetCurrentSourceFolder()
        {
            return Path.Combine(GetCurrentContentFolder(), SourceFolderName);
        }
        internal static string GetDefaultContentFolder()
        {
            return Path.Combine(GetRootPath(), "sql.builder.templates", "sql.builder");
        }
        internal static string GetRuntimeContentFolder()
        {
            return Path.Combine(GetRuntimePath(), "sql.builder");
        }
        internal static string GetCurrentContentFolder()
        {
            return UseProjectSourceFolder ? GetDefaultContentFolder() : GetRuntimeContentFolder();
        }
        // тут должны быть пакеты, которые требуется подгружать сразу при загрузке приложения
        public static string[] GetDemandProjects()
        {
            return new[] { "common" };
            //return new[] { "common", schemeName };
        }
        internal static IList<XElement> GetSortedProjects()
        {
            // Сортируем проекты так, чтобы референсы были первыми 
            // Это важно при прекомпиляции
            // Не уверен что работает как ожидалось
            var xprojects = XmlSpecialFiles.GetActualXml().Elements(EName.projects).Elements(EName.project);
            var dict = new Dictionary<string, Project>();
            foreach (var xproject in xprojects)
            {
                var proj = new Project(xproject);
                dict.Add(xproject.Attribute(AName.name).Value, proj);
            }
            foreach (var key in dict.Keys)
            {
                SortProject(key, dict);
            }
            var paths = dict
                .OrderBy(v => v.Value.Priority)
                .Select(v => v.Value.Xml)
                .ToList();
            return paths;
        }
        private static void SortProject(string name, Dictionary<string, Project> projects)
        {
            var cur = projects[name];
            if (cur.Processed) return;
            cur.Processed = true;
            var projects_referenced = cur.Xml
                .Elements(EName.references)
                .Elements(EName.reference)
                .Select(r => r.Attribute(AName.project).Value)
                .ToArray();
            foreach (var ref_name in projects_referenced)
            {
                SortProject(ref_name, projects);
            }
            if (projects_referenced.Length != 0) {
                cur.Priority = projects_referenced.Max(n => projects[n].Priority) + 1;
            }
        }
        private class Project
        {
            private XElement xml;
            private bool processed;
            private int priority;
            internal XElement Xml {
                get {
                    return this.xml;
                }
            }
            internal bool Processed {
                get {
                    return this.processed;
                }
                set {
                    this.processed = value;
                }
            }
            internal int Priority {
                get {
                    return this.priority;
                }
                set {
                    this.priority = value;
                }
            }
            internal Project(XElement xml)
            {
                this.xml = xml;
                this.processed = false;
                this.priority = 0;
            }
        }
        public static string[] GetProjectReferencesAndSelfNames(string project_name)
        {
            var projects_referenced = XmlSpecialFiles.GetActualXml()
                .Elements(EName.projects)
                .Elements(EName.project)
                .First(p => p.Attribute(AName.name).Value == project_name)
                .Elements(EName.references)
                .Elements(EName.reference)
                .Select(r => r.Attribute(AName.project).Value);
            var projects = System.Linq.Enumerable.Append(projects_referenced, project_name).ToArray();
            return projects;
        }
        //public static string[] GetProjectFolders()
        //{
        //    return new[] { GetRootPath(), GetRuntimePath() };
        //}
        public static bool IsAnySchemeChanged()
        {
            return GetSortedProjects().Any(IsProjectChanged) || IsOldSchemeChanged(schemeName);
        }
        public static void AssembleAllXml(bool force_reload)
        {
            bool changed_old;
            IList<XElement> changed_projects = GetSortedProjects();
            if (force_reload) {
                changed_old = true;
            } else {
                changed_old = IsOldSchemeChanged(schemeName);
                for (int index = changed_projects.Count - 1; index >= 0; index--) {
                    if (!IsProjectChanged(changed_projects[index])) {
                        changed_projects.RemoveAt(index);
                    }
                }
            }
            var filesToSave = new Dictionary<string, XElement>();
            if (changed_old) {
                #region Пути
                string[] folders_old = new string[] { SourceFolderName + @"\common", @"oldsource\scheme\[scheme]", @"oldsource\reports\[scheme]" };
                #endregion
                // old
                XElement xNativeOld = AssembleXml(schemeName, folders_old);
                //Compiled
                // создаю фэйковую environment в которой в NativeScheme сидит сам проект и те, на которые он ссылается
                _environment = new VEnvironment(db.Connection);
                VSXElement native_scheme = VSXElement.Get(new XElement(xNativeOld));
                VSXElement scheme = VSXElement.Get(new XElement(xNativeOld));
                _environment.Manager.SetProjectScheme(schemeName, scheme, native_scheme);
                // компилируем только сам проект
                Compiler.PreCompile(true, scheme);
                _environment = null;
                filesToSave.Add(Path.Combine(GetDefaultContentFolder(), schemeName + ".old.xml"), scheme);
            }
            if (changed_projects.Count != 0) {
                // пересобираем изменившиеся проекты
                XmlProject[] projects = AssembleXmlProjects(changed_projects).ToArray();
                // создаю фэйковую environment в которой в NativeScheme есть все пересобранное
                _environment = new VEnvironment(db.Connection);
                foreach (XmlProject project in projects) {
                    // компилируем только сам проект
                    VSXElement native_scheme = VSXElement.Get(new XElement(project.Xml));
                    VSXElement scheme = VSXElement.Get(new XElement(project.Xml));
                    _environment.Manager.SetProjectScheme(project.Name, scheme, native_scheme);
                    // компиляция
                    string[] project_names = GetProjectReferencesAndSelfNames(project.Name);
                    // дополнительно подгружаем те которые нужны, но не пересобирались и еще не загружались
                    // либо делаем видимыми, если проекты были скрыты ранее
                    _environment.Manager.LoadProjectsIfNeed(project_names);
                    //foreach (string pn in project_names) {
                    //    _environment.Manager.LoadProjectIfNeed(pn);
                    //}
                    // прячем проекты, которые уже загружены, но не должны быть видимы во время прекомпиляции
                    //string[] unused_project_names = _environment.Manager.GetVisibleProjects().Select(p => p.Name).Except(project_names).ToArray();
                    //foreach (string pn in unused_project_names) {
                    //    _environment.Manager.HideProject(pn);
                    //}
                    foreach (var p in _environment.Manager.GetVisibleProjects()) {
                        if (!project_names.Contains(p.Name)) {
                            _environment.Manager.HideProject(p.Name);
                        }
                    }
                    Compiler.PreCompile(false, scheme);
                    var proj = Environment.Manager.GetProject(project.Name);
                    filesToSave.Add(proj.FileNativePath, proj.SchemeNative);
                    filesToSave.Add(proj.FileCompiledPath, proj.Scheme);
                }
                _environment = null;
            }
            if (filesToSave.Count != 0) {
                // сначала делаем CheckOut
                //using (var tfs = new TFSServer()) {
                //    tfs.CheckOutFile(filesToSave.Keys.ToArray());
                //}
                // параллельно сохраняем все файлы
                filesToSave.AsParallel().ForAll(fs => fs.Value.Save(fs.Key));
            }
        }
        private static XElement AssembleXml(string scheme, IEnumerable<string> folders)
        {
            // имя файла, который нужно загрузить первым (должен лежать в первой папке folders)
            string base_file_name = "main.xml";
            string[] scheme_files =
            {
                "common.xml", 
                "common.native.xml",
                scheme + ".old.xml"
            };
            // получаем коллекцию всех путей ко всем файлам
            var all_file_paths = Enumerable.Empty<string>();
            foreach (var folder in folders)
            {
                var path = Path.Combine(GetDefaultContentFolder(), folder.Replace("[scheme]", scheme));
                if (Directory.Exists(path))
                {
                    all_file_paths = all_file_paths.Concat(Directory.GetFiles(path, "*.xml", SearchOption.AllDirectories));
                }
            }
            // достаем путь основного файла
            string base_file_path = all_file_paths.First(file => file.EndsWith(base_file_name));
            // исключаем основной файл из общего списка
            all_file_paths = all_file_paths.Where(f => f != base_file_path);
            // подгружаем основной файл
            XElement xRoot = XElement.Load(base_file_path);
            // перебираем пути файлов
            foreach (string file_path in all_file_paths) {
                string fn = Path.GetFileName(file_path);
                if (scheme_files.Contains(fn)) continue;
                LoadFile(file_path, xRoot);
            }
            xRoot.SetAttributeValue(AName.timestamp, DateTime.Now);
            return xRoot;
        }
        private static IEnumerable<XmlProject> AssembleXmlProjects(IEnumerable<XElement> xprojects)
        {
            // достаем базовый файл
            var base_file_path = Path.Combine(GetDefaultSourceFolder(), "common", "main.xml");
            var projectBase = XElement.Load(base_file_path);
            foreach (var e in projectBase.Elements()) e.RemoveNodes();
            var projects = new List<XmlProject>();
            // получаем коллекцию всех путей ко всем файлам
            var all_file_paths = Enumerable.Empty<string>();
            foreach (XElement xproject in xprojects)
            {
                string project_name = xproject.Attribute("name").Value;
                string project_path = Cmn.GetProjectPath(xproject);
                string[] scheme_files =
                {
                    project_name + ".xml", 
                    project_name + ".native.xml"
                };
                if (!Directory.Exists(project_path)) continue;

                all_file_paths = Directory.EnumerateFiles(project_path, "*.xml", SearchOption.AllDirectories);
                XElement project = new XElement(projectBase);
                // перебираем пути файлов
                foreach (var file_path in all_file_paths)
                {
                    var fn = Path.GetFileName(file_path);
                    if (scheme_files.Contains(fn)) continue;
                    LoadFile(file_path, project);
                }
                project.SetAttributeValue(AName.timestamp, DateTime.Now);
                projects.Add(new XmlProject(project_name, project));
            }
            return projects;
        }
        public static void LoadFile(string file_path)
        {
            LoadFile(file_path, Environment.Manager.GetScheme().First());
        }
        private static void LoadFile(string file_path, XElement xMainDocRoot)
        {
            // загружаем xml из файла
            var xml = Cmn.OpenXmlClearNS(file_path);
            // чистим NameSpace для всех элементов в файле
            // нужно для корректной работы Elements(), Descendants() и прочих
            //if (xml.Root.Name.NamespaceName != "")
            //{
            //    xml.Root.Attribute("xmlns").Remove();
            //    xml.Root.DescendantsAndSelf().ForEach(el => el.Name = el.Name.LocalName);
            //}
            // по пути определяем нужно ли помечать узлы аттрибутом class
            var in_scheme = (file_path.Contains("original") || file_path.Contains("extension")) && file_path.Contains("scheme");
            // перебираем все узлы первого уровня в результирующем файле - группы
            foreach (var xmaingroup in xMainDocRoot.Elements())
            {
                // находим такие же группы в загруженном файле
                foreach (var xgroup in xml.Root.Elements(xmaingroup.Name))
                {
                    // перебираем дочерние узлы в группе - элементы
                    var xchildren = xgroup.Elements()
                        .Where(el => el.Name.LocalName != "region")
                        .Concat(xgroup.Elements("region").Elements());
                    foreach (var xchild in xchildren)
                    {
                        if (in_scheme) xchild.SetAttributeValue("class", "1");
                        xchild.SetAttributeValue(AName.file, ucQueryEditor.remPathFromFilename(file_path));
                        //// элемент query может быть размазан по нескольким файлам
                        //if (xchild.Name.LocalName == "query")
                        //{
                        //    var original_query = FindOriginal(xchild, xmaingroup);
                        //    if (original_query != null)
                        //    {
                        //        xchild.SetAttributeValue("extend", original_query.Attribute("name").Value);
                        //    }
                        //}
                        // копируем элемент в ту же группу в результирующий файл
                        xmaingroup.Add(xchild);
                    }
                }
            }
        }
        public static XElement FindOriginal(XElement xchild, XElement xmaingroup)
        {
            string name;
            XAttribute attr = xchild.Attribute(AName.extend);
            if (attr != null) {
                name = attr.Value;
            } else {
                name = xchild.Attribute(AName.name).Value;
            }
            return xmaingroup.Elements(EName.query).FirstOrDefault(qry => qry.Attribute(AName.name).Value == name);
        }
        public static void FindAndExtentdQuery(XElement xchild, XElement xmaingroup)
        {
            // ищем query с таким именем в результирующем файле
            var original_query = FindOriginal(xchild, xmaingroup);
            if (original_query != null)
            {
                ExtentdQuery(xchild, original_query);
            }
        }
        public static void ExtentdQuery(XElement xchild, XElement original_query)
        {
            // обновляем и объединяем данные узлов
            original_query.CombineXAttributes(xchild, "pushpred");
            original_query.CombineXAttributes(xchild, "hint");
            original_query.CombineXElements(xchild, "params");
            original_query.CombineXElements(xchild, TextConst.EName.Expressions);
            original_query.CombineXElements(xchild, "select", prev_element_name: "params", doOverride: true);
            original_query.CombineXElements(xchild, "from", prev_element_name: "select");
            original_query.CombineXElements(xchild, "from/table", "links");
            original_query.CombineXElements(xchild, "where", prev_element_name: "from");
        }
        public static void SetColsetsVisible(XElement xScheme, XElement xScheme_preset, IEnumerable<string> visible_colsets)
        {
            foreach (var xviewcolumns_old in xScheme.Descendants(EName.viewcolumns))
            {
                var xviewcolumns_new = new XElement(xviewcolumns_old);
                var xviewcolumns_preset = xScheme_preset
                    .Descendants(EName.viewcolumns)
                    .First(vc => vc.Parent.Attribute(AName.name).Value == xviewcolumns_old.Parent.Attribute(AName.name).Value);
                var visible_band_titles = xviewcolumns_preset.Descendants(EName.band).Attributes(AName.title).Select(APredicate.AttributeValue);
                var visible_column_names = xviewcolumns_preset.Descendants(EName.column).Attributes(AName.name).Select(APredicate.AttributeValue);
                xviewcolumns_new.Descendants()
                    .Where(el => (el.Attribute(AName.colset) != null && !visible_colsets.Contains(el.Attribute(AName.colset).Value))
                              || (el.Attribute(AName.colset) == null && el.Name == EName.column && !visible_column_names.Contains(el.AttrOrDefault(AName.name, string.Empty)))
                              || (el.Attribute(AName.colset) == null && el.Name == EName.band && !visible_band_titles.Contains(el.AttrOrDefault(AName.title, string.Empty))))
                    .Remove();
                CorrectScheme(xviewcolumns_new, xviewcolumns_old);
                xviewcolumns_preset.ReplaceWith(xviewcolumns_new);
            }
        }
        public static void CorrectScheme(XElement xViewColumns_new, XElement xViewColumns_old)
        {
            var columns_old = xViewColumns_old.Descendants().Attributes(AName.name).Select(APredicate.AttributeValue);
            var aggs_old = xViewColumns_old.Descendants().Attributes(AName.agg).Select(APredicate.AttributeValue);
            var columns_new = xViewColumns_new.Descendants().Attributes(AName.name).Select(APredicate.AttributeValue).ToList();
            var columns_used_new = columns_new.Where(col_name => aggs_old.Contains(col_name)).ToList();
            var columns_missed = columns_used_new.Where(col_name => !columns_old.Contains(col_name));
            var xColumns_missed = xViewColumns_old.Descendants().Where(col_name => columns_missed.Contains(col_name.AttrOrDefault(AName.name, string.Empty))).ToList();
            // Бельченко, среди невидимых колонок есть обязательные факты из куба (например в журнале un_mat_pp), иначе при исключении всех фактов из списка колонок - ошибка
            xColumns_missed.AddRange(xViewColumns_old.Descendants(EName.column).Where(
                col => col.AttrOrDefault(AName.visible, string.Empty) == TextConst.AVBool.False &&
                col.AttrOrDefault(AName.colset, string.Empty) == string.Empty  // иначе colset не работает для колонок без заголовков
                ).ToList()
                );
            foreach (var xcol_missed in xColumns_missed.Distinct())
            {
                if (!columns_new.Contains(xcol_missed.AttrOrDefault(AName.name, string.Empty)))
                {
                    var xcol_missed_new = new XElement(xcol_missed);
                    xcol_missed_new.Elements().Remove();
                    xcol_missed_new.SetAttributeValue(AName.visible, TextConst.AVBool.False);
                    xViewColumns_new.Add(xcol_missed_new);
                }
            }
        }
        #region Обработка Xml
        internal static string GetXElementTitle(XElement xElement)
        {
            string title = xElement.AttrOrDefault(AName.title, null);
            if (!string.IsNullOrEmpty(title)) {
                return title;
            } else {
                return GetXElementName(xElement);
            }
            //return GetXAttributeValue(xElement, "title") != ""
            //    ? GetXAttributeValue(xElement, "title")
            //    : GetXElementName(xElement);
        }
        internal static string GetXElementName(XElement xElement)
        {
            string alias = xElement.AttrOrDefault(AName.@as, null);
            if (!string.IsNullOrEmpty(alias)) {
                return alias;
            }
            string name = xElement.AttrOrDefault(AName.name, null);
            if (!string.IsNullOrEmpty(name)) {
                return name;
            } else {
                return xElement.AttrOrDefault(AName.column, null);
            }
            //return GetXAttributeValue(xElement, "as") != ""
            //        ? GetXAttributeValue(xElement, "as")
            //        : GetXAttributeValue(xElement, "name") != ""
            //            ? GetXAttributeValue(xElement, "name")
            //            : GetXAttributeValue(xElement, "column");
        }
        //[Obsolete("Используйте XElementExtensions.SetAttrValue() или XElement.SetAttributeValue()")]
        internal static void SetXElementAttribute(XElement xElement, string atr_name, string atr_value)
        {
            xElement.SetAttrValue(atr_name, atr_value);
            //xElement.SetAttributeValue(atr_name, atr_value);
            //XAttribute attr = xElement.Attribute(atr_name);
            //if (attr != null) {
            //    attr.Value = atr_value;
            //} else {
            //    xElement.Add(new XAttribute(atr_name, atr_value));
            //}
        }
        //[Obsolete("Используйте XElementExtensions.AttrOrDefault()")]
        internal static string GetXAttributeValue(XElement xElement, string atr_name)
        {
            return xElement.AttrOrDefault(atr_name, string.Empty);
            //return xElement.Attribute(atr_name) != null
            //    ? xElement.Attribute(atr_name).Value
            //    : "";
        }
        // Поиск дочернего узла по имени и набору значений атрибутов
        // Если узел не найден - создается новый с таким именем и атрибутами
        /*internal static XElement GetOrCreateXElement(this XElement xParent, string child_name, Tuple<string, string>[] child_attributes = null)
        {
            XElement xChild = child_attributes == null
                            ? xParent.Element(child_name)
                            : xParent.Elements(child_name)
                                .FirstOrDefault(el => child_attributes.All(atr => atr.Item2 == el.AttrOrDefault(atr.Item1, string.Empty)));
            if (xChild == null)
            {
                xChild = child_attributes == null
                            ? new XElement(child_name)
                            : new XElement(child_name, child_attributes.Select(atr => new XAttribute(atr.Item1, atr.Item2)));
                xParent.Add(xChild);
            }
            return xChild;
        }*/
        internal static XElement GetOrCreateXElement(this XElement parent, XName child_name)
        {
            XElement child = parent.Element(child_name);
            if (child == null) {
                child = new XElement(child_name);
                parent.Add(child);
            }
            return child;
        }
        /// <summary>
        /// Обновление конкретного элемента узла данными из элемента другого узла схожей структуры
        /// </summary>
        /// <param name="dest">Обновляемый узел</param>
        /// <param name="src">Узел с новыми данными</param>
        /// <param name="dest_element_name">Имя элемента, который обновляем в узле</param>
        /// <param name="src_element_name">Имя элемента, из которого обновляем (по умолчанию равен dest_element_name)</param>
        /// <param name="prev_element_name">Имя элемента, за которым должен следовать обновленный элемент (для случая, если элемент в обновляемом узле отсутствует и его нужно будет добавить)</param>
        /// <param name="doOverride"></param>
        private static void CombineXElements(this XElement dest, XElement src, string dest_element_name, string src_element_name = null, string prev_element_name = null, bool doOverride = false)
        {
            // по умолчанию имя узла источника = имени узла приемника
            src_element_name = src_element_name ?? dest_element_name;
            // достаем элемент, который будем обновлять
            var dest_element = dest;
            foreach (var child_name in dest_element_name.Split('/'))
            {
                dest_element = dest_element.Element(child_name);
                if (dest_element == null) break;
            }
            // достает элемент, из которого обновляем
            var src_element = src;
            foreach (var child_name in src_element_name.Split('/'))
            {
                src_element = src_element.Element(child_name);
                if (src_element == null) break;
            }
            // если узел есть в источнике, но нет в приемнике
            if (dest_element == null && src_element != null)
            {
                // если имя узла, после которого добавлять элемент не определен
                // просто добавляем в начало
                if (prev_element_name == null) {
                    dest.AddFirst(src_element);
                } else {
                    // достает элемент, за которым должен следовать обновленный элемент
                    var prev_element = dest;
                    foreach (var child_name in prev_element_name.Split('/')) {
                        prev_element = prev_element.Element(child_name);
                        if (prev_element == null) break;
                    }
                    // если узел, после которого добавлять элемент, не найден
                    // просто копируем элемент в начало
                    if (prev_element == null) dest.AddFirst(src_element);
                    // если найден - вставляем после него
                    else prev_element.AddAfterSelf(src_element);
                }
            }
            else if (dest_element != null && src_element != null) // если узел есть в источнике и есть в приемнике
            {
                // просто копируем все дочерние элементы из источника в приемник
                dest_element.Add(src_element.Elements());
            }
        }
        private static void CombineXAttributes(this XElement dest, XElement src, string attr_name)
        {
            XAttribute src_attr = src.Attribute(attr_name);
            XAttribute dest_attr = dest.Attribute(attr_name);
            if (dest_attr == null && src_attr != null) {
                dest.Add(src_attr);
            } else if (dest_attr != null && src_attr != null) {
                dest_attr.Value = src_attr.Value;
            }
        }
        #endregion
        internal class XmlProject
        {
            private string name;
            private XElement xml;
            public string Name {
                get {
                    return this.name;
                }
            }
            public XElement Xml {
                get {
                    return this.xml;
                }
            }
            public XmlProject(string name, XElement xml)
            {
                this.name = name;
                this.xml  = xml;
            }
        }
    }
}
