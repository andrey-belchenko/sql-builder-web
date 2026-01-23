//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Diagnostics;
//using System.Drawing;
//using System.IO;
//using System.Linq;
////using System.Windows.Forms;
//using System.Xml.Linq;
//using Devart.Data.Oracle;
//using DevExpress.Data;
//using DevExpress.Utils;
//using DevExpress.XtraBars;
//using DevExpress.XtraBars.Controls;
//using DevExpress.XtraBars.Utils;
//using DevExpress.XtraEditors.Controls;
//using DevExpress.XtraEditors.Repository;
//using DevExpress.XtraTreeList;
//using DevExpress.XtraTreeList.Columns;
//using DevExpress.XtraTreeList.Nodes;
//using DevExpress.XtraTreeList.ViewInfo;
//using DevExpress.XtraTreeList.Data;
//using DevExpress.XtraPrinting;
//using infoenergo.core.Extensions;
//using sql.builder.DataApi;
//using sql.builder.DataApi.DataObjects;
//using sql.builder.UI;
//using sql.builder.WinForms;
//using sql.builder.XmlHelpers;
////using BandEventArgs = DevExpress.XtraGrid.Views.BandedGrid.BandEventArgs;
////using ShowButtonModeEnum = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum;

//namespace sql.builder.Controls.Grids.ReportViewModes
//{
//    internal partial class ucGridContainer :IGridContainer
//    {

//        #region TreeEvents

//        private bool treeEventsAttached = false;
//        private void AttachViewEvents_Tree()
//        {
//            if (treeEventsAttached) return;
//            treeEventsAttached = true;
//            tree.GetCustomSummaryValue += tree_GetCustomSummaryValue;
//            tree.CustomNodeCellEdit += tree_CustomNodeCellEdit;
//            tree.BeforeFocusNode += tree_BeforeFocusNode;
//            //tree.FocusedNodeChanged += tree_FocusedNodeChanged;
//            tree.ValidatingEditor += tree_ValidatingEditor;
        
//            tree.CustomDrawNodeIndicator += tree_CustomDrawNodeIndicator;
//            tree.CustomDrawNodeCell += tree_CustomDrawNodeCell;
//          //  tree.DoubleClick += tree_DoubleClick;
//            tree.MouseDown += tree_MouseDown;


//            tree.ColumnWidthChanged += tree_ColumnWidthChanged;
//            tree.BandWidthChanged += tree_BandWidthChanged;
//            tree.Resize += tree_Resize;

//            tree.DragDrop += tree_DragDrop;
//            tree.DragLeave += tree_DragLeave;
//            tree.DragOver += tree_DragOver;
//            tree.GiveFeedback += tree_GiveFeedback;
//        }


//        private void DettachViewEvents_Tree()
//        {
//            if (!treeEventsAttached) return;

//            treeEventsAttached = false;
//            tree.GetCustomSummaryValue -= tree_GetCustomSummaryValue;
//            tree.CustomNodeCellEdit -= tree_CustomNodeCellEdit;
//            tree.BeforeFocusNode -= tree_BeforeFocusNode;
//            //tree.FocusedNodeChanged -= tree_FocusedNodeChanged;
//            tree.ValidatingEditor -= tree_ValidatingEditor;

//            tree.CustomDrawNodeIndicator -= tree_CustomDrawNodeIndicator;
//            tree.CustomDrawNodeCell -= tree_CustomDrawNodeCell;
//            //  tree.DoubleClick -= tree_DoubleClick;
//            tree.MouseDown -= tree_MouseDown;


//            tree.ColumnWidthChanged -= tree_ColumnWidthChanged;
//            tree.BandWidthChanged -= tree_BandWidthChanged;
//            tree.Resize -= tree_Resize;

//            tree.DragDrop -= tree_DragDrop;
//            tree.DragLeave -= tree_DragLeave;
//            tree.DragOver -= tree_DragOver;
//            tree.GiveFeedback -= tree_GiveFeedback;
//        }

        
//        private void tree_MouseDown(object sender, MouseEventArgs args)
//        {
//            var hi = tree.CalcHitInfo(args.Location);

