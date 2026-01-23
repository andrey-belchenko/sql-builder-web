//namespace sql.builder
//{
//    internal partial class ucQueryEditor
//    {
//        /// <summary> 
//        /// Требуется переменная конструктора.
//        /// </summary>
//        private System.ComponentModel.IContainer components = null;

//        /// <summary> 
//        /// Освободить все используемые ресурсы.
//        /// </summary>
//        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
//        protected override void Dispose(bool disposing)
//        {
//            if (disposing && (components != null))
//            {
//                components.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//        #region Код, автоматически созданный конструктором компонентов

//        /// <summary> 
//        /// Обязательный метод для поддержки конструктора - не изменяйте 
//        /// содержимое данного метода при помощи редактора кода.
//        /// </summary>
//        private void InitializeComponent()
//        {
//            this.components = new System.ComponentModel.Container();
//            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucQueryEditor));
//            this.contextManagers = new sql.builder.ucQueryEditorContextManagers();
//            this.pcProps = new DevExpress.XtraEditors.PanelControl();
//            this.tlQueryScheme = new DevExpress.XtraTreeList.TreeList();
//            this.repositoryItemRichTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemRichTextEdit();
//            this.clUses = new sql.builder.ucQueriesEditorUses();
//            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
//            this.document1 = new DevExpress.XtraBars.Docking2010.Views.Tabbed.Document(this.components);
//            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
//            this.btnSave = new DevExpress.XtraBars.BarButtonItem();
//            this.beFile = new DevExpress.XtraBars.BarEditItem();
//            this.rbeFile = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
//            this.btnAddChild = new DevExpress.XtraBars.BarButtonItem();
//            this.btnAddNext = new DevExpress.XtraBars.BarButtonItem();
//            this.btnMakeSQL = new DevExpress.XtraBars.BarButtonItem();
//            this.btnDel = new DevExpress.XtraBars.BarButtonItem();
//            this.btnOpenLink = new DevExpress.XtraBars.BarButtonItem();
//            this.checkTitleAsNameB = new DevExpress.XtraBars.BarEditItem();
//            this.checkTitleAsName = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
//            this.btnUp = new DevExpress.XtraBars.BarButtonItem();
//            this.btnDown = new DevExpress.XtraBars.BarButtonItem();
//            this.btnRefresh = new DevExpress.XtraBars.BarButtonItem();
//            this.btnSchemeColumns = new DevExpress.XtraBars.BarButtonItem();
//            this.btnRepInfo = new DevExpress.XtraBars.BarButtonItem();
//            this.btnSaveLayout = new DevExpress.XtraBars.BarButtonItem();
//            this.btnOPenLayout = new DevExpress.XtraBars.BarButtonItem();
//            this.btnClearLayout = new DevExpress.XtraBars.BarButtonItem();
//            this.btnDesignForm = new DevExpress.XtraBars.BarButtonItem();
//            this.btnSQLText = new DevExpress.XtraBars.BarButtonItem();
//            this.btnExpSchemeToXl = new DevExpress.XtraBars.BarButtonItem();
//            this.btnOpenFieldLink = new DevExpress.XtraBars.BarButtonItem();
//            this.btnLoadFromDb = new DevExpress.XtraBars.BarButtonItem();
//            this.btnQubeInfo = new DevExpress.XtraBars.BarButtonItem();
//            this.barButtonItem8 = new DevExpress.XtraBars.BarButtonItem();
//            this.btnTextReplace = new DevExpress.XtraBars.BarButtonItem();
//            this.btnUndoChange = new DevExpress.XtraBars.BarButtonItem();
//            this.btnRedoChange = new DevExpress.XtraBars.BarButtonItem();
//            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
//            this.btnRepPkgSctipt = new DevExpress.XtraBars.BarButtonItem();
//            this.btnUpdateNavigators = new DevExpress.XtraBars.BarButtonItem();
//            this.barButtonItem6 = new DevExpress.XtraBars.BarButtonItem();
//            this.btnEditData = new DevExpress.XtraBars.BarButtonItem();
//            this.btnDelete = new DevExpress.XtraBars.BarButtonItem();
//            this.btnRename = new DevExpress.XtraBars.BarButtonItem();
//            this.btnWebMethod = new DevExpress.XtraBars.BarButtonItem();
//            this.btnTempTbl = new DevExpress.XtraBars.BarButtonItem();
//            this.btnGetCode = new DevExpress.XtraBars.BarButtonItem();
//            this.barButtonItem10 = new DevExpress.XtraBars.BarButtonItem();
//            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
//            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
//            this.ribbonPageGroup10 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
//            this.ribbonPageGroup5 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
//            this.ribbonPageGroup4 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
//            this.ribbonPageGroup9 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
//            this.rpgCurrentNode = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
//            this.ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
//            this.ribbonPageGroup7 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
//            this.ribbonPageGroup6 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
//            this.ribbonPageGroup11 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
//            this.ribbonPageGroup8 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
//            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
//            this.documentManager1 = new DevExpress.XtraBars.Docking2010.DocumentManager(this.components);
//            this.tabbedView1 = new DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView(this.components);
//            this.popupMenu = new DevExpress.XtraBars.PopupMenu(this.components);
//            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
//            this.popupMenu1 = new DevExpress.XtraBars.PopupMenu(this.components);
//            this.popupMenu2 = new DevExpress.XtraBars.PopupMenu(this.components);
//            ((System.ComponentModel.ISupportInitialize)(this.pcProps)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.tlQueryScheme)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRichTextEdit1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.document1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rbeFile)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.checkTitleAsName)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.documentManager1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.tabbedView1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.popupMenu)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.popupMenu2)).BeginInit();
//            this.SuspendLayout();
            // 
            // contextManagers
            // 
