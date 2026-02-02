using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using AName_ = sql.builder.DataApi.AName;

namespace sql.builder.DataApi
{
    public partial class VReport
    {
        public VDBSelectCommand GetRepInsertCommand()
        {
            string procText = sql.builder.XmlHelpers.SqlReportPkg.Generate(this.P_IdName, true,true,false);
            var cmd = new VDBSelectCommand(procText, this);
            cmd.CreateRetParam();
            return cmd;
        }
        public List<VQueryCall> Queries()
        {
            return this.Element(EName.queries).Descendants(EName.query).ToList().SelectAsArray(VSXElement.Get<VQueryCall>).ToList();
        }
        public override List<VQueryCall> AllSources()
        {
            return this.Queries();
        }
        public VQueryCall GetQuery(string name)
        {
            IList<VQueryCall> queries = this.Queries();
            for (int index = 0; index < queries.Count; index++) {
                VQueryCall query = queries[index];
                if (query.XName == name) {
                    return query;
                }
            }
            return null;
        }
        public List<VPrintTemplate> PrintTemplates()
        {
            return this.Descendants(EName.print_templates).Descendants(EName.template).ToList().SelectAsArray(VSXElement.Get<VPrintTemplate>).ToList();
        }
        public VReportProc GetReportProc()
        {
            IList<VSXElement> list = this.GetElementsP(EName.procedure);
            if (list.Count == 0) {
                return null;
            } else {
                return (VReportProc)list[0];
            }
        }
        private static string[] child_nodes = { TextConst.EName.Params, TextConst.EName.ReportProc, TextConst.EName.Queries, TextConst.EName.PrintTemplates, TextConst.EName.UsePart, TextConst.EName.Customers };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
        #region IdName
        public override string P_IdName {
            get {
                return this.AttrOrEmpty(AName_.name);
            }
            set {
                this.SetIdName(AName_.name, value);
            }
        }
        public new bool P_IdName_Exists()
        {
            return true;
        }
        #endregion
        #region Form
        public override bool P_Form_Exists()
        {
            return true;
        }
        #endregion
        #region ViewMode
        public override bool P_ViewMode_Exists()
        {
            return true;
        }
        #endregion
        #region ParamsCustomization
        public override bool P_ParamsCustomization_Exists()
        {
            return true;
        }
        #endregion
        #region AllowSave
        public override bool P_AllowSave_Exists()
        {
            return true;
        }
        #endregion
        #region EditColumns
        public override bool P_EditColumns_Exists()
        {
            return true;
        }
        #endregion
        #region Folder
        public override bool P_Folder_Exists()
        {
            return true;
        }
        #endregion
        #region DxExport
        public override bool P_DxExport_Exists()
        {
            return true;
        }
        #endregion
        #region AutoMerge
        public override bool P_AutoMerge_Exists()
        {
            return true;
        }
        #endregion
        #region NoGrid
        public override bool P_NoGrid_Exists()
        {
            return true;
        }
        #endregion
        #region SaveCompiled
        public override bool P_SaveCompiled_Exists()
        {
            return true;
        }
        #endregion
        #region Invisible
        public override bool P_Invisible_Exists()
        {
            return true;
        }
        #endregion
        #region UseTemp
        public override bool P_UseTemp_Exists()
        {
            return true;
        }
        #endregion
        #region UseDataReader
        public override bool P_UseDataReader_Exists()
        {
            return true;
        }
        #endregion
        #region MultiSelect
        public override string P_MultiSelect {
            get {
                return this.AttrOrEmpty(AName_.multi_select);
            }
            set {
                this.SetAttributeValue(AName_.multi_select, value);
            }
        }
        public override bool P_MultiSelect_Exists()
        {
            return true;
        }
        #endregion
    }
}