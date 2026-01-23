//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Drawing;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
////using System.Windows.Forms;
//using DevExpress.Spreadsheet;

//using System.Xml;
//using System.Xml.Linq;
//using sql.builder.DataApi;

//namespace sql.builder
//{
//    internal partial class ucExcelDataEditor : ucBase
//    {
//        public ucExcelDataEditor()
//        {
//            InitializeComponent();
           
//        }

//        public List<XElement> GetXml()
//        {
//            return SpreadsheetToXml.GetXml( spreadsheetControl1.Document);
//        }

//        public void SetSpreadsheetDataFromXml(List<XElement> xmlData)
//        {
//            SpreadsheetToXml.SetSpreadsheetData(spreadsheetControl1.Document, xmlData);
//        }

//        private static class SpreadsheetToXml // временное решение
//        {

//            public static void SetSpreadsheetData(IWorkbook wb,List<XElement> xmlData)
//            {
//                List<string> colNames = new List<string>();
//                List<string> colTypes = new List<string>();
//                List<string> colTitles = new List<string>();
//                var sh = wb.Worksheets[0];
//                foreach (VSXElement el in xmlData)
//                {
//                    if (el.P_Alias != "")
//                    {
//                        colNames.Add(el.XName);
//                        colTypes.Add(el.XDataType());
//                        colTitles.Add(el.P_Title);
                       
//                    }
//                    else
//                    {
//                        break;
//                    }
//                }

//                int ci=0;

//                foreach (var v in colNames)
//                {
//                    sh.Cells[0, ci].Value = colNames[ci];
//                    sh.Cells[1, ci].Value = colTypes[ci];
//                    sh.Cells[3, ci].Value = colTitles[ci];
//                    ci++;
//                }
//                ci = 0;
//                int ri =3;
//                foreach (VSXElement el in xmlData) {
//                    object val = Cmn.EvaluateOracleConst(el.Value);
//                    sh.Cells[ri, ci].SetValue(val);
//                    ci++;
//                    if (ci >= colNames.Count) {
//                        ci = 0;
//                        ri++;
//                    }
//                }
//            }
//            public static List<XElement> GetXml(IWorkbook wb)
//            {

//                var sh = wb.Worksheets[0];

              

//                bool ext = false;
//                List<string> colNames = new List<string>();
//                List<string> colTypes = new List<string>();
//                List<string> colTitles = new List<string>();
//                int ci=0;
//                while (true)
//                {
//                    if (!sh.Columns[ci].ExistingCells.Any()) break;
//                    colNames.Add(sh.GetCellValue(ci, 0).TextValue);
//                    colTypes.Add(sh.GetCellValue(ci, 1).TextValue);
//                    colTitles.Add(sh.GetCellValue(ci, 2).TextValue);
//                    ci++;
//                }
//                var res = new List<XElement>() ;
//                for (int ri = 3; ri < sh.GetDataRange().RowCount; ri++)
//                {

//                    for (int ci1 = 0; ci1 < ci; ci1++)
//                    {
//                        var xcnst = new XElement(TextConst.EName.Const);

//                        if (ri == 3)
//                        {
//                            xcnst.SetAttributeValue(TextConst.AName.As, colNames[ci1]);
//                            xcnst.SetAttributeValue(TextConst.AName.Type, colTypes[ci1]);
//                            xcnst.SetAttributeValue(TextConst.AName.Title, colTitles[ci1]);
//                        }
//                        object val=null;
//                        var cval = sh.GetCellValue(ci1,ri);
//                        switch (colTypes[ci1])
//                        {
//                            case TextConst.AVDataType.Number:
//                                val = cval.NumericValue;
//                                break;
//                            case TextConst.AVDataType.String:
//                                val = cval.TextValue;
//                                break;
//                            case TextConst.AVDataType.Date:
//                                val = cval.DateTimeValue;
//                                break;
//                        }
//                        var sval = Cmn.ToOracleString(val);
//                        xcnst.Value = sval;
//                        res.Add(xcnst);
//                    }
//                }

               
//                return res;
//            }
//        }

//        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            FindForm().DialogResult = DialogResult.OK;
//            FindForm().Close();
//        }

//        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            FindForm().DialogResult = DialogResult.Cancel;
//            FindForm().Close();
//        }
//    }

   

//}
