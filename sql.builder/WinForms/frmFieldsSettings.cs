//using System;
//using System.Data;
//using System.Linq;
////using System.Windows.Forms;
//using System.Xml.Linq;

////using DevExpress.XtraTreeList.Nodes;
//using infoenergo.core.Extensions;
//using sql.builder.DataApi;

//namespace sql.builder.WinForms
//{
//    partial class frmFieldsSettings : frmBaseSqlBuilder
//    {
//        DataTable _dt_fields;
//        readonly frmTableLayoutColumns _frmTableLayoutColumns;

//        private string _def_text_location = "left";
//        private string _def_layout_mode = "regular";
//        private bool _def_text_visible = true;
//        private int _def_width_perc = 100;

//        internal frmFieldsSettings()
//        {
//            InitializeComponent();
//            _frmTableLayoutColumns = new frmTableLayoutColumns();
//        }

//        internal void Initialize(VQuery vquery)
//        {
//            InitTable();
//            // заполняем DataTable
//            foreach (var vparam in vquery.Params())
//            {
//                var vfield = vparam.Field();
//                var node_name = vfield.P_NodeName;

//                var width_perc = (vparam.P_WidthPerc != "") ? (object)int.Parse(vparam.P_WidthPerc) : DBNull.Value;
//                var width_fixed = (vparam.P_WidthFixed != "") ? (object)int.Parse(vparam.P_WidthFixed) : DBNull.Value;
//                var title = (vparam.P_SelfTitle != "") ? vparam.P_SelfTitle : (vfield.P_Title != "") ? vfield.P_Title : node_name;

//                object text_visible = DBNull.Value;
//                object text_location = DBNull.Value;
//                if (TextConst.ENameArray.AllowTextMode.Contains(node_name))
//                {
//                    text_visible = vparam.P_TextVisible == "" || vparam.P_TextVisible == "1";
//                    text_location = (vparam.P_TextLocation != "") ? vparam.P_TextLocation : _def_text_location;
//                }


//                object layout_mode = DBNull.Value;
//                if (TextConst.ENameArray.AllowLayoutMode.Contains(node_name))
//                {
//                   // layout_mode = (vparam.P_LayoutMode != "") ? vparam.P_LayoutMode : _def_layout_mode;
//                }

//                _dt_fields.Rows.Add(node_name, title, width_perc, width_fixed, text_visible, text_location, layout_mode, 0, 0, vparam, vparam.Parent);
//            }
//            InitTree();
//        }
//        internal void Initialize(VForm vform)
//        {
//            InitTable();

//            // заполняем DataTable
//            var not_allowed_parents = new[] { TextConst.EName.Field, TextConst.EName.Column, TextConst.EName.Fact, TextConst.EName.UICommand, TextConst.EName.Toolbar, TextConst.EName.Events, TextConst.EName.LayoutColumns };
//            var not_allowed_nodes = new[] { TextConst.EName.Columns, TextConst.EName.Toolbar, TextConst.EName.Events, TextConst.EName.LayoutColumns };

//            foreach (var vsxitem in VSXElement.GetDescedantsP(vform.ContentElement()))
//            {
//                if (not_allowed_nodes.Contains(vsxitem.Name.LocalName)) continue;
//                if (vsxitem.Ancestors().Any(a => not_allowed_parents.Contains(a.Name.LocalName))) continue;

//                int image_index = 6;
//                switch (vsxitem.Name.LocalName)
//                {
//                    case TextConst.EName.Field:
//                    case TextConst.EName.Column:
//                    case TextConst.EName.Fact:
//                        if (vsxitem.Parent.Name.LocalName != TextConst.EName.Columns)// && vsxitem.AttrOrDef("column-visible","1") == "1")
//                        {
//                            image_index = 0; break;
//                        }
//                        else continue;

//                    case TextConst.EName.FieldGroup: image_index = 1; break;
//                    case TextConst.EName.UICommand: image_index = 2; break;
//                    case TextConst.EName.Label: image_index = 3; break;
//                    case TextConst.EName.Grid: image_index = 4; break;
//                    case TextConst.EName.Splitter: image_index = 5; break;
//                }

//                var width_perc = (vsxitem.P_WidthPerc != "") ? (object)int.Parse(vsxitem.P_WidthPerc) : DBNull.Value;
//                var width_fixed = (vsxitem.P_WidthFixed != "") ? (object)int.Parse(vsxitem.P_WidthFixed) : DBNull.Value;
//                var node_name = vsxitem.P_NodeName;

//                var title = (vsxitem.P_Title != "") ? vsxitem.P_Title : node_name;

//                object text_visible = DBNull.Value;
//                object text_location = DBNull.Value;
//                if (TextConst.ENameArray.AllowTextMode.Contains(node_name))
//                {
//                    text_visible = vsxitem.P_TextVisible == "" || vsxitem.P_TextVisible == "1";
//                    text_location = (vsxitem.P_TextLocation != "") ? vsxitem.P_TextLocation : _def_text_location;
//                }

