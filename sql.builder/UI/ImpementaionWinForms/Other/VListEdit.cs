//using System;
//using System.Diagnostics;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
////using DevExpress.XtraEditors;
////using System.Windows.Forms;
//using System.Data;
//using System.Drawing;
////using DevExpress.XtraTreeList.Columns;
////using DevExpress.XtraTreeList;
//using sql.builder.UI;
////using DevExpress.XtraEditors.Repository;
////using DevExpress.XtraTreeList.Nodes;
//using System.Xml.Linq;
////using DevExpress.Utils;
////using DevExpress.XtraWaitForm;
//
//namespace sql.builder.UI.WinForms
//{
//    internal class VListEdit : PopupContainerEdit, IVListEdit
//    {
//        #region поля
//        public /*временно*/ DevExpress.XtraEditors.PopupContainerEdit popupContainerEdit;
//        private /*временно*/ DevExpress.XtraEditors.PopupContainerControl popupContainerControl;
//        private int load_data_counter;
//        private /*временно*/ sql.builder.UI.IucGrid tree;
//        #endregion
//        public void DoPlacement()
//        {
//            (_controller as UIList).AddPart(popupContainerControl);
//        }
//        public void BeginLoadData()
//        {
//            int counter = Interlocked.Increment(ref this.load_data_counter);
//            if (counter == 1) {
//                this.loadingControl.Show();
//            }
//            //Debug.Write("VListEdit::BeginLoadData(): counter = " + counter.ToString());
//            //Debug.WriteLine(" from thread id = " + Thread.CurrentThread.ManagedThreadId.ToString("X8"));
//        }
//        public void EndLoadData()
//        {
//            int counter = Interlocked.Decrement(ref this.load_data_counter);
//            if (counter <= 0) {
//                this.loadingControl.Hide();
//                if (counter < 0) {           // Workaround
//                    Volatile.Write(ref this.load_data_counter, 0);
//                }
//            }
//            //Debug.Write("VListEdit::EndLoadData(): counter = " + counter.ToString());
//            //Debug.WriteLine(" from thread id = " + Thread.CurrentThread.ManagedThreadId.ToString("X8"));
//        }
//        public void ShowConditions(bool val)
//        {
//            #if DX15 
//            #else
//            treeWFOk.OptionsFilter.AllowAutoFilterConditionChange = (val) ? DefaultBoolean.True : DefaultBoolean.False;
//            #endif
//        }
//        public sql.builder.UI.IucGrid GetList()
//        {
//            return tree;
//        }       
//        private sql.builder.Controls.Grids.ReportViewModes.ucTreeWF treeWFOk
//        {
//            get
//            {
//                return (sql.builder.Controls.Grids.ReportViewModes.ucTreeWF)tree;
//            }
//        }
//        public sql.builder.Controls.Grids.ReportViewModes.ucTreeWF treeWFTmp
//        {
//            get
//            {
//                return (sql.builder.Controls.Grids.ReportViewModes.ucTreeWF)tree;
//            }
//        }
//        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rceParamVisible;
//        private DevExpress.XtraEditors.SimpleButton btnOk;
//        private DevExpress.XtraEditors.SimpleButton btnLoadAll;
//        private System.Windows.Forms.TableLayoutPanel table;
//        private System.Windows.Forms.Timer timer;
//        private DevExpress.XtraEditors.LabelControl lRowsLimit;
//        private IList _controller;
//        private ProgressPanel loadingControl;
//
//        public void SetController(IList controller)
//        {
//            _controller = controller;
//        }
//        public VListEdit(IucGrid list)
//        {
//
//            tree = list;
//            createControls();
//            BeginInit();
//            setControlsProperties();
//
//        }
//
//
//        public void SetListRowsLimit( int value)
//        {
//            lRowsLimit.Visible = (value>0);
//            btnLoadAll.Visible = (value>0);
//         
//            lRowsLimit.Text =" Первые "+ value.ToString();
//        }
//
//
//        public void SetPopupHeight(int val)
//        {
//
//            if (val > 0)
//            {
//                popupContainerControl.Height = val;
//            }
//
//
//            if (popupContainerControl.Height < 60)// Может быть это и не нужно
//            {
//                popupContainerControl.Height = 60;
//            }
//        }
//
//        public void SetPopupWidth(int val)
//        {
//            popupContainerControl.Width = val;
//        }
//
//
//        public int GetPopupHeight()
//        {
//            var popup = ((DevExpress.Utils.Win.IPopupControl)popupContainerEdit).PopupWindow;
//            if (popup != null)
//            {
//
//                return popup.Height;
//            }
//            return 0;
//        }
//
//        public int GetPopupWidth()
//        {
//            var popup = ((DevExpress.Utils.Win.IPopupControl)popupContainerEdit).PopupWindow;
//            if (popup != null)
//            {
//                return popup.Width;
//            }
//            return 0;
//        }
//
//        
//
//       
//       
//
//        
//
//        
//
//
//        //public void SetKeyFieldName(string columnName)
//        //{
//        //    treeWF.KeyFieldName = columnName;
//        //}
//
////        public void AddColumn(string columnName, string columnTitle, XElement xviewcolumn)
////        {
////            var tcol = treeWF.Columns.Add();
////            tcol.FieldName = columnName;
////            tcol.Caption = columnTitle;
//
////            if (columnName == "check")
////            {
////                tcol.Fixed = FixedStyle.Left;
////            }
////            if (xviewcolumn != null)
////            {
////                sql.builder.XmlHelpers.Parser.FillTreeColumnFromXml(tcol, null, xviewcolumn);
////            }
////            tcol.OptionsColumn.AllowEdit = false;
//
////#if DX15
////#else
////            tcol.OptionsFilter.AutoFilterCondition = AutoFilterCondition.BeginsWith;
////#endif  
//            
//
////        }
//
//        //public void SetParentFieldName(string columnName)
//        //{
//        //    if (columnName != null)
//        //    {
//        //        treeWF.OptionsView.ShowRoot = true;
//        //        treeWF.ParentFieldName = columnName;
//        //        treeWF.OptionsFilter.FilterMode = FilterMode.Extended;
//        //    }
//        //}
//
//        private void BeginUpdate()
//        {
//            treeWFOk.PostEditor();// можно ли всегда вызывать???
//            treeWFOk.BeginUpdate();
//            treeWFOk.LockReloadNodes();
//        }
//
//        private void beginUpdateNoLock()
//        {
//            treeWFOk.PostEditor();// можно ли всегда вызывать???
//            treeWFOk.BeginUpdate();
//           
//        }
//
//        private void EndUpdate()
//        {
//            treeWFOk.UnlockReloadNodes();
//            treeWFOk.EndUpdate();
//            treeWFOk.EndCurrentEdit();
//        }
//
//        private void endUpdateNoLock()
//        {
//
//            treeWFOk.EndUpdate();
//            treeWFOk.EndCurrentEdit();
//        }
//        //public void SetData(DataTable data)
//        //{
//        //    if (treeWF.DataSource != data)
//        //    {
//        //        treeWF.DataSource = data;
//        //        InitColumnsEditors();
//        //    }
//        //}
//        //bool columnsInited = false;
//        public void SetMultiselect()
//        {
//            tree.SetMultiSelect(true);
//            //treeWFOk.OptionsSelection.MultiSelect = true;
//            TreeListColumn column = treeWFOk.Columns.ColumnByFieldName("check");
//            column.Caption = string.Empty;
//            column.AllNodesSummary = true;
//            column.Width = 50;
//            column.VisibleIndex = 0;
//            column.OptionsColumn.FixedWidth = true;
//            column.OptionsColumn.AllowEdit = true;
//            var rep = new RepositoryItemCheckEdit();
//            rep.ValueChecked = Cmn.INT32_ONE;
//            rep.ValueUnchecked = Cmn.INT32_ZERO;
//            rep.ValueGrayed = Cmn.INT32_MINUS_ONE;
//            rep.EditValueChanged += ColumnEdit_EditValueChanged;
//            column.ColumnEdit = rep;
//        }
//        //public void InitColumnsEditors()
//        //{
//        //    if (columnsInited) return;
//        //    columnsInited = true;
//
//        //    foreach (TreeListColumn column in treeWF.Columns)
//        //    {
//        //        if (column.ColumnType == typeof(DateTime))
//        //        {
//        //            var rep = new RepositoryItemDateEdit();
//        //            rep.Buttons[0].Visible = false;
//        //            column.ColumnEdit = rep;
//        //        }
//        //        else if (column.ColumnType == typeof(String))
//        //        {
//        //            var rep = new RepositoryItemTextEdit();
//        //            column.ColumnEdit = rep;
//        //        }
//        //    }
//        //}
//
//        public void EndInitList()
//        {
//            //if (tree.VisibleColumns.Count > 2 || (tree.VisibleColumns.Count > 1 && !Controller.IsMultiselect()))
//            //{
//            if (treeWFOk.VisibleColumns.Count > 1)// вроде как колонка выбора позднее появляется
//            {
//                treeWFOk.OptionsView.ShowColumns = true;
//                treeWFOk.OptionsView.AutoWidth = false;
//            }
//        }
//
//        void ColumnEdit_EditValueChanged(object sender, EventArgs e)
//        {
//            if (treeWFOk.FocusedNode == null || treeWFOk.FocusedNode is TreeListAutoFilterNode) return;
//            _controller.SuppressChangeEvent();
//            // _need_get_data = false;
//            _controller.SetNeedGetData(false);
//
//            var fnode = treeWFOk.FocusedNode;
//
//            TreeListNode[] selected_nodes = null;
//
//            if (_controller.IsAutoCheck())
//            {
//                selected_nodes = treeWFOk.Selection.Cast<TreeListNode>().ToArray();
//                var visible_nodes = selected_nodes.Where(node => node.ParentNode == null || !selected_nodes.Contains(node.ParentNode)).Reverse();
//                selected_nodes = visible_nodes.ToArray();
//            }
//            else
//            {
//                selected_nodes = treeWFOk.Selection.Cast<TreeListNode>().Reverse().ToArray();
//            }
//            //(tree.DataSource as sql.builder.DataApi.VDataTable).SuppressChangeEvent();
//            if (selected_nodes.Length > 20) // если делать Lock, то глюки с фокусом если не делать - тормоза
//            {
//                BeginUpdate();
//            }
//            else
//            {
//                beginUpdateNoLock(); 
//            }
//            foreach (TreeListNode selected_node in selected_nodes)
//            {
//                
//                CheckWithChilds(selected_node, (int)treeWFOk.FocusedNode["check"]);
//                if (_controller.IsAutoCheck()) UpdateNodesStates(GetRootNode(selected_node), selected_node);
//            }
//            //(tree.DataSource as sql.builder.DataApi.VDataTable).ResumeChangeEvent();
//            if (selected_nodes.Length > 20) {
//                EndUpdate();
//            } else {
//                endUpdateNoLock();
//            }
//
//            _controller.ResumeChangeEvent();
//            _controller.RaiseChanged();
//            _controller.UpdateSelectedString();
//
//
//            _controller.SetNeedGetData(true);
//
//            // _need_get_data = true;
//
//            //Controller.ListItemSelected();
//        }
//        private void UpdateNodesStates(TreeListNode parent_node, TreeListNode border_node = null)
//        {
//            // обходим дерево снизу вверх
//            if (border_node != null && !parent_node.Nodes.Contains(border_node)) {
//                foreach (TreeListNode child_node in parent_node.Nodes) {
//                    UpdateNodesStates(child_node, border_node);
//                }
//            }
//            if (parent_node.Nodes.Count == 0) {
//                return;
//            }
//            if (parent_node.Nodes.All(node => Cmn.INT32_ONE.Equals(node["check"]))) {
//                if (!Cmn.INT32_ONE.Equals(parent_node["check"])) {
//                    parent_node["check"] = Cmn.INT32_ONE;
//                }
//            } else if (parent_node.Nodes.All(node => Cmn.INT32_ZERO.Equals(node["check"]))) {
//                if (!Cmn.INT32_ZERO.Equals(parent_node["check"])) {
//                    parent_node["check"] = Cmn.INT32_ZERO;
//                }
//            } else {
//                if (!Cmn.INT32_MINUS_ONE.Equals(parent_node["check"])) {
//                    parent_node["check"] = Cmn.INT32_MINUS_ONE;
//                }
//            }
//        }
//        private void CheckWithChilds(TreeListNode node, int state)
//        {
//            if (!state.Equals(node["check"])) {
//                node["check"] = state;
//            }
//            // Режим, когда чек родителя означает, что должны быть чекнуты все потомки
//            if (_controller.IsAutoCheck()) {
//                foreach (TreeListNode child_node in node.Nodes) {
//                    CheckWithChilds(child_node, state);
//                }
//            }
//        }
//        private TreeListNode GetRootNode(TreeListNode node)
//        {
//            TreeListNode root_node = node;
//            while (root_node.ParentNode != null) {
//                root_node = root_node.ParentNode;
//            }
//            return root_node;
//        }
//        public /*временно*/ void UpdateTreeView()
//        {
//            //if (Form.NoData) return; // Вроде не используем, если нужно поставить проверку в другом месте
//
//            treeWFOk.ForceInitialize();
//
//            // состояние потомков влияет на состояние родителей
//            if (_controller.IsAutoCheck())
//            {
//                foreach (TreeListNode node in treeWFOk.Nodes)
//                {
//                    UpdateNodesStates(node);
//                }
//            }
//
//            if (_controller.IsMultiselect()) UpdateCheckColumnWidth();
//            _controller.UpdateSelectedString();
//
//            // if (Form.FormUseType == UIFormC.UseType.SchemeEditor) tree.ClearColumnsFilter(); // поставить проверку в другом месте
//        }
//
//        private void UpdateCheckColumnWidth()
//        {
//            if (treeWFOk.Nodes.Count == 0) return;
//
//            int step = 20;
//            int min_width = 50;
//
//            int width = 20 + step * (GetAllNodes().Max(n => n.Level) + 1);
//            treeWFOk.Columns["check"].Width = width > min_width ? width : min_width;
//        }
//
//        private IEnumerable<TreeListNode> GetAllNodes()
//        {
//            return treeWFTmp.Nodes.SelectMany(Cmn.GetNodeBranch);
//        }
//        public void ShowFooterPanel()
//        {
//            table.RowStyles[1].Height = 25;
//        }
//
//        public void HideFooterPanel()
//        {
//            table.RowStyles[1].Height = 0;
//        }
//
//
//        //public void SetUseCurrent()
//        //{
//        //    currentEdit = this;
//        //}
//        public void Close_Popup()
//        {
//
//            if (popupContainerControl.OwnerEdit != null) popupContainerControl.OwnerEdit.ClosePopup();
//            else popupContainerEdit.ClosePopup();
//
//
//        }
//        //public void Show_Popup(object sender)
//        //{
//
//        //    var edit = (sender as PopupContainerEdit);
//        //    edit.Properties.PopupControl = popupContainerControl;
//        //    popupControlAssigned = true;
//        //    popupContainerEdit.ShowPopup();
//        //}
//
//        //public void ClearList()
//        //{
//        //    treeWF.Columns.Clear();
//        //    treeWF.DataSource = null;
//        //}
//
//        public void ClearListFocus()
//        {
//
//            treeWFOk.FocusedNode = treeWFOk.Nodes.AutoFilterNode;
//        }
//
//
//        public void SetFocusToFilter()
//        {
//
//            treeWFOk.FocusedNode = treeWFOk.Nodes.AutoFilterNode;
//            treeWFOk.ShowEditor();
//        }
//
//
//
//
//        public void SetText(object value)
//        {
//            popupContainerEdit.EditValue = value;
//        }
//
//        private object _value = DBNull.Value;
//        public void SetValue(object value)
//        {
//            // вызывается, много раз, не страшно ,если ничего не навешивать
//            _value = value;
//        }
//        public string GetText()
//        {
//            return (string)Cmn.Nvl(popupContainerEdit.EditValue, "");
//        }
//
//        private string getText() // не знаю в чем разница
//        {
//            return popupContainerEdit.Text;
//        }
//
//
//
//        public void BeginInit()
//        {
//
//            VControl.BeginInitObject(this.Properties);
//            VControl.BeginInitObject(this.popupContainerControl);
//            this.popupContainerControl.SuspendLayout();
//            this.table.SuspendLayout();
//            VControl.BeginInitObject(this.treeWFTmp);
//            VControl.BeginInitObject(this.rceParamVisible);
//        }
//
//        public void EndInit()
//        {
//            VControl.EndInitObject(this.Properties);
//            VControl.EndInitObject(this.popupContainerControl);
//            this.popupContainerControl.ResumeLayout(false);
//            this.table.ResumeLayout(false);
//            this.table.PerformLayout();
//            VControl.EndInitObject(this.treeWFTmp);
//            VControl.EndInitObject(this.rceParamVisible);
//        }
//
//        public void tmp_RestartTimer()
//        {
//            if (timer.Enabled) timer.Stop();
//            timer.Start();
//        }
//
//        private void createControls()
//        {
//
//
//            this.popupContainerEdit = this;
//
//            this.popupContainerControl = new DevExpress.XtraEditors.PopupContainerControl();
//
//            this.table = new System.Windows.Forms.TableLayoutPanel();
//            //this.tree = new DevExpress.XtraTreeList.TreeList();
//           // this.tree = (sql.builder.Controls.Grids.ReportViewModes.ucTreeWF)UIStatic.GetControlsfactory().CreateTree();
//            this.rceParamVisible = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
//            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
//            this.lRowsLimit = new DevExpress.XtraEditors.LabelControl();
//            this.btnLoadAll = new DevExpress.XtraEditors.SimpleButton();
//            this.timer = new System.Windows.Forms.Timer();
//            loadingControl = new ProgressPanel();
//
//        }
//
//        private void setControlsProperties()
//        {
//            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 =
//              new DevExpress.Utils.SerializableAppearanceObject();
//            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 =
//                new DevExpress.Utils.SerializableAppearanceObject();
//            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 =
//                new DevExpress.Utils.SerializableAppearanceObject();
//            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 =
//                new DevExpress.Utils.SerializableAppearanceObject();
//            // 
//            // pEditors
//            // 
//            //AddPart(popupContainerControl);
//            //AddPart(popupContainerEdit);
//            // 
//            // popupContainerEdit
//            // 
//
//
//            //uibase.AddPart(popupContainerEdit);
//            //uibase.AddPart(popupContainerControl);
//
//            this.popupContainerEdit.CausesValidation = false;
//            this.popupContainerEdit.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.popupContainerEdit.Location = new System.Drawing.Point(0, 0);
//            this.popupContainerEdit.Name = "popupContainerEdit";
//            this.popupContainerEdit.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.True;
//            this.popupContainerEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
//                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, true, true, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, serializableAppearanceObject2, serializableAppearanceObject3, serializableAppearanceObject4, "", null, null, true)});
//            this.popupContainerEdit.Properties.CloseUpKey = new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None);
//            this.popupContainerEdit.Properties.PopupControl = this.popupContainerControl;
//            this.popupContainerEdit.Properties.ReadOnly = true;
//            this.popupContainerEdit.Properties.ShowPopupCloseButton = false;
//            this.popupContainerEdit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
//            this.popupContainerEdit.Properties.UseReadOnlyAppearance = false;
//            this.popupContainerEdit.Size = new System.Drawing.Size(391, 20);
//            this.popupContainerEdit.TabIndex = 2;
//            this.popupContainerEdit.ErrorIconAlignment = ErrorIconAlignment.MiddleRight;
//            //События
//           // this.popupContainerEdit.Popup += popupContainerEdit_Popup;
//            this.popupContainerEdit.QueryPopUp += popupContainerEdit_QueryPopUp;
//            this.popupContainerEdit.Closed += popupContainerEdit_Closed;
//            //this.popupContainerEdit.ButtonClick += popupContainerEdit_ButtonClick;
//            this.popupContainerEdit.Enter += popupContainerEdit_Enter;
//            this.popupContainerEdit.KeyDown += popupContainerEdit_KeyDown;
//            this.popupContainerEdit.KeyPress += popupContainerEdit_KeyPress;
//            // 
//            // popupContainerControl
//            // 
//            this.popupContainerControl.Controls.Add(this.table);
//            this.popupContainerControl.Location = new System.Drawing.Point(6, 26);
//            this.popupContainerControl.Name = "popupContainerControl";
//            this.popupContainerControl.Size = new System.Drawing.Size(375, 275);
//            this.popupContainerControl.TabIndex = 3;
//
//            // 
//            // table
//            // 
//            this.table.ColumnCount = 3;
//            this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
//            this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
//            this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
//            this.table.Controls.Add(this.treeWFTmp, 0, 0);
//            this.table.Controls.Add(this.btnLoadAll, 1, 1);
//            this.table.Controls.Add(this.btnOk, 2, 1);
//            
//           
//            this.table.Controls.Add(this.lRowsLimit, 0, 1);
//            this.table.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.table.Location = new System.Drawing.Point(0, 0);
//            this.table.Name = "table";
//            this.table.RowCount = 2;
//            this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
//            this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
//            this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
//            this.table.Size = new System.Drawing.Size(375, 275);
//            this.table.TabIndex = 4;
//            this.table.SetColumnSpan(this.treeWFTmp, 3);
//            // 
//            // tree
//            // 
//
//
//            this.treeWFTmp.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
//                this.rceParamVisible});
//            this.treeWFTmp.PrepareForList();
//
//            this.treeWFTmp.GetNodeDisplayValue += tree_GetNodeDisplayValue;
//            this.treeWFTmp.BeforeFocusNode += tree_BeforeFocusNode;
//            this.treeWFTmp.CustomDrawNodeCell += tree_CustomDrawNodeCell;
//            this.treeWFTmp.FilterNode += tree_FilterNode;
//            this.treeWFTmp.ColumnFilterChanged += tree_ColumnFilterChanged;
//            this.treeWFTmp.MouseDown += tree_MouseDown;
//            this.treeWFTmp.MouseMove += tree_MouseMove;
//
//
//
//            //this.tree.DoubleClick += tree_DoubleClick;
//            this.treeWFTmp.KeyDown += tree_KeyDown;
//            // 
//            // rceParamVisible
//            // 
//            this.rceParamVisible.AutoHeight = false;
//            this.rceParamVisible.Caption = "Check";
//            this.rceParamVisible.Name = "rceParamVisible";
//            // 
//            // btnOk
//            // 
//            this.btnOk.Dock = System.Windows.Forms.DockStyle.Right;
//            this.btnOk.Location = new System.Drawing.Point(338, 253);
//            this.btnOk.Name = "btnOk";
//            this.btnOk.Size = new System.Drawing.Size(34, 19);
//            this.btnOk.TabIndex = 3;
//            this.btnOk.Text = "Ок";
//            this.btnOk.Click += btnOk_Click;
//
//            // 
//            // btnLoadAll
//            // 
//            //this.btnLoadAll.Dock = System.Windows.Forms.DockStyle.Left;
//           // this.btnLoadAll.Location = new System.Drawing.Point(67, 253);
//            this.btnLoadAll.Name = "btnLoadAll";
//            //this.btnLoadAll.AutoSize = true;
//            this.btnLoadAll.Size = new System.Drawing.Size(100, 19);
//           // this.btnLoadAll.TabIndex = 3;
//            this.btnLoadAll.TabIndex = 4;
//            this.btnLoadAll.Visible = false;
//            this.btnLoadAll.Text = "Загрузить все";
//            this.btnLoadAll.Click += btnLoadAll_Click;
//            //this.btnLoadAll.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
//           // this.btnLoadAll.Click += btnOk_Click;
//            // 
//            // lRowsLimit
//            // 
//           // this.lRowsLimit.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
//            this.lRowsLimit.Location = new System.Drawing.Point(3, 253);
//            this.lRowsLimit.Margin = new Padding(0,6,0,0);
//            this.lRowsLimit.Name = "lRowsLimit";
//            this.lRowsLimit.Size = new System.Drawing.Size(65, 19);
//            this.lRowsLimit.TabIndex = 4;
//            //this.lRowsLimit.Text = " Максимум 0";
//            this.lRowsLimit.Visible = false;
//            // 
//            // timer
//            // 
//            this.timer.Interval = 200;
//            this.timer.Tick += timer_Tick;
//
//            loadingControl.ShowCaption = false;
//            loadingControl.Description = "Загрузка данных...";
//            loadingControl.Dock = DockStyle.Bottom;
//            loadingControl.Visible = false;
//            loadingControl.ContentAlignment = ContentAlignment.MiddleCenter;
//            treeWFTmp.Resize += (sender, args) => loadingControl.Height = treeWFTmp.Height - 40;
//
//            treeWFTmp.Controls.Add(loadingControl);
//            // 
//            // UIList
//        }
//
//        void popupContainerEdit_QueryPopUp(object sender, System.ComponentModel.CancelEventArgs e)
//        {
//            PreparePopup(null);
//            if (_controller.HasCustomButtons() && !_isCustomPopupCall)
//            {
//                e.Cancel = true;
//            }
//            else
//            {
//
//             
//                PopUpProcessing();
//            }
//            _isCustomPopupCall = false;
//        }
//
//        void btnLoadAll_Click(object sender, EventArgs e)
//        {
//            _controller.LoadAllListRows();
//        }
//
//
//        private TreeListNode HotTrackNode = null;
//        void tree_MouseMove(object sender, MouseEventArgs e)
//        {
//            if (_controller.IsMultiselect()) return;
//            var info = treeWFOk.CalcHitInfo(treeWFOk.PointToClient(Control.MousePosition));
//
//            if (info.HitInfoType == HitInfoType.Cell)
//            {
//
//
//                if (HotTrackNode != info.Node)
//                {
//                    if (HotTrackNode != null)
//                    {
//                        if (HotTrackNode != treeWFOk.FocusedNode)
//                        {
//                            treeWFOk.UnselectNode(HotTrackNode);
//                        }
//                    }
//                    HotTrackNode = info.Node;
//                }
//
//                //tree.SelectNode(HotTrackNode);
//                if (HotTrackNode != null)
//                {
//                    treeWFOk.SelectNode(HotTrackNode);
//                }
//                //if (info.Node != null && info.Node != tree.Nodes.AutoFilterNode)
//                //{
//                //    Controller.SingeValueSelect(info.Node[tree.KeyFieldName]);
//                //}
//
//            }
//            else
//            {
//                if (HotTrackNode != null)
//                {
//                    if (HotTrackNode != treeWFOk.FocusedNode)
//                    {
//                        treeWFOk.UnselectNode(HotTrackNode);
//                    }
//                }
//            }
//        }
//
//
//
//
//        //public event EventHandler Tree_ColumnFilterChanged;
//        void tree_ColumnFilterChanged(object sender, EventArgs e)
//        {
//            _controller.Tree_ColumnFilterChanged();
//        }
//
//        //public event DevExpress.XtraTreeList.FilterNodeEventHandler Tree_FilterNode;
//        void tree_FilterNode(object sender, DevExpress.XtraTreeList.FilterNodeEventArgs e)
//        {
//            if (_controller.IsServerFilter())
//            {
//                e.Handled = true;
//            }
//        }
//
//        //public event SizeChangeEventHandler PopupResize;
//        //void popupContainerControl_Resize(object sender, EventArgs e)
//        //{
//        //    PopupResize(popupContainerControl.Width, popupContainerControl.Height);
//        //   
//        //}
//
//        
//        void timer_Tick(object sender, EventArgs e)
//        {
//            timer.Stop();
//            
//          _controller.listEdit_AutoFilterChanged();
//        }
//
//
//        void btnOk_Click(object sender, EventArgs e)
//        {
//            Close_Popup();
//        }
//
//
//
//        //public event KeyEventHandler Tree_KeyDown;
//        void tree_KeyDown(object sender, KeyEventArgs e)
//        {
//            if (e.KeyCode == Keys.Enter)
//            {
//                if (treeWFOk.FocusedNode != null)
//                {
//                    _controller.SingeValueSelect(treeWFOk.FocusedNode[_controller.GetKeyFieldName()]);
//                }
//            }
//        }
//
//
//
//
//        //public event EventHandler Tree_Click;
//        //void tree_Click(object sender, EventArgs e)
//        //{
//
//        //    var info = tree.CalcHitInfo(tree.PointToClient(Control.MousePosition));
//
//        //    if (info.HitInfoType == HitInfoType.Cell)
//        //    {
//        //        if (tree.FocusedNode != null)
//        //        {
//        //            Controller.SingeValueSelect(tree.FocusedNode[tree.KeyFieldName]);
//        //        }
//
//        //    }
//        //}
//
//
//
//        void tree_MouseDown(object sender, MouseEventArgs e)
//        {
//
//            var info = treeWFOk.CalcHitInfo(treeWFOk.PointToClient(Control.MousePosition));
//
//            if (info.HitInfoType == HitInfoType.Cell)
//            {
//
//                if (info.Node != null && info.Node != treeWFOk.Nodes.AutoFilterNode)
//                {
//                    _controller.SingeValueSelect(info.Node[_controller.GetKeyFieldName()]);
//                }
//
//            }
//        }
//
//        //public event DevExpress.XtraTreeList.CellValueChangedEventHandler Tree_CellValueChanging;
//        //void tree_CellValueChanging(object sender, DevExpress.XtraTreeList.CellValueChangedEventArgs e)
//        //{
//        //    Tree_CellValueChanging(sender, e);
//        //}
//
//        //public event DevExpress.XtraTreeList.CustomDrawNodeCellEventHandler Tree_CustomDrawNodeCell;
//        void tree_CustomDrawNodeCell(object sender, DevExpress.XtraTreeList.CustomDrawNodeCellEventArgs e)
//        {
//
//            if (e.Node is TreeListAutoFilterNode) return;
//            var rowId = e.Node[treeWFTmp.KeyFieldName];
//            var color = _controller.GetListCellColor(rowId, e.Column.FieldName);
//            if (color != System.Drawing.Color.Empty)
//            {
//                e.Appearance.ForeColor = color;
//            }
//            //Tree_CustomDrawNodeCell(sender, e);
//            
//        }
//
//
//        void tree_BeforeFocusNode(object sender, DevExpress.XtraTreeList.BeforeFocusNodeEventArgs e)
//        {
//
//            if (_controller.IsMultiselect())
//            {
//                if (Control.ModifierKeys == Keys.Control || Control.ModifierKeys == Keys.Shift) return;
//
//                var hitInfo = treeWFOk.CalcHitInfo(treeWFOk.PointToClient(Control.MousePosition));
//                if (hitInfo.Column != null && hitInfo.Column.FieldName == "check") return;
//
//                treeWFOk.Selection.Clear();
//            }
//        }
//
//        //public event DevExpress.XtraTreeList.GetNodeDisplayValueEventHandler Tree_GetNodeDisplayValue;
//        void tree_GetNodeDisplayValue(object sender, DevExpress.XtraTreeList.GetNodeDisplayValueEventArgs e)
//        {
//            if (e.Column.FieldName == _controller.GetNameFieldName())
//            {
//                if (e.Node != null)
//                {
//                    e.Value = new string('\t', e.Node.Level) + e.Value;
//                }
//            }
//        }
//
//        //public event KeyPressEventHandler PopupContainerEdit_KeyPress;
//        //void popupContainerEdit_KeyPress(object sender, KeyPressEventArgs e)
//        //{
//        //    PopupContainerEdit_KeyPress(sender, e);
//        //}
//
//        public void ClearFilter()
//        {
//            treeWFOk.ClearColumnsFilter();
//        }
//
//        public /*временно*/ void popupContainerEdit_KeyPress(object sender, KeyPressEventArgs e)
//        {
//
//            _controller.PrepareListSource();
//            bool ctrl_pressed = ((Control.ModifierKeys & Keys.Control) == Keys.Control);
//            bool ctrl_v_pressed = (e.KeyChar == 22);
//            if (ctrl_pressed && !ctrl_v_pressed) return;
//
//            var pcEdit = (sender as PopupContainerEdit);
//
//
//            // режим readonly
//
//
//            if (!_controller.IsEditable()) return;
//
//            //if (_need_refresh && !_custom_buttons)// Временное решение - если _custom_buttons то список не обновляется. Пока нет , но может быть _custom_buttons и список- обработать такую ситуацию когда понадобится
//            //{
//            //    tree.BeginUpdate();
//            //    ReloadDataLocal();
//            //    if (SourceType == ReturnType.Array) ReloadArrayEditValue();
//            //    tree.EndUpdate();
//            //    UpdateTreeView();
//            //}
//
//            //// важно взять колонку после refresh, иначе её AbsoluteIndex = -1 И ShowEditor не работает
//
//            var first_column = treeWFOk.VisibleColumns.FirstOrDefault(col => col.FieldName == _controller.GetSearchFieldName());
//            if (first_column == null) return;
//
//            string text;
//            int pos = -1;
//            // отдельно обрабатываем ситуацию с нажатым ctrl+v
//            if (ctrl_v_pressed)
//            {
//                text = Clipboard.GetText();
//
//                if (_controller.TrySelectValueByName(text))
//                {
//                    return;
//                }
//            }
//            else
//            {
//                if (!_controller.IsMultiselect())
//                {
//                    text = getText();// popupContainerEdit.Text;
//                    if ((Keys)e.KeyChar == Keys.Back)
//                    {
//                        if (!string.IsNullOrEmpty(text))
//                        {
//                            pos = (pcEdit.SelectionStart > 0) ? pcEdit.SelectionStart - 1 : 0;
//                            text = text.Substring(0, (pcEdit.SelectionStart > 0) ? pcEdit.SelectionStart - 1 : 0) +
//                                   text.Substring(pcEdit.SelectionStart + pcEdit.SelectionLength, text.Length - (pcEdit.SelectionStart + pcEdit.SelectionLength));
//                        }
//                    }
//                    else if (!Char.IsControl(e.KeyChar))
//                    {
//                        if (!string.IsNullOrEmpty(text))
//                        {
//                            pos = pcEdit.SelectionStart + 1;
//                            text = text.Substring(0, pcEdit.SelectionStart) +
//                                   e.KeyChar.ToString() +
//                                   text.Substring(pcEdit.SelectionStart + pcEdit.SelectionLength, text.Length - (pcEdit.SelectionStart + pcEdit.SelectionLength));
//                        }
//                        else
//                        {
//                            text = e.KeyChar.ToString();
//                        }
//                    }
//                    else
//                    {
//                        // управляющий символ
//                        return;
//                    }
//                }
//                else
//                {
//                    if (!Char.IsControl(e.KeyChar))
//                    {
//                        text = e.KeyChar.ToString();
//                    }
//                    else
//                    {
//                        // управляющий символ
//                        return;
//                    }
//                }
//
//            }
//
//
//            PreparePopup(pcEdit);
//            pcEdit.ShowPopup();
//            //Show_Popup(pcEdit);
//
//            treeWFOk.FocusedNode = treeWFOk.Nodes.AutoFilterNode;
//            treeWFOk.FocusedColumn = first_column;
//
//            treeWFOk.ActiveFilterString = string.Format("Contains([{0}], '{1}')", first_column.FieldName, text);
//            // с переходом на 17.2 перестало работать
//            //tree.FocusedNode[first_column] = text;
//
//            treeWFOk.ShowEditor();
//            var te = (treeWFOk.ActiveEditor as TextEdit);
//            if (te == null) return;
//
//            te.EditValue = text;
//            if (pos == -1) te.SelectionStart = text.Length;
//            else te.SelectionStart = pos;
//            te.SelectionLength = 0;
//            _controller.ProcessFilter();
//            //if (RowsLimit > 0) StartFiltering(null, null);
//        }
//
//
//        //public event KeyEventHandler PopupContainerEdit_KeyDown;
//        void popupContainerEdit_KeyDown(object sender, KeyEventArgs e)
//        {
//            if ( (e.KeyCode == Keys.Delete))
//            {
//                _controller.PopupContainerEdit_DeleteValue();
//            }
//          
//        }
//
//        //public event EventHandler PopupContainerEdit_Enter;
//        void popupContainerEdit_Enter(object sender, EventArgs e)
//        {
//            _controller.PopupContainerEdit_Enter();
//        }
//
//     
//        //void popupContainerEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
//        //{
//        //    if (e.Button.Caption == "" && e.Button.Kind ==DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
//        //    {
//        //       _controller.PopupContainerEdit_ComboButtonClick();
//        //    }
//        //}
//
//        //public event DevExpress.XtraEditors.Controls.ClosedEventHandler PopupContainerEdit_Closed;
//        public void popupContainerEdit_Closed(object sender, DevExpress.XtraEditors.Controls.ClosedEventArgs e)
//        {
//            _controller.PopupContainerEdit_Closed();
//        }
//
//        public bool canShowPopup = false;
//
//        public void PopUpProcessing()
//        {
//          
//            if (popupQueried) return;
//            popupQueried = true;
//
//            try
//            {
//                //if (!canShowPopup)
//                //{
//                //    currentEdit = (sender as PopupContainerEdit);
//                //    e.Cancel = true;
//                //}
//                //else
//                //{
//                // currentEdit = (sender as PopupContainerEdit);
//
//                _controller.PopupProcessing();
//                //if (_controller.IsNeedRefresh())
//                //{
//                //    _needPrepareList = true;
//                //    _controller.RefreshList();
//                //    //if (_needPrepareList)
//                //    //{
//                //    //    PrepareList();
//                //    //}
//                //    //  e.Cancel = true;
//                //}
//                //else
//                //{
//                //    PrepareList();
//                //    _controller.ApplyArrayValueToControlDataEditorList();
//                //    // это какая то химия для редактирования данных моногие ко многим через UIList.  Применялось только а тестовых формах, скорее всего уже сломано
//                //    currentEdit = null;
//                //}
//                //}
//            }
//            finally
//            {
//                popupQueried = false;
//            }
//        }
//        public void PreparePopup(PopupContainerEdit edit)
//        {
//            currentEdit = edit;
//            if (currentEdit == null) {
//                currentEdit = popupContainerEdit;
//            }           
//            currentEdit.Properties.PopupControl = popupContainerControl;
//           // canShowPopup = false;
//        }
//        private bool _isCustomPopupCall = false;
//        public void PopupShow()
//        {
//            //canShowPopup = true;
//            //PreparePopup(null);
//            _isCustomPopupCall = true;
//            currentEdit.ShowPopup();
//            //canShowPopup = false;
//        }
//        //public void Show_Popup(PopupContainerEdit edit)
//        //{
//        //    currentEdit = edit;
//        //    PrepareAndShowPopup();
//        //}
//        public PopupContainerEdit currentEdit = null;
//        bool popupQueried = false;
//        //bool _needPrepareList = false;
//        public void PrepareList()
//        {
//            this.GetList().DxUnlockReloadNodes();
//            //GetList().EndControlUpdate();
//            //GetList().DxEndCurrentEdit();
//            this._controller.SetFocusIfNeed();
//            // чтобы в редакторе схемы не растягивать первую колонку - утомляет
//            UIList uilist = this._controller as UIList;
//            if (uilist != null && uilist.UseType == UIFormC.UseType.SchemeEditor && treeWFOk.VisibleColumns.Count > 1) {
//                treeWFOk.VisibleColumns[0].BestFit();
//            }
//            if (!String.IsNullOrEmpty(treeWFOk.ParentFieldName) && !_controller.IsExpandAll()) {
//                ExpandCheckedNodes();
//            } else if (_controller.IsExpandAll()) {
//                treeWFOk.ExpandAll();
//            }
//            if (!_controller.IsMultiselect()) {
//                object cur_value = this._value;
//                if (Cmn.IsNullOrDBNull(cur_value)) {
//                    ClearListFocus();
//                } else {
//                    TreeListNode node = treeWFOk.FindNodeByKeyID(cur_value);
//                    if (node != null) {
//                        treeWFOk.Selection.Clear();
//                        treeWFOk.FocusedNode = node;
//                        treeWFOk.SelectNode(node);
//                    } else {
//                        ClearListFocus();
//                    }
//                }
//            }
//        }
//        // public event EventHandler PopupContainerEdit_Popup;
//        //public /*временно*/ void popupContainerEdit_Popup(object sender, EventArgs e)
//        //{
//        //    //PrepareList();
//        //}
//        private void ExpandCheckedNodes()
//        {
//            treeWFOk.BeginUpdate();
//            HasCheckedChildren(treeWFOk.Nodes);
//            treeWFOk.MakeNodeVisible(treeWFOk.FocusedNode);
//            treeWFOk.EndUpdate();
//        }
//        private bool HasCheckedChildren(TreeListNodes nodes)
//        {
//            bool has_checked_child = false;
//            bool any_checked = false;
//            foreach (TreeListNode node in nodes) {
//                if (HasCheckedChildren(node.Nodes)) {
//                    has_checked_child = true;
//                }
//                if (Cmn.DECIMAL_ONE.Equals(node["check"])) {
//                    any_checked = true;
//                }
//            }
//            if (has_checked_child) {
//                return true;
//            } else if (any_checked) {
//                treeWFOk.MakeNodeVisible(nodes[0]);
//                return true;
//            } else {
//                return false;
//            }
//        }
//        //void VTextEdit_EditValueChanged(object sender, EventArgs e)
//        //{
//        //    if (ValueChanged != null)
//        //    {
//        //        ValueChanged(this.EditValue);
//        //    }
//        //}
//        //void VTextEdit_Enter(object sender, EventArgs e)
//        //{
//        //    if (Entered != null)
//        //    {
//        //        Entered();
//        //    }
//        //}
//        //public void SetValue(object value)
//        //{
//        //    this.EditValue = value;
//        //}
//        //public event ValueChangeEventHandler ValueChanged;
//        //public event SimpleEventHandler Entered;
//        public Dictionary<string, string> GetFilterValues()
//        {
//            return treeWFOk.GetFilterValues();
//        }
//    }
//}
