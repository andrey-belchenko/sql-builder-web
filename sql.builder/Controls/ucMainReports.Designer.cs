//namespace sql.builder.Controls
//{
//    internal partial class ucMainReports
//    {
//        /// <summary>
//        /// Required designer variable.
//        /// </summary>
//        private System.ComponentModel.IContainer components = null;

//        #region Windows Form Designer generated code

//        /// <summary>
//        /// Required method for Designer support - do not modify
//        /// the contents of this method with the code editor.
//        /// </summary>
//        private void InitializeComponent()
//        {
//            this.components = new System.ComponentModel.Container();
//            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucMainReports));
//            this.btnColumnsEditor = new DevExpress.XtraBars.BarButtonItem();
//            this.ribbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
//            this.btnCreateTemplate = new DevExpress.XtraBars.BarButtonItem();
//            this.btnSaveTemplate = new DevExpress.XtraBars.BarButtonItem();
//            this.btnDeleteTemplate = new DevExpress.XtraBars.BarButtonItem();
//            this.btnExecuteReport = new DevExpress.XtraBars.BarButtonItem();
//            this.btnExportToXls = new DevExpress.XtraBars.BarButtonItem();
//            this.dbtnPrintForm = new DevExpress.XtraBars.BarLinkContainerItem();
//            this.cbDevexpressSkins = new DevExpress.XtraBars.BarEditItem();
//            this.rcbDevexpressSkins = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
//            this.cbLayouts = new DevExpress.XtraBars.BarEditItem();
//            this.rcbLayouts = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
//            this.btnWorkFolderPath = new DevExpress.XtraBars.BarEditItem();
//            this.rbtnWorkFolderPath = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
//            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
//            this.btnOpenTestQuery = new DevExpress.XtraBars.BarButtonItem();
//            this.btnCCBSQL = new DevExpress.XtraBars.BarButtonItem();
//            this.btnSaveReportToFile = new DevExpress.XtraBars.BarButtonItem();
//            this.btnCompareReports = new DevExpress.XtraBars.BarButtonItem();
//            this.btnLoadReportFromFile = new DevExpress.XtraBars.BarButtonItem();
//            this.btnReportParams = new DevExpress.XtraBars.BarButtonItem();
//            this.btnColsInfoForExcel = new DevExpress.XtraBars.BarButtonItem();
//            this.btnShowXMLSchema = new DevExpress.XtraBars.BarButtonItem();
//            this.btnClearRegistry = new DevExpress.XtraBars.BarButtonItem();
//            this.btnChangeGridMode = new DevExpress.XtraBars.BarButtonItem();
//            this.barCheckItemDebug = new DevExpress.XtraBars.BarCheckItem();
//            this.btnReloadXml = new DevExpress.XtraBars.BarButtonItem();
//            this.btnOpenStartupPath = new DevExpress.XtraBars.BarButtonItem();
//            this.btnTestOpenedReports = new DevExpress.XtraBars.BarButtonItem();
//            this.barStaticItem2 = new DevExpress.XtraBars.BarStaticItem();
//            this.barCheckItem1 = new DevExpress.XtraBars.BarCheckItem();
//            this.ceStoreDefaultParams = new DevExpress.XtraBars.BarEditItem();
//            this.rceStoreDefaultParams = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
//            this.btnExpressReport = new DevExpress.XtraBars.BarButtonItem();
//            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
//            this.ceUseRepository = new DevExpress.XtraBars.BarEditItem();
//            this.rceUseRepository = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
//            this.barButtonItem9 = new DevExpress.XtraBars.BarButtonItem();
//            this.btnWordPrintTest = new DevExpress.XtraBars.BarButtonItem();
//            this.btnExecuteVertica = new DevExpress.XtraBars.BarButtonItem();
//            this.btnTestAllReports = new DevExpress.XtraBars.BarButtonItem();
//            this.btnTest = new DevExpress.XtraBars.BarButtonItem();
//            this.btnLoadLogParams = new DevExpress.XtraBars.BarButtonItem();
//            this.btnCompareReports2 = new DevExpress.XtraBars.BarButtonItem();
//            this.ceShowInvisibleReports = new DevExpress.XtraBars.BarEditItem();
//            this.rceShowInvisibleReports = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
//            this.barButtonItem10 = new DevExpress.XtraBars.BarButtonItem();
//            this.btnReloadNavigator = new DevExpress.XtraBars.BarButtonItem();
//            this.btnTFSAutoCheckIn = new DevExpress.XtraBars.BarButtonItem();
//            this.btnGroupEditor = new DevExpress.XtraBars.BarButtonItem();
//            this.barButtonItem12 = new DevExpress.XtraBars.BarButtonItem();
//            this.btnSaveAllData = new DevExpress.XtraBars.BarButtonItem();
//            this.btnRefreshData = new DevExpress.XtraBars.BarButtonItem();
//            this.barEditItem2 = new DevExpress.XtraBars.BarEditItem();
//            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
//            this.barMsbiButton = new DevExpress.XtraBars.BarButtonItem();
//            this.btnParametersXML = new DevExpress.XtraBars.BarButtonItem();
//            this.btnGenerateSQL = new DevExpress.XtraBars.BarButtonItem();
//            this.btnRegExtact = new DevExpress.XtraBars.BarButtonItem();
//            this.btnGenerateLKK = new DevExpress.XtraBars.BarButtonItem();
//            this.btnASUTPPackage = new DevExpress.XtraBars.BarButtonItem();
//            this.btnASUTPDoc = new DevExpress.XtraBars.BarButtonItem();
//            this.btnASUTPPackageT = new DevExpress.XtraBars.BarButtonItem();
//            this.rpReports = new DevExpress.XtraBars.Ribbon.RibbonPage();
//            this.rpgReport = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
//            this.rpgReportTemplates = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
//            this.rpgReportParams = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
//            this.rpgWorkDirectory = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
//            this.rpgDataEditor = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
//            this.rpgHelp = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
//            this.rpgRepository = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
//            this.rpgDebug = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
//            this.rpgTrash = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
//            this.rpSettings = new DevExpress.XtraBars.Ribbon.RibbonPage();
//            this.rpgVisual = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
//            this.rpgTest = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
//            this.rpgDashboard = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
//            this.rteWorkFolderPath = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
//            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
//            this.rbiUpdateTableScheme = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
//            this.rpcReportParams = new DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit();
//            this.rceOldCompile = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
//            this.dockManager = new DevExpress.XtraBars.Docking.DockManager(this.components);
//            this.dmReports = new DevExpress.XtraBars.Docking2010.DocumentManager(this.components);
//            this.ic = new DevExpress.Utils.ImageCollection(this.components);
//            this.dmView = new DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView(this.components);
//            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
//            this.barEditItem1 = new DevExpress.XtraBars.BarEditItem();
//            this.rpgWebDev = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
//            this.ceDevMode = new DevExpress.XtraBars.BarEditItem();
//            this.rceDevMode = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
//            this.btnDevPrepareData = new DevExpress.XtraBars.BarButtonItem();
//            this.btnDevPrint = new DevExpress.XtraBars.BarButtonItem();
//            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rcbDevexpressSkins)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rcbLayouts)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rbtnWorkFolderPath)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rceStoreDefaultParams)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rceUseRepository)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rceShowInvisibleReports)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rteWorkFolderPath)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rbiUpdateTableScheme)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rpcReportParams)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rceOldCompile)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.dockManager)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.dmReports)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.ic)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.dmView)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rceDevMode)).BeginInit();
//            this.SuspendLayout();
            // 
            // btnColumnsEditor
            // 
