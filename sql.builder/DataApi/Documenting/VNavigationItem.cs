using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    /*internal class VNavigationItem:VSourcedElement
    {
        public VNavigationItem(XElement element)
            : base(element)
        {
            KeyField = TextConst.AName.Title;
    
        }
        public override string XName
        {
            get
            {
                return GetAttrValue("title");
            }
        }

        public string GetNavigationPath()
        {
            VSXElement el = this;
            string s = "";
            
            while (el is VNavigationItem)
            {
                s = el.P_SelfTitle + " / " + s;
                el = el.GetParent();
            }
            return s;
        }

        public override List<VSXElement> GetMEIFromSections()
        {
             var list = new List<VSXElement>();
            VSXElement el = this;

            while (el != null)
            {
               
                list.AddRange(
                    el.GetElementsP(TextConst.EName.From).ToList()
                    );
                el = el.GetParent();
            }
           
            return list;
        }

        #region NodeName

        public override void AllowedChildNodes(VDataTable table)
        {
            table.Rows.Add(TextConst.EName.NavigationItem, TextConst.EName.NavigationItem);
            table.Rows.Add(TextConst.EName.NavigationField, TextConst.EName.NavigationField);
            table.Rows.Add(TextConst.EName.From, TextConst.EName.From);
        }


        #endregion

        #region NodeText

        public override string GetNodeInfo()
        {
            return GetNodeOtherInfo();
        }

        public override string GetNodeOtherInfo()
        {
            return  Bold( P_SelfTitle);
        }
        #endregion

        #region SelfTitle

        public override bool P_SelfTitle_Exists()
        {

            return true;

        }
        #endregion



        #region Списки контекстного добавления элементов
        public override void MakeChildContextLists(XElement xlists)
        {

  
            MakeNavFieldsList(xlists);

        }

        public  void MakeNavFieldsList(XElement xlists)
        {
            
            var xlist = CreateContextListElement(xlists,TextConst.EName.NavigationField);
            foreach (VQueryCall qry in  AllSources())
            {
                VQuery query = qry.Query();
                if (query != null)
                {
                    foreach (VSXElement col in query.Columns())
                    {
                        XElement newCol = new XElement(TextConst.EName.NavigationField, new XAttribute("table", qry.XName), new XAttribute("column", col.XName));
                        xlist.Add(
                            newCol
                           );
                    }
                }
            }
        }
        #endregion



        //#region NavigationInfo



        //public override string P_NavigationInfo
        //{
        //    get
        //    {

        //        return GetNavigationPath();

        //    }

        //}

        
   
       

        //public override bool P_NavigationInfo_Exists()
        //{
          
        //    return true;

        //}
        //#endregion

    }*/
}