//            this.contextManagers.Location = new System.Drawing.Point(1107, 175);
//            this.contextManagers.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
//            this.contextManagers.Name = "contextManagers";
//            this.contextManagers.Size = new System.Drawing.Size(339, 514);
//            this.contextManagers.TabIndex = 11;
//            this.contextManagers.ChildItemSelected += new sql.builder.XElementEventHandler(this.contextManagers_ChildItemSelected);
            // 
            // pcProps
            // 
//            this.pcProps.Location = new System.Drawing.Point(3, 175);
//            this.pcProps.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
//            this.pcProps.Name = "pcProps";
//            this.pcProps.Size = new System.Drawing.Size(356, 514);
//            this.pcProps.TabIndex = 7;
            // 
            // tlQueryScheme
            // 
//            this.tlQueryScheme.Cursor = System.Windows.Forms.Cursors.Default;
//            this.tlQueryScheme.DataSource = null;
//            this.tlQueryScheme.ImageIndexFieldName = "image_name";
//            this.tlQueryScheme.KeyFieldName = "id";
//            this.tlQueryScheme.Location = new System.Drawing.Point(391, 182);
//            this.tlQueryScheme.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
//            this.tlQueryScheme.Name = "tlQueryScheme";
//            this.tlQueryScheme.OptionsBehavior.PopulateServiceColumns = true;
//            this.tlQueryScheme.OptionsClipboard.AllowCopy = DevExpress.Utils.DefaultBoolean.False;
//            this.tlQueryScheme.OptionsFilter.FilterMode = DevExpress.XtraTreeList.FilterMode.Extended;
//            this.tlQueryScheme.OptionsNavigation.AutoFocusNewNode = true;
//            this.tlQueryScheme.OptionsSelection.EnableAppearanceFocusedCell = false;
//            this.tlQueryScheme.OptionsSelection.MultiSelect = true;
//            this.tlQueryScheme.OptionsView.FocusRectStyle = DevExpress.XtraTreeList.DrawFocusRectStyle.None;
//            this.tlQueryScheme.OptionsView.ShowAutoFilterRow = true;
//            this.tlQueryScheme.ParentFieldName = "parent_id";
//            this.tlQueryScheme.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
//            this.repositoryItemRichTextEdit1});
//            this.tlQueryScheme.Size = new System.Drawing.Size(317, 514);
//            this.tlQueryScheme.TabIndex = 4;
//            this.tlQueryScheme.CustomNodeCellEdit += new DevExpress.XtraTreeList.GetCustomNodeCellEditEventHandler(this.tlQueryScheme_CustomNodeCellEdit);
//            this.tlQueryScheme.AfterExpand += new DevExpress.XtraTreeList.NodeEventHandler(this.tlQueryScheme_AfterExpand);
//            this.tlQueryScheme.AfterCollapse += new DevExpress.XtraTreeList.NodeEventHandler(this.tlQueryScheme_AfterCollapse);
//            this.tlQueryScheme.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.tlQueryScheme_FocusedNodeChanged);
//            this.tlQueryScheme.SelectionChanged += new System.EventHandler(this.tlQueryScheme_SelectionChanged);
//            this.tlQueryScheme.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tlQueryScheme_KeyDown);
            // 
            // repositoryItemRichTextEdit1
            // 
//            this.repositoryItemRichTextEdit1.DocumentFormat = DevExpress.XtraRichEdit.DocumentFormat.Html;
//            this.repositoryItemRichTextEdit1.Name = "repositoryItemRichTextEdit1";
//            this.repositoryItemRichTextEdit1.ShowCaretInReadOnly = false;
            // 
            // clUses
            // 
