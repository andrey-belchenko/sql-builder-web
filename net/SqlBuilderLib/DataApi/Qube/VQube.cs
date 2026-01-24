using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Reflection;

namespace sql.builder.DataApi
{
    internal sealed class VQube : VQueryCall, IVParent
    {
        internal VQube()
            : base(EName.qube)
        {
        }
        internal IList<VSXElement> DimSets()
        {
            var list = this.GetElementsP(EName.dimset);
            return list;
        }
        internal VSXElement GetDimSet(string alias)
        {
            return this.DimSets().FirstOrDefault(e => e.AttrOrEmpty(TextConst.AName.As) == alias);
        }
        internal List<VQueryCall> AllQubeLinks()
        {
            var links = Links(null);
            links.AddRange(AllDimsetsLinks());
            return links;
        }
        private List<VQueryCall> AllDimsetsLinks()
        {

            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null))
            {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as List<VQueryCall>);
            }

           

         



            var links = new List<VQueryCall>();



            foreach (VSXElement dimSet in DimSets())
            {
                foreach (VQueryCall link in dimSet.GetElementsP().OfType<VQueryCall>())
                {
                    links.Add(link);
                }

            }


            AddCashValue(links, MethodBase.GetCurrentMethod().ToString(), null);
            return links;
        }

        internal List<VQueryCall> FactLinks()
        {

            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null))
            {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as List<VQueryCall>);
            }


     

            var links = new List<VQueryCall>();

            var factLinksElement = this.GetElementsP(EName.factlinks).FirstOrDefault();

            if (factLinksElement != null)
            {

                foreach (VQueryCall link in factLinksElement.GetElementsP().OfType<VQueryCall>())
                {
                    links.Add(link);
                }
            }
            AddCashValue(links, MethodBase.GetCurrentMethod().ToString(), null);
            return links;
        }
        /*internal List<VQueryCall> AllLinks()
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null))
            {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as List<VQueryCall>);
            }


            var links= AllLinks(null);

            AddCashValue(links, MethodBase.GetCurrentMethod().ToString(), null);
            return links;

         
        }*/
        public override List<VQueryCall> AllLinks(VSourcedElement heir)
        {
            var links = new List<VQueryCall>();

            foreach (VQueryCall link in AllQubeLinks())
            {
                links.Add(link);
                foreach (VQueryCall link1 in link.AllLinks(heir))
                {
                    links.Add(link1);
                }
            }
            return links;
        }
        internal List<VQueryCall> GetDimsetLinks(string dimsetAlias)
        {

            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), dimsetAlias))
            {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), dimsetAlias) as List<VQueryCall>);
            }
           
            var links = new List<VQueryCall>();

            var dimSet = GetDimSet(dimsetAlias);

            if (dimSet != null)
            {

                foreach (VQueryCall link in dimSet.GetElementsP().OfType<VQueryCall>())
                {
                    links.Add(link);
                }
            }
            AddCashValue(links, MethodBase.GetCurrentMethod().ToString(), dimsetAlias);
            return links;
        }
        private static string[] child_nodes = { TextConst.EName.Link, TextConst.EName.DimSet, TextConst.EName.Where, TextConst.EName.Factlinks, TextConst.EName.UsePart };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
        #region NodeText
        public override string GetNodeInfo()
        {
            return this.GetNodeTypeInfo();
        }
        #endregion
        #region MergeDimsets
        public override bool P_MergeDimsets_Exists()
        {
            return true;
        }
        #endregion
        #region StarScheme
        public override bool P_StarScheme_Exists()
        {
            return true;
        }
        #endregion
        #region SingleWay
        public override bool P_SingleWay_Exists()
        {
            return true;
        }
        #endregion
    }
}