//            this.btnColumnsEditor.Caption = "Настройка колонок";
//            this.btnColumnsEditor.Enabled = false;
//            this.btnColumnsEditor.Hint = "Настройка порядка и видимости колонок отчёта";
//            this.btnColumnsEditor.Id = 36;
//            this.btnColumnsEditor.Name = "btnColumnsEditor";
//            this.btnColumnsEditor.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnColumnsEditor_ItemClick);
            // 
            // ribbon
            // 
//            this.ribbon.ExpandCollapseItem.Id = 0;
//            this.ribbon.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
//            this.ribbon.ExpandCollapseItem,
//            this.btnCreateTemplate,
//            this.btnSaveTemplate,
//            this.btnDeleteTemplate,
//            this.btnExecuteReport,
//            this.btnExportToXls,
//            this.dbtnPrintForm,
//            this.cbDevexpressSkins,
//            this.cbLayouts,
//            this.btnWorkFolderPath,
//            this.barButtonItem1,
//            this.btnOpenTestQuery,
//            this.btnCCBSQL,
//            this.btnSaveReportToFile,
//            this.btnCompareReports,
//            this.btnLoadReportFromFile,
//            this.btnReportParams,
//            this.btnColsInfoForExcel,
//            this.btnShowXMLSchema,
//            this.btnColumnsEditor,
//            this.btnClearRegistry,
//            this.btnChangeGridMode,
//            this.barCheckItemDebug,
//            this.btnReloadXml,
//            this.btnOpenStartupPath,
//            this.btnTestOpenedReports,
//            this.barStaticItem2,
//            this.barCheckItem1,
//            this.ceStoreDefaultParams,
//            this.btnExpressReport,
//            this.barButtonItem3,
//            this.ceUseRepository,
//            this.barButtonItem9,
//            this.btnWordPrintTest,
//            this.btnExecuteVertica,
//            this.btnTestAllReports,
//            this.btnTest,
//            this.btnLoadLogParams,
//            this.btnCompareReports2,
//            this.ceShowInvisibleReports,
//            this.barButtonItem10,
//            this.btnReloadNavigator,
//            this.btnTFSAutoCheckIn,
//            this.btnGroupEditor,
//            this.barButtonItem12,
//            this.btnSaveAllData,
//            this.btnRefreshData,
//            this.barEditItem2,
//            this.barMsbiButton,
//            this.btnParametersXML,
//            this.btnGenerateSQL,
//            this.btnRegExtact,
//            this.btnGenerateLKK,
//            this.btnASUTPPackage,
//            this.btnASUTPDoc,
//            this.btnASUTPPackageT,
//            this.ceDevMode,
//            this.btnDevPrepareData,
//            this.btnDevPrint
//            });
//            this.ribbon.Location = new System.Drawing.Point(0, 0);
//            this.ribbon.MaxItemId = 151;
//            this.ribbon.Name = "ribbon";
//            this.ribbon.OptionsCustomizationForm.FormIcon = ((System.Drawing.Icon)(resources.GetObject("resource.FormIcon")));
//            this.ribbon.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
//            this.rpReports,
//            this.rpSettings});
//            this.ribbon.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
//            this.rcbDevexpressSkins,
//            this.rcbLayouts,
//            this.rteWorkFolderPath,
//            this.rbtnWorkFolderPath,
//            this.repositoryItemTextEdit1,
//            this.rbiUpdateTableScheme,
//            this.rpcReportParams,
//            this.rceOldCompile,
//            this.rceStoreDefaultParams,
//            this.rceUseRepository,
//            this.rceShowInvisibleReports,
//            this.repositoryItemCheckEdit1,
//            this.rceDevMode,
//            });
//            this.ribbon.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
//            this.ribbon.ShowQatLocationSelector = false;
//            this.ribbon.ShowToolbarCustomizeItem = false;
//            this.ribbon.Size = new System.Drawing.Size(1976, 116);
//            this.ribbon.Toolbar.ShowCustomizeItem = false;
//            this.ribbon.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;
            // 
            // btnCreateTemplate
            // 
//            this.btnCreateTemplate.Caption = "Создать шаблон";
//            this.btnCreateTemplate.Enabled = false;
//            this.btnCreateTemplate.GroupIndex = 2;
//            this.btnCreateTemplate.Hint = "Создать шаблон настроек отчёта. К настройкам отчёта относятся заданые параметры, " +
//    "настройки колонок и вид таблицы.";
//            this.btnCreateTemplate.Id = 11;
//            this.btnCreateTemplate.Name = "btnCreateTemplate";
//            this.btnCreateTemplate.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnCreateSetting_ItemClick);
            // 
            // btnSaveTemplate
            // 
//            this.btnSaveTemplate.Caption = "Сохранить шаблон";
//            this.btnSaveTemplate.Enabled = false;
//            this.btnSaveTemplate.GroupIndex = 2;
//            this.btnSaveTemplate.Hint = "Сохранить изменения в шаблоне настроек отчёта";
//            this.btnSaveTemplate.Id = 12;
//            this.btnSaveTemplate.Name = "btnSaveTemplate";
//            this.btnSaveTemplate.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnSaveCurrentSetting_ItemClick);
            // 
            // btnDeleteTemplate
            // 
//            this.btnDeleteTemplate.Caption = "Удалить шаблон";
//            this.btnDeleteTemplate.Enabled = false;
//            this.btnDeleteTemplate.GroupIndex = 2;
//            this.btnDeleteTemplate.Hint = "Удалить шаблон настроек отчёта";
//            this.btnDeleteTemplate.Id = 13;
//            this.btnDeleteTemplate.Name = "btnDeleteTemplate";
//            this.btnDeleteTemplate.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnDeleteCurrentSetting_ItemClick);
            // 
            // btnExecuteReport
            // 
//            this.btnExecuteReport.Caption = "Сформировать отчёт";
//            this.btnExecuteReport.Enabled = false;
//            this.btnExecuteReport.Hint = "Заполнить выбранный отчёт данными, с учётом заданых параметров ";
//            this.btnExecuteReport.Id = 17;
//            this.btnExecuteReport.Name = "btnExecuteReport";
//            this.btnExecuteReport.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnExecuteReport_ItemClick);
            // 
            // btnExportToXls
            // 