//                object layout_mode = DBNull.Value;
//                object cols_count = DBNull.Value;
//                if (TextConst.ENameArray.AllowLayoutMode.Contains(node_name))
//                {
//                   // layout_mode = (vsxitem.P_LayoutMode != "") ? vsxitem.P_LayoutMode : _def_layout_mode;
//                    cols_count = (vsxitem.Element(TextConst.EName.LayoutColumns) != null) ? vsxitem.Element(TextConst.EName.LayoutColumns).Elements().Count() : 1;
//                }

//                _dt_fields.Rows.Add(node_name, title, width_perc, width_fixed, text_visible, text_location, layout_mode, cols_count, image_index, vsxitem, vsxitem.Parent);
//            }

//            InitTree();
//        }
//        internal void UpdateFieldsSettings()
//        {
//            tl.CloseEditor();
//            foreach (var node in tl.GetNodeList())
//            {
//                var xitem = node["key"] as XElement;
//                var node_name = node["node_name"];
//                var width_perc = node["width_perc"];
//                var width_fixed = node["width_fixed"];

//                if (width_perc == DBNull.Value || width_perc.Equals(_def_layout_mode)) width_perc = null;
//                if (width_fixed == DBNull.Value) width_fixed = null;
//                xitem.SetAttributeValue(TextConst.AName.WidthPerc, width_perc);
//                xitem.SetAttributeValue(TextConst.AName.WidthFixed, width_fixed);

//                if (TextConst.ENameArray.AllowTextMode.Contains(node_name))
//                {
//                    xitem.SetAttributeValue(TextConst.AName.TextVisible, (node["text_visible"].Equals(_def_text_visible)) ? null : "0");
//                    xitem.SetAttributeValue(TextConst.AName.TextLocation, (node["text_location"].Equals(_def_text_location)) ? null : node["text_location"]);
//                }
//                //if (TextConst.ENameArray.AllowLayoutMode.Contains(node_name))
//                //{
//                //    xitem.SetAttributeValue(TextConst.AName.LayoutMode, node["layout_mode"].Equals(_def_layout_mode) ? null : node["regular"]);
//                //}

//                // признак custom_layout
//                var size_attr = xitem.Attribute(TextConst.AName.Size);
//                if (size_attr == null && node.Checked)
//                {
//                    xitem.SetAttributeValue(TextConst.AName.Size, "20;20");
//                }
//            }
//        }

//        void InitTable()
//        {
//            _dt_fields = new DataTable();
//            _dt_fields.Columns.AddRange(new[]
//            {
//               new DataColumn("node_name", typeof(string)),
//               new DataColumn("title", typeof(string)),
//               new DataColumn("width_perc", typeof(int)),
//               new DataColumn("width_fixed", typeof(int)),
//               new DataColumn("text_visible", typeof(bool)),
//               new DataColumn("text_location", typeof(string)),
//               new DataColumn("layout_mode", typeof(string)),
//               new DataColumn("cols_count", typeof(int)),
//               new DataColumn("image_index", typeof(int)),
//               new DataColumn("key", typeof(XElement)),
//               new DataColumn("parent", typeof(XElement))
//            });
//        }
//        void InitTree()
//        {
//            var dt_layout_mode = new DataTable();
//            dt_layout_mode.Columns.Add("name");
//            dt_layout_mode.Rows.Add("regular");
//            dt_layout_mode.Rows.Add("table");
//            dt_layout_mode.Rows.Add("flow");
//            rleLayoutMode.DataSource = dt_layout_mode;

//            var dt_text_location = new DataTable();
//            dt_text_location.Columns.Add("name");
//            dt_text_location.Rows.Add("left");
//            dt_text_location.Rows.Add("top");
//            dt_text_location.Rows.Add("right");
//            dt_text_location.Rows.Add("bottom");
//            rleTextLocation.DataSource = dt_text_location;

//            tl.DataSource = _dt_fields;
//            tl.ForceInitialize();
//            tl.ExpandAll();

