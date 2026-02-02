using System.Collections.Concurrent;
using System.Xml.Linq;
//using DevExpress.XtraLayout;
using System.Collections.Generic;
//using infoenergo.core.Extensions;
using sql.builder.DataApi;
//using System.Windows.Forms;
using System.Data;
using System.Threading.Tasks;
//using DevExpress.XtraEditors;
//using sql.builder.Test;
using sql.builder.XmlHelpers;

namespace sql.builder.UI
{
    public partial class UIFormC : IForm
    {
        static SubFormsFactory SubFactory = new SubFormsFactory();

        private VUseForm useFormInfo = null;
        public UIFormC ContainerForm { get; set; }
        public List<UIFormC> SubForms { get; set; }

        public Task task { get; set; }
            
        public void PrepareAsync(XElement xuseform, UIFormC parent)
        {
            task = Task.Factory.StartNew(() => Prepare(xuseform, parent));
        }
        public void Prepare(XElement xuseform, UIFormC parent)
        {
            // настройка
            this._equiped = false;
            ContainerForm = parent;
            useFormInfo = VSXElement.Get<VUseForm>(new XElement(xuseform));
            //useFormInfo.environment = XmlReports.Environment;
            this._useType = UseType.DataEditor;

            // форма
            //var form = XmlReports.Environment.GetForm(useFormInfo.Attribute(AName.Call).Value);

            var fds = VForm.GetFormXelementAndDataSet(useFormInfo.Attribute(AName.call).Value);
            this._xform = fds.Item1;
            DataSource = fds.Item2;
            XParams = fds.Item3;
            //XForm = form.GetFormXElement();

            //// создание DataSet
            //DataSource = form.ProcessAndCreateDataSet();
            DataSource.Connection = ContainerForm.DataSource.Connection;

            // установка параметров
            VReport.ApplySimpleParams(null, this.DataSource, this._xform.Element(EName.@params)/* form.ParamsElement()*/);
            DataColumn col = null;
            if (xuseform.Element(EName.useparam) != null)
            {
                col = ContainerForm.DataSource.GetParamColumn(xuseform.Element(EName.useparam).Attribute(AName.name).Value);
            }
           
            DataRow r = null;
            // Обработаны только частные случаи
            if (col != null)
            {
                r = (col.Table as VDataTable).CurrentRow;
                (col.Table as VDataTable).AddChildDataset(DataSource, col.ColumnName);
            }
            else
            {
                var pars = useFormInfo.GetParamsRuntimeValues(ContainerForm.DataSource, r, null);
                DataSource.SetParamsValues(pars);
            }
        }

        public void loadFormForSelectedTabNew(VLayoutGroupInfo tab)
        {

            UIFormC frm = tab.GetProperty(TextConst.AName.Form) as UIFormC;
            XElement xuseform = null;
            if (frm == null)
            {
                xuseform = (XElement)tab.GetProperty(TextConst.EName.UseForm);

                if (xuseform == null) return;

                try
                {
                    var mode = (UIStatic._show_wait_forms) ? WaitUIMode.WaitPanel : WaitUIMode.WaitCursor;
                    WaitUIHelper.LastUsedUIHelper.Show("Загрузка формы", mode);
                    //Wait.Show("Загрузка формы", only_cursor: !UIStatic._show_wait_forms);


                    // Layout.CreateItem(tabInfo,

                    frm = SubFactory.Get(xuseform, this);
                    tab.SetProperty(TextConst.AName.Form, frm);
                    // frm.Visible = false;
                    // чтобы высота контролов установилась нормально
                    // frm.Width = item.Width;
                    //layoutControl.BeginUpdate();
                    // item.BeginInit();
                    //  item.Control.Controls.Add(frm);

                    //item.EndInit();
                    //layoutControl.EndUpdate();

                    if (SubForms == null) SubForms = new List<UIFormC>();
                    SubForms.Add(frm);
                    // frm.Visible = false;

                    // this.Visible = true;

                    var xtab = xuseform.Parent;

                    var tabInfo = (VLayoutGroupInfo)Layout.GetNodeByTag(xtab);
                    //tabInfo.NoSpaces = true;
                    // var item = Layout.CreateItem(tabInfo, frm, xuseform);
                    // item.IsFiller = true;
                    // Layout.RefreshLayoutIfNeed();
                    frm.Equip(tab);
                    Layout.RefreshLayout();
                    //if (!UIStatic.IsWeb())
                    //{
                    //    // MessageBox.Show("1");
                    //    // gctrl.Margin = new System.Windows.Forms.Padding(0);
                    //    gctrl.Dock = DockStyle.Fill;
                    //}
                    // Layout.RefreshLayout();
                    //(tabInfo.GetTypedControl() as Control).Visible = true;

                    // MessageBox.Show("2");
                }
                finally
                {
                    WaitUIHelper.LastUsedUIHelper.Hide();
                    //Wait.Hide();
                }

                tab.SetProperty(TextConst.EName.UseForm, null);
            }
            else
            {
                frm.DataSource.RefreshTopTableIfClear();// !!! Проконтролировать чтобы небыло лишних обновлений
            }
        }
        public void Equip(VLayoutGroupInfo parentLayoutGroup)
        {
            if (this._equiped) return;
            try
            {
                this.LayoutSuspend();
                this._equiped = true;
                //Wait.Show("Загрузка данных", only_cursor: !UIStatic._show_wait_forms);
                var mode = (UIStatic._show_wait_forms) ? WaitUIMode.WaitPanel : WaitUIMode.WaitCursor;
                WaitUIHelper.LastUsedUIHelper.Show("Загрузка данных", mode);
                // инициализация графики
                this.Initialize(this._xform, false, parentLayoutGroup);
                // получение данных
                if (this._init)
                {
                    this.RefreshData();
                    this.dataSource.RefreshTopTable(false);
                }
                this.LayoutResume();
            }
            finally
            {
                //Wait.Hide();
                WaitUIHelper.LastUsedUIHelper.Hide();
            }
        }
    }
}
