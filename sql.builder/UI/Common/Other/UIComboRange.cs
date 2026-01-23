using System;
using System.Data;
using System.Linq;
//using System.Windows.Forms;
using System.Xml.Linq;
//using DevExpress.XtraEditors.Controls;

namespace sql.builder.UI
{
    internal partial class UIComboRange : UIBase, IRange
    {
        public UIComboRange()
        {
            //InitializeComponent();
        }

        public override void Initialize(XElement xfield, UIFormC form)
        {
            throw new NotImplementedException();
            BaseInitialize(xfield, form, ReturnType.SimpleRange, null, true);
            InitControl();
        }

        protected override void InitControl()
        {
            //luControlFrom.ErrorIconAlignment = ErrorIconAlignment.MiddleRight;
            //luControlTo.ErrorIconAlignment = ErrorIconAlignment.MiddleRight;

            //luControlFrom.Properties.ValueMember = value_field_name;
            //luControlFrom.Properties.DisplayMember = name_field_name;
            //luControlFrom.Properties.DataSource = DataTableList;


            //luControlTo.Properties.ValueMember = value_field_name;
            //luControlTo.Properties.DisplayMember = name_field_name;
            //luControlTo.Properties.DataSource = DataTableList;

            //luControlFrom.EditValueChanged += (sender, args) => SetSourceValue(luControlFrom.EditValue, 1);
            //luControlTo.EditValueChanged += (sender, args) => SetSourceValue(luControlTo.EditValue, 2);

            //if (Form.NoData) return;

            //foreach (DataColumn col in DataTableList.Columns)
            //{
            //    if (col.ColumnName == "absent" || col.ColumnName == "check") continue;

            //    if (luControlFrom.Properties.Columns.Count >= 2 && (col.Caption == "" || col.Caption == col.ColumnName))
            //    {
            //        continue;
            //    }

            //    var columnInfoFrom = new LookUpColumnInfo(col.ColumnName, col.Caption);
            //    var columnInfoTo = new LookUpColumnInfo(col.ColumnName, col.Caption);

            //    var is_pk = DataTableList.PrimaryKey.Contains(col);
            //    columnInfoFrom.Visible = !is_pk;
            //    columnInfoTo.Visible = !is_pk;

            //    luControlFrom.Properties.Columns.Add(columnInfoFrom);
            //    luControlTo.Properties.Columns.Add(columnInfoTo);
            //}
        }

        public override void SetControlValue(object value, int index = 1)
        {
            //switch (index)
            //{
            //    case 1:
            //        luControlFrom.EditValue = value;
            //        break;
            //    case 2:
            //        luControlTo.EditValue = value;
            //        break;
            //}
        }
        public override void RefreshData()
        {
            //XElement master_values = OnNeedMasterValues(this);
            //this.data_set_list.Refresh(master_values);
            //if (this.UseDefaultQuery && this.data_set_default != null) {
            //    this.data_set_default.Refresh(master_values);
            //    DataRowCollection rows = this.data_set_default.Tables[0].Rows;
            //    if (rows.Count > 0) {
            //        object value = rows[0][0];
            //        this.luControlFrom.EditValue = value;
            //        this.luControlTo.EditValue = value;
            //    }
            //}
            // из-за особенностей работы LookUp дефолтные значения не заполнялись
            //else if (Form.FormUseType == UIFormC.UseType.ParamEditor)
            //{
            //    if (Mandatory == "1" && DataLocal.Rows.Count > 0)
            //    {
            //        if (Cmn.Nvl(luControlFrom.EditValue, null) == null) luControlFrom.EditValue = DataLocal.Rows[0][0];
            //        if (Cmn.Nvl(luControlTo.EditValue, null) == null) luControlTo.EditValue = DataLocal.Rows[0][0];
            //    }
            //}
            base.RefreshData();
        }
        public override void SetError(string text, int index = 1)
        {
            SetErr(text);
            //switch (index)
            //{
            //    case 1:
            //       // luControlFrom.ErrorText = text;
            //        SetError(luControlFrom, text);
            //        break;
            //    case 2:
            //        //luControlTo.ErrorText = text;
            //        SetError(luControlTo, text);
            //        break;
            //}
        }
        #region IRange
        void IRange.GetText(out string value_1, out string value_2)
        {
            throw new NotImplementedException();
            //if (this.UseType == UIFormC.UseType.DataEditor) {
            //    value_1 = this.GetBoundColumn(1).GetFieldValueName();
            //    value_2 = this.GetBoundColumn(2).GetFieldValueName();
            //} else {
            //    value_1 = this.luControlFrom.Text;
            //    value_2 = this.luControlTo.Text;
            //}
        }
        #endregion
    }
}