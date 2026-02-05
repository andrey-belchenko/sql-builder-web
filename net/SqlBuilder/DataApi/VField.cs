using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using sql.builder.UI;
using AName_ = sql.builder.DataApi.AName;

namespace sql.builder.DataApi
{
    public sealed partial class VField : VSXElement, IVParent
    {
        public VField()
            : base(EName.field)
        {
            this.KeyField = AName_.id;
        }
        public VQueryCall ListQuery()
        {
            IList<VSXElement> list = this.GetDescedantsP(EName.listquery);
            if (list.Count == 0) {
                return null;
            }
            list = list[0].GetElementsP(EName.query);
            if (list.Count == 0) {
                return null;
            } else {
                return (VQueryCall)list[0];
            }
        }

        public VQueryCall DefaultQuery()
        {
            IList<VSXElement> list = this.GetDescedantsP(EName.defaultquery);
            if (list.Count == 0) {
                return null;
            }
            list = list[0].GetElementsP(EName.query);
            if (list.Count == 0) {
                return null;
            } else {
                return (VQueryCall)list[0];
            }
        }
        /*public VQueryCall DefaultQuery()
        {
            var el = GetDescedantsP(TextConst.EName.DefaultQuery)
                .Select(e => e.GetElementsP(TextConst.EName.Query)
                    .FirstOrDefault()).FirstOrDefault();
            return (VQueryCall)el;
        }*/
        public override VField Field()
        {
            return this;
        }
        public static string[] child_nodes = { TextConst.EName.ListQuery, TextConst.EName.DefaultQuery, TextConst.EName.Buttons, TextConst.EName.UsePart };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
        #region SelfTitle
        public override bool P_SelfTitle_Exists()
        {
            return true;
        }
        #endregion
        #region ControlType
        public override bool P_ControlType_Exists()
        {
            return true;
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
            string name = this.P_IdName;
            if (!string.IsNullOrEmpty(name)) {
                s = name + " ";
            } else {
                s = string.Empty;
            }
            s += this.P_FormalParName + " " + Italic(this.P_SelfTitle);
            return s;
        }
        #endregion
        #region Size
        public override bool P_Size_Exists()
        {
            return true;
        }
        public override bool P_Size_Editable()
        {
            return true;
        }
        #endregion
        #region Position
        public override bool P_Position_Exists()
        {
            return true;
        }
        public override bool P_Position_Editable()
        {
            return true;
        }
        #endregion
        #region TextVisible
        public override bool P_TextVisible_Exists()
        {
            return true;
        }
        public override bool P_TextVisible_Editable()
        {
            return true;
        }
        #endregion
        #region TextLocation
        public override bool P_TextLocation_Exists()
        {
            return true;
        }
        public override bool P_TextLocation_Editable()
        {
            return true;
        }
        #endregion
        #region Step
        public override bool P_Step_Exists()
        {
            return (this.P_ControlType == TextConst.AVControlType.Number);
        }
        #endregion
        #region EditMask
        public override bool P_EditMask_Exists()
        {
            return (this.P_ControlType == TextConst.AVControlType.Number);
        }
        #endregion
        //#region Required
        //public override bool P_Required_Exists()
        //{
        //    return true;
        //}
        //#endregion
        #region FormalParName
        public override bool P_FormalParName_Exists()
        {
            return true;
        }
        #endregion
        #region FormalParNameS
        public override bool P_FormalParNameS_Exists()
        {
            return true;
        }
        #endregion
        #region RowsLimit
        public override bool P_RowsLimit_Exists()
        {
            return true;
        }
        #endregion
        #region IdName
        public override string P_IdName {
            get {
                return this.AttrOrEmpty(AName_.id);
            }
            set {
                this.SetIdName(AName_.id, value);
            }
        }
        public override bool P_IdName_Exists()
        {
            return this.IsMainElement();
        }
        #endregion
        #region ParentFieldName
        public override bool P_ParentFieldName_Exists()
        {
            return true;
        }
        #endregion
        #region ColumnEditable
        public override bool P_ColumnEditable_Exists()
        {
            return true;
        }
        #endregion
        #region Editable
        public override bool P_Editable_Exists()
        {
            return true;
        }
        #endregion
        #region ColumnVisible
        public override bool P_ColumnVisible_Exists()
        {
            return true;
        }
        #endregion
        #region DataType
        public override bool P_DataType_Exists()
        {
            return true;
        }
        #endregion
        #region Checked
        public override bool P_Checked_Exists()
        {
            return true;
        }
        #endregion
        #region NewVal
        public override bool P_NewVal_Exists()
        {
            return true;
        }
        #endregion
        #region ColumnMandatory
        public override bool P_ColumnMandatory_Exists()
        {
            return true;
        }
        #endregion
        #region ShowCheckbox
        public override string P_ShowCheckbox {
            get {
                if (this.AttrOrEmpty(AName_.show_checkbox) == string.Empty) {
                    return TextConst.AVBool.True;
                } else {
                    return string.Empty;
                }
            }
            set {
                string s;
                if (value == string.Empty) {
                    s = TextConst.AVBool.False;
                } else {
                    s = null;
                }
                this.SetAttributeValue(AName_.show_checkbox, s);
            }
        }
        public override string P_ShowCheckbox_Title()
        {
            return "Показывать checkbox";
        }
        public override bool P_ShowCheckbox_Exists()
        {
            return true;
        }
        #endregion
        //#region SelfDataType
        //public override bool P_DataType_Exists()
        //{
        //    return (P_ControlType == TextConst.AVControlType.Custom);
        //}
        //#endregion
        #region CustomControl
        public override bool P_CustomControl_Exists()
        {
            return (this.P_ControlType == TextConst.AVControlType.Custom);
        }
        #endregion
        #region ShowNulls
        public override bool P_ShowNulls_Exists()
        {
            return true;// (P_ControlType == TextConst.AVControlType.List) || (P_ControlType == TextConst.AVControlType.Combo);
        }
        #endregion
        #region ExpandAll
        public override bool P_ExpandAll_Exists()
        {
            string control_type = this.P_ControlType;
            return (control_type == TextConst.AVControlType.List) || (control_type == TextConst.AVControlType.Combo);
        }
        #endregion
        #region AutoCheck
        public override bool P_AutoCheck_Exists()
        {
            string control_type = this.P_ControlType;
            return (control_type == TextConst.AVControlType.List) || (control_type == TextConst.AVControlType.Combo);
        }
        #endregion
        #region MaxLength
        public override bool P_MaxLength_Exists()
        {
            string control_type = this.P_ControlType;
            return (control_type == TextConst.AVControlType.Number) || (control_type == TextConst.AVControlType.Text) || (control_type == TextConst.AVControlType.TextEx);
        }
        #endregion
        #region ClearOnListChange
       

        public override bool P_ClearOnListChange_Exists()
        {

            return P_AutoCheck_Exists();

        }
        #endregion
        //#region StoreInDB
        //public override bool P_StoreInDB_Exists()
        //{
        //    return (P_ControlType == TextConst.AVControlType.List);
        //}
        //#endregion
    }
}