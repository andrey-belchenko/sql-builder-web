//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using Microsoft.Build.Evaluation;
//using sql.builder.TFS;

//namespace sql.builder.XmlHelpers
//{
//    internal static class VSProjectHelper
//    {
//        private static string _proj_root;
//        /// <summary>
//        /// Файл проекта sql.builder.templates.csproj
//        /// </summary>
//        private static string _proj_path;
//        private static Project _proj;
//        private static Project Proj {
//            get {
//                if (_proj == null) {
//                    _proj_path = Path.Combine(XmlReports.GetRootPath(), "sql.builder.templates", "sql.builder.templates.csproj");
//                    _proj = ProjectCollection.GlobalProjectCollection.GetLoadedProjects(_proj_path).FirstOrDefault() ?? new Project(_proj_path);
//                }
//                return _proj;
//            }
//        }
//        /// <summary>
//        /// Метаданные для добавления файлов в проект
//        /// </summary>
//        private static KeyValuePair<string, string>[] metadata = new KeyValuePair<string, string>[1] { new KeyValuePair<string, string>("CopyToOutputDirectory", "PreserveNewest") };
//        /// <summary>
//        /// По абсолютному пути возвращает относительный, например:
//        /// GetRelativeFilepath(@"C:\infoenergo\root\main\all\sql.builder.templates\sql.builder\printTemplate\excel\64627.xml") =&gt; @"sql.builder\printTemplate\excel\64627.xml"
//        /// </summary>
//        /// <param name="absolute_path">абсолютный путь</param>
//        /// <returns>относительный путь</returns>
//        private static string GetRelativeFilepath(string absolute_path)
//        {
//            if (_proj_root == null) {
//                _proj_root = Path.Combine(XmlReports.GetRootPath(), @"sql.builder.templates\");
//            }
//            if (absolute_path.StartsWith(_proj_root)) {
//                return absolute_path.Substring(_proj_root.Length);
//            } else {
//                return string.Empty;
//            }
//        }
//        /*private static string GetRelativeFilepath(string absolute_path)
//        {
//            // Может быть, можно совместить с ucQueryEditor.remPathFromFilename() из \main\all\sql.builder\Controls\QueryEditor\ucQueryEditor.cs?..
//            // internal static string remPathFromFilename(string filename)
//            string root = @"\sql.builder.templates\";
//            int pos = absolute_path.IndexOf(root);
//            if (pos >= 0) {
//                return absolute_path.Substring(pos + root.Length);
//            } else {
//                return string.Empty;
//            }
//        }*/
//        /// <summary>
//        /// Добавляет файл в проект как контент и устанавливает на него признак "копировать в выходной каталог" = "да, если новее"
//        /// </summary>
//        /// <param name="filepath">Полный путь к файлу</param>
//        internal static void AddContentToProject(string filepath)
//        {
//            if (!XmlReports.IsDeveloperMode()) {
//                return;
//            }
//            if (!File.Exists(filepath)) {
//                throw new ArgumentException("Попытка добавить отсутствующий файл в проект - не надо так делать \"" + filepath + "\"");
//            }
//            string relative_filepath = GetRelativeFilepath(filepath);
//            Proj.AddItem("Content", relative_filepath, metadata);
//            Save();
//        }
//        /// <summary>
//        /// Проверяет, добавлен ли в файл в проект. Если нет - добавляет.
//        /// Проверяет что признак "копировать в выходной каталог" на файле равен "да, если новее". Если нет - меняет признак.
//        /// </summary>
//        /// <param name="filepath">Полный путь к файлу</param>
//        internal static void RestoreContent(string filepath)
//        {
//            if (!XmlReports.IsDeveloperMode()) {
//                return;
//            }
//            string relative_filepath = GetRelativeFilepath(filepath);
//            ProjectItem item = Proj.GetItemsByEvaluatedInclude(relative_filepath).FirstOrDefault();
//            if (item == null) {
//                Proj.AddItem("Content", relative_filepath, metadata);
//                Save();
//            } else {
//                ProjectMetadata copy = item.GetMetadata("CopyToOutputDirectory");
//                if (copy == null) {
//                    item.SetMetadataValue("CopyToOutputDirectory", "PreserveNewest");
//                    Save();
//                } else if (copy.UnevaluatedValue != "PreserveNewest") {
//                    copy.UnevaluatedValue = "PreserveNewest";
//                    Save();
//                }
//            }
//        }
//        private static void Save()
//        {
//            if (_proj != null) {
//                TFSHelper.CheckOutFile(_proj_path);
//                Proj.Save();
//            }
//        }
//    }
//}