using System;
using System.Collections.Generic;
using System.Linq;
using AName_ = sql.builder.DataApi.AName;

namespace sql.builder.DataApi
{
	internal partial class VField : VSXElement
	{
		protected override List<ElementUse> searchUses()
		{
			var list = new List<ElementUse>();
			string partName = this.AttrOrEmpty(AName_.id); //get current part name  //WRONG IdName is name instead of ID
			IList<VSXElement> use_fields = XmlReports.Environment.GetUseFieldElements(partName);
            for (int index = 0; index < use_fields.Count; index++) {
                list.Add(new ElementUse(this, use_fields[index], TextConst.AName.Field));
			}
			return list;
		}
	}
}