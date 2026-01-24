using System.Xml.Linq;

namespace sql.builder.DataApi
{
    /*internal class VNavigationField:VColumn
    {
        public VNavigationField(XElement element)
            : base(element)
        {
          
    
        }

        public override VSourcedElement RootQuery()
        {

            return (VSourcedElement)GetParent();
        }

        public string GetNavigationPath()
        {
            return (GetParent() as VNavigationItem).GetNavigationPath() + P_SelfTitle;
        }
        #region NodeText

        public override string GetNodeOtherInfo()
        {
            string s = "";
            string title=P_SelfTitle;
            if (title != "")
            {
                title = Bold(title);
            }
            else
            {
                title = P_Title;
            }
            s += title + " " + P_Table + "." + P_Column;
                
           
            return s;
        }
        #endregion
    }*/
}
