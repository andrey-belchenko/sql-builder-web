using System;
//using System.Windows.Forms;
//using DevExpress.XtraEditors;
using System.Xml.Linq;
using System.Xml;
namespace sql.builder.UI
{
    internal partial class UIBase
    {
        //        public void Dispose()
        //        {
        //            var table = Form.DataSource.GetTable(this.table_name);

        //            if (table != null)
        //            {
        //                table.AsyncLoadComplete -= VDataTable_OnAsyncLoadComplete;
        //                table.AsyncLoadCanceled -= VDataTable_OnAsyncLoadCanceled;
        //            }

        //            if (this.data_set_list != null)
        //            {
        //                this.data_set_list.SchemeChanged -= DataSource_SchemeChanged;
        //                DataTableList.TableRefreshed -= DataLocal_TableRefreshed;
        //            }
        //            if (SourceType == ReturnType.Array) {
        //                if (this.array_edit_value != null) {
        //                    this.array_edit_value.Changed -= VDataTable_Changed;
        //                }
        //                if (DataTableList != null) {
        //                    DataTableList.MyRowChanged -= DataLocal_RowValueChanged;
        //                }
        //            } // else{
        //    //var column = GetBoundColumn();
        //    //if (column != null) column.UnbindControl(this);
        //    // table.MyColumnChanged -= VDataTable_OnColumnChanged;
        //}
        //            var column = GetBoundColumn();// ��� Array ���� ����� ���� �������, ����� �������� ���������
        //            if (column != null) column.UnbindControl(this);
        //            detachTableEvents(table);
        //            if (this.xfield != null) {
        //                SaveStateToRegistry();
        //            }
        //var cRoot1 = (checkContainer as sql.builder.UI.WinForms.VCheckContainer); // ��������
        //if (!cRoot1.IsDisposed) cRoot1.Dispose();// ��������
        //        }

        //protected void SetProperiesForTextEx()// ������ ����� �� �����, ����� ������� ��� ���� ���������
        //{
        //    this.pEditors.Appearance.BackColor = System.Drawing.Color.Transparent;
        //    this.pEditors.Appearance.Options.UseBackColor = true;


        //}

        //        private IVCheckContainer checkContainer;

        //        public virtual void InitializeComponent(XElement xfield)
        //        {
        //            initializeComponent(xfield);
        //        }
        //        protected virtual void initializeComponent()
        //        {
        //        }
        //        protected virtual void initializeComponent(XElement xfield)
        //        {
        //            initializeComponent();
        //        }
        //        protected void BeginInitializeBase()
        //        {
        //            checkContainer = UIStatic.GetControlsfactory().CreateCheckContainer(this);

        //  checkContainer = new sql.builder.UI.WinForms.VCheckContainer();

        //            ////////////////////////////////////////
        //cRoot1 = new DevExpress.XtraEditors.XtraUserControl();
        //  this.pSettings1 = new DevExpress.XtraEditors.PanelControl();
        // this.ceUsed1 = new DevExpress.XtraEditors.CheckEdit();
        //this.pEditors = UIStatic.GetControlsfactory().CreatePanel();

        //  ((System.ComponentModel.ISupportInitialize)(this.pSettings1)).BeginInit();
        //  this.pSettings1.SuspendLayout();


        //      ((System.ComponentModel.ISupportInitialize)(this.ceUsed1.Properties)).BeginInit();
        //  ((System.ComponentModel.ISupportInitialize)(this.pEditors)).BeginInit();
        //cRoot1.SuspendLayout();


        //((System.ComponentModel.ISupportInitialize)(this.pEditors)).BeginInit();
        //this.pEditors.SuspendLayout();

        // this.pEditors.BeginInit();

        // 
        // pSettings1
        // 
        //this.pSettings1.Appearance.BackColor = System.Drawing.Color.Transparent;
        //this.pSettings1.Appearance.Options.UseBackColor = true;
        //this.pSettings1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
        //this.pSettings1.Dock = System.Windows.Forms.DockStyle.Left;
        //this.pSettings1.Location = new System.Drawing.Point(0, 0);
        //this.pSettings1.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
        //this.pSettings1.Name = "pSettings1";
        //this.pSettings1.Padding = new System.Windows.Forms.Padding(2, 1, 0, 0);
        //this.pSettings1.Size = new System.Drawing.Size(19, 20);
        //this.pSettings1.TabIndex = 4;


