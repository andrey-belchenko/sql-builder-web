using System.Collections.Generic;

namespace sql.builder.DataApi
{
    public partial class VPart : VSXElement
    {
        protected override List<ElementUse> searchUses()
        {
            var list = new List<ElementUse>();
            string partName = P_IdName; //get current part name
            IList<VSXElement> use_parts = XmlReports.Environment.GetUsePartElements(partName);
            for (int index = 0; index < use_parts.Count; index++)
            {
                list.Add(new ElementUse(this, use_parts[index], TextConst.AName.Part));
            }
            return list;
        }
    }
}