//            this.clUses.Location = new System.Drawing.Point(731, 182);
//            this.clUses.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
//            this.clUses.Name = "clUses";
//            this.clUses.Size = new System.Drawing.Size(313, 514);
//            this.clUses.TabIndex = 0;
            // 
            // panelControl1
            // 
//            this.panelControl1.Location = new System.Drawing.Point(1246, 48);
//            this.panelControl1.Name = "panelControl1";
//            this.panelControl1.Size = new System.Drawing.Size(181, 343);
//            this.panelControl1.TabIndex = 6;
            // 
            // document1
            // 
//            this.document1.Caption = "Схема запроса";
//            this.document1.ControlName = "dpQueryScheme";
//            this.document1.FloatLocation = new System.Drawing.Point(550, 386);
//            this.document1.FloatSize = new System.Drawing.Size(200, 200);
//            this.document1.Properties.AllowClose = DevExpress.Utils.DefaultBoolean.False;
//            this.document1.Properties.AllowFloat = DevExpress.Utils.DefaultBoolean.True;
//            this.document1.Properties.AllowFloatOnDoubleClick = DevExpress.Utils.DefaultBoolean.True;
            // 
            // ribbonControl1
            // 
//            this.ribbonControl1.ExpandCollapseItem.Id = 0;
//            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
//            this.ribbonControl1.ExpandCollapseItem,
//            this.btnSave,
//            this.beFile,
//            this.btnAddChild,
//            this.btnAddNext,
//            this.btnMakeSQL,
//            this.btnDel,
//            this.btnOpenLink,
//            this.checkTitleAsNameB,
//            this.btnUp,
//            this.btnDown,
//            this.btnRefresh,
//            this.btnSchemeColumns,
//            this.btnRepInfo,
//            this.btnSaveLayout,
//            this.btnOPenLayout,
//            this.btnClearLayout,
//            this.btnDesignForm,
//            this.btnSQLText,
//            this.btnExpSchemeToXl,
//            this.btnOpenFieldLink,
//            this.btnLoadFromDb,
//            this.btnQubeInfo,
//            this.barButtonItem8,
//            this.btnTextReplace,
//            this.btnUndoChange,
//            this.btnRedoChange,
//            this.barButtonItem1,
//            this.btnRepPkgSctipt,
//            this.btnUpdateNavigators,
//            this.barButtonItem6,
//            this.btnEditData,
//            this.btnDelete,
//            this.btnRename,
//            this.btnWebMethod,
//            this.btnTempTbl,
//            this.btnGetCode,
//            this.barButtonItem10});
//            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
//            this.ribbonControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
//            this.ribbonControl1.MaxItemId = 50;
//            this.ribbonControl1.Name = "ribbonControl1";
//            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
//            this.ribbonPage1});
//            this.ribbonControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
//            this.rbeFile,
//            this.checkTitleAsName});
//            this.ribbonControl1.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
//            this.ribbonControl1.ShowToolbarCustomizeItem = false;
//            this.ribbonControl1.Size = new System.Drawing.Size(2057, 145);
//            this.ribbonControl1.Toolbar.ShowCustomizeItem = false;
//            this.ribbonControl1.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;
            // 
            // btnSave
            // 
//            this.btnSave.Caption = "Сохранить";
//            this.btnSave.Id = 1;
//            this.btnSave.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S));
//            this.btnSave.Name = "btnSave";
//            this.btnSave.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnSave_ItemClick);
            // 
            // beFile
            // 
//            this.beFile.Edit = this.rbeFile;
//            this.beFile.EditValue = "";
//            this.beFile.EditWidth = 300;
//            this.beFile.Id = 2;
//            this.beFile.Name = "beFile";
//            this.beFile.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.beFile_ItemClick);
            // 
            // rbeFile
            // 
//            this.rbeFile.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
//            this.rbeFile.Appearance.Options.UseFont = true;
//            this.rbeFile.AutoHeight = false;
//            this.rbeFile.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
//            new DevExpress.XtraEditors.Controls.EditorButton()});
//            this.rbeFile.Name = "rbeFile";
//            this.rbeFile.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
//            this.rbeFile.ButtonPressed += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.repositoryItemButtonEdit1_ButtonPressed);
//            this.rbeFile.CustomDisplayText += new DevExpress.XtraEditors.Controls.CustomDisplayTextEventHandler(this.repositoryItemButtonEdit1_CustomDisplayText);
//            this.rbeFile.Click += new System.EventHandler(this.repositoryItemButtonEdit1_Click_1);
            // 
            // btnAddChild
            // 
