namespace SqlBuilderLib.DevTools
{
    public interface IDevUtilsProvider
    {
        bool IsAnalyzerEnabled();
        bool IsBuildingTs();
        bool IsPrepareOnly();
        void AnalyzeExecSql(string sql);

        void AnalyzeCmdSql(string sql);

    }

    public class DevUtilsProvider : IDevUtilsProvider
    {
        public bool IsAnalyzerEnabled()
        {
            return false;
        }
        public bool IsPrepareOnly()
        {
            return false;
        }
        public void AnalyzeExecSql(string sql)
        {

        }

        public void AnalyzeCmdSql(string sql)
        {

        }

        public bool IsBuildingTs()
        {
            return false;
        }

        public static IDevUtilsProvider Instance = new DevUtilsProvider();

    }
}