//            this.btnExportToXls.Caption = "Экспорт в Excel";
//            this.btnExportToXls.Enabled = false;
//            this.btnExportToXls.Hint = "Экспортировать данные текущего отчёта в файл Excel";
//            this.btnExportToXls.Id = 18;
//            this.btnExportToXls.Name = "btnExportToXls";
//            this.btnExportToXls.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnExportToXls_ItemClick);
            // 
            // dbtnPrintForm
            // 
//            this.dbtnPrintForm.Caption = "Печатная форма";
//            this.dbtnPrintForm.Enabled = false;
//            this.dbtnPrintForm.Hint = "Выгрузить данные отчёта в заранее предопределенной форме ";
//            this.dbtnPrintForm.Id = 19;
//            this.dbtnPrintForm.Name = "dbtnPrintForm";
//            this.dbtnPrintForm.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.Caption;
//            this.dbtnPrintForm.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnPrintForm_OnClick);
            // 
            // cbDevexpressSkins
            // 
//            this.cbDevexpressSkins.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
//            this.cbDevexpressSkins.Caption = "Стиль                  ";
//            this.cbDevexpressSkins.Edit = this.rcbDevexpressSkins;
//            this.cbDevexpressSkins.EditWidth = 140;
//            this.cbDevexpressSkins.Id = 3;
//            this.cbDevexpressSkins.Name = "cbDevexpressSkins";
//            this.cbDevexpressSkins.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.Caption;
            // 
            // rcbDevexpressSkins
            // 
//            this.rcbDevexpressSkins.AutoHeight = false;
//            this.rcbDevexpressSkins.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
//            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
//            this.rcbDevexpressSkins.Name = "rcbDevexpressSkins";
//            this.rcbDevexpressSkins.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // cbLayouts
            // 
//            this.cbLayouts.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
//            this.cbLayouts.Caption = "Вид интерфейса ";
//            this.cbLayouts.CategoryGuid = new System.Guid("6ffddb2b-9015-4d97-a4c1-91613e0ef537");
//            this.cbLayouts.Edit = this.rcbLayouts;
//            this.cbLayouts.EditWidth = 140;
//            this.cbLayouts.Id = 7;
//            this.cbLayouts.Name = "cbLayouts";
//            this.cbLayouts.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            // 
            // rcbLayouts
            // 
//            this.rcbLayouts.AutoHeight = false;
//            this.rcbLayouts.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
//            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
//            this.rcbLayouts.Name = "rcbLayouts";
//            this.rcbLayouts.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // btnWorkFolderPath
            // 
//            this.btnWorkFolderPath.AllowHtmlText = DevExpress.Utils.DefaultBoolean.False;
//            this.btnWorkFolderPath.Edit = this.rbtnWorkFolderPath;
//            this.btnWorkFolderPath.EditValue = "";
//            this.btnWorkFolderPath.EditWidth = 290;
//            this.btnWorkFolderPath.Id = 11;
//            this.btnWorkFolderPath.Name = "btnWorkFolderPath";
            // 
            // rbtnWorkFolderPath
            // 
//            this.rbtnWorkFolderPath.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
//            this.rbtnWorkFolderPath.Appearance.Options.UseFont = true;
//            this.rbtnWorkFolderPath.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
//            new DevExpress.XtraEditors.Controls.EditorButton()});
//            this.rbtnWorkFolderPath.Name = "rbtnWorkFolderPath";
//            this.rbtnWorkFolderPath.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
//            this.rbtnWorkFolderPath.ButtonPressed += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.rbtnWorkFolderPath_ButtonPressed);
//            this.rbtnWorkFolderPath.CustomDisplayText += new DevExpress.XtraEditors.Controls.CustomDisplayTextEventHandler(this.rbtnWorkFolderPath_CustomDisplayText);
//            this.rbtnWorkFolderPath.Click += new System.EventHandler(this.rbtnWorkFolderPath_Click);
            // 
            // barButtonItem1
            // 
//            this.barButtonItem1.Caption = "МегаТест";
//            this.barButtonItem1.Id = 13;
//            this.barButtonItem1.Name = "barButtonItem1";
//            this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick);
            // 
            // btnOpenTestQuery
            // 
//            this.btnOpenTestQuery.Caption = "Открыть TestQuery";
//            this.btnOpenTestQuery.Id = 14;
//            this.btnOpenTestQuery.Name = "btnOpenTestQuery";
//            this.btnOpenTestQuery.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnOpenTestQuery_ItemClick);
            // 
            // btnCCBSQL
            // 
//            this.btnCCBSQL.Caption = "CCB SQL";
//            this.btnCCBSQL.Id = 15;
//            this.btnCCBSQL.Name = "btnCCBSQL";
//            this.btnCCBSQL.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnCCBSQL_ItemClick);
            // 
            // btnSaveReportToFile
            // 
//            this.btnSaveReportToFile.Caption = "Сохранить отчёт в файл";
//            this.btnSaveReportToFile.Id = 20;
//            this.btnSaveReportToFile.Name = "btnSaveReportToFile";
//            this.btnSaveReportToFile.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnSaveReportToFile_ItemClick);
            // 
            // btnCompareReports
            // 
//            this.btnCompareReports.Caption = "Сравнить отчёты";
//            this.btnCompareReports.Id = 21;
//            this.btnCompareReports.Name = "btnCompareReports";
//            this.btnCompareReports.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnCompareReports_ItemClick);
            // 
            // btnLoadReportFromFile
            // 
//            this.btnLoadReportFromFile.Caption = "Загрузить отчёт из файла";
//            this.btnLoadReportFromFile.Id = 24;
//            this.btnLoadReportFromFile.Name = "btnLoadReportFromFile";
//            this.btnLoadReportFromFile.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnLoadReportFromFile_ItemClick);
            // 
            // btnReportParams
            // 
//            this.btnReportParams.Caption = "Выбор параметров";
//            this.btnReportParams.Enabled = false;
//            this.btnReportParams.Hint = "Выбор параметров, которые будут отбражаться на панели с параметрами отчёта";
//            this.btnReportParams.Id = 26;
//            this.btnReportParams.Name = "btnReportParams";
//            this.btnReportParams.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnReportParams_ItemClick);
            // 
            // btnColsInfoForExcel
            // 
//            this.btnColsInfoForExcel.Caption = "Колонки для Excel";
//            this.btnColsInfoForExcel.Id = 30;
//            this.btnColsInfoForExcel.Name = "btnColsInfoForExcel";
//            this.btnColsInfoForExcel.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnColsInfoForExcel_ItemClick);
            // 
            // btnShowXMLSchema
            // 
//            this.btnShowXMLSchema.Caption = "Посмотреть XML схему";
//            this.btnShowXMLSchema.Id = 34;
//            this.btnShowXMLSchema.Name = "btnShowXMLSchema";
//            this.btnShowXMLSchema.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnShowXMLSchema_ItemClick);
            // 
            // btnClearRegistry
            // 
