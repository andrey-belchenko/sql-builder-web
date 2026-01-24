using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Reflection;
using sql.builder.Exceptions;
using System.Text.RegularExpressions;
using System;
using System.Text;
using System.Diagnostics.Contracts;
using AName_ = sql.builder.DataApi.AName;

namespace sql.builder.DataApi
{
    internal sealed partial class VQuery : VSourcedElement, IVParent
    {
        internal VQuery()
             : base(EName.query)
        {
            this.ParentName = TextConst.EName.Queries;
        }
        private VQuery(XElement element)
            : base(element)
        {
            this.ParentName = TextConst.EName.Queries;
        }
        private bool IsReport()
        {
            return this.P_IsReport == TextConst.AVBool.True;
        }
        //public XElement GetExtended()
        //{
        //    XElement main = new XElement( GetMainIE());
        //    foreach (XElement ext in GetExtensionsAndParent())
        //    {
        //        XmlReports.ExtentdQuery(new XElement(ext), main);
        //    }
        //    return main;
        //}
        //public XElement GetExtended()
        //{
        //    var main = GetMainE();
        //    XElement xmain = new XElement(main);
        //  //  var parent=main.
        //    foreach (XElement ext in GetExtensions())
        //    {
        //        XmlReports.ExtentdQuery(new XElement(ext), xmain);
        //    }
        //    if (main.IsInherit())
        //    {
        //        var xpar =new XElement( main.GetMainIE());
        //        Compiler.addColumnsAlias(xpar);
        //        Compiler.addColumnsAlias(xmain);
        //        Compiler.Inheritance(xmain, xpar);
        //    }
        //    return xmain;
        //}
        public override VQuery Query()
        {
            return this;
        }
        public XElement GetQubeInfo()
        {
            VQuery qry = VSXElement.Get<VQuery>(new XElement(this));
            qry.Descendants().Where(e => Cmn.GetAttrValue(e, TextConst.AName.Exclude) == TextConst.AVBool.True).Remove();
            //qry.environment = GetEnvironment();
            qry.Elements(TextConst.EName.Menu).Remove();
            qry.Elements(TextConst.EName.Grouping).Remove();
			qry.Elements(TextConst.EName.Events).Remove();
            var info = VQubeUtils.GetQubeInfo(qry);
            return info;

        }

		public VSXElement ContentElement()
		{
			var cnt = this.GetElementsP(EName.content).FirstOrDefault();
			return cnt;
		}

