using System;

namespace sql.builder.Clean
{
    public class Settings
    {


        public string TempPath = @"C:\Temp\SqlBuilder";

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
