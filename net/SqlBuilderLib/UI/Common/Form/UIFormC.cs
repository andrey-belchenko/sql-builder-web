using System;
using System.Collections.Generic;
using System.Diagnostics; // Debug, Stopwatch
using Contract = System.Diagnostics.Contracts.Contract;
using System.Drawing;
using System.Data;
using System.Linq;
//using System.Windows.Forms;
using System.Xml.Linq;
//using DevExpress.DashboardCommon.Native;
//using DevExpress.XtraEditors;
//using DevExpress.XtraGrid.Views.Grid;
//using DevExpress.XtraBars;
//using DevExpress.XtraBars.Docking2010.Views;
//using DevExpress.XtraEditors.Controls;
//using infoenergo.core.Extensions;
using sql.builder.Controls;
using sql.builder.DataApi;
using sql.builder.FieldInfo;
using sql.builder.WinForms;
//using infoenergo.ui.win.Base;
using sql.builder.Controls.FormFields;
//using sql.builder.Controls.Grids;
using sql.builder.XmlHelpers;
using sql.builder.Exceptions;
namespace sql.builder.UI
{
    public partial class UIFormC : IForm
    {
        internal enum UseType
        {
            ParamEditor,
            SchemeEditor,
            DataEditor
        }
        #region Переменные и свойства
        private XElement _xform;
        /// <summary>
        /// XML-описание формы
        /// </summary>
        public XElement XForm
        {
            get
            {
                return this._xform;
            }
        }
        /// <summary>
        /// Наименование формы (form@name)
        /// </summary>
        private string _form_name;
        /// <summary>
        /// Возвращает наименование формы (form@name)
        /// </summary>
        public string GetFormName()
        {
            return this._form_name;
        }
        private string _security_id;
        /// <summary>
        /// Необходимые права на форму (form@security-id)
        /// </summary>
        public string SecurityID
        {
            get
            {
                return _security_id;
            }
        }
        public bool WithBehavior
        {
            get
            {
                return this._useType == UseType.DataEditor;
            }
        }
        private SortedList<string, object> _props = null;
        public void SetProp(string name, object value = null)
        {
            if (this._props == null)
            {
                this._props = new SortedList<string, object>();
            }
            this._props[name] = value;
        }
        public void RemoveProp(string name)
        {
            if (this._props != null)
            {
                if (this._props.ContainsKey(name))
                {
                    this._props.Remove(name);
                }
            }
        }
        public object GetPropVal(string name)
        {
            if (this._props == null)
            {
                return null;
            }
            object value;
            if (this._props.TryGetValue(name, out value))
            {
                return value;
            }
            else
            {
                return null;
            }
        }
        public bool HasProp(string name)
        {
            if (this._props == null)
            {
                return false;
            }
            return this._props.ContainsKey(name);
        }
        private bool _auto_refresh;
        public bool AutoRefresh
        {
            get
            {
                return this._auto_refresh;
            }
        }
        internal UIBase LastActiveField { get; set; }
        public bool ClearDataOnClose { get; set; }
        public string GroupName { get; set; }
        public XElement DefaultParams { get; set; }
        public bool HasDefaultParams
        {
            get
            {
                return this.DefaultParams != null;
            }
        }
        public bool IsNew = true;
        private List<string> _title_variables;
        /// <summary>
        /// Подстановочные символы в заголовке окна
        /// </summary>
        public List<string> TitleVariables
        {
            get
            {
                return this._title_variables;
            }
        }
        private string titleOriginal = null;
        /// <summary>
        /// Заголовок окна с подстановочными символами
        /// </summary>
        public string TitleOriginal
        {
            get
            {
                return titleOriginal;
            }
            set
            {
                titleOriginal = value;
                // Емцов поставил проверку, тк вылетало
                if (titleOriginal != null)
                {
                    this._title_variables = Cmn.ExtractParamsFromString(titleOriginal);
                    if (this._title_variables == null)
                    {
                        this._title_variables = new List<string>();
                    }
                }
                this._title = titleOriginal;
            }
        }
        /// <summary>
        /// Заголовок окна (подстановочные символы заменены на их значения)
        /// </summary>
        private string _title;
        /// <summary>
        /// Возвращает заголовок окна (подстановочные символы заменены на их значения)
        /// </summary>
        public string GetTitle()
        {
            return this._title;
        }
        /// <summary>
        /// Обновляет заголовок окна, заменяя подстановочные символы на их значения
        /// </summary>
        public void UpdateTitle()
        {
            this._title = this.ReplaceParameters(this.titleOriginal, this._title_variables);
            setTitle(this._title);
        }
        /// <summary>
        /// Заменяет в строке <paramref name="text"/> подстановочные символы значениями полей формы
        /// </summary>
        /// <param name="text">Строка с подстановочными символами</param>
        /// <returns>Результат замены</returns>
        public string ReplaceParameters(string text)
        {
            return this.ReplaceParameters(text, null);
        }
        private string ReplaceParameters(string text, List<string> titleVars)
        {
            if (titleVars == null)
            {
                titleVars = Cmn.ExtractParamsFromString(text);
                if (titleVars == null)
                {
                    titleVars = new List<string>(0);
                }
            }
            foreach (string varName in titleVars)
            {
                UIBase fld = null;
                if (this._controls.TryGetValue(varName, out fld))
                {
                    text = text.Replace("[:" + varName + "]", fld.GetText());
                }
            }
            return text;
        }
        private readonly Dictionary<string, UIBase> _controls;
        /// <summary>
        /// Набор полей формы
        /// </summary>
        internal Dictionary<string, UIBase> controls
        {
            get
            {
                return this._controls;
            }
            //set;
        }
        //internal List<CommandItemController> commandItems;
        //bool _layout_shown;
        public XElement XParams;
        private VDataSet dataSource;
        internal VDataSet DataSource
        {
            get
            {
                if (this.dataSource == null)
                {
                    dataSource = new VDataSet();
                    dataSource.ParamsTable = new VDataTable(true);
                    dataSource.Tables.Add(dataSource.ParamsTable);
                    dataSource.ParamsTable.Rows.Add();
                    AttachDataSourceEvents();
                    dataSource.Form = this;
                }
                return dataSource;
            }
            set
            {
                if (dataSource != null)
                {
                    dataSource.Form = null;
                    DetachDataSourceEvents();
                    foreach (VDataTable t in dataSource.Tables)
                    {
                        t.DetachEvents();
                    }
                    DetachDataSourceEvents();
                    detachChangeCompletedEvent();
                    detachControlStateEvent();
                }
                dataSource = value;
                if (dataSource != null)
                {
                    dataSource.Form = this;
                    foreach (VDataTable t in dataSource.Tables)
                    {
                        t.AttachEvents();
                    }
                    if (dataSource.ParamsTable == null)
                    {
                        dataSource.ParamsTable = (VDataTable)dataSource.Tables[0];
                    }
                    AttachDataSourceEvents();
                    attachChangeCompletedEvent();
                    if (this._controls != null)
                    {
                        foreach (UIBase control in this._controls.Values)
                        {
                            control.BindData();
                        }
                    }
                }
            }
        }
        private UseType _useType;
        internal UseType FormUseType
        {
            get
            {
                return this._useType;
            }
        }
        /// <summary>
        /// Количество контролов на форме
        /// </summary>
        public int UIControlsCount
        {
            get
            {
                if (this._controls == null)
                {
                    return 0;
                }
                else
                {
                    return this._controls.Count;
                }
            }
        }
        private bool _no_data;
        public bool NoData
        {
            get
            {
                return this._no_data;
            }
        }
        internal VDataTable SelectionTarget = null;
        //private readonly Dictionary<string, ucTableViewerContainer> _grids;
        /// <summary>
        /// Набор гридов на форме. 
        /// Ключом является grid@table
        /// </summary>
        //internal Dictionary<string, ucTableViewerContainer> Grids {
        //    get {
        //        return this._grids;
        //    }
        //}
        private bool FlagRefreshingAll;
        private bool _init;
        /// <summary>
        /// Возвращает true, если форма была проинициализирована
        /// </summary>
        public bool Init
        {
            get
            {
                return _init;
            }
        }
        #endregion
        #region События
        public event EventHandler AnyValueChanged;
        public event Action<UIFormC, string, object> SpecialTypeChanged;
        public event Func<UIFormC, XElement> NeedReportScheme;
        internal event Func<UIFormC, VReport> NeedReport;
        #endregion
        #region Открытые методы
        public UIFormC()
        {
            // InitializeComponent();
            this._equiped = true;
            this.ClearDataOnClose = true;
            // TmpGetControlAsWinFormCtrl().Dock = DockStyle.Fill;
            this._useType = UseType.ParamEditor;
            //this._grids = new Dictionary<string, ucTableViewerContainer>();
            //XGrids = new Dictionary<string, XElement>();
            this._controls = new Dictionary<string, UIBase>();
            //innerSubForms = new Dictionary<string, LayoutControl>();
            //_captions_location = Locations.Default;
            //if (XmlReports.IsDeveloperMode()) tbSearch.Visible = true;
            //this.GetControl().Disposed += this.OnDisposed;
            // TmpGetControlAsWinFormCtrl().Disposed += OnDisposed; // !!!!
            //InitImages();
        }
        private void InitImages()
        {
            //this.GetBarButton(TextConst.AVFormButtonType.Save).SetImage(infoenergo.ui.resources.Properties.Resources.Commit_24);
            //this.GetBarButton(TextConst.AVFormButtonType.Refresh).SetImage(infoenergo.ui.resources.Properties.Resources.Refresh_24);
            //this.GetBarButton(TextConst.AVFormButtonType.Delete).SetImage(infoenergo.ui.resources.Properties.Resources.Delete_24);
            //this.GetBarButton(TextConst.AVFormButtonType.SaveAndClose).SetImage(infoenergo.ui.resources.Properties.Resources.CommitAndClose_24);
            // Button.Glyph = Cmn.GetIcon("Refresh_24");
        }
        private void OnDisposed(object sender, EventArgs args)
        {
            // контролы могут не лежать на форме, но Dispose должен быть вызван
            foreach (UIBase c in this._controls.Values)
            {
                c.Dispose();
            }
            this._controls.Clear();
            //foreach (ucTableViewerContainer grid in this._grids.Values)
            //{
            //    //grid.ModifiedData -= Grid_OnModifiedData;
            //    //grid.RefreshedData -= Grid_OnRefreshedData;
            //    //grid.CommitedData -= Grid_OnCommitedData;
            //    grid.RemoveHasMessageHandler(Grid_OnHasMessage, Grid_OnHasMessage2);
            //    grid.GetControl().VDispose();
            //}
            //this._grids.Clear();
            if (this.dataSource != null)
            {
                this.dataSource.Form = null;
                DetachDataSourceEvents();
                foreach (VDataTable t in this.dataSource.Tables)
                {
                    t.DetachEvents();
                }
                this.DetachDataSourceEvents();
                this.detachChangeCompletedEvent();
                this.detachControlStateEvent();
            }
            if (this.SubForms != null)
            {
                foreach (UIFormC f in this.SubForms)
                {
                    //(f.TmpGetControlAsWinFormCtrl() as Control).Dispose();
                }
            }
            if (innerSubFormsNew != null)
            {
                foreach (var sub in innerSubFormsNew.Values)
                {
                    if (sub.Parent != null) sub.Parent.Dispose();
                    sub.Layout.Dispose();
                }
            }
        }
        internal UIFormC(VDataSet ds, UseType useType, bool noData = false, VDataTable selectionTarget = null)
            : this()
        {
            this.Equip(ds, useType, noData, selectionTarget);
        }
        private bool _equiped;
        public bool Equiped
        {
            get
            {
                return this._equiped;
            }
        }
        internal void Equip(VDataSet ds, UseType useType, bool noData = false, VDataTable selectionTarget = null)
        {
            this._equiped = true;
            this.SelectionTarget = selectionTarget;
            this._no_data = noData;
            this.DataSource = ds;
            this._useType = useType;
        }
        private VLayout _layout;
        public VLayout Layout
        {
            get
            {
                return this._layout;
            }
        }
        public VLayoutGroupInfo MainLayoutGroup = null;
        private IVForm _control;
        private SortedList<string, IVBarButton> specialButtons = new SortedList<string, IVBarButton>();
        private IVBarButton GetBarButton(string buttonType)
        {
            return specialButtons[buttonType];
        }
        private void AddBarButton(string buttonType, string caption)
        {
            //var btn = UIStatic.GetControlsfactory().CreateBarButton();
            //_control.GetToolBar().AddBarButton(btn,false);
            //btn.SetCaption(caption);
            //specialButtons.Add(buttonType, btn);
            //btn.SetVisible(false);
        }
        public UIFormC Form
        {
            get { return this; }
        }
        public bool IsVisibleInLayout()
        {
            return MainLayoutGroup.IsVisible();
        }
        public void Initialize(XElement xform, bool isDialog = false, VLayoutGroupInfo parentLayoutGroup = null)
        {
            Contract.Assert(xform != null);
#if DEBUG
            Stopwatch sw = new Stopwatch();
            sw.Start();
#endif
            VCashUtils.ClearCashNotErrors(); // Нужно чтобы периодически кеш зачищался наверняка есть утечка поставлю зачистку на открытие формы 
            this._init = false;
            this._xform = xform;
            this.SetTitle(xform.AttrOrDefault(AName.title, string.Empty));
            this._form_name = xform.AttrOrDefault(AName.name, string.Empty);
            if (this._form_name.StartsWith("ur_"))
            {
                UIStatic.LoadProject("asuse2");
            }
            this._auto_refresh = xform.AttrOrDefault(AName.auto_refresh, false);
            //WithBehavior = xform.AttrOrDefault(AName.WithBehavior, true);
            string security_id = xform.AttrOrDefault(AName.security_id, null);
            this._security_id = security_id ?? string.Empty;
            if (parentLayoutGroup == null)
            {
                this._layout = new VLayout();
                parentLayoutGroup = this._layout.GetMainGroup();
            }
            else
            {
                this._layout = parentLayoutGroup.GetLayoutController();
            }
            this.MainLayoutGroup = parentLayoutGroup;
            this.UpdateFormToolbar(xform, Command_ItemClick, BarItemControl_EditValueChanged, BarItemRepository_EditValueChanged, this, isDialog);
            XElement xevents = xform.Element(EName.events);
            if (xevents != null)
            {
                this.UpdateEvents(xevents);
            }
            XElement xcontent = xform.Element(EName.content) ?? xform;
            this.LoadContentFromXmlNew(xcontent, parentLayoutGroup, null, null);
            foreach (UIBase control in this._controls.Values)
            {
                // если мастер контрол SimpleRange, учитывыем его параметры один раз
                if (control.UseType == UseType.ParamEditor)
                {
                    this.UpdateControlDependence(control);
                }
            }
            //this.UIControlsCount = this.controls.Count();
            if (this.dataSource != null && this._useType == UseType.SchemeEditor)
            {
                this.dataSource.ParamsTable.RaiseCurrentRowChanged();// Чтобы в редакторе запроса нормально обрабатывалач нулевая строка выделяемая по умолчанию
            }
            this.UpdateButtonsState();
            if (this._layout != null)
            {
                VLayoutGroupInfo layoutMainGroup = this._layout.GetMainGroup();
                //pControls.Parent.Controls.Remove(pControls);
                //layoutControl.Parent.Controls.Remove(layoutControl);
                if (parentLayoutGroup == layoutMainGroup)
                {
                    this.SetLayout(layoutMainGroup);
                }
                //Layout.RefreshLayout();
            }
            if (this.DataSource != null)
            {
                //if (DataSource.Tables.Cast<VDataTable>().Any(t => t.AutoRefresh))
                //{
                //    attachControlStateEvent();
                //}
                this.attachControlStateEvent();
            }
            // this.pControls.Controls.Add(layoutMainCtrl);
            ////////////////////
            //var item_names = layoutControl.Items.OfType<LayoutControlItem>().Select(i => i.Text.Trim()).ToArray();
            //rteSearch.Items.AddRange(item_names);
            this._init = true;
#if DEBUG
            sw.Stop();
            Debug.WriteLine("UIFormC.Initialize(), " + this._form_name + ": " + sw.ElapsedMilliseconds.ToString() + " мс");
#endif
        }
        internal void UpdateControlDependence(UIBase control)
        {
            foreach (string name in control.GetParamsNames())
            {
                if (!name.EndsWith("_filter"))
                {
                    UIBase master = null;
                    this._controls.TryGetValue(name, out master);
                    if (!control.Masters.ContainsKey(name))
                    {
                        control.Masters.Add(name, master);
                    }
                    if (master != null)
                    {
                        if (!master.Dependants.ContainsKey(control.FieldName))
                        {
                            master.Dependants.Add(control.FieldName, control);
                        }
                    }
                }
            }
        }
        public bool IsRefreshig = false;
        public void RefreshData(XElement defaultParams = null)
        {
            this.IsRefreshig = true;
            if (this.NoData) return;
            if (defaultParams == null)
            {
                defaultParams = this.DefaultParams;
            }
            if (this._controls.Values.Any(c => c.SpecialType == "colsets"))
            {
                if (SpecialTypeChanged != null)
                {
                    this.SpecialTypeChanged(this, "colsets", Enumerable.Empty<string>());
                }
            }
            foreach (UIBase c in this._controls.Values)
            {
                if (!c.Mandatory)
                {
                    c.Used = false;
                }
            }
            if (defaultParams != null)
            {
                this.SetReportParams(defaultParams);
            }
            //for (int i = controls.Values.Count - 1; i >= 0; i--)
            //{
            //    controls.Values[i].RefreshData();
            //}
            if (defaultParams != null)
            {
                foreach (UIBase c in this._controls.Values)
                {
                    c.UseDefaultQuery = false;
                }
            }
            foreach (UIBase control in this._controls.Values.Reverse())
            {
                control.RefreshData();
            }
            if (defaultParams != null)
            {
                foreach (UIBase c in this._controls.Values)
                {
                    c.UseDefaultQuery = true;
                }
            }
            foreach (UIBase control in this._controls.Values)
            {
                control.UpdateControlData();
                control.UpdateControlValidation(null);
                control.UpdateControlEditable(null);
            }
            foreach (VDataColumn col in DataSource.ParamsTable.Columns)// Дефолтные значения полей с параметрами по новому варианту
            {
                col.ApplyDefaultValue(this.DataSource.ParamsTable.Rows[0]);
            }
            this.IsRefreshig = false;
        }