//            this.btnClearRegistry.Caption = "Очистить реестр";
//            this.btnClearRegistry.Id = 44;
//            this.btnClearRegistry.Name = "btnClearRegistry";
//            this.btnClearRegistry.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnClearRegistry_ItemClick);
            // 
            // btnChangeGridMode
            // 
//            this.btnChangeGridMode.Caption = "Показать/спрятать грид";
//            this.btnChangeGridMode.Id = 45;
//            this.btnChangeGridMode.Name = "btnChangeGridMode";
//            this.btnChangeGridMode.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnChangeGridMode_ItemClick);
            // 
            // barCheckItemDebug
            // 
//            this.barCheckItemDebug.BindableChecked = true;
//            this.barCheckItemDebug.Caption = "Отладка";
//            this.barCheckItemDebug.CheckBoxVisibility = DevExpress.XtraBars.CheckBoxVisibility.AfterText;
//            this.barCheckItemDebug.Checked = true;
//            this.barCheckItemDebug.Id = 50;
//            this.barCheckItemDebug.Name = "barCheckItemDebug";
            // 
            // btnReloadXml
            // 
//            this.btnReloadXml.Caption = "Перезагрузить схему";
//            this.btnReloadXml.Id = 51;
//            this.btnReloadXml.ItemShortcut = new DevExpress.XtraBars.BarShortcut(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
//                | System.Windows.Forms.Keys.R));
//            this.btnReloadXml.Name = "btnReloadXml";
//            this.btnReloadXml.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnReloadXml_ItemClick);
            // 
            // btnOpenStartupPath
            // 
//            this.btnOpenStartupPath.Caption = "Открыть стартовую директорию";
//            this.btnOpenStartupPath.Id = 52;
//            this.btnOpenStartupPath.Name = "btnOpenStartupPath";
//            this.btnOpenStartupPath.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnOpenStartupPath_ItemClick);
            // 
            // btnTestOpenedReports
            // 
//            this.btnTestOpenedReports.Caption = "Тестирование открытых отчётов";
//            this.btnTestOpenedReports.Id = 53;
//            this.btnTestOpenedReports.Name = "btnTestOpenedReports";
//            this.btnTestOpenedReports.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
//            this.btnTestOpenedReports.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnTestOpenedReports_ItemClick);
            // 
            // barStaticItem2
            // 
//            this.barStaticItem2.Id = 63;
//            this.barStaticItem2.Name = "barStaticItem2";
            // 
            // barCheckItem1
            // 
//            this.barCheckItem1.Caption = "barCheckItem1";
//            this.barCheckItem1.Id = 64;
//            this.barCheckItem1.Name = "barCheckItem1";
            // 
            // ceStoreDefaultParams
            // 
//            this.ceStoreDefaultParams.Caption = "Запомнить параметры";
//            this.ceStoreDefaultParams.Edit = this.rceStoreDefaultParams;
//            this.ceStoreDefaultParams.Enabled = false;
//            this.ceStoreDefaultParams.Hint = "Сохранять параметры отчета и загружать их при последующем открытии отчета";
//            this.ceStoreDefaultParams.Id = 71;
//            this.ceStoreDefaultParams.Name = "ceStoreDefaultParams";
            // 
            // rceStoreDefaultParams
            // 
//            this.rceStoreDefaultParams.Appearance.BackColor = System.Drawing.Color.Transparent;
//            this.rceStoreDefaultParams.Appearance.Options.UseBackColor = true;
//            this.rceStoreDefaultParams.AutoHeight = false;
//            this.rceStoreDefaultParams.Caption = "";
//            this.rceStoreDefaultParams.Name = "rceStoreDefaultParams";
//            this.rceStoreDefaultParams.EditValueChanged += new System.EventHandler(this.rceStoreDefaultParams_EditValueChanged);
            // 
            // btnExpressReport
            // 
//            this.btnExpressReport.Caption = "Открыть ExpressForm";
//            this.btnExpressReport.Id = 79;
//            this.btnExpressReport.Name = "btnExpressReport";
//            this.btnExpressReport.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnExpressReport_ItemClick);
            // 
            // barButtonItem3
            // 
//            this.barButtonItem3.Caption = "XViewColumns";
//            this.barButtonItem3.Id = 81;
//            this.barButtonItem3.Name = "barButtonItem3";
//            this.barButtonItem3.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem3_ItemClick);
            // 
            // ceUseRepository
            // 
//            this.ceUseRepository.Caption = "Использовать хранилище";
//            this.ceUseRepository.Edit = this.rceUseRepository;
//            this.ceUseRepository.Id = 83;
//            this.ceUseRepository.Name = "ceUseRepository";
            // 
            // rceUseRepository
            // 
//            this.rceUseRepository.Appearance.BackColor = System.Drawing.Color.Transparent;
//            this.rceUseRepository.Appearance.Options.UseBackColor = true;
//            this.rceUseRepository.AutoHeight = false;
//            this.rceUseRepository.Caption = "";
//            this.rceUseRepository.Name = "rceUseRepository";
//            this.rceUseRepository.ValueChecked = 1;
//            this.rceUseRepository.ValueUnchecked = 0;
//            this.rceUseRepository.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.rceUseRepository_EditValueChanging);
            // 
            // barButtonItem9
            // 
//            this.barButtonItem9.Caption = "Наш экспорт в Excel";
//            this.barButtonItem9.Id = 84;
//            this.barButtonItem9.Name = "barButtonItem9";
//            this.barButtonItem9.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
//            this.barButtonItem9.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem9_ItemClick);
            // 
            // btnWordPrintTest
            // 
//            this.btnWordPrintTest.Caption = "Печать Word";
//            this.btnWordPrintTest.Id = 89;
//            this.btnWordPrintTest.Name = "btnWordPrintTest";
//            this.btnWordPrintTest.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
//            this.btnWordPrintTest.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnWordPrintTest_ItemClick);
            // 
            // btnExecuteVertica
            // 
//            this.btnExecuteVertica.Caption = "Сформировать Vertica";
//            this.btnExecuteVertica.Id = 96;
//            this.btnExecuteVertica.Name = "btnExecuteVertica";
//            this.btnExecuteVertica.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
//            this.btnExecuteVertica.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnExecuteVertica_ItemClick);
            // 
            // btnTestAllReports
            // 
//            this.btnTestAllReports.Caption = "Тестирование всех отчётов";
//            this.btnTestAllReports.Id = 101;
//            this.btnTestAllReports.Name = "btnTestAllReports";
//            this.btnTestAllReports.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
//            this.btnTestAllReports.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnTestAllReports_ItemClick);
            // 
            // btnTest
            // 
//            this.btnTest.Caption = "Тест размазывания колонок";
//            this.btnTest.Id = 108;
//            this.btnTest.Name = "btnTest";
//            this.btnTest.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnTest_ItemClick);
            // 
            // btnLoadLogParams
            // 