		public List<VSXElement> Fields()
		{
			var list = new List<VSXElement>();
			var cnt = ContentElement();
			if (cnt != null)
			{
                list = ContentElement().GetDescedantsP(EPredicate.IsFieldOrUseField);
			}
			return list;
		}
		/*public List<VSXElement> Groups()
		{

		
			var list = new List<VSXElement>();
			var cnt = ContentElement();
			if (cnt != null)
			{
				list = ContentElement().GetDescedantsP(EName.fieldgroup);
			}
			return list;
		}*/
        internal string GetSql(bool useTitlesAsHeads = false)
        {
            XElement qry = new XElement(this.GetMainE());
            if (!qry.Elements().Any(e => e.Name != EName.@const)) {
                qry.Elements().Remove();
                qry.Add(new XElement(EName.select, Factory.NewColumn("a", TextConst.AVColumn.All)));
                qry.Add(new XElement(EName.from,
                            new XElement(EName.query,
                                new XAttribute(AName_.name, qry.Attribute(AName_.name).Value),
                                new XAttribute(AName_.@as, "a"))
                            )
                        );
            }
            if (useTitlesAsHeads) {
                for (int i = 0; i < this.Element(EName.select).Elements().Count(); i++) {
                    VSXElement origCol = VSXElement.Get(this.Element(EName.select).Elements().ElementAt(i));
                    XElement newCol = qry.Element(EName.select).Elements().ElementAt(i);
                    newCol.SetAttributeValue(AName_.@as, "\"" + Cmn.CutString(origCol.P_Title, 30) + "\"");
                }
            }
        //    qry.Attributes("name").Remove();
            qry.SetAttributeValue("noname", "1");
            qry.Elements(EName.menu).Remove();
			qry.Elements(EName.events).Remove();
            XElement compiledQuery = Compiler.GetCompiledAndProcessedQuery(qry);
            string selectText =  Compiler.GetQuerySelectStatmentFromCompiledQuery(compiledQuery);
            // вынос скрипта из узла procedure перед основным запросом
            XElement procedure = qry.Element(EName.procedure);
            if (procedure != null) {
                string proc = procedure.Value;
                string[] paramNames = Cmn.ExtractParameterNamesFromSQL(proc);
                //замена параметров в скрипте procedure на константы из узла params
                foreach (string par in paramNames) {
                    XElement param = qry.Element(EName.@params).Elements(EName.param).SearchByAttribute(AName_.name, par);
                    string datatype = param.AttrOrEmpty(AName_.type);
                    StringBuilder pv = new StringBuilder();
                    if (datatype == TextConst.AVDataType.Array) {
                        pv.Append('(');
                    }
                    IList<XElement> vals = param.Descendants(EName.@const).ToList();
                    for (int index = 0; index < vals.Count; index++) {
                        if (index != 0) {
                            pv.Append(',');
                        }
                        pv.Append(vals[index].Value);
                    }
                    if (datatype == TextConst.AVDataType.Array) {
                        pv.Append(')');
                    }
                    string pattern = ":\b" + par + "\b";
                    proc = Regex.Replace(proc, pattern, pv.ToString());
                }                
                selectText = proc + "/" + selectText;
            }
            string procText = Compiler.GetQuerProcedureFromCompiledQuery(compiledQuery);
            if (procText != null) {
                selectText = procText + "/" + selectText;
            }
            return Cmn.ClearUndefined(selectText);
        }
        /*public VColumn AddColumn(string tableName,string columnName)
        {
            VColumn col = new VColumn();
            col.SetAttributeValue("table", tableName);
            col.SetAttributeValue("column", columnName);
            if (this.Element("select") == null)
            {
                this.AddFirst(new XElement("select"));
            }
            this.Element("select").Add(col);

            return col;
        }*/
        /*public void ClearFrom()
        {
            this.Elements("from").Remove();
        }*/
        internal static VQuery GetOrCreate(VEnvironment enviroment, string name)
        {
            XElement element = enviroment.Manager.GetScheme().Elements("queries").Elements("query").FirstOrDefault(e => e.Attribute("name").Value == name);
            return GetOrCreate(element);
        }
        private static VQuery GetOrCreate(XElement element)
        {
            if (element == null) {
                return null;
            }
            if (element is VQuery) {
                return (VQuery)element;
            } else {
                Contract.Assume(element.Name == EName.query);
                VQuery item = new VQuery(element);
                element.ReplaceWith(item);
                if (item.AttrOrEmpty(AName_.@class) == "1") {
                    item.EntityType = new VEntityType(item);
                }
                //item.environment = SearchEnvironment(item);
                return item;
            }
        }
         public new string Name {
             get {
                 return this.AttrOrEmpty(AName_.name);
             }
         }
         //public List<VQuery> Extensions()
         //{

         //    return Environment().SchemeNative.Elements("queries").Elements("query").Where(q => Cmn.GetAttrValue(q,"extend") == Name).ToList().Select(q1 => (VQuery)VSXElement.Get(q1)).ToList();
         //}
         public string Title()
         {
             return this.AttrOrEmpty(AName_.title);
             //if (title == "")
             //{
             //    List<VQuery> exts = Extensions();
             //    title = Cmn.Nvl(Extensions().Attributes("title").Select(a => a.Value).FirstOrDefault(), "").ToString();
             //}
             //return title;
         }
         private VEntityType entityType = null;

         public VEntityType EntityType
         {
             get
             {
                 if (entityType == null)
                 {
                     entityType = new VEntityType(this);
                 }
                 return entityType;
             }
             set
             {
                 entityType = value;
             }
         }

       


        

       

         

         

         public List<VSXElement> DimensionsOld()
         {
             List<VSXElement> list = new List<VSXElement>();
             foreach (XElement el in this.Elements("select").Elements().Where(e => Cmn.GetAttrValue(e, "group") == "1" && Cmn.GetAttrValue(e, "stored") != "0").ToList())
             {
                 list.Add(Get(el));

             }
             return list;
         }

         //public VSXElement KeyDimension()
         //{
         //    return  SelfColumns().Where(e => e.P_KeyDimension != "").FirstOrDefault();
         //}

         public VDimension GetDimension()
         {
             return XmlReports.Environment.GetDimensionByQueryName(GetMainE().P_IdName);
         }

