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
//using DevExpress.XtraGrid;
//using DevExpress.XtraGrid.Columns;
//using DevExpress.XtraGrid.Views.BandedGrid;
//using DevExpress.XtraGrid.Views.BandedGrid.ViewInfo;
//using DevExpress.XtraGrid.Views.Base;
//using DevExpress.XtraGrid.Views.Grid;
//using DevExpress.XtraGrid.Views.Grid.ViewInfo;
//using DevExpress.XtraPrinting;
//using infoenergo.core.Extensions;
//using sql.builder.DataApi;
//using sql.builder.DataApi.DataObjects;
//using sql.builder.UI;
//using sql.builder.WinForms;
//using sql.builder.XmlHelpers;
//using BandEventArgs = DevExpress.XtraGrid.Views.BandedGrid.BandEventArgs;
//using ShowButtonModeEnum = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum;
//using System.Text.RegularExpressions;

//namespace sql.builder.Controls.Grids.ReportViewModes
//{
//    internal partial class ucGridContainerWFUtils // временно выношу сюда куски которые сложно уложить в общую логику с интерфейсами 
//    {
        
//        public static void CompareDataSets(ucGridContainer obj,  VDataSet compared_ds)
//        {
//            var _source = obj.GetDataSource();
//            if (_source == null) return;
//            var grid = obj.GetGrid() as ucGridWF;
//            obj.BeginUpdate();
//            obj.DetachDataSourceEvents();
//            var dict = new Dictionary<string, string[]>();
//            foreach (DataTable table in _source.Tables) {
//                dict.Add(table.TableName, table.PrimaryKey.Select<DataColumn, string>(Cmn.GetDataColumnName));
//            }
//            // заплатка для сравенения через WCF
//            string caption2;
//            if (compared_ds.Scheme != null) {
//                caption2 = compared_ds.Scheme.Attribute(AName.timestamp).Value;
//            } else {
//                caption2 = "***";
//            }
//            GridDesigner.SetComparedGridView(grid, dict, _source.Scheme.Attribute(AName.timestamp).Value, caption2);

//            _source = GridDesigner.CompareDataSets(_source, compared_ds);
//            grid.DataSource = obj.GetTopTable();

//            // выводим колличество различий
//            var divergences_count = 0;
//            foreach (GridView view in grid.ViewCollection)
//            {
//                var result_columns = view.Columns.Where(col => col.FieldName.EndsWith("_result") && !String.IsNullOrEmpty(col.UnboundExpression));
//                for (int i = 0; i < view.RowCount; i++)
//                {
//                    divergences_count += result_columns.Count(result_column => (decimal)view.GetRowCellValue(i, result_column.FieldName) != 0M);
//                }
//            }

//            obj.SetFooterVisible(true);
//            obj.GetControlAsWFControl().lCompareResult.Visibility = BarItemVisibility.Always;
//            obj.GetControlAsWFControl().lCompareResult.Caption = "Расхождений всего: " + divergences_count;

//            obj.EndUpdate();

//            obj.IsCompareMode = true;
//        }

//        public static void SetComplexColumnsCaptions(ucGridContainer obj, VDataSet data_set)
//        {
//            var gr = (obj.GetGrid() as ucGridWF);
//            if (gr == null) return; 
//            var views = gr.ViewCollection.Cast<GridView>();
//            var tables = data_set.Tables.Cast<DataTable>();
//            // Маска для поиска в заголовке вхождения [a.id], где 
//            // a - идентификатор таблицы
//            // id - идентификатор колонки, имя которой должно быть подставлено
//            var regex = new Regex(@"\[[a-zA-Z0-9_]{1,30}\.[a-zA-Z0-9_]{1,30}\]");
//            // Отбираем все колонки со сложными заголовками
//            var parameterezed_columns = views
//                .SelectMany(v => v.Columns)
//                .Where(c => regex.IsMatch(c.Caption));

//            // Перебор колонок
//            foreach (var parameterezed_column in parameterezed_columns)
//            {
//                // Сохроняем оригинальный заголовок колонки
//                if (parameterezed_column.Tag == null)
//                {
//                    parameterezed_column.Tag = new Dictionary<string, string>();
//                }
//                ((Dictionary<string, string>)parameterezed_column.Tag).Add("original_title", parameterezed_column.Caption);

//                // Ищем вхождения маски для замены
//                foreach (Match match in regex.Matches(parameterezed_column.Caption))
//                {
//                    // Текст типа [a.id] для замены
//                    var text_replace = match.Value;
//                    // 0 - таблица; 1 - колонка
//                    var array = text_replace.Trim('[', ']').Split('.');
//                    // Ищем view по имени таблицы
//                    DataTable tbl = tables.FirstOrDefault(v => v.TableName == array[0]);
//                    //   var grid_view = views.FirstOrDefault(v => v.Name == array[0]);
//                    if (tbl == null) continue;
//                    // Ищем колонку в найденой view
//                    DataColumn col = tbl.Columns.Cast<DataColumn>().FirstOrDefault(c => c.ColumnName == array[1]);
//                    if (col == null) continue;
//                    // Подстовляем значение из первой строки в заголовок
//                    if (tbl.Rows.Count > 0)
//                    {
//                        parameterezed_column.Caption = parameterezed_column.Caption.Replace(text_replace, tbl.Rows[0][col].ToString());
//                    }
//                    else
//                    {
//                        parameterezed_column.Caption = parameterezed_column.Caption.Replace(text_replace, "...");
//                    }
//                }
//            }
//        }

    
//    }



   
//}