//            this.btnLoadLogParams.Caption = "Загрузить параметры из лога";
//            this.btnLoadLogParams.Id = 117;
//            this.btnLoadLogParams.Name = "btnLoadLogParams";
//            this.btnLoadLogParams.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnLoadLogParams_ItemClick);
            // 
            // btnCompareReports2
            // 
//            this.btnCompareReports2.Caption = "Сравнить отчёт с другой версией";
//            this.btnCompareReports2.Id = 120;
//            this.btnCompareReports2.Name = "btnCompareReports2";
//            this.btnCompareReports2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnCompareReports2_ItemClick);
            // 
            // ceShowInvisibleReports
            // 
//            this.ceShowInvisibleReports.Caption = "Скрытые отчёты";
//            this.ceShowInvisibleReports.Edit = this.rceShowInvisibleReports;
//            this.ceShowInvisibleReports.Id = 121;
//            this.ceShowInvisibleReports.Name = "ceShowInvisibleReports";
            // 
            // rceShowInvisibleReports
            // 
//            this.rceShowInvisibleReports.Appearance.BackColor = System.Drawing.Color.Transparent;
//            this.rceShowInvisibleReports.Appearance.Options.UseBackColor = true;
//            this.rceShowInvisibleReports.AppearanceFocused.BackColor = System.Drawing.Color.Transparent;
//            this.rceShowInvisibleReports.AppearanceFocused.Options.UseBackColor = true;
//            this.rceShowInvisibleReports.AutoHeight = false;
//            this.rceShowInvisibleReports.Name = "rceShowInvisibleReports";
//            this.rceShowInvisibleReports.CheckedChanged += new System.EventHandler(this.rceShowInvisibleReports_CheckedChanged);
            // 
            // barButtonItem10
            // 
//            this.barButtonItem10.Caption = "barButtonItem10";
//            this.barButtonItem10.Id = 122;
//            this.barButtonItem10.Name = "barButtonItem10";
            // 
            // btnReloadNavigator
            // 
//            this.btnReloadNavigator.Caption = "Перезагрузить список";
//            this.btnReloadNavigator.Id = 125;
//            this.btnReloadNavigator.Name = "btnReloadNavigator";
//            this.btnReloadNavigator.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnReloadNavigator_ItemClick);
            // 
            // btnTFSAutoCheckIn
            // 
//            this.btnTFSAutoCheckIn.Caption = "TFS Auto CheckIn";
//            this.btnTFSAutoCheckIn.Id = 130;
//            this.btnTFSAutoCheckIn.Name = "btnTFSAutoCheckIn";
//            this.btnTFSAutoCheckIn.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnTFSAutoCheckIn_ItemClick);
            // 
            // btnGroupEditor
            // 
//            this.btnGroupEditor.Caption = "Группировки";
//            this.btnGroupEditor.Id = 131;
//            this.btnGroupEditor.Name = "btnGroupEditor";
//            this.btnGroupEditor.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
//            this.btnGroupEditor.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnGroupEditor_ItemClick);
            // 
            // barButtonItem12
            // 
//            this.barButtonItem12.Caption = "barButtonItem12";
//            this.barButtonItem12.Id = 132;
//            this.barButtonItem12.Name = "barButtonItem12";
            // 
            // btnSaveAllData
            // 
//            this.btnSaveAllData.Caption = "Сохранить данные";
//            this.btnSaveAllData.Id = 133;
//            this.btnSaveAllData.Name = "btnSaveAllData";
            // 
            // btnRefreshData
            // 
//            this.btnRefreshData.Caption = "Обновить данные";
//            this.btnRefreshData.Id = 134;
//            this.btnRefreshData.Name = "btnRefreshData";
            // 
            // barEditItem2
            // 
//            this.barEditItem2.Caption = "Разрешить несколько окон";
//            this.barEditItem2.Edit = this.repositoryItemCheckEdit1;
//            this.barEditItem2.EditValue = false;
//            this.barEditItem2.Hint = "Возможность открытия нескольких одинаковых окон";
//            this.barEditItem2.Id = 135;
//            this.barEditItem2.Name = "barEditItem2";
            // 
            // repositoryItemCheckEdit1
            // 
//            this.repositoryItemCheckEdit1.Appearance.BackColor = System.Drawing.Color.Transparent;
//            this.repositoryItemCheckEdit1.Appearance.Options.UseBackColor = true;
//            this.repositoryItemCheckEdit1.AutoHeight = false;
//            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            // 
            // barMsbiButton
            // 
//            this.barMsbiButton.Caption = "Данные для АСФО";
//            this.barMsbiButton.Id = 141;
//            this.barMsbiButton.Name = "barMsbiButton";
//            this.barMsbiButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnShowMsbiMenu_ItemClick);
            // 
            // btnParametersXML
            // 
//            this.btnParametersXML.Caption = "ParsXml";
//            this.btnParametersXML.Id = 142;
//            this.btnParametersXML.Name = "btnParametersXML";
//            this.btnParametersXML.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnParametersXML_ItemClick);
            // 
            // btnGenerateSQL
            // 
//            this.btnGenerateSQL.Caption = "Получить SQL";
//            this.btnGenerateSQL.Id = 143;
//            this.btnGenerateSQL.Name = "btnGenerateSQL";
//            this.btnGenerateSQL.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnGenerateSQL_ItemClick);
            // 
            // btnRegExtact
            // 
//            this.btnRegExtact.Caption = "Зарегистрировать представление";
//            this.btnRegExtact.Id = 146;
//            this.btnRegExtact.Name = "btnRegExtact";
//            this.btnRegExtact.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnRegExtact_ItemClick);
            // 
            // btnGenerateLKK
            // 
//            this.btnGenerateLKK.Caption = "Генерация ЛКК";
//            this.btnGenerateLKK.Id = 147;
//            this.btnGenerateLKK.Name = "btnGenerateLKK";
//            this.btnGenerateLKK.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnGenerateLKK_ItemClick);
            // 
            // btnASUTPPackage
            // 
//            this.btnASUTPPackage.Caption = "АСУТП Пакеты";
//            this.btnASUTPPackage.Id = 148;
//            this.btnASUTPPackage.Name = "btnASUTPPackage";
//            this.btnASUTPPackage.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnASUTPPackage_ItemClick);
            // 
            // btnASUTPDoc
            // 
//            this.btnASUTPDoc.Caption = "АСУТП описание";
//            this.btnASUTPDoc.Id = 149;
//            this.btnASUTPDoc.Name = "btnASUTPDoc";
//            this.btnASUTPDoc.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnASUTPDoc_ItemClick);
            // 
            // btnASUTPPackageT
            // 