         public List<VSXElement> FactColumns()
         {
             return SelfColumns().Where(e => e.P_Fact != "").ToList();
         }
         //public override void ClearBeforeSave(XElement el)
         //{
         //   if (Cmn.GetAttrValue(el, "name") == Cmn.GetAttrValue(el, "extend")) {
         //        el.Attributes("extend").Remove();
         //   }
         //}
         // public override string GetName(string name)
         //{
         //    string ext = "";
         //    if (Attribute("extend") != null)
         //    {
         //        ext = Attribute("extend").Value + ".";
         //    }
         //    return ext + GetCurrentName(name);
         //}
         //public List<VSXElement> MarkedVidColumns()
         //{
         //    return Columns().Where(e => e.IsVid()).ToList();
         //}
         public VTable SearchSourceTable()
         {
             var tbl = SourceTable();
             if (tbl != null)
             {
                 return tbl;
             }

             return MainSource().Query().SearchSourceTable();
            
            
         }

         public VTable SourceTable()
         {
             VSXElement table = Get(GetMEIFromSections().Elements(TextConst.EName.Table).FirstOrDefault());

             return (table as VTable);
         }


         public bool IsNonDb()
         {
             if (P_ClientCalulation == TextConst.AVBool.True)
             {
                 return true;
             }
             var tbl = SourceTable();
            
             if (tbl == null)
             {
                 return false;
             }
             else
             {
                 return (tbl.P_ClientCalulation == TextConst.AVBool.True);
             }
             

         }

         public List<VSXElement> NativeColumns()
         {
             var tbl=SourceTable();
             List<VSXElement> list = null;
             if (tbl != null)
             {
                 list = Columns().Where(e => e.P_Table == tbl.XName && !TextConst.AVColumnArray.SysColNamesForEditedObject.Contains(e.P_Column) && e.XName == e.P_Column).ToList();
             }
             else
             {
                 list = new List<VSXElement>();
             }
             return list;

         }

         

         public VSXElement OriginalKeyColumn()
         {
             VSXElement table = SourceTable();
             if (table != null)
             {
                 return GetMainIE().SelfColumns().First(e => e.P_Table == table.XName);
             }
             else
             {
                 return GetMainIE().SelfColumns().FirstOrDefault();
             }
           
         }
         internal VSXElement KeyColumn()
         {
             IList<VSXElement> cols = this.Columns();
             IList<VSXElement> marked = cols.Where(e => e.P_Key == TextConst.AVBool.True).ToList();
             if (marked.Count == 1) {
                 return marked[0];
             } else {
                 IList<VSXElement> grCols = cols.Where(e => e.P_Group == TextConst.AVGroup.Group).ToList();
                 if (grCols.Count == 1) {
                     return grCols[0];
                 } else {
                     return this.OriginalKeyColumn();
                 }
             }
         }
         public VSXElement TreeParentColumn()
         {
             if (P_ParentFieldName == "") return null;
             return Columns().First(e => e.XName == P_ParentFieldName);
         }
         public bool IsExtNameColumn()
         {
             if (Columns().Where(e => e.P_IsNameColumn == TextConst.AVBool.True).Count() > 1)
             {
                 return true;
             }
             else
             {
                 return false;
             }
             
             
         }
       


