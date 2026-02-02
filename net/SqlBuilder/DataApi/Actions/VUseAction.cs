using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    public partial class VUseAction : VAction, IVParent
    {
        protected VUseAction(XName name)
            : base(name)
        {
        }
        public VUseAction()
            : base(EName.useaction)
        {
        }
        public override string CalledControlX()
        {
            string control = this.P_Control;
            if (control != string.Empty) {
                return control;
            }
            VAction act = this.Action();
            if (act != null) {
                return act.P_Control;
            } else {
                return string.Empty;
            }
        }
        public override bool IsWithFormX()
        {
            VAction act = this.ActionOrSelf();
            if (act != null) {
                return act.IsWithForm();
            } else {
                return false;
            }
        }
        public override VForm CalledFormX()
        {
            VForm frm = this.CalledForm();
            if (frm == null) {
                VAction act = this.Action();
                if (act != null) {
                    frm = act.CalledFormX();
                }
            }
            return frm;
        }
        public override VAction Action()
        {
            string name = this.AName();
            if (this.P_CalledObject == string.Empty) {
                if (name == string.Empty) {
                    return null;
                } else {
                    //return (VAction)XmlReports.Environment.GetElement(TextConst.EName.Actions, name);
                    return XmlReports.Environment.GetAction(name);
                }
            } else {
                VQueryCall obj = this.GetObject();
                if (obj != null) {
                    VQuery qry = obj.Query();
                    if (qry != null) {
                        return qry.GetAction(name);
                    }
                }
            }
            return null;
        }
        //public VQueryCall Query()
        //{
        //    return (VQueryCall)this.GetParent().GetParent();
        //}
        private bool IsColumnEvent()
        {
            return this.P_EventName == TextConst.AVEventName.DoubleClick;
        }
        public override bool IsElementUser()
        {
            return true;
        }
        public override List<VSXElement> GetUsedElements()
        {
            VAction act = this.Action();
            if (act != null) {
                return act.AsList();
            } else if (this.IsWithQuery()) {
                return this.CalledQuery().AsList();
            } else if (this.IsWithForm()) {
                return this.CalledForm().AsList();
            } else if (this.IsWithReport()) {
                return this.CalledReport().AsList();
            } else {
                return new List<VSXElement>();
            }
        }
        public static string[] child_nodes_custom = { TextConst.EName.UseAction, TextConst.EName.UsePart };
        public static string[] child_nodes_other = { TextConst.EName.Column, TextConst.EName.Call, TextConst.EName.Const, TextConst.EName.UseParam, TextConst.EName.Fact, TextConst.EName.ColDimVal, TextConst.EName.UsePart };
        IList<string> IVParent.AllowedChildNodes()
        {
            if (this.P_ActionType == TextConst.AVActionType.Custom) {
                return child_nodes_custom;
            } else {
                return child_nodes_other;
            }
        }
        #region CalledAction
        public override bool P_CalledAction_Exists()
        {
            return true;
        }
        public void P_CalledAction_List(VDataTable table)
        {
            table.AddColumn("id", "Имя");
            table.AddColumn("title", "Заголовок");
        }
        public void P_CalledAction_ListRefresh(VDataTable table)
        {
            table.Rows.Clear();
            IList<VAction> list;
            if (this.P_CalledObject == string.Empty) {
                list = XmlReports.Environment.GetElements(TextConst.EName.Actions).Cast<VAction>().ToList();
            } else {
                list = this.GetObject().Query().Actions();
            }
            for (int index = 0; index < list.Count; index++) {
                VAction el = list[index];
                table.Rows.Add(el.P_IdName, el.P_Title);
            }
        }
        #endregion
        #region CalledObject
        public override bool P_CalledObject_Exists()
        {
            return true;
        }
        public void P_CalledObject_List(VDataTable table)
        {
            table.AddColumn("id", "Имя");
            table.AddColumn("title", "Заголовок");
        }
        public void P_CalledObject_ListRefresh(VDataTable table)
        {
            table.Rows.Clear();
            IList<VQueryCall> list = (this.RootQuery() as VForm).MainAndRelatedQueries();
            for (int index = 0; index < list.Count; index++) {
                VQueryCall el = list[index];
                table.AddRow(el.XName, el.P_Title);
            }
        }
        #endregion
        #region Column
        public override void P_Column_ListRefresh(VDataTable table)
        {
            if (!this.IsColumnEvent()) {
                base.P_Column_ListRefresh(table);
                return;
            }
            //P_Column_List(table);
            table.Rows.Clear();
            VSourcedElement rootQuery = null;
            if (!(this.GetMainParent() is VReport)) {
                rootQuery = this.ExtendedOrRootQuery();
            } else {
                rootQuery = XmlReports.Environment.GetQuery(this.GetParent().GetParent().P_CalledQuery);
            }
            //var exsistsNames = rootQuery.ViewColumns().Select(e => e.P_Column).ToList();
            HashSet<string> exsistsNames = new HashSet<string>();
            IList<VViewColumn> view_cols = rootQuery.ViewColumns();
            int index;
            for (index = 0; index < view_cols.Count; index++) {
                string name = view_cols[index].P_Column;
                if (!exsistsNames.Contains(name)) {
                    exsistsNames.Add(name);
                }
            }
            IList<VSXElement> cols = rootQuery.Columns();
            for (index = 0; index < cols.Count; index++) {
                VSXElement el = cols[index];
                string name = el.XName;
                if (!exsistsNames.Contains(name)) {
                    AddColumnInfoToList(table, name, el);
                }
            }
        }
        public override bool P_Column_Exists()
        {
            return this.IsColumnEvent() || base.P_Column_Exists();
        }
        #endregion
        #region NodeText
        public override string GetNodeInfo()
        {
            return this.GetNodeOtherInfo();
        }
        public override string GetNodeOtherInfo()
        {
            string s;
            string event_name = this.P_EventName;
            if (!string.IsNullOrEmpty(event_name)) {
                s = "on " + this.P_Column + " " + event_name + ":";
            } else {
                s = string.Empty;
            }
            string called_object = this.P_CalledObject;
            if (!string.IsNullOrEmpty(called_object)) {
                s +=Bold(called_object + ".");
            }
            s += Bold(this.P_CalledAction);
            string action = this.P_ActionType;
            if (!string.IsNullOrEmpty(action)) { 
                s += " " + action + " ";
            }
            string call = this.P_Call;
            if (!string.IsNullOrEmpty(call)) { 
                s += " " + Bold(call);
            }
            if (string.IsNullOrEmpty(s)) {
                s = base.GetNodeOtherInfo();
            }
            string title = this.P_Title;
            if (!string.IsNullOrEmpty(title)) { 
                s += " " + Italic(title);
            }
            return s;
        }
        #endregion
        #region Title
        public override bool P_Title_Exists()
        {
            return true;
        }
        public override string P_Title {
            get {
                string title = this.P_SelfTitle;
                if (!string.IsNullOrEmpty(title)) {
                    return title;
                }
                VAction act = this.Action();
                if (act != null) {
                    return act.P_Title;
                } else if (this.IsWithForm()) {
                    VForm frm = this.CalledForm();
                    if (frm != null) {
                        return frm.P_SelfTitle;
                    }
                }
                return string.Empty;
            }
        }
        #endregion
        #region Modal
        public override bool P_Modal_Exists()
        {
            return this.IsWithFormX();
        }
        #endregion
        #region ActionType
        public override bool P_ActionType_Exists()
        {
            return true;
        }
        #endregion
        #region EventName
        public override bool P_EventName_Exists()
        {
            return this.GetParent() is VEvents;
        }
        public void P_EventName_List(VDataTable table)
        {
            table.AddColumn("id");
            table.AddColumn("name");
        }
        public void P_EventName_ListRefresh(VDataTable table)
        {
            VSXElement.FillDataTableFromStringArray(table, TextConst.AVEventNameArray.All);
        }
        #endregion
        #region ColumnVisible
        public override bool P_ColumnVisible_Exists()
        {
            return true;
        }
        #endregion
		#region DetailsUseZeros
		public override bool P_DetailsUseZeros_Exists()
        {
			return TextConst.AVActionTypeArray.DetailReport.Contains(this.P_ActionType);
        }
        #endregion
    }}