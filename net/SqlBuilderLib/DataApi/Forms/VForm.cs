using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Reflection;
using AName_ = sql.builder.DataApi.AName;

namespace sql.builder.DataApi
{
    internal sealed partial class VForm : VSourcedElement, IVParent
    {
        internal VForm()
            : base(EName.form)
        {
        }
        //private List<VSXElement> getSourceColumns(VQueryCall queryCall)
        //{
        //    var list1 = getColumns(queryCall);
        //    var list = list1.SelectMany(e => e.SourceColumn()).Distinct().ToList();
        //    return list;
        //}
        private IList<VColumn> GetQueryUsedColumns(VQueryCall queryCall)
        {
            var list1 = queryCall.SelfAndAllMasterLinks();
            var list2 = list1.SelectMany(e1 => e1.UsedColumns()).ToList();
            return list2;
        }
        private VSXElement getVirtcolumnExpr(VQuery sourceQuery, string name)
        {
            string[] ss = name.Split('.');
            VSXElement expr;
            if (ss[0] == TextConst.Pfx.QubeQueryAlias) {
                expr = VSXElement.Get(new XElement(EName.fact));
                //expr.environment = sourceQuery.GetEnvironment();
                expr.P_Column = ss[1];
            } else {
                expr = sourceQuery.SearchColumn(name);
            }
            return expr;
        }
        private List<VAction> GetAllActions()
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as List<VAction>);
            }
            List<VAction> list = VSXElement.GetDescedantsP(this).OfType<VAction>().ToList();
            AddCashValue(list, MethodBase.GetCurrentMethod().ToString(), null);
            return list;
        }
        internal List<VAction> GetRefreshColumnActions()
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as List<VAction>);
            }
            List<VAction> list = new List<VAction>();
            list = GetAllActions().Where(e => e.P_ActionType == TextConst.AVActionType.RefreshColumn).ToList();
            AddCashValue(list, MethodBase.GetCurrentMethod().ToString(), null);
            return list;
        }
        private List<VColumn> getColumns(VQueryCall queryCall)
        {
            string cashName = queryCall.XName;
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), cashName)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), cashName) as List<VColumn>);
            }
            IList<VColumn> list2 = this.GetQueryUsedColumns(queryCall);
            //List<VSXElement> calls = new List<VSXElement>();
            for (int index = list2.Count - 1; index >= 0; index--) {
                if (list2[index].GetAncestorsAndSelf(EName.call).Count != 0) {
                    list2.RemoveAt(index);
                }
            }

            //calls = calls.Distinct().ToList();
           

            List<VColumn> list = new List<VColumn>();
            List<string> names = new List<string>();
           // var usedBase = list2.Select(e => e.BaseElementOrSelf()).ToList();
            string[] usedIds = list2.SelectAsArray(e => e.P_Table + "." + e.P_Column);
            IList<VSXElement> orderedCols = this.GetElementsP(EName.content).First().GetDescedantsP(EPredicate.IsColumnOrFact).ToList();
            foreach (VColumn col in orderedCols ) {
                if (usedIds.Contains(col.P_Table + "." + col.P_Column) && !names.Contains(col.XName)) {
                    list.Add(col);
                    names.Add(col.XName);
                }
            }

            var sourceQuery = queryCall.Query();
            VSXElement parent =null;
            List<string> addNames = new List<string>();
        
            foreach (VColumn col in list.ToList())
            {
                VSXElement vidCol = null;
                VRelation rel = null;
                bool setAlias = true;
                var xp = "";
                if (col.P_TextSourceResult == "")
                {
                   
                    rel = col.TypeRelation(ref xp);


                    if (rel != null)
                    {

                        if (parent == null)
                        {
                            parent = list[0].GetParent();
                        }

                        vidCol = rel.ParentQuery().NameColumn();

                        

                    }
                }
                else
                {
                    var sn = GetSourceTextSourceName(col);
                    if (!string.IsNullOrEmpty(sn))
                    {
                        //vidCol = sourceQuery.SearchColumn(sn);
                        vidCol = getVirtcolumnExpr(sourceQuery, sn);
                    }
                    else
                    {
                        sn = GetClientTextSourceName(col);
                        if (!string.IsNullOrEmpty(sn))
                        {
                            setAlias = false;
                            vidCol = SearchVariableSource(sn);
                        }
                    }

                }

                if (vidCol != null)
                {
                    var relName = "";

                    if (rel != null)
                    {
                        relName = "." + rel.PName();
                    }
                    var col1 = addVirtualColToListIfNeed( col.Source().XName +xp + relName, vidCol, list);
                    if (setAlias)
                    {
                        col1.P_Alias = col.XName + TextConst.Pfx.ExtValName;
                    }

                    col1.P_SelfTitle = col.P_Title;
                    col1.P_DataType = vidCol.P_DataTypeS;
                    col1.P_FixedSide = col.P_FixedSide;
                    col1.IsAddisionForName = true;
                    col1.VirtualParent = parent;
                    col1.TextSourceFor = col.XName;
					col1.SetAttributeValue(TextConst.AName.InvisibleInColumnChooser, TextConst.AVBool.False);
					col.SetAttributeValue(TextConst.AName.InvisibleInColumnChooser, TextConst.AVBool.True);
                    col.IsRelation = true;

                    VSXElement srcCol = col.SourceColumn().First();

                    if (col.P_ColumnVisible == "")
                    {
                        col1.P_ColumnVisible = srcCol.P_ColumnVisible;
                    }
                    else
                    {
                        col1.P_ColumnVisible = col.P_ColumnVisible;
                    }
                }


                if (col.P_Table == queryCall.XName)
                {

                    var colSource = col.SourceColumn().FirstOrDefault();

                    List<string> behCols = null;
                    if (col is VFact)
                    {
                        behCols = TextConst.ANameArray.RoBehaviorColumns.ToList();

                    }
                    else
                    {
                        behCols = TextConst.ANameArray.BehaviorColumns.ToList();
                    }

                    if (colSource != null)
                    {
                        foreach (var attr in colSource.Attributes().Where(a => behCols.Contains(a.Name.LocalName)).ToArray())
                        {
                            if (!col.Attributes(attr.Name).Any())
                            {
                                var name = attr.Value;
                                if (!addNames.Contains(name))
                                {
                                    addNames.Add(name);
                                }
                            }
                        }
                        if (colSource is VColumn)
                        {
                            var ColsUsedAsParamsForList = (colSource as VColumn).ListQueryCallUsedColumns();

                            if (ColsUsedAsParamsForList != null)
                            {
                                foreach (VColumn col1 in ColsUsedAsParamsForList)
                                {
                                    if (!addNames.Contains(col1.P_Column))
                                    {
                                        addNames.Add(col1.P_Column);
                                    }
                                }
                            }
                        }
                    }


                }
             
            }

            foreach (VColumn col in  sourceQuery.VirtualSysColumns())
            {
                addNames.Add(col.XName);
            }

            if (sourceQuery.P_DeleteValidation != "")
            {
                addNames.Add(sourceQuery.P_DeleteValidation);
            }
            var qcolName=queryCall.P_Column;
            if (qcolName != "")// колонка значение в arrayeditvalue
            {
                if (!names.Contains(qcolName))
                {
                    addNames.Add(qcolName);
                }
            }

            foreach (string name in addNames)
            {
                VSXElement expr = getVirtcolumnExpr(sourceQuery,name);
                var col1 = addVirtualColToListIfNeed(queryCall.XName, expr, list);
                col1.P_Alias = expr.XName;
            //    col1.IsAddisionForName = true;
                if (parent == null)
                {
                    parent = list[0].GetParent();
                }
                col1.VirtualParent = parent;
            }
            


            

            //if (list.Count() > 0)
            //{
            var keyCol = sourceQuery.KeyColumn();
                
                // Добавление ключевой колонки

                if (keyCol != null)
                {

                    addVirtualColToListIfNeed(queryCall.XName, keyCol, list).IsKey = true;
                }

                if (queryCall is VELink)
                {
                    keyCol = queryCall.GetRelation().ChildColumnSource();
                    // Добавление  связующей колонки
                    addVirtualColToListIfNeed(queryCall.XName, keyCol, list);
                }

                //foreach (VQueryCall link in getOtherUpdatebleTables(queryCall))
                //{
                //    keyCol =link.Query().KeyColumn();
                //    addVirtualColToListIfNeed(link.XName, keyCol, list);
                //}

            //}

                AddCashValue(list, MethodBase.GetCurrentMethod().ToString(), cashName);
            return list;
        }

        private VColumn addVirtualColToListIfNeed(string table, VSXElement col, List<VColumn> list)
        {
            VColumn col1 = list.FirstOrDefault(e => e.P_Table == table && e.P_Column == col.XName);
            if (col1 == null) {
                var otherCol = list.FirstOrDefault();
                VSXElement parentEl;
                if (otherCol != null) {
                    parentEl = otherCol.GetParent();
                } else {
                    parentEl = this.GetContentSections().First();
                }
                col1 = createVirtualColumn(parentEl, table, col, null);
                list.Add(col1);
            }
            return col1;
        }
        private static VColumn createVirtualColumn(VSXElement parent, string table, VSXElement col, string alias)
        {
            XElement extKeyCol = null;
            if (col is VColumn) {
                extKeyCol = new XElement(col.Name);
            } else {
                extKeyCol = new XElement(EName.column);
            }
            extKeyCol.Add(new XAttribute(AName_.table, table));
            extKeyCol.Add(new XAttribute(AName_.column, col.XName));
            extKeyCol.Add(new XAttribute(AName_.invisible_in_column_chooser, TextConst.AVBool.True));
            if (alias != null) {
                extKeyCol.SetAttributeValue(AName_.@as, alias);
            }
            VColumn col1 = VSXElement.Get<VColumn>(extKeyCol);
            col1.IsAddision = true;
            col1.VirtualParent = parent;
            return col1;
        }
        private List<VQueryCall> MainQueries()
        {
            List<VQueryCall> list = GetNamedSections(TextConst.EName.From).SelectMany(VSXElement.GetElementsP).Cast<VQueryCall>().ToList();
            VSXElement pars = GetNamedSections(TextConst.EName.Params).FirstOrDefault();     
            if (pars != null) {
                var list1 = pars.GetElementsP().Where(e => (e as VParam).IsObject()).Cast<VQueryCall>().ToList();
                list.AddRange(list1);
            }
            return list;
        }
        internal List<VQueryCall> MainAndRelatedQueries()
        {
            return MainQueries().SelectMany(e => e.SelfAndELinks()).ToList();
        }
        internal VSXElement ContentElement()
        {
            IList<VSXElement> list = this.GetElementsP(EName.content);
            if (list.Count != 0) {
                return list[list.Count - 1];
            } else {
                return this;
            }
        }
        internal List<VSXElement> Fields()
        {
            return this.ContentElement().GetDescedantsP(EPredicate.IsFieldOrUseField);
        }
        internal List<VSXElement> Groups()
        {
            return this.ContentElement().GetDescedantsP(EName.fieldgroup);
        }
        //public VReturn GetReturnElement()
        //{
        //    return (VReturn)GetElementsApplyingParts(TextConst.EName.Return).FirstOrDefault();
        //}
        /*private VQueryCall GetReturnTable()
        {
            //var ret = GetReturnElement();
            //if (ret != null) {
            //   return ret.Source();
            //}
            return null;
        }*/
        public override List<VSXElement> Columns()
        {
            List<VSXElement> list = new List<VSXElement>();
            foreach (VSXElement el in this.GetContentSections()) { //.SelectMany(e => e.GetDescedantsP(EPredicate.IsColumnOrFact))) {
                list.AddRange(el.GetDescedantsP(EPredicate.IsColumnOrFact));
            }
            return list;
        }
        private List<VSXElement> ColumnsAndExpressions()
        {
            List<VSXElement> list = new List<VSXElement>();
            foreach (VSXElement el in this.GetContentSections()) { //  .SelectMany(e => e.GetDescedantsP(e1 => (e1.Name == EName.column) || (e1.Name == EName.fact) || (e1.Name == EName.call)))) {
                list.AddRange(el.GetDescedantsP(EPredicate.IsColumnOrFact));
                list.AddRange(el.GetDescedantsP(EName.call));
            }
            return list;
        }
        internal IList<VSXElement> VariableColumns()
        {
            return this.ColumnsAndExpressions().Where(VSXElement.HasParameterName).ToList();
        }
        private VSXElement ParamsElement()
        {
            return this.GetElementsP(EName.@params).FirstOrDefault();
        }
        internal VSXElement SearchVariableSource(string name)
        {
            VSXElement col = this.VariableColumns().FirstOrDefault(e => e.P_ParName == name);
            if (col == null) {
                col = this.Params().FirstOrDefault(e => e.P_FormalParName == name);
            }
            return col;
        }
        private static string[] child_nodes = { TextConst.EName.Params, TextConst.EName.From, TextConst.EName.Content, TextConst.EName.Where, 
                                                TextConst.EName.Field, TextConst.EName.UseField, TextConst.EName.FieldGroup, TextConst.EName.ScrollArea,
                                                TextConst.EName.Actions, TextConst.EName.Events, TextConst.EName.Toolbar, TextConst.EName.Customers, 
                                                TextConst.EName.Expressions, TextConst.EName.UsePart };
        IList<string> IVParent.AllowedChildNodes()
        {
            

            return child_nodes;
        }
        #region IdName
        public override string P_IdName {
            get {
                return this.AttrOrEmpty(AName_.name);
            }
            set {
                this.SetIdName(AName_.name, value);
            }
        }
        public override bool P_IdName_Exists()
        {
            return true;
        }
        #endregion
        #region ShowToolBar
        public override bool P_ShowToolBar_Exists()
        {
            return true;
        }
        #endregion
        #region FormSize
        public override bool P_FormSize_Exists()
        {
            return true;
        }
        #endregion
        #region AutoRefresh
        public override bool P_AutoRefresh_Exists()
        {
            return true;
        }
        #endregion
        #region SecurityId
        public override bool P_SecurityId_Exists()
        {
            return true;
        }
        #endregion
        #region Title
        public override string P_Title {
            get {
                return P_SelfTitle;
            }
        }
        #endregion
        #region Alias
        public override string P_Alias_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.MainOther;
        }
        #endregion
    }
}