//            this.btnAddChild.Caption = "Добавить дочерний";
//            this.btnAddChild.Id = 3;
//            this.btnAddChild.ItemShortcut = new DevExpress.XtraBars.BarShortcut(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
//                | System.Windows.Forms.Keys.Insert));
//            this.btnAddChild.Name = "btnAddChild";
//            this.btnAddChild.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnAddChild_ItemClick);
            // 
            // btnAddNext
            // 
//            this.btnAddNext.Caption = "Добавить следующий";
//            this.btnAddNext.Id = 4;
//            this.btnAddNext.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Insert));
//            this.btnAddNext.Name = "btnAddNext";
//            this.btnAddNext.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnAddNext_ItemClick);
            // 
            // btnMakeSQL
            // 
//            this.btnMakeSQL.Caption = "Получить SQL";
//            this.btnMakeSQL.Id = 5;
//            this.btnMakeSQL.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E), (System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y));
//            this.btnMakeSQL.Name = "btnMakeSQL";
//            this.btnMakeSQL.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnMakeSQL_ItemClick);
            // 
            // btnDel
            // 
//            this.btnDel.Caption = "Удалить";
//            this.btnDel.Id = 6;
//            this.btnDel.Name = "btnDel";
//            this.btnDel.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnDel_ItemClick);
            // 
            // btnOpenLink
            // 
//            this.btnOpenLink.Caption = "Перейти к определению";
//            this.btnOpenLink.Id = 7;
//            this.btnOpenLink.Name = "btnOpenLink";
//            this.btnOpenLink.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnOpenLink_ItemClick);
            // 
            // checkTitleAsNameB
            // 
//            this.checkTitleAsNameB.Caption = "Заголовки как наименования колонок";
//            this.checkTitleAsNameB.Edit = this.checkTitleAsName;
//            this.checkTitleAsNameB.EditValue = false;
//            this.checkTitleAsNameB.Id = 9;
//            this.checkTitleAsNameB.Name = "checkTitleAsNameB";
            // 
            // checkTitleAsName
            // 
//            this.checkTitleAsName.Appearance.BackColor = System.Drawing.Color.Transparent;
//            this.checkTitleAsName.Appearance.Options.UseBackColor = true;
//            this.checkTitleAsName.AutoHeight = false;
//            this.checkTitleAsName.Name = "checkTitleAsName";
            // 
            // btnUp
            // 
//            this.btnUp.Caption = "Переметить выше";
//            this.btnUp.Id = 10;
//            this.btnUp.Name = "btnUp";
//            this.btnUp.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnUp_ItemClick);
            // 
            // btnDown
            // 
//            this.btnDown.Caption = "Переместить ниже";
//            this.btnDown.Id = 11;
//            this.btnDown.Name = "btnDown";
            // 
            // btnRefresh
            // 
//            this.btnRefresh.Caption = "Обновить";
//            this.btnRefresh.Id = 12;
//            this.btnRefresh.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R));
//            this.btnRefresh.Name = "btnRefresh";
//            this.btnRefresh.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnRefresh_ItemClick);
            // 
            // btnSchemeColumns
            // 
//            this.btnSchemeColumns.Caption = "Выбор колонок";
//            this.btnSchemeColumns.Id = 13;
//            this.btnSchemeColumns.Name = "btnSchemeColumns";
//            this.btnSchemeColumns.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnSchemeColumns_ItemClick);
            // 
            // btnRepInfo
            // 
//            this.btnRepInfo.Caption = "Информация по отчету";
//            this.btnRepInfo.Id = 14;
//            this.btnRepInfo.Name = "btnRepInfo";
//            this.btnRepInfo.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            // 
            // btnSaveLayout
            // 
//            this.btnSaveLayout.Caption = "Сохранить расположение";
//            this.btnSaveLayout.Id = 15;
//            this.btnSaveLayout.ItemClickFireMode = DevExpress.XtraBars.BarItemEventFireMode.Immediate;
//            this.btnSaveLayout.Name = "btnSaveLayout";
//            this.btnSaveLayout.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnSaveLayout_ItemClick);
            // 
            // btnOPenLayout
            // 
//            this.btnOPenLayout.Caption = "Применить расположение";
//            this.btnOPenLayout.Id = 16;
//            this.btnOPenLayout.Name = "btnOPenLayout";
//            this.btnOPenLayout.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnOPenLayout_ItemClick);
            // 
            // btnClearLayout
            // 
//            this.btnClearLayout.Caption = "Очитстить";
//            this.btnClearLayout.Id = 17;
//            this.btnClearLayout.Name = "btnClearLayout";
//            this.btnClearLayout.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnClearLayout_ItemClick);
            // 
            // btnDesignForm
            // 
