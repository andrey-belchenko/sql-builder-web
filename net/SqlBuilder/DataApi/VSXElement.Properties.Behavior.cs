using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.IO;
//using System.Windows.Forms;
using Devart.Data.Oracle;
using sql.builder.FieldInfo;
using System.Reflection;
using sql.builder.UI;
using System.Diagnostics;
using AName_ = sql.builder.DataApi.AName;

namespace sql.builder.DataApi
{
    public partial class VSXElement : VXElement
    {
        private VSXElement BehaviorPropSource(string property_name, bool useColumnOptionsForLink)
        {
            // св-во P_XXXXX
            string propName = PropPfx + property_name;
            string value = (string)VFieldInfo.GetValue(this, propName);
            if (!string.IsNullOrEmpty(value)) {
                return this;
            }
            // св-во P_ColumnXXXXX
            string colPropName = PropPfx + TextConst.Pfx.BehaviorPropCol + property_name;
            value = (string)VFieldInfo.GetValue(this, colPropName);
            if (!string.IsNullOrEmpty(value)) {
                return this;
            }
            // св-во P_XXXXXInvert
            string invPropName = PropPfx + property_name + TextConst.Pfx.BehaviorPropInv;
            value = (string)VFieldInfo.GetValue(this, invPropName);
            if (!string.IsNullOrEmpty(value)) {
                return this;
            }
            //
            if (this.RootQuery() is VForm) {
                VSXElement src = this.SourceColumns().FirstOrDefault();
                if (src == null) {
                    return null;
                }
                if (useColumnOptionsForLink || !((this as VColumn).Source() is VLink)) {
                    value = (string)VFieldInfo.GetValue(src, propName);
                    if (!string.IsNullOrEmpty(value)) {
                        return src;
                    }
                    value = (string)VFieldInfo.GetValue(src, colPropName);
                    if (!string.IsNullOrEmpty(value)) {
                        return src;
                    }
                    value = (string)VFieldInfo.GetValue(src, invPropName);
                    if (!string.IsNullOrEmpty(value)) {
                        return src;
                    } else {
                        return null;
                    }
                } else {
                    return null;
                }
            } else {
                return null;
            }
        }
        private string BehaviorResult(string property_name)
        {
            string propNameRes = PropPfx + property_name + TextConst.Pfx.BehaviorPropRes; // P_XXXXXResult
            VSXElement src = VFieldInfo.Source(this, propNameRes);
            if (src == null) {
                return string.Empty;
            }
            string result;
            string colPropName = PropPfx + TextConst.Pfx.BehaviorPropCol + property_name; // P_ColumnXXXXX
            string invPropName = PropPfx + property_name + TextConst.Pfx.BehaviorPropInv; // P_XXXXXInvert
            string value = (string)VFieldInfo.GetValue(src, colPropName);
            if (!string.IsNullOrEmpty(value)) {
                result = TextConst.EName.Query + ":" + value;
            } else {
                string propName = PropPfx + property_name;
                value = (string)VFieldInfo.GetValue(src, propName);
                if (string.IsNullOrEmpty(value)) {
                    result = string.Empty;
                } else {
                    if (src.RootQuery() is VForm) {
                        result = TextConst.EName.Param + ":";
                    } else {
                        result = TextConst.EName.Column + ":";
                    }
                    result = result + value;
                }
            }
            value = (string)VFieldInfo.GetValue(src, invPropName);
            if (value == TextConst.AVBool.True) {
                result = "!" + result;
            }
            return result;
        }
        #region Visible
        public virtual string P_Visible {
            get {
                return this.AttrOrEmpty(AName_.visible);
            }
            set {
                this.SetAttributeValue(AName_.visible, value);
            }
        }
        public virtual XElement P_Visible_UsedEl()
        {
            if (string.IsNullOrEmpty(this.P_Visible)) {
                return null;
            }
            VSourcedElement rootQuery = this.RootQuery();
            VSXElement col;
            if (rootQuery is VQuery) {
                col = rootQuery.SearchColumn(this.P_Visible);
            } else {
                col = (rootQuery as VForm).SearchVariableSource(this.P_Visible);
            }
            return col;
        }
        public virtual string P_Visible_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.BehaviorVisibile;
        }
        public virtual string P_Visible_Title()
        {
            return "Поле видимое если";
        }
        public virtual string P_Visible_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual void P_Visible_List(VDataTable table)
        {
            this.P_Editable_List(table);
        }
        public virtual void P_Visible_ListRefresh(VDataTable table)
        {
            this.P_Editable_ListRefresh(table);
        }
        public virtual bool P_Visible_Exists()
        {
            return this.P_Editable_Exists();
        }
        #endregion
        #region ColumnVisible
        public virtual string P_ColumnVisible {
            get {
                return this.AttrOrEmpty(AName_.column_visible);
            }
            set {
                this.SetAttributeValue(AName_.column_visible, value);
            }
        }
        public virtual string P_ColumnVisible_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.BehaviorVisibile;
        }
        public virtual string P_ColumnVisible_Title()
        {
            return "Колонка видимая если";
        }
        public virtual string P_ColumnVisible_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual void P_ColumnVisible_List(VDataTable table)
        {
            this.P_ColumnEditable_List(table);
        }
        public virtual void P_ColumnVisible_ListRefresh(VDataTable table)
        {
            this.P_ColumnEditable_ListRefresh(table);
        }
        public virtual bool P_ColumnVisible_Exists()
        {
            return false;
        }
        #endregion
        #region VisibleInvert
        public virtual string P_VisibleInvert
        {
            get
            {
                return GetAttrValue(TextConst.AName.VisibleInvert);
            }
            set
            {
                SetAttributeNotEmpty(TextConst.AName.VisibleInvert, value);

            }

        }
        public virtual string P_VisibleInvert_FieldGroup()
        {

            return TextConst.SchEdirorFieldGr.BehaviorVisibile;

        }

        public virtual string P_VisibleInvert_ControlType()
        {

            return typeof(UICheck).Name;

        }

        public virtual string P_VisibleInvert_Title()
        {

            return "Инвертировать видимость";

        }

        public virtual bool P_VisibleInvert_Exists()
        {

            return P_Visible_Exists();

        }
        #endregion
        #region VisibleResult
        public virtual string P_VisibleResult {
            get {
                return this.BehaviorResult("Visible");
            }
        }
        public virtual string P_VisibleResult_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.BehaviorVisibile;
        }
        public virtual string P_VisibleResult_Title()
        {
            return "Видимость*";
        }
        public virtual VSXElement P_VisibleResult_Source()
        {
           return this.BehaviorPropSource("Visible", true);
        }
        public virtual VSXElement P_VisibleResult_UsedEl()
        {
            return null;
        }
        public virtual string P_VisibleResult_ControlType()
        {
            return typeof(UIText).Name;
        }
        public virtual bool P_VisibleResult_Exists()
        {
            return P_Visible_Exists();
        }
        public virtual bool P_VisibleResult_Editable()
        {
            return false;
        }
        #endregion
        #region Editable
        public virtual string P_Editable {
            get {
                return this.AttrOrEmpty(AName_.editable);
            }
            set {
                this.SetAttributeValue(AName_.editable, value);
            }
        }
        public virtual string P_Editable_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.BehaviorEditable;
        }
        public virtual string P_Editable_Title()
        {
            return "Поле редактируемое если";
        }
        public virtual string P_Editable_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual void P_Editable_List(VDataTable table)
        {
            table.AddColumn("id");
            table.AddColumn("name", "Имя");
            table.AddColumn("title", "Заголовок");
        }
        protected void ObjectFieldPropertyListRefresh(VDataTable table, bool addBool = false, bool addNull = false)
        {
            table.Rows.Clear();
            VSourcedElement rootQuery = RootQuery();
            if (rootQuery is VForm || VSXElement.Get(getPartParent(this.GetParent())).GetAncestorsAndSelf(EName.content).Count != 0) {
                if (addBool) {
                    table.Rows.Add("1", "true");
                    table.Rows.Add("0", "false");
                }
                if (addNull) {
                    table.Rows.Add("null", "null");
                }
                ObjectFieldPropertyListRefreshForForm(table);
            }
            else if (rootQuery is VQuery) {
                ObjectFieldPropertyListRefreshForQuery(table);
            }
        }
        protected void ObjectFieldPropertyListRefreshForForm(VDataTable table)
        {

           
            var rootQuery = RootQuery();
            VForm rootForm = RootQuery() as VForm;
            if (rootForm !=null)
            {
               
                foreach (VSXElement col in rootForm.VariableColumns())
                {
                    table.Rows.Add(col.P_ParName, col.P_ParName, col.P_Title);
                }

                foreach (VParam col in rootForm.Params())
                {
                    table.Rows.Add(col.P_FormalParName, col.P_FormalParName, col.P_Title);
                }

                foreach (string p in TextConst.AVParamArray.FormExtPars)
                {
                    table.Rows.Add(p, p);
                }
            }

            foreach (VSXElement el in rootQuery.ParamFields())
            {

                table.Rows.Add(el.P_FormalParNameS, el.P_FormalParNameS);
            }



            if (rootForm != null)
            {
                foreach (string p in TextConst.AVParamArray.TableExtPars)
                {
                    foreach (VQueryCall qry in rootForm.MainAndRelatedQueries())
                    {
                        var name = qry.XName + p;
                        table.Rows.Add(name, name);
                    }

                }
            }
        }


        protected void ObjectFieldPropertyListRefreshForQuery(VDataTable table)
        {
            List<string> names = new List<string>();
            VSourcedElement rootQuery = RootQuery();
            foreach (VSXElement el in rootQuery.Columns()) {
                if (!names.Contains(el.XName)) {
                    table.Rows.Add(el.XName, el.XName, el.P_Title);
                    names.Add(el.XName);
                }
            }
            foreach (VSXElement exp in XmlReports.Environment.GetElements(TextConst.EName.Queries).SelectMany(e => (e as VQuery).FactColumns()).Distinct()) {
                string name = TextConst.Pfx.QubeQueryAlias + "." + exp.P_Fact;
                table.Rows.Add(name, name, exp.P_Title);

            }
            foreach (VExpression exp in XmlReports.Environment.GetElements(TextConst.EName.ExpressionPackages).SelectMany(VSXElement.GetElementsP)) {
                string name = TextConst.Pfx.QubeQueryAlias + "." + exp.XName;
                table.Rows.Add(name, name, exp.P_Title);
            }
        }
        public virtual void P_Editable_ListRefresh(VDataTable table)
        {
            ObjectFieldPropertyListRefresh(table,true);
        }

        public virtual bool P_Editable_Exists()
        {
            return false;
        }
        #endregion
        #region ColumnEditable
        public virtual string P_ColumnEditable {
            get {
                return this.AttrOrEmpty(AName_.column_editable);
            }
            set {
                this.SetAttributeValue(AName_.column_editable, value);
            }
        }
        public virtual string P_ColumnEditable_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.BehaviorEditable;
        }
        public virtual string P_ColumnEditable_Title()
        {
            return "Колонка редактируемая если";
        }
        public virtual string P_ColumnEditable_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual void P_ColumnEditable_List(VDataTable table)
        {
            table.AddColumn("id");
            table.AddColumn("name", "Имя");
            table.AddColumn("title", "Заголовок");
        }
        public virtual void P_ColumnEditable_ListRefresh(VDataTable table)
        {
            VSXElement.FillDataTableFromRealQueries(table);
            table.AddRow(TextConst.AVBool.True, "true");
            table.AddRow(TextConst.AVBool.False, "false");
        }
        public virtual bool P_ColumnEditable_Exists()
        {
            return false;
        }
        #endregion
        #region EditableInvert
        public virtual string P_EditableInvert
        {
            get
            {
                return GetAttrValue(TextConst.AName.EditableInvert);
            }
            set
            {
                SetAttributeNotEmpty(TextConst.AName.EditableInvert, value);

            }

        }
        public virtual string P_EditableInvert_FieldGroup()
        {

            return TextConst.SchEdirorFieldGr.BehaviorEditable;

        }

        public virtual string P_EditableInvert_ControlType()
        {

            return typeof(UICheck).Name;

        }

        public virtual string P_EditableInvert_Title()
        {

            return "Инвертировать активность";

        }

        public virtual bool P_EditableInvert_Exists()
        {

            return P_Editable_Exists();

        }
        #endregion
        #region EditableResult
        public virtual string P_EditableResult {
            get {
                return this.BehaviorResult("Editable");
            }
        }
        public virtual string P_EditableResult_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.BehaviorEditable;
        }
        public virtual string P_EditableResult_Title()
        {
            return "Активность*";
        }
        public virtual VSXElement P_EditableResult_Source()
        {
            return this.BehaviorPropSource("Editable", false);
        }
        public virtual VSXElement P_EditableResult_UsedEl()
        {
            return null;
        }
        public virtual string P_EditableResult_ControlType()
        {
            return typeof(UIText).Name;
        }
        public virtual bool P_EditableResult_Exists()
        {
            return P_ColumnEditable_Exists();
        }
        public virtual bool P_EditableResult_Editable()
        {
            return false;
        }
        #endregion
        #region Exists
        public virtual string P_Exists
        {
            get
            {
                return GetAttrValue(TextConst.AName.Exists);
            }
            set
            {
                SetAttribute(TextConst.AName.Exists, value);
            }
        }

        public virtual string P_Exists_FieldGroup()
        {

            return TextConst.SchEdirorFieldGr.BehaviorExists;

        }
        public virtual string P_Exists_Title()
        {

            return "Поле существует если";

        }

        public virtual string P_Exists_ControlType()
        {

            return typeof(UICombo).Name;

        }

        public virtual void P_Exists_List(VDataTable table)
        {

            P_Editable_List(table);
        }


        public virtual void P_Exists_ListRefresh(VDataTable table)
        {
            P_Editable_ListRefresh(table);
        }
        public virtual bool P_Exists_Exists()
        {

            return P_Visible_Exists();

        }
        #endregion
        #region ColumnExists
        public virtual string P_ColumnExists {
            get {
                return GetAttrValue(TextConst.AName.ColumnExists);
            }
            set {
                SetAttribute(TextConst.AName.ColumnExists, value);
            }
        }
        public virtual string P_ColumnExists_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.BehaviorExists;
        }
        public virtual string P_ColumnExists_Title()
        {
            return "Колонка существует если";
        }
        public virtual string P_ColumnExists_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual void P_ColumnExists_List(VDataTable table)
        {
            this.P_ColumnEditable_List(table);
        }
        public virtual void P_ColumnExists_ListRefresh(VDataTable table)
        {
            this.P_ColumnEditable_ListRefresh(table);
        }
        public virtual bool P_ColumnExists_Exists()
        {
            return P_Exists_Exists();
        }
        #endregion
        #region ExistsInvert
        public virtual string P_ExistsInvert
        {
            get
            {
                return GetAttrValue(TextConst.AName.ExistsInvert);
            }
            set
            {
                SetAttributeNotEmpty(TextConst.AName.ExistsInvert, value);

            }

        }
        public virtual string P_ExistsInvert_FieldGroup()
        {

            return TextConst.SchEdirorFieldGr.BehaviorExists;

        }

        public virtual string P_ExistsInvert_ControlType()
        {

            return typeof(UICheck).Name;

        }

        public virtual string P_ExistsInvert_Title()
        {

            return "Инвертировать доступ";

        }

        public virtual bool P_ExistsInvert_Exists()
        {

            return P_Exists_Exists();

        }
        #endregion
        #region ExistsResult
        public virtual string P_ExistsResult {
            get {
                return this.BehaviorResult("Exists");
            }
        }
        public virtual string P_ExistsResult_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.BehaviorExists;
        }
        public virtual string P_ExistsResult_Title()
        {
            return "Доступ*";
        }
        public virtual VSXElement P_ExistsResult_Source()
        {
            return this.BehaviorPropSource("Exists", true);
        }
        public virtual VSXElement P_ExistsResult_UsedEl()
        {
            return null;
        }
        public virtual string P_ExistsResult_ControlType()
        {
            return typeof(UIText).Name;
        }
        public virtual bool P_ExistsResult_Exists()
        {
            return P_Exists_Exists();
        }
        public virtual bool P_ExistsResult_Editable()
        {
            return false;
        }
        #endregion
        #region Mandatory
        public virtual string P_Mandatory
        {
            get
            {
                return GetAttrValue(TextConst.AName.Mandatory);
            }
            set
            {
                SetAttribute(TextConst.AName.Mandatory, value);
            }
        }

        public virtual string P_Mandatory_FieldGroup()
        {

            return TextConst.SchEdirorFieldGr.BehaviorRequired;

        }
        public virtual string P_Mandatory_Title()
        {

            return "Поле обязательное если";

        }

        public virtual string P_Mandatory_ControlType()
        {

            return typeof(UICombo).Name;

        }

        public virtual void P_Mandatory_List(VDataTable table)
        {

            P_Editable_List(table);
        }


        public virtual void P_Mandatory_ListRefresh(VDataTable table)
        {
            P_Editable_ListRefresh(table);
        }
        public virtual bool P_Mandatory_Exists()
        {

            return P_Editable_Exists();

        }
        #endregion
        #region ColumnMandatory
        public virtual string P_ColumnMandatory
        {
            get
            {
                return GetAttrValue(TextConst.AName.ColumnMandatory);
            }
            set
            {
                SetAttribute(TextConst.AName.ColumnMandatory, value);
            }
        }
        public virtual string P_ColumnMandatory_FieldGroup()
        {

            return TextConst.SchEdirorFieldGr.BehaviorRequired;

        }

        public virtual string P_ColumnMandatory_Title()
        {

            return "Колонка обязательная если";

        }

        public virtual string P_ColumnMandatory_ControlType()
        {

            return typeof(UICombo).Name;

        }

        public virtual void P_ColumnMandatory_List(VDataTable table)
        {

            P_ColumnEditable_List(table);
        }


        public virtual void P_ColumnMandatory_ListRefresh(VDataTable table)
        {
            P_ColumnEditable_ListRefresh(table);

        }

        public virtual bool P_ColumnMandatory_Exists()
        {

            return false;

        }
        #endregion
        #region MandatoryInvert
        public virtual string P_MandatoryInvert
        {
            get
            {
                return GetAttrValue(TextConst.AName.MandatoryInvert);
            }
            set
            {
                SetAttributeNotEmpty(TextConst.AName.MandatoryInvert, value);

            }

        }
        public virtual string P_MandatoryInvert_FieldGroup()
        {

            return TextConst.SchEdirorFieldGr.BehaviorRequired;

        }

        public virtual string P_MandatoryInvert_ControlType()
        {

            return typeof(UICheck).Name;

        }

        public virtual string P_MandatoryInvert_Title()
        {

            return "Инвертировать обязательность";

        }

        public virtual bool P_MandatoryInvert_Exists()
        {

            return P_Mandatory_Exists();

        }
        #endregion
        #region MandatoryResult
        public virtual string P_MandatoryResult {
            get {
                return this.BehaviorResult("Mandatory");
            }
        }
        public virtual string P_MandatoryResult_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.BehaviorRequired;
        }
        public virtual string P_MandatoryResult_Title()
        {
            return "Обязательность*";
        }
        public virtual VSXElement P_MandatoryResult_Source()
        {
            return BehaviorPropSource("Mandatory", false);
        }
        public virtual VSXElement P_MandatoryResult_UsedEl()
        {
            return null;
        }
        public virtual string P_MandatoryResult_ControlType()
        {
            return typeof(UIText).Name;
        }
        public virtual bool P_MandatoryResult_Exists()
        {
            return P_Mandatory_Exists();
        }
        public virtual bool P_MandatoryResult_Editable()
        {
            return false;
        }
        #endregion
        #region Default
        public virtual string P_Default
        {
            get
            {
                return GetAttrValue(TextConst.AName.Default);
            }
            set
            {
                SetAttribute(TextConst.AName.Default, value);
            }
        }
        public virtual string P_Default_FieldGroup()
        {

            return TextConst.SchEdirorFieldGr.BehaviorDefault;

        }

        public virtual string P_Default_Title()
        {

            return "Значение по умолчанию";

        }

        public virtual string P_Default_ControlType()
        {

            return typeof(UICombo).Name;

        }

        public virtual void P_Default_List(VDataTable table)
        {

            P_Editable_List(table);
        }


        public virtual void P_Default_ListRefresh(VDataTable table)
        {
            ObjectFieldPropertyListRefresh(table);

        }
        public virtual bool P_Default_Exists()
        {

            return false;

        }
        #endregion
        #region ColumnDefault
        public virtual string P_ColumnDefault
        {
            get
            {
                return GetAttrValue(TextConst.AName.ColumnDefault);
            }
            set
            {
                SetAttribute(TextConst.AName.ColumnDefault, value);
            }
        }
        public virtual string P_ColumnDefault_FieldGroup()
        {

            return TextConst.SchEdirorFieldGr.BehaviorDefault;

        }

        public virtual string P_ColumnDefault_Title()
        {

            return "Значение по умолчанию (для колонки)";

        }

        public virtual string P_ColumnDefault_ControlType()
        {

            return typeof(UICombo).Name;

        }

        public virtual void P_ColumnDefault_List(VDataTable table)
        {

            P_ColumnEditable_List(table);
        }


        public virtual void P_ColumnDefault_ListRefresh(VDataTable table)
        {
            P_ColumnEditable_ListRefresh(table);

        }

        public virtual bool P_ColumnDefault_Exists()
        {

            return P_ColumnMandatory_Exists();

        }
        #endregion
        #region DefaultResult
        public virtual string P_DefaultResult {
            get {
                return this.BehaviorResult("Default");
            }
        }
        public virtual string P_DefaultResult_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.BehaviorDefault;
        }
        public virtual string P_DefaultResult_Title()
        {
            return "Значение по умолчанию*";
        }
        public virtual VSXElement P_DefaultResult_Source()
        {
            return this.BehaviorPropSource("Default", false);
        }
        public virtual VSXElement P_DefaultResult_UsedEl()
        {
            return null;
        }
        public virtual string P_DefaultResult_ControlType()
        {
            return typeof(UIText).Name;
        }
        public virtual bool P_DefaultResult_Exists()
        {
            return P_Default_Exists();
        }
        public virtual bool P_DefaultResult_Editable()
        {
            return false;
        }
        #endregion
        #region NewVal
        public virtual string P_NewVal
        {
            get
            {
                return GetAttrValue(TextConst.AName.NewVal);
            }
            set
            {
                SetAttribute(TextConst.AName.NewVal, value);
            }
        }

        public virtual string P_NewVal_FieldGroup()
        {

            return TextConst.SchEdirorFieldGr.BehaviorNewVal;

        }
        public virtual string P_NewVal_Title()
        {

            return "Новое значение";

        }

        public virtual string P_NewVal_ControlType()
        {

            return typeof(UICombo).Name;

        }

        public virtual void P_NewVal_List(VDataTable table)
        {


            P_Editable_List(table);
        }


        public virtual void P_NewVal_ListRefresh(VDataTable table)
        {
            ObjectFieldPropertyListRefresh(table);

        }
        public virtual bool P_NewVal_Exists()
        {

            return false;

        }
        #endregion
        #region NewValResult
        public virtual string P_NewValResult {
            get {
                return this.BehaviorResult("NewVal");
            }
        }
        public virtual string P_NewValResult_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.BehaviorNewVal;
        }
        public virtual string P_NewValResult_Title()
        {
            return "Новое значение*";
        }
        public virtual VSXElement P_NewValResult_Source()
        {
            return this.BehaviorPropSource("NewVal", false);
        }
        public virtual VSXElement P_NewValResult_UsedEl()
        {
            return null;
        }
        public virtual string P_NewValResult_ControlType()
        {
            return typeof(UIText).Name;
        }
        public virtual bool P_NewValResult_Exists()
        {
            return P_NewVal_Exists();
        }
        public virtual bool P_NewValResult_Editable()
        {
            return false;
        }
        #endregion
        #region Valid
        public virtual string P_Valid {
            get {
                return this.AttrOrEmpty(AName_.valid);
            }
            set {
                this.SetAttributeValue(AName_.valid, value);
            }
        }
        public virtual string P_Valid_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.BehaviorValid;
        }
        public virtual string P_Valid_Title()
        {
            return "Валидация";
        }
        public virtual string P_Valid_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual void P_Valid_List(VDataTable table)
        {
            this.P_Editable_List(table);
        }
        public virtual void P_Valid_ListRefresh(VDataTable table)
        {
            ObjectFieldPropertyListRefresh(table,false,true);
        }
        public virtual bool P_Valid_Exists()
        {
            return this.P_Visible_Exists();
        }
        #endregion
        #region ValidResult
        public virtual string P_ValidResult {
            get {
                return this.BehaviorResult("Valid");
            }
        }
        public virtual string P_ValidResult_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.BehaviorValid;
        }
        public virtual string P_ValidResult_Title()
        {
            return "Валидация*";
        }
        public virtual VSXElement P_ValidResult_Source()
        {
            return this.BehaviorPropSource("Valid", false);
        }
        public virtual VSXElement P_ValidResult_UsedEl()
        {
            return null;
        }
        public virtual string P_ValidResult_ControlType()
        {
            return typeof(UIText).Name;
        }
        public virtual bool P_ValidResult_Exists()
        {
            return P_Valid_Exists();
        }
        public virtual bool P_ValidResult_Editable()
        {
            return false;
        }
        #endregion
        #region TextSource
        public virtual string P_TextSource {
            get {
                return this.AttrOrEmpty(AName_.textsource);
            }
            set {
                this.SetAttributeValue(AName_.textsource, value);
            }
        }
        public virtual string P_TextSource_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.BehaviorTextSource;
        }
        public virtual string P_TextSource_Title()
        {
            return "Источник текста";
        }
        public virtual string P_TextSource_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual void P_TextSource_List(VDataTable table)
        {
            this.P_Editable_List(table);
        }
        public virtual void P_TextSource_ListRefresh(VDataTable table)
        {
            ObjectFieldPropertyListRefresh(table);
        }
        public virtual bool P_TextSource_Exists()
        {
            return this.P_Visible_Exists();
        }
        #endregion
        #region TextSourceResult
        public virtual string P_TextSourceResult {
            get {
                return this.BehaviorResult("TextSource");
            }
        }
        public virtual string P_TextSourceResult_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.BehaviorTextSource;
        }
        public virtual string P_TextSourceResult_Title()
        {
            return "Источник текста*";
        }
        public virtual VSXElement P_TextSourceResult_Source()
        {
            return this.BehaviorPropSource("TextSource", false);
        }
        public virtual VSXElement P_TextSourceResult_UsedEl()
        {
            return null;
        }
        public virtual string P_TextSourceResult_ControlType()
        {
            return typeof(UIText).Name;
        }
        public virtual bool P_TextSourceResult_Exists()
        {
            return P_TextSource_Exists();
        }
        public virtual bool P_TextSourceResult_Editable()
        {
            return false;
        }
        #endregion
        #region BackColor
        public virtual string P_BackColor {
            get {
                return this.AttrOrEmpty(AName_.color);
            }
            set {
                this.SetAttributeValue(AName_.color, value);
            }
        }
        public virtual string P_BackColor_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.BehaviorColor;
        }
        public virtual string P_BackColor_Title()
        {
            return "Цвет фона";
        }
        public virtual string P_BackColor_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual void P_BackColor_List(VDataTable table)
        {
            this.P_Editable_List(table); 
        }
        public virtual void P_BackColor_ListRefresh(VDataTable table)
        {
            ObjectFieldPropertyListRefresh(table);
        }
        public virtual bool P_BackColor_Exists()
        {
            return this.P_Valid_Exists();
        }
        #endregion
        #region BackColorResult
        public virtual string P_BackColorResult {
            get {
                return this.BehaviorResult("BackColor");
            }
        }
        public virtual string P_BackColorResult_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.BehaviorColor;
        }
        public virtual string P_BackColorResult_Title()
        {
            return "Цвет фона*";
        }
        public virtual VSXElement P_BackColorResult_Source()
        {
            return this.BehaviorPropSource("BackColor", true);
        }
        public virtual VSXElement P_BackColorResult_UsedEl()
        {
            return null;
        }
        public virtual string P_BackColorResult_ControlType()
        {
            return typeof(UIText).Name;
        }
        public virtual bool P_BackColorResult_Exists()
        {
            return P_BackColor_Exists();
        }
        public virtual bool P_BackColorResult_Editable()
        {
            return false;
        }
        #endregion
        #region FontColor
        public virtual string P_FontColor {
            get {
                return this.AttrOrEmpty(AName_.font_color);
            }
            set {
                this.SetAttributeValue(AName_.font_color, value);
            }
        }
        public virtual string P_FontColor_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.BehaviorColor;
        }
        public virtual string P_FontColor_Title()
        {
            return "Цвет текста";
        }
        public virtual string P_FontColor_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual void P_FontColor_List(VDataTable table)
        {
            this.P_Editable_List(table);
        }
        public virtual void P_FontColor_ListRefresh(VDataTable table)
        {
            ObjectFieldPropertyListRefresh(table);
        }
        public virtual bool P_FontColor_Exists()
        {
            return this.P_Valid_Exists();
        }
        #endregion
        #region FontColorResult
        public virtual string P_FontColorResult {
            get {
                return this.BehaviorResult("FontColor");
            }
        }
        public virtual string P_FontColorResult_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.BehaviorColor;
        }
        public virtual string P_FontColorResult_Title()
        {
            return "Цвет текста*";
        }
        public virtual VSXElement P_FontColorResult_Source()
        {
            return this.BehaviorPropSource("FontColor", true);
        }
        public virtual VSXElement P_FontColorResult_UsedEl()
        {
            return null;
        }
        public virtual string P_FontColorResult_ControlType()
        {
            return typeof(UIText).Name;
        }
        public virtual bool P_FontColorResult_Exists()
        {
            return P_FontColor_Exists();
        }
        public virtual bool P_FontColorResult_Editable()
        {
            return false;
        }
        #endregion
        #region CanBeChecked
        public virtual string P_CanBeChecked
        {
            get
            {
                return GetAttrValue(TextConst.AName.CanBeChecked);
            }
            set
            {
                SetAttribute(TextConst.AName.CanBeChecked, value);
            }
        }
        public virtual string P_CanBeChecked_FieldGroup()
        {

            return TextConst.SchEdirorFieldGr.BehaviorSelect;

        }

        public virtual string P_CanBeChecked_Title()
        {

            return "Строка НЕ может быть выбрана если";

        }

        public virtual string P_CanBeChecked_ControlType()
        {

            return typeof(UICombo).Name;

        }

        public virtual void P_CanBeChecked_List(VDataTable table)
        {

            P_Editable_List(table);

        }




        public virtual void P_CanBeChecked_ListRefresh(VDataTable table)
        {
            ObjectFieldPropertyListRefresh(table);
        }
        public virtual bool P_CanBeChecked_Exists()
        {

            return false;

        }
        #endregion
    }
}