using System;
using System.IO;

using sql.builder.MP.Tools;


namespace sql.builder.MP
{
    public class MPSqlLoader: IMPDataLoader
    {
        public Announcer Announcer { get; set; }

        public bool FillTable(string tableName, MPColumn[] columns)
        {
            ClearFiles();

            var cmd = new CommandPrompt();
            var sqlLoader = new SqlLoader(cmd);
            sqlLoader.FillTable(tableName, columns);

            if (cmd.ErrorText != null && Announcer != null)
            {
                Announcer.Announce("Ошибка: " + cmd.ErrorText.Trim(), MessageStatus.Error);
            }
			if (cmd.ErrorText != null)
			{
				//if (!cmd.ErrorText.Contains("LRM")) return true;
				throw new Exception(cmd.ErrorText);
			}
            return (cmd.ErrorText == null);
        }

        public string GetLastLogText()
        {
            string logFilePath = Path.ChangeExtension(MPEnvironment.GetSettingsFilePath(), ".log");
            if (!File.Exists(logFilePath)) return "";

            string text = File.ReadAllText(logFilePath, MPEnvironment.DefaultEncoding);
            return text;
        }
        public string GetLastBadText()
        {
            string badFilePath = MPEnvironment.GetBadFilePath();
            if (!File.Exists(badFilePath)) return "";

            string text = File.ReadAllText(badFilePath, MPEnvironment.DefaultEncoding);
            return text;
        }
        public string GetLastDiscardText()
        {
            string discardFilePath = MPEnvironment.GetDiscardFilePath(); 
            if (!File.Exists(discardFilePath)) return "";

            string text = File.ReadAllText(discardFilePath, MPEnvironment.DefaultEncoding);
            return text;
        }

        private void ClearFiles()
        {
            var paths = new[] { MPEnvironment.GetBadFilePath(), MPEnvironment.GetDiscardFilePath() };
            foreach (var path in paths)
            {
                if (File.Exists(path)) File.Delete(path); 
            }
        }
    }
}