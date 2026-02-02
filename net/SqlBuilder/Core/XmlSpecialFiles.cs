using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace sql.builder.Core
{
    public static class XmlSpecialFiles
    {
        private static string[] Files  
        {
            get
            {

                var ff = new List<string>();
                string rootPath = XmlReports.GetCurrentSourceFolder();
                var ptmp=@"common\special\";
                var di = new DirectoryInfo(Path.Combine(rootPath, ptmp));
                foreach (var f in di.GetFiles("*.xml", SearchOption.AllDirectories))
                {
                    ff.Add(ptmp+ f.Name);
                    
                }
              
                return ff.ToArray();
            }
          
        }

      


        private static Dictionary<string, XmlSpecialFile> _files;

        private static void ReloadFile(FileInfo fi)
        {
            var sf = _files[fi.FullName];
            sf.Xml = Cmn.OpenXmlClearNS(fi.FullName).Root;
            sf.LastChangedTime = fi.LastWriteTime;
        }
        private static void ReloadAll()
        {
            _files = new Dictionary<string, XmlSpecialFile>();
            foreach (var file2 in Files)
            {
                string rootPath = XmlReports.GetCurrentSourceFolder();
                string path = Path.Combine(rootPath, file2);

                var fi = new FileInfo(path);
                _files.Add(path, new XmlSpecialFile()
                {
                    Path = path,
                    Xml = Cmn.OpenXmlClearNS(path).Root,
                    LastChangedTime = fi.LastWriteTime
                });
            }
        }
        private static void ReloadIfNeed()
        {
            if(_files == null)
            {
                ReloadAll();
            }
            else
            {
                foreach (var pair in _files)
                {
                    var fi = new FileInfo(pair.Key);
                    if (fi.LastWriteTime > pair.Value.LastChangedTime)
                    {
                        ReloadFile(fi);
                    }
                }   
            }
        }

        public static IEnumerable<XElement> GetActualXml()
        {
            ReloadIfNeed();

            return _files.Values.Select(v => v.Xml);
        }

        private class XmlSpecialFile
        {
            public string Path { get; set; }
            public XElement Xml { get; set; }
            public DateTime LastChangedTime { get; set; }
        }
    }
}