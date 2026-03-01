using System.Xml.Linq;
using AName_ = sql.builder.DataApi.AName;

namespace sql.builder.DataApi
{
    public sealed class VUseForm : VUseAction
    {
        public VUseForm()
            : base(EName.useform)
        {
        }
        public static VUseForm Get(string form_name)
        {
            XElement xuseform = XmlReports.GetCurrentNavigator().Descendants(EName.useform).SearchByAttribute(AName_.form, form_name);
            if (xuseform != null)
            {
                return VSXElement.Get(xuseform) as VUseForm;
            }
            else
            {
                return null;
            }
        }
        #region SelfTitle
        public override bool P_SelfTitle_Exists()
        {
            return (this.GetMainParent() is VNavigator);
        }
        #endregion
        #region NodeText
        public override string GetNodeInfo()
        {
            if (this.GetMainParent() is VNavigator)
            {
                string form = this.P_Form;
                string s = this.P_NodeName + " " + Bold(form);
                VForm f = XmlReports.Environment.GetFormOrQueryAsForm(form);
                if (f != null)
                {
                    s += " " + Italic(f.P_SelfTitle);
                }
                return s;
            }
            else
            {
                return base.GetNodeInfo();
            }
        }
        #endregion
        #region Form
        public override string P_Form
        {
            get
            {
                if (this.GetMainParent() is VNavigator)
                {
                    return this.AttrOrEmpty(AName_.form);
                }
                else
                {
                    return base.P_Form;
                }
            }
            set
            {
                if (this.GetMainParent() is VNavigator)
                {
                    this.SetAttributeNotEmpty(AName_.form, value);
                    VForm form = XmlReports.Environment.GetForm(value);
                    if (form != null)
                    {
                        P_SelfTitle = form.P_SelfTitle;
                        P_Project = Cmn.ExtractProjectName(form.Attribute(AName_.file).Value);
                    }
                }
                else
                {
                    base.P_Form = value;
                }
            }
        }
        public override bool P_Form_Exists()
        {
            if (this.GetMainParent() is VNavigator)
            {
                return true;
            }
            else
            {
                return base.P_Form_Exists();
            }
        }
        #endregion
        #region Project
        public override bool P_Project_Exists()
        {
            return (this.GetMainParent() is VNavigator);
        }
        #endregion
    }
}