//            this.btnDesignForm.Caption = "Дизайн формы";
//            this.btnDesignForm.Id = 18;
//            this.btnDesignForm.Name = "btnDesignForm";
//            this.btnDesignForm.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnDesignForm_ItemClick);
            // 
            // btnSQLText
            // 
//            this.btnSQLText.Caption = "Тест запроса";
//            this.btnSQLText.Id = 19;
//            this.btnSQLText.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E), (System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T));
//            this.btnSQLText.Name = "btnSQLText";
//            this.btnSQLText.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnSQLText_ItemClick);
            // 
            // btnExpSchemeToXl
            // 
//            this.btnExpSchemeToXl.Caption = "Экспорт в Excel";
//            this.btnExpSchemeToXl.Id = 19;
//            this.btnExpSchemeToXl.Name = "btnExpSchemeToXl";
//            this.btnExpSchemeToXl.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnExpSchemeToXl_ItemClick);
            // 
            // btnOpenFieldLink
            // 
//            this.btnOpenFieldLink.Caption = "Открыть элемент по ссылке";
//            this.btnOpenFieldLink.Id = 20;
//            this.btnOpenFieldLink.Name = "btnOpenFieldLink";
//            this.btnOpenFieldLink.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnOpenFieldLink_ItemClick);
            // 
            // btnLoadFromDb
            // 
//            this.btnLoadFromDb.Caption = "Загрузить из БД (выделенные)";
//            this.btnLoadFromDb.Id = 25;
//            this.btnLoadFromDb.Name = "btnLoadFromDb";
//            this.btnLoadFromDb.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnLoadFromDb_ItemClick);
            // 
            // btnQubeInfo
            // 
//            this.btnQubeInfo.Caption = "QubeInfo";
//            this.btnQubeInfo.Id = 26;
//            this.btnQubeInfo.Name = "btnQubeInfo";
//            this.btnQubeInfo.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnQubeInfo_ItemClick);
            // 
            // barButtonItem8
            // 
//            this.barButtonItem8.Caption = "Скрипт таблицы (изменения)";
//            this.barButtonItem8.Id = 27;
//            this.barButtonItem8.Name = "barButtonItem8";
//            this.barButtonItem8.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem8_ItemClick);
            // 
            // btnTextReplace
            // 
//            this.btnTextReplace.Caption = "Замена текста";
//            this.btnTextReplace.Id = 28;
//            this.btnTextReplace.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H));
//            this.btnTextReplace.Name = "btnTextReplace";
//            this.btnTextReplace.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnTextReplace_ItemClick);
            // 
            // btnUndoChange
            // 
//            this.btnUndoChange.Caption = "Отменить действие";
//            this.btnUndoChange.Id = 29;
//            this.btnUndoChange.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnUndoChange.ImageOptions.Image")));
//            this.btnUndoChange.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnUndoChange.ImageOptions.LargeImage")));
//            this.btnUndoChange.Name = "btnUndoChange";
//            this.btnUndoChange.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText;
//            this.btnUndoChange.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnUndoChange_ItemClick);
            // 
            // btnRedoChange
            // 
//            this.btnRedoChange.Caption = "Повторить действие";
//            this.btnRedoChange.Id = 30;
//            this.btnRedoChange.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnRedoChange.ImageOptions.Image")));
//            this.btnRedoChange.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnRedoChange.ImageOptions.LargeImage")));
//            this.btnRedoChange.Name = "btnRedoChange";
//            this.btnRedoChange.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText;
//            this.btnRedoChange.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnRedoChange_ItemClick);
            // 
            // barButtonItem1
            // 
//            this.barButtonItem1.Caption = "Скрипт мат. view";
//            this.barButtonItem1.Id = 32;
//            this.barButtonItem1.Name = "barButtonItem1";
//            this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick);
            // 
            // btnRepPkgSctipt
            // 
//            this.btnRepPkgSctipt.Caption = "Скрипт пакета";
//            this.btnRepPkgSctipt.Id = 33;
//            this.btnRepPkgSctipt.Name = "btnRepPkgSctipt";
//            this.btnRepPkgSctipt.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem6_ItemClick);
            // 
            // btnUpdateNavigators
            // 
//            this.btnUpdateNavigators.Caption = "Обновить навигаторы";
//            this.btnUpdateNavigators.Id = 34;
//            this.btnUpdateNavigators.Name = "btnUpdateNavigators";
//            this.btnUpdateNavigators.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnUpdateNavigators_ItemClick);
            // 
            // barButtonItem6
            // 
//            this.barButtonItem6.Caption = "Скрипт таблицы (создание)";
//            this.barButtonItem6.Id = 35;
//            this.barButtonItem6.Name = "barButtonItem6";
//            this.barButtonItem6.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem6_ItemClick_1);
            // 
            // btnEditData
            // 
