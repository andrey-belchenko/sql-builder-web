using SqlBuilderLib.DevTools;

public class DevUtilsProviderImpl : IDevUtilsProvider
{
    public bool IsAnalyzerEnabled()
    {
        return DevAnalyzer.Enabled;
    }

    public bool IsBuildingTs()
    {
        return TsBuilder.Enabled;
    }
    public bool IsPrepareOnly()
    {
        return DevAnalyzer.PrepareOnly;
    }
    public void AnalyzeExecSql(string sql)
    {

        if (!DevAnalyzer.Enabled) return;
        DevAnalyzer.AnalyzeExecSql(sql);
    }

    public void AnalyzeCmdSql(string sql)
    {
        if (!DevAnalyzer.Enabled) return;
        DevAnalyzer.AnalyzeCmdSql(sql);
    }

}
