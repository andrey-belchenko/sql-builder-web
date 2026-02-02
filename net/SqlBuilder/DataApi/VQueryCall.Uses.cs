using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Reflection;
namespace sql.builder.DataApi
{


    public partial class VQueryCall : VSXElement 
    {
        #region old
        protected override List<ElementUse> searchUses()
        {
            var list = new List<ElementUse>();
            if (true)
            {
                var list1 = this.UsedColumns();
               

                foreach (var el1 in list1)
                {
                    list.Add(new ElementUse(this,el1, TextConst.AName.Table));
                }
            }


            if (this is VRelation)
            {
                var r = (this as VRelation);
                var list1 = XmlReports.Environment.GetSourcedElements();

                foreach (var el1 in list1)
                {
                    var list2 = el1.AllSources();
                    foreach (var el2 in list2)
                    {
                        if (el2 is VLink)
                        {
                            if (el2.GetRelation() == r)
                            {
                                var qry = el2.Query();
                                if (qry != null)
                                {
                                    if (qry.P_IdName==this.Query().P_IdName)
                                    {
                                        list.Add(new ElementUse(this,el2, TextConst.AName.Name));
                                        if (el2.XName == el2.P_CalledQuery)
                                        {
                                            var uses = el2.searchUses();
                                            list.AddRange(uses);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }


            return list;
        }
        #endregion

       
    }
}