//            this.btnEditData.Caption = "Редактировать данные";
//            this.btnEditData.Id = 38;
//            this.btnEditData.Name = "btnEditData";
//            this.btnEditData.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnEditData_ItemClick);
            // 
            // btnDelete
            // 
//            this.btnDelete.Caption = "Удалить";
//            this.btnDelete.Id = 39;
//            this.btnDelete.Name = "btnDelete";
//            this.btnDelete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnDelete_ItemClick);
            // 
            // btnRename
            // 
//            this.btnRename.Caption = "Переименовать";
//            this.btnRename.Id = 44;
//            this.btnRename.Name = "btnRename";
//            this.btnRename.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnRename_ItemClick);
            // 
            // btnWebMethod
            // 
//            this.btnWebMethod.Id = 47;
//            this.btnWebMethod.Name = "btnWebMethod";
            // 
            // btnTempTbl
            // 
//            this.btnTempTbl.Caption = "Скрипт временной таблицы";
//            this.btnTempTbl.Id = 46;
//            this.btnTempTbl.Name = "btnTempTbl";
//            this.btnTempTbl.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnTempTbl_ItemClick);
            // 
            // btnGetCode
            // 
//            this.btnGetCode.Caption = "C# код узла";
//            this.btnGetCode.Id = 48;
//            this.btnGetCode.Name = "btnGetCode";
//            this.btnGetCode.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnGetCode_ItemClick);
            // 
            // barButtonItem10
            // 
//            this.barButtonItem10.Caption = "Обернуть в функцию";
//            this.barButtonItem10.Id = 49;
//            this.barButtonItem10.Name = "barButtonItem10";
//            this.barButtonItem10.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem10_ItemClick);
            // 
            // ribbonPage1
            // 
//            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
//            this.ribbonPageGroup1,
//            this.ribbonPageGroup10,
//            this.ribbonPageGroup5,
//            this.ribbonPageGroup4,
//            this.ribbonPageGroup9,
//            this.rpgCurrentNode,
//            this.ribbonPageGroup3,
//            this.ribbonPageGroup7,
//            this.ribbonPageGroup6,
//            this.ribbonPageGroup11,
//            this.ribbonPageGroup8});
//            this.ribbonPage1.Name = "ribbonPage1";
//            this.ribbonPage1.Text = "Редактор схемы";
            // 
            // ribbonPageGroup1
            // 
//            this.ribbonPageGroup1.ItemLinks.Add(this.beFile);
//            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
//            this.ribbonPageGroup1.Text = "Файл";
            // 
            // ribbonPageGroup10
            // 
//            this.ribbonPageGroup10.ItemLinks.Add(this.btnLoadFromDb);
//            this.ribbonPageGroup10.Name = "ribbonPageGroup10";
//            this.ribbonPageGroup10.Text = "Среда";
            // 
            // ribbonPageGroup5
            // 
//            this.ribbonPageGroup5.ItemLinks.Add(this.btnSchemeColumns);
//            this.ribbonPageGroup5.ItemLinks.Add(this.btnExpSchemeToXl);
//            this.ribbonPageGroup5.Name = "ribbonPageGroup5";
//            this.ribbonPageGroup5.Text = "Схема";
            // 
            // ribbonPageGroup4
            // 
//            this.ribbonPageGroup4.ItemLinks.Add(this.btnSave);
//            this.ribbonPageGroup4.ItemLinks.Add(this.btnRefresh);
//            this.ribbonPageGroup4.ItemLinks.Add(this.btnDelete);
//            this.ribbonPageGroup4.Name = "ribbonPageGroup4";
//            this.ribbonPageGroup4.Text = "Элемент";
            // 
            // ribbonPageGroup9
            // 
//            this.ribbonPageGroup9.ItemLinks.Add(this.btnUndoChange);
//            this.ribbonPageGroup9.ItemLinks.Add(this.btnRedoChange);
//            this.ribbonPageGroup9.Name = "ribbonPageGroup9";
//            this.ribbonPageGroup9.Text = "Редактирование";
            // 
            // rpgCurrentNode
            // 
//            this.rpgCurrentNode.ItemLinks.Add(this.btnOpenLink);
//            this.rpgCurrentNode.ItemLinks.Add(this.btnAddChild);
//            this.rpgCurrentNode.ItemLinks.Add(this.btnAddNext);
//            this.rpgCurrentNode.ItemLinks.Add(this.btnDel);
//            this.rpgCurrentNode.ItemLinks.Add(this.btnUp);
//            this.rpgCurrentNode.ItemLinks.Add(this.btnDown);
//            this.rpgCurrentNode.ItemLinks.Add(this.btnGetCode);
//            this.rpgCurrentNode.ItemLinks.Add(this.barButtonItem10);
//            this.rpgCurrentNode.Name = "rpgCurrentNode";
//            this.rpgCurrentNode.Text = "Текущий узел";
            // 
            // ribbonPageGroup3
            // 