//            if (_multiselect)
//            {
//                if (args.Clicks == 1)
//                {
//                    if (args.Button == MouseButtons.Left)
//                    {
//                        tree.BeginSelection();
//                        if (hi.HitInfoType == HitInfoType.Column) {
//                            if (Control.ModifierKeys != Keys.Control) {
//                                tree.Selection.Clear();
//                            }
//                            var nodes = tree.GetNodeList().ToArray();
//                            if (nodes.Length != 0) {
//                                tree.SelectCells(nodes[0], hi.Column, nodes[nodes.Length - 1], hi.Column);
//                            }
//                        }
//                        tree.EndSelection();
//                    }
//                    else if (args.Button == MouseButtons.Right)
//                    {
//                        if (_menu == null || _menu.IsEmpty()) return;
//                        _menu.Show();
//                       // _menu.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
//                    }
//                }
//            }
//        }
//        //private void tree_DoubleClick(object sender, EventArgs e)
//        //{
//        //    Point pt = tree.PointToClient(Control.MousePosition);
//        //    TreeListHitInfo info = tree.CalcHitInfo(pt);

//        //    if (info.Node != null)
//        //    {
//        //        // из ucReferenceGrid
//        //        RaiseUIEvent2("", TextConst.AVEventName.DoubleClick, null, null);

//        //        if (tree.FocusedNode == null) return;

//        //        DataRow row =   GetNodeData(tree.FocusedNode);

//        //        VDataColumn col = null;
//        //        if (info.Column != null)
//        //        {
//        //            col = (VDataColumn)row.Table.Columns[info.Column.FieldName];
//        //        }

//        //        // из ucReportGrid
//        //        RaiseUIEvent2(_top_table_name, TextConst.AVEventName.DoubleClick, row, col);
//        //    }
//        //}
//        public DataRow GetNodeData(TreeListNode node)
//        {
//            DataRowView rowView = tree.GetDataRecordByNode(node) as DataRowView;
//            return (rowView != null) ? rowView.Row : null;
//        }

//        private bool IsEditorMode()
//        {
//            return _mode == ControlMode.Select || _mode == ControlMode.Data;
//        }
//        private void tree_CustomDrawNodeCell(object sender, CustomDrawNodeCellEventArgs args)
//        {
//            var row = GetNodeData(args.Node);
//            if (row == null) return;

//            // цвет текста
//            var table = row.Table as VDataTable;

//            // перенес из события RowCellStyle для грида с отчётами
//            var vcol = row.Table.Columns[args.Column.FieldName] as VDataColumn;
//            //if (vcol != null)
//            //{
//            var scolor = table.GetBackColor(row, vcol);
//            //var scolor = vcol.GetBackColor(row);

//            if (scolor != null)
//            {

//                //if (args.Node.Selected)
//                //{

//                //args.Appearance.BackColor = VColorUtils.GetSelectionColorFromRGBString(scolor, args.Appearance.BackColor);
//                //args.Appearance.ForeColor = VColorUtils.GetSelectionForeColor(args.Appearance.ForeColor);
//                //}
//                //else
//                //{
//                args.Appearance.BackColor = VColorUtils.GetColorFromRGBString(scolor);
//                //}
//            }
//            //}

//            if (IsEditorMode())
//            {
//                var colName = table.GetNameForText(args.Column.FieldName);
//                if (colName == null) return;

//                var column = table.Columns[colName] as VDataColumn;

//                // кешировать все значения - валидация уже

//                bool drawwarning = false;
//                bool drawedit = false;
//                RepositoryItem rep = null;

//                string rgb = column.GetFontColor(row);
//                Color color;
//                if (VColor.ParseRGB(rgb, out color)) {
//                    args.Appearance.ForeColor = color;
//                }

//                var msg = table.GetCellError(row, colName);

//                if (!string.IsNullOrEmpty(msg))
//                {
//                    drawwarning = true;
//                }

//                if (column.GetEditable(row) || column.HasAdditionalButtons()) // !!! проконтролировать время 
//                {
//                    var row_index = table.Rows.IndexOf(row); // !!! проконтролировать время
//                    rep = repositories().GetCellRepository(table, colName, row_index /*, true*/); // !!! проконтролировать время
//                    var isNoButtons = false;
//                    if (rep == null)
//                    {
//                        isNoButtons = true;
//                    }
//                    else
//                    {
//                        var repb = rep as RepositoryItemButtonEdit;
//                        if (repb != null)
//                        {
//                            isNoButtons = repb.Buttons.Count == 0;
//                        }
//                    }
//                    if (isNoButtons)
//                    {
//                        drawedit = true;
//                    }
//                }

//                if (drawwarning || drawedit)
//                {
//                    string text = args.CellText;

