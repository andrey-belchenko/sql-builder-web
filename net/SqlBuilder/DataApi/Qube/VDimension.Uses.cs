using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Reflection;
namespace sql.builder.DataApi
{


    internal partial class VDimension : VSXElement 
    {
        #region old
        protected override List<ElementUse> searchUses()
        {
            var list = new List<ElementUse>();
            if (true)
            {
                var list1 = XmlReports.Environment.GetElements(TextConst.EName.Queries);

                foreach (VQuery qry in list1)
                {
                    foreach (VSXElement el in qry.Columns())
                    {
                        if (el.P_Dimension == this.P_Name)
                        {
                            list.Add(new ElementUse(this, el, TextConst.AName.Dimension));
                        }
                    }
                }
            }


            if (true)
            {
                var list1 = XmlReports.Environment.GetSourcedElements();
                foreach (var el1 in list1)
                {
                   
                    var list2 = el1.AllSources();
                    foreach (var el2 in list2)
                    {
                        if ((el2.GetParent() is VQube || el2.GetParent() is VDimSet))
                        {
                            if (el2.P_CalledQuery==this.P_Name)
                            {
                                list.Add(new ElementUse(this, el2, TextConst.AName.Name));
                                if (el2.XName == el2.P_CalledQuery)
                                {
                                    var uses = el2.SearchUses();
                                    list.AddRange(uses);
                                }
                            }
                            
                        }
                    }


                    foreach (var el2 in el1.GetDescedantsP(EName.dimlink))
                    {
                       
                        if (el2.P_CalledQuery == this.P_Name)
                        {
                            list.Add(new ElementUse(this, el2, TextConst.AName.Name));
                            
                        }

                        
                    }

                    foreach (var exp in el1.Expressions())
                    {
                        foreach (var col in exp.GetDescedantsP(EName.column))
                        {
                            if (col.P_Table == this.P_Name)
                            {
                                list.Add(new ElementUse(this, col, TextConst.AName.Column));
                            }
                        }
                    }



                }
            }

            foreach (var exp in XmlReports.Environment.GetExpressions())
            {
                foreach (var col in exp.GetDescedantsP(EName.column))
                {
                    if (col.P_Table == this.P_Name)
                    {
                        list.Add(new ElementUse(this, col, TextConst.AName.Column));
                    }
                }
            }
            return list;
        }
        #endregion

       
    }
}
