using System.Collections.Generic;
using System.Linq;
using AName_ = sql.builder.DataApi.AName;

namespace sql.builder.DataApi
{
    public sealed class VAddition : VSXElement
    {
        public VAddition()
            : base(EName.addition)
        {
        }
        public override bool IsElementUser()
        {
            return true;
        }
        private VQuery Query()
        {
            VQuery q = XmlReports.Environment.GetQuery(this.P_CalledQuery);
            return q;
        }
        public override List<VSXElement> GetUsedElements()
        {
            VQuery query = Query();
            if (query != null)
            {
                return query.GetExtensionsAndParentAndMain().ToList<VSXElement>();
            }
            return null;
        }
        #region CalledQuery
        public override string P_CalledQuery
        {
            get
            {
                return this.AttrOrEmpty(AName_.name);
            }
            set
            {
                this.SetAttributeNotEmpty(AName_.name, value);
            }
        }
        public override string P_CalledQuery_Title()
        {
            return "Запрос";
        }
        public override void P_CalledQuery_ListRefresh(VDataTable table)
        {
            table.Rows.Clear();
            foreach (VQuery el in XmlReports.Environment.GetElements(TextConst.EName.Queries))
            {
                if (!el.IsExtension() && el.GetQubeElement() != null)
                {
                    table.Rows.Add(el.P_IdName, el.P_IdName, el.P_Title);
                }
            }
        }
        public override bool P_CalledQuery_Exists()
        {
            return true;
        }
        #endregion
        #region NodeText
        public override string GetNodeOtherInfo()
        {
            return Bold(this.P_CalledQuery);
        }
        #endregion
    }
}