//            this.ribbonPageGroup3.ItemLinks.Add(this.btnMakeSQL);
//            this.ribbonPageGroup3.ItemLinks.Add(this.btnSQLText);
//            this.ribbonPageGroup3.ItemLinks.Add(this.btnQubeInfo);
//            this.ribbonPageGroup3.ItemLinks.Add(this.barButtonItem6);
//            this.ribbonPageGroup3.ItemLinks.Add(this.barButtonItem8);
//            this.ribbonPageGroup3.ItemLinks.Add(this.barButtonItem1);
//            this.ribbonPageGroup3.ItemLinks.Add(this.btnRepPkgSctipt);
//            this.ribbonPageGroup3.ItemLinks.Add(this.btnUpdateNavigators);
//            this.ribbonPageGroup3.ItemLinks.Add(this.btnEditData);
//            this.ribbonPageGroup3.ItemLinks.Add(this.btnTempTbl);
//            this.ribbonPageGroup3.Name = "ribbonPageGroup3";
//            this.ribbonPageGroup3.Text = "Функции элемента";
            // 
            // ribbonPageGroup7
            // 
//            this.ribbonPageGroup7.ItemLinks.Add(this.btnOpenFieldLink);
//            this.ribbonPageGroup7.Name = "ribbonPageGroup7";
//            this.ribbonPageGroup7.Text = "Текущее поле";
            // 
            // ribbonPageGroup6
            // 
//            this.ribbonPageGroup6.ItemLinks.Add(this.btnSaveLayout);
//            this.ribbonPageGroup6.ItemLinks.Add(this.btnOPenLayout);
//            this.ribbonPageGroup6.ItemLinks.Add(this.btnClearLayout);
//            this.ribbonPageGroup6.Name = "ribbonPageGroup6";
//            this.ribbonPageGroup6.Text = "Интерфейс";
            // 
            // ribbonPageGroup11
            // 
//            this.ribbonPageGroup11.ItemLinks.Add(this.btnRename);
//            this.ribbonPageGroup11.Name = "ribbonPageGroup11";
//            this.ribbonPageGroup11.Text = "Менеджер ссылок";
            // 
            // ribbonPageGroup8
            // 
//            this.ribbonPageGroup8.ItemLinks.Add(this.checkTitleAsNameB);
//            this.ribbonPageGroup8.ItemLinks.Add(this.btnDesignForm);
//            this.ribbonPageGroup8.ItemLinks.Add(this.btnRepInfo);
//            this.ribbonPageGroup8.Name = "ribbonPageGroup8";
//            this.ribbonPageGroup8.Text = "Корзина (не используются но вдруг понадобится)";
//            this.ribbonPageGroup8.Visible = false;
            // 
            // saveFileDialog1
            // 
//            this.saveFileDialog1.Filter = "XML files|*.xml";
//            this.saveFileDialog1.OverwritePrompt = false;
            // 
            // documentManager1
            // 
//            this.documentManager1.ContainerControl = this;
//            this.documentManager1.MenuManager = this.ribbonControl1;
//            this.documentManager1.View = this.tabbedView1;
//            this.documentManager1.ViewCollection.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseView[] {
//            this.tabbedView1});
            // 
            // popupMenu
            // 
//            this.popupMenu.ItemLinks.Add(this.btnTextReplace);
//            this.popupMenu.Name = "popupMenu";
//            this.popupMenu.Ribbon = this.ribbonControl1;
            // 
            // openFileDialog1
            // 
//            this.openFileDialog1.CheckFileExists = false;
//            this.openFileDialog1.FileName = "openFileDialog1";
//            this.openFileDialog1.Filter = "XML files|*.xml";
//            this.openFileDialog1.ReadOnlyChecked = true;
            // 
            // popupMenu1
            // 
//            this.popupMenu1.Name = "popupMenu1";
//            this.popupMenu1.Ribbon = this.ribbonControl1;
            // 
            // popupMenu2
            // 
//            this.popupMenu2.Name = "popupMenu2";
//            this.popupMenu2.Ribbon = this.ribbonControl1;
            // 
            // ucQueryEditor
            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.Controls.Add(this.tlQueryScheme);