        internal void ItemVisibleChanged(UIBase ctrl, bool update_immediately = false)// new done
        {
        }
        internal void SetLayoutItemVisible(UIBase ctrl, bool visible, bool update_immediately = false)
        {
        }

        internal void SetControlOptions(string name, VFieldStateAndOtherInfo options)
        {
            UIBase control = null;
            if (!this._controls.TryGetValue(name, out control))
            {
                return;
            }
            // теперь работает как надо
            this.SetLayoutItemVisible(control, options.VisibleInForm);
            // используется VDataColumn.GetEditable
            control.UpdateControlEditable(null);
            // Если контрол спрятан - сбрасываем выбранные значения
            if (!options.Exists && !control.Mandatory)
            {
                // для колсетсов значение устанавливается всегда
                if (control.SpecialType != "colsets") control.Used = false;
            }
        }
        public void SetDefaultParams(XElement xParams, bool allow_defaults = false)
        {
            if (xParams == null)
            {
                this.DefaultParams = null;
                return;
            }
            if (!allow_defaults)
            {
                var loaded_params_names = new HashSet<string>();
                foreach (XElement field in xParams.Element(EName.content).Descendants(EName.field))
                {
                    loaded_params_names.Add(field.Attribute(AName.name).Value);
                }
                foreach (UIBase ctrl in this._controls.Values)
                {
                    if (loaded_params_names.Contains(ctrl.FieldName))
                    {
                        ctrl.UseDefaultQuery = false;
                    }
                }
            }
            this.DefaultParams = xParams;
        }
        public void SetReportParams(XElement xParams)
        {
            Contract.Assert(xParams != null);
            var xroot = new XElement(EName.root);
            Parser.SaveReportParamsToXml(xroot, this);
            var report = this.UIForm_GetReport();
            bool hide_new_fields = false;
            // для поисковой формы report == null
            if (report != null && report.P_ParamsCustomization == TextConst.AVBool.True)
            {
                hide_new_fields = true;
            }
            else // сюда может прийти рашьше чем присваивается обработчик NeedReport. А обработчик для получения схемы присваивается еще раньше
            {
                XElement sch = UIForm_GetReportScheme();
                if (sch != null)
                {
                    if (sch.AttrOrDefault(AName.params_customization, false))
                    {
                        hide_new_fields = true;
                    }
                }
                else
                {
                    hide_new_fields = true;
                }
            }
            xParams = Parser.RepairParams(xParams, xroot.Element(EName.@params), this._xform, hide_new_fields);
            VDataSet.FromXml(xParams, this.DataSource, clean_ds: false);
            XElement xcontent = xParams.Element(EName.content);
            if (xcontent == null) return;
            foreach (XElement xitem in xcontent.Descendants())
            {
                if (xitem.Name == EName.field)
                {
                    bool visible = xitem.AttrOrDefault(AName.visible, true);
                    var ctrl = this._controls[xitem.Attribute(AName.name).Value];
                    // Емцов - загрузка видимости ломает поведение
                    this.SetLayoutItemVisible(ctrl, visible);
                }
                else if (xitem.Name == EName.fieldgroup && this._useType != UseType.DataEditor)
                {
                    // левый узел
                    if (xitem.Attribute(AName.title).Value == "Параметры отчёта" && EPredicate.IsContentOrForm(xitem.Parent)) continue;
                    bool visible = xitem.AttrOrDefault(AName.visible, true);
                    bool expanded = xitem.AttrOrDefault(AName.expanded, true);
                    string title = xitem.Attribute(AName.title).Value;
                    VLayoutGroupInfo group = Layout.GetAllGroups().First(g => g.GetText() == title);
                    group.SetExpanded(expanded);
                    // Емцов - загрузка видимости ломает поведение
                    if (!this.WithBehavior)
                    {
                        group.SetVisibility(visible);
                    }
                }
            }
            this.ApplyVisibitlity();
            // Емцов - иначе не прогружаются simple параметры в условиях поиска
        }

