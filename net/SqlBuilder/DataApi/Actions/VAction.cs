using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using sql.builder.Exceptions;
using AName_ = sql.builder.DataApi.AName;

namespace sql.builder.DataApi
{
    public partial class VAction : VSXElement, IVParent
    {
        protected VAction(XName name)
            : base(name)
        {
        }
        public VAction()
            : base(EName.action)
        {
        }
        protected bool IsWithQuery()
        {
            return TextConst.AVActionTypeArray.WithQuery.Contains(this.P_ActionType);
        }
        private bool IsWithThisFormGroupControl()
        {
            return TextConst.AVActionTypeArray.WithThisFormGroupControl.Contains(this.P_ActionType);
        }
        private bool IsWithThisFormFieldControl()
        {
            return TextConst.AVActionTypeArray.WithThisFormFieldControl.Contains(this.P_ActionType);
        }
        public bool IsWithForm()
        {
            return TextConst.AVActionTypeArray.WithForm.Contains(this.P_ActionType);
        }
        protected bool IsWithReport()
        {
            return TextConst.AVActionTypeArray.WithReport.Contains(this.ActionOrSelf().P_ActionType);
        }
        private bool IsWithColumn()
        {
            return TextConst.AVActionTypeArray.WithColumn.Contains(this.ActionOrSelf().P_ActionType);
        }
        private bool IsWithTargetTable()
        {
            return TextConst.AVActionTypeArray.WithTargetTable.Contains(this.P_ActionType);
        }
        private bool IsRowAction()
        {
            return TextConst.AVActionTypeArray.RowActions.Contains(this.P_ActionType);
        }
        public override List<VParam> GetCalledElementFormalParams()
        {
            return this.FormalParams();
        }
        private List<VParam> FormalParams()
        {
            if (this.IsWithQuery())
            {
                VQuery cq = this.CalledQuery();
                if (cq == null)
                {
                    return new List<VParam>();
                }
                else
                {
                    return cq.Params();
                }
            }
            else if (this.IsWithForm())
            {
                VForm frm = this.CalledFormX();
                if (frm == null)
                {
                    throw new VCompilerException("Форма " + this.P_Form + " не найдена", this.GetMainParent(), this);
                }
                return frm.Params();
            }
            else if (this.IsWithReport())
            {
                return this.CalledReport().Params();
            }
            else
            {
                return this.ActionOrSelf().GetElementsP(EName.@params).SelectMany(VSXElement.GetElementsP).Cast<VParam>().ToList();
            }
        }
        public VQuery CalledQuery()
        {
            if (!this.IsWithQuery())
            {
                return null;
            }
            else
            {
                return XmlReports.Environment.GetQuery(this.P_Call);
            }
        }
        protected VForm CalledForm()
        {
            if (!this.IsWithFormX())
            {
                return null;
            }
            else
            {
                return (VForm)XmlReports.Environment.GetElement(EName.forms, this.P_Form, TextConst.AName.Name);
            }
        }
        protected VSourcedElement CalledReport()
        {
            if (!this.IsWithReport())
            {
                return null;
            }
            else
            {
                return (VSourcedElement)XmlReports.Environment.GetReportOrQuery(this.ActionOrSelf().P_Report);
            }
        }
        public virtual VForm CalledFormX()
        {
            return this.CalledForm();
        }
        public virtual string CalledControlX()
        {
            return this.P_Control;
        }
        public virtual bool IsWithFormX()
        {
            return this.IsWithForm();
        }
        private VSXElement CalledElement()
        {
            VSXElement el = this.CalledForm();
            if (el == null)
            {
                el = this.CalledQuery();
            }
            return el;
        }
        public override bool IsElementUser()
        {
            return this.IsWithQuery() || this.IsWithForm();
        }
        public override List<VSXElement> GetUsedElements()
        {
            VSXElement el = this.CalledElement();
            if (el != null)
            {
                return new List<VSXElement>(1) { el };
            }
            else
            {
                return new List<VSXElement>(0);
            }
        }
        public static string[] child_nodes_custom = { TextConst.EName.Params, TextConst.EName.UseAction, TextConst.EName.UsePart };
        public static string[] child_nodes_other = { TextConst.EName.Params, TextConst.EName.UsePart };
        IList<string> IVParent.AllowedChildNodes()
        {
            if (this.P_ActionType == TextConst.AVActionType.Custom)
            {
                return child_nodes_custom;
            }
            else
            {
                return child_nodes_other;
            }
        }
        #region Call
        public override bool P_Call_Exists()
        {
            return TextConst.AVActionTypeArray.Outer.Contains(this.P_ActionType);
        }
        #endregion
        #region IdName
        public override bool P_IdName_Exists()
        {
            return true;
        }
        public override string P_IdName
        {
            get
            {
                return this.AttrOrEmpty(AName_.name);
            }
            set
            {
                this.SetIdName(AName_.name, value);
            }
        }
        #endregion
        #region NodeText
        public override string GetNodeInfo()
        {
            return this.GetNodeOtherInfo();
        }
        public override string GetNodeOtherInfo()
        {
            string s = this.P_ActionType + " " + Bold(this.P_IdName) + " " + this.P_Call;
            return s;
        }
        #endregion
        #region ActionType
        public override bool P_ActionType_Exists()
        {
            return true;
        }
        #endregion
        #region CalledQuery
        public override string P_CalledQuery
        {
            get
            {
                return this.AttrOrEmpty(AName_.call);
            }
            set
            {
                this.SetAttributeNotEmpty(AName_.call, value);
            }
        }
        public override void P_CalledQuery_ListRefresh(VDataTable table)
        {
            table.Rows.Clear();
            IList<VSXElement> queries = XmlReports.Environment.GetElements(TextConst.EName.Queries);
            for (int index = 0; index < queries.Count; index++)
            {
                VQuery el = (VQuery)queries[index];
                if (!el.IsExtension())
                {
                    string name = el.P_Name;
                    table.AddRow(name, name, el.P_Title);
                }
            }
        }
        public override bool P_CalledQuery_Exists()
        {
            return this.IsWithQuery();
        }
        #endregion
        #region Form
        public override string P_Form
        {
            get
            {
                return this.AttrOrEmpty(AName_.call);
            }
            set
            {
                this.SetAttributeNotEmpty(AName_.call, value);
            }
        }
        public override bool P_Form_Exists()
        {
            return this.IsWithForm();
        }
        #endregion
        #region Report
        public override string P_Report
        {
            get
            {
                return this.AttrOrEmpty(AName_.call);
            }
            set
            {
                this.SetAttributeNotEmpty(AName_.call, value);
            }
        }
        public override bool P_Report_Exists()
        {
            return this.IsWithReport();
        }
        public void P_Report_List(VDataTable table)
        {
            table.AddColumn("id", "Отчёт");
            table.AddColumn("title", "Заголовок");
        }
        public void P_Report_ListRefresh(VDataTable table)
        {
            table.Rows.Clear();
            IList<VSXElement> list;
            if (this.P_ActionType == TextConst.AVActionType.OpenColGrDetailReport)
            {
                list = XmlReports.Environment.GetQReports();
            }
            else
            {
                list = XmlReports.Environment.GetReportsAndQReports();
            }
            for (int index = 0; index < list.Count; index++)
            {
                VSXElement report = list[index];
                table.AddRow(report.XName, report.P_Title);
            }
        }
        #endregion
        #region Control
        public override void P_Control_ListRefresh(VDataTable table)
        {
            table.Rows.Clear();
            VForm form = null;

            if (IsWithFormX())
            {
                form = CalledFormX();
                foreach (VFieldGroup group in form.Groups())
                {
                    if (group.P_Alias != "")
                    {
                        table.Rows.Add(group.P_Alias, group.P_Alias);
                    }
                }
            }
            else if (IsWithThisFormGroupControl())
            {
                var flds = new List<VSXElement>();
                if (RootQuery() is VForm)
                {
                    flds = ((VForm)RootQuery()).Fields();
                }
                else if (RootQuery() is VQuery)
                {
                    flds = ((VQuery)RootQuery()).Fields();
                }
                if (flds != null)
                {
                    foreach (VSXElement group in flds)
                    {
                        if (group.P_Alias != "")
                        {
                            table.Rows.Add(group.P_Alias, group.P_Alias);
                        }
                    }
                }
            }
            else if (IsWithThisFormFieldControl())
            {
                var flds = new List<VSXElement>();
                if (RootQuery() is VForm)
                {
                    flds = ((VForm)RootQuery()).Fields();
                }
                else if (RootQuery() is VQuery)
                {
                    flds = ((VQuery)RootQuery()).Fields();
                }
                if (flds != null)
                {
                    foreach (VSXElement fld in flds)
                    {
                        table.Rows.Add(fld.P_Name, fld.P_Name);
                    }
                }
            }

        }
        public override bool P_Control_Exists()
        {
            return this.IsWithFormX() || this.IsWithThisFormGroupControl() || this.IsWithThisFormFieldControl();
        }
        #endregion
        #region SelfTitle
        public override bool P_SelfTitle_Exists()
        {
            return true;
        }
        #endregion
        #region Title
        public override bool P_Title_Exists()
        {
            return true;
        }
        public override string P_Title
        {
            get
            {
                string title = this.P_SelfTitle;
                if (!string.IsNullOrEmpty(title))
                {
                    return title;
                }
                if (this.IsWithForm())
                {
                    VForm frm = this.CalledForm();
                    if (frm != null)
                    {
                        return frm.P_SelfTitle;
                    }
                }
                return string.Empty;
            }
        }
        #endregion
        #region UpdateTarget
        public override bool P_UpdateTarget_Exists()
        {
            return IsWithTargetTable();
        }
        #endregion
        #region UpdateTargetS
        public override bool P_UpdateTargetS_Exists()
        {
            return IsWithTargetTable();
        }
        #endregion
        #region ActionRows
        public override bool P_ActionRows_Exists()
        {
            return true;
        }
        #endregion
        #region Text
        public override string P_Text
        {
            get
            {
                XElement text_node = this.Element(EName.text);
                string text;
                if (text_node == null)
                {
                    text = string.Empty;
                }
                else
                {
                    text = text_node.Value;
                    if (string.IsNullOrEmpty(text))
                    {
                        text = string.Empty;
                    }
                    else
                    {
                        text = text.Trim();
                    }
                }
                return text;
            }
            set
            {
                string text;
                if (string.IsNullOrEmpty(value))
                {
                    text = string.Empty;
                }
                else
                {
                    text = value.Trim();
                }
                XCData cdata_node = new XCData(text);
                XElement text_node = this.Element(EName.text);
                if (text_node == null)
                {
                    text_node = new XElement(EName.text, cdata_node);
                    this.Add(text_node);
                }
                else
                {
                    text_node.RemoveNodes();
                    text_node.Add(cdata_node);
                }
            }
        }
        public override bool P_Text_Exists()
        {
            string action_type = this.AttrOrDefault(AName_.action_type, null);
            return action_type == TextConst.AVActionType.CallPlsql || action_type == TextConst.AVActionType.CallPlsqlAdd;
        }
        #endregion
        #region Column
        public override void P_Column_ListRefresh(VDataTable table)
        {
            this.P_Column_List(table);
            table.Rows.Clear();
            VForm frm = this.RootQuery() as VForm;
            if (frm == null)
            {
                return;
            }
            VQueryCall src = frm.AllSources().First(e => e.XName == this.P_CalledObject);
            HashSet<string> srcNames = new HashSet<string>();
            foreach (VQueryCall q in src.SelfAndAllMasterLinks())
            {
                string name = q.XName;
                if (!srcNames.Contains(name))
                {
                    srcNames.Add(name);
                }
            }
            List<VSXElement> cols = frm.Columns().Where(e => srcNames.Contains(e.P_Table)).ToList();
            //var grids = frm.GetContentSections().SelectMany(e => e.GetDescedantsApplyingParts(TextConst.EName.Column)).Select(e1 => (VGrid)e1).ToList();
            //var grid = grids.Where(g => g.P_Table == P_CalledObject).FirstOrDefault();
            //if (grid == null)
            //{
            //    return;
            //}
            foreach (VSXElement col in cols)
            {
                AddColumnInfoToList(table, col.XName, col);
            }
        }
        public override bool P_Column_Exists()
        {
            return IsWithColumn();
        }
        #endregion
        #region IsRet
        public override string P_IsRet
        { // в action нужен для случая когда выполняются много разных действий создающих строки а возвращат нужно  не все созданные коды
            get
            {
                if (this.AttrOrEmpty(AName_.is_ret) != TextConst.AVBool.False)
                {
                    return TextConst.AVBool.True;
                }
                else
                {
                    return TextConst.AVBool.False;
                }
            }
            set
            {
                string val = value;
                if (val == TextConst.AVBool.True)
                {
                    val = null;
                }
                else
                {
                    val = TextConst.AVBool.False;
                }
                this.SetAttributeValue(AName_.is_ret, val);
            }
        }
        public override string P_IsRet_Title()
        {
            return "Может возвращать значение";
        }
        public override bool P_IsRet_Exists()
        {
            return true;
        }
        #endregion
        #region EditorButtonType
        public override bool P_EditorButtonType_Exists()
        {
            return true;
        }
        #endregion
        #region EditorButtonType
        public override bool P_EditorButtonSide_Exists()
        {
            return true;
        }
        #endregion
        #region Message
        public override string P_Message
        {
            get
            {
                return this.AttrOrEmpty(AName_.message);
            }
            set
            {
                this.SetAttributeNotEmpty(AName_.message, value);
            }
        }
        #endregion
        #region Notification
        public override string P_Notification
        {
            get
            {
                return this.AttrOrEmpty(AName_.notification);
            }
            set
            {
                this.SetAttributeNotEmpty(AName_.notification, value);
            }
        }
        #endregion
        #region Prompt
        public override string P_Prompt
        {
            get
            {
                return this.AttrOrEmpty(AName_.prompt);
            }
            set
            {
                this.SetAttributeNotEmpty(AName_.prompt, value);
            }
        }
        #endregion
    }
}