//                    if (drawwarning)
//                    {
//                        var s = "     ";
//                        if (text != "" && !text.StartsWith(s))
//                        {
//                            text = s + text;
//                        }
//                    }
//                    if (drawedit)
//                    {
//                        if (!(rep is RepositoryItemHyperLinkEdit))
//                        {
//                            var s = "    ";
//                            if (text != "" && !text.EndsWith(s))
//                            {
//                                text = text + s;
//                            }
//                        }
//                    }

//                    args.CellText = text;
//                    args.DefaultDraw();

//                    if (drawwarning)
//                    {
//                        if (rep == null)
//                        {
//                            var row_index = table.Rows.IndexOf(row); // !!! проконтролировать время
//                            rep = repositories().GetCellRepository(table, colName, row_index /*, true*/); // !!! проконтролировать время
//                        }
//                        var shift = Cmn.GetLeftButtonsSize(colName, rep);
//                        Image im = Cmn.ImageWarning14;
//                        args.Graphics.DrawImage(im, new Point(args.Bounds.Location.X + 1 + shift, args.Bounds.Location.Y + 2));
//                    }
//                    if (drawedit)
//                    {
//                        Image im = Cmn.ImageEdit12;
//                        args.Graphics.DrawImage(im, new Point(args.Bounds.Location.X + args.Bounds.Width - 16, args.Bounds.Location.Y + 1));
//                    }

//                    args.Handled = true;
//                }
//            }
//        }
//        //private void tree_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs args)
//        //{
//        //    GetTopTable().CurrentRow = GetNodeData(tree.FocusedNode);
//        //}
//        private void tree_BeforeFocusNode(object sender, BeforeFocusNodeEventArgs args)
//        {
//            if (args.Node == args.OldNode) return;
//            if (IsEditorMode())
//            {

//                if (!GetTopTable().IsChangeAccepting/*событие почему то срабатывает при акцепте*/ && GetTopTable().HasChildrenUserChanges())
//                {
//                    RaiseHasMessage(this, new HasMessageArgs() { Message = "Имеются несохранённые изменения в дочерних таблицах" });
//                    args.CanFocus = false;
//                    return;
//                }
//            }
//        }
//        private void tree_ValidatingEditor(object sender, BaseContainerValidateEditorEventArgs args)
//        {
//            if (IsEditorMode()) args.Valid = true;
//        }
//        private void tree_CustomDrawNodeIndicator(object sender, CustomDrawNodeIndicatorEventArgs args)
//        {
//            if (IsEditorMode())
//            {
//                if (args.IsNodeIndicator && args.Node != null)
//                {
//                    var table = GetTopTable();
//                    var row = GetNodeData(args.Node);

//                    if (row == null) return;

//                    if (table.IsRowDeleted(row)) args.ImageIndex = 4;
//                    else if (row.RowState == DataRowState.Added) args.ImageIndex = 2;
//                    else if (row.RowState == DataRowState.Modified) args.ImageIndex = 1;
//                    else args.ImageIndex = -1;

//                    // для выбранных строк красим индикатор в цвет самой строки
//                    if (args.Node.Selected && table.ClientBackColorSource != null)
//                    {
//                        var color = Cmn.GetHighlightColor();
//                        var backBrush = new SolidBrush(color);
//                        args.DefaultDraw();
//                        var bounds = new Rectangle(args.Bounds.X + 1, args.Bounds.Y + 1, args.Bounds.Width - 2, args.Bounds.Height - 2);


//                        args.Graphics.FillRectangle(backBrush, bounds);


//                        //ControlPaint.DrawBorder3D(args.Graphics, args.Bounds, Border3DStyle.RaisedInner);

//                        if (args.ImageIndex > -1)
//                        {
//                            Image indImage = tree.Painter.IndicatorImages.Images[args.ImageIndex];
//                            int imageLeft = args.Bounds.Left + (args.Bounds.Width - indImage.Width) / 2;
//                            int imageTop = args.Bounds.Top + (args.Bounds.Height - indImage.Height) / 2;
//                            args.Cache.Graphics.DrawImage(indImage, new Point(imageLeft, imageTop));
//                        }

//                        args.Handled = true;
//                    }
//                }
//            }
//        }
//        private void tree_CustomNodeCellEdit(object sender, GetCustomNodeCellEditEventArgs args)
//        {
//            if (IsEditorMode())
//            {
//                var table = GetTopTable();
//                //  if (table.IsManualDeleteIgnore()) return; // почему то виснет на удалении многократно выполняется этот обработчик
//                var column_name = args.Column.FieldName;
//                column_name = table.GetNameForText(column_name);