        internal XElement GetValue(Dictionary<string, UIBase> namedControls = null)
        {
            if (this.dataSource == null) return null;
            List<UIBase> controls;
            ICollection<string> names = new HashSet<string>();
            if (namedControls == null)
            {
                controls = new List<UIBase>(this._controls.Count);
                foreach (UIBase c in this._controls.Values)
                {
                    controls.Add(c);
                    if (c.Used)
                    {
                        names.Add(c.FieldName);
                    }
                }
            }
            else
            {
                controls = new List<UIBase>(namedControls.Count);
                foreach (KeyValuePair<string, UIBase> pair in namedControls)
                {
                    if (pair.Value != null)
                    {
                        controls.Add(pair.Value);
                        names.Add(pair.Key);
                    }
                }
            }
            List<XElement> returns = new List<XElement>();
            if (this._useType == UseType.ParamEditor)
            {
                for (int index = 0; index < controls.Count; index++)
                {
                    UIBase control = controls[index];
                    this.DataSource.SetParamInfo(control.FieldName, control.GetText());
                    if (control.XField.Element("return") != null)
                    {
                        returns.Add(control.XField);
                    }
                }
            }
            XElement pars = this.dataSource.GetParamsAsXml(names, this._xform.AttrOrEmpty(AName.name), false);
            // ставим атрибут, сохранять значения в бд
            //IEnumerable<string> inDBNames = controls.Where(c => c.StoreInDB).Select(c => c.FieldName);
            //foreach (string inDBName in inDBNames)
            //{
            //    var par = pars.Elements(TextConst.EName.Param).FirstOrDefault(p => p.Attribute(TextConst.AName.Name).Value == inDBName);
            //    if(par != null) par.SetAttributeValue(TextConst.AName.StoreInDB, TextConst.AVBool.True);
            //}
            foreach (XElement ret in returns)
            { // старый вариант применения опциональных параметров, например отчет  21445-2
                string name = ret.AttrOrDefault(AName.name, string.Empty);
                XElement par = pars.Elements().SearchByAttribute(AName.name, name);
                if (par != null)
                {
                    XElement retEl = new XElement(ret.Element("return"));
                    XElement retVal = retEl.Descendants("fieldvalue").First();
                    retVal.ReplaceWith(par.Elements());
                    par.Elements().Remove();
                    par.Add(retEl.Elements());
                }
            }
            return pars;
        }
        public XElement GetReportParams()
        {
            //string[] allowed_items = { TextConst.EName.FieldGroup, TextConst.EName.Field, TextConst.EName.TabContainer };
            XElement xparams = VDataSet.ToXml(this.DataSource, EName.@params);
            XElement xcontent = new XElement(EName.content);
            xparams.Add(xcontent);
            XElement xcontent_native = this._xform.Element(EName.content) ?? this._xform;
            var queue = new Queue<Tuple<XElement, XElement>>();
            foreach (var xi in xcontent_native.Elements().Where(EPredicate.IsFieldGroupOrFieldOrTabContainer))
            {
                queue.Enqueue(new Tuple<XElement, XElement>(xi, xcontent));
            }
            XElement xitem_new = null;
            while (queue.Count > 0)
            {
                var item = queue.Dequeue();
                var item_info = this._layout.GetNodeByTag(item.Item1);
                xitem_new = new XElement(item.Item1.Name);
                xitem_new.CopyAttributes(item.Item1.Attributes());
                xitem_new.Elements().Remove();
                xitem_new.SetAttrValue(AName.visible, item_info.IsSelfVisible());
                if (item.Item1.Name == EName.field)
                {
                    //var crtl = ((item_info as VLayoutControlContainerInfo).GetContainedControl() as UIBase);
                    //xitem_new.SetAttributeValue(AName.Mandatory, crtl.Mandatory);
                    xitem_new.SetAttributeValue(AName.mandatory, item.Item1.AttrOrDefault(AName.mandatory, null));
                }
                else if (item.Item1.Name == EName.fieldgroup)
                {
                    xitem_new.SetAttrValue(AName.expanded, (item_info as VLayoutGroupInfo).IsExpanded());
                }
                item.Item2.Add(xitem_new);
                foreach (var xi in item.Item1.Elements().Where(EPredicate.IsFieldGroupOrFieldOrTabContainer))
                {
                    queue.Enqueue(new Tuple<XElement, XElement>(xi, xitem_new));
                }
            }
            return xparams;
        }
        private XElement GetParamsAsColumnsXml()
        {
            XElement fieldsXml = this._xform.Element(EName.content) ?? this._xform;
            XElement vc = new XElement(EName.viewcolumns);
            this.convertFielsXmlToColumnsXmlLevel(fieldsXml, vc);
            return vc;
        }
        private void convertFielsXmlToColumnsXmlLevel(XElement parent, XElement outputParent)
        {
            foreach (XElement element in parent.Elements())
            {
                XElement newElement = null;
                if (element.Name == EName.fieldgroup)
                {
                    newElement = new XElement(EName.band);
                }
                else if (element.Name == EName.field)
                {
                    newElement = new XElement(EName.column);
                }
                if (newElement != null)
                {
                    VLayoutNodeInfo item_info = this._layout.GetNodeByTag(element);
                    newElement.CopyAttributes(element.Attributes());
                    if (newElement.Attribute(AName.title) == null)
                    {
                        newElement.Add(new XAttribute(AName.title, "  "));
                    }
                    newElement.SetAttributeValue(AName.id, item_info.GetId().ToString());
                    newElement.SetAttrValue(AName.visible, item_info.IsSelfVisible());
                    outputParent.Add(newElement);
                    this.convertFielsXmlToColumnsXmlLevel(element, newElement);
                }
                else
                {
                    this.convertFielsXmlToColumnsXmlLevel(element, outputParent);
                }
            }
        }
        public void SetParamsVisibilitiyFromXml(XElement xcontentVisibleNew)// new done
        {
            foreach (XElement el in xcontentVisibleNew.Descendants())
            {
                bool visible = el.Attribute(AName.visible).Value == TextConst.AVBool.True;
                var item = this._layout.GetNodeById(el.Attribute(AName.id).Value);
                if (!visible && el.Name == EName.column)
                {
                    UIBase ctrl = this._controls[el.Attribute(AName.name).Value];
                    ctrl.SetChecked(false);
                    if (ctrl.GetBoundColumn().GetMandatory(null))
                    {
                        visible = true;
                    }
                }
                item.SetVisibility(visible);
            }
            this.ApplyVisibitlity();
        }
        #endregion
        #region Закрытые методы
        internal UIBase CreateUIControl(XElement xfield)
        {
            Contract.Assert(xfield != null);
            string typeName = xfield.Attribute(AName.controlType).Value;
            if (UIStatic.IsWeb() && typeName == TextConst.AVControlType.Number)
            {
                typeName = TextConst.AVControlType.Text;
            }
            Type control_type = UIBase.GetConcreteType(typeName);
            // создаем объект, вызывая нужный конструктор
            UIBase control = Activator.CreateInstance(control_type) as UIBase;
            // control.InitializeContent();
            control.FullInitialize(xfield, this);
            string fullName = control.FullName;
            if (this._controls.ContainsKey(fullName))
            {
                control = this._controls[fullName];
                //throw new VCompilerException("Повторяющееся имя поля", xfield.Ancestors(AName.Form).First(), xfield);
            }
            else
            {
                // подписываемся на события
                control.EditValueChanged += this.UIForm_EditValueChanged;
                control.NeedMasterValues += this.UIForm_NeedMasterValues;
                control.SpecialTypeChanged += this.UIForm_SpecialTypeChanged;
                control.Form = this;
                //if (!UIStatic.IsWeb())
                //{
                //     ( control.GetRootControl() as Control).Dock = DockStyle.Fill; // временно
                //}
                if (!this._no_data)
                {
                    control.BindData();
                }
                //int i = 1;
                //while (controls.ContainsKey(fullName))
                //{
                //    fullName = control.FullName + i++;
                //}
                control.FullName = fullName;
                this._controls.Add(control.FullName, control);
                XElement xbuttons = xfield.Element(EName.buttons);
                if (xbuttons != null)
                {
                    if (!UIStatic.IsWeb())// временно
                    {
                        foreach (XElement xcmd in xbuttons.Elements())
                        {
                            EditorButtonInfo btn = this.CreateEditorButton(xcmd, fullName);
                            //control.AddEditorButton(btn.ButtonControl, null, Button_Click);
                            //control.AddEditorAdditionalButton(btn, null, EditorButton_Click/*,xcmd.Attributes(AName.Visible).Any()*/);
                        }
                    }
                }
            }
            return control;
        }
        internal void LoadContentFromXmlNew(XElement xparent, VLayoutContainerInfo parent, TabContainerItem tabContainerItem, TabItem tabItem)
        {
            Contract.Assert(xparent != null);
            VLayoutNodeInfo item_info = null;
            UIBase f = null;
            foreach (XElement xitem in xparent.Elements())
            {
                XName name = xitem.Name;
                if (name == EName.field)
                {
                    item_info = this.CreateFieldNew(xitem, (VLayoutGroupInfo)parent, ref f);
                }
                else if (name == EName.empty_item)
                {
                    item_info = this.CreateEmptyItemNew(xitem, (VLayoutGroupInfo)parent);
                }
                else if (name == EName.menu)
                {
                    item_info = this.CreateButtonNew(xitem, (VLayoutGroupInfo)parent);
                }
                else if (name == EName.uicommand)
                {
                    item_info = this.CreateButtonNew(xitem, (VLayoutGroupInfo)parent);
                }
                else if (name == EName.fieldgroup)
                {
                    item_info = this.CreateFieldGroupNew(xitem, parent);
                    this.LoadContentFromXmlNew(xitem, item_info as VLayoutGroupInfo, null, tabItem);
                    //} else if (name == EName.ScrollArea) {
                    //    item_info = CreateScrollArea(xitem, parent);
                    //    LoadContentFromXml(xitem, item_info.ScrollControl.Root, null, tabItem);
                }
                else if (name == EName.tabcontainer)
                {
                    item_info = this.CreateTabContainerNew(xitem, (VLayoutGroupInfo)parent, tabItem);
                    // item_info = GetAnyItemInfo(tab_container.TabContainer);
                }
                else if (name == EName.splitcontainer)
                {
                    item_info = this.CreateSplitContainerNew(xitem, (VLayoutGroupInfo)parent);
                    // item_info = GetAnyItemInfo(tab_container.TabContainer);
                    this.LoadContentFromXmlNew(xitem, (VLayoutContainerInfo)item_info, null, tabItem);
                }
                else if (name == EName.grid)
                {
                    item_info = this.CreateFieldGridNew(xitem, (VLayoutGroupInfo)parent);
                    //} else if (name == EName.Splitter) {
                    //    item_info = CreateSplitter(xitem, parent);
                    //} else if (name == EName.UseForm) {
                    //    item_info = CreateFieldSubForm(xitem, parent);
                    //    break;
                }
                else if (name == EName.label)
                {
                    item_info = this.CreateLabelNew(xitem, (VLayoutGroupInfo)parent);
                }
            }
        }
        private VLayoutItemInfo CreateFieldNew(XElement xfield, VLayoutGroupInfo parent_group, ref UIBase ctrl, bool isInGrid = false)
        {
            UIBase control = this.CreateUIControl(xfield);
            control.isInGrid = isInGrid;
            ctrl = control;
            if (parent_group == null)
            {
                return null;
            }
            VLayoutItemInfo item_info = this.CreateControlContainerNew(xfield,
                //control.GetRootControl(), 
                null,
                parent_group);
            if (!isInGrid)
            {
                XElement list_query = xfield.Element(EName.listquery);
                if (list_query != null)
                {
                    foreach (XElement usepar in list_query.Descendants(EName.useparam))
                    {
                        this.AddVariableStateDependance(
                            //control.GetRootControl(), 
                            null,
                            usepar.Attribute(AName.name).Value, TextConst.EName.ListQuery);
                    }
                }
            }
            return item_info;
        }
        private VLayoutItemInfo CreateEmptyItemNew(XElement xfield, VLayoutGroupInfo parent_group)
        {
            var item_info = this.CreateControlContainerNew(xfield, null, parent_group);
            return item_info;
        }
        private VLayoutItemInfo CreateFieldGridNew(XElement xgrid, VLayoutGroupInfo parent_group)
        {
            throw new NotImplementedException();
            //var xgroup = new XElement(EName.FieldGroup, new XAttribute(AName.Title, "ttttt")); // чтобы смотреть/редактировать данные строки в форме, доделать для web
            //var gr_info = CreateFieldGroupNew(xgroup, parent_group);
            //var control = CreateGrid(xgrid,gr_info);
            //ucTableViewerContainer control = this.CreateGrid(xgrid);
            //var item_info = Layout.CreateItem(parent_group, control.GetControl(), xgrid);
            //(control as IControlWithTableSource).LayoutContainer = item_info;
            //item_info.IsFiller = true;
            //return item_info;
        }
        private VLayoutGroupInfo CreateFieldGroupNew(XElement xfieldgroup, VLayoutContainerInfo parent_group)
        {
            Contract.Assert(xfieldgroup != null);
            string group_title = xfieldgroup.AttrOrDefault(AName.title, string.Empty);
            var layout = Layout;
            bool another_form = xfieldgroup.AttrOrDefault(AName.is_form, false);
            string name = xfieldgroup.AttrOrDefault(AName.@as, null);
            VLayoutGroupInfo item_info = null;
            if (another_form)
            {
                layout = new VLayout();
                innerSubFormsNew[name] = new InnerSubFormInfo() { Layout = layout };
                item_info = layout.GetMainGroup();
            }
            else
            {
                item_info = layout.CreateGroup(parent_group, xfieldgroup);
                item_info.SetUncollapsible(xfieldgroup.AttrOrDefault(AName.uncollapsible, false));
                item_info.SetExpanded(xfieldgroup.AttrOrDefault(AName.expanded, true));
                item_info.SetBorderVisibility(!xfieldgroup.AttrOrDefault(AName.noborder, false));
                if (item_info.HasBorder)
                {
                    item_info.SetIsLayoutBlock(xfieldgroup.AttrOrDefault(AName.is_layout_block, true));
                }
                else
                {
                    item_info.SetIsLayoutBlock(xfieldgroup.AttrOrDefault(AName.is_layout_block, false));
                }
                XAttribute widthPercAttr = xfieldgroup.Attribute(AName.width_perc);
                if (widthPercAttr != null)
                {
                    int perc = Convert.ToInt32(widthPercAttr.Value);
                    item_info.SetWidthPercent(perc);
                }
                if (xfieldgroup.Descendants(EName.grid).Any())
                {
                    item_info.IsFiller = true;
                }
            }

            if (xfieldgroup.Parent != null && xfieldgroup.Parent.Name.LocalName != TextConst.EName.TabContainer && xfieldgroup.Elements(EName.useform).Any())
            {
                item_info.SetProperty(TextConst.EName.UseForm, xfieldgroup.Elements(EName.useform).FirstOrDefault());
                item_info.IsFiller = true;
                item_info.Showed += onGroupShowed;
            }





            if (xfieldgroup.AttrOrDefault(AName.show_toolbar, false))
            {
                item_info.SetProperty(TextConst.AName.ShowToolBar, TextConst.AVBool.True);
                XElement xtoolbar = xfieldgroup.Element(EName.toolbar);
                if (xtoolbar != null)
                {
                    //var grctrl = (item_info.GetControl() as sql.builder.UI.WinForms.VLayoutBarGroup);
                    //var mngr = grctrl.BarManager;
                    //var bar = mngr.Bars[0];
                    //var grctrl = (item_info.GetControl() as IVLayoutGroup);
                    //Cmn.UpdateToolbar(TmpGetBarManager(), grctrl.GetBar(), xtoolbar, Command_ItemClick, BarItemControl_EditValueChanged, BarItemRepository_EditValueChanged, this, getVariableDepandantceController());
                }
            }
            //Text = group_title,
            //           GroupBordersVisible = xfieldgroup.AttrOrDefault(TextConst.AName.NoBorder, "0") != "1",
            //           ExpandButtonVisible = true,
            //           Expanded = xfieldgroup.AttrOrDefault(TextConst.AName.Expanded, "1") != "0"
            item_info.SetText(group_title);
            foreach (XAttribute attr in xfieldgroup.Attributes().Where(APredicate.IsBehaviorColumns))
            {
                string attrName = attr.Name.LocalName;
                bool invert = xfieldgroup.AttrOrDefault(attrName + "-" + TextConst.AName.Invert, false);
                this.AddVariableStateDependance(item_info, attr.Value, attrName, invert);
            }
            return item_info;
        }
        private VLayoutItemInfo CreateButtonNew(XElement xfield, VLayoutGroupInfo parent_group)
        {
            Contract.Assert(xfield != null);
            IVButton control = this.CreateButtonControl(xfield);
            var item_info = CreateControlContainerNew(xfield, control, parent_group);
            if (xfield.Name == EName.menu)
            {
                IVPopupMenu menu = this.CreatePopupMenu(xfield);
                control.Menu = menu;
            }
            XAttribute widthFixedAttr = xfield.Attribute(AName.width_fixed);
            if (widthFixedAttr != null)
            {
                int val = control.GetTextWith();
                item_info.SetWidthFixed(val);
            }
            return item_info;
        }
        private void SetLayoutItemPropsNew(XElement xfield, VLayoutItemInfo item_info)
        {
            Contract.Assert(xfield != null);
            XAttribute textAttr = xfield.Attribute(AName.title);
            if (textAttr != null && xfield.Name != EName.uicommand && xfield.Name != EName.menu)
            {
                if (textAttr.Value != "" && textAttr.Value != " ")
                {
                    item_info.SetText(textAttr.Value);
                }
            }
            int widthMin = 250;
            XAttribute widthPercAttr = xfield.Attribute(AName.width_perc);
            if (widthPercAttr != null)
            {
                int perc = Convert.ToInt32(widthPercAttr.Value);
                item_info.SetWidthPercent(perc);
            }
            string hintText = xfield.AttrOrDefault(AName.hint, string.Empty);
            if (hintText == "LIKE")
            {
                hintText = "Символ % соответствует любой строке любой длины." + Environment.NewLine + "Для поиска значения, включающего подстроку, следует вводить %подстрока% .";
            }
            else if (!string.IsNullOrEmpty(hintText))
            {
                item_info.SetHint(hintText);
            }
            widthMin = Convert.ToInt32((decimal)item_info.GetWidthPercentWithParents() / 100m * (decimal)widthMin);
            item_info.SetWidthMin(widthMin);
            if ((!xfield.AttrOrDefault(AName.visible, true)) || (!xfield.AttrOrDefault(AName.column_visible, true)))
            {
                item_info.SetVisibility(false);
            }
            if (xfield.AttrOrDefault(AName.side, string.Empty) == TextConst.AVSides.Right)
            {
                (item_info as VLayoutControlContainerInfo).ControlDock = VLayout.Dock.Right;
            }
            item_info.IsFiller = xfield.AttrOrDefault(AName.fill_height, false);
            if (item_info.IsFiller)
            {
                foreach (var g in item_info.GetAllParentGroups()) g.IsFiller = true;
            }
        }
        private VLayoutItemInfo CreateLabelNew(XElement xfield, VLayoutGroupInfo parent_group)
        {
            var item_info = Layout.CreateLabel(parent_group, xfield);
            this.SetLayoutItemPropsNew(xfield, item_info);
            return item_info;
        }
        private VLayoutItemInfo CreateControlContainerNew(XElement xfield, object control, VLayoutGroupInfo parent_group)
        {
            var item_info = Layout.CreateItem(parent_group, control, xfield);
            this.SetLayoutItemPropsNew(xfield, item_info);
            return item_info;
        }
        private VLayoutSplitContainerInfo CreateSplitContainerNew(XElement xitem, VLayoutGroupInfo parent_group)
        {
            var item_info = Layout.CreateSplitContainer(parent_group, xitem);
            item_info.IsVertical = xitem.AttrOrDefault(AName.is_vertical, false);
            return item_info;
        }
        public bool HasTabs = false;// проблемы с layout для табов нужно обновлять дважды
        private VLayoutTabsInfo CreateTabContainerNew(XElement xitem, VLayoutGroupInfo parent_group, TabItem tabItem)
        {
            this.HasTabs = true;
            var tabsInfo = Layout.CreateTabContainer(parent_group, xitem);
            ////////////
            List<Tuple<VLayoutGroupInfo, XElement>> childInfos = new List<Tuple<VLayoutGroupInfo, XElement>>();
            foreach (XElement xitemChild in xitem.Elements())
            {
                var item_info_child = this.CreateFieldGroupNew(xitemChild, tabsInfo);
                childInfos.Add(new Tuple<VLayoutGroupInfo, XElement>(item_info_child, xitemChild));
            }
            //var vtabsCtrl = tabsInfo.GetControl() as sql.builder.UI.WinForms.VLayoutTabs;
            //  var tgroup = (tabsInfo.GetControl() as VLayoutTabs).TabbedGroup;
            // tgroup.SelectedPageChanged += tgroup_OnSelectedPageChanged;
            // var tabContainerItem = AddTabContainerItem(tgroup, tabItem);
            foreach (var item_info_child1 in childInfos)
            {
                //string name = null;
                //if (xitem.Attribute(AName.As) != null)
                //{
                //    name = xitem.Attribute(.AName.As).Value;
                //}
                //var tabCtrl = vtabsCtrl.GetLayoutItemByTabInfo(item_info_child1.Item1);
                //var tabItem1 = AddTabItem(tabCtrl, tabContainerItem, name);
                //tabCtrl.Tag = item_info_child1.Item2.Elements(TextConst.EName.UseForm).FirstOrDefault(); // вместо CreateSubForm
                item_info_child1.Item1.SetProperty(TextConst.EName.UseForm, item_info_child1.Item2.Elements(EName.useform).FirstOrDefault());
                item_info_child1.Item1.TabSelected += onSelectedPageChangedNew;
                //LoadContentFromXmlNew(item_info_child1.Item2, item_info_child1.Item1, null, tabItem1);
                this.LoadContentFromXmlNew(item_info_child1.Item2, item_info_child1.Item1, null, null);
                //добавть то что в конце LoadContentFromXmlNew для item_info_child1.Item1
            }
            return tabsInfo;
        }
        private void onSelectedPageChangedNew(object sender, EventArgs args)
        {

            loadFormForSelectedTabNew((VLayoutGroupInfo)sender);

        }