         public List<VQuery> GetHeirs()
         {

             if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null))
             {
                 return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as List<VQuery>);
             }

             var list = new List<VQuery>();
             foreach (var q in XmlReports.Environment.GetElements(TextConst.EName.Queries).Where(q => q.P_Inherit == this.P_Name))
             {
                 list.Add((VQuery)q);
             }

             AddCashValue(list, MethodBase.GetCurrentMethod().ToString(), null);
             return list;

         }

         public VSXElement NameColumn()
         {

             if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null))
             {
                 return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as VSXElement);
             }

            

             var cols = NameColumns();

             VSXElement col = null;
             if (cols.Count == 1) {
                 col= cols.First();
             }             
             if (col == null && cols.Count > 0) {
                 var cc = VSXElement.Get(new XElement(TextConst.EName.Call,
                     new XAttribute(TextConst.AName.Function, TextConst.AVFunction.Concat),
                     new XAttribute(TextConst.AName.As, TextConst.SpecCols.Name),
                     new XAttribute(TextConst.AName.Title, string.Empty)));
                 cc.VirtualParent = cols.First().GetParent();
                 XElement q = null;
                 foreach (VSXElement col1 in cols)
                 {
                     if (q != null)
                     {

                         cc.Add(q);
                     }

                     XElement col2 = null;

                     if (col1 is VColumn)
                     {
                         col2 = (col1 as VColumn).AsNameColumnOrSelf();
                        
                     }
                     else
                     {
                         col2 = new XElement(col1);
                     }

                     cc.Add(col2);
                     
                     q = new XElement(TextConst.EName.Const, new XText("' '"));
                 }
                 col = cc;
             }


             AddCashValue(col, MethodBase.GetCurrentMethod().ToString(), null);
             return col;

         }
         internal List<VSXElement> NameColumns()
         {
             var cols = Columns().Where(e => e.P_IsNameColumn == TextConst.AVBool.True).ToList();
             if (cols.Count == 0) {
                 var col = Columns().FirstOrDefault(VSXElement.IsListColumn);
                 if (col == null) {
                     col = Columns().FirstOrDefault(e => e.P_Title != "");
                 }
                 if (col == null) {
                     col = KeyColumn();
                 }
                 if (col != null) {
                     cols = col.AsList();
                 }
             }
             return cols;
         }
         public override List<VSXElement> GetUsedElements()
         {
             return GetExtensionsAndParentAndMain().Cast<VSXElement>().ToList();
         }
         public List<XElement> CreateNameColumns()
         {


             List<XElement> list = new List<XElement>();

             foreach (var col in Columns())
             {
                 if (col is VColumn)
                 {
                     var ncol = (col as VColumn).CreateNameColumn();
                     if (ncol != null)
                     {
                         list.Add(ncol);
                     }
                 }
             }

             return list;
         }
         public VQube GetQubeElement()
         {
             return (this.GetMEIFromSections().SelectMany(e => e.GetElementsP(EName.qube)).FirstOrDefault() as VQube);
         }
         IList<string> IVParent.AllowedChildNodes()
         {
             var child_nodes = new List<string>(22);
             child_nodes.Add(TextConst.EName.Select);
             child_nodes.Add(TextConst.EName.From);
             child_nodes.Add(TextConst.EName.Where);
             child_nodes.Add(TextConst.EName.Group);
             child_nodes.Add(TextConst.EName.Having);
             child_nodes.Add(TextConst.EName.Columns);
             child_nodes.Add(TextConst.EName.Params);
             child_nodes.Add(TextConst.EName.Actions);
             child_nodes.Add(TextConst.EName.Events);
             child_nodes.Add(TextConst.EName.Grouping);
             child_nodes.Add(TextConst.EName.Expressions);
             child_nodes.Add(TextConst.EName.Content);
             child_nodes.Add(TextConst.EName.Additions);
             child_nodes.Add(TextConst.EName.Visualizers);
             child_nodes.Add(TextConst.EName.Links);
             child_nodes.Add(TextConst.EName.Connect);
             child_nodes.Add(TextConst.EName.Start);
             if (this.IsReport()) {
                 child_nodes.Add(TextConst.EName.Customers);
                 child_nodes.Add(TextConst.EName.Menu);
                 child_nodes.Add(TextConst.EName.ReportProc);
                 child_nodes.Add(TextConst.EName.PrintTemplates);
             }
             child_nodes.Add(TextConst.EName.UsePart);
             return child_nodes;
         }
         #region IdName
         public override string P_IdName {
             get {
                 return this.AttrOrEmpty(AName_.name);
             }
             set {
                 // Емцов - чтобы не падало, если случайно вводишь уже существующее имя
                 VQuery qry = XmlReports.Environment.GetQuery(value);
                 if (qry != null && qry != this) {
                     return;
                 }
                 this.SetIdName(AName_.name, value);
             }
         }

         public new bool P_IdName_Exists()
         {

             return true;

         }
         #endregion
         #region CalledQuery
         public new string P_CalledQuery
         {
             get
             {
                 return null;
             }
             set
             {
               
             }
         }

         public new void P_CalledQuery_ListRefresh(VDataTable table)
         {
            


         }

         public new bool P_CalledQuery_Exists()
         {

             return false;

         }
         #endregion
         #region NodeText
         public override string GetNodeTypeInfo()
         {
             string s = base.GetNodeTypeInfo();
             if (this.IsReport()) {
                 s += "(" + TextConst.EName.Report + ")";
             }
             return s;
         }
         public override string GetNodeInfo()
         {
             return GetNodeTypeInfo()+":"+GetNodeOtherInfo();
         }
         public override string GetNodeOtherInfo()
         {
             string s = Bold(this.AttrOrEmpty(AName_.name));
             string alias = this.P_Alias;
             if (!string.IsNullOrEmpty(alias)) {
                 s += " as " + Bold(alias);
             }
             VSourcedElement mq = GetMainE();
             VDimension d = XmlReports.Environment.GetDimensionByQueryName(mq.P_IdName);
             if (d != null) {
                 s += " dim " + Bold(d.P_Name);
             }
             string title = this.P_Title;
             if (!string.IsNullOrEmpty(title)) {
                 s += "  " + Italic(P_Title);
             }
             return s;
         }
         #endregion
         #region Alias

         public override bool P_Alias_Exists()
         {

             return GetParent() is VSelect;

         }

         #endregion
         #region Materialize

         public override bool P_Materialize_Exists()
         {

             return true;

         }
         #endregion
         #region Extend
         public override bool P_Extend_Exists()
         {

             return IsMainElement();

         }
         #endregion
         #region Invisible
   

         public override bool P_Invisible_Exists()
         {

             return IsReport();

         }
         #endregion
         #region AutoMerge
         public override bool P_AutoMerge_Exists()
         {
             return this.IsReport();
         }
         #endregion
         #region NoGrid
         public override bool P_NoGrid_Exists()
         {
             return this.IsReport();
         }
         #endregion
         #region SaveCompiled


         public override bool P_SaveCompiled_Exists()
         {

             return true;

         }
         #endregion
         #region Form



         public override bool P_Form_Exists()
         {

             return IsReport();

         }
        #endregion
        #region ViewMode
        public override bool P_ViewMode_Exists()
        {

            return IsReport();

        }
        #endregion
        #region AutoFilter
        public override bool P_AutoFilter_Exists()
        {

            return IsReport();

        }
        #endregion
        #region ParamsCustomization


        public override bool P_ParamsCustomization_Exists()
         {

             return IsReport();

         }
         #endregion
         #region AllowSave


         public override bool P_AllowSave_Exists()
         {

             return IsReport();

         }
         #endregion
         #region EditColumns


         public override bool P_EditColumns_Exists()
         {

             return IsReport();

         }

         #endregion
         #region Folder
         public override bool P_Folder_Exists()
         {
             return this.IsReport();
         }
         #endregion
         #region AddNames
        

         public override bool P_AddNames_Exists()
         {

             return true;

         }
         #endregion
        #region Stored
       

        public override bool P_Stored_Exists()
        {

            return GetParent().Name.LocalName == TextConst.EName.Queries;

        }
        #endregion
         //#region NameFieldName
         //public override void P_NameFieldName_ListRefresh(VDataTable table)
         //{
         //    table.Rows.Clear();
         //    foreach (VSXElement col in this.Columns())
         //    {
         //        table.Rows.Add(col.XName, col.XName, col.P_Title);
         //    }
         //}
         //public override bool P_NameFieldName_Exists()
         //{
         //    return true;
         //}
         //#endregion
         #region ClientCalulation
         public override bool P_ClientCalulation_Exists()
         {
             return true;
         }
         #endregion
         #region Deletable
        
         public override bool P_DeleteValidation_Exists()
         {

             return true;

         }
         #endregion
         #region ParentFieldName
         public override void P_ParentFieldName_ListRefresh(VDataTable table)
         {
             table.Rows.Clear();
             foreach (VSXElement el in Columns()) {
                 table.AddRow(el.XName, el.XName);
             }
         }
         public override string P_ParentFieldName {
             get {
                 return this.AttrOrEmpty(TextConst.AName.ParentNodeId);
             }
             set {
                 this.SetAttributeNotEmpty(TextConst.AName.ParentNodeId, value);
             }
         }
         public override bool P_ParentFieldName_Exists()
         {
             return true;
         }
         #endregion
         #region UseTemp
         public override bool P_UseTemp_Exists()
         {
             return this.IsReport();
         }
         #endregion
         #region UseDataReader
         public override bool P_UseDataReader_Exists()
         {
             return this.IsReport();
         }
         #endregion
         #region Order
         public override bool P_Order_Exists()
         {
             return true;
         }
         #endregion
         #region CanUseSimpleParams
         public override bool P_CanUseSimpleParams_Exists()
         {
             return true;
         }
         #endregion
         #region Interval
         public override bool P_Interval_Exists()
         {
             return true;
         }
         #endregion
         #region MultiSelect
         public override string P_MultiSelect {
             get {
                 return this.AttrOrEmpty(AName_.multi_select);
             }
             set {
                 this.SetAttributeValue(AName_.multi_select, value);
             }
         }        
         public override bool P_MultiSelect_Exists()
         {
             return true;
         }
         #endregion
         #region TableCode


         public override bool P_TableCode_Exists()
         {

             return true;

         }

         #endregion
    }
}