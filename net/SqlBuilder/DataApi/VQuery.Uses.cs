using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Reflection;
namespace sql.builder.DataApi
{


    internal partial class VQuery
    {
        #region old
        protected override List<ElementUse> searchUses()
        {
            var list = new List<ElementUse>();
            var list1 = XmlReports.Environment.GetSourcedElements();

            foreach (var el1 in list1)
            {
                var list2 = el1.AllSources();
                foreach (var el2 in list2)
                {
                    if (el2.Name.LocalName != TextConst.EName.Table)
                    {
                        var qry = el2.Query();
                        if (qry != null)
                        {
                            if (qry.P_IdName == P_IdName && el2.P_CalledQuery == P_IdName && !(el2.GetParent() is VQube || el2.GetParent() is VDimSet)) //не измерение и link исключаются если не совпадает имя
                            {
                                //if (el2.GetMainParent().P_IdName == "sr_opl_bank_ext")
                                //{
                                //}
                                list.Add(new ElementUse(this,el2, TextConst.AName.Name));
                                if (el2.XName == el2.P_CalledQuery)
                                {
                                    if (el2.P_Alias != "")
                                    {
                                        list.Add(new ElementUse(this, el2, TextConst.AName.As));
                                    }
                                    var uses = el2.SearchUses();
                                    list.AddRange(uses);
                                }
                            }
                        }
                    }
                }

                
            }

            var d = GetDimension();
            if (d != null)
            {
                list.Add(new ElementUse(this, d, TextConst.AName.ClassType));
                if (d.P_Name == d.P_CalledQuery)
                {
                    list.Add(new ElementUse(this, d, TextConst.AName.Name));
                    var uses = d.SearchUses();
                    list.AddRange(uses);
                }
            }


            foreach (var col in Columns())
            {
                if (col.P_PrFact == TextConst.AVBool.True && col.P_FactName == "")
                {
                    var uses = SearchFactUses(col);
                    list.AddRange(uses);
                }
            }

            if (true)
            {
                var list2 = XmlReports.Environment.GetQueryCallsInFields();
                foreach (var el2 in list2)
                {
                    if (el2 != null)
                    {
                        var qry = el2.Query();
                        if (qry != null)
                        {
                            if (el2.P_CalledQuery == P_IdName)
                            {
                                list.Add(new ElementUse(this, el2, TextConst.AName.Name));

                            }
                        }
                    }
                       
                    
                }
               
            }

            if (this.EntityType != null)
            {

                foreach (var rel in this.EntityType.ParentLinks())
                {
                    if (rel.P_DName != "" && rel.P_DName==P_IdName)
                    {
                        list.Add(new ElementUse(this, rel, TextConst.AName.DName));
                    }
                }
            }


            foreach (var el in XmlReports.Environment.GetQueryUseByColumnLink(this.P_IdName))
            {
                list.Add(new ElementUse(this, el, TextConst.AName.Link));
            }

            return list;
        }
        #endregion
        public static List<ElementUse> SearchFactUses(VSXElement fact)
        {
            var list = new List<ElementUse>();
            var list1 =  XmlReports.Environment.GetSourcedElements();
            

            var fn = fact.P_Fact;
            if (fn != "")
            {
                foreach (var el1 in list1)
                {
                    foreach (var exp in el1.Expressions())
                    {
                        foreach (var col in exp.GetDescedantsP(EName.fact))
                        {
                            if (col.P_Column == fact.P_Fact)
                            {
                                list.Add(new ElementUse(fact, col, TextConst.AName.Column));
                            }
                        }
                    }
                }





                foreach (var exp in XmlReports.Environment.GetExpressions())
                {
                    if (fn != "")
                    {
                        foreach (var col in exp.GetDescedantsP(EName.fact))
                        {
                            if (col.P_Column == fact.P_Fact)
                            {
                                list.Add(new ElementUse(fact, col, TextConst.AName.Column));
                            }
                        }
                    }
                }
            }
           
            return list;
        }

        public  List<ElementUse> SearchColumnUses(VSXElement column)
        {
            var list = new List<ElementUse>();
            var list1 = XmlReports.Environment.GetSourcedElements();
            var d = this.GetDimension();

          

            foreach (var el1 in list1)
            {

                var list2 = el1.AllSources();
                foreach (var el2 in list2)
                {
                    if (el2.Name.LocalName != TextConst.EName.Table)
                    {
                        var qry = el2 as VQueryCall;
                        //if (qry != null)
                        //{
                            if (qry.P_IdName == this.P_IdName)
                            {

                                foreach (VColumn col in qry.UsedColumns())
                                {
                                    if (col.P_Column == column.XName)
                                    {
                                        list.Add(new ElementUse(column, col, TextConst.AName.Column));
                                    }
                                }
                            }
                        //}
                    }
                }

                foreach (var col in el1.AllUsedFacts())
                {
                    if (col.P_Column == column.XName)
                    {
                        list.Add(new ElementUse(column, col, TextConst.AName.Column));                         
                    }
                }


                if (d != null)
                {
                    foreach (var exp in el1.Expressions())
                    {
                         

                            foreach (var col in exp.GetDescedantsP(EName.column))
                            {
                                if (col.P_Table == d.P_Name && col.P_Column == this.P_Fact)
                                {
                                    list.Add(new ElementUse(column, col, TextConst.AName.Column));
                                }
                            }
                        
                    }
                }

                


            }

            if (d != null )
            {
                foreach (var exp in XmlReports.Environment.GetExpressions())
                {
                    

                        foreach (var col in exp.GetDescedantsP(EName.column))
                        {
                            if (col.P_Table == d.P_Name && col.P_Column == column.XName)
                            {
                                list.Add(new ElementUse(column, col, TextConst.AName.Column));
                            }
                        }
                    
                }
            }
            var uses = SearchFactUses(column);
            list.AddRange(uses);


            foreach (var col in Columns())
            {
                foreach (var att in TextConst.ANameArray.BehaviorColumns)
                {
                    if (Cmn.GetAttrValue(col, att) == column.XName)
                    {
                        list.Add(new ElementUse(column, col, att));
                    }
                }
            }

            return list;
        }


        public List<VSXElement>  Uses_QueryFrom()
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null))
            {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as List<VSXElement>);
            }
            List<VSXElement> list = new List<VSXElement>();

            var main = GetMainE();

            var queries = XmlReports.Environment.GetElements(TextConst.EName.Queries);
            

            foreach (VQuery query in queries)
            {
                foreach (VQueryCall qryCall in query.AllSources())
                {
                    var qry = qryCall.Query();
                    if (qry != null)
                    {
                        if (qry.GetUniqueKey() == main.GetUniqueKey())
                        {
                            list.Add(qryCall);
                        }
                    }
                } 
            }
            AddCashValue(list, MethodBase.GetCurrentMethod().ToString(), null);
            return list;

        }

        public List<VSXElement> Uses_Report()
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null))
            {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as List<VSXElement>);
            }
            List<VSXElement> list = new List<VSXElement>();

            var main = GetMainE();

            var elements = XmlReports.Environment.GetElements(TextConst.EName.Reports);


            foreach (VReport element in elements)
            {
                foreach (VQueryCall elemenUse in element.AllSources())
                {
                    var source = elemenUse.Query();
                    if (source != null)
                    {
                        if (source.GetUniqueKey() == main.GetUniqueKey())
                        {
                            list.Add(elemenUse);
                        }
                    }
                }
            }
            AddCashValue(list, MethodBase.GetCurrentMethod().ToString(), null);
            return list;

        }

    }
}