//            this.Controls.Add(this.contextManagers);
//            this.Controls.Add(this.clUses);
//            this.Controls.Add(this.ribbonControl1);
//            this.Controls.Add(this.pcProps);
//            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
//            this.Name = "ucQueryEditor";
//            this.Size = new System.Drawing.Size(2057, 693);
//            this.Load += new System.EventHandler(this.ucQueryEditor_Load);
//            ((System.ComponentModel.ISupportInitialize)(this.pcProps)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.tlQueryScheme)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRichTextEdit1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.document1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rbeFile)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.checkTitleAsName)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.documentManager1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.tabbedView1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.popupMenu)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.popupMenu2)).EndInit();
//            this.ResumeLayout(false);
//            this.PerformLayout();

//        }

//        #endregion

//        private DevExpress.XtraTreeList.TreeList tlQueryScheme;
//        private DevExpress.XtraEditors.PanelControl panelControl1;
//        private DevExpress.XtraEditors.PanelControl pcProps;
//        private ucQueryEditorContextManagers contextManagers;
//        private DevExpress.XtraBars.Docking2010.Views.Tabbed.Document document1;
//        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
//        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
//        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
//        private DevExpress.XtraBars.BarButtonItem btnSave;
//        private DevExpress.XtraEditors.Repository.RepositoryItemRichTextEdit repositoryItemRichTextEdit1;
//        private DevExpress.XtraBars.BarEditItem beFile;
//        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit rbeFile;
   
//        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
//        private DevExpress.XtraBars.BarButtonItem btnAddChild;
//        private DevExpress.XtraBars.BarButtonItem btnAddNext;
//        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rpgCurrentNode;
//        private DevExpress.XtraBars.BarButtonItem btnMakeSQL;
//        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup3;
//        private ucQueriesEditorUses clUses;
//        private DevExpress.XtraBars.BarButtonItem btnDel;
//        private DevExpress.XtraBars.BarButtonItem btnOpenLink;
//        private DevExpress.XtraBars.BarEditItem checkTitleAsNameB;
//        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit checkTitleAsName;
//        private DevExpress.XtraBars.BarButtonItem btnUp;
//        private DevExpress.XtraBars.BarButtonItem btnDown;
//        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup4;
//        private DevExpress.XtraBars.BarButtonItem btnRefresh;
//        private DevExpress.XtraBars.Docking2010.DocumentManager documentManager1;
//        private DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView tabbedView1;
//        private DevExpress.XtraBars.BarButtonItem btnSchemeColumns;
//        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup5;
//        private DevExpress.XtraBars.BarButtonItem btnRepInfo;
//        private DevExpress.XtraBars.BarButtonItem btnSaveLayout;
//        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup6;
//        private DevExpress.XtraBars.BarButtonItem btnOPenLayout;
//        private DevExpress.XtraBars.BarButtonItem btnClearLayout;
//        private DevExpress.XtraBars.BarButtonItem btnDesignForm;
//        private DevExpress.XtraBars.BarButtonItem btnExpSchemeToXl;
//        private DevExpress.XtraBars.BarButtonItem btnSQLText;
//        private DevExpress.XtraBars.BarButtonItem btnOpenFieldLink;
//        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup7;
//        private DevExpress.XtraBars.BarButtonItem btnLoadFromDb;
//        private DevExpress.XtraBars.BarButtonItem btnQubeInfo;
//        private DevExpress.XtraBars.BarButtonItem barButtonItem8;
//        private DevExpress.XtraBars.PopupMenu popupMenu;
//        private DevExpress.XtraBars.BarButtonItem btnTextReplace;
//        private DevExpress.XtraBars.BarButtonItem btnUndoChange;
//        private DevExpress.XtraBars.BarButtonItem btnRedoChange;
//        private System.Windows.Forms.OpenFileDialog openFileDialog1;
//        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
//        private DevExpress.XtraBars.BarButtonItem btnRepPkgSctipt;
//        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup8;
//        private DevExpress.XtraBars.BarButtonItem btnUpdateNavigators;
//        private DevExpress.XtraBars.BarButtonItem barButtonItem6;

//        private DevExpress.XtraBars.PopupMenu popupMenu1;
//        private DevExpress.XtraBars.PopupMenu popupMenu2;
//        private DevExpress.XtraBars.BarButtonItem btnEditData;
//        private DevExpress.XtraBars.BarButtonItem btnDelete;
//        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup9;
//        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup10;
//        private DevExpress.XtraBars.BarButtonItem btnRename;
//        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup11;
//        private DevExpress.XtraBars.BarButtonItem btnWebMethod;
//        private DevExpress.XtraBars.BarButtonItem btnTempTbl;
//        private DevExpress.XtraBars.BarButtonItem btnGetCode;
//        private DevExpress.XtraBars.BarButtonItem barButtonItem10;
//    }
//}
