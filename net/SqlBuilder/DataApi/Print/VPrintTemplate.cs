using System.Collections.Generic;
using System.IO;
//using sql.builder.TFS;
using AName_ = sql.builder.DataApi.AName;

namespace sql.builder.DataApi
{
    /// <summary>
    /// &lt;template name="" title="" /&gt;
    /// </summary>
    public sealed class VPrintTemplate : VSXElement
    {
        public VPrintTemplate()
            : base(EName.template)
        {
        }
        public override string XName
        {
            get
            {
                return this.AttrOrEmpty(AName_.name);
            }
        }
        public override bool IsElementUser()
        {
            return true;
        }
        public override List<VSXElement> GetUsedElements()
        {
            string s = Path.Combine(ExcelPrintDocument.GetProjectExcelTemplates(this.GetParent().Name.LocalName), this.XName);
            //TFSHelper.CheckOutFile(s);
            //Cmn.FocusFile(s);
            return new List<VSXElement>();
            //return GetEnvironment().GetElement(TextConst.EName.ExcelTemplates, XName).AsList();
        }
        #region NodeText
        public override string GetNodeOtherInfo()
        {
            return this.XName + " " + Italic(this.P_Title);
        }
        #endregion
        #region Title
        public override string P_Title
        {
            get
            {
                return this.P_SelfTitle;
            }
        }
        public override bool P_Title_Exists()
        {
            return true;
        }
        #endregion
        #region SelfTitle
        public override bool P_SelfTitle_Exists()
        {
            return true;
        }
        #endregion
        #region Name
        public override bool P_Name_Exists()
        {
            return true;
        }
        #endregion
        #region ClientView
        public override string P_ClientView
        {
            get
            {
                return this.AttrOrEmpty(AName_.client_view);
            }
            set
            {
                this.SetAttributeValue(AName_.client_view, value);
            }
        }
        public override bool P_ClientView_Exists()
        {
            return true;
        }
        #endregion
        #region PrintXlsx
        public override string P_PrintXlsx
        {
            get
            {
                return this.AttrOrEmpty(AName_.print_xlsx);
            }
            set
            {
                this.SetAttributeValue(AName_.print_xlsx, value);
            }
        }
        public override bool P_PrintXlsx_Exists()
        {
            return this.GetParent() is VExcel;
        }
        #endregion
        #region UseFlexCel
        public override string P_UseFlexCel
        {
            get
            {
                return this.AttrOrEmpty(AName_.use_flexcel);
            }
            set
            {
                this.SetAttributeValue(AName_.use_flexcel, value);
            }
        }
        public override bool P_UseFlexCel_Exists()
        {
            return this.GetParent() is VExcel;
        }
        #endregion
        #region PostProcess
        public override string P_PostProcess
        {
            get
            {
                if (this.AttrOrEmpty(AName_.post_process) == TextConst.AVBool.False)
                {
                    return string.Empty;
                }
                else
                {
                    return TextConst.AVBool.True;
                }
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    value = TextConst.AVBool.False;
                }
                else
                {
                    value = null;
                }
                this.SetAttributeValue(AName_.post_process, value);
            }
        }
        public override bool P_PostProcess_Exists()
        {
            return true;
        }
        #endregion
    }
}