//                //var column_name = args.Column.FieldName.Replace(TextConst.Pfx.ExtValName, "");
//                var row = GetNodeData(args.Node);
//                if (row == null) return;
//                var row_index = table.Rows.IndexOf(row);

//                RepositoryItem rep = repositories().GetCellRepository(table, column_name, row_index);
//                args.RepositoryItem = rep;
//            }
//        }
//        private void tree_GetCustomSummaryValue(object sender, GetCustomSummaryValueEventArgs args)
//        {
//            // не работает
//        }
//        private void tree_ColumnWidthChanged(object sender, ColumnChangedEventArgs args)
//        {
//            ProcessColWidthChange(tree, args.Column.Name);
            
//        }
//        private void tree_BandWidthChanged(object sender, BandEventArgs args)
//        {
//            ProcessColWidthChange(tree, null);
//        }
//        private void tree_Resize(object sender, EventArgs args)
//        {
//            ProcessColWidthChange(tree, null);
//        }

//        private void tree_GiveFeedback(object sender, GiveFeedbackEventArgs args)
//        {
//            args.UseDefaultCursors = false;
//        }
//        private void tree_DragOver(object sender, DragEventArgs args)
//        {
//            DXDragEventArgs args2 = tree.GetDXDragEventArgs(args);
//            if (args2.Node == null)
//            {
//                if (args2.HitInfo.HitInfoType == HitInfoType.Empty || args2.TargetNode != null)
//                {
//                    args2.Effect = DragDropEffects.Copy;
//                }
//                else
//                {
//                    args2.Effect = DragDropEffects.None;
//                }
//            }

//            if (args2.Effect == DragDropEffects.Move) tree.Cursor = new Cursor(Properties.Resources.move.Handle);
//            else if (args2.Effect == DragDropEffects.Copy) tree.Cursor = new Cursor(Properties.Resources.copy.Handle);
//            else if (args2.Effect == DragDropEffects.None) tree.Cursor = Cursors.No;
//        }
//        private void tree_DragLeave(object sender, EventArgs args)
//        {
//            tree.Cursor = Cursors.Default;
//        }

//        TreeListNode before = null;
//        TreeListNode after = null;
//        TreeListNode parent = null;
//        private void tree_DragDrop(object sender, DragEventArgs args)
//        {
//            DXDragEventArgs args2 = tree.GetDXDragEventArgs(args);

//            if (args2.Node != null)
//            {


//                if (args2.DragInsertPosition == DragInsertPosition.Before)
//                {
//                    before = args2.TargetNode;
//                }
//                else if (args2.DragInsertPosition == DragInsertPosition.After)
//                {
//                    after = args2.TargetNode;

//                }
//                else if (args2.DragInsertPosition == DragInsertPosition.AsChild)
//                {
//                    parent = args2.TargetNode;
//                }

//                tree.SetNodeIndex(args2.Node, tree.GetNodeIndex(args2.TargetNode));
//            }

//            tree.Cursor = Cursors.Default;
//        }
//        //private void tree_AfterDropNode(object sender, AfterDropNodeEventArgs args)
//        //{
//        //    if (!args.IsSuccess) return;

//        //    //tree.BeginUpdate();
//        //    //PreventTreeRebuild();

//        //    if (before != null)
//        //    {
//        //        args.Node[_order_field_name] = before[_order_field_name];
//        //        MoveNodesDownInParent(before);

//        //        before = null;
//        //    }
//        //    else if (after != null)
//        //    {
//        //        if (after.NextNode != null)
//        //        {
//        //            args.Node[_order_field_name] = after.NextNode[_order_field_name];
//        //            MoveNodesDownInParent(after.NextNode);
//        //        }
//        //        else
//        //        {
//        //            args.Node[_order_field_name] = (decimal)after[_order_field_name] + 1M;
//        //        }

//        //        after = null;
//        //    }
//        //    else if (parent != null)
//        //    {
//        //        if (parent.HasChildren)
//        //        {
//        //            args.Node[_order_field_name] = (decimal)parent.Nodes.LastNode[_order_field_name] + 1M;
//        //        }
//        //        else
//        //        {
//        //            args.Node[_order_field_name] = 1M;
//        //        }
//        //        parent = null;
//        //    }

//        //    //ResumeTreeRebuild();
//        //    //tree.EndUpdate();
//        //}
//        #endregion

    
//    }



    
//}
