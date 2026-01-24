using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
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
using sql.builder.DataApi.Documenting;
//using sql.builder.TFS;
using sql.builder.UI;
using sql.builder.WinForms;
using sql.builder.XmlHelpers;
using AName_ = sql.builder.DataApi.AName;
using Contract = System.Diagnostics.Contracts.Contract;

namespace sql.builder.DataApi
{
    public partial class VSXElement : VXElement
    {
        internal VSXElement(XElement other)
            : base(other)
        {
            this.KeyField = AName_.name;
        }
        protected VSXElement(XName name)
            : base(name)
        {
            this.KeyField = AName_.name;
        }
        public bool TreeNodeExpanded = false;
        public bool TreeNodeExpandedWithParents()
        {
            var node = this;
            while (node != null)
            {
                if (!node.TreeNodeExpanded) return false;
                if (node.IsMainElement()) return true;
               
                node = node.GetParent();
            }
            return false;
        }
        public List<VAction> Events()
        {
            return this.GetElementsP(EName.events).SelectMany(VSXElement.GetElementsP).Cast<VAction>().ToList();
        }
        private static XElement getPartParent(XElement parent)
        {
            if (parent == null) {
                return null;
            }
            if (parent.Name != EName.part) {
                return parent;
            }
            string name = parent.AttrOrDefault(AName_.id, string.Empty);
            if (string.IsNullOrEmpty(name)) {
                name = parent.Elements().First().Attribute(AName_.part_id).Value;
            }
            VUsePart firstUse = VPart.FirstUse(XmlReports.Environment, name);
            if (firstUse != null) {
                return firstUse.Parent;
            } else {
                return null;
            }
        }
        private static VSXElement Create(XElement other, XElement parent = null)
        {
            Contract.Assert(other != null);
            if (parent == null) {
                parent = other.Parent;
            }
            VSXElement newElement = null;
            //XElement other1 = new XElement(other.Name);
            switch (other.Name.LocalName) {
                case TextConst.EName.Query:
                    // !!! Навести порядок, есть лишнее
                    if (parent != null && 
                        (
                        (parent.Name == EName.query  ) || // в отчете другие уровни

                         (parent.Parent != null && parent.Parent.Name == EName.report) // в отчете первый уровень
                        )
                        )
                    {
                        newElement = new VFromQuery();
                    }
                    else if ((parent != null && parent.Name == EName.part) || parent == null) {
                        if (other.Attribute(AName_.name) != null && other.Element(EName.select) == null) {
                            newElement = new VFromQuery();
                        } else {
                            newElement = new VQuery();
                        }
                    } else {
                        if (parent != null && parent.Parent != null
                            && parent.Name != EName.call
                            && parent.Name != EName.select
                            && parent.Parent.Name != EName.root &&
                            (other.Attribute(AName_.name) != null || other.Attribute(AName_.@as) == null)) {
                                if (other.Attribute(AName_.join) == null) {
                                    newElement = new VFromQuery();
                            } else {
                                newElement = new VRelation();
                            }
                        } else {
                            newElement = new VQuery();
                        }
                    }
                    break;
                case TextConst.EName.Queries:
                    newElement = new VReportQueries();
                    break;
                case TextConst.EName.Qube:
                    newElement = new VQube();
                    break;
                case TextConst.EName.Select:
                    newElement = new VSelect();
                    break;
                case TextConst.EName.From:
                    newElement = new VFrom();
                    break;
                case TextConst.EName.Column:
                    if (other.GetAncestor(EName.viewcolumns) != null || (other.GetAncestor(EName.columns) != null && other.GetAncestor(EName.query) != null)) {
                        newElement = new VViewColumn();
                    } else {
                        newElement = new VColumn();
                    }
                    break;
                case TextConst.EName.Call:
                    var prnt = getPartParent(parent);
                    if (prnt != null && (prnt.Name == EName.expression_package || prnt.Name == EName.expressions)) {
                        newElement = new VExpression();
                    } else {
                        newElement = new VCall();
                    }
                    break;
                case "link":
                    newElement = new VLink();
                    break;
                case "dlink":
                    newElement = new VDLink();
                    break;
                case "elink":
                    newElement = new VELink();
                    break;
                case "slink":
                    newElement = new VSLink();
                    break;
                case "const":
                    newElement = new VScalarConst();
                    break;
                case "array":
                    newElement = new VArray();
                    break;
                case "function":
                    newElement = new VFunction();
                    break;
                case "part":
                    newElement = new VPart();
                    break;
                case "usepart":
                    newElement = new VUsePart();
                    break;
                case "where":
                    newElement = new VWhere();
                    break;
                case TextConst.EName.Using:
                    newElement = new VUsing();
                    break;
                case "table":
                    newElement = new VTable();
                    break;
                case "report":
                    newElement = new VReport();
                    break;
                case TextConst.EName.Form:
                    newElement = new VForm();
                    break;
                case TextConst.EName.Field:
                    newElement = new VField();
                    break;
                case TextConst.EName.Having:
                    newElement = new VHaving();
                    break;
                case TextConst.EName.Content:
                    newElement = new VFormContent();
                    break;
                case TextConst.EName.FieldGroup:
                    newElement = new VFieldGroup();
                    break;
                case TextConst.EName.ColDimVal:
                    newElement = new VColDimVal();
                    break;
                case TextConst.EName.PrintTemplates:
                    newElement = new VPrintTemplates();
                    break;
                case TextConst.EName.Excel:
                    newElement = new VExcel();
                    break;
                case TextConst.EName.Word:
                    newElement = new VWord();
                    break;
                case TextConst.EName.Template:
                    newElement = new VPrintTemplate();
                    break;
                case TextConst.EName.Action:
                    newElement = new VAction();
                    break;
                case TextConst.EName.UseAction:
                    newElement = new VUseAction();
                    break;
                case TextConst.EName.Param:
                    newElement = new VParam();
                    break;
                case TextConst.EName.Grid:
                    newElement = new VGrid();
                    break;
                case TextConst.EName.UseField:
                    newElement = new VUseField();
                    break;
                case TextConst.EName.TabContainer:
                    newElement = new VTabContainer();
                    break;
                case TextConst.EName.SplitContainer:
                    newElement = new VSplitContainer();
                    break;
                case TextConst.EName.Params:
                    newElement = new VParams();
                    break;
                case TextConst.EName.UseParam:
                    newElement = new VUseParam();
                    break;
                case TextConst.EName.UseGlobParam:
                    newElement = new VUseGlobParam();
                    break;
                case TextConst.EName.ScrollArea:
                    newElement = new VScrollArea();
                    break;
                //case TextConst.EName.Return:
                //    newElement = new VReturn(other1);
                //    break;
                case TextConst.EName.If:
                    newElement = new VIf();
                    break;
                case TextConst.EName.Pivot:
                    newElement = new VPivot();
                    break;
                case TextConst.EName.Links:
                    newElement = new VLinks();
                    break;
                case TextConst.EName.ListQuery:
                    newElement = new VListQuery(EName.listquery);
                    break;
                case TextConst.EName.DefaultQuery:
                    newElement = new VListQuery(EName.defaultquery);
                    break;
                case TextConst.EName.ExtendWhere:
                    newElement = new VExtendWhere();
                    break;
                case TextConst.EName.WithParams:
                    newElement = new VWithParams();
                    break;
                case TextConst.EName.Actions:
                    newElement = new VActions();
                    break;
                case TextConst.EName.Scope:
                    newElement = new VScope();
                    break;
                case TextConst.EName.Connect:
                    newElement = new VConnect();
                    break;
                case TextConst.EName.Start:
                    newElement = new VStart();
                    break;
                //case TextConst.EName.Lists:
                //    newElement = new VLists(other1);
                //    break;
                case TextConst.EName.ReportProc:
                    newElement = new VReportProc();
                    break;
                case TextConst.EName.Customer:
                    if (parent != null && parent.Parent.Name == EName.root) {
                        newElement = new VCustomer();
                    } else {
                        newElement = new VCustomerUse();
                    }
                    break;
                case TextConst.EName.Customers:
                    newElement = new VCustomers();
                    break;
                case TextConst.EName.UseForm:
                    newElement = new VUseForm();
                    break;
                case TextConst.EName.Section:
                    newElement = new VSection();
                    break;
                case TextConst.EName.Columns:
                    if (other.GetAncestor(EName.grid) != null) {
                        newElement = new VGridColumns();
                    } else {
                        newElement = new VColumns();
                    }
                    break;
                case TextConst.EName.Band:
                    if (other.GetAncestor(EName.grid) != null) {
                        newElement = new VGridBand();
                    } else {
                        newElement = new VBand();
                    }
                    break;
                case TextConst.EName.Folder:
                    newElement = new VFolder();
                    break;
                case TextConst.EName.Fact:
                    newElement = new VFact();
                    break;
                case TextConst.EName.ExpressionPackage:
                    newElement = new VExpressionPackage();
                    break;
                case TextConst.EName.DimensionPackage:
                    newElement = new VDimensionPackage();
                    break;
                case TextConst.EName.Dimension:
                    newElement = new VDimension();
                    break;
                case TextConst.EName.Menu:
                    newElement = new VMenu();
                    break;
                case TextConst.EName.Toolbar:
                    newElement = new VToolbar();
                    break;
                case TextConst.EName.Events:
                    newElement = new VEvents();
                    break;
                case TextConst.EName.UICommand:
                    newElement = new VUICommand();
                    break;
                case TextConst.EName.Splitter:
                    newElement = new VSplitter();
                    break;
                case TextConst.EName.Buttons:
                    newElement = new VButtons();
                    break;
                case TextConst.EName.DimSet:
                    newElement = new VDimSet();
                    break;
                case TextConst.EName.Grouping:
                    newElement = new VGrouping();
                    break;
                case TextConst.EName.Expressions:
                    newElement = new VExpressions();
                    break;
                case TextConst.EName.Group:
                    newElement = new VGroup();
                    break;
                case TextConst.EName.Grset:
                    newElement = new VGrset();
                    break;
                case TextConst.EName.SourceLink:
                    newElement = new VSourceLink();
                    break;
                case TextConst.EName.Color:
                    newElement = new VColor();
                    break;
                case TextConst.EName.ColorPackage:
                    newElement = new VColorPackage();
                    break;
                case TextConst.EName.UseColor:
                    newElement = new VUseColor();
                    break;
                case TextConst.EName.Format:
                    newElement = new VFormat();
                    break;
                case TextConst.EName.FormatPackage:
                    newElement = new VFormatPackage();
                    break;
                case TextConst.EName.Factlinks:
                    newElement = new VFactLinks();
                    break;
                case TextConst.EName.Additions:
                    newElement = new VAdditions();
                    break;
                case TextConst.EName.Addition:
                    newElement = new VAddition();
                    break;
                case TextConst.EName.Label:
                    newElement = new VLabel();
                    break;
                case TextConst.EName.Role:
                    newElement = new VRole();
                    break;
                case TextConst.EName.SecurityPackage:
                    newElement = new VSecurityPackage();
                    break;
                case TextConst.EName.UseObject:
                    newElement = new VUseObject();
                    break;
                case TextConst.EName.UseRole:
                    newElement = new VUseRole();
                    break;
                case TextConst.EName.DimLink:
                    newElement = new VDimLink();
                    break;
                //case TextConst.EName.Visualizers:
                //    newElement = new VVisualizers(other1);
                //    break;
                case TextConst.EName.UseReport:
                    newElement = new VUseReport();
                    break;
                case TextConst.EName.Navigator:
                    newElement = new VNavigator();
                    break;
                default:
                    newElement = new VSXElement(other.Name);
                    break;
            }
            Contract.Assert(newElement.Name == other.Name);
            return newElement;
        }
        private static VSXElement Parse(XElement other, VEnvironment env = null)
        {
            if (other == null) {
                return null;
            }
            VSXElement newElement = Create(other);
            //newElement.environment = env;
            if (newElement != null) {
                foreach (XAttribute attr in other.Attributes()) {
                    newElement.Add(new XAttribute(attr));
                }
                foreach (XNode el in other.Nodes().ToList()) {
                    if (el is XElement) {
                        VSXElement newEl = VSXElement.Parse((XElement)el);
                        newElement.AddLast(newEl);
                    } else if (el is XText) {
                        newElement.Add(new XText((el as XText).Value));
                    }
                }
            }
            return newElement;
        }
        internal static VSXElement Get(XElement other)
        {
            return GetP(other, null);
        }
        internal static T Get<T>(XElement other)
            where T : VSXElement
        {
            return (T)Get(other);
        }
        internal static VSXElement GetP(XElement other, XElement parent)
        {
            
            VSXElement el;

            if (other == null)
            {
                return null;
            }

            if (!(other is VSXElement))
            {
                el = Create(other,parent);

                //if (other is VXElement)
                //{
                //    el.environment = (other as VXElement).environment;
                //}

                ReplaceUseContent(other, el);
            }
            else
            {
                el = (VSXElement)other;
            }
            return el;
        }
        private static void ReplaceUseContent(XElement oldElement, XElement newElement)
        {
            newElement.RemoveAttributes();
            foreach (XAttribute attr in oldElement.Attributes().ToList())
            {
                attr.Remove();
                newElement.Add(attr);
            }

            newElement.Nodes().Remove();
            foreach (XNode el in oldElement.Nodes().ToList())
            {
                el.Remove();
                newElement.Add(el);
            }

            if (oldElement.Parent != null)
            {
                oldElement.ReplaceWith(newElement);
            }
            else
            {
                if (oldElement.Document != null)
                {
                    XDocument doc = oldElement.Document;
                    oldElement.Remove();
                    doc.Add(newElement);

                }
            }
        }

