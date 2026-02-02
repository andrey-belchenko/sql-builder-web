using SqlBuilderLib.DevTools;

public class DevUtilsProviderImpl : IDevUtilsProvider
    {
        public bool IsAnalyzerEnabled()
        {
            return DevAnalyzer.Enabled;
        }
        public bool IsPrepareOnly()
        {
            return DevAnalyzer.PrepareOnly;
        }
        public void AnalyzeExecSql(string sql)
        {
            DevAnalyzer.AnalyzeExecSql(sql);
        }

        public void AnalyzeCmdSql(string sql)
        {
           DevAnalyzer.AnalyzeCmdSql(sql);
        }

    }