//            this.btnASUTPPackageT.Caption = "АСУТП Пакеты (Т)";
//            this.btnASUTPPackageT.Id = 150;
//            this.btnASUTPPackageT.Name = "btnASUTPPackageT";
//            this.btnASUTPPackageT.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnASUTPPackageT_ItemClick);
            // 
            // rpReports
            // 
//            this.rpReports.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
//            this.rpgReport,
//            this.rpgReportTemplates,
//            this.rpgReportParams,
//            this.rpgWorkDirectory,
//            this.rpgDataEditor,
//            this.rpgHelp,
//            this.rpgRepository,
//            this.rpgDebug,
//            this.rpgTrash});
//            this.rpReports.Name = "rpReports";
//            this.rpReports.Text = "Работа с отчётами";
            // 
            // rpgReport
            // 
//            this.rpgReport.AllowTextClipping = false;
//            this.rpgReport.ItemLinks.Add(this.btnExecuteReport);
//            this.rpgReport.ItemLinks.Add(this.btnExportToXls);
//            this.rpgReport.ItemLinks.Add(this.dbtnPrintForm);
//            this.rpgReport.Name = "rpgReport";
//            this.rpgReport.ShowCaptionButton = false;
//            this.rpgReport.Text = "Формирование и экспорт";
            // 
            // rpgReportTemplates
            // 
//            this.rpgReportTemplates.AllowTextClipping = false;
//            this.rpgReportTemplates.ItemLinks.Add(this.btnCreateTemplate);
//            this.rpgReportTemplates.ItemLinks.Add(this.btnSaveTemplate);
//            this.rpgReportTemplates.ItemLinks.Add(this.btnDeleteTemplate);
//            this.rpgReportTemplates.Name = "rpgReportTemplates";
//            this.rpgReportTemplates.ShowCaptionButton = false;
//            this.rpgReportTemplates.Text = "Сохранение настроек отчёта";
            // 
            // rpgReportParams
            // 
//            this.rpgReportParams.ItemLinks.Add(this.btnReportParams);
//            this.rpgReportParams.ItemLinks.Add(this.btnColumnsEditor);
//            this.rpgReportParams.ItemLinks.Add(this.ceStoreDefaultParams);
//            this.rpgReportParams.ItemLinks.Add(this.btnGroupEditor);
//            this.rpgReportParams.Name = "rpgReportParams";
//            this.rpgReportParams.ShowCaptionButton = false;
//            this.rpgReportParams.Text = "Параметры отчёта";
            // 
            // rpgWorkDirectory
            // 
//            this.rpgWorkDirectory.ItemLinks.Add(this.btnWorkFolderPath);
//            this.rpgWorkDirectory.Name = "rpgWorkDirectory";
//            this.rpgWorkDirectory.ShowCaptionButton = false;
//            this.rpgWorkDirectory.Text = "Рабочая папка";
            // 
            // rpgDataEditor
            // 
//            this.rpgDataEditor.ItemLinks.Add(this.btnSaveAllData);
//            this.rpgDataEditor.ItemLinks.Add(this.btnRefreshData);
//            this.rpgDataEditor.ItemLinks.Add(this.barEditItem2);
//            this.rpgDataEditor.Name = "rpgDataEditor";
//            this.rpgDataEditor.Text = "Редактор данных";
//            this.rpgDataEditor.Visible = false;
            // 
            // rpgHelp
            // 
//            this.rpgHelp.Name = "rpgHelp";
//            this.rpgHelp.Text = "Справка";
//            this.rpgHelp.Visible = false;
            // 
            // rpgRepository
            // 
//            this.rpgRepository.ItemLinks.Add(this.ceUseRepository);
//            this.rpgRepository.Name = "rpgRepository";
//            this.rpgRepository.Text = "Хранилище данных";
//            this.rpgRepository.Visible = false;
            // 
            // rpgDebug
            // 
//            this.rpgDebug.ItemLinks.Add(this.btnReloadXml);
//            this.rpgDebug.ItemLinks.Add(this.btnReloadNavigator);
//            this.rpgDebug.ItemLinks.Add(this.btnLoadLogParams);
//            this.rpgDebug.ItemLinks.Add(this.btnOpenTestQuery);
//            this.rpgDebug.ItemLinks.Add(this.btnColsInfoForExcel);
//            this.rpgDebug.ItemLinks.Add(this.btnShowXMLSchema);
//            this.rpgDebug.ItemLinks.Add(this.btnClearRegistry);
//            this.rpgDebug.ItemLinks.Add(this.btnChangeGridMode);
//            this.rpgDebug.ItemLinks.Add(this.btnOpenStartupPath);
//            this.rpgDebug.ItemLinks.Add(this.btnExpressReport);
//            this.rpgDebug.ItemLinks.Add(this.btnTest);
//            this.rpgDebug.ItemLinks.Add(this.ceShowInvisibleReports);
//            this.rpgDebug.ItemLinks.Add(this.btnCCBSQL);
//            this.rpgDebug.ItemLinks.Add(this.btnTFSAutoCheckIn);
//            this.rpgDebug.ItemLinks.Add(this.barMsbiButton);
//            this.rpgDebug.ItemLinks.Add(this.btnParametersXML);
//            this.rpgDebug.ItemLinks.Add(this.btnGenerateSQL);
//            this.rpgDebug.ItemLinks.Add(this.btnGenerateLKK);
//            this.rpgDebug.ItemLinks.Add(this.btnASUTPPackage);
//            this.rpgDebug.ItemLinks.Add(this.btnASUTPPackageT);
//            this.rpgDebug.ItemLinks.Add(this.btnASUTPDoc);
//            this.rpgDebug.Name = "rpgDebug";
//            this.rpgDebug.Text = "Отладка";
//            this.rpgDebug.Visible = false;
            // 
            // rpgTrash
            // 
//            this.rpgTrash.ItemLinks.Add(this.barButtonItem1);
//            this.rpgTrash.ItemLinks.Add(this.btnExecuteVertica);
//            this.rpgTrash.ItemLinks.Add(this.barButtonItem9);
//            this.rpgTrash.ItemLinks.Add(this.btnWordPrintTest);
//            this.rpgTrash.ItemLinks.Add(this.barButtonItem3);
//            this.rpgTrash.Name = "rpgTrash";
//            this.rpgTrash.Text = "rpgTrash";
//            this.rpgTrash.Visible = false;
            // 
            // rpSettings
            // 
//            this.rpSettings.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
//            this.rpgVisual,
//            this.rpgTest,
//            this.rpgDashboard,
//            this.rpgWebDev
//            });
//            this.rpSettings.Name = "rpSettings";
//            this.rpSettings.Text = "Дополнительно";
            // 
            // rpgVisual
            // 
//            this.rpgVisual.ItemLinks.Add(this.cbDevexpressSkins);
//            this.rpgVisual.ItemLinks.Add(this.cbLayouts);
//            this.rpgVisual.Name = "rpgVisual";
//            this.rpgVisual.ShowCaptionButton = false;
//            this.rpgVisual.Text = "Визуальные настройки";
            // 
            // rpgTest
            // 