        public VSXElement ChangeType()
        {

            VSXElement newEl = Create(this);
            if (this.GetType() != newEl.GetType())
            {
                ReplaceUseContent(this, newEl);
                newEl.Row = this.Row;

                if (newEl.Row != null)
                {
                    Row["node"] = newEl;
                }
                if (newEl.Row!=null){
                    //(newEl.Row.Table as VDataTable).UnsuppressChangeEvent();
                    //newEl.UpdateDataRow();
                    //(newEl.Row.Table as VDataTable).ResumeChangeEvent();
                    (newEl.Row.Table as VDataTable).RaiseCurrentRowChanged();

                }
           
            }
            else
            {
                newEl = this;
            }

           
            return newEl;
        }
        protected virtual bool ChildDependant { get { return false; } }
        //public void RaiseChange()
        //{
        //    if (this.Row != null)
        //    {
        //        (this.Row.Table as VDataTable).RaiseRowChanged(this.Row);
        //    }
        //}
        internal void RaiseParensChange()
        {
            VSXElement parent = this.Parent as VSXElement;
            if (parent != null && parent.ChildDependant) {
                // parent.dataChangeProcessing = true;
                parent.UpdateDataRow();
                parent.RaiseParensChange();
                // parent.dataChangeProcessing = false;
            }
        }
        internal static string RemovePropPfx(string propName)
        {
            return propName.Substring(PropPfx.Length, propName.Length - PropPfx.Length);
        }
        private int GetPropertyOrder(string name)
        {
            return VFieldInfo.Order(this, PropPfx + name);
        }
        internal IList<string> GetPropNames()
        {
            if (propNames == null) {
                List<string> list = new List<string>();
                PropertyInfo[] pis = this.GetType().GetProperties();
                for (int i1 = 0; i1 < pis.Length; i1++) {
                    PropertyInfo pi = pis[i1];
                    string property = pi.Name;
                    if (property.StartsWith(PropPfx) && property.IndexOf('_', PropPfx.Length) < 0) {
                        list.Add(property.Substring(PropPfx.Length));
                    }
                }
                propNames = list.OrderBy(this.GetPropertyOrder).ToArray<string>();
            }
            return propNames;
        }
        private static int idCounter = 0;
        // private bool dataChangeProcessing = false;
        private Stack<bool> suppressChangeEventStack = new Stack<bool>(new bool[] { false });
        private bool IsChangeEventSuppressed()
        {
            return this.GetMainParent().suppressChangeEventStack.Peek();
        }
        private void SuppressChangeEvent()
        {
            this.GetMainParent().suppressChangeEventStack.Push(true);
        }
        //private void UnsuppressChangeEvent()
        //{
        //    this.GetMainParent().suppressChangeEventStack.Push(false);
        //}
        private void ResumeChangeEvent()
        {
            this.GetMainParent().suppressChangeEventStack.Pop();
        }
        private static void onDataChanged(object sender, EventArgs e)
        {
            DataRow row = (DataRow)Cmn.GetProperty(e, "Row");
            if (row.RowState == DataRowState.Detached || row.RowState == DataRowState.Deleted) {
                return;
            }
            if (Cmn.IsNullOrDBNull(row["node"])) {
                return;
            }
            if ((row.Table as VDataTable).NewCurRowApplying) {
                return;
            }
            VSXElement element = (VSXElement)row["node"];
            if (element.IsChangeEventSuppressed()) {
                return;
            }
            element.SuppressChangeEvent();
            element.ClearCash();
            VDataColumn col = (VDataColumn)Cmn.GetProperty(e, "Column", null);
            //if (element.dataChangeProcessing)
            //{
            //    return;
            //}
            //element.dataChangeProcessing = true;
            if (col != null) {
                if (VDataColumn.HasBoundControl(col)) { // !!! Заплатка чтобы не обрабатывать служебные колонки. Не универсально
                    if (VFieldInfo.Exists(element, PropPfx + col.ColumnName)) {
                        object val = Cmn.GetProperty(element, PropPfx + col.ColumnName); // вроде как повторное получение свойства, тоже замедление
                        if (!val.Equals(row[col])) {
                            //Cmn.SetProperty(element, PropPfx + col.ColumnName, row[col]);
                            //element = (VSXElement)row["node"];// В случае смены типа элемент заменяется на новый 
                            //element.dataChangeProcessing = true;
                            //element.UpdateDataRow();// !!! Может сильно замедлять. Обновление всех полей при изменении любого
                            element = (VSXElement)row["node"];
                            if (VFieldInfo.Editable(element, PropPfx + col.ColumnName)) {
                                Cmn.SetProperty(element, PropPfx + col.ColumnName, row[col]);
                                element = (VSXElement)row["node"]; // В случае смены типа элемент заменяется на новый 
                                //element.dataChangeProcessing = true;
                                element.UpdateDataRow();// !!! Может сильно замедлять. Обновление всех полей при изменении любого
                            } else {
                                element.UpdateDataCell(col.ColumnName);
                            }
                        }
                    }
                }
            }
            element.RaiseParensChange();
            element.ResumeChangeEvent();
            //element.dataChangeProcessing = false;
        }
        internal const string PropPfx = "P_";
        internal XName KeyField;
        internal string SavedKey = string.Empty;
        internal string ParentName = string.Empty;
        internal XElement ClearBeforeSaveCmn()
        {
            XElement el = new XElement(this);
            el.DescendantsAndSelf().Attributes().Where(a => Cmn.XElementsToDataTableSysFieldsNames.Contains(a.Name.LocalName)).Remove();
            el.RemoveAttribute(AName_.file);
            // Делаем timestamp последним атрибутом
            XAttribute attr = el.Attribute(AName_.timestamp);
            if (attr != null && attr.NextAttribute != null) {
                attr.Remove();
                el.Add(attr);
            }
            //ClearBeforeSave(el);
            return el;
        }
        //protected virtual void ClearBeforeSave(XElement el)
        //{
        //}
        public virtual void MoveAfter(VSXElement prev)
        {
            XElement parent = this.Parent;
            this.Remove();
            if (prev != null)
            {
                prev.AddAfterSelf(this);
            }
            else
            {
                parent.AddFirst(this);
            }
        }
        public static string AddPathToFilename(string filename)
        {
            return XmlReports.GetRootPath() + "\\" + filename;
           
        }
        public void SaveInDefSourceFile()
        {
            ParentName = Parent.Name.LocalName;
            SourceFileName = AddPathToFilename(this.AttrOrEmpty(AName_.file));
            SaveInSourceFile();
        }


