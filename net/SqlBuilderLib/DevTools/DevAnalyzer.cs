using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SqlBuilderLib.DevTools
{
   
    internal static class DevAnalyzer
    {
        public static bool Enabled = false;
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
