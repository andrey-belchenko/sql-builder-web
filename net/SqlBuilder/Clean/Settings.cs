using System.IO;

namespace sql.builder.Clean
{
    public class Settings
    {


        private string _tempPath = @"C:\Temp\SqlBuilder";
        public string TempPath
        {
            get
            {
                if (!Directory.Exists(_tempPath))
                {
                    Directory.CreateDirectory(_tempPath);
                }
                return _tempPath;
            }
        }

        private static Settings Instance;
        public static Settings GetInstance()
        {
            if (Instance == null)
            {
                Instance = new Settings();
            }
            return Instance;
        }
    }
}