        public void SaveInSourceFile()
        {
            throw new NotImplementedException();
        }
        public void DeleteSaved()
        {
            if (SavedSourceFileName == "")
            {
                return;
            }

            if (SavedKey == "")
            {
                SavedKey = this.AttrOrEmpty(this.KeyField);
            }
            XDocument doc = Cmn.OpenXmlClearNS(SavedSourceFileName);
            XElement item = doc.Root.Elements().Elements(this.Name).SearchByAttribute(this.KeyField, this.SavedKey);
            if (item != null)
            {
                item.Remove();
            }
            Cmn.SaveXmlWithCheckOut(doc, SavedSourceFileName);
            SavedSourceFileName = "";

        }

        public string SavedSourceFileName;
        private string sourceFileName = "";
        public string SourceFileName
        {
            get
            {
                return sourceFileName;
            }
            set
            {
                if (sourceFileName == "")
                {
                    SavedSourceFileName = value;
                }
                sourceFileName = value;
            }
        }
        private VDataColumn CreateColumn(string propName)
        {
            VDataColumn col = new VDataColumn(propName, typeof(string));
            string name = PropPfx + propName;
            string controlType = VFieldInfo.ControlType(this, name);
            if (controlType == typeof(UICombo).Name || controlType == typeof(UIList).Name) {
                col.SelectionList = VFieldInfo.GetList(this, name);
                col.SelectionList.OwnerColumn = col;
                col.SelectionList.CustomRefresh = VFieldInfo.RefreshListMethod;
            }
            col.Caption = VFieldInfo.Title(this, name);
            col.IsHtml = VFieldInfo.IsHtml(this, name);
            return col;
        }
        private static void AddSpecColumns(DataTable table)
        {
            if (!table.Columns.Contains("id")) {
                table.Columns.Add(new VDataColumn("id"));
                table.Columns.Add(new VDataColumn("parent_id"));
                VDataColumn coln = new VDataColumn("node", typeof(VSXElement));
                table.Columns.Add(coln);
            }
        }
        private int ElementId = -1;
        public DataTable AsDataTable()
        {

            //!!! Запретить повторный перевод в DataTable

            VDataTable table = new VDataTable(true);
           // table.SuppressChangeEvent();

            AddSpecColumns(table);
            table.AddColumn("ord", typeof(decimal));
            var propNames = GetPropNames();
            for (int i = 0; i < propNames.Count; i++) {
                VDataColumn col = this.CreateColumn(propNames[i]);
                table.Columns.Add(col);
                table.CustomFieldInfoProc = VFieldInfo.GetFieldInfoForDataTableOfVSXElementCell;
                col.Visible = VFieldInfo.VisibleInTable(this, PropPfx + propNames[i]);
            }
            ToDataRows(table, null);
            foreach (VSXElement el in this.DescendantsAndSelf().ToList())
            {
                el.UpdateChildOrder();
            }
            table.Changed += onDataChanged;
            return table;
        }

