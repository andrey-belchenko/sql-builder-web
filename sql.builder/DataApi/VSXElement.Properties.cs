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
using System.Diagnostics;
using System.Reflection;
using Devart.Data.Oracle;
//using DevExpress.XtraEditors.Controls;
//using infoenergo.core.Extensions;
using sql.builder.FieldInfo;
using sql.builder.UI;
using AName_ = sql.builder.DataApi.AName;

namespace sql.builder.DataApi
{
    internal partial class VSXElement
    {
        #region Default
        public string Default_Title()
        {
            return string.Empty;
        }
        public virtual string Default_ControlType()
        {
            return typeof(UIText).Name;
        }
        public virtual string Default_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.MainOther;
        }
        public virtual bool Default_Exists()
        {
            return true;
        }
        public virtual bool Default_VisibleInTable()
        {
            return false;
        }
        public virtual bool Default_VisibleInForm()
        {
            return true;
        }
        public virtual int Default_Order()
        {
            return 100000;
        }
        public virtual bool Default_IsHtml()
        {
            return false;
        }
        public virtual bool Default_Editable()
        {
            return true;
        }
        #endregion
        /// <summary>
        /// Заполняет две первые колонки в <paramref name="table"/> значениями из массива строк <paramref name="arr"/>.
        /// Используется в процедурах P_XXX_ListRefresh
        /// </summary>
        /// <param name="table"></param>
        /// <param name="arr"></param>
        internal static void FillDataTableFromStringArray(DataTable table, string[] arr)
        {
            table.Rows.Clear();
            for (int row = 0; row < arr.Length; row++) {
                string value = arr[row];
                table.AddRow(value, value);
            }
        }
        /// <summary>
        /// Заполняет две первые колонки в <paramref name="table"/> именем запроса (query@name),
        /// а третью - заглавием (query@title)
        /// </summary>
        /// <param name="table"></param>
        internal static void FillDataTableFromRealQueries(DataTable table)
        {
            table.Rows.Clear();
            HashSet<string> names = new HashSet<string>();
            foreach (XElement el in XmlReports.Environment.Manager.GetNativeScheme().Elements(EName.queries).Elements(EName.query)) {
                if (el.Attribute(AName_.extend) == null) {
                    string name = el.AttrOrEmpty(AName_.name);
                    if (names.Contains(name)) {
                        Debug.WriteLine("Запрос c именем \"" + name + "\" дублируется, используйте XPath //queries/query[@name=\"" + name + "\"] для поиска дублей.");
                    } else {
                        string title = el.AttrOrEmpty(AName_.title);
                        table.AddRow(name, name, title);
                        names.Add(name);
                    }
                }
            }
        }
        #region NodeName
        public virtual int P_NodeName_Order()
        {
            return 100;
        }
        public virtual string P_NodeName {
            get {
                return this.Name.LocalName;
            }
            set {
                this.Name = value;
                this.ChangeType();
            }
        }
        public virtual string P_NodeName_Title()
        {
            return "Имя узла";
        }
        public virtual string P_NodeName_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual void P_NodeName_List(VDataTable table)
        {
            table.AddColumn("id", "Имя");
        }
        public void P_NodeName_ListRefresh(VDataTable table)
        {
            table.Rows.Clear();
            IVParent parent = this.Parent as IVParent;
            if (parent != null) {
                IList<string> node_names = parent.AllowedChildNodes();
                for (int index = 0; index < node_names.Count; index++) {
                    table.AddRow(node_names[index]);
                }
            } else {
                table.AddRow(this.Name.LocalName);
            }
        }
        public virtual bool P_NodeName_Exists()
        {
            return !this.IsMainElement();
        }
        public virtual string P_NodeName_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.MainMain;
        }
        #endregion
        #region NodeText
        public virtual int P_NodeText_Order()
        {
            return 50;
        }
        public virtual string P_NodeText {
            get {
                string s = this.GetNodeFinalInfo();
                if (this.P_Exclude == TextConst.AVBool.True) {
                    s = Strike(s);
                }
                if (this.P_PartId != string.Empty) {
                    s += " " + ColorGreen(P_PartId);
                }
                if (this.P_Comment != string.Empty) {
					s += " " + ColorGold("comments");
				}
                return s;
            }
            set { }
        }
        public virtual string P_NodeText_Title()
        {
            return "Текст";
        }
        private static List<VSXElement> _elementsWithError = null;
        internal static void SetElemntsWithError(List<VSXElement> elements)
        {
            VCashUtils.ClearCashNotErrors();
            var olsEls = _elementsWithError;
            _elementsWithError = elements;
            if (olsEls != null) {
                foreach (var el in olsEls) {
                    el.UpdateDataRow();
                }
            }
            if (_elementsWithError != null) {
                foreach (var el in _elementsWithError) {
                    el.UpdateDataRow();
                }
            }
        }
        private bool HasError()
        {
            if (_elementsWithError == null) {
                return false;
            } else {
                return _elementsWithError.Contains(this);
            }
        }
        private string GetNodeFinalInfo()
        {
            string key;
            if (this.TreeNodeExpanded) {
                key = "2";
            } else {
                key = "1";
            }
            string s;
            if (this.IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), key)) {
                s = (this.GetCashValue(MethodBase.GetCurrentMethod().ToString(), key) as string);
            } else {
                s = this.GetNodeInfo();
                 AddCashValue(s, MethodBase.GetCurrentMethod().ToString(), key);
            }
            if (this.HasError()) {
                s = ColorRed(s);
            }
            return s;
        }
        public virtual string GetNodeInfo()
        {
            return this.GetNodeTypeInfo() + " " + this.GetNodeOtherInfo();
        }
        public virtual string GetNodeTypeInfo()
        {
            return this.P_NodeName;
        }
        public virtual string GetNodeOtherInfo()
        {
            return string.Empty;
        }
        public virtual bool P_NodeText_VisibleInTable()
        {
            return true;
        }
        public virtual bool P_NodeText_VisibleInForm()
        {
            return false;
        }
        public static string Bold(string s)
        {
            return "<b>" + s + "</b>";
        }
        public static string Sup(string s)
        {
            return "<sup>" + s + "</sup>";
        }
        public static string Italic(string s)
        {
            return "<i>" + s + "</i>";
        }
        public static string Strike(string s)
        {
            return "<s>" + s + "</s>";
        }
        public static string ColorRed(string s)
        {
            return "<font color='red'>" + s + "</font>";
        }
        public static string ColorBlue(string s)
        {
            return "<font color='blue'>" + s + "</font>";
        }
        public static string ColorGroup(string s)
        {
            return "<font color='blue'>" + s + "</font>";
        }
        public static string ColorGray(string s)
        {
            return "<font color='gray'>" + s + "</font>";
        }
        public static string ColorBrown(string s)
        {
            return "<font color='brown'>" + s + "</font>";
        }
        public static string ColorGold(string s)
        {
            return "<font color='#cca300'>" + s + "</font>";
        }
        public static string ColorGreen(string s)
        {
            return "<font color='green'>" + s + "</font>";
        }
        #endregion
        #region Function
        public virtual string P_Function {
            get {
                return this.AttrOrEmpty(AName_.function);
            }
            set {
                this.SetAttributeValue(AName_.function, value);
            }
        }
        public virtual string P_Function_Title()
        {
            return "Вызываемая функция";
        }
        public virtual string P_Function_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual bool P_Function_Exists()
        {
            return false;
        }
        public virtual string P_Function_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.MainOther;
        }
        #endregion
        #region Part
        public virtual string P_Part {
            get {
                return this.AttrOrEmpty(AName_.part);
            }
            set {
                this.SetAttributeValue(AName_.part, value);
            }
        }
        public virtual string P_Part_Title()
        {
            return "Используемая часть";
        }
        public virtual string P_Part_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual void P_Part_List(VDataTable table)
        {
            table.AddColumn("id");
            table.AddColumn("name");
        }
        public virtual void P_Part_ListRefresh(VDataTable table)
        {
            table.Rows.Clear();
            HashSet<string> names = new HashSet<string>();
            IList<VSXElement> parts = XmlReports.Environment.GetParts();
            for (int index = 0; index < parts.Count; index++) {
                VSXElement el = parts[index];
                string id = el.PartId();
                if (!names.Contains(id)) {
                    names.Add(id);
                    table.AddRow(id, id);
                }
            }
        }
        public virtual bool P_Part_Exists()
        {
            return false;
        }
        public virtual string P_Part_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.MainMain;
        }
        #endregion
        #region Color (VUseColor)
        public virtual string P_Color {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }
        public virtual string P_Color_Title()
        {
            return "Цвет";
        }
        public virtual string P_Color_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual bool P_Color_Exists()
        {
            return false;
        }
        #endregion
        #region IdName
        private string applyedName = null;
        private string ApplyedName()
        {
            if (this.applyedName == null) {
                this.applyedName = this.P_IdName;
            }
            return applyedName;
        }
        internal bool IsRenamed()
        {
            return this.ApplyedName() != this.P_IdName;
        }
        public event VRenameEventHandler Renamed;
        public bool RaiseRenamed(string newName)
        {
            if (SavedKey == "")
            {
           
                SavedKey = P_IdName;
            }
            if (Renamed != null)
            {
                string oldName = P_IdName;

                return RaiseRenamed(oldName, newName);
            }
            return true;
        }

        public bool RaiseRenamed(string oldName, string newName)
        {
            if (applyedName == null)
            {
                applyedName = P_IdName;
            }
            if (Renamed != null)
            {
                var e = new VRenameEventArgs(this, oldName, newName);
                Renamed(this, e);
                return !e.Cancel;
            }
            return true;
        }
        internal void SetIdName(XName name, string value)
        {
            if (this.RaiseRenamed(value)) {
                this.SetAttributeValue(name, value);
            } else {
                this.UpdateDataRow();
            }
        }
        public virtual string P_IdName
        {
            get {
                if (this.KeyField != null) {
                    return this.AttrOrEmpty(this.KeyField);
                } else {
                    return string.Empty;
                }
            }
            set {
            }
        }
        public virtual string P_IdName_Title()
        {
            return "Наименование";
        }
        public virtual bool P_IdName_Exists()
        {
            return false;
        }
        public virtual string P_IdName_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.MainMain;
        }
        #endregion
        #region CalledQuery
        public virtual string P_CalledQuery {
            get {
                return string.Empty;
                //return GetAttrValue("function");
            }
            set {
                //SetAttribute("function", value);
            }
        }
        public virtual string P_CalledQuery_Title()
        {
            return "Вызываемый запрос/ссылка";
        }
        public virtual string P_CalledQuery_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual string P_CalledQuery_ValueInfo(object value)
        {
            if (value == null) {
                return string.Empty;
            } else {
                return value.ToString();
            }
        }
        public virtual void P_CalledQuery_List(VDataTable table)
        {
            table.AddColumn("id");
            table.AddColumn("name", "Имя");
            table.AddColumn("title", "Заголовок");
            table.AddColumn("query", "Таблица");
        }
        public virtual void P_CalledQuery_ListRefresh(VDataTable table)
        {
            table.Rows.Clear();
        }
        public virtual bool P_CalledQuery_Exists()
        {
            return false;
        }
        public virtual string P_CalledQuery_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.MainOther;
        }
        public virtual VSXElement P_CalledQuery_UsedEl()
        {
            return XmlReports.Environment.GetQuery(this.P_CalledQuery);
        }
        #endregion
        #region Table
        public virtual string P_Table {
            get {
                return this.AttrOrEmpty(AName_.table);
            }
            set {
                this.SetAttributeValue(AName_.table, value);
            }
        }
        public void P_Table_Set(object value)
        {
            SetProperty(MethodBase.GetCurrentMethod().Name, value);
        }
        public virtual string P_Table_Title()
        {
            return "Таблица/Запрос";
        }
        public virtual string P_Table_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual void P_Table_List(VDataTable table)
        {
            table.AddColumn("id");
            table.AddColumn("as", "Псевдоним");
            table.AddColumn("name", "Имя");
            table.AddColumn("title", "Заголовок");
        }
        internal void TableListRowFromElement(VDataTable table, VQueryCall el)
        {
            string name = el.XName;
            table.AddRow(name ?? string.Empty, name, el.SName(), el.XTitle);
        }
        public virtual void P_Table_ListRefresh(VDataTable table)
        {
            table.Rows.Clear();
            var ss = new List<string>();
            var mq = this.ExtendedOrRootQuery();
            var mprt = this.GetMainParent();
            var srcs = mq.AllSources().ToList();
            if (mq != mprt && mprt is VSourcedElement) {
                srcs.AddRange((mprt as VSourcedElement).AllSources());
            }

            foreach (VQueryCall el in srcs) {
                if (!ss.Contains(el.XName)) {
                    ss.Add(el.XName);// При наличии dimset link может повторяться
                    TableListRowFromElement(table, el);
                    if (el.P_IsTree == TextConst.AVBool.True) {
                        foreach (string s in TextConst.TreeSourcesArray.All) {
                            var s1 = el.XName + "-" + s;
                            table.AddRow(s1, s1, "", "");
                        }
                    }
                }
            }
            table.AddRow("*", "*", "", "");
            table.AddRow("this", "this", "", "");
        }
        public virtual bool P_Table_Exists()
        {
            return false;
        }
        public virtual string P_Table_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.MainOther;
        }
        #endregion
        #region Column
        public virtual string P_Column {
            get {
                return this.AttrOrEmpty(AName_.column);
            }
            set {
                this.SetAttributeValue(AName_.column, value);
            }
        }
        public virtual string P_Column_Title()
        {
            return "Колонка";
        }
        public virtual string P_Column_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual void P_Column_List(VDataTable table)
        {
            table.ClearColumns();
            table.AddColumn("id");
            table.AddColumn("name", "Имя");
            table.AddColumn("title", "Заголовок");
            table.AddColumn("etype", "Ссылка");
        }
        public virtual string P_Column_ValueInfo(object value)
        {
            return value.ToString();
        }
        internal static void AddColumnInfoToList(VDataTable table, string name, VSXElement el)
        {
            string etype;
            VColumn col = el as VColumn;
            if (col != null) {
                etype = col.EType;
            } else {
                etype = string.Empty;
            }
            table.AddRow(name, name, el.P_Title, etype);
        }
        public virtual void P_Column_ListRefresh(VDataTable table)
        {
            table.Rows.Clear();
        }
        public virtual bool P_Column_Exists()
        {
            return false;
        }
        public virtual string P_Column_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.MainOther;
        }
        #endregion
        #region Condition (VFact)
        public virtual string P_Condition {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }
        public virtual string P_Condition_Title()
        {
            return "Условие";
        }
        public virtual string P_Condition_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.Cube;
        }
        public virtual string P_Condition_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual bool P_Condition_Exists()
        {
            return false;
        }
        #endregion
        #region Title
        public virtual string P_Title
        {
            get
            {
                //  return XTitle();

                if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null))
                {
                    return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as string);
                }
                var t = MakeMultidefinedPropertyValue(P_Title_Search().SelectAsArray(e => e.P_SelfTitle).ToList());
                AddCashValue(t, MethodBase.GetCurrentMethod().ToString(), null);
                return t;
            }

        }
        public virtual List<VSXElement> P_Title_Search()
        {

            VLookupAnalyzer anl = new VLookupAnalyzer();
            anl.CheckReturn = (VSXElement element) =>
            {
                if (element.P_SelfTitle != "")
                {
                    return true;
                }
                return false;
            };
            anl.CheckStop = (VSXElement element) =>
            {
                return anl.CheckReturn(element) && !IsStringAddision(element.P_SelfTitle);
            };

            return LookUpSources(anl);
        }
        public virtual string P_Title_Title()
        {

            return "Заголовок(вычисляемый)";

        }

        public virtual string P_Title_ControlType()
        {

            return typeof(UIText).Name;

        }

        public virtual bool P_Title_Exists()
        {
            return false;
        }

        public virtual bool P_Title_Editable()
        {
            return false;
        }

        public virtual string P_Title_FieldGroup()
        {
          return  P_SelfTitle_FieldGroup();
            
        }
        #endregion
        #region HAlign
        public virtual string P_HAlign {
            get {
                return this.AttrOrEmpty(AName_.halign);
            }
            set {
                this.SetAttributeValue(AName_.halign, value);
            }
        }
        public virtual string P_HAlign_Title()
        {
            return "Выравнивание по горизонтали";
        }
        public virtual string P_HAlign_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual string P_HAlign_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.Appearance;
        }
        public virtual void P_HAlign_List(VDataTable table)
        {
            table.AddColumn("id");
            table.AddColumn("name");
        }
        public virtual void P_HAlign_ListRefresh(VDataTable table)
        {
            FillDataTableFromStringArray(table, TextConst.AVHAlignArray.All);
        }
        public virtual bool P_HAlign_Exists()
        {
            return false;
        }
        #endregion
        #region ViewMode (VReport и VQuery)
        public virtual string P_ViewMode {
            get {
                return this.AttrOrEmpty(AName_.mode);
            }
            set {
                this.SetAttributeValue(AName_.mode, value);
            }
        }
        public virtual string P_ViewMode_Title()
        {
            return "Режим отображения по умолчанию";
        }
        public virtual string P_ViewMode_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual void P_ViewMode_List(VDataTable table)
        {
            table.AddColumn("id");
            table.AddColumn("name");
        }
        public virtual void P_ViewMode_ListRefresh(VDataTable table)
        {
            FillDataTableFromStringArray(table, TextConst.AVViewModesArray.All);
        }
        public virtual bool P_ViewMode_Exists()
        {
            return false;
        }
        #endregion
        #region Format 
        public virtual string P_FormatS {
            get {
                return this.AttrOrEmpty(AName_.format);
            }
            set {
                this.SetAttributeValue(AName_.format, value);
            }
        }
        public virtual string P_FormatS_Title()
        {
            return "Формат";
        }
        public virtual string P_FormatS_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual void P_FormatS_List(VDataTable table)
        {
            table.AddColumn("id", "Формат");
            table.AddColumn("pkg", "Пакет");
        }
        public virtual void P_FormatS_ListRefresh(VDataTable table)
        {
            table.Rows.Clear();
            foreach (XElement package in XmlReports.Environment.Manager.GetNativeScheme().Elements(EName.format_packages).Elements(EName.format_package)) {
                string package_name = package.AttrOrEmpty(AName_.name);
                foreach (XElement format in package.Elements(EName.format).Where(EPredicate.IsNotExcuded)) {
                    table.AddRow(format.AttrOrEmpty(AName_.name), package_name);
                }
            }
        }
        public virtual bool P_FormatS_Exists()
        {
            return this.P_ControlType_Exists();
        }
        #endregion
        #region SelfTitle
        public virtual string P_SelfTitle {
            get {
                return this.AttrOrEmpty(AName_.title);
            }
            set {
                this.SetAttributeNotEmpty(AName_.title, value);
            }
        }
        public virtual string P_SelfTitle_Title()
        {
            return "Заголовок";
        }
        public virtual string P_SelfTitle_ControlType()
        {
            return typeof(UIText).Name;
        }
        public virtual bool P_SelfTitle_Exists()
        {
            return false;
        }
        public virtual string P_SelfTitle_FieldGroup()
        {
            if (this.GetParent() is VOutputElement || this.IsMainElement()) {
                return TextConst.SchEdirorFieldGr.MainMain;
            } else {
                return TextConst.SchEdirorFieldGr.MainOther;
            }
        }
        #endregion
        #region Stored
        public virtual string P_Stored
        {
            get
            {
                return GetAttrValue(TextConst.AName.Stored);
            }
            set
            {
                SetAttributeNotEmpty(TextConst.AName.Stored, value);

            }

        }

        public virtual string P_Stored_Title()
        {

            return "Имя хранилища";

        }

        public virtual bool P_Stored_Exists()
        {

            return false;

        }
        #endregion
        #region Comment
        public virtual string P_Comment {
            get {
                return this.AttrOrEmpty(AName_.comment);
            }
            set {
                this.SetAttributeNotEmpty(AName_.comment, value);
            }
        }
        public virtual string P_Comment_Title()
        {
            return "Комментарий";
        }
        public virtual string P_Comment_ControlType()
        {
            return typeof(UIText).Name;
        }
        public virtual bool P_Comment_Exists()
        {
            return true;
        }
        #endregion
        #region Interval
        public virtual string P_Interval
        {
            get
            {
                return GetAttrValue(TextConst.AName.Interval);
            }
            set
            {
                SetAttributeNotEmpty(TextConst.AName.Interval, value);

            }

        }

        public virtual string P_Interval_Title()
        {

            return "Интервал";

        }

        public virtual string P_Interval_ControlType()
        {

            return typeof(UIText).Name;

        }

        public virtual bool P_Interval_Exists()
        {

            return false;

        }
        #endregion
        #region If
        public virtual string P_If_Title()
        {
            return "Имя If для повторного использования";
        }
        public virtual string P_If {
            get {
                return this.AttrOrEmpty(AName_.@if);
            }
            set {
                this.SetAttributeNotEmpty(AName_.@if, value);
            }
        }
        public virtual bool P_If_Exists()
        {
            return false;
        }
        #endregion
        #region Dimname
        public virtual string P_Dimname_Title()
        {

            return "Имя Dimname для повторного использования";

        }

        public virtual string P_Dimname
        {
            get
            {
                return GetAttrValue(TextConst.AName.Dimname);
            }
            set
            {
                SetAttributeNotEmpty(TextConst.AName.Dimname, value);

            }

        }

        public virtual bool P_Dimname_Exists()
        {

            return false;

        }
        #endregion
        #region ConstComboValue
        public virtual string P_ConstComboValue {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }
        public virtual string P_ConstComboValue_Title()
        {
            return "Значение(ссылка)";
        }
        public virtual string P_ConstComboValue_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual bool P_ConstComboValue_Exists()
        {
            return false;
        }
        #endregion
        #region ConstListValue
        public virtual string P_ConstListValue {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }
        public virtual string P_ConstListValue_Title()
        {
            return "Значение(список)";
        }
        public virtual string P_ConstListValue_ControlType()
        {
            return typeof(UIList).Name;
        }
        public virtual bool P_ConstListValue_Exists()
        {
            return false;
        }
        #endregion
        #region ConstValue
        public virtual string P_ConstValue {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }
        public virtual string P_ConstValue_Title()
        {
            return "Значение";
        }
        public virtual string P_ConstValue_ControlType()
        {
            return typeof(UIText).Name;
        }
        public virtual bool P_ConstValue_Exists()
        {
            return false;
        }
        #endregion
        #region Alias
        public virtual string P_Alias_Title()
        {
            return "Псевдоним";
        }
        public virtual string P_Alias {
            get {
                return this.AttrOrEmpty(AName_.@as);
            }
            set {
                this.SetAttributeValue(AName_.@as, value);
            }
        }
        public virtual bool P_Alias_Exists()
        {
            return false;
        }
        public virtual string P_Alias_FieldGroup()
        {
            if (this.GetParent() is VOutputElement) {
                return TextConst.SchEdirorFieldGr.MainMain;
            } else {
                return TextConst.SchEdirorFieldGr.MainOther;
            }
        }
        #endregion
        #region PostProcess
        public virtual string P_PostProcess_Title()
        {
            return "Пост обработка";
        }
        public virtual string P_PostProcess {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }
        public virtual string P_PostProcess_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual bool P_PostProcess_Exists()
        {
            return false;
        }
        #endregion
        #region IsListColumn

        public virtual string P_IsListColumn_Title()
        {

            return "Колонка для списка";

        }

        public virtual string P_IsListColumn
        {
            get
            {
                return GetAttrValue(TextConst.AName.IsListColumn);
            }
            set
            {
                SetAttribute(TextConst.AName.IsListColumn, value);
            }
        }
        public virtual string P_IsListColumn_ControlType()
        {

            return typeof(UICheck).Name;

        }

        public virtual bool P_IsListColumn_Exists()
        {

            return false;

        }

        #endregion


        #region AutoFilter

        public virtual string P_AutoFilter
        {
            get
            {
                return GetAttrValue(TextConst.AName.AutoFilter);
            }
            set
            {
                SetAttribute(TextConst.AName.AutoFilter, value);
            }
        }

        public virtual string P_AutoFilter_Title()
        {

            return "Автофильтр";

        }

        
        public virtual string P_AutoFilter_ControlType()
        {

            return typeof(UICheck).Name;

        }

        public virtual bool P_AutoFilter_Exists()
        {

            return false;

        }
        #endregion
		#region AllowSelectMoveColumns
		public virtual string P_AllowSelectMoveColumns {
			get {
                return this.AttrOrEmpty(AName_.allow_select_move_columns);
			}
			set {
				this.SetAttributeValue(AName_.allow_select_move_columns, value);
			}
		}
		public virtual string P_AllowSelectMoveColumns_Title()
		{
			return "Разрешить выбор и перемещение колонок";
		}
		public virtual string P_AllowSelectMoveColumns_ControlType()
		{
			return typeof(UICheck).Name;
		}
		public virtual bool P_AllowSelectMoveColumns_Exists()
		{
			return false;
		}
		#endregion
		#region InvisibleInColumnChooser
		public virtual string P_InvisibleInColumnChooser {
			get {
                return this.AttrOrEmpty(AName_.invisible_in_column_chooser);
			}
			set {
				this.SetAttributeValue(AName_.invisible_in_column_chooser, value);
			}
		}
		public virtual string P_InvisibleInColumnChooser_Title()
		{
			return "Запретить отображение колонки при выборе колонок";
		}
		public virtual string P_InvisibleInColumnChooser_ControlType()
		{
			return typeof(UICheck).Name;
		}
		public virtual bool P_InvisibleInColumnChooser_Exists()
		{
			return false;
		}
		#endregion
        #region AllRows
        public virtual string P_AllRows_Title()
        {
            return "Все строки";
        }
        public virtual string P_AllRows {
            get {
                return this.AttrOrEmpty(AName_.all_rows);
            }
            set {
                this.SetAttributeValue(AName_.all_rows, value);
            }
        }
        public virtual string P_AllRows_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual bool P_AllRows_Exists()
        {
            return false;
        }
        #endregion
        #region OnlyForCond
        public virtual string P_OnlyForCond_Title()
        {

            return "Только для условий";

        }

        public virtual string P_OnlyForCond
        {
            get
            {
                return GetAttrValue(TextConst.AName.OnlyForCond);
            }
            set
            {
                SetAttribute(TextConst.AName.OnlyForCond, value);
            }
        }
        public virtual string P_OnlyForCond_ControlType()
        {

            return typeof(UICheck).Name;

        }

        public virtual bool P_OnlyForCond_Exists()
        {

            return false;

        }

        #endregion
        #region IsNameColumn

        public virtual string P_IsNameColumn_Title()
        {

            return "Колока с именем объекта";

        }

        public virtual string P_IsNameColumn
        {
            get
            {
                return GetAttrValue(TextConst.AName.IsNameColumn);
            }
            set
            {
                SetAttribute(TextConst.AName.IsNameColumn, value);
            }
        }
        public virtual string P_IsNameColumn_ControlType()
        {

            return typeof(UICheck).Name;

        }

        public virtual bool P_IsNameColumn_Exists()
        {

            return false;

        }

        #endregion
        #region Group
        public virtual string P_Group {
            get {
                return this.AttrOrEmpty(AName_.group);
            }
            set {
                this.SetAttributeNotEmpty(AName_.group, value);
            }
        }
        public virtual string P_Group_Title()
        {
            return "Группировка";
        }
        public virtual string P_Group_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual void P_Group_List(VDataTable table)
        {
            table.AddColumn("id");
            table.AddColumn("name");
        }
        public virtual void P_Group_ListRefresh(VDataTable table)
        {
            if (table.Rows.Count == 0) {
                FillDataTableFromStringArray(table, Compiler.aggFuncsNames);
            }
        }
        public virtual bool P_Group_Exists()
        {
            return false;
        }
        public virtual string P_Group_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.MainMain;
        }
        #endregion
        #region Dgroup
        public virtual string P_Dgroup {
            get {
                return this.AttrOrEmpty(AName_.dgroup);
            }
            set {
                this.SetAttributeNotEmpty(AName_.dgroup, value);
            }
        }
        public virtual string P_Dgroup_Title()
        {
            return "Группировка подзапроса";
        }
        public virtual string P_Dgroup_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual void P_Dgroup_List(VDataTable table)
        {
            table.AddColumn("id");
            table.AddColumn("name");
        }
        public virtual void P_Dgroup_ListRefresh(VDataTable table)
        {
            if (table.Rows.Count == 0) {
                FillDataTableFromStringArray(table, Compiler.grFuncsNames);
            }
        }
        public virtual bool P_Dgroup_Exists()
        {
            return false;
        }
        #endregion
        #region ConstrDelOption (VQueryCall)
        public virtual string P_ConstrDelOption {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }
        public virtual string P_ConstrDelOption_Title()
        {
            return "on delete";
        }
        public virtual string P_ConstrDelOption_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual bool P_ConstrDelOption_Exists()
        {
            return false;
        }
        #endregion
        #region Join (VQueryCall)
        public virtual string P_Join {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }
        public virtual string P_Join_Title()
        {
            return "Тип объединения";
        }
        public virtual string P_Join_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual bool P_Join_Exists()
        {
            return false;
        }
        #endregion
        #region ControlType
        public virtual string P_ControlType {
            get {
                return this.AttrOrEmpty(AName_.controlType);
            }
            set {
                this.SetAttributeNotEmpty(AName_.controlType, value);
            }
        }
        public virtual string P_ControlType_Title()
        {
            return "Элемент управления";
        }
        public virtual bool P_ControlType_Exists()
        {
            return false;
        }
        public virtual bool P_ControlType_Editable()
        {
            return true;
        }
        public virtual string P_ControlType_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual void P_ControlType_List(VDataTable table)
        {
            table.AddColumn("id");
            table.AddColumn("name", "Элемент управления");
        }
        public virtual void P_ControlType_ListRefresh(VDataTable table)
        {
            FillDataTableFromStringArray(table, TextConst.AVControlTypeArray.All);
        }
        #endregion
        #region EditMask
        public virtual string P_EditMask {
            get {
                return this.AttrOrEmpty(AName_.edit_mask);
            }
            set {
                this.SetAttributeNotEmpty(AName_.edit_mask, value);
            }
        }
        public virtual string P_EditMask_Title()
        {
            return "Формат числа";
        }
        public virtual bool P_EditMask_Exists()
        {
            return false;
        }
        public virtual bool P_EditMask_Editable()
        {
            return true;
        }
        public virtual string P_EditMask_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual void P_EditMask_List(VDataTable table)
        {
            table.AddColumn("name", "Формат");
            table.AddColumn("abbr", "Описание");
        }
        public virtual void P_EditMask_ListRefresh(VDataTable table)
        {
            table.Rows.Clear();
            table.AddRow("d", "Целый");
            table.AddRow("f", "С точкой");
            table.AddRow("c", "Валюта");
        }
        #endregion
        #region Form
        public virtual string P_Form {
            get {
                return this.AttrOrEmpty(AName_.form);
            }
            set {
                this.SetAttributeNotEmpty(AName_.form, value);
            }
        }
        public virtual string P_Form_Title()
        {
            return "Форма";
        }
        public virtual string P_Form_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual void P_Form_List(VDataTable table)
        {
            table.AddColumn("id", "Форма");
            table.AddColumn("title", "Заголовок");
        }
        public virtual void P_Form_ListRefresh(VDataTable table)
        {
            table.Rows.Clear();
            foreach (XElement el in XmlReports.Environment.Manager.GetNativeScheme().Elements(EName.forms).Elements(EName.form)) {
                table.AddRow(el.AttrOrEmpty(AName_.name), el.AttrOrEmpty(AName_.title));
            }
        }
        public virtual bool P_Form_Exists()
        {
            return false;
        }
        public virtual string P_Form_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.MainMain;
        }
        #endregion
        #region Report
        public virtual string P_Report {
            get {
                return this.AttrOrEmpty(AName_.report);
            }
            set {
                this.SetAttributeNotEmpty(AName_.report, value);
            }
        }
        public virtual string P_Report_Title()
        {
            return "Отчёт";
        }
        public virtual string P_Report_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual bool P_Report_Exists()
        {
            return false;
        }
        public virtual string P_Report_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.MainMain;
        }
        #endregion
        #region TemplateName
        public virtual string P_TemplateName
        {
            get
            {
                return GetAttrValue(TextConst.AName.TemplateName);
            }
            set
            {
                SetAttributeNotEmpty(TextConst.AName.TemplateName, value);

            }

        }

        public virtual string P_TemplateName_Title()
        {

            return "Использовать как шаблон с именем";

        }

        public virtual bool P_TemplateName_Exists()
        {

            return IsMainElement();

        }
        #endregion
        #region Call
        public virtual string P_Call {
            get {
                return this.AttrOrEmpty(AName_.call);
            }
            set {
                this.SetAttributeNotEmpty(AName_.call, value);
            }
        }
        public virtual string P_Call_Title()
        {
            return "Вызов";
        }
        public virtual bool P_Call_Exists()
        {
            return false;
        }
        #endregion
        #region ParName
        public virtual string P_ParName {
            get {
                return this.AttrOrEmpty(AName_.parname);
            }
            set {
                this.SetAttributeNotEmpty(AName_.parname, value);
            }
        }
        public virtual string P_ParName_Title()
        {
            return "Имя переменной";
        }
        public virtual bool P_ParName_Exists()
        {
            VSXElement parent = this.GetParent();
            return (parent is VUseAction) || (parent is VUsePart); // дополнить
        }
        #endregion
        #region DName
        public virtual string P_DName
        {
            get
            {
                return GetAttrValue(TextConst.AName.DName);
            }
            set
            {
                SetAttributeNotEmpty(TextConst.AName.DName, value);

            }

        }

        public virtual string P_DName_Title()
        {

            return "Обратное имя связи";

        }

        public virtual bool P_DName_Exists()
        {

            return false;

        }
        #endregion

        #region DTitle
        public virtual string P_DTitle
        {
            get
            {
                return GetAttrValue(TextConst.AName.DTitle);
            }
            set
            {
                SetAttributeNotEmpty(TextConst.AName.DTitle, value);

            }

        }

        public virtual string P_DTitle_Title()
        {

            return "Обратный заголовок связи";

        }

        public virtual bool P_DTitle_Exists()
        {

            return false;

        }
        #endregion

        #region DXName
        public virtual string P_DXName
        {
            get
            {
                var s = P_DName;
                if (s == "")
                {
                    s = RootQuery().GetMainE().P_CalledQuery;
                }
                return s;
            }
        }

        public virtual string P_DXName_Title()
        {

            return "Обратное имя связи (вычисляемое)";

        }

        public virtual bool P_DXName_Editable()
        {

            return false;

        }

        public virtual bool P_DXName_Exists()
        {

            return P_DName_Exists();

        }
        #endregion
        #region DXTitle
        public virtual string P_DXTitle
        {
            get
            {
                return "";
            }
        }

        public virtual string P_DXTitle_Title()
        {

            return "Обратный заголовок связи (вычисляемый)";

        }

        public virtual bool P_DXTitle_Exists()
        {

            return P_DTitle_Exists();

        }
        #endregion

        #region Prior
        public virtual string P_Prior
        {
            get
            {
                return GetAttrValue(TextConst.AName.Prior);
            }
            set
            {
                SetAttributeNotEmpty(TextConst.AName.Prior, value);

            }

        }
        public virtual string P_Prior_ControlType()
        {

            return typeof(UICheck).Name;

        }
        public virtual string P_Prior_Title()
        {

            return "Prior";

        }

        public virtual bool P_Prior_Exists()
        {

            return false;

        }
        #endregion
        #region Key
        public virtual string P_Key {
            get {
                return this.AttrOrEmpty(AName_.key);
            }
            set {
                this.SetAttributeNotEmpty(AName_.key, value);
            }
        }
        public virtual string P_Key_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual string P_Key_Title()
        {
            return "Колонка является ключевой";
        }
        public virtual bool P_Key_Exists()
        {
            return this.GetParent() is VSelect;
        }
        #endregion
        #region ClearOnListChange
        public virtual string P_ClearOnListChange
        {
            get
            {
                return GetAttrValue(TextConst.AName.ClearOnListChange);
            }
            set
            {
                SetAttributeNotEmpty(TextConst.AName.ClearOnListChange, value);

            }

        }
        public virtual string P_ClearOnListChange_ControlType()
        {

            return typeof(UICheck).Name;

        }
        public virtual string P_ClearOnListChange_Title()
        {

            return "Очищать при изменении списка";

        }

        public virtual bool P_ClearOnListChange_Exists()
        {

            return false;

        }

        public virtual string P_ClearOnListChange_FieldGroup()
        {

            return TextConst.SchEdirorFieldGr.Behavior;

        }
        #endregion
        #region ParamsCustomization (VReport и VQuery)
        public virtual string P_ParamsCustomization {
            get {
                return this.AttrOrEmpty(AName_.params_customization);
            }
            set {
                this.SetAttributeNotEmpty(AName_.params_customization, value);
            }
        }
        public virtual string P_ParamsCustomization_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual string P_ParamsCustomization_Title()
        {
            return "Разрешен выбор параметров";
        }
        public virtual bool P_ParamsCustomization_Exists()
        {
            return false;
        }
        #endregion
        #region UseTemp (VReport и VQuery)
        public virtual string P_UseTemp {
            get {
                return this.AttrOrEmpty(AName_.use_temp);
            }
            set {
                this.SetAttributeNotEmpty(AName_.use_temp, value);
            }
        }
        public virtual string P_UseTemp_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual string P_UseTemp_Title()
        {
            return "Сохранить результат во временную таблицу";
        }
        public virtual bool P_UseTemp_Exists()
        {
            return false;
        }
        #endregion
        #region AllowSave (VReport и VQuery)
        public virtual string P_AllowSave {
            get {
                return this.AttrOrEmpty(AName_.allow_save);
            }
            set {
                this.SetAttributeNotEmpty(AName_.allow_save, value);
            }
        }
        public virtual string P_AllowSave_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual string P_AllowSave_Title()
        {
            return "Разрешено сохранение шаблона";
        }
        public virtual bool P_AllowSave_Exists()
        {
            return false;
        }
        #endregion
        #region EditColumns (VReport и VQuery)
        public virtual string P_EditColumns {
            get {
                return this.AttrOrEmpty(AName_.edit_columns);
            }
            set {
                this.SetAttributeNotEmpty(AName_.edit_columns, value);
            }
        }
        public virtual string P_EditColumns_ControlType()
        {
            return typeof(UIText).Name;
        }
        public virtual string P_EditColumns_Title()
        {
            return "Разрешен выбор колонок (нужно бы заменить на check но бывает 2-ка)";
        }
        public virtual bool P_EditColumns_Exists()
        {
            return false;
        }
        #endregion
        #region Field (VParam и VUseField)
        public virtual string P_Field {
            get {
                return this.AttrOrEmpty(AName_.field);
            }
            set {
                this.SetAttributeNotEmpty(AName_.field, value);
            }
        }
        public virtual string P_Field_Title()
        {
            return "Поле";
        }
        public virtual string P_Field_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual void P_Field_List(VDataTable table)
        {
            table.AddColumn("id", "Поле");
            table.AddColumn("par_name", "Имя параметра");
            table.AddColumn("title", "Заголовок");
        }
        public virtual void P_Field_ListRefresh(VDataTable table)
        {
            table.Rows.Clear();
            HashSet<string> names = new HashSet<string>();
            foreach (XElement el in XmlReports.Environment.Manager.GetNativeScheme().Elements(EName.fields).Elements(EName.field)) {
                string id = el.AttrOrEmpty(AName_.id);
                if (names.Contains(id)) {
                    Debug.WriteLine("Поле c именем \"" + id + "\" дублируется, используйте XPath //fields/field[@id=\"" + id + "\"] для поиска дублей.");
                } else {
                    table.AddRow(id, el.AttrOrEmpty(AName_.name), el.AttrOrEmpty(AName_.title));
                    names.Add(id);
                }
            }
        }
        public virtual bool P_Field_Exists()
        {
            return false;
        }
        #endregion
        #region FormalParName
        public virtual string P_FormalParName {
            get {
                return this.AttrOrEmpty(AName_.name);
            }
            set {
                this.SetAttributeNotEmpty(AName_.name, value);
            }
        }
        public virtual string P_FormalParName_Title()
        {
            return "Имя параметра";
        }
        public virtual bool P_FormalParName_Exists()
        {
            return false;
        }
        public virtual string P_FormalParName_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.MainMain;
        }
        #endregion
        #region FormalParNameS
        public virtual string P_FormalParNameS
        {
            get
            {

                return P_FormalParName;

            }


        }

        public virtual string P_FormalParNameS_Title()
        {

            return "Имя параметра (вычисляемое)";

        }

        public virtual bool P_FormalParNameS_Exists()
        {

            return false;

        }

        public virtual bool P_FormalParNameS_Editable()
        {

            return false;

        }

        public virtual bool P_FormalParNameS_VisibleInForm()
        {

            return false;

        }


        #endregion
        #region UsedParName
        public virtual string P_UsedParName {
            get {
                return this.AttrOrEmpty(AName_.name);
            }
            set {
                this.SetAttributeNotEmpty(AName_.name, value);
            }
        }
        public virtual string P_UsedParName_Title()
        {
            return "Параметр";
        }
        public virtual string P_UsedParName_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual bool P_UsedParName_Exists()
        {
            return false;
        }
        #endregion
        //#region SelfDataType
        //public virtual string P_SelfDataType
        //{
        //    get
        //    {
        //        return GetAttrValue(TextConst.AName.DataType);
        //    }
        //    set
        //    {

        //        SetAttributeNotEmpty(TextConst.AName.DataType, value);
        //    }
        //}



        //public virtual string P_SelfDataType_Title()
        //{

        //    return "Тип данных";

        //}

        //public virtual string P_SelfDataType_ControlType()
        //{

        //    return typeof(UICombo).Name;

        //}

        //public virtual void P_SelfDataType_List(VDataTable table)
        //{

        //    table.AddColumn("name", "Тип данных");

        //}



        //public virtual void P_SelfDataType_ListRefresh(VDataTable table)
        //{
        //    table.Rows.Clear();
        //    table.Rows.Add(TextConst.AVType.Number);
        //    table.Rows.Add(TextConst.AVType.String);
        //    table.Rows.Add(TextConst.AVType.Date);
        //    table.Rows.Add(TextConst.AVType.Array);

        //}
        //public virtual bool P_SelfDataType_Exists()
        //{

        //    return false;

        //}

        //public virtual string P_SelfDataType_FieldGroup()
        //{
        //    if (GetParent() is VOutputElement)
        //    {
        //        return TextConst.SchEdirorFieldGr.MainMain;
        //    }
        //    else
        //    {
        //        return TextConst.SchEdirorFieldGr.MainOther;
        //    }

        //}
        //#endregion
        #region Exclude
        public virtual string P_Exclude {
            get {
                return this.AttrOrEmpty(AName_.exclude);
            }
            set {
                this.SetAttributeNotEmpty(AName_.exclude, value);
            }
        }
        public virtual string P_Exclude_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual string P_Exclude_Title()
        {
            return "Исключить";
        }
        public virtual bool P_Exclude_Exists()
        {
            return !this.IsMainElement();
        }
        public virtual string P_Exclude_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.MainMain;
        }
        #endregion
        //#region Prime
        //public virtual string P_Prime
        //{
        //    get
        //    {
        //        return GetAttrValue(TextConst.AName.Prime);
        //    }
        //    set
        //    {
        //        SetAttributeNotEmpty(TextConst.AName.Prime, value);

        //    }

        //}

        //public virtual string P_Prime_ControlType()
        //{

        //    return typeof(UICheck).Name;

        //}

        //public virtual string P_Prime_Title()
        //{

        //    return "Назначить главным";

        //}

        //public virtual bool P_Prime_Exists()
        //{

        //    return false;

        //}
        //#endregion
        #region Materialize
        public virtual string P_Materialize {
            get {
                return this.AttrOrEmpty(AName_.materialize);
            }
            set {
                this.SetAttributeNotEmpty(AName_.materialize, value);
            }
        }
        public virtual string P_Materialize_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual string P_Materialize_Title()
        {
            return "Материализовать";
        }
        public virtual bool P_Materialize_Exists()
        {
            return false;
        }
        #endregion
        #region Size
        public virtual string P_Size {
            get {
                return this.AttrOrEmpty(AName_.size);
            }
            set {
                this.SetAttributeNotEmpty(AName_.size, value);
            }
        }
        public virtual string P_Size_Title()
        {
            return "Размер";
        }
        public virtual bool P_Size_Exists()
        {
            return P_Position_Exists();
        }
        public virtual bool P_Size_Editable()
        {
            return false;
        }
        #endregion
        #region FormSize
        public virtual string P_FormSize
        {
            get
            {
                return GetAttrValue(TextConst.AName.FormSize);
            }
            set
            {
                SetAttributeNotEmpty(TextConst.AName.FormSize, value);
            }
        }

        public virtual string P_FormSize_Title()
        {
            return "Размер формы";
        }

        public virtual bool P_FormSize_Exists()
        {
            return P_Position_Exists();
        }

        public virtual bool P_FormSize_Editable()
        {
            return true;
        }
        #endregion
        #region DataSize (VColumn)
        public virtual string P_DataSize {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }
        public string XDataSize()
        {
            VSXElement e = this.P_DataSize_Search().FirstOrDefault();
            if (e == null) {
                return string.Empty;
            } else {
                return e.P_DataSize;
            }
        }
        public List<VSXElement> P_DataSize_Search()
        {
            VLookupAnalyzer anl = new VLookupAnalyzer();
            anl.CheckReturn = (VSXElement element) =>
            {
                if (element.P_DataSize != "")
                {
                    return true;
                }
                return false;
            };
            anl.CheckStop = (VSXElement element) =>
            {
                return anl.CheckReturn(element);
            };
            return LookUpSources(anl);
        }
        public virtual string P_DataSize_Title()
        {
            return "Размер данных";
        }
        public virtual bool P_DataSize_Exists()
        {
            return false;
        }
        #endregion
        #region Position
        public virtual string P_Position {
            get {
                return this.AttrOrEmpty(AName_.position);
            }
            set {
                this.SetAttributeNotEmpty(AName_.position, value);
            }
        }
        public virtual string P_Position_Title()
        {
            return "Координаты";
        }
        public virtual bool P_Position_Exists()
        {
            return false;
        }
        public virtual bool P_Position_Editable()
        {
            return false;
        }
        #endregion
        //#region LayoutMode
        //public virtual string P_LayoutMode
        //{
        //    get
        //    {
        //        return GetAttrValue(TextConst.AName.LayoutMode);
        //    }
        //    set
        //    {
        //        SetAttributeNotEmpty(TextConst.AName.LayoutMode, value);
        //    }
        //}

        //public virtual string P_LayoutMode_Title()
        //{
        //    return "Расположение элементов";
        //}

        //public virtual string P_LayoutMode_ControlType()
        //{

        //    return typeof(UICombo).Name;

        //}

        //public virtual void P_LayoutMode_List(VDataTable table)
        //{
        //    table.AddColumn("id");
        //    table.AddColumn("name", "Наименование");
        //}

        //public virtual void P_LayoutMode_ListRefresh(VDataTable table)
        //{
        //    table.Columns["id"].AllowDBNull = true;

        //    table.Rows.Clear();
        //    table.Rows.Add(DBNull.Value, "");
        //    table.Rows.Add("regular", "Обычный");
        //    table.Rows.Add("table", "Табличный");
        //    table.Rows.Add("flow", "Плавающий");
        //}

        //public virtual bool P_LayoutMode_Exists()
        //{
        //    return false;
        //}

        //public virtual bool P_LayoutMode_Editable()
        //{
        //    return false;
        //}
        //#endregion
        #region WidthPerc
        public virtual string P_WidthPerc {
            get {
                return this.AttrOrEmpty(AName_.width_perc);
            }
            set {
                this.SetAttributeNotEmpty(AName_.width_perc, value);
            }
        }
        public virtual string P_WidthPerc_Title()
        {
            return "Относительная ширина";
        }
        public virtual string P_WidthPerc_ControlType()
        {
            return typeof(UIText).Name;
        }
        public virtual bool P_WidthPerc_Exists()
        {
            VSXElement parent = this.GetParent();
            return (parent is VFieldGroup) || (parent is VFormContent);
        }
        public virtual bool P_WidthPerc_Editable()
        {
            return P_WidthPerc_Exists();
        }
        public virtual string P_WidthPerc_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.Layout;
        }
        #endregion
        #region WidthFixed
        public virtual string P_WidthFixed {
            get {
                return this.AttrOrEmpty(AName_.width_fixed);
            }
            set {
                this.SetAttributeNotEmpty(AName_.width_fixed, value);
            }
        }
        public virtual string P_WidthFixed_Title()
        {
            return "Фиксированая ширина";
        }
        public virtual string P_WidthFixed_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual bool P_WidthFixed_Exists()
        {
            VSXElement parent = this.GetParent();
            return (parent is VFieldGroup) || (parent is VFormContent);
        }
        public virtual bool P_WidthFixed_Editable()
        {
            return P_WidthPerc_Exists();
        }

        public virtual string P_WidthFixed_FieldGroup()
        {

            return TextConst.SchEdirorFieldGr.Layout;

        }
        #endregion
        #region Updateable (VQueryCall)
        public virtual string P_Updateable {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }
        public virtual string P_Updateable_Title()
        {
            return "Редактируемый";
        }
        public virtual string P_Updateable_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual bool P_Updateable_Exists()
        {
            return false;
        }
        #endregion
        #region FillHeight
        public virtual string P_FillHeight
        {
            get
            {
                return GetAttrValue(TextConst.AName.FillHeight);
            }
            set
            {
                SetAttributeNotEmpty(TextConst.AName.FillHeight, value);
            }
        }

        public virtual string P_FillHeight_Title()
        {
            return "Заполняет высоту";
        }

        public virtual string P_FillHeight_ControlType()
        {
            return typeof(UICheck).Name;
        }

        public virtual bool P_FillHeight_Exists()
        {
            return (GetParent() is VFieldGroup) || (GetParent() is VFormContent);
        }

        public virtual bool P_FillHeight_Editable()
        {
            return P_WidthPerc_Exists();
        }

        public virtual string P_FillHeight_FieldGroup()
        {

            return TextConst.SchEdirorFieldGr.Layout;

        }
        #endregion
        #region TextVisible
        public virtual string P_TextVisible {
            get {
                return this.AttrOrEmpty(AName_.text_visible);
            }
            set {
                this.SetAttributeNotEmpty(AName_.text_visible, value);
            }
        }
        public virtual string P_TextVisible_Title()
        {
            return "Видимость текста";
        }
        public virtual string P_TextVisible_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual bool P_TextVisible_Exists()
        {
            return false;
        }
        public virtual bool P_TextVisible_Editable()
        {
            return false;
        }
        #endregion
        #region TextLocation
        public virtual string P_TextLocation
        {
            get
            {
                return GetAttrValue(TextConst.AName.TextLocation);
            }
            set
            {
                SetAttributeNotEmpty(TextConst.AName.TextLocation, value);
            }
        }

        public virtual void P_TextLocation_List(VDataTable table)
        {
            table.AddColumn("id");
            table.AddColumn("name", "Наименование");
        }
        public virtual void P_TextLocation_ListRefresh(VDataTable table)
        {
            table.Columns["id"].AllowDBNull = true;
            table.Rows.Clear();
            table.AddRow(DBNull.Value, "");
            table.AddRow("top", "Сверху");
            table.AddRow("left", "Слева");
            table.AddRow("right", "Справа");
            table.AddRow("bottom", "Снизу");
        }
        public virtual string P_TextLocation_Title()
        {
            return "Позиция текста";
        }
        public virtual string P_TextLocation_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual bool P_TextLocation_Exists()
        {
            return false;
        }
        public virtual bool P_TextLocation_Editable()
        {
            return false;
        }
        #endregion
        #region MultiSelectColumn (VQueryCall)
        public virtual string P_MultiSelectColumn {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }
        public virtual string P_MultiSelectColumn_Title()
        {
            return "Колонка для множествнного выбора";
        }
        public virtual string P_MultiSelectColumn_ControlType()
        {
            return typeof(UICombo).Name;
        }
        /*public virtual void P_MultiSelectColumn_List(VDataTable table)
        {
        }
        public virtual void P_MultiSelectColumn_ListRefresh(VDataTable table)
        {
        }*/
        public virtual bool P_MultiSelectColumn_Exists()
        {
            return false;
        }
        #endregion
        #region MultiSelectTarget (VQueryCall)
        public virtual string P_MultiSelectTarget {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }
        public virtual string P_MultiSelectTarget_Title()
        {
            return "Целевая таблица множествнного выбора";
        }
        public virtual string P_MultiSelectTarget_ControlType()
        {
            return typeof(UICombo).Name;
        }
        /*public virtual void P_MultiSelectTarget_List(VDataTable table)
        {
        }
        public virtual void P_MultiSelectTarget_ListRefresh(VDataTable table)
        {
        }*/
        public virtual bool P_MultiSelectTarget_Exists()
        {
            return false;
        }
        #endregion
        #region Extend
        public virtual string P_Extend {
            get {
                return this.AttrOrEmpty(AName_.extend);
            }
            set {
                this.SetAttributeNotEmpty(AName_.extend, value);
            }
        }
        public virtual string P_Extend_Title()
        {
            return "Расширяет запрос";
        }
        public virtual string P_Extend_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual void P_Extend_List(VDataTable table)
        {
            table.AddColumn("id");
            table.AddColumn("name", "Имя");
            table.AddColumn("title", "Заголовок");
        }
        public virtual void P_Extend_ListRefresh(VDataTable table)
        {
            VSXElement.FillDataTableFromRealQueries(table);
        }
        public virtual bool P_Extend_Exists()
        {
            return false;
        }
        #endregion
        #region Inherit
        public virtual string P_Inherit {
            get {
                return this.AttrOrEmpty(AName_.inherit);
            }
            set {
                this.SetAttributeNotEmpty(AName_.inherit, value);
            }
        }
        public virtual string P_Inherit_Title()
        {
            return "Наследник от";
        }
        public virtual string P_Inherit_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual void P_Inherit_List(VDataTable table)
        {
            table.AddColumn("id");
            table.AddColumn("name", "Имя");
            table.AddColumn("title", "Заголовок");
        }
        public virtual void P_Inherit_ListRefresh(VDataTable table)
        {
            table.Rows.Clear();
            foreach (VQuery el in XmlReports.Environment.GetElements(TextConst.EName.Queries)) {
                try {
                    if (!el.IsExtension()) {
                        table.AddRow(el.P_IdName, el.P_IdName, el.Title());
                    }
                } catch {
                }
            }

        }
        public virtual bool P_Inherit_Exists()
        {
            return this.P_Extend_Exists();
        }
        public virtual VSXElement P_Inherit_UsedEl()
        {
            return XmlReports.Environment.GetQuery(this.P_Inherit);
        }
        #endregion
        #region UpdateTarget
        public virtual string P_UpdateTarget {
            get {
                return this.AttrOrEmpty(AName_.update_target);
            }
            set {
                this.SetAttributeNotEmpty(AName_.update_target, value);
            }
        }
        public virtual string P_UpdateTarget_Title()
        {
            return "Меняет таблицу";
        }
        public virtual string P_UpdateTarget_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual void P_UpdateTarget_List(VDataTable table)
        {
            this.P_Extend_List(table);
        }
        public virtual void P_UpdateTarget_ListRefresh(VDataTable table)
        {
            this.P_Extend_ListRefresh(table);
        }
        public virtual bool P_UpdateTarget_Exists()
        {
            return this.P_Extend_Exists();
        }
        #endregion
        #region Expanded
        public virtual string P_Expanded {
            get {
                string value = this.AttrOrDefault(AName_.expanded, TextConst.AVBool.True);
                if (value == TextConst.AVBool.False) {
                    value = string.Empty;
                }
                return value;
            }
            set {
                if (string.IsNullOrEmpty(value)) {
                    value = TextConst.AVBool.False;
                } else if (value == TextConst.AVBool.True) {
                    value = null;
                }
                this.SetAttributeValue(AName_.expanded, value);
            }
        }
        public virtual string P_Expanded_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual string P_Expanded_Title()
        {
            return "Развёрнута";
        }
        public virtual bool P_Expanded_Exists()
        {
            return false;
        }
        #endregion
        #region Uncollapsible
        public virtual string P_Uncollapsible
        {
            get
            {
                return GetAttrValue(TextConst.AName.Uncollapsible); 
            }
            set
            {
                SetAttributeNotEmpty(TextConst.AName.Uncollapsible, value);
            }
        }

        public virtual string P_Uncollapsible_ControlType()
        {
            return typeof(UICheck).Name;
        }

        public virtual string P_Uncollapsible_Title()
        {
            return "Несворачиваемая";
        }

        public virtual bool P_Uncollapsible_Exists()
        {
            return false;
        }
        #endregion
        #region Optional
        public virtual string P_Optional {
            get {
                return this.AttrOrEmpty(AName_.optional);
            }
            set {
                this.SetAttributeNotEmpty(AName_.optional, value);
            }
        }
        public virtual string P_Optional_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual string P_Optional_Title()
        {
            return "Исключить условие, если не задан параметр";
        }
        public virtual bool P_Optional_Exists()
        {
            return false;
        }
        #endregion
        #region UseDataReader (VReport и VQuery)
        public virtual string P_UseDataReader {
            get {
                return this.AttrOrEmpty(AName_.datareader);
            }
            set {
                this.SetAttributeNotEmpty(AName_.datareader, value);
            }
        }
        public virtual string P_UseDataReader_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual string P_UseDataReader_Title()
        {
            return "Не загружать данные на клиент";
        }
        public virtual bool P_UseDataReader_Exists()
        {
            return false;
        }
        #endregion
        #region IsHyperlink
        public virtual string P_IsHyperlink
        {
            get
            {
                return GetAttrValue(TextConst.AName.IsHyperlink);
            }
            set
            {
                SetAttributeNotEmpty(TextConst.AName.IsHyperlink, value);

            }

        }

        public virtual string P_IsHyperlink_ControlType()
        {

            return typeof(UICheck).Name;

        }

        public virtual string P_IsHyperlink_Title()
        {

            return "Гиперссылка";

        }

        public virtual bool P_IsHyperlink_Exists()
        {

            return false;

        }
        #endregion
        #region UseOnlyWithOther
        public virtual string P_UseOnlyWithOther
        {
            get
            {
                return GetAttrValue(TextConst.AName.UseOnlyWithOther);
            }
            set
            {
                SetAttributeNotEmpty(TextConst.AName.UseOnlyWithOther, value);

            }

        }

        public virtual string P_UseOnlyWithOther_ControlType()
        {

            return typeof(UICheck).Name;

        }

        public virtual string P_UseOnlyWithOther_Title()
        {

            return "Исключить усл., если нет других усл.";

        }

        public virtual bool P_UseOnlyWithOther_Exists()
        {

            return false;

        }
        #endregion
        //#region ExcludeIfSet
        //public virtual string P_ExcludeIfSet
        //{
        //    get
        //    {
        //        return GetAttrValue(TextConst.AName.ExcludeIfSet);
        //    }
        //    set
        //    {
        //        SetAttributeNotEmpty(TextConst.AName.ExcludeIfSet, value);

        //    }

        //}

        //public virtual string P_ExcludeIfSet_ControlType()
        //{

        //    return typeof(UICheck).Name;

        //}

        //public virtual string P_ExcludeIfSet_Title()
        //{

        //    return "Исключить условие, если задан параметр";

        //}

        //public virtual bool P_ExcludeIfSet_Exists()
        //{

        //    return false;

        //}
        //#endregion
        #region DxExport (VReport, VQuery и VGrid)
        public virtual string P_DxExport {
            get {
                return this.AttrOrEmpty(AName_.dx_export);
            }
            set {
                this.SetAttributeNotEmpty(AName_.dx_export, value);
            }
        }
        public virtual string P_DxExport_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual string P_DxExport_Title()
        {
            return "Экспорт через DevExpress";
        }
        public virtual bool P_DxExport_Exists()
        {
            return false;
        }
        #endregion
        #region ActionType
        public virtual string P_ActionType {
            get {
                return this.AttrOrEmpty(AName_.action_type);
            }
            set {
                this.SetAttributeNotEmpty(AName_.action_type, value);
            }
        }
        public virtual string P_ActionType_Title()
        {
            return "Тип операции";
        }
        public virtual string P_ActionType_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual void P_ActionType_List(VDataTable table)
        {
            table.AddColumn("id");
            table.AddColumn("name");
        }
        public virtual void P_ActionType_ListRefresh(VDataTable table)
        {
            FillDataTableFromStringArray(table, TextConst.AVActionTypeArray.All);
        }
        public virtual bool P_ActionType_Exists()
        {
            return false;
        }
        #endregion
        #region Project
        public virtual string P_Project {
            get {
                return this.AttrOrEmpty(AName_.project);
            }
            set {
                this.SetAttributeNotEmpty(AName_.project, value);
            }
        }
        public virtual string P_Project_Title()
        {
            return "Проект";
        }
        public virtual string P_Project_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual void P_Project_List(VDataTable table)
        {
            table.AddColumn("id");
            table.AddColumn("name");
        }
        public virtual void P_Project_ListRefresh(VDataTable table)
        {
            table.Rows.Clear();
            foreach (sql.builder.Core.Project p in XmlReports.Environment.Manager.GetAllProjects()) {
                string s = p.Name;
                table.AddRow(s, s);
            }
        }
        public virtual bool P_Project_Exists()
        {
            return false;
        }
        #endregion
        #region CalledAction (VUseAction only)
        public virtual string P_CalledAction {
            get {
                return this.AttrOrEmpty(AName_.name);
            }
            set {
                this.SetAttributeNotEmpty(AName_.name, value);
            }
        }
        public virtual string P_CalledAction_Title()
        {
            return "Вызываемая операция";
        }
        public virtual string P_CalledAction_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual bool P_CalledAction_Exists()
        {
            return false;
        }
        #endregion
        #region CalledObject
        public virtual string P_CalledObject {
            get {
                return this.AttrOrEmpty(AName_.@object);
            }
            set {
                this.SetAttributeNotEmpty(AName_.@object, value);
            }
        }
        public virtual string P_CalledObject_Title()
        {
            return "Объект";
        }
        public virtual string P_CalledObject_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual bool P_CalledObject_Exists()
        {
            return false;
        }
        #endregion
        #region Selective
        public virtual string P_Selective
        {
            get
            {
                return GetAttrValue(TextConst.AName.Selective);
            }
            set
            {
                SetAttributeNotEmpty(TextConst.AName.Selective, value);

            }

        }

        public virtual string P_Selective_ControlType()
        {

            return typeof(UICheck).Name;

        }

        public virtual string P_Selective_Title()
        {

            return "Применять выборочно";

        }

        public virtual bool P_Selective_Exists()
        {

            return false;

        }
        #endregion
        #region IsReport (VQuery)
        public virtual string P_IsReport {
            get {
                return this.AttrOrEmpty(AName_.is_report);
            }
            set {
                this.SetAttributeNotEmpty(AName_.is_report, value);
            }
        }
        public virtual string P_IsReport_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual string P_IsReport_Title()
        {
            return "Использовать как отчет";
        }
        public virtual bool P_IsReport_Exists()
        {
            return false;
        }
        #endregion
        #region Invisible
        public virtual string P_Invisible {
            get {
                if (this.AttrOrEmpty(AName_.visible) == TextConst.AVBool.False) {
                    return TextConst.AVBool.True;
                } else {
                    return string.Empty;
                }
            }
            set {
                if (value != null && value == TextConst.AVBool.True) {
                    value = TextConst.AVBool.False;
                } else {
                    value = null;
                }
                this.SetAttributeValue(AName_.visible, value);
            }
        }
        public virtual string P_Invisible_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual string P_Invisible_Title()
        {
            return "Невидимый";
        }
        public virtual bool P_Invisible_Exists()
        {
            return false;
        }
        #endregion
        #region Customer
        public virtual string P_Customer {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }
        public virtual string P_Customer_Title()
        {
            return "Заказчик";
        }
        public virtual string P_Customer_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual bool P_Customer_Exists()
        {
            return false;
        }
        #endregion
        #region AddNames
        public virtual string P_AddNames
        {
            get
            {
                return GetAttrValue(TextConst.AName.AddNames);
            }
            set
            {
                SetAttributeNotEmpty(TextConst.AName.AddNames, value);
            }

        }

        public virtual string P_AddNames_ControlType()
        {

            return typeof(UICheck).Name;

        }

        public virtual string P_AddNames_Title()
        {

            return "Добавить колонки с именами";

        }

        public virtual bool P_AddNames_Exists()
        {

            return false;

        }
        #endregion
        #region RowsLimit
        public virtual string P_RowsLimit {
            get {
                return this.AttrOrEmpty(AName_.rows_limit);
            }
            set {
                this.SetAttributeNotEmpty(AName_.rows_limit, value);
            }
        }
        public virtual string P_RowsLimit_Title()
        {
            return "Ограничение по к-ву загружаемых строк";
        }
        public virtual bool P_RowsLimit_Exists()
        {
            return false;
        }
        #endregion
        #region Step
        public virtual string P_Step {
            get {
                return this.AttrOrEmpty(AName_.step);
            }
            set {
                this.SetAttributeNotEmpty(AName_.step, value);
            }
        }
        public virtual string P_Step_ControlType()
        {
            return typeof(UINumber).Name;
        }
        public virtual string P_Step_Title()
        {
            return "Инкримент";
        }
        public virtual bool P_Step_Exists()
        {
            return false;
        }
        #endregion
        #region MaxLength
        public virtual string P_MaxLength {
            get {
                return this.AttrOrEmpty(AName_.max_length);
            }
            set {
                this.SetAttributeNotEmpty(AName_.max_length, value);
            }
        }
        public virtual string P_MaxLength_ControlType()
        {
            return typeof(UINumber).Name;
        }
        public virtual string P_MaxLength_Title()
        {
            return "Максимальное число знаков";
        }
        public virtual bool P_MaxLength_Exists()
        {
            return false;
        }
        #endregion
        #region ColumnWidth
        public virtual string P_ColumnWidth
        {
            get
            {
                return GetAttrValue(TextConst.AName.ColumnWidth);
            }
            set
            {
                SetAttributeNotEmpty(TextConst.AName.ColumnWidth, value);

            }

        }

        public virtual string P_ColumnWidth_ControlType()
        {
            return typeof(UINumber).Name;
        }

        public virtual string P_ColumnWidth_Title()
        {
            return "Ширина колонки";
        }

        public virtual bool P_ColumnWidth_Exists()
        {
            return false;
        }
        #endregion
        #region ParentFieldName
        public virtual string P_ParentFieldName {
            get {
                return this.AttrOrEmpty(AName_.parent_field_name);
            }
            set {
                this.SetAttributeNotEmpty(AName_.parent_field_name, value);
            }
        }
        public virtual string P_ParentFieldName_Title()
        {
            return "Родительское поле дерева";
        }
        public virtual string P_ParentFieldName_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual void P_ParentFieldName_List(VDataTable table)
        {
            table.AddColumn("id");
            table.AddColumn("name", "Поле");
        }
        public virtual void P_ParentFieldName_ListRefresh(VDataTable table)
        {
            table.Rows.Clear();
            foreach (VSXElement el in Field().ListQuery().Query().Columns()) {
                string name = el.XName;
                table.AddRow(name, name);
            }
        }
        public virtual bool P_ParentFieldName_Exists()
        {
            return false;
        }
        #endregion
        #region OrderFieldName
        public virtual string P_OrderFieldName
        {
            get
            {
                return GetAttrValue(TextConst.AName.OrderFieldName);
            }
            set
            {

                SetAttributeNotEmpty(TextConst.AName.OrderFieldName, value);
            }
        }



        public virtual string P_OrderFieldName_Title()
        {

            return "Порядковое поле";

        }

        public virtual string P_OrderFieldName_ControlType()
        {

            return typeof(UICombo).Name;

        }

        public virtual void P_OrderFieldName_List(VDataTable table)
        {
            P_ParentFieldName_List(table);

        }

        public virtual void P_OrderFieldName_ListRefresh(VDataTable table)
        {
            table.Rows.Clear();
            P_ParentFieldName_ListRefresh(table);
        }
        public virtual bool P_OrderFieldName_Exists()
        {

            return false;

        }
        #endregion


        //#region CheckFieldName
        //public virtual string P_CheckFieldName
        //{
        //    get
        //    {
        //        return GetAttrValue(TextConst.AName.CheckFieldName);
        //    }
        //    set
        //    {

        //        SetAttributeNotEmpty(TextConst.AName.CheckFieldName, value);
        //    }
        //}



        //public virtual string P_CheckFieldName_Title()
        //{

        //    return "Поле выбора";

        //}

        //public virtual string P_CheckFieldName_ControlType()
        //{

        //    return typeof(UICombo).Name;

        //}

        //public virtual void P_CheckFieldName_List(VDataTable table)
        //{
           
        //    P_ParentFieldName_List(table);
        //}

        //public virtual void P_CheckFieldName_ListRefresh(VDataTable table)
        //{
           
        //    P_ParentFieldName_ListRefresh(table);
            
        //}
        //public virtual bool P_CheckFieldName_Exists()
        //{

        //    return P_OrderFieldName_Exists();

        //}
        //#endregion
        #region ClassTitle
        public virtual string P_ClassTitle {
            get {
                return this.AttrOrEmpty(AName_.class_title);
            }
            set {
                this.SetAttributeNotEmpty(AName_.class_title, value);
            }
        }
        public virtual string P_ClassTitle_Title()
        {
            return "Заголовок группы (собственный)";
        }
        public virtual string P_ClassTitle_ControlType()
        {
            return typeof(UIText).Name;
        }
        public virtual bool P_ClassTitle_Exists()
        {
            return false;
        }
        #endregion
        #region ClassTitleS
        public virtual string P_ClassTitleS {
            get {
                IList<VSXElement> src = this.P_ClassTitleS_Search();
                if (src.Count != 0) {
                    return (src[0].P_ClassTitle);
                } else {
                    return string.Empty;
                }
            }
        }
        private IList<VSXElement> P_ClassTitleS_Search()
        {
            VLookupAnalyzer anl = new VLookupAnalyzer();
            anl.CheckReturn = (VSXElement element) =>
            {
                if (element.P_ClassTitle != "")
                {
                    anl.NeedStop = true;
                    return true;
                }
                return false;
            };
            return LookUpSources(anl);
        }
        public virtual string P_ClassTitleS_Title()
        {

            return "Заголовок группы";

        }

        public virtual string P_ClassTitleS_ControlType()
        {

            return typeof(UIText).Name;

        }

        public virtual bool P_ClassTitleS_Exists()
        {

            //return GetParent() is VOutputElement;
            return false;

        }


        public virtual bool P_ClassTitleS_Editable()
        {
            return false;
        }

        #endregion

        #region AliasPfx

        public virtual string P_AliasPfx_Title()
        {

            return "Псевдоним (постфикс)";

        }

        public virtual string P_AliasPfx
        {
            get
            {
                return GetAttrValue(TextConst.AName.Pfx);
            }
            set
            {
                SetAttribute(TextConst.AName.Pfx, value);
            }
        }


        public virtual bool P_AliasPfx_Exists()
        {

            return false;

        }
        #endregion
        #region UseRepository
        public virtual string P_UseRepository {
            get {
                return this.AttrOrEmpty(AName_.use_repository);
            }
            set {
                this.SetAttributeNotEmpty(AName_.use_repository, value);
            }
        }
        public virtual string P_UseRepository_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual string P_UseRepository_Title()
        {
            return "Использовать хранилище";
        }
        public virtual bool P_UseRepository_Exists()
        {
            return false;
        }
        #endregion
        #region Index
        public virtual string P_Index {
            get {
                return this.AttrOrEmpty(AName_.index);
            }
            set {
                this.SetAttributeNotEmpty(AName_.index, value);
            }
        }
        public virtual string P_Index_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual string P_Index_Title()
        {
            return "Индекс (используется для mat view)";
        }
        public virtual bool P_Index_Exists()
        {
            return false;
        }
        #endregion
        #region IsTree
        public virtual string P_IsTree
        {
            get
            {
                return GetAttrValue(TextConst.AName.IsTree);
            }
            set
            {
                SetAttributeNotEmpty(TextConst.AName.IsTree, value);

            }

        }
        public virtual string P_IsTree_ControlType()
        {

            return typeof(UICheck).Name;

        }
        public virtual string P_IsTree_Title()
        {

            return "Дерево";

        }

        public virtual bool P_IsTree_Exists()
        {

            return false;

        }
        #endregion
        #region IsTreeSplitCols
        public virtual string P_IsTreeSplitCols
        {
            get
            {
                return GetAttrValue(TextConst.AName.IsTreeSplitCols);
            }
            set
            {
                SetAttributeNotEmpty(TextConst.AName.IsTreeSplitCols, value);

            }

        }
        public virtual string P_IsTreeSplitCols_ControlType()
        {

            return typeof(UICheck).Name;

        }
        public virtual string P_IsTreeSplitCols_Title()
        {

            return "Отдельные колонки для каждого уровня дерева";

        }

        public virtual bool P_IsTreeSplitCols_Exists()
        {

            return false;

        }
        #endregion
        #region Folder (VReport и VQuery)
        public virtual string P_Folder {
            get {
                return this.AttrOrEmpty(AName_.folder);
            }
            set {
                this.SetAttributeNotEmpty(AName_.folder, value);
            }
        }
        public virtual string P_Folder_Title()
        {
            return "Папка";
        }
        public virtual string P_Folder_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual void P_Folder_List(VDataTable table)
        {
            table.AddColumn("id", "Имя");
            table.AddColumn("title", "Заголовок");
        }
        public virtual void P_Folder_ListRefresh(VDataTable table)
        {
            table.Rows.Clear();
            var names = new HashSet<string>();
            foreach (XElement folder in XmlReports.Environment.Manager.GetNativeScheme().Elements(EName.folders).Descendants(EName.folder)) {
                string name = folder.AttrOrEmpty(AName_.name);
                if (!names.Contains(name)) {
                    names.Add(name);
                    table.AddRow(name, folder.AttrOrEmpty(AName_.title));
                }
            }
        }
        public virtual bool P_Folder_Exists()
        {
            return false;
        }
        #endregion
        #region AggregationS
        public virtual string P_AggregationS {
            get {
                string agg = this.AttrOrEmpty(AName_.agg);
                if (string.IsNullOrEmpty(agg)) {
                    return this.P_Group;
                } else {
                    return agg;
                }
            }
        }
        public virtual string P_AggregationS_Title()
        {
            return "Аггрегация";
        }
        public virtual string P_AggregationS_ControlType()
        {
            return typeof(UIText).Name;
        }
        public virtual bool P_AggregationS_Exists()
        {
            return this.P_Group_Exists();
        }
        public virtual bool P_AggregationS_Editable()
        {
            return false;
        }
        #endregion
        #region AggCmlS
        public virtual string P_AggCmlS
        {
            get
            {

                var s =  GetAttrValue(TextConst.AName.AggCml); 
                if (s == "")
                {
                    s = P_AggregationS;
                }
                return s;
            }

        }

        public virtual string P_AggCmlS_Title()
        {

            return "Аггрегация для нарастающего итога";

        }

        public virtual string P_AggCmlS_ControlType()
        {

            return typeof(UIText).Name;

        }

        public virtual bool P_AggCmlS_Exists()
        {


            return P_Group_Exists();

        }


        public virtual bool P_AggCmlS_Editable()
        {
            return false;
        }

        #endregion
        #region Fact
        public virtual string P_Fact
        {
            get
            {
               // return GetAttrValue(TextConst.AName.Fact);
                var s = P_FactName;
                if (s != "")
                {
                    return s;
                }
                if (P_PrFact == TextConst.AVBool.True)
                {
                    return GetMainParent().P_IdName + "_" + XName;
                }
                return "";
               
            }
            

        }

        public virtual string P_Fact_Title()
        {

            return "Факт";

        }

        public virtual string P_Fact_ControlType()
        {

            return typeof(UIText).Name;

        }

        public virtual bool P_Fact_Exists()
        {

            return (GetParent() is VSelect || GetParent() is VPart);

        }
        public virtual bool P_Fact_Editable()
        {

            return false;

        }

        public virtual string P_Fact_FieldGroup()
        {

            return TextConst.SchEdirorFieldGr.Cube;

        }
        #endregion
        #region FactName
        public virtual string P_FactName {
            get {
                return this.AttrOrEmpty(AName_.fact);
            }
            set {
                this.SetAttributeNotEmpty(AName_.fact, value);
            }
        }
        public virtual string P_FactName_Title()
        {
            return "Факт";
        }
        public virtual string P_FactName_ControlType()
        {
            return typeof(UIText).Name;
        }
        public virtual bool P_FactName_Exists()
        {
            VSXElement parent = this.GetParent();
            return (parent is VSelect) || (parent is VPart);
        }
        public virtual bool P_FactName_Editable()
        {
            return true;
        }
        public virtual string P_FactName_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.Cube;
        }
        #endregion
        #region Dimension
        public virtual string P_Dimension {
            get {
                return this.AttrOrEmpty(AName_.dimension);
            }
            set {
                if (value != string.Empty && this.AttrOrEmpty(AName_.dimension) != value) {
                    this.SetAttributeNotEmpty(AName_.is_private_dimension, value);
                    this.SetAttributeNotEmpty(AName_.is_final_dimension, value);
                }
                this.SetAttributeNotEmpty(AName_.dimension, value);                
            }
        }
        public virtual string P_Dimension_Title()
        {
            return "Измерение";
        }
        public virtual string P_Dimension_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual void P_Dimension_List(VDataTable table)
        {
            table.AddColumn("id");
            table.AddColumn("name", "Имя");
            table.AddColumn("table", "Объект");
            table.AddColumn("time_line", "Временная линия");
        }
        public virtual void P_Dimension_ListRefresh(VDataTable table)
        {
            table.Rows.Clear();
            IList<VDimension> list = XmlReports.Environment.GetDimensions();
            for (int index = 0; index < list.Count; index++) {
                VDimension dim = list[index];
                string dim_name = dim.P_Name;
                if (dim.P_TimeType != string.Empty) {
                    table.AddRow(dim_name, dim_name, null, dim.P_Timeline);
                } else {
                    string queryName;
                    VQuery qry = dim.Query();
                    if (qry != null) {
                        queryName = qry.P_Name;
                    } else {
                        queryName = "[missing]";
                    }
                    table.AddRow(dim_name, dim_name, queryName, null);
                }
            }
        }
        public virtual bool P_Dimension_Exists()
        {
            return this.GetParent() is VSelect;
        }
        public virtual bool P_Dimension_Editable()
        {
            return this.GetParent() is VSelect;
        }
        public virtual string P_Dimension_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.Cube;
        }
        #endregion
        #region FactDimension
        public virtual string P_FactDimension {
            get {
                return this.AttrOrEmpty(AName_.fact_dimension);
            }
            set {
                this.SetAttributeNotEmpty(AName_.fact_dimension, value);
            }
        }
        public virtual string P_FactDimension_Title()
        {
            return "Факт возвращает измерение";
        }
        public virtual string P_FactDimension_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual void P_FactDimension_List(VDataTable table)
        {
            this.P_Dimension_List(table);
        }
        public virtual void P_FactDimension_ListRefresh(VDataTable table)
        {
            this.P_Dimension_ListRefresh(table);
        }
        public virtual bool P_FactDimension_Exists()
        {
            return this.P_Fact_Exists();
        }
        public virtual string P_FactDimension_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.Cube;
        }
        #endregion
        #region Aggregation
        public virtual string P_Aggregation {
            get {
                return this.AttrOrEmpty(AName_.agg);
            }
            set {
                this.SetAttributeNotEmpty(AName_.agg, value);
            }
        }
        public virtual string P_Aggregation_Title()
        {
            return "Агрегация";
        }
        public virtual string P_Aggregation_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual void P_Aggregation_List(VDataTable table)
        {
            table.AddColumn("id");
            table.AddColumn("name", "Имя");
        }
        public virtual void P_Aggregation_ListRefresh(VDataTable table)
        {
            FillDataTableFromStringArray(table, TextConst.AVAggArray.ForAgg);
        }
        public virtual bool P_Aggregation_Exists()
        {
            return this.P_Fact_Exists();
        }
        public virtual string P_Aggregation_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.Cube;
        }
        #endregion
        #region AggCml
        public virtual string P_AggCml {
            get {
                return this.GetAttrValue(TextConst.AName.AggCml);
            }
            set {
                this.SetAttributeNotEmpty(TextConst.AName.AggCml, value);
            }
        }
        public virtual string P_AggCml_Title()
        {
            return "Агрегация (нарастающий итог)";
        }
        public virtual string P_AggCml_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual void P_AggCml_List(VDataTable table)
        {
            table.AddColumn("id");
            table.AddColumn("name", "Имя");
        }
        public virtual void P_AggCml_ListRefresh(VDataTable table)
        {
            FillDataTableFromStringArray(table, TextConst.AVAggArray.ForAggCml);
        }
        public virtual bool P_AggCml_Exists()
        {
            return P_Fact_Exists() ;
        }
        public virtual string P_AggCml_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.Cube;
        }
        #endregion
        //#region KeyDimension
        //public virtual string P_KeyDimension
        //{
        //    get
        //    {
        //        return GetAttrValue(TextConst.AName.KeyDimension);
        //    }
        //    set
        //    {
        //        SetAttributeNotEmpty(TextConst.AName.KeyDimension, value);
        //    }
        //}
        //public virtual string P_KeyDimension_Title()
        //{
        //    return "Ключевое измерение";
        //}
        //public virtual string P_KeyDimension_ControlType()
        //{
        //    return typeof(UIText).Name;
        //}
        //public virtual bool P_KeyDimension_Exists()
        //{
        //    return (GetParent() is VSelect);
        //}
        //public virtual string P_KeyDimension_FieldGroup()
        //{
        //    return TextConst.SchEdirorFieldGr.Qube;
        //}
        //#endregion
        #region PrDimension (VQueryCall)
        public virtual string P_PrDimension {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }
        public virtual string P_PrDimension_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual string P_PrDimension_Title()
        {
            return "Является измерением";
        }
        public virtual string P_PrDimension_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.Cube;
        }
        public virtual bool P_PrDimension_Exists()
        {
            return false;
        }
        #endregion
        #region PrFact
        public virtual string P_PrFact {
            get {
                if (!string.IsNullOrEmpty(this.P_FactName)) {
                    return TextConst.AVBool.True;
                } else {
                    return this.AttrOrEmpty(AName_.is_fact);
                }
            }
            set {
                this.SetAttributeNotEmpty(AName_.is_fact, value);
            }
        }
        public virtual string P_PrFact_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual string P_PrFact_Title()
        {
            return "Является фактом";
        }
        public virtual string P_PrFact_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.Cube;
        }
        public virtual bool P_PrFact_Exists()
        {
            return this.P_Fact_Exists();
        }
        public virtual bool P_PrFact_Editable()
        {
            return this.P_Fact != TextConst.AVBool.True;
        }
        #endregion
        #region IsFinalDimension
        public virtual string P_IsFinalDimension {
            get {
                if (!string.IsNullOrEmpty(this.AttrOrEmpty(AName_.is_final_dimension))) {
                    return TextConst.AVBool.True;
                } else {
                    return string.Empty;
                }
            }
            set {
                this.SetAttributeNotEmpty(AName_.is_final_dimension, value);
            }
        }
        public virtual string P_IsFinalDimension_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual string P_IsFinalDimension_Title()
        {
            return "Не искать измерения дальше";
        }
        public virtual string P_IsFinalDimension_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.Cube;
        }
        public virtual bool P_IsFinalDimension_Exists()
        {
            return this.P_Dimension_Exists() || this.P_PrDimension_Exists();
        }
        #endregion
        #region IsPrivateDimension
        public virtual string P_IsPrivateDimension {
            get {
                if (!string.IsNullOrEmpty(this.AttrOrEmpty(AName_.is_private_dimension))) {
                    return TextConst.AVBool.True;
                } else {
                    return string.Empty;
                }
            }
            set {
                this.SetAttributeNotEmpty(AName_.is_private_dimension, value);
            }
        }
        public virtual string P_IsPrivateDimension_ControlType()
        {

            return typeof(UICheck).Name;

        }

        public virtual string P_IsPrivateDimension_Title()
        {

            return "Приватное";

        }
        public virtual string P_IsPrivateDimension_FieldGroup()
        {

            return TextConst.SchEdirorFieldGr.Cube;

        }
        public virtual bool P_IsPrivateDimension_Exists()
        {

            return P_Dimension_Exists() || P_PrDimension_Exists();

        }
        #endregion
        //#region NameFieldName
        //public virtual string P_NameFieldName
        //{
        //    get
        //    {
        //        return GetAttrValue(TextConst.AName.NameFieldName);
        //    }
        //    set
        //    {
        //        SetAttributeNotEmpty(TextConst.AName.NameFieldName, value);
        //    }
        //}


        //public virtual string P_NameFieldName_Title()
        //{

        //    return "Колонка c именем объекта";

        //}

        //public virtual string P_NameFieldName_ControlType()
        //{

        //    return typeof(UICombo).Name;

        //}

        //public virtual void P_NameFieldName_List(VDataTable table)
        //{

        //    table.AddColumn("id");
        //    table.AddColumn("name", "Имя");
        //    table.AddColumn("title", "Заголовок");
        //}

        //public virtual void P_NameFieldName_ListRefresh(VDataTable table)
        //{
        //    table.Rows.Clear();

        //}
        //public virtual bool P_NameFieldName_Exists()
        //{

        //    return false;

        //}
        //#endregion
        #region Name
        public virtual string P_Name {
            get {
                return this.AttrOrEmpty(AName_.name);
            }
            set {
                this.SetAttributeNotEmpty(AName_.name, value);
            }
        }
        public virtual string P_Name_Title()
        {
            return "Имя";
        }
        public virtual bool P_Name_Exists()
        {
            return false;
        }
        public virtual int P_Name_Order()
        {
            return 200;
        }
        public virtual string P_Name_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.MainMain;
        }
        #endregion
        #region Rgb
        public virtual string P_Rgb {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }
        public virtual string P_Rgb_Title()
        {
            return "Цвет(R,G,B)";
        }
        public virtual bool P_Rgb_Exists()
        {
            return false;
        }
        #endregion
        #region Format
        public virtual string P_Format {
            get {
                return this.AttrOrEmpty(AName_.format);
            }
            set {
                this.SetAttributeNotEmpty(AName_.format, value);
            }
        }
        public virtual string P_Format_Title()
        {
            return "Формат";
        }
        public virtual bool P_Format_Exists()
        {
            return false;
        }
        #endregion
        #region ButtonType (VUICommand)
        public virtual string P_ButtonType {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }
        public virtual string P_ButtonType_Title()
        {
            return "Тип кнопки";
        }
        public virtual string P_ButtonType_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual bool P_ButtonType_Exists()
        {
            return false;
        }
        #endregion
        #region ShowToolBar
        public virtual string P_ShowToolBar {
            get {
                return this.AttrOrEmpty(AName_.show_toolbar);
            }
            set {
                this.SetAttributeNotEmpty(AName_.show_toolbar, value);
            }
        }
        public virtual string P_ShowToolBar_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual string P_ShowToolBar_Title()
        {
            return "Toolbar";
        }
        public virtual bool P_ShowToolBar_Exists()
        {
            return false;
        }
        #endregion
        #region ShowBottomToolBar
        public virtual string P_ShowBottomToolBar {
            get {
                return this.AttrOrEmpty(AName_.show_bottom_toolbar);
            }
            set {
                this.SetAttributeNotEmpty(AName_.show_bottom_toolbar, value);
            }
        }
        public virtual string P_ShowBottomToolBar_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual string P_ShowBottomToolBar_Title()
        {
            return "Нижний Toolbar";
        }
        public virtual bool P_ShowBottomToolBar_Exists()
        {
            return false;
        }
        #endregion
        #region ShowFooter
        public virtual string P_ShowFooter {
            get {
                return this.AttrOrEmpty(AName_.show_footer);
            }
            set {
                this.SetAttributeNotEmpty(AName_.show_footer, value);
            }
        }
        public virtual string P_ShowFooter_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual string P_ShowFooter_Title()
        {
            return "Итоговая строка";
        }
        public virtual bool P_ShowFooter_Exists()
        {
            return false;
        }
        #endregion
        #region ShowAggPanel
        public virtual string P_ShowAggPanel {
            get {
                return this.AttrOrEmpty(AName_.show_agg_panel);
            }
            set {
                this.SetAttributeNotEmpty(AName_.show_agg_panel, value);
            }
        }
        public virtual string P_ShowAggPanel_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual string P_ShowAggPanel_Title()
        {
            return "Панель агрегации выделенных значений";
        }
        public virtual bool P_ShowAggPanel_Exists()
        {
            return P_ShowFooter_Exists();
        }
        #endregion
        //#region ListQuery
        //public virtual string P_ListQuery
        //{
        //    get
        //    {
        //        return GetAttrValue(TextConst.AName.Listquery);
        //    }
        //    set
        //    {
        //        SetAttribute(TextConst.AName.Listquery, value);
        //    }
        //}


        //public virtual string P_ListQuery_Title()
        //{

        //    return "Запрос для выбора значений";

        //}

        //public virtual string P_ListQuery_ControlType()
        //{

        //    return typeof(UICombo).Name;

        //}

        //public virtual void P_ListQuery_List(VDataTable table)
        //{

        //    P_ColumnEditable_List(table);
        //}


        //public virtual void P_ListQuery_ListRefresh(VDataTable table)
        //{
        //    P_ColumnEditable_ListRefresh(table);

        //}

        //public virtual bool P_ListQuery_Exists()
        //{

        //    return P_ColumnMandatory_Exists();

        //}
        //#endregion
        #region DeleteValidation
        public virtual string P_DeleteValidation {
            get {
                return this.AttrOrEmpty(AName_.delete_validation);
            }
            set {
                this.SetAttributeValue(AName_.delete_validation, value);
            }
        }
        public virtual string P_DeleteValidation_Title()
        {
            return "Валидация удаления (объект)";
        }
        public virtual string P_DeleteValidation_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.BehaviorValid;
        }
        public virtual string P_DeleteValidation_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual void P_DeleteValidation_List(VDataTable table)
        {
            this.P_Editable_List(table);
        }
        public virtual void P_DeleteValidation_ListRefresh(VDataTable table)
        {
            this.P_Editable_ListRefresh(table);
        }
        public virtual bool P_DeleteValidation_Exists()
        {
            return false;
        }
        #endregion
        #region DataType
        public virtual string P_DataTypeS
        {
            get
            {
                return XDataType();
            }


        }

        public virtual string P_DataTypeS_Title()
        {

            return "Тип данных (вычисляемый)";

        }

        public virtual string P_DataTypeS_ControlType()
        {

            return typeof(UIText).Name;

        }

        public virtual bool P_DataTypeS_Exists()
        {
            return false;
        }
        public virtual bool P_DataTypeS_Editable()
        {
            
            return false;

        }

        public virtual string P_DataTypeS_FieldGroup()
        {

            return P_DataType_FieldGroup();

        }
        #endregion
        #region DataType
        public virtual string P_DataType {
            get {
                return this.AttrOrEmpty(AName_.type);
            }
            set {
                this.SetAttributeValue(AName_.type, value);
            }
        }
        public virtual string P_DataType_Title()
        {
            return "Тип данных";
        }
        public virtual string P_DataType_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual void P_DataType_List(VDataTable table)
        {
            table.AddColumn("id");
            table.AddColumn("name");
        }
        public virtual void P_DataType_ListRefresh(VDataTable table)
        {
            FillDataTableFromStringArray(table, TextConst.AVTypeArray.Real);
        }
        public virtual bool P_DataType_Exists()
        {
            return false;
        }
        public virtual string P_DataType_FieldGroup()
        {
            if (this.GetParent() is VOutputElement) {
                return TextConst.SchEdirorFieldGr.MainMain;
            } else {
                return TextConst.SchEdirorFieldGr.MainOther;
            }
        }
        #endregion
        #region Control
        public virtual string P_Control {
            get {
                return this.AttrOrEmpty(AName_.control);
            }
            set {
                this.SetAttributeValue(AName_.control, value);
            }
        }
        public virtual string P_Control_Title()
        {
            return "Имя элемента управления";
        }
        public virtual string P_Control_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual void P_Control_List(VDataTable table)
        {
            table.AddColumn("id");
            table.AddColumn("name");
        }
        public virtual void P_Control_ListRefresh(VDataTable table)
        {
            table.Rows.Clear();
        }
        public virtual bool P_Control_Exists()
        {
            return false;
        }
        #endregion
        #region EditorButtonType
        public virtual string P_EditorButtonType {
            get {
                return this.AttrOrEmpty(AName_.type);
            }
            set {
                this.SetAttributeValue(AName_.type, value);
            }
        }
        public virtual string P_EditorButtonType_Title()
        {
            return "Вид кнопки";
        }
        public virtual string P_EditorButtonType_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual void P_EditorButtonType_List(VDataTable table)
        {
            table.AddColumn("id");
            table.AddColumn("name");
        }
        public virtual void P_EditorButtonType_ListRefresh(VDataTable table)
        {
            //FillDataTableFromStringArray(table, Enum.GetNames(typeof(ButtonPredefines)));
        }
        public virtual bool P_EditorButtonType_Exists()
        {
            return false;
        }
        #endregion
        #region EditorButtonSide
        public virtual string P_EditorButtonSide {
            get {
                return this.AttrOrEmpty(AName_.side);
            }
            set {
                this.SetAttributeValue(AName_.side, value);
            }
        }
        public virtual string P_EditorButtonSide_Title()
        {
            return "Сторона";
        }
        public virtual string P_EditorButtonSide_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual void P_EditorButtonSide_List(VDataTable table)
        {
            table.AddColumn("id");
            table.AddColumn("name");
        }
        public virtual void P_EditorButtonSide_ListRefresh(VDataTable table)
        {
            FillDataTableFromStringArray(table, TextConst.AVSidesArray.All);
        }
        public virtual bool P_EditorButtonSide_Exists()
        {
            return false;
        }
        #endregion
        #region EventName
        public virtual string P_EventName {
            get {
                return this.AttrOrEmpty(AName_.event_name);
            }
            set {
                this.SetAttributeValue(AName_.event_name, value);
            }
        }
        public virtual string P_EventName_Title()
        {
            return "Событие";
        }
        public virtual string P_EventName_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual bool P_EventName_Exists()
        {
            return false;
        }
        #endregion
        #region Modal
        public virtual string P_Modal
        {
            get
            {
                return GetAttrValue(TextConst.AName.Modal);
            }
            set
            {
                SetAttributeNotEmpty(TextConst.AName.Modal, value);

            }

        }

        public virtual string P_Modal_ControlType()
        {

            return typeof(UICheck).Name;

        }

        public virtual string P_Modal_Title()
        {

            return "Модальное окно";

        }

        public virtual bool P_Modal_Exists()
        {

            return false;

        }
        #endregion
        #region UseParentDsId
        public virtual string P_UseParentDsId
        {
            get
            {
                return GetAttrValue(TextConst.AName.UseParentDsId);
            }
            set
            {
                SetAttributeNotEmpty(TextConst.AName.UseParentDsId, value);

            }

        }

        public virtual string P_UseParentDsId_ControlType()
        {

            return typeof(UICheck).Name;

        }

        public virtual string P_UseParentDsId_Title()
        {

            return "Исп. значения главн. формы";

        }

        public virtual bool P_UseParentDsId_Exists()
        {

            return P_Modal_Exists();

        }
        #endregion
        #region ShowCheckbox
        public virtual string P_ShowCheckbox {
            get {
                return this.AttrOrEmpty(AName_.show_checkbox);
            }
            set {
                this.SetAttributeNotEmpty(AName_.show_checkbox, value);
            }
        }
        public virtual string P_ShowCheckbox_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual string P_ShowCheckbox_Title()
        {
            return "Колонка выбора";
        }
        public virtual bool P_ShowCheckbox_Exists()
        {
            return false;
        }
        #endregion
        #region UpdateTargetS
        public virtual string P_UpdateTargetS {
            get {
                string update_target = this.P_UpdateTarget;
                if (!string.IsNullOrEmpty(update_target)) {
                    return update_target;
                } else {
                    VAction act = this as VAction;
                    if (act != null) {
                        VQuery query = act.CalledQuery();
                        if (query != null) {
                            return query.P_UpdateTarget;
                        }
                    }
                }
                return string.Empty;
            }
        }
        public virtual string P_UpdateTargetS_Title()
        {
            return "Меняет таблицу()";
        }
        public virtual bool P_UpdateTargetS_Exists()
        {
            return false;
        }
        public virtual bool P_UpdateTargetS_Editable()
        {
            return false;
        }
        #endregion
        #region TimeType
        public virtual string P_TimeType {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }
        public virtual string P_TimeType_Title()
        {
            return "Специальный тип";
        }
        public virtual string P_TimeType_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual bool P_TimeType_Exists()
        {
            return false;
        }
        #endregion
        #region Cumulate
        public virtual string P_Cumulate
        {
            get
            {
                return GetAttrValue(TextConst.AName.Cumulate);
            }
            set
            {
                SetAttribute(TextConst.AName.Cumulate, value);
            }
        }


        public virtual string P_Cumulate_Title()
        {

            return "Нарастающий итог по";

        }

        public virtual string P_Cumulate_ControlType()
        {

            return typeof(UICombo).Name;

        }

        public virtual void P_Cumulate_List(VDataTable table)
        {

            table.AddColumn("id");
            table.AddColumn("name");
        }


        public virtual void P_Cumulate_ListRefresh(VDataTable table)
        {
            table.Rows.Clear();
        }
        public virtual bool P_Cumulate_Exists()
        {

            return false;

        }
        #endregion
        #region Timeline (VDimension)
        public virtual string P_Timeline {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }
        public virtual string P_Timeline_Title()
        {
            return "Временная линия";
        }
        public virtual bool P_Timeline_Exists()
        {
            return false;
        }
        #endregion
        #region IsRet
        public virtual string P_IsRet
        {
            get {
                return this.AttrOrEmpty(AName_.is_ret);
            }
            set {
                this.SetAttributeNotEmpty(AName_.is_ret, value);
            }
        }
        public virtual string P_IsRet_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual string P_IsRet_Title()
        {
            return "Возвращаемый";
        }
        public virtual bool P_IsRet_Exists()
        {
            return false;
        }
        #endregion
        #region Checked
        public virtual string P_Checked {
            get {
                return this.AttrOrEmpty(AName_.@checked);
            }
            set {
                this.SetAttributeNotEmpty(AName_.@checked, value);
            }
        }
        public virtual string P_Checked_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.BehaviorDefault;
        }
        public virtual string P_Checked_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual string P_Checked_Title()
        {
            return "Параметр установлен по умолчанию";
        }
        public virtual bool P_Checked_Exists()
       {
            return false;
        }
        #endregion
        #region WriteAccess (VUseObject)
        public virtual string P_WriteAccess {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }
        public virtual string P_WriteAccess_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual string P_WriteAccess_Title()
        {
            return "Запись";
        }
        public virtual bool P_WriteAccess_Exists()
        {
            return false;
        }
        #endregion
        #region SpecTable
        public virtual string P_SpecTable
        {
            get
            {
                return GetAttrValue(TextConst.AName.SpecTable);
            }
            set
            {
                SetAttribute(TextConst.AName.SpecTable, value);
            }
        }


        public virtual string P_SpecTable_Title()
        {

            return "Специальная таблица";

        }

        public virtual string P_SpecTable_ControlType()
        {

            return typeof(UICombo).Name;

        }

        public virtual void P_SpecTable_List(VDataTable table)
        {


            table.AddColumn("id");
            table.AddColumn("name", "Имя");
        }


        public virtual void P_SpecTable_ListRefresh(VDataTable table)
        {
            

        }
        public virtual bool P_SpecTable_Exists()
        {

            return false;

        }
        #endregion
        #region SpecColumn
        public virtual string P_SpecColumn
        {
            get
            {
                return GetAttrValue(TextConst.AName.SpecColumn);
            }
            set
            {
                SetAttribute(TextConst.AName.SpecColumn, value);
            }
        }


        public virtual string P_SpecColumn_Title()
        {

            return "Специальная колонка";

        }

        public virtual string P_SpecColumn_ControlType()
        {

            return typeof(UICombo).Name;

        }

        public virtual void P_SpecColumn_List(VDataTable table)
        {


            table.AddColumn("id");
            table.AddColumn("name", "Имя");
        }


        public virtual void P_SpecColumn_ListRefresh(VDataTable table)
        {


        }
        public virtual bool P_SpecColumn_Exists()
        {

            return false;

        }
        #endregion
        #region AutoMerge (VReport и VQuery)
        public virtual string P_AutoMerge {
            get {
                return this.AttrOrEmpty(AName_.auto_merge);
            }
            set {
                this.SetAttributeNotEmpty(AName_.auto_merge, value);
            }
        }
        public virtual string P_AutoMerge_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual string P_AutoMerge_Title()
        {
            return "Объединение ячеек";
        }
        public virtual bool P_AutoMerge_Exists()
        {
            return false;
        }
        #endregion
        #region NoGrid (VReport и VQuery)
        public virtual string P_NoGrid {
            get {
                return this.AttrOrEmpty(AName_.nogrid);
            }
            set {
                this.SetAttributeNotEmpty(AName_.nogrid, value);
            }
        }
        public virtual string P_NoGrid_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual string P_NoGrid_Title()
        {
            return "Не показывать грид";
        }
        public virtual bool P_NoGrid_Exists()
        {
            return false;
        }
        #endregion
        #region SaveCompiled (VReport и VQuery)
        public virtual string P_SaveCompiled {
            get {
                return this.AttrOrEmpty(AName_.save_compiled);
            }
            set {
                this.SetAttributeNotEmpty(AName_.save_compiled, value);
            }
        }
        public virtual string P_SaveCompiled_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual string P_SaveCompiled_Title()
        {
            return "Сохранить собранную модель в проекте";
        }
        public virtual bool P_SaveCompiled_Exists()
        {
            return false;
        }
        #endregion
        #region CanUseSimpleParams (VQuery only)
        public virtual string P_CanUseSimpleParams
        {
            get
            {
                return GetAttrValue(TextConst.AName.CanUseSimpleParams);
            }
            set
            {
                SetAttributeNotEmpty(TextConst.AName.CanUseSimpleParams, value);

            }

        }

        public virtual string P_CanUseSimpleParams_ControlType()
        {

            return typeof(UICheck).Name;

        }

        public virtual string P_CanUseSimpleParams_Title()
        {

            return "Типизированные параметры";

        }

        public virtual bool P_CanUseSimpleParams_Exists()
        {

            return false;

        }
        #endregion
        #region PartId
        public virtual string P_PartId {
            get {
                return this.AttrOrEmpty(AName_.part_id);
            }
            set {
                this.SetAttributeNotEmpty(AName_.part_id, value);
            }
        }
        public virtual string P_PartId_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.Part;
        }
        public virtual string P_PartId_Title()
        {
            return "Part Id";
        }
        public virtual string P_PartId_ControlType()
        {
            return typeof(UIText).Name;
        }
        public virtual bool P_PartId_Exists()
        {
            return true;
        }
        #endregion
        #region NewRowsVisForOtherTbls (VQueryCall)
        public virtual string P_NewRowsVisForOtherTbls {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }
        public virtual string P_NewRowsVisForOtherTbls_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual string P_NewRowsVisForOtherTbls_Title()
        {
            return "Нов. строки видны для др. таблиц";
        }
        public virtual bool P_NewRowsVisForOtherTbls_Exists()
        {
            return false;
        }
        #endregion
        #region Async (VQueryCall)
        public virtual string P_Async {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }
        public virtual string P_Async_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual string P_Async_Title()
        {
            return "Асинхронное выполнение";
        }
        public virtual bool P_Async_Exists()
        {
            return false;
        }
        #endregion
        #region AutoRefresh (VQueryCall)
        public virtual string P_AutoRefresh {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }
        public virtual string P_AutoRefresh_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual string P_AutoRefresh_Title()
        {
            return "Автообновление при изменении переменных (Форма - при активации)";
        }
        public virtual bool P_AutoRefresh_Exists()
        {
            return false;
        }
        #endregion
        #region OnlyVisibleRefresh (VQueryCall)
        public virtual string P_OnlyVisibleRefresh {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }
        public virtual string P_OnlyVisibleRefresh_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual string P_OnlyVisibleRefresh_Title()
        {
            return "Обновлять только видимую";
        }
        public virtual bool P_OnlyVisibleRefresh_Exists()
        {
            return false;
        }
        #endregion
        #region OnlyForceRefresh (VQueryCall)
        public virtual string P_OnlyForceRefresh {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }
        public virtual string P_OnlyForceRefresh_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual string P_OnlyForceRefresh_Title()
        {
            return "Обновлять только по команде";
        }
        public virtual bool P_OnlyForceRefresh_Exists()
        {
            return false;
        }
        #endregion
        #region ActionRows
        public virtual string P_ActionRows {
            get {
                return GetAttrValue(TextConst.AName.ActionRows);
            }
            set {
                SetAttributeNotEmpty(TextConst.AName.ActionRows,value);
            }
        }
        public virtual string P_ActionRows_Title()
        {
            return "Применить к строкам";
        }
        public virtual string P_ActionRows_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual void P_ActionRows_List(VDataTable table)
        {
            table.AddColumn("id");
            table.AddColumn("name");
        }
        public virtual void P_ActionRows_ListRefresh(VDataTable table)
        {
            FillDataTableFromStringArray(table, TextConst.AVActionRowsArray.All);
        }
        public virtual bool P_ActionRows_Exists()
        {
            return false;
        }
        #endregion
        #region IsForm
        public virtual string P_IsForm {
            get {
                return this.AttrOrEmpty(AName_.is_form);
            }
            set {
                this.SetAttributeNotEmpty(AName_.is_form, value);
            }
        }
        public virtual string P_IsForm_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual string P_IsForm_Title()
        {
            return "Дочерняя форма";
        }
        public virtual bool P_IsForm_Exists()
        {
            return false;
        }
        #endregion
        #region NoBorder
        public virtual string P_NoBorder {
            get {
                return this.AttrOrEmpty(AName_.noborder);
            }
            set {
                this.SetAttributeNotEmpty(AName_.noborder, value);
            }
        }
        public virtual string P_NoBorder_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual string P_NoBorder_Title()
        {
            return "Невидимые границы";
        }
        public virtual bool P_NoBorder_Exists()
        {
            return false;
        }
        #endregion
        #region IsLayoutBlock
        public virtual string P_IsLayoutBlock {
            get {
                string val = this.AttrOrEmpty(AName_.is_layout_block);
                if (val == TextConst.AVBool.True || val == TextConst.AVBool.False || this.P_NoBorder == TextConst.AVBool.True) {
                    return val;
                } else {
                    return TextConst.AVBool.True;
                }
            }
            set {
                if (this.P_NoBorder == TextConst.AVBool.True || value == TextConst.AVBool.True) {
                    this.SetAttributeNotEmpty(AName_.is_layout_block, value);
                } else {
                    this.SetAttributeNotEmpty(AName_.is_layout_block, TextConst.AVBool.False);
                }
            }
        }
        public virtual string P_IsLayoutBlock_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual string P_IsLayoutBlock_Title()
        {
            return "Блок синхронизации размеров";
        }
        public virtual bool P_IsLayoutBlock_Exists()
        {
            return P_NoBorder_Exists() ;
        }
        public virtual string P_IsLayoutBlock_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.Layout;
        }
        #endregion
        #region IsVertical
        public virtual string P_IsVertical {
            get {
                return this.AttrOrEmpty(AName_.is_vertical);
            }
            set {
                this.SetAttributeNotEmpty(AName_.is_vertical, value);
            }
        }
        public virtual string P_IsVertical_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual string P_IsVertical_Title()
        {
            return "Вертикальный";
        }
        public virtual bool P_IsVertical_Exists()
        {
            return false;
        }
        #endregion
        #region FormalParamInfo
        public bool IsUnderParametrizedCall()
        {
            VSXElement parent = this.GetParent();
            return (parent is VAction) || (parent is VUseAction);
        }
        public virtual List<VParam> GetCalledElementFormalParams()
        {
            return null;
        }
        private VParam GetCalledElementFormalParamByIndex(int index)
        {
            IList<VParam> pars = this.GetCalledElementFormalParams();
            if (pars == null || index >= pars.Count) {
                return null;
            } else {
                return pars[index];
            }
        }
        public virtual string P_FormalParamInfo {
            get {
                int index = this.ElementsBeforeSelf().Count();
                VParam fpar = this.GetParent().GetCalledElementFormalParamByIndex(index);
                if (fpar != null) {
                    return fpar.P_Name;
                } else {
                    return string.Empty;
                }
            }
        }
        public virtual string P_FormalParamInfo_Title()
        {
            return "Имя формального параметра";
        }
        public virtual string P_FormalParamInfo_ControlType()
        {
            return typeof(UIText).Name;
        }
        public virtual bool P_FormalParamInfo_Exists()
        {
            return this.IsUnderParametrizedCall();
        }
        public virtual bool P_FormalParamInfo_Editable()
        {
            return false;
        }
        #endregion
        #region Text
        public virtual string P_Text {
            get {
                XElement txtNode = this.Element(EName.text);
                if (txtNode != null) {
                    return txtNode.Value;
                } else {
                    return string.Empty;
                }
            }
            set {
                XElement txtNode = this.Element(EName.text);
                if (txtNode == null) {
                    this.Add(new XElement(EName.text, value));
                } else {
                    txtNode.Value = value;
                }
            }
        }
        public virtual string P_Text_Title()
        {
            return "Текст";
        }
        public virtual string P_Text_ControlType()
        {
            throw new NotImplementedException();
            //return typeof(UITextEx).Name;
        }
        public virtual bool P_Text_Exists()
        {
            return false;
        }
        public virtual string P_Text_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.MainOther;
        }
        #endregion
        #region MultiSelect (VReport, VQuery и VGrid)
        public virtual string P_MultiSelect {
            get {
                string value = this.AttrOrDefault(AName_.multi_select, TextConst.AVBool.True);
                if (value == TextConst.AVBool.False) {
                    value = string.Empty;
                }
                return value;
            }
            set {
                if (string.IsNullOrEmpty(value)) {
                    value = TextConst.AVBool.False;
                } else if (value == TextConst.AVBool.True) {
                    value = null;
                }
                this.SetAttributeValue(AName_.multi_select, value);
            }
        }
        public virtual string P_MultiSelect_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual string P_MultiSelect_Title()
        {
            return "Множественный выбор";
        }
        public virtual bool P_MultiSelect_Exists()
        {
            return false;
        }
        #endregion
        #region NullIf
        public virtual string P_NullIf_Title()
        {
            return "Пусто если";
        }
        public virtual string P_NullIf {
            get {
                return this.AttrOrEmpty(AName_.nullif);
            }
            set {
                this.SetAttributeNotEmpty(AName_.nullif, value);
            }
        }
        public virtual bool P_NullIf_Exists()
        {
            return false;
        }
        #endregion
        #region Nvl
        public virtual string P_Nvl_Title()
        {
            return "Если пусто, то";
        }
        public virtual string P_Nvl
        {
            get {
                return this.AttrOrEmpty(AName_.nvl);
            }
            set {
                this.SetAttributeNotEmpty(AName_.nvl, value);
            }
        }
        public virtual bool P_Nvl_Exists()
        {
            return P_NullIf_Exists();
        }
        #endregion
        //#region Window
        //public virtual string P_Window
        //{
        //    get
        //    {
        //        return GetAttrValue(TextConst.AName.Window);
        //    }
        //    set
        //    {
        //        SetAttribute(TextConst.AName.Window, value);
        //    }
        //}
        //public virtual string P_Window_Title()
        //{
        //    return "Сумма по окну";
        //}
        //public virtual string P_Window_ControlType()
        //{
        //    return typeof(UICombo).Name;
        //}
        //public virtual void P_Window_List(VDataTable table)
        //{
        //    P_Editable_List(table);
        //}
        //public virtual void P_Window_ListRefresh(VDataTable table)
        //{
        //    P_Editable_ListRefresh(table);
        //}
        //public virtual bool P_Window_Exists()
        //{
        //    return false;
        //}
        //#endregion
        #region ParamType (VParam)
        public virtual string P_ParamType {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }
        public virtual string P_ParamType_Title()
        {
            return "Доп. логика параметра";
        }
        public virtual string P_ParamType_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual bool P_ParamType_Exists()
        {
            return false;
        }
        #endregion
        #region FixedSide
        public virtual string P_FixedSide {
            get {
                return this.AttrOrEmpty(AName_.fixed_side);
            }
            set {
                this.SetAttributeNotEmpty(AName_.fixed_side, value);
            }
        }
        public virtual string P_FixedSide_Title()
        {
            return "Фиксация";
        }
        public virtual string P_FixedSide_ControlType()
        {
            return typeof(UICombo).Name;
        }
        public virtual void P_FixedSide_List(VDataTable table)
        {
            table.AddColumn("id");
            table.AddColumn("name");
        }
        public virtual void P_FixedSide_ListRefresh(VDataTable table)
        {
            FillDataTableFromStringArray(table, TextConst.AVFixedSideArray.All);
        }
        public virtual bool P_FixedSide_Exists()
        {
            return false;
        }
        #endregion

        #region TableCode
        public virtual string P_TableCode
        {
            get
            {
                return GetAttrValue(TextConst.AName.TableCode);
            }
            set
            {
                SetAttributeNotEmpty(TextConst.AName.TableCode, value);
            }
        }

        public virtual string P_TableCode_Title()
        {
            return "Код в RK_IZM_TAB";
        }

        //public virtual string P_TableCode_ControlType()
        //{
        //    return typeof(UINumber).Name;
        //}

        public virtual bool P_TableCode_Exists()
        {
            return false;
        }
        #endregion
        #region Multiplicer
        public virtual string P_Multiplicer_Title()
        {
            return "Множитель 10 в степени";
        }
        public virtual string P_Multiplicer {
            get {
                return this.AttrOrEmpty(AName_.mp);
            }
            set {
                this.SetAttributeValue(AName_.mp, value);
            }
        }
        public virtual bool P_Multiplicer_Exists()
        {
            return false;
        }
        #endregion
        #region Colset
        public virtual string P_Colset_Title()
        {
            return "Группа колонок в отчете";
        }
        public virtual string P_Colset {
            get {
                return this.AttrOrEmpty(AName_.colset);
            }
            set {
                this.SetAttributeValue(AName_.colset, value);
            }
        }
        public virtual bool P_Colset_Exists()
        {
            return GetParent() is VOutputElement;
        }
        #endregion
        #region SecurityId
        public virtual string P_SecurityId_Title()
        {
            return "SecurityId";
        }
        public virtual string P_SecurityId {
            get {
                return this.AttrOrEmpty(AName_.security_id);
            }
            set {
                this.SetAttributeNotEmpty(AName_.security_id, value);
            }
        }
        public virtual bool P_SecurityId_Exists()
        {
            return false;
        }
        #endregion
        #region CustomControl
        public virtual string P_CustomControl {
            get {
                return this.AttrOrEmpty(AName_.type_name);
            }
            set {
                this.SetAttributeNotEmpty(AName_.type_name, value);
            }
        }
        public virtual string P_CustomControl_Title()
        {
            return "Контрол ICustom";
        }
        public virtual string P_CustomControl_ControlType()
        {
            throw new NotImplementedException();
            //return typeof(UITextEx).Name;
        }
        public virtual bool P_CustomControl_Exists()
        {
            return false;
        }
        public virtual bool P_CustomControl_Editable()
        {
            return P_CustomControl_Exists();
        }
        #endregion
        #region Order (VQueryCall, VQuery и VGrset)
        public virtual string P_Order {
            get {
                return this.AttrOrEmpty(AName_.order);
            }
            set {
                this.SetAttributeNotEmpty(AName_.order, value);
            }
        }
        public virtual string P_Order_ControlType()
        {
            return typeof(UIText).Name;
        }
        public virtual string P_Order_Title()
        {
            return "order by";
        }
        public virtual bool P_Order_Exists()
        {
            return false;
        }
        #endregion
        #region ShowNulls
        public virtual string P_ShowNulls {
            get {
                return this.AttrOrEmpty(AName_.show_nulls);
            }
            set {
                this.SetAttrValue(AName_.show_nulls, value);
            }
        }
        public virtual string P_ShowNulls_Title()
        {
            return "Наличие пустых значений";
        }
        public virtual string P_ShowNulls_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual bool P_ShowNulls_Exists()
        {
            return false;
        }
        #endregion
        #region ExpandAll
        public virtual string P_ExpandAll {
            get {
                return this.AttrOrEmpty(AName_.expand_all);
            }
            set {
                this.SetAttrValue(AName_.expand_all, value);
            }
        }
        public virtual string P_ExpandAll_Title()
        {
            return "Раскрывать узлы дерева";
        }
        public virtual string P_ExpandAll_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual bool P_ExpandAll_Exists()
        {
            return false;
        }
        #endregion
        #region AutoCheck
        public virtual string P_AutoCheck {
            get {
                return this.AttrOrEmpty(AName_.auto_check);
            }
            set {
                this.SetAttrValue(AName_.auto_check, value);
            }
        }
        public virtual string P_AutoCheck_Title()
        {
            return "С родительским помечать дочерние";
        }
        public virtual string P_AutoCheck_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual string P_AutoCheck_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.Behavior;
        }
        public virtual bool P_AutoCheck_Exists()
        {
            return false;
        }
        #endregion
        #region MergeDimsets

        public virtual string P_MergeDimsets_Title()
        {

            return "MergeDimsets";

        }

        public virtual string P_MergeDimsets
        {
            get
            {
                return GetAttrValue(TextConst.AName.MergeDimsets);
            }
            set
            {
                SetAttribute(TextConst.AName.MergeDimsets, value);
            }
        }
        public virtual string P_MergeDimsets_ControlType()
        {

            return typeof(UICheck).Name;

        }
       
        public virtual bool P_MergeDimsets_Exists()
        {

            return false;

        }

        #endregion


        #region StarScheme

        public virtual string P_StarScheme_Title()
        {

            return "Схема \"звезда\"";

        }

        public virtual string P_StarScheme
        {
            get
            {
                return GetAttrValue(TextConst.AName.StarScheme);
            }
            set
            {
                SetAttribute(TextConst.AName.StarScheme, value);
            }
        }
        public virtual string P_StarScheme_ControlType()
        {

            return typeof(UICheck).Name;

        }
       
        public virtual bool P_StarScheme_Exists()
        {

            return false;

        }

        #endregion


        #region SingleWay

        public virtual string P_SingleWay_Title()
        {

            return "Запретить неоднозначные связи";

        }

        public virtual string P_SingleWay
        {
            get
            {
                return GetAttrValue(TextConst.AName.SingleWay);
            }
            set
            {
                SetAttribute(TextConst.AName.SingleWay, value);
            }
        }
        public virtual string P_SingleWay_ControlType()
        {

            return typeof(UICheck).Name;

        }

        public virtual bool P_SingleWay_Exists()
        {

            return false;

        }

        #endregion

        #region DontPushpred
        public virtual string P_DontPushpred
        {
            get
            {
                return GetAttrValue(TextConst.AName.DontPushpred);
            }
            set
            {
                SetAttributeNotEmpty(TextConst.AName.DontPushpred, value);

            }

        }

        public virtual string P_DontPushpred_ControlType()
        {

            return typeof(UICheck).Name;

        }

        public virtual string P_DontPushpred_Title()
        {

            return "Не передавать предикат в подзапрос";

        }
        public virtual string P_DontPushpred_FieldGroup()
        {

            return TextConst.SchEdirorFieldGr.Cube;

        }
        public virtual bool P_DontPushpred_Exists()
        {

            return false;

        }
        #endregion
        #region DontPush
        public virtual string P_DontPush {
            get {
                return this.AttrOrEmpty(AName_.dont_push);
            }
            set {
                this.SetAttributeNotEmpty(AName_.dont_push, value);
            }
        }
        public virtual string P_DontPush_Title()
        {
            return "Не проталкивать условие в дочерние кубы";
        }
        public virtual string P_DontPush_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual bool P_DontPush_Exists()
        {
            return false;
        }
        #endregion
        #region Prompt (VAction и наследники: VUseAction и VUICommand)
        public virtual string P_Prompt {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }
        public virtual string P_Prompt_Title()
        {
            return "Предупреждение";
        }
        public virtual string P_Prompt_ControlType()
        {
            throw new NotImplementedException();
            //return typeof(UITextEx).Name;
        }
        public virtual bool P_Prompt_Exists()
        {
            return false;
        }
        public virtual bool P_Prompt_Editable()
        {
            return true;
        }
        #endregion
        #region Message (VAction и наследники: VUseAction и VUICommand)
        public virtual string P_Message {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }
        public virtual string P_Message_Title()
        {
            return "Сообщение";
        }
        public virtual string P_Message_ControlType()
        {
            throw new NotImplementedException();
            //return typeof(UITextEx).Name;
        }
        public virtual bool P_Message_Exists()
        {
            return false;
        }
        public virtual bool P_Message_Editable()
        {
            return true;
        }
        #endregion
        #region Notification (VAction и наследники: VUseAction и VUICommand)
        public virtual string P_Notification {
			get {
                throw new NotImplementedException();
			}
            set {
                throw new NotImplementedException();
            }
		}
		public virtual string P_Notification_Title()
		{
			return "Уведомление";
		}
		public virtual string P_Notification_ControlType()
		{
            throw new NotImplementedException();
            //return typeof(UITextEx).Name;
		}
		public virtual bool P_Notification_Exists()
		{
			return false;
		}
		public virtual bool P_Notification_Editable()
		{
			return true;
		}
		#endregion
		#region ClientCalulation
		public virtual string P_ClientCalulation_Title()
        {
            return "Вып. выч. на клиенте  (для выр. верхн. ур. и усл. в dimset и для table=не в базе.)";
        }
        public virtual string P_ClientCalulation {
            get {
                return this.AttrOrEmpty(AName_.client_calc);
            }
            set {
                this.SetAttributeValue(AName_.client_calc, value);
            }
        }
        public virtual string P_ClientCalulation_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual bool P_ClientCalulation_Exists()
        {
            return false;
        }
        #endregion
        #region ExcelCalulation

        public virtual string P_ExcelCalulation_Title()
        {

            return "Экспоритровать выражение в excel";

        }

        public virtual string P_ExcelCalulation
        {
            get
            {
                return GetAttrValue(TextConst.AName.ExcelCalulation);
            }
            set
            {
                SetAttribute(TextConst.AName.ExcelCalulation, value);
            }
        }
        public virtual string P_ExcelCalulation_ControlType()
        {

            return typeof(UICheck).Name;

        }

        public virtual bool P_ExcelCalulation_Exists()
        {

            return false;

        }
        #endregion
        #region ClientView
        public virtual string P_ClientView_Title()
        {
            return "Просмотр Excel";
        }
        public virtual string P_ClientView {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }
        public virtual string P_ClientView_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual bool P_ClientView_Exists()
        {
            return false;
        }
        #endregion
        #region PrintXlsx (VPrintTemplate)
        public virtual string P_PrintXlsx {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }
        public virtual string P_PrintXlsx_Title()
        {
            return "Печать через Xlsx";
        }
        public virtual string P_PrintXlsx_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual bool P_PrintXlsx_Exists()
        {
            return false;
        }
        #endregion
        #region UseFlexCel (VPrintTemplate)
        public virtual string P_UseFlexCel {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }
        public virtual string P_UseFlexCel_Title()
        {
            return "Обработка через FlexCel";
        }
        public virtual string P_UseFlexCel_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual bool P_UseFlexCel_Exists()
        {
            return false;
        }
        #endregion
		#region DetailsUseZeros 
		public virtual string P_DetailsUseZeros
		{
			get
			{
				return GetAttrValue(TextConst.AName.DetailsUseZeros);
			}
			set
			{
				SetAttributeNotEmpty(TextConst.AName.DetailsUseZeros, value);
			}
		}

		public virtual string P_DetailsUseZeros_Title()
		{
			return "Показывать строки с нулями";
		}

		public virtual string P_DetailsUseZeros_ControlType()
		{
			return typeof(UICheck).Name;
		}

		public virtual bool P_DetailsUseZeros_Exists()
		{
			return false;
		}

		#endregion

        #region EnableShowHiddenCollumnsOption
        public virtual string P_EnableShowHiddenCollumnsOption
        {
            get
            {
                return GetAttrValue(TextConst.AName.EnableShowHiddenCollumnsOption);
            }
            set
            {
                SetAttributeNotEmpty(TextConst.AName.EnableShowHiddenCollumnsOption, value);
            }
        }

        public virtual string P_EnableShowHiddenCollumnsOption_Title()
        {
            return "Включить опцию \"Выбор колонок и категорий\"";
        }

        public virtual string P_EnableShowHiddenCollumnsOption_ControlType()
        {
            return typeof(UICheck).Name;
        }

        public virtual bool P_EnableShowHiddenCollumnsOption_Exists()
        {
            return false;
        }

        #endregion

		#region hint
		public virtual string P_Hint {
			get {
                return this.AttrOrEmpty(AName_.hint);
			}
			set {
				this.SetAttributeNotEmpty(AName_.hint, value);
			}
		}
		public virtual string P_Hint_Title()
		{
			return "Подсказка";
		}
		public virtual string P_Hint_ControlType()
		{
            throw new NotImplementedException();
            //return typeof(UITextEx).Name;
		}
		public virtual bool P_Hint_Exists()
		{
			return false;
		}
		public virtual bool P_Hint_Editable()
		{
			return true;
		}
		#endregion
		#region intern
        public virtual string P_Intern {
            get {
                return this.AttrOrDefault(AName_.intern, TextConst.AVBool.False);
            }
            set {
                if (value != TextConst.AVBool.True) {
                    this.RemoveAttribute(AName_.intern);
                } else {
                    this.SetAttrValue(AName_.intern, TextConst.AVBool.True);
                }
            }
        }
        public virtual string P_Intern_Title()
        {
            return "Интернирование значений";
        }
        public virtual string P_Intern_ControlType()
        {
            return typeof(UICheck).Name;
        }
        public virtual bool P_Intern_Exists()
		{
			return false;
		}
        public virtual bool P_Intern_Editable()
        {
            return true;
        }
        #endregion
	}
}