        private void onGroupShowed(object sender, EventArgs args)
        {
            //if (tab_page_adding) return;
            loadFormForSelectedTabNew((VLayoutGroupInfo)sender);
            //if (Layout == null)
            //{
            //    loadFormForSelectedTab(args);
            //}
            //else
            //{
            //    loadFormForSelectedTabNew(args);
            //}
        }
        public static UIFormC ActiveForm = null;
        private void SetActiveForm()
        {
            ActiveForm = this;
        }
        internal void ApplyVisibitlityForce()
        {
            this._layout.RefreshLayout();
        }
        public void ApplyVisibitlity()
        {
            //this.SetActiveForm();
            //// гридам кресса нужна форма
            //if (!_init || (this._grids.Any() && !HasParentContainer())) return;
            //this._layout.RefreshLayoutIfNeed();
            //foreach (var grid in Grids.Values)
            //{
            //    grid.UpdateDummyColumnWidth();
            //}
        }
        private IVButton CreateButtonControl(XElement xcmd)
        {
            throw new NotImplementedException();
        }
        private void btn_ButtonClick(object actionInfo)
        {
            Button_Click(actionInfo, null);
        }
        internal class EditorButtonInfo
        {
            public IVEditorButton ButtonControl = null;
            public UIBase FieldControl = null;
            public string VisibilitySource = null;
        }
        private EditorButtonInfo CreateEditorButton(XElement xcmd, string fieldName, string default_side = TextConst.AVSides.Left)
        {
            throw new NotImplementedException();
        }
        //private void UpdateGridToolbar(ucTableViewerContainer grid, XElement xtoolbar, bool bottom = false)
        //{
        //    IVBarItem[] items = Cmn.CreateBarItems(xtoolbar, this.TmpGetBarManager(), Command_ItemClick, this.BarItemControl_EditValueChanged, this.BarItemRepository_EditValueChanged, this, this.getVariableDepandantceController());
        //    grid.UpdateToolbar(xtoolbar, items, bottom);
        //}
        private bool _toolBarVisible = false;
        /*private static bool IsShowToolbar(XElement xform)
        {
            Contract.Assert(xform != null); 
            return xform.AttrOrDefault(AName.ShowToolBar, false);
        }*/
        private IVPopupMenu CreatePopupMenu(XElement xmenu)
        {

            throw new NotImplementedException();
        }
        public void UpdateFormToolbar(XElement xform, ValueChangeEventHandler handler, EventHandler ctrlEditValueChangedHandler, EventHandler repEditValueChangedHandler, UIFormC form, bool isDialog)
        {
            Contract.Assert(xform != null);
            if (!xform.AttrOrDefault(AName.show_toolbar, false))
            {
                return;
            }
            this.AddBarButton(TextConst.AVFormButtonType.Refresh, "Обновить");
            this.AddBarButton(TextConst.AVFormButtonType.Save, "Сохранить");
            this.AddBarButton(TextConst.AVFormButtonType.SaveAndClose, "Сохранить и закрыть");
            this.AddBarButton(TextConst.AVFormButtonType.Delete, "Удалить");
            this.AddBarButton(TextConst.AVFormButtonType.Choice, "Выбрать");
            this.AddBarButton(TextConst.AVFormButtonType.SaveSettings, "Сохранить шаблон");
            this.AddBarButton(TextConst.AVFormButtonType.LoadSettings, "Загрузить шаблон");
            this.AddBarButton(TextConst.AVFormButtonType.ExtParams, "Дополнительные параметры");
            //временно
            this.GetBarButton(TextConst.AVFormButtonType.Choice).ButtonClick += this.ButtonChoice_ItemClick;
            this.GetBarButton(TextConst.AVFormButtonType.Delete).ButtonClick += this.buttonDelete_ItemClick;
            this.GetBarButton(TextConst.AVFormButtonType.Refresh).ButtonClick += this.ButtonRefresh_ItemClick;
            this.GetBarButton(TextConst.AVFormButtonType.Save).ButtonClick += this.ButtonSave_ItemClick;
            this.GetBarButton(TextConst.AVFormButtonType.SaveAndClose).ButtonClick += this.ButtonSaveAndClose_ItemClick;
            //this.GetBarButton(FormBarButtonType.Test).ItemClick += ButtonTest_ItemClick;
            this.GetBarButton(TextConst.AVFormButtonType.ExtParams).ButtonClick += this.btnExtParams_ItemClick;
            this.GetBarButton(TextConst.AVFormButtonType.LoadSettings).ButtonClick += this.btnLoadSettings_ItemClick;
            this.GetBarButton(TextConst.AVFormButtonType.SaveSettings).ButtonClick += this.btnSaveSettings_ItemClick;
            this._toolBarVisible = true;
            if (this._useType == UseType.DataEditor)
            {
                this.GetBarButton(TextConst.AVFormButtonType.Refresh).SetVisible(true);//.Visibility = BarItemVisibility.Always;
                this.GetBarButton(TextConst.AVFormButtonType.Save).SetVisible(true);//.Visibility = BarItemVisibility.Always;
                // barButtonItem4.Visibility = BarItemVisibility.Always;
                //GetBarButton(FormBarButtonType.Test).Visibility = BarItemVisibility.Always;
                if (isDialog) this.GetBarButton(TextConst.AVFormButtonType.SaveAndClose).SetVisible(true);//.Visibility = BarItemVisibility.Always;
            }
            this.InitImages();
            this.TmpBarManagetInitialize();
            //TmpGetBarManager().ForceLinkCreate();
            //TmpGetBarManager().ForceInitialize();
            //if (xform.AttrOrDefault(AName.ShowToolBar, false))
            //{
            //TmpGetToolBar().Visible = true;
            //}
            //this.GetControl().GetToolBar().SetVisible(true);
            if (xform.AttrOrDefault(AName.params_customization, false))
            {
                this.GetBarButton(TextConst.AVFormButtonType.ExtParams).SetVisible(true);//.Visibility = BarItemVisibility.Always;
            }
            if (xform.AttrOrDefault(AName.allow_save, false))
            {
                this.GetBarButton(TextConst.AVFormButtonType.SaveSettings).SetVisible(true);//.Visibility = BarItemVisibility.Always;
                this.GetBarButton(TextConst.AVFormButtonType.LoadSettings).SetVisible(true);//.Visibility = BarItemVisibility.Always;
            }
            XElement xtoolbar = xform.Element(EName.toolbar);
            if (xtoolbar != null)
            {
                // настраиваем дефолтные кнопки
                foreach (XElement xcmd in xtoolbar.Elements(EName.uicommand))
                {
                    XAttribute attr = xcmd.Attribute(AName.control_name);
                    if (attr != null)
                    {
                        IVBarButton btn = this.GetBarButton(attr.Value);
                        //var btn = (this.GetControl() as sql.builder.UI.WinForms.UIFormControl).GetToolBarItem(xcmd.Attribute(AName.ControlName).Value);
                        bool visible = xcmd.AttrOrDefault(AName.column_visible, false);
                        this.SetControlVisible(btn, visible);
                    }
                }
                if (!UIStatic.IsWeb())
                {//временно
                    //Cmn.UpdateToolbar(this.TmpGetBarManager(), this.GetToolBar(), xtoolbar, handler, ctrlEditValueChangedHandler, repEditValueChangedHandler, form, this.getVariableDepandantceController());
                }
            }
        }
        private void BarItemRepository_EditValueChanged(object sender, EventArgs e)
        {
            //var editor = sender as BaseEdit;
            //var control = editor.Properties.Tag as UIBase;

            //var val1 = editor.EditValue;
            //var val2 = control.GetSimpleSourceValue();
            //if (!object.Equals(val1, val2))
            //{
            //    control.SetControlValue(val1);
            //}
        }
        private void BarItemControl_EditValueChanged(object sender, EventArgs args)
        {
        }
        private void UpdateGroupsCaptionsNew()
        {
        }
        private void AttachDataSourceEvents()
        {
            this.DataSource.Changed += this.OnDataChanged;
            this.DataSource.TopTableRefreshed += this.Data_OnTopTableRefreshed;
            this.DataSource.TopTableCommited += this.Data_OnTopTableCommited;
            foreach (VDataTable tbl in this.DataSource.Tables)
            {
                if (tbl.StructureType == StructureType.Table)
                {
                    tbl.CurrentRowChanged += this.OnDataCurrentRowChanged;
                    tbl.CurrentRowRefreshed += this.OnDataCurrentRowChanged;
                    tbl.UserChangedData += this.OnUserChangedData;
                }
            }
        }
        private void DetachDataSourceEvents()
        {
            this.DataSource.Changed -= this.OnDataChanged;
            this.DataSource.TopTableRefreshed -= this.Data_OnTopTableRefreshed;
            this.DataSource.TopTableCommited -= this.Data_OnTopTableCommited;
            foreach (VDataTable tbl in this.DataSource.Tables)
            {
                if (tbl.StructureType == StructureType.Table)
                {
                    tbl.CurrentRowChanged -= this.OnDataCurrentRowChanged;
                    tbl.CurrentRowRefreshed -= this.OnDataCurrentRowChanged;
                    tbl.UserChangedData -= this.OnUserChangedData;
                }
            }
        }
        private void UpdateControlVisibility(VDataColumn col, VFieldStateAndOtherInfo fs)
        {
            foreach (var c in col.BoundControls) ((UIBase)c).Form.SetControlOptions(((UIBase)c).FieldName, fs);
        }
        public string GetValidation()
        {
            return dataSource.Validate().Error;
        }

