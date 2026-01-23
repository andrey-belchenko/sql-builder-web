//using System;
//using System.Collections.Concurrent;
//using System.Data;
//using System.Linq;
//using DevExpress.Utils;
//using DevExpress.XtraEditors.Controls;
//using DevExpress.XtraEditors.Repository;
//using DevExpress.XtraGrid;
//using DevExpress.XtraGrid.Views.Grid;

//using DevExpress.XtraTreeList;
//using sql.builder.DataApi;
//using sql.builder.UI;

//namespace sql.builder.Controls.Grids.ReportViewModes
//{
//    internal class RepositoryManager : IDisposable
//    {
//        GridControl _grid;
//        TreeList _tree;

//        ConcurrentDictionary<string, RepositoryItem> _repositoriesReadonly = new ConcurrentDictionary<string, RepositoryItem>();
//        ConcurrentDictionary<string, RepositoryItem> _repositoriesEditable = new ConcurrentDictionary<string, RepositoryItem>();

//        public RepositoryManager(GridControl grid)
//        {
//            _grid = grid;
//        }

//        public RepositoryManager(TreeList tree)
//        {
//            _tree = tree;
//        }

//        private static DataTable _checkTable;

//        private static DataTable checkTable()
//        {
//            if (_checkTable == null) {
//                DataTable dt = new DataTable();
//                dt.Columns.Add("val", typeof(decimal));
//                dt.Columns.Add("text", typeof(string));
//                dt.AddRow(Cmn.DECIMAL_ONE, "V");
//                dt.AddRow(Cmn.DECIMAL_ZERO, string.Empty);
//                dt.AddRow(DBNull.Value, string.Empty);
//                _checkTable = dt;
//            }
//            return _checkTable;
//        }
            

//        public RepositoryItem GetCellRepository(VDataTable vtable, string column_name, int row_index)
//        {
//            if (column_name == null) return null; // системная колонка с чекбоксом

//            RepositoryItem rep = null;

//            var key = column_name;

//            var vcol = (VDataColumn)vtable.Columns[column_name];

//            if (vcol == null) return null;// системная колонка с чекбоксом

//            if (!VDataColumn.HasBoundControl(vcol)) { // потом убрать это
//                return null; 
//            }
//            UIBase ctrl = vcol.BoundControls[0];

//            var row = vtable.Rows[row_index];
//            string buttonsVisibilityStr = "";

//            if (ctrl is UIText || ctrl is UINumber || ctrl is UIDate || ctrl is UIDateTime)
//            {
                
//                buttonsVisibilityStr = ctrl.GetButtonsVisibilityString(row);// для остальных реализовать при необходимости
//            }

//            key += buttonsVisibilityStr;
         
//            //key += row_index.ToString();
//            var read_only = !vcol.GetEditable(row);
//            //if (vcol.ColumnName == "vr_sprav_razdel_ip_vid")
//            //{

//            //}
//            vcol.GetVisibility(row);// чтобы зачистились невидимые значения
//            if (read_only)
//            {
//                _repositoriesReadonly.TryGetValue(key, out rep);
//            }
//            else
//            {
//                _repositoriesEditable.TryGetValue(key, out rep);
//            }

//            if (rep == null)
//            {
//                rep = ctrl.GetRepositoryItem();
//                var crep = rep as RepositoryItemCheckEdit;
//                if (crep != null)
//                {

//                }
//                if (crep != null && read_only)
//                {
//                    //crep.CheckStyle = CheckStyles.UserDefined;

//                    //crep.PictureChecked = Cmn.ImageCheck12;
//                    //rep = null;
//                   // rep=new RepositoryItemTextEdit();
//                    var erep = new RepositoryItemLookUpEdit();
//                    erep.ValueMember = "val";
//                    erep.DisplayMember = "text";
//                    erep.NullText = "";

//                    erep.DataSource = checkTable();
//                    erep.Buttons.Clear();
                    
//                    erep.ReadOnly = true;
//                    rep = erep;
//                    //rep.CustomDisplayText += rep_CustomDisplayText;
                   
//                    rep.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
//                    //erep.Appearance.Font = new System.Drawing.Font(erep.Appearance.Font, System.Drawing.FontStyle.Bold);
//                    //erep.Appearance.Options.UseFont = true;
//                    // это должно быть при инициализации грида,но там нет информации о том что колонка с UICheck
//                    if (_grid != null)
//                    {
//                        var gcol = (_grid.MainView as DevExpress.XtraGrid.Views.Grid.GridView) .Columns.ColumnByFieldName(vcol.ColumnName);
//                        if (gcol != null)
//                        {
//                            gcol.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//                        }
//                    }