        public List<VSXElement> Childs()
        {
            List<VSXElement> childs = this.Elements().ToList().SelectAsArray(VSXElement.Get).ToList();
            return childs;
        }


        public void UpdateChildOrder()
        {

            if (this.Row != null)
            {
               // (this.Row.Table as VDataTable).SuppressChangeEvent();
                int i = 0;
                foreach (VSXElement child in Childs())
                {
                    DataRow row = child.GetRow();
                    row["ord"] = i;
                    i++;
                }
               // (this.Row.Table as VDataTable).ResumeChangeEvent();
            }
        }
        private static VSXElement Create()
        {
            VSXElement el = new VSXElement("item");
            return el;
        }
        public VSXElement AddChild()
        {
            VSXElement el = Create();
            AddChild(el);
            return el;
        }
        public VSXElement InsertChild(XElement child, XElement prev, bool append = true, bool updateOrder = true)
        {

            if (child.Parent != null)
            {
                child.Remove();
            }

            if (prev == null)
            {
                this.AddFirst(child);
            }
            else
            {
                prev.AddAfterSelf(child);
            }
            VSXElement child1 = VSXElement.Get(child);

            foreach (XElement element in child1.DescendantsAndSelf().ToList().SelectAsArray(VSXElement.Get)) {
                Append((element.Parent as VSXElement), element, updateOrder);
            }
            return child1;
        }

        public void InsertChilds(List<XElement> childs, XElement prev, bool append = true)
        {
            if (this.Row.Table != null)
            {
                (this.Row.Table as VDataTable).SuppressChangeEvent();
                this.Row.Table.BeginLoadData();
            }
            foreach (XElement child in childs)
            {
                this.InsertChild(child, prev, append, false);
            }
            this.UpdateChildOrder();
            if (this.Row.Table != null)
            {
                (this.Row.Table as VDataTable).ResumeChangeEvent();
                this.Row.Table.EndLoadData();
            }
        }


