using System;
using System.Collections.Generic;
using System.Xml.Linq;
using AName_ = sql.builder.DataApi.AName;

namespace sql.builder.DataApi
{
    internal sealed class VUseField : VSXElement, IVParent
    {
        internal VUseField()
            : base(EName.usefield)
        {
        }
        public override VField Field()
        {
            return XmlReports.Environment.GetField(this.P_Field);
        }
        public override List<VSXElement> GetUsedElements()
        {
            return this.Field().AsList();
        }
        public override bool IsElementUser()
        {
            return true;
        }
        public override string XDataType()
        {
            string dt = this.DataType();
            if (string.IsNullOrEmpty(dt)) {
                dt = this.Field().DataType();
            }
            return dt;
        }
        IList<string> IVParent.AllowedChildNodes()
        {
            return VField.child_nodes;
        }
        #region NodeText
        public override string GetNodeOtherInfo()
        {
            string s = Bold(this.P_Field) + " " + this.P_FormalParNameS + " " + Italic(this.P_Title);
            return s;
        }
        #endregion
        #region FormalParName
        public override string P_FormalParName_Title()
        {
            return "Имя параметра (собственное)";
        }
        public override bool P_FormalParName_Exists()
        {
            return true;
        }
        #endregion
        #region FormalParNameS
        public override string P_FormalParNameS
        {
            get
            {

               
                var s = P_FormalParName;
                if (s == "")
                {
                    var fld = Field();
                    if (fld != null)
                    {
                        s = fld.P_FormalParName;
                    }
                }
                return s;
            }
        }


        public override bool P_FormalParNameS_Exists()
        {

            return true;

        }
        public override bool P_FormalParNameS_VisibleInForm()
        {

            return true;

        }
       
        #endregion
        #region Field
        public override bool P_Field_Exists()
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
        #region ParentFieldName
        public override bool P_ParentFieldName_Exists()
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
        #region Title
        public override string P_Title {
            get {
                string title = this.P_SelfTitle;
                if (string.IsNullOrEmpty(title)) {
                    VField fld = this.Field();
                    if (fld != null) {
                        title = fld.P_SelfTitle;
                    }
                }
                return title;
            }
        }
        public override bool P_Title_Exists()
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
        #region ColumnVisible
        public override bool P_ColumnVisible_Exists()
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
        #region ControlType
        public override string P_ControlType
        {
            get { 
                VField fld = this.Field();
                if (fld != null) {
                    return fld.P_ControlType;
                } else {
                    return string.Empty;
                }
            }
        }
        public override bool P_ControlType_Exists()
        {

            return true;

        }
        public override bool P_ControlType_Editable()
        {

            return false;

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
        #region Default
        public override bool P_Default_Exists()
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
                if (string.IsNullOrEmpty(this.AttrOrEmpty(AName_.show_checkbox))) {
                    return TextConst.AVBool.True;
                }
                else {
                    return string.Empty;
                }
            }
            set {
                if (string.IsNullOrEmpty(value)) {
                    value = TextConst.AVBool.False;
                } else {
                    value = null;
                }
                this.SetAttributeValue(AName_.show_checkbox, value);
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
        #region DataType
        public override bool P_DataType_Exists()
        {
            return true;
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
		#region Hint
		public override bool P_Hint_Exists()
		{
			return true;
		}
		#endregion
    }
}