        //   AddPart1(ceUsed1, PartDest.Settings);
        // 
        // ceUsed1
        // 
        //this.ceUsed1.Location = new System.Drawing.Point(0, 1);
        //this.ceUsed1.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
        //this.ceUsed1.Name = "ceUsed1";
        //this.ceUsed1.Properties.Caption = "";
        //this.ceUsed1.Properties.ValueChecked = new decimal(new int[] {
        //1,
        //0,
        //0,
        //0});
        //this.ceUsed1.Properties.ValueGrayed = new decimal(new int[] {
        //999,
        //0,
        //0,
        //-2147483648});
        //this.ceUsed1.Properties.ValueUnchecked = new decimal(new int[] {
        //0,
        //0,
        //0,
        //0});
        //this.ceUsed1.Size = new System.Drawing.Size(20, 19);
        //this.ceUsed1.TabIndex = 4;
        //this.ceUsed1.EditValueChanged += new System.EventHandler(this.onCheckedChanged);


        //            checkContainer.CheckedChanged += checkContainer_CheckedChanged;

        // 
        // pEditors
        // 


        //this.pEditors.Appearance.BackColor = System.Drawing.Color.Transparent;
        //this.pEditors.Appearance.Options.UseBackColor = true;
        //this.pEditors.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
        //this.pEditors.Dock = System.Windows.Forms.DockStyle.Fill;
        //this.pEditors.Location = new System.Drawing.Point(19, 0);
        //this.pEditors.Name = "pEditors";
        //this.pEditors.Size = new System.Drawing.Size(201, 20);
        //this.pEditors.TabIndex = 5;

        // UIBase
        //cRoot1.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        //cRoot1.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        //            ////cRoot1.DoubleBuffered = true;
        //cRoot1.Margin = new System.Windows.Forms.Padding(0);
        //cRoot1.Name = "UIBase";
        //cRoot1.Size = new System.Drawing.Size(220, 20);

        // ������� �����
        // AddPart1( (pEditors as VPanel).GetMainControl() , PartDest.Root);// ��������



        //AddPart1(pSettings1, PartDest.Root);

        //        }

        //        private bool? _checkd = null;
        //        void checkContainer_CheckedChanged(object value)
        //        {
        //            used = (bool)value;
        //            if (_checkd != used)
        //            {
        //                onCheckedChanged();
        //            }
        //            _checkd = used;
        //        }


        //        protected void EndInitializeBase()
        //        {
        //            checkContainer.EndInit();
        //            ///////////////////////////
        //((System.ComponentModel.ISupportInitialize)(this.pSettings1)).EndInit();
        //this.pSettings1.ResumeLayout(false);
        //((System.ComponentModel.ISupportInitialize)(this.ceUsed1.Properties)).EndInit();
        //((System.ComponentModel.ISupportInitialize)(this.pEditors)).EndInit();
        //cRoot1.ResumeLayout(false);



        //((System.ComponentModel.ISupportInitialize)(this.pEditors)).EndInit();
        //this.pEditors.ResumeLayout(false);

        //   this.pEditors.EndInit();
        //        }

        // private DevExpress.XtraEditors.CheckEdit ceUsed1;
        //  private DevExpress.XtraEditors.PanelControl pSettings1;
        //  private DevExpress.XtraEditors.PanelControl pEditors;

        //private IVPanel pEditors;
        //  private DevExpress.XtraEditors.XtraUserControl cRoot1;

        //protected void AddPart1(Control control, PartDest dest)
        //{
        //    switch (dest)
        //    {
        //        case PartDest.Root:
        //            cRoot1.Controls.Add(control);
        //            break;
        //        case PartDest.Settings:
        //            pSettings1.Controls.Add(control);
        //            break;
        //        default:
        //            throw new ArgumentOutOfRangeException("dest", dest, null);
        //    }
        //}


        //        public void AddPart(Control control) // �������� �. ���� IVControl
        //        {

        //                (checkContainer as sql.builder.UI.WinForms.VCheckContainer).GetContentPanel().Controls.Add(control); // ��������

        //        }



        //        private IVControl editor = null;
        //        public void SetEditor(IVControl control) 
        //        {
        //            editor = control;
        //            checkContainer.AddChild(control as IVControl);