        public void AddChild(VSXElement child, bool append = true, bool updateOrder = true)
        {

            this.AddFirst(child);
            if (append)
            {
                Append(this, child, updateOrder);
            }

        }




        public void AddLast(VSXElement child)
        {

            this.Add(child);
            Append(this, child);

        }

        public VSXElement AddNext()
        {

            VSXElement el = Create();
            AddNext(el);
            return el;
        }



        public void AddNext(VSXElement child)
        {
            this.AddAfterSelf(child);
            Append((this.Parent as VSXElement), child);

        }

        public void AddPrev(VSXElement child)
        {
            this.AddBeforeSelf(child);
            Append((this.Parent as VSXElement), child);

        }

        public void Append(VSXElement parent, XElement child, bool updateOrder = true)
        {
           
            VSXElement child1 = VSXElement.Get(child);
            if (parent.Row != null)
            {
               
                DataRow row = child1.ToDataRow(parent.Row.Table);
               // (row.Table as VDataTable).SuppressChangeEvent();
                row["parent_id"] = parent.Row["id"];
               // (row.Table as VDataTable).ResumeChangeEvent();
            }
            if (updateOrder)
            {
                parent.UpdateChildOrder();// !!! Тормозит придобавлении большого к-ва эл-тов
            }
        }

        public DataRow Row;
        private void ToDataRows(DataTable table, string parentId)
        {
            DataRow row = ToDataRow(table);

            row["parent_id"] = parentId;


            foreach (var child in Childs())
            {
                child.ToDataRows(table, row["id"].ToString());
            }

        }

        public DataRow GetRow()
        {
            if (Row != null)
            {
                return Row;
            }

            return ToDataRow(SearchTable());


        }


