using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using AName_ = sql.builder.DataApi.AName;

namespace sql.builder.DataApi
{
    public sealed class VViewColumn : VOutputElement
    {
        public VViewColumn()
            : base(EName.column)
        {
        }
        private VSXElement Column()
        {
            return this.RootQuery().SearchColumn(this.P_Column);
        }
        #region SelfTitle
        public override bool P_SelfTitle_Exists()
        {
            return true;
        }
        #endregion
        #region Column
        public override string P_Column {
            get {
                return this.AttrOrEmpty(AName_.name);
            }
            set {
                this.SetAttributeNotEmpty(AName_.name, value);
            }
        }
        public override void P_Column_ListRefresh(VDataTable table)
        {
            table.Rows.Clear();
            VSourcedElement rootQuery = this.ExtendedOrRootQuery();
            IList<VViewColumn> view_cols = rootQuery.ViewColumns();
            HashSet<string> names = new HashSet<string>();
            int index;
            string name;
            for (index = 0; index < view_cols.Count; index++) {
                name = view_cols[index].P_Column;
                if (!names.Contains(name)) {
                    names.Add(name);
                }
            }
            IList<VSXElement> cols = rootQuery.Columns();
            for (index = 0; index < cols.Count; index++) {
                VSXElement el = cols[index];
                name = el.XName;
                if (!names.Contains(name)) {
                    AddColumnInfoToList(table, name, el);
                }
            }
        }
        public override bool P_Column_Exists()
        {
            return true;
        }
        #endregion
        #region NodeText
        public override string GetNodeInfo()
        {
            return this.GetNodeOtherInfo();
        }
        public override string GetNodeOtherInfo()
        {
            return Bold(this.P_Column) + " " + Italic(this.P_Title);
        }
        #endregion
        #region Title
        public override string P_Title {
            get {
                string s = this.P_SelfTitle;
                if (string.IsNullOrEmpty(s)) {
                    VSXElement col = this.Column();
                    if (col != null) {
                        s = col.P_Title;
                    }
                }
                return s;
            }
        }
        public override bool P_Title_Exists()
        {
            return true;
        }
        #endregion
        #region Aggregation
        public override bool P_Aggregation_Exists()
        {
            return true;
        }
        #endregion
    }
}