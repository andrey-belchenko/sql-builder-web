using System.Linq;
using System.Xml.Linq;
using AName_ = sql.builder.DataApi.AName;

namespace sql.builder.DataApi
{
    public sealed class VCustomerUse : VSXElement
    {
        public VCustomerUse()
            : base(EName.customer)
        {
        }
        #region Customer
        public override string P_Customer
        {
            get
            {
                return this.AttrOrEmpty(AName_.id);
            }
            set
            {
                this.SetAttributeNotEmpty(AName_.id, value);
            }
        }
        public override bool P_Customer_Exists()
        {
            return true;
        }
        public void P_Customer_List(VDataTable table)
        {
            table.AddColumn("id", "Код");
            table.AddColumn("name", "Наименование");
        }
        public void P_Customer_ListRefresh(VDataTable table)
        {
            table.Rows.Clear();
            foreach (XElement el in XmlReports.Environment.Manager.GetNativeScheme().Elements(EName.customers).Elements(EName.customer))
            {
                if (EPredicate.IsNotExcuded(el))
                {
                    table.AddRow(el.AttrOrEmpty(AName_.id), el.AttrOrEmpty(AName_.title));
                }
            }
        }
        #endregion
        #region Title
        public override string P_Title
        {
            get
            {
                XElement el = XmlReports.Environment.Manager.GetNativeScheme().Elements(EName.customers).Elements(EName.customer).Where(EPredicate.IsNotExcuded).SearchByAttribute(AName_.id, this.P_Customer);
                if (el == null)
                {
                    return string.Empty;
                }
                else
                {
                    return el.AttrOrEmpty(AName_.title);
                }
            }
        }
        public override string P_Title_Title()
        {
            return "Наименование";
        }
        public override bool P_Title_Exists()
        {
            return true;
        }
        #endregion
        #region NodeText
        public override string GetNodeOtherInfo()
        {
            return this.P_Customer + " " + Italic(this.P_Title);
        }
        #endregion
    }
}