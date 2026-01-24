using System.Text;
using Devart.Data.Oracle;

namespace sql.builder.MP
{
    public static class MPEnvironment
    {
        public static string FieldsDelimiter = "^|";
        public static string RowsDelimiter = "^*";
        public static Encoding DefaultEncoding = Encoding.GetEncoding("Windows-1251");

        public static string DBName = "kido";
        public static string DBUserName = "planb";
        public static string DBPassword = "kl0pik";

        public static string WorkFolder = "";

        public static string DataFileName = "data.txt";
        public static string BadFileName = "bad.txt";
        public static string DiscardFileName = "discard.txt";
        public static string SettingsFileName = "settings.txt";

        public static OracleConnection Connection;

        public static int ExcelReaderBlockSize = 10000;
        public static int DataToFileBlockSize = 5000;

        public static string GetDataFilePath()
        {
            return (WorkFolder != "") 
                ? WorkFolder + "\\" + DataFileName 
                : DataFileName;
        }
        public static string GetBadFilePath()
        {
            return (WorkFolder != "")
                ? WorkFolder + "\\" + BadFileName
                : BadFileName;
        }
        public static string GetDiscardFilePath()
        {
            return (WorkFolder != "")
                ? WorkFolder + "\\" + DiscardFileName
                : DiscardFileName;
        }
        public static string GetSettingsFilePath()
        {
            return (WorkFolder != "")
                ? WorkFolder + "\\" + SettingsFileName
                : SettingsFileName;
        }
    }
}