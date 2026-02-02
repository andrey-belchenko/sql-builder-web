using System;
using System.Data;
using System.Drawing;
//using System.Windows.Forms; // Control
using System.Xml.Linq;
//using DevExpress.XtraEditors.Repository;
//using BarEditItem = DevExpress.XtraBars.BarEditItem;
using sql.builder.DataApi;

namespace sql.builder.UI
{
    public partial class UICheck : UIBase
    {
        public UICheck()
            : base()
        {
        }
        public override bool IsBool()
        {
            return true;
        }
        protected override object GetNullObj()
        {
            //return this.GetValueUnchecked();
            return Cmn.DECIMAL_ZERO;
        }
        public override void Initialize(XElement xfield, UIFormC form)
        {
            if (form.FormUseType == UIFormC.UseType.SchemeEditor) {
                this.BaseInitialize(xfield, form, ReturnType.Simple, typeof(string), true, true);
                //this.SetValueChecked(TextConst.AVBool.True);
                //this.SetValueUnchecked(DBNull.Value);
            } else {
                this.BaseInitialize(xfield, form, ReturnType.Simple, typeof(decimal), true, true);
                //this.SetValueChecked(Cmn.DECIMAL_ONE);
                //this.SetValueUnchecked(Cmn.DECIMAL_ZERO);
            }
            this.InitControl();
        }
        public override void OnCheckedChanged()
        {
            base.OnCheckedChanged();
            //this.SetSourceValue(GetCheckEditValue());
        }
        public override void SetControlValue(object value, int index = 1)
        {
            if (this.UseType != UIFormC.UseType.SchemeEditor && value == DBNull.Value) {
                value = Cmn.DECIMAL_ZERO;
            }
            //this.SetCheckEditValue(value);
        }


        public void UpdateRepItemValue()
        {
            //BarEditItem item = ((Control)this.GetRootControl()).Tag as BarEditItem; // временно
            //object val1 = item.EditValue;
            //object val2 = this.GetSimpleSourceValue();
            //if (val2 == DBNull.Value) {
            //    val2 = this.GetValueUnchecked();
            //}
            //if (!object.Equals(val1, val2)) {
            //    item.EditValue = val2;
            //}
        }
        //public override RepositoryItem GetRepositoryItem()
        //{
        //    RepositoryItemCheckEdit rep = new RepositoryItemCheckEdit();
        //    rep.ValueChecked = this.GetValueChecked();
        //    rep.ValueUnchecked = this.GetValueUnchecked();
        //    rep.ValueGrayed = DBNull.Value;
        //    rep.Tag = this;
        //    rep.Appearance.BackColor = Color.Transparent;
        //    return rep;
        //}
        public override void RefreshData()
        {
            if (this.UseDefaultQuery && this.data_set_default != null) {
                XElement master_values = this.OnNeedMasterValues(this);
                this.data_set_default.Refresh(master_values);
                DataRowCollection rows = this.data_set_default.Tables[0].Rows;
                if (rows.Count > 0) {
                    object value = rows[0][0];
                    if ((!Cmn.IsNullOrDBNull(value)) && value.ToString() != TextConst.AVBool.False) {
                        this.SetChecked(true);
                    } else {
                        this.SetChecked(false);
                    }
                } else {
                    this.SetChecked(false);
                }
            } else {
                if (this.xfield.AttrOrDefault(AName.@checked, false) && this.Form.DefaultParams == null) {
                    this.SetChecked(true);
                }
            }
            base.RefreshData();
        }
        public override string GetText()
        {
            return this.Used ? "Да" : "Нет";
        }
    }
}