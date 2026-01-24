using System.Diagnostics;
using System.IO;
using System.Text;
//using DevExpress.Utils.Drawing;
using sql.builder.MP.Tools;


namespace sql.builder.MP
{
    public class SqlLoader
    {
        CommandPrompt _cmd;

        public SqlLoader(CommandPrompt cmd)
        {
            _cmd = cmd;
        }

        public void FillTable(string tableName, MPColumn[] columns)
        {
            // генерируем скрипт и сохраняем его в файл
            string script = SqlLoaderScriptGenerator.Generate(tableName, columns, MPEnvironment.GetDataFilePath(), MPEnvironment.GetBadFilePath(), MPEnvironment.GetDiscardFilePath());
            File.WriteAllText(MPEnvironment.GetSettingsFilePath(), script, Encoding.Default);
            // выполняем загрузку в БД
            db.ExecuteNonQuery(" begin delete " + tableName+"; commit; end;");
           
            string command = string.Format("sqlldr userid='{1}/{2}@{0}' control=\"{3}\", LOG=\"{4}\"",
                MPEnvironment.DBName, MPEnvironment.DBUserName, MPEnvironment.DBPassword, MPEnvironment.GetSettingsFilePath(), Path.GetTempPath() + "sqllder.log");
            _cmd.Execute(command);
        }
    }
}