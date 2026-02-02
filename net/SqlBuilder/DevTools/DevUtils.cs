using System.ComponentModel;

namespace SqlBuilderLib.DevTools
{
    public interface IDevUtilsProvider
    {
        bool IsAnalyzerEnabled();
        bool IsPrepareOnly();
        void AnalyzeExecSql(string sql);

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

        public static IDevUtilsProvider Instance =  new DevUtilsProvider(); 

    }
}