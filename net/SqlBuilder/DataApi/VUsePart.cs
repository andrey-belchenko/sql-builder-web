using System.Collections.Generic;
using AName_ = sql.builder.DataApi.AName;

namespace sql.builder.DataApi
{
    public sealed class VUsePart : VSXElement
    {
        public VUsePart()
            : base(EName.usepart)
        {
        }
        public VPart Part()
        {
            return XmlReports.Environment.GetPart(this.AttrOrEmpty(AName_.part));
        }

        public List<VSXElement> Content()
        {
            return Part().Content(this);
        }





        public static List<VSXElement> PartContentOrSelf(VSXElement element)
        {
            if (element is VUsePart)
            {
                return (element as VUsePart).Content();
            }

            var list = new List<VSXElement>();

            list.Add(element);

            return list;
        }

        public override bool IsElementUser()
        {
            return true;
        }

        public override List<VSXElement> GetUsedElements()
        {
            VSXElement p = Part();

            if (p != null)
            {
                if (p.VirtualParent != null)
                {
                    p = (VSXElement)p.VirtualParent;// для случая когда part определен через указание part-id
                }
            }
            return p.AsList();
        }

        #region Свойства

        #region Part
        public override bool P_Part_Exists()
        {

            return true;

        }
        #endregion

        #region NodeText

        public override string GetNodeInfo()
        {
            return GetNodeTypeInfo() + " " + GetNodeOtherInfo();

        }

        public override string GetNodeOtherInfo()
        {

            return Bold(P_Part);
        }

        #endregion

        #endregion
    }
}