        public DataTable SearchTable()
        {
            VSXElement element = this;

            while (element != null && element.Row == null)
            {
                element = element.GetParent();
            }
           return element.Row.Table;


        }
        private DataRow ToDataRow(DataTable table)
        {
            #if DEBUG
            Stopwatch sw = new Stopwatch();
            sw.Start();
            #endif
            // (table as VDataTable).SuppressChangeEvent();
            DataRow row = table.Rows.Add();
            IList<string> names = this.GetPropNames();
            for (int index = 0; index < names.Count; index++) {
                string name = names[index];
                VDataColumn col = (VDataColumn)table.Columns[name];
                if (col.Visible) {
                    string property = PropPfx + name;
                    if (VFieldInfo.Exists(this, property)) {
                        row[col] = VFieldInfo.GetValue(this, property);
                    } else {
                        row[col] = null;
                    }
                }
            }
            row["node"] = this;
            if (ElementId < 0) {
                ElementId = idCounter;
                idCounter++;
            }
            row["id"] = ElementId;
            idCounter++;
            this.Row = row;
            // (table as VDataTable).ResumeChangeEvent();
            #if DEBUG
            sw.Stop();
            Debug.WriteLine("VSXElement.ToDataRow(): " + sw.ElapsedTicks.ToString() + " тактов = " + sw.ElapsedMilliseconds.ToString() + " мс");
            #endif
            return row;
        }
        /*private DataRow ToDataRowSingle(DataTable table, bool addCol = false, int key = 0)
        {
            DataRow row = table.Rows.Add();
            IList<string> names = GetPropNames();
            for (int i = 0; i < names.Count; i++) {
                string name = names[i];
                string property = PropPfx + name;
                if (addCol) {
                    AddSpecColumns(table);
                    if (VFieldInfo.Exists(this, property)) {
                        VDataColumn col = (VDataColumn)table.Columns[name];
                        if (col == null) {
                            col = this.CreateColumn(name);
                            table.Columns.Add(col);
                        }
                        col.Visible = VFieldInfo.VisibleInTable(this, property);
                        if (col.Visible) {
                            row[col] = VFieldInfo.GetValue(this, property);
                        }
                        row["id"] = key.ToString();
                    }
                } else {
                    VDataColumn col = (VDataColumn)table.Columns[name];
                    if (col.Visible) {
                        if (VFieldInfo.Exists(this, property)) {
                            row[col] = VFieldInfo.GetValue(this, property);
                        } else {
                            row[col] = null;
                        }
                    }
                }
            }
            row["node"] = this;
            return row;
        }*/
        internal void UpdateDataRow()
        {
            DataRow row = this.Row;
            if (row != null && row.RowState != DataRowState.Detached) {
                // SuppressChangeEvent();
                IList<string> names = GetPropNames();
                for (int i = 0; i < names.Count; i++) {
                    string name = names[i];
                    object new_value = this.getPropertyValue(name);
                    object old_value = row[name];
                    if (Cmn.Nvl(new_value, string.Empty).ToString() != Cmn.Nvl(old_value, string.Empty).ToString()) {
                        row[name] = new_value;
                    }
                }
                // ResumeChangeEvent();
            }
        }
        internal void UpdateDataCell(string name)
        {
            DataRow row = this.Row;
            if (row.RowState != DataRowState.Detached) {
                object new_value = this.getPropertyValue(name);
                object old_value = row[name];
                if (Cmn.Nvl(new_value, string.Empty).ToString() != Cmn.Nvl(old_value, string.Empty).ToString()) {
                    row[name] = new_value;
                }
            }
        }
        private object getPropertyValue(string name)
        {
            string property = PropPfx + name;
            if (VFieldInfo.Exists(this, property)) {
                return VFieldInfo.GetValue(this, property);
            } else {
                return null;
            }
        }
        private static IList<string> propNames;
        protected void SetAttribute(string attributeName, string value)
        {
            this.SetAttributeValue(attributeName, value);
        }
        protected void SetAttributeNotEmpty(string attributeName, string value)
        {
            this.SetAttributeNotEmpty((XName)attributeName, value);
        }
        protected void SetAttributeNotEmpty(XName name, string value)
        {
            if (string.IsNullOrEmpty(value)) {
                this.RemoveAttribute(name);
            } else {
                this.SetAttrValue(name, value);
            }
        }
        internal VPart RootPart()
        {
            //!!! Может замедлять, если так , сделать кеширование
            VPart query = (VPart)this.GetAncestorsAndSelf(EName.part).LastOrDefault();
            return query;
        }
        public virtual VSourcedElement RootQuery()
        {
            //!!! Может замедлять, если так , сделать кеширование
            //VSXElement query = (VSXElement)this.GetAncestorsAndSelf(new string[] { TextConst.EName.Query, TextConst.EName.Form, /*TextConst.EName.ExcelTemplate, TextConst.EName.NavigationItem,*/ TextConst.EName.Report }).LastOrDefault();
            VSXElement query = this.GetAncestorsAndSelf().LastOrDefault(e => (e.Name == EName.query) || (e.Name == EName.form) || (e.Name == EName.report));
            if (query == null) {
                VPart rootPart = this.RootPart();
                if (rootPart != null) {
                    VSXElement partUse = rootPart.FirstUse();
                    if (partUse != null) {
                        query = partUse.RootQuery();
                    }
                }
            }
            return query as VSourcedElement;
        }
        public virtual VSourcedElement ExtendedOrRootQuery()
        {
            VSXElement exwhere = this.GetAncestorsAndSelf(EName.extendwhere).FirstOrDefault();
            if (exwhere != null) {
                VQueryCall call = exwhere.GetParent() as VQueryCall;
                if (call == null) {
                    return null;
                } else {
                    return call.Query();
                }
            } else {
                VSXElement qry = this.GetAncestorsAndSelf(EName.query).FirstOrDefault(q => q is VQuery);
                if (qry == null) {
                    qry = RootQuery();
                }
                return (VSourcedElement)qry;
            }
        }
        public virtual string XName
        {
            get {
                return this.AttrOrEmpty(AName_.@as);
            }
        }
        //public virtual string XTitle()
        //{
        //     return GetAttrValue("title");
        //}
        public virtual string AName()
        {
            return this.AttrOrEmpty(AName_.name);
        }
        public virtual string DataType()
        {
            return this.AttrOrEmpty(AName_.type);
        }
        public virtual string XDataType()
        {
            return this.DataType();
        }
        public virtual string Format()
        {
            return this.AttrOrEmpty(AName_.format);
        }
        public virtual string XFormat()
        {
            return this.Format();
        }
        public virtual string XHAlign()
        {
            return this.P_HAlign;
        }
        private static bool IsSimpleType(string type)
        {
            return type == TextConst.AVDataType.String || type == TextConst.AVDataType.Number || type == TextConst.AVDataType.Date || type == TextConst.AVDataType.Variant;
        }
        private static List<XElement> FunctionsListForType(string typ)
        {
            if (typ == string.Empty) {
                return XmlReports.Environment.Manager.GetScheme().Elements(EName.functions).Elements(EName.function).ToList();
            } else if (typ == "any") {
                return XmlReports.Environment.Manager.GetScheme().Elements(EName.functions).Elements(EName.function).Where(e => IsSimpleType(e.AttrOrDefault(AName_.type, string.Empty))).ToList();
            } else if (!IsSimpleType(typ)) {
                return XmlReports.Environment.Manager.GetScheme().Elements(EName.functions).Elements(EName.function).Where(e => e.AttrOrDefault(AName_.type, string.Empty) == typ).ToList();
            } else {
                return XmlReports.Environment.Manager.GetScheme().Elements(EName.functions).Elements(EName.function).Where(e => e.AttrOrDefault(AName_.type, string.Empty) == typ || e.AttrOrDefault(AName_.type, string.Empty) == TextConst.AVDataType.Variant).ToList();
            }
        }
        //public string ImageName()
        //{
        //    XElement typeInfo=   this.Environment().SchemeNative.Element("eltypes").Elements().Where(e => Cmn.GetAttrValue(e, "name").ToLower() == this.GetType().Name.ToLower()).FirstOrDefault();
        //    if (typeInfo != null)
        //    {
        //        return this.Environment().GetImageIndex(  Cmn.GetAttrValue(typeInfo, "image")).ToString();
        //    }
        //    return null;
        //}
        internal void SetProperty(string info, object value)
        {
            string propName = info.Replace("_Set", "");
            VFieldInfo.SetValue(this, propName, value);
        }
        internal class ElementUse
        {
            public ElementUse(VSXElement parent,VSXElement user, string attribute)
            {
                Parent= parent;
                User = user;
                Attribute = attribute;
            }
            public VSXElement User;
            public VSXElement Parent;
            public string Attribute;
            private string buildId()
            {
                var s = User.GetHashCode().ToString() + "-" + Attribute;
                return s;
            }
            public static List<ElementUse> Distinct(IEnumerable<ElementUse> list)
            {
                var list1 = new List<ElementUse>();

                var checkList = new HashSet<string>();

                foreach (var obj in list)
                {
                    var id = obj.buildId();
                    if (!checkList.Contains(id))
                    {
                        checkList.Add(id);
                        list1.Add(obj);
                    }
                }
                return list1;
            }
        }

