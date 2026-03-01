using System.Collections.Generic;
using System.Linq;

namespace sql.builder.DataApi
{
    public sealed class VGrid : VSXElement, IVParent
    {
        public VGrid()
            : base(EName.grid)
        {
        }
        public VQueryCall Source()
        {
            return this.RootQuery().AllSources().FirstOrDefault(e => e.XName == this.P_Table);
        }
        //public VColumn MultiSelectColumnSource()
        //{
        //    return (VColumn)MultiSelectColumn().SourceColumn().FirstOrDefault();
        //}
        private IList<VColumn> Columns()
        {
            return this.GetDescedantsP(EName.column).Cast<VColumn>().ToList();
        }
        /*public VColumn MultiSelectColumn()
        {
            return this.Columns().First(e => e.XName == P_MultiSelectColumn);
        }*/
        private static string[] child_nodes = { TextConst.EName.Columns, TextConst.EName.Toolbar, TextConst.EName.Menu, TextConst.EName.Events, TextConst.EName.UsePart };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
        #region Table
        public override bool P_Table_Exists()
        {
            return true;
        }
        public override void P_Table_ListRefresh(VDataTable table)
        {
            table.Rows.Clear();
            foreach (VQueryCall el in this.RootQuery().AllSources())
            {
                if (el is VFromQuery || el.GetType() == typeof(VELink))
                {
                    TableListRowFromElement(table, el);
                }
            }
        }
        public override string P_Table_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.MainMain;
        }
        #endregion
        #region NodeText
        public override string GetNodeOtherInfo()
        {
            return Bold(this.P_Table);
        }
        #endregion
        #region ColumnEditable
        public override bool P_ColumnEditable_Exists()
        {
            return true;
        }
        #endregion
        #region ShowToolBar
        public override bool P_ShowToolBar_Exists()
        {
            return true;
        }
        #endregion
        #region ShowBottomToolBar
        public override bool P_ShowBottomToolBar_Exists()
        {
            return true;
        }
        #endregion
        #region ShowFooter
        public override bool P_ShowFooter_Exists()
        {
            return true;
        }
        #endregion
        #region ShowCheckbox
        public override bool P_ShowCheckbox_Exists()
        {
            return true;
        }
        #endregion
        #region SelfTitle
        public override bool P_SelfTitle_Exists()
        {
            return true;
        }
        #endregion
        #region MultiSelect
        public override bool P_MultiSelect_Exists()
        {
            return true;
        }
        #endregion
        #region ParentFieldName
        public override void P_ParentFieldName_ListRefresh(VDataTable table)
        {
            table.Rows.Clear();
            foreach (VColumn el in this.Columns())
            {
                table.AddRow(el.P_Name, el.P_Name);
            }
        }
        public override bool P_ParentFieldName_Exists()
        {
            return true;
        }
        #endregion
        #region OrderFieldName
        public override void P_OrderFieldName_ListRefresh(VDataTable table)
        {
            table.Rows.Clear();
            foreach (VColumn el in this.Columns())
            {
                table.AddRow(el.P_Name, el.P_Name);
            }
        }
        public override bool P_OrderFieldName_Exists()
        {
            return true;
        }
        #endregion
        #region Alias
        public override string P_SelfTitle_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.MainOther;
        }
        #endregion
        #region DxExport
        public override string P_DxExport
        {
            get
            {
                if (this.AttrOrEmpty(TextConst.AName.DxExport) == TextConst.AVBool.False)
                {
                    return TextConst.AVBool.False;
                }
                else
                {
                    return TextConst.AVBool.True;
                }
            }
            set
            {
                if (value == TextConst.AVBool.True)
                {
                    value = null;
                }
                else
                {
                    value = TextConst.AVBool.False;
                }
                this.SetAttributeValue(TextConst.AName.DxExport, value);
            }
        }
        public override bool P_DxExport_Exists()
        {
            return true;
        }
        #endregion
        #region AutoFilter
        public override bool P_AutoFilter_Exists()
        {
            return true;
        }
        #endregion
        #region AllowSelectMoveColumns
        public override bool P_AllowSelectMoveColumns_Exists()
        {
            return true;
        }
        #endregion
    }
}