        public bool SaveData(bool isClosing)
        {
            throw new NotImplementedException();
        }
        public bool WasChanges = false;
        public void SaveDataAndClose()
        {
            if (this.SaveData(true))
            {
                this.Close();
            }
        }
        public void Choice()
        {
            AcceptSelection(DataSource.TopTable.First().TableName);
            //TmpGetControlAsWinFormCtrl().FindForm().DialogResult = DialogResult.OK;

        }
        public void Close()
        {
            throw new NotImplementedException();
        }
        public bool ChooseReportParams(ref DataTable dt_params)
        {
            // тут не используется
            return true;
        }
        public void UpdateButtonsState()
        {
            if (!this._toolBarVisible) return;
            if (this._useType == UseType.DataEditor)
            {
                bool modified = this.IsModifiedSelfOrSub();
                this.SetButtonEnable(TextConst.AVFormButtonType.Save, modified);
                //this.SetButtonEnable(FormBarButtonType.SaveAndClose, modified);
                this.SetButtonEnable(TextConst.AVFormButtonType.Delete, (DataSource.TopTable[0].CurrentRow != null));
                if (ContainerForm != null)
                {
                    this.ContainerForm.UpdateButtonsState();
                }
            }
        }
        public IEnumerable<UIFormC> GetRelativeFormsAndSelf()
        {
            var forms = new List<UIFormC>();
            var stack = new Stack<UIFormC>();
            var form = this as UIFormC;
            while (true)
            {
                forms.Add(form);
                if (form.SubForms != null)
                {
                    foreach (var s in form.SubForms) stack.Push(s);
                }

                if (stack.Count == 0) break;

                form = stack.Pop();
            }

            return forms;
        }
        #endregion
        #region Обработчики событий
        //private void UIFormC_Load(object sender, EventArgs e)
        //{
        //    //_loaded = true;
        //    //ApplyVisibitlity();
        //}
        //private void UIFormC_Paint(object sender, PaintEventArgs e)
        //{
        //    //Equip();
        //}
        public XElement UIForm_GetReportScheme()
        {
            return (NeedReportScheme != null) ? NeedReportScheme(this) : null;
        }
        internal VReport UIForm_GetReport()
        {
            return (NeedReport != null) ? NeedReport(this) : null;
        }
        private XElement UIForm_NeedMasterValues(UIBase sender)
        {
            return GetValue(sender.Masters);
        }
        private void UIForm_EditValueChanged(object sender, EventArgs e)
        {
            UpdateGroupsCaptionsNew();
            if (AnyValueChanged != null)
            {
                AnyValueChanged(this, e);
            }
        }
        private void UIForm_SpecialTypeChanged(string special_type, object data)
        {
            if (SpecialTypeChanged != null)
            {
                SpecialTypeChanged(this, special_type, data);
            }
        }