//            // расставляем галочки
//            tl.BeginUpdate();
//            foreach (var node in tl.GetNodeList())
//            {
//                var custom = ((XElement)node["key"]).Attribute(TextConst.AName.Size) != null;
//                tl.SetNodeCheckState(node, (custom) ? CheckState.Checked : CheckState.Unchecked);
//            }
//            tl.EndUpdate();
//        }
//        void SetCheckedState(CheckState state, bool selected_only)
//        {
//            tl.BeginUpdate();
//            var nodes = (selected_only) ? tl.Selection.Cast<TreeListNode>() : tl.GetNodeList();
//            foreach (var node in nodes) tl.SetNodeCheckState(node, state);
//            tl.EndUpdate();
//        }
//        void SetTextVisible(bool visible)
//        {
//            tl.BeginUpdate();
//            var nodes = tl.Selection.Cast<TreeListNode>().ToArray();
//            foreach (var n in nodes.Where(n => TextConst.ENameArray.AllowTextMode.Contains(n["node_name"]))) n["text_visible"] = visible;
//            tl.EndUpdate();
//        }
//        void SetTextLocation(string location)
//        {
//            tl.BeginUpdate();
//            var nodes = tl.Selection.Cast<TreeListNode>().ToArray();
//            foreach (var n in nodes.Where(n => TextConst.ENameArray.AllowTextMode.Contains(n["node_name"]))) n["text_location"] = location;
//            tl.EndUpdate();
//        }

//        private void tl_CustomNodeCellEdit(object sender, DevExpress.XtraTreeList.GetCustomNodeCellEditEventArgs e)
//        {
//            if (e.Node is TreeListAutoFilterNode) return;

//            var node_name = e.Node["node_name"];
//            var is_field = TextConst.ENameArray.AllowTextMode.Contains(node_name);
//            var is_group = TextConst.ENameArray.AllowLayoutMode.Contains(node_name);

//            if (e.Column.FieldName == "width_perc")
//            {
//                e.RepositoryItem = rseWidthPerc;
//            }
//            else if (e.Column.FieldName == "width_fixed")
//            {
//                e.RepositoryItem = rseWidthFixed;
//            }
//            else if (e.Column.FieldName == "text_visible" && is_field)
//            {
//                e.RepositoryItem = rceTextVisible;
//            }
//            else if (e.Column.FieldName == "text_location" && is_field)
//            {
//                e.RepositoryItem = rleTextLocation;
//            }
//            else if (e.Column.FieldName == "layout_mode" && is_group)
//            {
//                e.RepositoryItem = rleLayoutMode;
//            }
//            else if (e.Column.FieldName == "cols_count" && is_group)
//            {
//                e.RepositoryItem = rbeCols;
//            }
//            else
//            {
//                e.RepositoryItem = rteEmpty;
//            }
//        }
//        private void tl_MouseClick(object sender, MouseEventArgs e)
//        {
//            if (e.Button == MouseButtons.Right) menu.ShowPopup(Cursor.Position);
//        }

//        private void btnSelectAll_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            SetCheckedState(CheckState.Checked, false);
//        }
//        private void btnUnselectAll_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            SetCheckedState(CheckState.Unchecked, false);
//        }
//        private void btnSelect_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            SetCheckedState(CheckState.Checked, true);
//        }
//        private void btnUnselect_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            SetCheckedState(CheckState.Unchecked, true);
//        }

//        private void btnCalculateWidthPerc_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            var nodes = tl.Selection.Cast<TreeListNode>().ToList();
//            var count = nodes.Count;
//            if (count == 0) return;
//            var width_perc = (int)(100 / count);
//            foreach (var node in nodes) {
//                node["width_perc"] = width_perc;
//            }
//        }
//        private void btnClearWidthPerc_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            var nodes = tl.Selection.Cast<TreeListNode>().ToList();
//            var count = nodes.Count;
//            if (count == 0) return;
//            foreach (var node in nodes) {
//                node["width_perc"] = DBNull.Value;
//            }
//        }
//        private void btnClearWidthFixed_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            var nodes = tl.Selection.Cast<TreeListNode>().ToList();
//            var count = nodes.Count;
//            if (count == 0) return;
//            foreach (var node in nodes) {
//                node["width_fixed"] = DBNull.Value;
//            }
//        }
//        private void btnShowText_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            SetTextVisible(true);
//        }
//        private void btnHideText_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            SetTextVisible(false);
//        }
//        private void btnTextTopLocation_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            SetTextLocation("top");
//        }
//        private void btnTextLeftLocation_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            SetTextLocation("left");
//        }

//        private void btnAccept_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            UpdateFieldsSettings();

//            DialogResult = DialogResult.OK;
//        }
//        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            DialogResult = DialogResult.Cancel;
//        }

//        private void rbeCols_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
//        {
//            // не доделал!!
//            var xitem = (XElement)tl.FocusedNode["key"];
//            _frmTableLayoutColumns.Initialize(xitem);
//            var result = _frmTableLayoutColumns.ShowDialog();
//            if (result != DialogResult.OK) return;

//            xitem.Elements(TextConst.EName.LayoutColumns).Remove();
//            var xlayoutcolumns = _frmTableLayoutColumns.GetLayoutColumns();
//            if (xlayoutcolumns != null) xitem.AddFirst(xlayoutcolumns);

//            tl.BeginUpdate();
//            tl.FocusedNode["cols_count"] = (xlayoutcolumns != null) ? xlayoutcolumns.Elements().Count() : 1;
//            tl.EndUpdate();
//        }
//    }
//}