        //        }




        //        public IVCheckContainer GetRootControl() // ��������
        //        {

        //            return checkContainer;
        // return (checkContainer as sql.builder.UI.WinForms.VCheckContainer) as XtraUserControl;// ��������
        //        }

        //        private object valueUnchecked;
        //        private object valueChecked;
        //        protected void SetValueUnchecked(object val)
        //        {
        //            valueUnchecked = val;
        //ceUsed1.Properties.ValueUnchecked = val;
        //        }

        //        protected void SetValueChecked(object val)
        //        {
        //            valueChecked = val;
        // ceUsed1.Properties.ValueChecked = val;
        //        }

        protected object GetValueUnchecked()
        {
            return Cmn.DECIMAL_ZERO;
        }

        protected object GetValueChecked()
        {
            return Cmn.DECIMAL_ONE;
        }
        internal void SetChecked(bool value)
        {
            this.used = value;
            //this.checkContainer.SetChecked(used);
            //ceUsed1.Checked = value;
        }
        public void SetUnChecked()
        {
            //ceUsed1.Checked = false;
            this.used = false;
        }
        //        private bool checkEnabled = true;
        //        public void SetCheckEnabled(bool value)
        //        {
        //            checkEnabled = value;
        //            checkContainer.SetCheckEnabled(checkEnabled && !getCheckReadonly());
        //ceUsed1.Enabled = value;
        //        }
        //        private bool checkReadOnly = false;


        //        private bool getCheckReadonly()
        //        {

        //            if (this.Form.FormUseType == UIFormC.UseType.ParamEditor)
        //            {

        //                if (this is UICombo && this.mandatory) //��������, � ������ �������� (Paramseditor) ����� ��� ������� ������������, ����� ������ �� ����� ��� , � ����� ��������(DataEditor) ������ ������ - ������������� ��������� 
        // 20170608 ������� �������� �� mandatory
        //                {
        //                    return true;
        //                }
        //            }
        //            return checkReadOnly;
        //        }

        //        public void SetCheckReadOnly(bool value)
        //        {
        //            checkReadOnly = value;
        //            checkContainer.SetCheckEnabled(checkEnabled && !getCheckReadonly());
        //ceUsed1.ReadOnly = value;
        //        }

        //        public void SetCheckVisible(bool value)
        //        {
        //            checkContainer.SetCheckVisible(value);
        //ceUsed1.Visible = value;
        //(checkContainer as VCheckContainer).GetSettingsPanel().Visible = value; // ��������

        //        }

        //        protected void SetCheckEditValue(object val)
        //        {

        //            if (valueChecked.Equals(val))
        //            {
        //                checkContainer.SetChecked(true);
        //            }
        //            else if (valueUnchecked.Equals(val))
        //            {

        //                checkContainer.SetChecked(false);
        //            }
        //            else if (Cmn.Nvle(valueUnchecked, null) == Cmn.Nvle(valueUnchecked, null))
        //            {

        //                checkContainer.SetChecked(false);
        //            }
        //            else
        //            {
        //                throw new NotImplementedException();
        //            }
        //ceUsed1.EditValue = val;
        //        }

        //        protected object GetCheckEditValue()
        //        {
        //            if (used)
        //            {
        //                return valueChecked;
        //            }
        //            else
        //            {
        //                return valueUnchecked;
        //            }
        // return ceUsed1.EditValue;
        //        }


        //        public void SetRootName(string value)
        //        {
        //            checkContainer.SetName(value);
        //(checkContainer as sql.builder.UI.WinForms.VCheckContainer).Name = value; 
        //        }
        //        public IVCheckContainer GetCheckContainer() // ��������
        //        {
        //            return  checkContainer;
        //        }

        //        public BaseEdit GetCheckControl() // ��������
        //        {
        //            return (BaseEdit)(checkContainer as sql.builder.UI.WinForms.VCheckContainer).GetCheckControl();
        //        }

        //        public Control GetEditorsPanelControl()
        //        {

        //            return (checkContainer as sql.builder.UI.WinForms.VCheckContainer).GetContentPanel();// ��������
        //        }


        protected enum PartDest
        {
            Root,
            Editors,
            Settings
        }
    }
}