        private void OnDataCurrentRowChanged(object sender, DataRowChangeEventArgs e)
        {
            this.FlagRefreshingAll = true;
            VDataTable table = (VDataTable)sender;
            foreach (VDataColumn col in table.Columns)
            {
                if (!VDataColumn.HasBoundControl(col)) continue;
                if (this._useType == UseType.SchemeEditor) //!!! Для редактора запросов
                {
                    var fs = col.GetFieldState();
                    UpdateControlVisibility(col, fs);
                    if (fs.VisibleInForm)
                    {
                        foreach (UIBase ctrl in col.BoundControls)
                        {
                            ctrl.UpdateControlData();
                            ctrl.RefreshData();
                            //ctrl.Changed(Cmn.Nvl(e.Row[col.ColumnName], null) == null);
                            ctrl.Changed(e.Row[col.ColumnName] == null);
                        }
                    }
                }
                else if (this._useType == UseType.DataEditor)
                {
                    foreach (UIBase ctrl in col.BoundControls)
                    {
                        // ctrl.RefreshData();
                        if (!ctrl.isInGrid || UIStatic.IsWeb())
                        {
                            ctrl.UpdateControlData();
                            ctrl.UpdateControlEditable(null);
                            ctrl.UpdateControlValidation(null);
                            ctrl.UpdateControlVisibitity();
                        }
                    }

                }
            }
            //!!!Нужно  обновлять только связанные с  соотв таблицей
            this.UpdateAllControlsStates();
            if (!this.DataSource.ChangesNotCompleted)
            {
                this.ApplyVisibitlity();  // мигало, закоментировал теперь не работает в редакторе схемы при смене строки. Сделал флаг
            }
            this.UpdateButtonsState();
            this.FlagRefreshingAll = false;
        }

