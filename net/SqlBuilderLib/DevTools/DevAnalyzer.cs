using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using sql.builder.Clean.Extensions;
using sql.builder.DataApi;

namespace SqlBuilderLib.DevTools
{
   
    internal static class DevAnalyzer
    {
        public static bool Enabled = false;

        public static HashSet<string> tablenames = new HashSet<string>();

        public static void AnalyzeReport(XElement xelement, string name = null)
        {
            var xtables = xelement.Descendants(TextConst.EName.Table);
            foreach (var xtable in xtables) { 
                var xtext = xelement.Element(TextConst.EName.Text);
                if (xtext == null)
                {
                    tablenames.Add(xtable.GetAttributeValue(TextConst.AName.Name));
                }
            }
            LogXElement(xelement, name);
        }
        public static void LogXElement(XElement element, string name= null)
        {
            if (!Enabled) return;
            if (element == null) return;

            // If name is not provided, try to find it from the element's name attribute
            if (string.IsNullOrEmpty(name))
            {
                var nameAttr = element.Attribute("name");
                if (nameAttr == null)
                {
                    // Search in descendants for a name attribute
                    var elementWithName = element.DescendantsAndSelf()
                        .FirstOrDefault(e => e.Attribute("name") != null);
                    nameAttr = elementWithName?.Attribute("name");
                }
                
                if (nameAttr != null)
                {
                    name = nameAttr.Value;
                }
                else
                {
                    // Fallback to element name if no name attribute found
                    name = element.Name.LocalName;
                }
            }

            // Get project root directory (where SqlBuilder.slnx is located)
            string projectRoot = GetProjectRoot();
            if (string.IsNullOrEmpty(projectRoot)) return;

            // Ensure Temp folder exists
            string tempFolder = Path.Combine(projectRoot, "Temp");
            Directory.CreateDirectory(tempFolder);

            // Generate filename with index if file already exists
            string baseFileName = $"{name}-par-val.xml";
            string filePath = Path.Combine(tempFolder, baseFileName);
            
            // If file exists, add index to filename
            if (File.Exists(filePath))
            {
                int index = 1;
                string fileNameWithoutExt = Path.GetFileNameWithoutExtension(baseFileName);
                string extension = Path.GetExtension(baseFileName);
                
                do
                {
                    string indexedFileName = $"{fileNameWithoutExt}-{index}{extension}";
                    filePath = Path.Combine(tempFolder, indexedFileName);
                    index++;
                } while (File.Exists(filePath));
            }

            element.Save(filePath);
        }

        public static void ClearTempFolder()
        {
            // Get project root directory (where SqlBuilder.slnx is located)
            string projectRoot = GetProjectRoot();
            if (string.IsNullOrEmpty(projectRoot)) return;

            // Get Temp folder path
            string tempFolder = Path.Combine(projectRoot, "Temp");
            
            // Check if Temp folder exists
            if (!Directory.Exists(tempFolder)) return;

            try
            {
                // Delete all files in the Temp folder
                string[] files = Directory.GetFiles(tempFolder);
                foreach (string file in files)
                {
                    File.Delete(file);
                }

                // Optionally delete all subdirectories
                string[] directories = Directory.GetDirectories(tempFolder);
                foreach (string directory in directories)
                {
                    Directory.Delete(directory, true);
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                // For now, silently fail or you could throw/rethrow
                throw new IOException($"Failed to clear Temp folder: {ex.Message}", ex);
            }
        }

        private static string GetProjectRoot()
        {
            try
            {
                // Start from the assembly location
                string assemblyLocation = Assembly.GetExecutingAssembly().Location;
                if (string.IsNullOrEmpty(assemblyLocation))
                {
                    // Fallback to AppContext.BaseDirectory for .NET 8
                    assemblyLocation = AppContext.BaseDirectory;
                }

                DirectoryInfo dir = new DirectoryInfo(Path.GetDirectoryName(assemblyLocation));
                
                // Navigate up the directory tree to find SqlBuilder.slnx
                while (dir != null)
                {
                    if (File.Exists(Path.Combine(dir.FullName, "SqlBuilder.slnx")))
                    {
                        return dir.FullName;
                    }
                    dir = dir.Parent;
                }
            }
            catch
            {
                // Return empty string if we can't determine the root
            }
            
            return string.Empty;
        }
    }
}
