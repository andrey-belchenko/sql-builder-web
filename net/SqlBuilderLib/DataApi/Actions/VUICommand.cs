using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using AName_ = sql.builder.DataApi.AName;

namespace sql.builder.DataApi
{
    internal sealed class VUICommand : VUseAction
    {
        internal VUICommand()
            : base(EName.uicommand)
        {
        }
        internal VSXElement ButtonType()
        {
            string button_type = this.P_ButtonType;
            if (string.IsNullOrEmpty(button_type)) {
                return null;
            } else {
                return XmlReports.Environment.GetElement(EName.button_types, button_type, TextConst.AName.Name);
            }
        }
        //#region NodeName
        //public override void AllowedChildNodes(VDataTable table)
        //{
        //    table.Rows.Add(TextConst.EName.Column, TextConst.EName.Column);
        //    //table.Rows.Add(TextConst.EName.Call, TextConst.EName.Call);
        //    table.Rows.Add(TextConst.EName.Const, TextConst.EName.Const);
        //    table.Rows.Add(TextConst.EName.Fact, TextConst.EName.Fact);
        //    table.Rows.Add(TextConst.EName.UseParam, TextConst.EName.UseParam);
        //}
        //#endregion
        #region SelfTitle
        public override bool P_SelfTitle_Exists()
        {
            return true;
        }
        #endregion
        #region ButtonType
        public override string P_ButtonType {
            get {
                return this.AttrOrEmpty(AName_.button_type);
            }
            set {
                this.SetAttributeValue(AName_.button_type, value);
            }
        }
        public override bool P_ButtonType_Exists()
        {
            return true;
        }
        public void P_ButtonType_List(VDataTable table)
        {
            table.AddColumn("id", "Имя");
            table.AddColumn("title", "Заголовок");
        }
        public void P_ButtonType_ListRefresh(VDataTable table)
        {
            table.Rows.Clear();
            foreach (XElement el in XmlReports.Environment.Manager.GetNativeScheme().Elements(EName.button_types).Elements(EName.button_type)) {
                table.AddRow(el.AttrOrEmpty(AName_.name), el.AttrOrEmpty(AName_.title));
            }
        }
        #endregion
        #region NodeText
        public override string GetNodeOtherInfo()
        {
            string s = base.GetNodeOtherInfo();
            string button_type = this.P_ButtonType;
            if (!string.IsNullOrEmpty(button_type)) {
                s += " (" + button_type + ")";
            }
            return s;
        }
        #endregion
        #region Editable
        public override bool P_Editable_Exists()
        {
            return true;
        }
        #endregion
        #region Title
        public override string P_Title {
            get {
                string s = base.P_Title;
                if (string.IsNullOrEmpty(s)) {
                    VSXElement btntype = this.ButtonType();
                    if (btntype != null) {
                        s = btntype.P_SelfTitle;
                    }
                }
                return s;
            }
        }
        #endregion
        #region SecurityId
        public override bool P_SecurityId_Exists()
        {
            return true;
        }
        #endregion
        #region Message
        public override bool P_Message_Exists()
        {
            return true;
        }
        #endregion
		#region Notification
        public override bool P_Notification_Exists()
		{
			return true;
		}
		#endregion
		#region Prompt
        public override bool P_Prompt_Exists()
        {
            return true;
        }
        #endregion
    }
}