//                    if (_tree != null)
//                    {
//                        var gcol = _tree.Columns.ColumnByFieldName(vcol.ColumnName);
//                        if (gcol != null)
//                        {
//                            gcol.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//                        }
//                    }
//                    //rep.Enabled = false;
                    
//                }

//                if (_grid != null)
//                {
//                    _grid.RepositoryItems.Add(rep);
//                    if(ctrl.ChangeSourceImmediately)
//                    {
//                        rep.EditValueChanged += (sender, args) =>
//                        {
//                            _grid.MainView.PostEditor();
//                        };
//                    }
//                }
//                else if (_tree != null)
//                {
//                    _tree.RepositoryItems.Add(rep);
//                    if (ctrl.ChangeSourceImmediately)
//                    {

//                        rep.EditValueChanged += (sender, args) => _tree.PostEditor();
//                    }
//                }

//                // чтобы время отображалось в ячейках без фокуса
//                if (ctrl is UIDateTime)
//                {
//                    if (_grid != null)
//                    {
//                        var view = _grid.MainView as GridView;
//                        var gcol = view.Columns[vcol.ColumnName];
//                        gcol.DisplayFormat.FormatType = FormatType.DateTime;
//                        gcol.DisplayFormat.FormatString = "g";
//                    }
//                    else if (_tree != null)
//                    {
//                        var gcol = _tree.Columns[vcol.ColumnName];
//                        gcol.Format.FormatType = FormatType.DateTime;
//                        gcol.Format.FormatString = "g";
//                    }
//                }
//                if (ctrl is UIDateTime)
//                {
                    
//                }
//                SetRepositoryState(rep, read_only, ctrl, buttonsVisibilityStr);

//                if (read_only)
//                {
//                    _repositoriesReadonly.TryAdd(key, rep);
//                }
//                else
//                {
//                    _repositoriesEditable.TryAdd(key, rep);
//                }
//            }

//            return rep;
//        }

        
//        public void SetRepositoryState(RepositoryItem rep, bool read_only, UIBase ctrl, string buttonsVisibilityStr)
//        {
//            // list, combobox
//            if (ctrl is UIList)
//            {
//                var edit = rep as RepositoryItemPopupContainerEdit;
//                // edit.ButtonsStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

//                edit.UseReadOnlyAppearance = read_only;

//                var statusButton = edit.Buttons.Cast<EditorButton>().FirstOrDefault(b => b.Tag is string && b.Tag.ToString() == "status");

//                foreach (var b in edit.Buttons.Cast<EditorButton>().Where(b => b != statusButton)) b.Visible = !read_only;
//            }
//            // file
//            else if (ctrl is UIFile)
//            {
//                var edit = rep as RepositoryItemButtonEdit;
//                edit.UseReadOnlyAppearance = read_only;
//                // в режиме readonly можно просматривать файлы

//                foreach (var b in edit.Buttons.Cast<EditorButton>().Where(b => b.Kind != ButtonPredefines.Search)) b.Visible = !read_only;
//            }
//            // check
//            else if (ctrl is UICheck)
//            {
//                var edit = rep as RepositoryItemCheckEdit;
//                rep.ReadOnly = read_only;
//                //edit.Enabled = !read_only;// не красиво
//            }
//            else if (ctrl is UIText || ctrl is UINumber || ctrl is UIDateTime || ctrl is UIDate)
//            {
//                rep.ReadOnly = read_only;

//                var edit = rep as RepositoryItemButtonEdit;
//                if (edit != null)
//                {
//                    // edit.Buttons.Cast<EditorButton>().ForEach(b => b.Visible = !read_only);
//                    int i = 0;
//                    foreach (var btn in edit.Buttons.Cast<EditorButton>())// для остальных реализовать при необходимости
//                    {
//                        btn.Visible = buttonsVisibilityStr[i] == '1';
//                        i++;
//                    }
//                }

//            }
//            else
//            {
//                var edit = rep as RepositoryItemButtonEdit;
//                foreach (var b in edit.Buttons.Cast<EditorButton>()) b.Visible = !read_only;
//                rep.ReadOnly = read_only;

//            }



//        }
//        public void Dispose()
//        {
//            _repositoriesEditable.Clear();
//            _repositoriesReadonly.Clear();
//        }
//    }
//}