        public DataTable GetUsesAsDataTable()
        {
            DataTable tbl = new VDataTable();
           // tbl.Columns.Add(new VDataColumn("file_name"));
            tbl.Columns.Add(new VDataColumn("etype"));
            tbl.Columns.Add(new VDataColumn("etext"));
            tbl.Columns.Add(new VDataColumn("node_name"));
            tbl.Columns.Add(new VDataColumn("attr"));
            tbl.Columns.Add(new VDataColumn("text") { IsHtml = true });
            DataColumn col = new VDataColumn("node", typeof(XElement));
            tbl.Columns.Add(col);
            var uses = SearchUses();
            foreach (var use in uses) {
                var el = use.User;
                VSXElement main = el.GetMainParent();
                tbl.Rows.Add(
                    main.GetNodeTypeInfo(), 
                    main.P_IdName, 
                    el.Name.LocalName,
                    use.Attribute,
                   // el.GetNodeTypeInfo(),
                    el.GetNodeOtherInfo(), el);
            }
            return tbl;
        }
        public  List<ElementUse> SearchUses()
        {
          var list = searchUses();
		  string partId = P_PartId; //get current part name
		  if (!(partId == ""))
		  {
			  List<VSXElement> list1 = XmlReports.Environment.GetUsePartElements(P_PartId);
			  foreach (var el1 in list1)
			  {
				  list.Add(new ElementUse(this, el1, TextConst.AName.Part));
			  }
		  }
       
          return ElementUse.Distinct(list);
        }
        protected virtual List<ElementUse> searchUses()
        {
            if (GetParent() is VSelect)
            {
                if (GetMainParent() is VQuery)
                {
                    return (GetMainParent() as VQuery).SearchColumnUses(this);
                }
            }
            return new List<ElementUse>();
        }
        internal string GetAttrForRename()
        {
            string attrName;
            if (this.IsMainElement()) {
                attrName = XmlReports.Environment.GetElementTypeKeyAttrName(this.Name.LocalName);
            } else {
                attrName = TextConst.AName.As;
            }
            return attrName;
        }
        internal void RenameUses(string newName)
        {
            var list = SearchUses();
            string attrName = GetAttrForRename();
            string oldName = this.AttrOrEmpty(attrName);
            var mainElList = new List<VSXElement>();
            if (!this.IsMainElement()) {
                list.Add(new ElementUse(this, this, attrName));
            }
            foreach (var use in list) {
                string oldName1 = use.User.AttrOrEmpty(use.Attribute);
                if (!oldName1.StartsWith(oldName)) {
                    throw new sql.builder.Exceptions.VCompilerException("Переимнование не выполнено. Ошибка в алгоритме поиска ссылок. Перезапустите приложение.", use.User.GetMainParent(), use.User);
                }
                string newName1 = newName + oldName1.Substring(oldName.Length, oldName1.Length - oldName.Length);
                use.User.SetAttributeValue(use.Attribute, newName1);
                use.User.UpdateDataRow();
                if (use.User.Row != null) {
                    (use.User.Row.Table as VDataTable).RaiseCurrentRowChanged();
                }
                var mp = use.User.GetMainParent();
                if (!mainElList.Contains(mp)) {
                    mainElList.Add(mp);
                }
            }
            if (this.IsMainElement()) {
                if (!mainElList.Contains(this)) {
                    mainElList.Add(this);
                }
                SetIdName(attrName, newName);
                UpdateDataRow();
                if (Row != null) {
                    (Row.Table as VDataTable).RaiseCurrentRowChanged();
                }
            }
            foreach (var el in mainElList) {
                el.SaveInDefSourceFile();
            }
        }
        internal VSXElement GetMainParent()  //!!! Сделать покрасивее
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null))
            {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as VSXElement);
            }
            
           
                        
            if (this.GetParent() == null)
            {
                return null;
            }

            if (this.GetParent().GetParent() == null)
            {
                return null;
            }

            VSXElement el = this;

            while (el.GetParent().GetParent().Name.LocalName != "root")
            {
                el = el.GetParent();
                if (el.GetParent().GetParent() == null)
                {
                    return el.GetParent();
                }
            }

            var vel= VSXElement.Get(el);
            AddCashValue(vel, MethodBase.GetCurrentMethod().ToString(), null);
            return vel;
        }


        public VSXElement GetSubMainParent()
        {
            if (this.GetParent() == null)
            {
                return null;
            }

            if (this.GetParent().GetParent() == null)
            {
                return null;
            }

            if (this.GetParent().GetParent().GetParent() == null)
            {
                return null;
            }

            VSXElement el = this;

            while (el.GetParent().GetParent().GetParent().Name.LocalName != "root")
            {
                el = el.GetParent();
            }
            return VSXElement.Get(el);

        }

        public virtual void Delete()
        {
            var t = (VDataTable)this.Row.Table;
           // t.SuppressChangeEvent();
            foreach (VSXElement el in DescendantsAndSelf().Select(VSXElement.Get).ToList())
            {
                if (el.Row != null && el.Row.RowState != DataRowState.Detached)
                {
                    t.Rows.Remove(el.Row);
                }
            }

            this.Remove();
           // t.ResumeChangeEvent();
        }

        public XElement VirtualParent = null;

        public VSXElement GetParent()
        {
            if (VirtualParent != null)
            {
                return VSXElement.Get(VirtualParent);

            }
            else
            {
                return VSXElement.Get(Parent);
            }

            //if (BaseElementOrSelf().VirtualParent != null)
            //{
            //    return VSXElement.Get(BaseElementOrSelf().VirtualParent);

            //}
            //else
            //{
            //    return VSXElement.Get(BaseElementOrSelf().Parent);
            //}
        }


        public VSXElement GetBaseParent()
        {
            //if (VirtualParent != null)
            //{
            //    return VSXElement.Get(VirtualParent);

            //}
            //else
            //{
            //    return VSXElement.Get(Parent);
            //}

            if (BaseElementOrSelf().VirtualParent != null)
            {
                return VSXElement.Get(BaseElementOrSelf().VirtualParent);

            }
            else
            {
                return VSXElement.Get(BaseElementOrSelf().Parent);
            }
        }
        /*public List<VSXElement> GetAncestorsAndSelf(string[] names)
        {
            List<VSXElement> list = new List<VSXElement>();
            var el = this;
            while (el != null) {
                if (names[0] == null || names.Contains(el.Name.LocalName)) {
                    list.Add(el);
                }
                el = el.GetParent();
            }
            return list;
        }
        public List<VSXElement> GetAncestorsAndSelf(string name)
        {
            return GetAncestorsAndSelf(new[] { name });
        }*/
        internal IList<VSXElement> GetAncestorsAndSelf(XName name)
        {
            List<VSXElement> list = new List<VSXElement>();
            VSXElement el = this;
            while (el != null) {
                if (el.Name == name) {
                    list.Add(el);
                }
                el = el.GetParent();
            }
            return list;
        }
        internal IList<VSXElement> GetAncestorsAndSelf()
        {
            List<VSXElement> list = new List<VSXElement>();
            VSXElement el = this;
            while (el != null) {
                list.Add(el);
                el = el.GetParent();
            }
            return list;
        }
        public virtual bool IsElementUser()
        {
            return false;
        }
        /// <summary>
        /// Возвращает true для элементов второго уровня (например, для /root/queries/query )
        /// </summary>
        /// <returns></returns>
        internal bool IsMainElement()
        {
            VSXElement parent = this.GetParent();
            if (parent == null) {
                return false;
            }
            VSXElement grand_parent = parent.GetParent();
            if (grand_parent == null) {
                return false;
            }
            return grand_parent.Name == EName.root;
        }
        public virtual List<VSXElement> GetUsedElements()
        {
            return new List<VSXElement>();
        }
        internal string GetFullName(string name = null)
        {
            return this.GetNodeTypeInfo() + " " + this.GetName(name);
        }
        private string GetName(string name)
        {
            if (name == null) {
                return this.P_IdName;
            } else {
                return name;
            }
        }
        internal List<VSXElement> AsList()
        {
            List<VSXElement> list = new List<VSXElement>(1);
            list.Add(this);
            return list;
        }
        internal virtual IList<VSXElement> SourceColumns()
        {
            return Array.Empty<VSXElement>();
        }
        /*internal virtual IList<VSXElement> SelfOrMultipleSource()
        {
            return new VSXElement[1] { this };
        }*/
        internal VSXElement SelfParentOrUsepartParent()
        {
            VSXElement parent = this.GetParent();
            VPart part = parent as VPart;
            if (part != null) {
                VUsePart fu = part.FirstUse();
                if (fu != null) {
                    return fu.SelfParentOrUsepartParent();
                }
            }
            return parent;
        }
        public virtual void LookUpNextSources(List<VSXElement> list, VLookupAnalyzer analyzer)
        {
            analyzer.CheckAndReturn(list, this);
        }
        public virtual List<VSXElement> LookUpSources(VLookupAnalyzer analyzer)
        {
            var list = new List<VSXElement>();
            LookUpNextSources(list, analyzer);
            return list;
        }
        public virtual List<VColumn> UsedColumns()
        {
            return new List<VColumn>();
        }
        internal static bool IsListColumn(VSXElement e)
        {
            return e.AttrOrDefault(TextConst.AName.IsListColumn, false);
        }
        private int uniqueKey = 0;

        public int GetUniqueKey()
        {
            if (uniqueKey == 0)
            {
                uniqueKey = GetHashCode();
            }
            return uniqueKey;
        }
        public virtual VField Field()
        {
            return null;
        }
        private VIf IfElementSelf()
        {
            IList<VSXElement> list = this.GetElementsP(EName.@if);
            if (list.Count == 0) {
                return null;
            } else {
                return (VIf)list[0];
            }
        }
        private VIf IfElement()
        {
            VIf ifEl = this.IfElementSelf();
            if (ifEl == null && this.P_If != "") {
                VSXElement IfSource = this.RootQuery().ColumnsWithDublers().FirstOrDefault(e => e.P_If == P_If && e.IfElementSelf() != null);
                if (IfSource != null) {
                    return IfSource.IfElementSelf();
                }
            }
            return ifEl;
        }
        public VPivot PivotElementSelf()
        {
            return (VPivot)GetElementsP(EName.pivot).FirstOrDefault();
        }

        public VPivot PivotElement()
        {
            var ifEl = PivotElementSelf();
            if (ifEl == null)
            {
                if (P_Dimname != "")
                {
                    var IfSource = RootQuery().ColumnsWithDublers().Where(e => e.P_Dimname == P_Dimname && e.PivotElementSelf() != null).FirstOrDefault();

                    if (IfSource != null)
                    {
                        return IfSource.PivotElementSelf();
                    }
                }
            }
            return ifEl;
        }
        internal string AliasPfx()
        {

            var ifEl = IfElement();
            if (ifEl != null)
            {
                return ifEl.P_AliasPfx;
            }
            var pivEl = PivotElement();
            if (pivEl != null)
            {
                return pivEl.P_AliasPfx;
            }

            return "";
        }
        public virtual List<VSXElement> GetFactColumns()
        {
            if (P_Fact != "") {
                return this.AsList();
            }
            return null;
        }
        internal DateTime? GetTimeStamp()
        {
            XAttribute atr = this.Attribute(AName_.timestamp);
            if (atr == null) {
                return null;
            } else {
                return DateTime.ParseExact(atr.Value, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None);
            }
        }
        internal void SetTimeStamp()
        {
            this.SetAttributeValue(AName_.timestamp, DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
        }
        #region Условные предикаты
        internal static bool HasParameterName(VSXElement e)
        {
            return !string.IsNullOrEmpty(e.P_ParName);
        }
        #endregion
    }
    internal class VRenameEventArgs : EventArgs
    {
        public string OldName;
        public string NewName;
        public VSXElement Element;
        public bool Cancel = false;
        public VRenameEventArgs(VSXElement element, string oldName, string newName)
            : base()
        {
            Element = element;
            OldName = oldName;
            NewName = newName;
        }



    }

    internal delegate void VRenameEventHandler(object sender, VRenameEventArgs e);

    internal class NameCheck
    {
        public NameCheck(IList<string> names)
        {
            foreach (string name in names)
            {
                NameAndIndex ni = new NameAndIndex(name);
                AddName(ni);
            }
        }

        public void AddName(NameAndIndex ni)
        {

            if (!Names.ContainsKey(ni.Name))
            {
                Names.Add(ni.Name, new List<Int32>());
            }
            Names[ni.Name].Add(ni.Index);
        }

        public SortedList<string, List<int>> Names = new SortedList<string, List<int>>();

        public string GetName(string name)
        {
            NameAndIndex ni = new NameAndIndex(name);
            if (!Names.ContainsKey(ni.Name))
            {
                AddName(ni);
                return name;
            }

            if (!Names[ni.Name].Contains(ni.Index))
            {
                AddName(ni);
                return name;
            }

            ni.Index++;
            AddName(ni);
            return ni.Value();
        }

        public string GetAlias(string name)
        {

            string newName = GetName(name);

            if (name != newName)
            {
                return newName;
            }
            else
            {
                return "";
            }

        }

    }

    internal class NameAndIndex
    {
        public NameAndIndex(string name)
        {
            string[] ss = name.Split('_');
            if (Cmn.IsNumeric(ss[ss.Length - 1]))
            {
                Index = Convert.ToInt32(ss[ss.Length - 1]);
                for (int i = 0; i < ss.Length - 1; i++)
                {
                    Name += ss[i];
                }
            }
            else
            {
                Index = 0;
                Name = name;
            }
        }
        public string Name = "";
        public int Index;

        public string Value()
        {
            if (Index == 0)
            {
                return Name;
            }
            else
            {
                return Name + "_" + Index.ToString();
            }
        }
    }
}