        private void OnDataChanged(object sender, EventArgs e)
        {
            VDataTable table = (VDataTable)sender;
            if (table.StructureType == StructureType.Info) return;
            UIBase ctrl = table.Control;
            if (ctrl == null)
            {
                if (e is DataColumnChangeEventArgs)
                {
                    var e1 = (DataColumnChangeEventArgs)e;
                    if (e1.Row == table.CurrentRow)
                    {
                        var col = (VDataColumn)e1.Column;
                        ctrl = col.BoundControls.FirstOrDefault() as UIBase;
                        if (ctrl != null && ctrl.SourceType != ReturnType.Array)
                        {
                            ctrl.Changed(e1.Row[col] == DBNull.Value || e1.Row[col].ToString() == "" || ((ctrl is UICheck) && e1.Row[col].ToString() == "0"));
                            if (this._useType == UseType.SchemeEditor)//!!! Для редактора запросов
                            {
                                if (col.SelectionList != null)
                                {
                                    if (!(col.BoundControls.First() as UIBase).Form.FlagRefreshingAll)
                                    {
                                        (col.BoundControls.First() as UIBase).Form.FlagRefreshingAll = true;
                                        foreach (VDataColumn col1 in table.Columns)
                                        {
                                            if (VDataColumn.HasBoundControl(col1))
                                            {
                                                if (col1.BoundControls.First() != ctrl)
                                                {
                                                    VFieldStateAndOtherInfo fs = col1.GetFieldState();
                                                    UpdateControlVisibility(col1, fs);
                                                    if (fs.VisibleInForm)
                                                    {
                                                        (col1.BoundControls.First() as UIBase).RefreshData();
                                                    }
                                                }
                                            }
                                        }
                                        (col.BoundControls.First() as UIBase).Form.FlagRefreshingAll = false;
                                    }
                                }
                                ApplyVisibitlity();
                            }
                        }
                    }
                }
            }
            else
            {
                if (ctrl.ArrayEditValue.Rows.Count == 0)
                {
                    ctrl.Used = false;
                    ctrl.Changed(true);
                }
                else
                {
                    ctrl.Changed(false);
                }
            }
        }
        #region реализация IForm
        public void LayoutSuspend()
        {
            if (this._layout != null)
            {
                this._layout.Suspended = true;
            }
        }
        public void LayoutResume()
        {
            if (this._layout != null)
            {
                this._layout.Suspended = false;
                // Layout.RefreshLayoutIfNeed();
            }
        }
        #endregion
        public void LayoutRefresh()
        {
            if (this._layout != null)
            {
                this._layout.RefreshLayoutIfNeed();
            }
        }
        public bool LoadData(object[] pars)
        {
            return UIStatic.UpdateForm(this, pars);
        }
        public void RefreshSource(object[] new_pars = null, bool isCreation = false)
        {
            LayoutSuspend();
            //RefreshData();
            dataSource.RefreshTopTable(isCreation);
            if (isCreation)
            {
                dataSource.WasRefresh = true;// поставил т.к. не работала подгрузка доп. информации об абоненте при создании карточки ПИР
            }
            if (SubForms != null)
            {
                foreach (var frm in SubForms)
                {
                    if (frm.Equiped)
                    {
                        if (frm.DataSource.ParentDataTable == null)
                        {
                            frm.RefreshSource();
                        }
                    }
                }
            }
            RaiseUIEvent(TextConst.AVEventName.FormLoaded);
            LayoutResume();
        }
        private void ButtonSave_ItemClick(object sender)
        {
            this.SaveData(false);
        }
        private void ButtonSaveAndClose_ItemClick(object sender)
        {
            this.SaveDataAndClose();
        }
        private void ButtonChoice_ItemClick(object sender)
        {
            this.Choice();
        }
        private void Command_ItemClick(object sender)
        {
            throw new NotImplementedException();
        }
        //private static UIBase _popupMenuOwner = null;
        private void EditorButton_Click(object sender)
        {
            var btn = (IVEditorButton)sender;
            this.Button_Click(btn, null);
        }
        internal void Button_Click(object sender, EventArgs e)
        {

            throw new NotImplementedException();
        }
        #endregion
        public void AcceptSelection(string tableName)
        {
            //this._grids[tableName].AcceptSelection();
        }
        public void InitSelection(IEnumerable<object> values)
        {
            //this._grids[GetCheckTableName()].SetSelection(values);
        }
        internal string GetCheckTableName()
        {
            throw new NotImplementedException();
            //var grid = Grids.FirstOrDefault(g => g.Value.GetMainView().OptionsSelection.MultiSelectMode == GridMultiSelectMode.CheckBoxRowSelect);
            //var grid = this._grids.FirstOrDefault(g => g.Value.IsSelectMode());
            //return (!grid.Equals(default(KeyValuePair<string, ucTableViewerContainer /*IReportGrid*/>))) ? grid.Key : this._grids.First().Key;
        }
        #region Сохранение/загрузка настроек формы
        private decimal _selected_kod_gs;
        internal XElement SaveParamsXML()
        {
            throw new NotImplementedException();
            //var xroot = new XElement(EName.root);
            //Parser.SaveReportParamsToXml(xroot, this);
            //return xroot;
        }
        internal void LoadParamsXML(XElement reportParams)
        {
            throw new NotImplementedException();
            //Parser.LoadReportParamsFromXml(reportParams, this);
        }
        internal void SaveGS()
        {
            throw new NotImplementedException();
        }
        internal void LoadGS()
        {
        }
        internal bool ChooseReportParams(string title = null)
        {
            throw new NotImplementedException();
        }
        private void btnExtParams_ItemClick(object sender)
        {
            ChooseReportParams();
        }
        private void btnSaveSettings_ItemClick(object sender)
        {
            SaveGS();
        }
        private void btnLoadSettings_ItemClick(object sender)
        {
            LoadGS();
        }
        #endregion
        public void DeleteCurrentRowFromFirstTableAndSave()
        {
        }
        private void buttonDelete_ItemClick(object sender)
        {
            DeleteCurrentRowFromFirstTableAndSave();
        }
        public string GetSecurityID()
        {
            return SecurityID;
        }
        public void SetAccessDenied(string security_id)
        {
            //var ad = new ucAccessDenied(security_id, false) { Dock = DockStyle.Fill };
            //foreach (var c in TmpGetControlAsWinFormCtrl().Controls.Cast<Control>()) c.Visible = false;
            //TmpGetControlAsWinFormCtrl().Controls.Add(ad);
        }
        public void SetLayout(VLayoutGroupInfo root)
        {

            //IVForm c = this.GetControl();
            //c.ClearChilds();
            ////TmpGetControlAsWinFormCtrl().Controls.Clear();
            //var layoutMainCtrl = (IVControl)root.GetControl();
            ////layoutMainCtrl.Dock = DockStyle.Fill;
            //c.AddChild(layoutMainCtrl);
            // TmpGetControlAsWinFormCtrl().Controls.Add(layoutMainCtrl);
        }
        public void SetTitle(string title)
        {
            this.TitleOriginal = title;
            this.UpdateTitle();
        }
        private void setTitle(string title)
        {
        }
        internal void SetButtonVisible(string btn, bool visible)
        {
            GetBarButton(btn).SetVisible(visible);
            //BarButtonItem button = GetBarButton(btn);
            //button.Visibility = (visible) ? BarItemVisibility.Always : BarItemVisibility.Never;
        }
        internal void SetButtonEnable(string btn, bool enable)
        {
            GetBarButton(btn).SetEnabled(enable);
            //BarButtonItem button = GetBarButton(btn);
            //button.Enabled = enable;
        }
        public bool AskUnsavedChangesQuestion()
        {
            throw new NotImplementedException();
            //DialogResult result = ShowMessage.Show(ShowMessage.MType.UnsavedChangesQuestion);
            //return (result == DialogResult.Yes);


        }
        /*
        public bool AskDeleteRow(string title)
        {
            throw new NotImplementedException();
        }
        public void ShowInformation(string text)
        {
            throw new NotImplementedException();
        }
        public void ShowWaitForm(string text)
        {
            throw new NotImplementedException();
        }
        public void HideWaitForm()
        {
            throw new NotImplementedException();
        }
        public XElement LoadSettings()
        {
            throw new NotImplementedException();
        }
        public void ShowInnerSubform(InnerSubFormInfo sub, string name)
        {
            throw new NotImplementedException();
        }
        */
        public IBase GetControlByFullName(string name)
        {
            return this._controls[name];
        }
        public void MoveFieldToOtherLayoutGroup(string fieldName, VLayoutGroupInfo newGroup)
        {
            //var field = this._controls[fieldName];
            //var rctrl = field.GetRootControl();
            //var containerInfo = Layout.GetItemByControl(rctrl);
            //if (containerInfo.label != null)
            //{
            //    containerInfo.label.ChangeParent(newGroup);
            //}
            //containerInfo.ChangeParent(newGroup);
        }
        /// <summary>
        /// Возвращает экземпляр поля для манипуляций с ним извне
        /// </summary>
        /// <param name="name">Имя параметра из xml-описания формы</param>
        /// <returns></returns>
        public ParamField GetParamField(string name)
        {
            UIBase control = null;
            if (this._controls.TryGetValue(name, out control))
            {
                return ParamField.Create(control);
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<ParamField> GetParamFields()
        {
            return this._controls.Select(it => ParamField.Create(it.Value));
        }
        #region реализация IForm
        public object _dialogContainer = null;
        public void SetDialogContainer(object value)
        {
            this._dialogContainer = value;
        }
        public object GetDialogContainer()
        {
            return this._dialogContainer;
        }
        #endregion
    }
}
