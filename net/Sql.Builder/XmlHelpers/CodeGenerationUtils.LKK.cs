using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Devart.Data.Oracle;
using sql.builder.DataApi;
using System.Collections.Generic;


namespace sql.builder.XmlHelpers
{
    internal static class CodeGenerationUtilsLKK
    {

        public static void Generate()
        {

            XmlReports.Environment.Manager.LoadProjectIfNeed("kido_lkk");
            CodeGenerationUtils.PfxToReaplace = new string[] { "_tmp", "_cls"};
            CodeGenerationUtils.PfxToReaplaceWeb = new string[] { "vcs_" };
            CodeGenerationUtils.PfxToReaplaceCls = new string[] { "vcs_" };
            var sb = new StringBuilder();
            sb.Append(DataFileBegin());
            sb.Append(CodeGenerationUtils.ClassDeclaration(new string[] { "vcs_user_login_cls", "vcs_get_login_info", "vcs_get_all_login_list", "vcs_get_petition", "vcs_get_company_list", "vcs_get_counteragent_list" }));
            sb.Append(DataFileBeginMethods());
            
            sb.Append(CodeGenerationUtils.UpdateTempFromObject("vcs_user_login_input"));
            sb.Append(CodeGenerationUtils.UpdateTempFromObject("vcs_petition_input"));
            sb.Append(CodeGenerationUtils.ExecuteMerge("vcs_user_login_merge"));
            sb.Append(CodeGenerationUtils.ExecuteMerge("vcs_user_login_cld_merge"));
            sb.Append(CodeGenerationUtils.ExecuteMerge("vcs_user_login_full_merge"));
            sb.Append(CodeGenerationUtils.ExecuteMerge("vcs_bind_request"));
            sb.Append(CodeGenerationUtils.ExecuteMerge("vcs_confirm_new_login"));
            sb.Append(CodeGenerationUtils.ExecuteDelete("vcs_user_login_delete"));
            
            sb.Append(CodeGenerationUtils.ExecuteSelect("vcs_get_login_info"));
            sb.Append(CodeGenerationUtils.ExecuteSelect("vcs_get_new_login_list"));
            sb.Append(CodeGenerationUtils.ExecuteSelect("vcs_get_bing_req_err"));
            sb.Append(CodeGenerationUtils.ExecuteSelect("vcs_check_can_bind"));
            sb.Append(CodeGenerationUtils.ExecuteSelect("vcs_get_all_login_list"));
            sb.Append(CodeGenerationUtils.ExecuteSelect("vcs_get_user_id"));
            sb.Append(CodeGenerationUtils.ExecuteSelect("vcs_get_petition"));
            sb.Append(CodeGenerationUtils.ExecuteSelect("vcs_get_petition_list"));
            sb.Append(CodeGenerationUtils.ExecuteSelect("vcs_get_company_list"));
            sb.Append(CodeGenerationUtils.ExecuteSelect("vcs_get_counteragent_list"));
            sb.Append(DataFileEndMethods());

            sb.Append(DataFileEnd());
            var dataFileName = XmlReports.GetRootPath() +
                               @"\..\..\web\lenenergo.lkk.data\BaseData.Belchenko.Generated.cs";
            Cmn.SaveTextWithCheckOut(sb.ToString(), dataFileName);

            sb = new StringBuilder();
            sb.Append(ServiceFileBegin());
            sb.Append(CodeGenerationUtils.MethodSelect("vcs_get_login_info",true));
            sb.Append(CodeGenerationUtils.MethodSelect("vcs_get_new_login_list", true));
            sb.Append(CodeGenerationUtils.MethodSelect("vcs_get_all_login_list",false));
            sb.Append(CodeGenerationUtils.MethodSelect("vcs_get_user_id", false));
            sb.Append(CodeGenerationUtils.MethodSelect("vcs_get_petition", false));
            sb.Append(CodeGenerationUtils.MethodSelect("vcs_get_petition_list", false));
            sb.Append(CodeGenerationUtils.MethodSelect("vcs_get_company_list", false));
            sb.Append(CodeGenerationUtils.MethodSelect("vcs_get_counteragent_list", false));
            sb.Append(ServiceFileEnd());
            var serviceFileName = XmlReports.GetRootPath() +
                                  @"\..\..\web\lenenergo.lkk.svc.web\Service.Belchenko.Generated.cs";
            Cmn.SaveTextWithCheckOut(sb.ToString(), serviceFileName);
            CodeGenerationUtils.PfxToReaplace = new string[] { };
            CodeGenerationUtils.PfxToReaplaceWeb = new string[] { };
            CodeGenerationUtils.PfxToReaplaceCls = new string[] { };
        }

        public static string ServiceFileBegin()
        {
            return
                @"using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using  lenenergo.lkk.data;
namespace lenenergo.lkk.svc.web
{
    public partial class Service : WebService
    {";
        }

        public static string ServiceFileEnd()
        {
            return
                @"}
}";
        }

        public static string DataFileBegin()
        {
            return
                @"using Devart.Data.Oracle;
using System;
using System.Data;
using  System.Collections.Generic;
using  System.Linq;
namespace lenenergo.lkk.data
{";
        }

        public static string DataFileBeginMethods()
        {
            return
                @"
    public partial class BelchenkoData:BaseData
    {";
        }

        public static string DataFileEndMethods()
        {
            return
                @"}";
        }
        public static string DataFileEnd()
        {
            return
                @"}";
        }




    }
}
