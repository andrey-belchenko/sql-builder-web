using System.Linq;
using System.Xml.Linq;
using AName_ = sql.builder.DataApi.AName;

namespace sql.builder.DataApi
{
    public sealed class VRelation : VQueryCall
    {
        public VRelation()
            : base(EName.query)
        {
        }
        public VQuery ChildQuery()
        {
            XElement query = this.Ancestors(EName.query).First();
            return VSXElement.Get<VQuery>(query);
        }
        public VSXElement ChildColumnSource()
        {
            VSXElement col = this.GetDescedantsP(EName.column).First(e => P_Table != this.XName || P_Table == "");
            return RootQuery().SearchColumn(col.P_Column);
        }
        public VSXElement ParentColumn()
        {
            VSXElement col = this.GetDescedantsP(EName.column).First(e => P_Table == this.XName || P_Table == "");
            return col;
        }
        public VQuery ParentQuery()
        {
            XElement query = XmlReports.Environment.GetQuery(this.ParentName);
            return (VQuery)query;
        }
        public new string ParentName {
            get {
                return this.AttrOrEmpty(AName_.name);
            }
        }
        //public override string SName()
        //{
        //        return GetAttrValue("as");
        //}
        public string Title()
        {
            return this.AttrOrEmpty(AName_.title);
        }
        #region DName
        public override bool P_DName_Exists()
        {
            return true;
        }
        #endregion
        #region DTitle
        public override bool P_DTitle_Exists()
        {
            return true;
        }
        #endregion
        #region DXTitle
        public override string P_DXTitle {
            get {
                string s = this.P_DTitle;
                if (s == "") {
                    VSourcedElement qry = this.RootQuery();
                    if (qry != null) {
                        s = qry.P_SelfTitle;
                    }
                }
                return s;
            }
        }
        #endregion
    }
}