//            this.rpgTest.ItemLinks.Add(this.btnSaveReportToFile);
//            this.rpgTest.ItemLinks.Add(this.btnLoadReportFromFile);
//            this.rpgTest.ItemLinks.Add(this.btnCompareReports);
//            this.rpgTest.ItemLinks.Add(this.btnTestOpenedReports);
//            this.rpgTest.ItemLinks.Add(this.btnTestAllReports);
//            this.rpgTest.ItemLinks.Add(this.btnCompareReports2);
//            this.rpgTest.Name = "rpgTest";
//            this.rpgTest.Text = "Тестирование(beta)";
            // 
            // rpgDashboard
            // 
//            this.rpgDashboard.ItemLinks.Add(this.btnRegExtact);
//            this.rpgDashboard.Name = "rpgDashboard";
//            this.rpgDashboard.Text = "Аналитические представления";
            // 
            // rteWorkFolderPath
            // 
//            this.rteWorkFolderPath.AutoHeight = false;
//            this.rteWorkFolderPath.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
//            this.rteWorkFolderPath.Name = "rteWorkFolderPath";
            // 
            // repositoryItemTextEdit1
            // 
//            this.repositoryItemTextEdit1.AutoHeight = false;
//            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // rbiUpdateTableScheme
            // 
//            this.rbiUpdateTableScheme.Name = "rbiUpdateTableScheme";
            // 
            // rpcReportParams
            // 
//            this.rpcReportParams.AutoHeight = false;
//            this.rpcReportParams.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
//            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
//            this.rpcReportParams.Name = "rpcReportParams";
            // 
            // rceOldCompile
            // 
//            this.rceOldCompile.Appearance.BackColor = System.Drawing.Color.Transparent;
//            this.rceOldCompile.Appearance.Options.UseBackColor = true;
//            this.rceOldCompile.AutoHeight = false;
//            this.rceOldCompile.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.Buffered;
//            this.rceOldCompile.Name = "rceOldCompile";
            // 
            // dockManager
            // 
//            this.dockManager.DockingOptions.ShowCloseButton = false;
//            this.dockManager.Form = this;
//            this.dockManager.TopZIndexControls.AddRange(new string[] {
//            "DevExpress.XtraBars.BarDockControl",
//            "DevExpress.XtraBars.StandaloneBarDockControl",
//            "System.Windows.Forms.StatusBar",
//            "System.Windows.Forms.MenuStrip",
//            "System.Windows.Forms.StatusStrip",
//            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
//            "DevExpress.XtraBars.Ribbon.RibbonControl"});
            // 
            // dmReports
            // 
//            this.dmReports.ContainerControl = this;
//            this.dmReports.Images = this.ic;
//            this.dmReports.View = this.dmView;
//            this.dmReports.ViewCollection.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseView[] {
//            this.dmView});
//            this.dmReports.DocumentActivate += new DevExpress.XtraBars.Docking2010.Views.DocumentEventHandler(this.dmReports_DocumentActivate);
            // 
            // ic
            // 
//            this.ic.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("ic.ImageStream")));
//            this.ic.InsertGalleryImage("open_16x16.png", "images/actions/open_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/open_16x16.png"), 0);
//            this.ic.Images.SetKeyName(0, "open_16x16.png");
//            this.ic.InsertGalleryImage("report_16x16.png", "images/reports/report_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/reports/report_16x16.png"), 1);
//            this.ic.Images.SetKeyName(1, "report_16x16.png");
//            this.ic.InsertGalleryImage("edittask_16x16.png", "images/tasks/edittask_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/tasks/edittask_16x16.png"), 2);
//            this.ic.Images.SetKeyName(2, "edittask_16x16.png");
//            this.ic.InsertGalleryImage("template_16x16.png", "images/support/template_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/support/template_16x16.png"), 3);
//            this.ic.Images.SetKeyName(3, "template_16x16.png");
//            this.ic.InsertGalleryImage("hide_16x16.png", "images/actions/hide_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/hide_16x16.png"), 4);
//            this.ic.Images.SetKeyName(4, "hide_16x16.png");
            // 
            // dmView
            // 
//            this.dmView.DocumentSelectorProperties.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.True;
//            this.dmView.Orientation = System.Windows.Forms.Orientation.Vertical;
//            this.dmView.RootContainer.Orientation = System.Windows.Forms.Orientation.Vertical;
//            this.dmView.UseLoadingIndicator = DevExpress.Utils.DefaultBoolean.False;
//            this.dmView.DocumentAdded += new DevExpress.XtraBars.Docking2010.Views.DocumentEventHandler(this.dmView_DocumentAdded);
//            this.dmView.DocumentClosing += new DevExpress.XtraBars.Docking2010.Views.DocumentCancelEventHandler(this.dmView_DocumentClosing);
            // 
            // ribbonPageGroup1
            // 
//            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
//            this.ribbonPageGroup1.Text = "MainMenu";
            // 
            // barEditItem1
            // 
//            this.barEditItem1.Caption = "Запомнить параметры";
//            this.barEditItem1.Edit = this.rceStoreDefaultParams;
//            this.barEditItem1.Enabled = false;
//            this.barEditItem1.Hint = "Сохранять параметры отчета и загружать их при последующем открытии отчета";
//            this.barEditItem1.Id = 71;
//            this.barEditItem1.Name = "barEditItem1";
            // 
            // ucMainReports
            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.Controls.Add(this.ribbon);
//            this.Name = "ucMainReports";
//            this.Size = new System.Drawing.Size(1976, 497);
            // 
            // rpgWebDev
            // 
//            this.rpgWebDev.Name = "rpgWebDev";
//            this.rpgWebDev.Text = "Инструменты разработчика";
//            this.rpgWebDev.ItemLinks.Add(this.ceDevMode);
//            this.rpgWebDev.ItemLinks.Add(this.btnDevPrepareData);
//            this.rpgWebDev.ItemLinks.Add(this.btnDevPrint);
            // 
            // ceDevMode
            // 
//            this.ceDevMode.Caption = "Режим разработки";
//            this.ceDevMode.Edit = this.rceDevMode;
//            this.ceDevMode.EditValue = false;
//            this.ceDevMode.Id = 72;
//            this.ceDevMode.Name = "ceDevMode";
            // 
            // rceDevMode
            // 
//            this.rceDevMode.Appearance.BackColor = System.Drawing.Color.Transparent;
//            this.rceDevMode.Appearance.Options.UseBackColor = true;
//            this.rceDevMode.AutoHeight = false;
//            this.rceDevMode.Caption = "";
//            this.rceDevMode.Name = "rceDevMode";
//            this.rceDevMode.EditValueChanged += new System.EventHandler(this.rceDevMode_EditValueChanged);
            // 
            // btnDevPrepareData
            // 
