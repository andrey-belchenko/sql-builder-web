//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Xml.Linq;
//using System.Text;
////using System.Windows.Forms;
////using DevExpress.XtraEditors;
////using DevExpress.XtraEditors.Controls;
//
//namespace sql.builder.UI.WinForms
//{
//    class VCheckContainer : XtraUserControl, IVCheckContainer
//    {
//        private PanelControl pEditors;
//
//        protected override void Dispose(bool disposing)
//        {
//            if(disposing)
//            {
//                _uibase.Dispose();
//                base.Dispose(disposing);
//            }
//        }
//        private CheckEdit ceUsed1;
//        private PanelControl pSettings1;
//        private XtraUserControl cRoot1;
//        private IBase _uibase=null;
//
//        
//        public VCheckContainer(IBase uibase)
//        {
//            _uibase = uibase;
//            createControls();
//            BeginInit();
//            setControlsProperties();
//          //  EndInit();
//        }
//
//        public int GetWidthDisplacement()
//        {
//            if (_uibase is UICustom)
//            {
//                var co = (_uibase as UICustom).GetControlObject();
//                if (co is ICustomDisplacement)
//                {
//                    return (co as ICustomDisplacement).GetWidthDisplacement();
//                }
//            }
//            
//
//                return 0;
//            
//
//
//        }
//
//        private void createControls()
//        {
//            cRoot1 = this;
//            pSettings1 = new PanelControl();
//            ceUsed1 = new CheckEdit();
//            pEditors = new PanelControl();
//        }
//
//        private void setControlsProperties()
//        {
//            this.pSettings1.Appearance.BackColor = System.Drawing.Color.Transparent;
//            this.pSettings1.Appearance.Options.UseBackColor = true;
//            this.pSettings1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
//            this.pSettings1.Dock = System.Windows.Forms.DockStyle.Left;
//            this.pSettings1.Location = new System.Drawing.Point(0, 0);
//            this.pSettings1.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
//            this.pSettings1.Name = "pSettings1";
//            this.pSettings1.Padding = new System.Windows.Forms.Padding(2, 1, 0, 0);
//            this.pSettings1.Size = new System.Drawing.Size(19, 20);
//            this.pSettings1.TabIndex = 4;
//
//            pSettings1.Controls.Add(ceUsed1);
//           
//            // 
//            // ceUsed1
//            // 
//            this.ceUsed1.Location = new System.Drawing.Point(0, 1);
//            this.ceUsed1.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
//            this.ceUsed1.Name = "ceUsed1";
//            this.ceUsed1.Properties.Caption = "";
//            this.ceUsed1.Properties.ValueChecked = new decimal(new int[] {
//            1,
//            0,
//            0,
//            0});
//            this.ceUsed1.Properties.ValueGrayed = new decimal(new int[] {
//            999,
//            0,
//            0,
//            -2147483648});
//            this.ceUsed1.Properties.ValueUnchecked = new decimal(new int[] {
//            0,
//            0,
//            0,
//            0});
//            this.ceUsed1.Size = new System.Drawing.Size(20, 19);
//            this.ceUsed1.TabIndex = 4;
//            this.ceUsed1.EditValueChanged += new System.EventHandler(this.onCheckedChanged);
//            // 
//            // pEditors
//            // 
//
//
//            pEditors.Appearance.BackColor = System.Drawing.Color.Transparent;
//            pEditors.Appearance.Options.UseBackColor = true;
//            pEditors.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
//            pEditors.Dock = System.Windows.Forms.DockStyle.Fill;
//            //_control.Location = new System.Drawing.Point(19, 0);
//            //_control.Name = "pEditors";
//            //_control.Size = new System.Drawing.Size(201, 20);
//            //_control.TabIndex = 5;
//
//            // UIBase
//            cRoot1.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//			//GDI Fonts handles fix
//            //cRoot1.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//
//            //cRoot1.DoubleBuffered = true;
//
//            cRoot1.Margin = new System.Windows.Forms.Padding(0);
//            cRoot1.Name = "UIBase";
//            cRoot1.Size = new System.Drawing.Size(220, 20);
//
//            // порядок важен
//           
//            cRoot1.Controls.Add(pEditors);
//            cRoot1.Controls.Add(pSettings1);
//
//            SetCheckEnabled(false);
//         
//        }
//        bool _oldChecked = false;
//        private void onCheckedChanged(object sender, EventArgs e)
//        {
//            if (_oldChecked != ceUsed1.Checked)
//            {
//                if (CheckedChanged != null)
//                {
//                    CheckedChanged(ceUsed1.Checked);
//                }
//            }
//            _oldChecked = ceUsed1.Checked;
//        }
//
//
//        public void BeginInit()
//        {
//          
//            VControl.BeginInitObject(pSettings1);
//            VControl.BeginInitObject(pEditors);
//
//
//            VControl.BeginInitObject(ceUsed1.Properties);
//         
//            this.pSettings1.SuspendLayout();
//            this.pEditors.SuspendLayout();
//            this.pSettings1.SuspendLayout();
//
//         
//        }
//
//        public void EndInit()
//        {
//
//            VControl.EndInitObject(pSettings1);
//            VControl.EndInitObject(pEditors);
//
//
//            VControl.EndInitObject(ceUsed1.Properties);
//
//            this.pSettings1.ResumeLayout();
//            this.pEditors.ResumeLayout();
//            this.pSettings1.ResumeLayout();
//        }
//
//        public Control GetContentPanel() // временно
//        {
//            return pEditors;
//        }
//
//        public BaseEdit GetCheckControl() // временно
//        {
//            return ceUsed1;
//        }
//
//
//       
//
//        public void AddChild(IVControl control)
//        {
//            var tc = (Control)control;
//            pEditors.Controls.Add(tc);
//
//        }
//
//        public void SetChecked(bool value)
//        {
//            ceUsed1.Checked = value;
//        }
//
//        public void SetCheckEnabled(bool value)
//        {
//            if (this._uibase.GetType().Name == typeof( UIList).Name && value)
//            {
//            }
//            ceUsed1.Enabled = value;
//        }
//
//        public void SetCheckVisible(bool value)
//        {
//            ceUsed1.Visible = value;
//            pSettings1.Visible = value;
//        }
//
//        public event ValueChangeEventHandler CheckedChanged;
//
//        
//        ////////////
//
//
//
//
//        public void SetName(string value)
//        {
//             this.Name = value; 
//        }
//
//        private void SetEditorButtonDefaultVisibility(EditorButton btn, bool value)
//        {
//            XElement tag = btn.Tag as XElement;
//            // костыль, чтобы не приятать кнопки для нередактируемого поля если видимость установлена через переменную
//            if (tag == null || !tag.Attributes(sql.builder.DataApi.TextConst.AName.Visible).Any())
//            {
//                btn.Visible = value;
//            }
//            
//           
//            
//        }
//
//        public void SetEnabled(bool value)
//        {
//            var read_only = !value;
//            foreach (var ctrl in GetBaseEditControls())
//            {
//                // list, combobox
//                
//                if (_uibase is UIList)
//                {
//                    var edit = ctrl as PopupContainerEdit;
//                    edit.Properties.UseReadOnlyAppearance = read_only;
//                    foreach (var b in edit.Properties.Buttons.Cast<EditorButton>())
//                    {
//                        SetEditorButtonDefaultVisibility(b, !read_only);
//                    }
//                }
//                // file
//                else if (_uibase is UIFile)
//                {
//                    var edit = ctrl as ButtonEdit;
//                    edit.Properties.UseReadOnlyAppearance = read_only;
//                    // в режиме readonly можно просматривать файлы
//                    foreach (var b in edit.Properties.Buttons.Cast<EditorButton>().Where(b => b.Kind != ButtonPredefines.Search))
//                    {
//                        SetEditorButtonDefaultVisibility(b, !read_only);
//                    }
//                }
//                // Остальные
//                else
//                {
//                    ctrl.Properties.ReadOnly = read_only;
//
//                    var edit = ctrl as ButtonEdit;
//                    if (edit != null)
//                    {
//                        foreach (var b in edit.Properties.Buttons.Cast<EditorButton>())
//                        {
//                            SetEditorButtonDefaultVisibility(b, !read_only);
//                        }
//                        //edit.Properties.Buttons.Cast<EditorButton>().ForEach(b => b.Visible = true);
//                    }
//                }
//            }
//        }
//
//
//        public IEnumerable<BaseEdit> GetBaseEditControls()
//        {
//            if (_uibase is UICheck)
//            {
//                yield return GetCheckControl(); // временно
//            }
//            else
//            {
//                var queue = new Queue<Control>();
//                queue.Enqueue(GetContentPanel());  // временно
//                do
//                {
//                    var control = queue.Dequeue();
//
//                    var base_edit = control as BaseEdit;
//                    if (base_edit != null) yield return base_edit;
//
//                    // Емцов - на нём лежат скрол бары и какой-то левый TextEdit
//                    if (control is DevExpress.XtraTreeList.TreeList) continue;
//
//                    foreach (Control child in control.Controls)
//                    {
//                        queue.Enqueue(child);
//                    }
//
//                } while (queue.Count > 0);
//            }
//        }
//
//
//
//
//        public void SetError(string value)
//        {
//            var ctrl = (Cmn.GetChildControlsOfType<ButtonEdit>(this)).FirstOrDefault(); 
//            ctrl.ErrorText = value;
//            ctrl.ErrorIcon = Cmn.ImageWarning14;
//            ctrl.ErrorIconAlignment = ErrorIconAlignment.MiddleLeft;
//        }
//
//
//        public void AddButton(IVEditorButton button)
//        {
//           
//            (GetBaseEditControls().First() as ButtonEdit).Properties.Buttons.Add((EditorButton)button);
//        }
//    }
//}
