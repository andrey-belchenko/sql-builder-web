using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    /*internal class VPrintColumn:VSXElement
    {
        public VPrintColumn(XElement element)
            : base(element)
        {

    
        }

        public override void LookUpNextSources(List<VSXElement> list, VLookupAnalyzer analyzer)
        {
            
            var els = GetElementsP();
            foreach (VSXElement el in els)
            {
                el.LookUpNextSources(list, analyzer);
            }
        }

        #region NodeText

        public override string GetNodeInfo()
        {
            return GetNodeOtherInfo();
        }

        public override string GetNodeOtherInfo()
        {
            return  P_SelfTitle;
        }
        #endregion

        #region SelfTitle

        public override bool P_SelfTitle_Exists()
        {

            return true;

        }
        #endregion


        //#region DocDataType
        //public override string P_DocDataType
        //{
        //    get
        //    {
        //        return
        //            VDocumenting.GetQueryColumnType(
        //            ((VColumn)GetElementsApplyingParts().FirstOrDefault())
        //            );
        //    }

        //}
        //public override bool P_DocDataType_Exists()
        //{

        //    return true;

        //}
        //#endregion


        //#region DescriptionSearched
        //public override string P_DescriptionSearched
        //{
        //    get
        //    {
        //        string s = base.P_DescriptionSearched;

        //        if (s == "")
        //        {
        //            s = GetElementsApplyingParts().Where(e => e is VColumn).Select(e1 => (e1 as VColumn).P_Title).Where(s1=>Cmn.Nvl(s1,"").ToString()!="").FirstOrDefault();
        //            if (s == null){
        //                s = "";
        //            }
        //        }
        //        return s;

        //    }

        //}

        
        //#endregion
    }*/
}