//            this.btnDevPrepareData.Caption = "Подготовка данных";
//            this.btnDevPrepareData.Id = 301;
//            this.btnDevPrepareData.Name = "btnDevPrepareData";
//            this.btnDevPrepareData.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnDevPrepareData_ItemClick);
            // 
            // btnDevPrint
            // 
//            this.btnDevPrint.Caption = "Печатная форма";
//            this.btnDevPrint.Id = 302;
//            this.btnDevPrint.Name = "btnDevPrint";
//            this.btnDevPrint.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnDevPrint_ItemClick);

//            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rcbDevexpressSkins)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rcbLayouts)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rbtnWorkFolderPath)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rceStoreDefaultParams)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rceUseRepository)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rceShowInvisibleReports)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rteWorkFolderPath)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rbiUpdateTableScheme)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rpcReportParams)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rceOldCompile)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.dockManager)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.dmReports)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.ic)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.dmView)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rceDevMode)).EndInit();
//            this.ResumeLayout(false);
//            this.PerformLayout();

//        }
//        #endregion

//        private DevExpress.XtraBars.Docking.DockManager dockManager;
//        private DevExpress.XtraBars.Docking2010.DocumentManager dmReports;
//        private DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView dmView;
//        private DevExpress.Utils.ImageCollection ic;
//        private DevExpress.XtraBars.BarButtonItem btnCreateTemplate;
//        private DevExpress.XtraBars.BarButtonItem btnSaveTemplate;
//        private DevExpress.XtraBars.BarButtonItem btnDeleteTemplate;
//        private DevExpress.XtraBars.BarButtonItem btnExecuteReport;
//        private DevExpress.XtraBars.BarButtonItem btnExportToXls;
//        private DevExpress.XtraBars.BarLinkContainerItem dbtnPrintForm;
//        private DevExpress.XtraBars.Ribbon.RibbonControl ribbon;
//        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox rcbDevexpressSkins;
//        private DevExpress.XtraBars.Ribbon.RibbonPage rpReports;
//        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rpgReportTemplates;
//        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rpgReport;
//        private DevExpress.XtraBars.BarEditItem cbDevexpressSkins;
//        private DevExpress.XtraBars.BarEditItem cbLayouts;
//        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox rcbLayouts;
//        private DevExpress.XtraBars.Ribbon.RibbonPage rpSettings;
//        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rpgVisual;
//        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rpgWorkDirectory;
//        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
//        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rpgDebug;
//        private DevExpress.XtraBars.BarEditItem btnWorkFolderPath;
//        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit rbtnWorkFolderPath;
//        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit rteWorkFolderPath;
//        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
//        private DevExpress.XtraBars.BarButtonItem btnOpenTestQuery;
//        private DevExpress.XtraBars.BarButtonItem btnCCBSQL;
//        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit rbiUpdateTableScheme;
//        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
//        private DevExpress.XtraBars.BarButtonItem btnSaveReportToFile;
//        private DevExpress.XtraBars.BarButtonItem btnCompareReports;
//        private DevExpress.XtraBars.BarButtonItem btnLoadReportFromFile;
//        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rpgReportParams;
//        private DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit rpcReportParams;
//        private DevExpress.XtraBars.BarButtonItem btnReportParams;
//        private DevExpress.XtraBars.BarButtonItem btnColsInfoForExcel;
//        private DevExpress.XtraBars.BarButtonItem btnShowXMLSchema;
//        private DevExpress.XtraBars.BarButtonItem btnClearRegistry;
//        private DevExpress.XtraBars.BarButtonItem btnChangeGridMode;
//        private DevExpress.XtraBars.BarButtonItem btnColumnsEditor;
//        private DevExpress.XtraBars.BarCheckItem barCheckItemDebug;
//        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rpgTest;
//        private DevExpress.XtraBars.BarButtonItem btnReloadXml;
//        private DevExpress.XtraBars.BarButtonItem btnOpenStartupPath;
//        private DevExpress.XtraBars.BarButtonItem btnTestOpenedReports;
//        private DevExpress.XtraBars.BarStaticItem barStaticItem2;
//        private DevExpress.XtraBars.BarCheckItem barCheckItem1;
//        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rceOldCompile;

//        private DevExpress.XtraBars.BarEditItem ceStoreDefaultParams;
//        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rceStoreDefaultParams;
//        private DevExpress.XtraBars.BarButtonItem btnExpressReport;
//        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
//        private DevExpress.XtraBars.BarEditItem ceUseRepository;
//        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rceUseRepository;
//        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rpgRepository;
//        private DevExpress.XtraBars.BarEditItem barEditItem1;
//        private DevExpress.XtraBars.BarButtonItem barButtonItem9;
//        private DevExpress.XtraBars.BarButtonItem btnWordPrintTest;
//        private DevExpress.XtraBars.BarButtonItem btnExecuteVertica;
//        private DevExpress.XtraBars.BarButtonItem btnTestAllReports;
//        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rpgHelp;
//        private DevExpress.XtraBars.BarButtonItem btnTest;
//        private DevExpress.XtraBars.BarButtonItem btnLoadLogParams;
//        private DevExpress.XtraBars.BarButtonItem btnCompareReports2;
//        private DevExpress.XtraBars.BarEditItem ceShowInvisibleReports;
//        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rceShowInvisibleReports;
//        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rpgTrash;
//        private DevExpress.XtraBars.BarButtonItem barButtonItem10;
//        private DevExpress.XtraBars.BarButtonItem btnReloadNavigator;
//        private DevExpress.XtraBars.BarButtonItem btnTFSAutoCheckIn;
//        private DevExpress.XtraBars.BarButtonItem btnGroupEditor;
//        private DevExpress.XtraBars.BarButtonItem barButtonItem12;
//        private DevExpress.XtraBars.BarButtonItem btnSaveAllData;
//        private DevExpress.XtraBars.BarButtonItem btnRefreshData;
//        private DevExpress.XtraBars.BarEditItem barEditItem2;
//        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
//        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rpgDataEditor;
//		private DevExpress.XtraBars.BarButtonItem barMsbiButton;
//        private DevExpress.XtraBars.BarButtonItem btnParametersXML;
//		private DevExpress.XtraBars.BarButtonItem btnGenerateSQL;
//        private DevExpress.XtraBars.BarButtonItem btnRegExtact;
//        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rpgDashboard;
//        private DevExpress.XtraBars.BarButtonItem btnGenerateLKK;
//        private DevExpress.XtraBars.BarButtonItem btnASUTPPackage;
//        private DevExpress.XtraBars.BarButtonItem btnASUTPDoc;
//        private DevExpress.XtraBars.BarButtonItem btnASUTPPackageT;
//        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rpgWebDev;
//        private DevExpress.XtraBars.BarEditItem ceDevMode;
//        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rceDevMode;
//        private DevExpress.XtraBars.BarButtonItem btnDevPrepareData;
//        private DevExpress.XtraBars.BarButtonItem btnDevPrint;
//    }
//}
