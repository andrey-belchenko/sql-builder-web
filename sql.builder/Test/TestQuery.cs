//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Diagnostics;
//using System.IO;
//using System.Linq;
//using System.Text;
////using System.Windows.Forms;
//using System.Xml;
//using System.Xml.Linq;
//using sql.builder.Test;
//using Devart.Data.Oracle;
////using DevExpress.LookAndFeel;
////using DevExpress.Spreadsheet;
////using DevExpress.XtraEditors;

//using infoenergo.core.Data.Tables;
//using infoenergo.ui.win;
//using sql.builder.DataApi;
//using sql.builder.Properties;
//using sql.builder.XmlHelpers;
//using reports.word.XmlPrint;
//using sql.builder.TFS;
//using Excel = Microsoft.Office.Interop.Excel;
//using Word = Microsoft.Office.Interop.Word;
//using sql.builder.UI;
//namespace sql.builder
//{
//    internal partial class TestQuery : XtraForm
//    {

//        private static class MpedDoc
//        {

//            static DataSet ds = null;
//            static SortedList<string, string> epkgNames = null;
//            static string getColTitInfo(VSXElement se)
//            {
//                var info = "";

//                if (se.P_Title != "" && se.P_Title!="-")
//                {
//                    info = se.P_Title;
//                    var par = se.GetParent();
//                    while (par != null && !(par is VSelect) && !(par is VForm) && !(par is VQuery))
//                    {
//                        var tit = par.P_Title;
//                        if (tit == "-")
//                        {
//                            tit = "";
//                        }
//                        if (tit != "")
//                        {
//                            info = tit + " / " + info;
//                        }
//                        par = par.GetParent();
//                    }
//                }

//                return info;
//            }
//            static string getColSrcInfo(VSXElement se, bool we=true)
//            {
//                var src = "";
//                if (se is VFact)
//                {
                    
//                    var exp = (se as VFact).GetFactSource() as VExpression;
//                    if (exp != null)
//                    {
//                        var epc = exp.GetParent().P_Comment;
//                        var epid = epkgNames[epc];


//                        src = epid + "." + se.P_Column;

//                    }
//                    else
//                    {
//                        var sc = (se as VFact).GetFactSource();
//                        src = sc.GetMainParent().P_IdName + "." + se.P_Column;
//                    }

//                }
//                else if (se is VColumn)
//                {
//                    src = se.P_Table + "." + se.P_Column;
//                }
//                else if (se is VCall || se is VExpression)
//                {
//                    var q = "";
//                    if (we)
//                    {
//                        src = "выражение(";
//                    }
//                    List<string> colNames = new List<string>();
//                    var cols = se.GetDescedantsP(EPredicate.IsColumnOrFact);
//                    foreach (VColumn col in cols.ToList())
//                    {
//                        if (col is VFact)
//                        {
//                            var cs = (col as VFact).GetConditionSource();
//                            if (cs != null)
//                            {
//                                var cols1 = cs.GetDescedantsP(EPredicate.IsColumnOrFact);
//                                cols.AddRange(cols1);
//                            }
                           
//                        }
//                    }
//                    foreach (VColumn col in cols.Distinct().ToList())
//                    {
//                        if (col is VFact)
//                        {
//                           var cs= (col as VFact).GetConditionSource();

//                        }
//                        var nm =getColSrcInfo(col);
//                        if (!colNames.Contains(nm))
//                        {
//                            colNames.Add(nm);
//                        }
//                    }

//                    //foreach (VSXElement col in se.GetDescedantsApplyingParts(TextConst.EName.Const))
//                    //{

//                    //    var nm = col.Value;
//                    //    if (!colNames.Contains(nm))
//                    //    {
//                    //        colNames.Add(nm);
//                    //    }
//                    //}
//                    foreach (var nm in colNames.Distinct().ToArray())
//                    {


//                        src += q + nm;
//                        q = ", ";
//                    }
//                    if (we)
//                    {
//                        src += ")";
//                    }

//                }
//                return src;
//            }
//            public static void MpedList()
//            {


//                ds = new DataSet();
//                epkgNames = new SortedList<string, string>();
//                var tblObj = new DataTable("o");
//                tblObj.Columns.Add("id");
//                tblObj.Columns.Add("tip");
//                tblObj.Columns.Add("tip_info");
//                tblObj.Columns.Add("name");
//                tblObj.Columns.Add("info");
//                tblObj.Columns.Add("el");
//                tblObj.Columns["el"].DataType = typeof(VSXElement);
//                XmlReports.Environment.Manager.LoadProjectIfNeed("mped");
//                var objs = XmlReports.Environment.GetElements().Where(e => e.GetProjectName() == "mped" && e.P_Comment != "").Distinct().OrderBy(e1=>e1.Name.LocalName+" "+e1.P_IdName).ToList();
//                var objs1 = objs.Elements(TextConst.EName.Expressions).ToList().SelectAsArray(VSXElement.Get).Where(e1 => e1.P_Comment != "").ToList();
//                objs.AddRange(objs1);
//                var ots = new List<string>();
//                var i = 11;
//                var en = "exp_pkg";
//                foreach (VSXElement obj in objs)
//                {

                    
//                    var tip = obj.Name.LocalName;
//                    var name = obj.P_IdName;
//                    if (tip == TextConst.EName.Expressions)
//                    {
//                        tip = "expression package";
//                        name = en + i.ToString();
//                        epkgNames.Add(obj.P_Comment, name);
//                        i++;
//                    }
//                    if (obj.P_IsReport == TextConst.AVBool.True)
//                    {
//                        tip = TextConst.EName.Report;
//                    }
                  
//                    var r = tblObj.NewRow();
//                    r["tip"] =tip;
//                    r["name"] = name;
//                    r["info"] = obj.P_Comment;
//                    r["el"] = obj;
//                    tblObj.Rows.Add(r);
//                    var comment = obj.P_Comment;
//                    var obj1 = obj;
//                    if (tip == TextConst.EName.Query || tip==TextConst.EName.Report)
//                    {
//                        bool add = false;
//                        var name1 = obj.P_IdName;
//                        if (obj.AsXElementApplyingParts().Descendants(EName.table).Any(t => !t.Attributes(TextConst.AName.View).Any()))
//                        {
//                            tip = TextConst.EName.Table;
//                            name1 = obj.P_IdName;
//                            add = true;
//                        }
//                        else if (obj.P_Stored!="")
//                        {
//                            name1 = obj.P_Stored;
//                            tip = "materialized view";
//                            add = true;
//                        }
//                        else if (obj.GetElementsP(EName.print_templates).Count != 0) {
//                            obj1 = obj.GetElementsP(EName.print_templates).First()
//                                .GetElementsP(EName.excel).First().GetElementsP().First();
//                            name1 = obj1.P_Name;
//                            tip = "excel template";
//                            comment = obj.P_Comment;
//                            add = true;
                           
//                        }

//                        if (add)
//                        {

                           
//                            var r1 = tblObj.NewRow();
//                            r1["tip"] = tip;
//                            r1["name"] = name1;
//                            r1["info"] = comment;
//                            r1["el"] = obj1;
//                            tblObj.Rows.Add(r1);
//                        }


//                    }
//                }


//                var tblObjType = new DataTable("ot");
//                tblObjType.Columns.Add("name");
//                tblObjType.Columns.Add("info");

//                 i = 0;
//                foreach (DataRow ro in tblObj.AsEnumerable())
//                {
                 
                    
                    
//                    var info = "";
//                    DataRow r = null;
//                    if (!ots.Contains(ro["tip"].ToString()))
//                    {
                      
//                         r = tblObjType.NewRow();
//                        r["name"] = ro["tip"];
//                    }
                    
//                    switch (ro["tip"].ToString())
//                    {
//                        case TextConst.EName.Query:
//                            info = "Метаданные для построениия SQL запроса";
//                            break;
//                        case TextConst.EName.Form:
//                            info = "Метаданные формы пользовательского интерфейса";
//                            break;
//                        case TextConst.EName.Field:
//                            info = "Метаданные поля для установки значения (фильтры, условия)";
//                            break;
//                        case TextConst.EName.Report:
//                            info = "Метаданные отчета";
//                            break;
//                        case TextConst.EName.Table:
//                            info = "Таблица в базе данных";
//                            break;
//                        case "materialized view":
//                            info = "Материализованное представление в базе данных";
//                            break;
//                        case "expression package":
//                            info = "Пакет выражений, метаданные для построения SQL запросов";
//                            break;
//                        case "excel template":
//                            info = "Шаблон для выгрузки данных в excel";
//                            break;
                           
//                    }
                    
//                    ro["tip_info"] = info;
//                    var id = ro["tip"].ToString() + "-" + ro["name"].ToString();
                    
                    
//                    ro["id"] = id;
//                    if (!ots.Contains(ro["tip"].ToString()))
//                    {
//                        ots.Add(ro["tip"].ToString());
//                        r["info"] = info;
//                        tblObjType.Rows.Add(r);
//                    }
                  
//                }

               


//                ds.Tables.Add(tblObj);
//                ds.Tables.Add(tblObjType);

//                var tblObjLinkType = new DataTable("lt");
//                tblObjLinkType.Columns.Add("id");
//                tblObjLinkType.Columns.Add("obj");
//                tblObjLinkType.Columns.Add("name");

//                ds.Tables.Add(tblObjLinkType);
//                tblObjLinkType.ParentRelations.Add(new DataRelation("", tblObj.Columns["id"], tblObjLinkType.Columns["obj"]));
//                foreach (DataRow r in tblObj.AsEnumerable())
//                {
//                    var names = new List<string>();
//                    var el = (VSXElement)r["el"];
//                    var elId=r["id"].ToString();
//                    if (el.GetElementsP(EName.print_templates).Count != 0) {
//                       var tt = el.GetElementsP(EName.print_templates).First()
//                            .GetElementsP(EName.excel).First().GetElementsP().First();
//                       var name1 = tt.P_Name;
//                       addSubElProp(elId, "tmpl", elId, "val", name1);
//                    }
//                    if ((string)r["tip"] == "expression package") {
//                        foreach (var se in VSXElement.GetElementsP(el).Where(e => e.P_Group != ""))
//                        {
//                            var subElId = se.XName;
//                            addSubElProp(elId, "fact", subElId, "name", se.XName);
//                            addSubElProp(elId, "fact", subElId, "info", se.P_Title);
//                            addSubElProp(elId, "fact", subElId, "dtip", se.XDataType());
//                            addSubElProp(elId, "fact", subElId, "src", getColSrcInfo(se));
//                        }
//                    }

//                    if (el is VField)
//                    {
//                        addSubElProp(elId, "ctrl", elId, "val",el.P_ControlType);
//                        addSubElProp(elId, "dtp", elId, "val", el.XDataType());
//                        var lq = el.GetElementsP(EName.listquery).FirstOrDefault();
//                        if (lq != null)
//                        {
//                            lq = VSXElement.GetElementsP(lq).FirstOrDefault();
//                            addSubElProp(elId, "lqry", elId, "val", lq.P_CalledQuery);
//                        }
//                        lq = el.GetElementsP(EName.defaultquery).FirstOrDefault();
//                        if (lq != null)
//                        {
//                            lq = VSXElement.GetElementsP(lq).FirstOrDefault();
//                            addSubElProp(elId, "dqry", elId, "val", lq.P_CalledQuery);
//                        }
//                        addSubElProp(elId, "ctrl", elId, "val", el.P_ControlType);
//                    }

//                    foreach (VSXElement par in el.GetElementsP(EName.@params).SelectMany(VSXElement.GetElementsP))
//                    {
//                        addSubElProp(elId, "pars", par.XName, "name", par.XName);
//                        addSubElProp(elId, "pars", par.XName, "tip", par.XDataType());
//                    }
//                    if (el is VQuery)
//                    {
//                        foreach (VSXElement uf in el.GetDescedantsP(EName.usefield))
//                        {
//                            addSubElProp(elId, "pars", uf.P_FormalParNameS, "name", uf.P_FormalParNameS);
//                            addSubElProp(elId, "pars", uf.P_FormalParNameS, "tip", uf.XDataType());

//                        }
//                    }

//                    foreach (VSXElement uf in el.GetDescedantsP(EName.usefield))
//                    {
//                        if (uf.Field().P_Comment != "")
//                        {
//                            addSubElProp(elId, "uf", uf.P_FormalParNameS, "name", uf.P_FormalParNameS);
//                            addSubElProp(elId, "uf", uf.P_FormalParNameS, "fld", uf.Field().P_IdName);
//                            addSubElProp(elId, "uf", uf.P_FormalParNameS, "info", getColTitInfo(uf));
//                        }
//                    }

//                    if (r["tip"].ToString() == TextConst.EName.Query || r["tip"].ToString() == TextConst.EName.Report || r["tip"].ToString() == TextConst.EName.Table || r["tip"].ToString() == TextConst.EName.Form)
//                    {

//                        var epkg = new SortedList<string, VSXElement>();

//                        List<VSXElement> cols1 = null;
//                        var f = "";
//                        if (el is VForm)
//                        {
//                            f = "f";
//                            cols1 = el.GetElementsP(EName.content).SelectMany(e => e.GetDescedantsP(EPredicate.IsColumnOrFact)).ToList();
//                        }
//                        else
//                        {
//                            cols1 = (el as VSourcedElement).Columns();
//                            if (cols1.Count == 0 && VSourcedElement.GetSelfSelectSections((VSourcedElement)el).Count == 0) {
//                                cols1 = el.GetElementsP(EName.@const).Where(e => e.P_Alias != "").ToList();
//                            }
//                        }
//                        cols1 = cols1.Where(c => !TextConst.AVColumnArray.SysColNamesForEditedObject.Contains(c.XName)).ToList();

//                        if (el is VQuery && (el as VQuery).GetDimension() != null && r["tip"].ToString() != TextConst.EName.Table)
//                        {
//                            addSubElProp(elId, "dim", elId, "val", (el as VQuery).GetDimension().P_Name);
//                        }
                        
                        
//                        foreach (var se in cols1)
//                        {

//                            var subElId =se.P_Table+"."+ se.XName;
//                            if (se is VFact)
//                            {
//                                var exp = (se as VFact).GetFactSource() as VExpression;
//                                if (exp != null)
//                                {
//                                    var epc = exp.GetParent().P_Comment;
//                                    var epid = epkgNames[epc];

//                                    if (!epkg.ContainsKey(epid))
//                                    {
//                                        epkg.Add(epid, exp.GetParent());
//                                    }

//                                }
//                            }



//                            var src = "";
//                            if (r["tip"].ToString() != TextConst.EName.Table)
//                            {
//                                src = getColSrcInfo(se);
//                            }

//                            if (r["tip"].ToString() != TextConst.EName.Table || (se is VColumn))
//                            {
//                                addSubElProp(elId,f+ "col", subElId, "name", se.XName);
//                                addSubElProp(elId, f + "col", subElId, "info", getColTitInfo(se));
//                                addSubElProp(elId, f + "col", subElId, "dtip", se.XDataType());
//                                addSubElProp(elId, f + "col", subElId, "src", src);
//                            }
//                            if (r["tip"].ToString() != TextConst.EName.Table && se.P_Fact != "")
//                            {
//                                addSubElProp(elId, f + "col", subElId, "fact", se.P_Fact);
//                            }
//                            if (el is VForm)
//                            {
//                                if (se.P_EditableResult != "query:0")
//                                {
                                    
//                                }
//                            }


//                        }
//                        foreach (var se in epkg)
//                        {
//                            addSubElProp(elId, "srcf", se.Key, "name", se.Key);
                          
//                        }
//                        if (r["tip"].ToString() != TextConst.EName.Table)
//                        {
                            
//                            foreach (var se in (el as VSourcedElement).AllSources())
//                            {
//                                var subElId = se.XName;
//                                var scols = "";
//                                if (se.P_Join == TextConst.AVJoin.LeftOuter)
//                                {
//                                    addSubElProp(elId, "link", subElId, "name", subElId);
//                                    addSubElProp(elId, "link", subElId, "src", se.P_CalledQuery);
//                                    var cols = se.GetElementsP(EName.call).SelectMany(VSXElement.GetDescedantsP).ToList();
//                                    var q = "";
//                                    foreach (var col in cols)
//                                    {
//                                        var colInf = "";
//                                        if (col is VColumn)
//                                        {
//                                            var alias = col.P_Table;
//                                            var ms = (el as VSourcedElement).MainSource();
//                                            if (ms != null && alias == ms.XName)
//                                            {
//                                                alias = "this";
//                                            }
//                                            colInf = alias + "." + col.P_Column;
//                                        }
//                                        else if (col is VConst || col is VArray)
//                                        {
//                                            colInf += col.Value;
//                                        }
//                                        if (colInf != "")
//                                        {
//                                            scols += q + colInf;
//                                            q = ", ";
//                                        }
//                                    }
//                                    addSubElProp(elId, "link", subElId, "fld", scols);
//                                }
                                
//                                bool isDim = false;

//                                //bool isUsed = se.UsedColumns().Any(e => e.GetAncestorsAndSelf(new string[] { TextConst.EName.Select, TextConst.EName.Where }).Count != 0);
//                                bool isUsed = se.UsedColumns().Any(e => e.GetAncestorsAndSelf().Any(e1 => (e1.Name == EName.select) || (e1.Name == EName.where)));
//                                if (el is VForm) isUsed = true;
//                                if (!isUsed) continue;
//                                if (se.GetParent() is VQube || se.GetParent() is VDimSet)
//                                {
//                                    isDim = true;
//                                }
//                                if (se is VTable)
//                                {
//                                    addSubElProp(elId, "srct", subElId, "as", se.XName);
//                                    addSubElProp(elId, "srct", subElId, "name", se.P_CalledQuery);
//                                }
//                                else if (se.P_CalledQuery != "")
//                                {

//                                    {
//                                        if (isDim)
//                                        {
//                                            addSubElProp(elId, "srcd", subElId, "as", se.XName);
//                                            addSubElProp(elId, "srcd", subElId, "name", se.P_CalledQuery);
//                                        }
//                                        else
//                                        {
//                                            addSubElProp(elId, "srcq", subElId, "as", se.XName);
//                                            addSubElProp(elId, "srcq", subElId, "name", se.P_CalledQuery);
//                                            var wp = se.GetElementsP(EName.withparams).FirstOrDefault();
//                                            var spars = "";
//                                            if (wp != null)
//                                            {
//                                                var factPars = VSXElement.GetElementsP(wp);

//                                                var q = "";

//                                                foreach (var p in factPars)
//                                                {
//                                                    var pInfo = "";
//                                                    if (p is VUseParam)
//                                                    {
//                                                        pInfo = ":" + p.P_Name;
//                                                    }
//                                                    else if (p is VConst || p is VArray)
//                                                    {
//                                                        pInfo = p.Value;
//                                                    }
//                                                    else if (p is VColumn)
//                                                    {
//                                                        pInfo = p.P_Table + p.P_Column;
//                                                    }
//                                                    spars += q + pInfo;
//                                                    q = ", ";
//                                                }

//                                            }
//                                            addSubElProp(elId, "srcq", subElId, "pars", spars);

//                                        }
//                                    }
                                    

//                                }
//                            }
//                        }


//                    }

//                }
               
//                string fullPath = Printing.PrintWord(ds, "mped_list.docx", "Список объектов");

//                Cmn.OpenPrintedFile(fullPath);

//            }


//            public static void addSubElProp(string elId, string subElType,string subElId, string propName, string propVal)
//            {
//                var tMainName = subElType;

                

//                var tName = "r" + subElType;
//                DataTable tmain = ds.Tables[tMainName];
//                DataTable tsub = ds.Tables[tName];
//                DataTable tobj = ds.Tables["o"];
//                if (tmain == null)
//                {
//                    tmain = new DataTable(tMainName);
//                    ds.Tables.Add(tmain);
//                    var idcol1= tmain.Columns.Add("id");
//                    tmain.Columns.Add("obj");
//                    tmain.ParentRelations.Add(new DataRelation("", tobj.Columns["id"], tmain.Columns["obj"]));
//                    tmain.PrimaryKey = new DataColumn[] { idcol1 };
                  
//                    tsub = new DataTable(tName);
//                    ds.Tables.Add(tsub);
//                    var idcol = tsub.Columns.Add("id");
//                    tsub.Columns.Add("obj");
//                    tsub.PrimaryKey = new DataColumn[] { idcol };
//                    tsub.ParentRelations.Add(new DataRelation("", tmain.Columns["id"], tsub.Columns["obj"]));
//                }
//                var r = tmain.Rows.Find(elId);

//                if (r == null)
//                {
//                    r = tmain.NewRow();
//                    r["id"] = elId;
//                    r["obj"] = elId;
//                    tmain.Rows.Add(r);
//                }

//                DataColumn col = tsub.Columns[propName];
//                if (col == null)
//                {
//                    tsub.Columns.Add(propName);
//                }

//                var subId = elId + "-" + subElId;

//                var sr = tsub.Rows.Find(subId);

//                if (sr == null)
//                {
//                    sr = tsub.NewRow();
//                    sr["id"] = subId;
//                    sr["obj"] = elId;
//                    tsub.Rows.Add(sr);
//                }
//                sr[propName] = propVal;
//            }
//        }


//        public TestQuery()
//        {
//            InitializeComponent();

//            //string skinName;
//            //if (Settings.Default.skinName.Length > 2)
//            //{
//            //    skinName = Settings.Default.skinName;
//            //}
//            //else
//            //{
//            //    skinName = "Office 2010 Silver";
//            //}

//            //UserLookAndFeel.Default.SetSkinStyle(skinName);
//            //Settings.Default.skinName = skinName;
//            //Settings.Default.Save();
//        }

//        private void TestQuery_Load(object sender, EventArgs e)
//        {
//            refresh();
//        }

//        public void refresh()
//        {
//            XmlReports.Init();

//            var dt_queries = new DataTable();
//            dt_queries.Columns.Add(new DataColumn("name"));

//            foreach (XElement query in XmlReports.Environment.Manager.GetScheme().Elements("queries").Elements("query"))
//            {
//                dt_queries.Rows.Add(query.Attribute("name").Value);
//            }

//            foreach (XElement query in XmlReports.Environment.Manager.GetOldScheme().Elements("queries").Elements("query"))
//            {
//                dt_queries.Rows.Add(query.Attribute("name").Value);
//            }

//            lookUpEdit1.Properties.DataSource = dt_queries;
//            lookUpEdit1.Properties.ValueMember = dt_queries.Columns[0].ColumnName;
//            lookUpEdit1.Properties.DisplayMember = dt_queries.Columns[0].ColumnName;
//            lookUpEdit1.EditValue = Settings.Default.testQueryS1;
//            textEdit1.EditValue = Settings.Default.testQueryS2;

//            var dt_reports = new DataTable();
//            dt_reports.Columns.Add(new DataColumn("name"));

//            foreach (XElement report in XmlReports.Environment.Manager.GetScheme().Elements("reports").Elements("report"))
//            {
//                dt_reports.Rows.Add(report.Attribute("name").Value);
//            }

//            foreach (XElement report in XmlReports.Environment.Manager.GetOldScheme().Elements("reports").Elements("report"))
//            {
//                dt_reports.Rows.Add(report.Attribute("name").Value);
//            }

//            lookUpEdit2.Properties.DataSource = dt_reports;
//            lookUpEdit2.Properties.ValueMember = dt_reports.Columns[0].ColumnName;
//            lookUpEdit2.Properties.DisplayMember = dt_reports.Columns[0].ColumnName;
//            lookUpEdit2.EditValue = Settings.Default.testQueryS3;
//        }

//        private void lookUpEdit1_EditValueChanged(object sender, EventArgs e)
//        {
//            Settings.Default.testQueryS1 = lookUpEdit1.EditValue.ToString();
//            Settings.Default.Save();
//        }

//        private void textEdit1_EditValueChanged(object sender, EventArgs e)
//        {
//            Settings.Default.testQueryS2 = textEdit1.EditValue.ToString();
//            Settings.Default.Save();
//        }

//        private void simpleButton1_Click(object sender, EventArgs e)
//        {
//            var qname = lookUpEdit1.EditValue.ToString();
//            WaitUIHelper.LastUsedUIHelper.Show(String.Format("Генерация скрипта {0}", qname), WaitUIMode.WaitPanel);
//            //Wait.Show(String.Format("Генерация скрипта {0}", qname), form: this);
//            XmlReports.Init();

//            var sql = Compiler.GetSql(XmlReports.getItemProcessedXml2("query", qname, false));

//            // В.Емцов - так гарантированно освобождаются занятые ресурсы
//            using (var sw = new StreamWriter(new FileStream(textEdit1.EditValue.ToString(), FileMode.Create), Encoding.GetEncoding(1251)))
//            {
//                sw.Write(Cmn.ClearUndefined(sql));
//            }
//            WaitUIHelper.LastUsedUIHelper.Hide();
//            //Wait.Hide();
//            Process.Start(textEdit1.EditValue.ToString());
//        }

//        private void simpleButton2_Click(object sender, EventArgs e)
//        {

//            sql.builder.SqlBuilder.PreviewReport(report_name: "41050-current", auto_execute: true);
//        }

//        private void lookUpEdit2_EditValueChanged(object sender, EventArgs e)
//        {
//            Settings.Default.testQueryS1 = lookUpEdit1.EditValue.ToString();
//            Settings.Default.testQueryS3 = lookUpEdit2.EditValue.ToString();
//            Settings.Default.Save();
//        }

//        private void simpleButton3_Click_1(object sender, EventArgs e)
//        {
//            // 30.10.14 В.Емцов
//            // Для Ленэнерго схема PLAN
//            //var db_scheme = DataHelper.SqlGetDecimal("select customer_id from rs_rep_sets", XmlReports.CurrentConnection) == 17M
//            //    ? "PLAN"
//            //    : "ASUSE";
//            var db_scheme = "ASUSE";
//            // тоже Ленэнерго
//            //var db_scheme = "PRES";
//         //   db_scheme = db.ExecuteDataTable("select upper(USER) as s from dual", db.Connection).Rows[0][0].ToString();
//            var new_file = XmlSchemeBuilder.XmlTableStructure(textBox1.Text);

//            //string filename = @"C:\tfs\all\sql.builder\sql.builder\source\scheme\" + XmlReports.schemeName + @"\original\" + textBox1.Text + ".xml";
//            if (new_file == null) return;

//            string filename = Path.Combine(XmlReports.GetCurrentContentFolder() + "\\" + XmlReports.schemeName + @"\scheme\original\" + textBox1.Text + ".xml");
//            TFSHelper.CheckOutFile(filename);

//            if (!File.Exists(filename))
//            {
//                using (var sw = new StreamWriter(new FileStream(filename, FileMode.Create), Encoding.Unicode))
//                {
//                    sw.Write(new_file.ToString());
//                }
//                using (var tfs = new TFSServer())
//                {
//                    tfs.AddFile(filename);
//                }
//            }
//            else
//            {

//                var old_file = Cmn.OpenXmlClearNS(filename).Root;
//                var updated_file = XmlSchemeBuilder.UpdateScheme(old_file, new_file);

//                //using (var sw = new StreamWriter(new FileStream(filename, FileMode.Create), Encoding.Unicode))
//                //{
//                //    sw.Write(updated_file.ToString());
//                //}

//                Cmn.SaveXmlWithCheckOut(new XDocument(updated_file), filename);
//            }
//        }

//        //  private DataSet ds;
//        private void simpleButton4_Click_2(object sender, EventArgs e)
//        {
//            //string queryName = lookUpEdit1.EditValue.ToString();

//            //var xmldoc = new XmlDocument();
//            //xmldoc.LoadXml(Compiler.SchemeRoot.ToString());

//            //var xmldoc_old = new XmlDocument();
//            //xmldoc_old.LoadXml(Compiler.schemeRootOld.ToString());

//            //XmlNode queryXml = XmlReports.getItemProcessedXml("query", lookUpEdit1.EditValue.ToString(), false, xmldoc, xmldoc_old);
//            //XDocument doc = XDocument.Parse(queryXml.OuterXml);

//            //XElement query = doc.Root;
//            //List<XElement> tables = query.Descendants("table").ToList();

//            //SortedList<string, string> tableNames = new SortedList<string, string>();
//            //SortedList<string, List<string>> usedColumns = new SortedList<string, List<string>>();
//            //SortedList<string, List<string>> usedRelations = new SortedList<string, List<string>>();
//            //foreach (XElement table in tables)
//            //{
//            //    string tableName = Cmn.GetAttrValue(table, "name");
//            //    if (!tableNames.Keys.Contains(tableName))
//            //    {
//            //        XElement table1 = XmlReports.Environment.Scheme.Descendants("table").Where(t => Cmn.GetAttrValue(t, "name") == tableName).First();
//            //        tableNames.Add(tableName, table1.Ancestors().Where(e2 => Cmn.GetAttrValue(e2, "file") != "").Select(e1 => Cmn.GetAttrValue(e1, "file")).First());

//            //        usedColumns.Add(tableName, new List<string>());
//            //        usedRelations.Add(tableName, new List<string>());
//            //    }

//            //    XElement query2 = table.Ancestors("query").First();

//            //    //    int i=0;
//            //    foreach (XElement col in query2.Element("select").Elements("column").Where(e1 => e1.Attribute("table").Value == table.Attribute("as").Value))
//            //    {
//            //        usedColumns[tableName].Add(col.Attribute("column").Value);
//            //    }
//            //    foreach (XElement rel in query2.Element("from").Elements("query"))
//            //    {
//            //        usedColumns[tableName].Add(rel.Attribute("as").Value);
//            //        usedRelations[tableName].Add(rel.Attribute("as").Value);
//            //    }

//            //}

//            //string sss = "";
//            //foreach (string tableName in tableNames.Keys)
//            //{

//            //    XDocument doc1 = XDocument.Load(tableNames[tableName]);

//            //    XElement query1 = doc1.Descendants("queries").Elements("query").Where(e2 => Cmn.GetAttrValue(e2, "name").Equals(tableName)).First();
//            //    sss += "create table " + tableName + "(";
//            //    string q = "";
//            //    string keyFieldName = "";
//            //    foreach (XElement col in query1.Element("select").Elements())
//            //    {
//            //        if (q == "")
//            //        {
//            //            keyFieldName = col.Attribute("column").Value;
//            //        }
//            //        string colname = col.Attribute("column").Value;
//            //        string dataType = col.Attribute("type").Value;

//            //        if (dataType == "string")
//            //        {
//            //            dataType = "varchar2 " + "(250)";
//            //        }

//            //        if (q == "" || usedColumns[tableName].Contains(colname))
//            //        {

//            //            sss += Environment.NewLine + q + colname + " " + dataType;
//            //            q = ",";
//            //        }
//            //    }
//            //    sss += ")" + Environment.NewLine;

//            //    sss += "/";
//            //    sss += Environment.NewLine;
//            //    string spk = string.Format(@"ALTER TABLE {0} ADD CONSTRAINT {0}_PK  PRIMARY KEY ({1})", tableName, keyFieldName);
//            //    sss += spk;
//            //    sss += Environment.NewLine;
//            //    sss += "/";
//            //    sss += Environment.NewLine;
//            //    sss += Environment.NewLine;
//            //}
//            //sss += Environment.NewLine;

//            //int i1 = 0;

//            //foreach (string tableName in tableNames.Keys)
//            //{

//            //    XDocument doc1 = XDocument.Load(tableNames[tableName]);

//            //    XElement query1 = doc1.Descendants("queries").Elements("query").Where(e2 => Cmn.GetAttrValue(e2, "name").Equals(tableName)).First();

//            //    foreach (XElement qry in query1.Element("from").Elements("query").Where(e1 => Cmn.GetAttrValue(e1, "join") != null))
//            //    {
//            //        if (usedRelations[tableName].Contains(qry.Attribute("as").Value) || usedColumns[tableName].Contains(qry.Attribute("as").Value))
//            //        {
//            //            string col1Name = qry.Element("call").Descendants("column").Where(col => col.Attribute("table").Value == query1.Element("from").Elements("table").First().Attribute("as").Value).Select(col1 => col1.Attribute("column").Value).First();
//            //            string col2Name = qry.Element("call").Descendants("column").Where(col => col.Attribute("table").Value == qry.Attribute("as").Value).Select(col1 => col1.Attribute("column").Value).First();
//            //            string sfk = string.Format(@"ALTER TABLE {0} ADD CONSTRAINT {4} FOREIGN KEY ({1}) REFERENCES {2}({3}) ", tableName, col1Name, qry.Attribute("name").Value, col2Name, "FK" + i1.ToString());
//            //            sss += sfk;
//            //            sss += Environment.NewLine;
//            //            sss += "/";
//            //            sss += Environment.NewLine;
//            //            i1++;
//            //        }
//            //    }
//            //}
//            //FileStream fs = new FileStream(textEdit1.EditValue.ToString(), FileMode.Create);
//            //StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding(1251));
//            //sw.Write(Cmn.ClearUndefined(sss));
//            //sw.Close();
//            //Process.Start(textEdit1.EditValue.ToString());

//        }

//        private void simpleButton5_Click_1(object sender, EventArgs e)
//        {
//            var frm = new FormTest5();
//            frm.Show();

//        }

//        private void testClick(object sender, EventArgs e)
//        {
//            MessageBox.Show("yeep");


//        }

//        private void simpleButton6_Click(object sender, EventArgs e)
//        {

//        }

//        private void simpleButton7_Click(object sender, EventArgs e)
//        {

//            queriesTree();

//        }

//        private XElement queriesTree()
//        {
//            string queryName = lookUpEdit1.EditValue.ToString();

//            var xmldoc = new XmlDocument();
//            xmldoc.LoadXml(XmlReports.Environment.Manager.GetScheme().First().ToString());

//            var xmldoc_old = new XmlDocument();
//            xmldoc_old.LoadXml(XmlReports.Environment.Manager.GetOldScheme().First().ToString());

//            XmlNode queryXml = XmlReports.getItemProcessedXml("query", lookUpEdit1.EditValue.ToString(), false, xmldoc, xmldoc_old);
//            XDocument doc = XDocument.Parse(queryXml.OuterXml);
//            XDocument output = new XDocument();
//            output.Add(new XElement("root"));


//            queriesTreeNode(doc.Root.Element("query"), output.Root);
//            foreach (XElement table in output.Root.Descendants("table").ToList())
//            {
//                table.Parent.ReplaceWith(table);
//            }


//            foreach (XElement table in output.Root.Descendants().Where(e2 => e2.ElementsBeforeSelf().Where(e1 => e1.Attribute("name").Value == e2.Attribute("name").Value).Count() > 0).ToList())
//            {
//                table.Remove();
//            }

//            return output.Root;


//        }


//        private void queriesTreeNode(XElement element, XElement output)
//        {
//            XElement node = new XElement(element.Name.LocalName, new XAttribute("name", element.Attribute("name").Value));
//            output.Add(node);

//            IEnumerable<XElement> childs1 = element.Descendants().Where(e => (new string[] { "query", "table" }).Contains(e.Name.LocalName) && Cmn.GetAttrValue(e, "name") != "").ToArray();
//            IEnumerable<XElement> childs2 = childs1.Where(e => e.Ancestors("query").Where(e1 => Cmn.GetAttrValue(e1, "name") != "").First() == element).ToArray();
//            foreach (XElement child in childs2)
//            {
//                queriesTreeNode(child, node);
//            }



//        }


//        private void simpleButton8_Click(object sender, EventArgs e)
//        {
//            List<string> list = new List<string>();
//            list.Add(lookUpEdit1.EditValue.ToString());
//            int count = -1;

//            List<XElement> queries = XmlReports.Environment.Manager.GetScheme().Elements("queries").Elements("query").ToList();
//            while (count != 0)
//            {
//                count = 0;
//                foreach (string name in queries.Where(e1 => e1.Descendants("query").Any(e2 => e2.Attribute("name") != null && list.Contains(e2.Attribute("name").Value))).Select(e3 => e3.Attribute("name").Value).ToList())
//                {
//                    if (!list.Contains(name))
//                    {
//                        list.Add(name);
//                        count++;
//                    }

//                }
//            }

//            IEnumerable<XElement> reports = XmlReports.Environment.Manager.GetScheme().Elements("reports").Elements("report").Where(e0 => e0.Descendants("query").Any(e2 => e2.Attribute("name") != null && list.Contains(e2.Attribute("name").Value))).ToArray();

//            treeList1.Columns.Clear();
//            treeList1.DataSource = Cmn.XElementsToDataTable(reports);

//        }

//        private void layoutControl1_Resize(object sender, EventArgs e)
//        {

//        }

//        private void simpleButton9_Click(object sender, EventArgs e)
//        {

//            XmlReports.LoadXml();

//            string queryName = lookUpEdit1.EditValue.ToString();

//            FileStream fs = new FileStream(textEdit1.EditValue.ToString(), FileMode.Create);
//            StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding(1251));

//            string functionSql = getFuncText(queryName);
//            sw.Write(Cmn.ClearUndefined(functionSql));
//            sw.Close();
//            Process.Start(textEdit1.EditValue.ToString());
//        }

//        private string getFuncText(string queryName)
//        {
//            var xmldoc = new XmlDocument();
//            xmldoc.LoadXml(XmlReports.Environment.Manager.GetScheme().First().ToString());

//            var xmldoc_old = new XmlDocument();
//            xmldoc_old.LoadXml(XmlReports.Environment.Manager.GetOldScheme().First().ToString());

//            VQuery query = XmlReports.Environment.GetPrecompiledQuery(queryName);
//            string pars = "";
//            string q = "";
//            foreach (XElement par in query.Elements("params").Elements())
//            {
//                par.Elements().Remove();
//                par.Add(new XElement("const", new XText(par.Attribute("name").Value)));
//                pars += q + par.Attribute("name").Value + " " + par.Attribute("type").Value;
//                q = ",";
//            }
//            XmlNode queryXml = XmlReports.getItemProcessedXml("query", queryName, false, xmldoc, xmldoc_old);
//            string fname = query.Attribute("name").Value.Split('-')[1];
//            string fRetType = queryXml.SelectSingleNode("//select/*[@type]").Attributes["type"].Value;
//            string fRetType2 = fRetType;
//            // Емцов - строковый параметр
//            if (fRetType == "string")
//            {
//                fRetType = "varchar2";
//                fRetType2 = "varchar2(100)";
//            }


//            string querySql = XmlReports.getQuerySql(queryXml);
//            string functionSql = string.Format(@"
//function {0} ({1}) return {2} 
//is  
//    v_ret {3};
//begin
//    select 
//    (
//        {4}
//    ) into v_ret from dual;
//    return v_ret;
//end;
//", fname, pars, fRetType, fRetType2, querySql);
//            return functionSql;
//        }

//        private string getFuncHead(string queryName)
//        {
//            var xmldoc = new XmlDocument();
//            xmldoc.LoadXml(XmlReports.Environment.Manager.GetScheme().First().ToString());

//            var xmldoc_old = new XmlDocument();
//            xmldoc_old.LoadXml(XmlReports.Environment.Manager.GetOldScheme().First().ToString());

//            VQuery query = XmlReports.Environment.GetPrecompiledQuery(queryName);
//            string pars = "";
//            string q = "";
//            foreach (XElement par in query.Elements("params").Elements())
//            {
//                par.Elements().Remove();
//                par.Add(new XElement("const", new XText(par.Attribute("name").Value)));
//                pars += q + par.Attribute("name").Value + " " + par.Attribute("type").Value;
//                q = ",";
//            }
//            XmlNode queryXml = XmlReports.getItemProcessedXml("query", queryName, false, xmldoc, xmldoc_old);
//            string fname = query.Attribute("name").Value.Split('-')[1];
//            string fRetType = queryXml.SelectSingleNode("//select/*[@type]").Attributes["type"].Value;

//            string functionSql = string.Format(@"
//function {0} ({1}) return {2};
//", fname, pars, fRetType);
//            return functionSql;
//        }
//        private void simpleButton10_Click(object sender, EventArgs e)
//        {
//            XmlReports.LoadXml();
//            string queryName = lookUpEdit1.EditValue.ToString();
//            VQuery query = XmlReports.Environment.GetPrecompiledQuery(queryName);
//            string fileName = query.AttrOrEmpty(AName.file);
//            string functionSql = "";
//            foreach (XElement query1 in XmlReports.Environment.Manager.GetScheme().Elements(EName.queries).Elements(EName.query).Where(e1 => e1.AttrOrEmpty(AName.file) == fileName).ToList()) {
//                functionSql += getFuncText(query1.Attribute(AName.name).Value) + @"





//";
//            }
//            FileStream fs = new FileStream(textEdit1.EditValue.ToString(), FileMode.Create);
//            StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding(1251));
//            sw.Write(Cmn.ClearUndefined(functionSql));
//            sw.Close();
//            Process.Start(textEdit1.EditValue.ToString());
//        }
//        private void simpleButton11_Click(object sender, EventArgs e)
//        {
//            XmlReports.LoadXml();

//            string queryName = lookUpEdit1.EditValue.ToString();

//            VQuery query = XmlReports.Environment.GetPrecompiledQuery(queryName);
//            string fileName = query.AttrOrEmpty(AName.file);
//            string functionSql = "";
//            foreach (XElement query1 in XmlReports.Environment.Manager.GetScheme().Elements(EName.queries).Elements(EName.query).Where(e1 => e1.AttrOrEmpty(AName.file) == fileName).ToList()) {
//                functionSql += getFuncHead(query1.Attribute(AName.name).Value);
//            }
//            // В.Емцов - так гарантированно освобождаются занятые ресурсы
//            using (var sw = new StreamWriter(new FileStream(textEdit1.EditValue.ToString(), FileMode.Create), Encoding.GetEncoding(1251))) {
//                sw.Write(Cmn.ClearUndefined(functionSql));
//            }
//            Process.Start(textEdit1.EditValue.ToString());
//        }
//        private void simpleButton12_Click(object sender, EventArgs e)
//        {

//            XElement qTree = queriesTree();
//            Compiler.DontPrecompile = true;
//            XmlReports.LoadXml();
//            Compiler.DontPrecompile = false;



//            foreach (string qname in qTree.Descendants().Attributes("name").Select(a => a.Value).Distinct())
//            {
//                XElement qry = XmlReports.Environment.Manager.GetScheme().Elements().Elements("queries").Elements("query").First(q => q.Attribute("name").Value == qname);

//                XDocument doc = new XDocument();
//                doc.Add(new XElement("root"));
//                doc.Root.Add(new XElement("queries", qry));

//                foreach (string pname in qry.Descendants("usepart").Attributes("part").Select(a => a.Value).Distinct())
//                {

//                    XElement part = XmlReports.Environment.Manager.GetScheme().Elements().Elements("parts").Elements("part").First(q => q.Attribute("id").Value == pname);
//                    if (doc.Root.Element("parts") == null)
//                    {
//                        doc.Root.Add(new XElement("parts"));
//                    }
//                    doc.Root.Element("parts").Add(part);
//                }

//                doc.Save(textEdit1.EditValue.ToString() + qry.Attribute("name").Value + ".xml");

//            }

//        }

//        private void btnRepPackageSql_Click(object sender, EventArgs e)
//        {
//            //using (var frm = new XtraForm() {Width = 800, Height = 800, StartPosition = FormStartPosition.CenterScreen})
//            //{
//            //    var memo = new MemoEdit()
//            //    {
//            //        EditValue = XmlRepositories.GetPackageSql(),
//            //        Dock = DockStyle.Fill
//            //    };
//            //    frm.Controls.Add(memo);
//            //    frm.ShowDialog();
//            //}

//            // В.Емцов - так гарантированно освобождаются занятые ресурсы
//            using (var sw = new StreamWriter(new FileStream(textEdit1.EditValue.ToString(), FileMode.Create), Encoding.GetEncoding(1251)))
//            {
//                sw.Write(Cmn.ClearUndefined(SqlRepository.GetPackageSql()));
//            }
//            Process.Start(textEdit1.EditValue.ToString());
//        }

//        private void simpleButton13_Click(object sender, EventArgs e)
//        {
//            //Cmn.SaveText(XmlRepositories.IndexesExpr("ipr_fin_body_united","vr_ipr"),textEdit1.EditValue.ToString());

//            Process.Start(textEdit1.EditValue.ToString());
//        }

//        private void simpleButton14_Click(object sender, EventArgs e)
//        {
//            //Cmn.SaveText(XmlRepositories.RefreshByCreateExpr ("ipr_fin_body_united", "vr_ipr"), textEdit1.EditValue.ToString());

//            Process.Start(textEdit1.EditValue.ToString());
//        }

//        private void simpleButton15_Click(object sender, EventArgs e)
//        {
//            Cmn.SaveText(SqlRepository.GetViewSql(), textEdit1.EditValue.ToString(), Encoding.Unicode);

//            Process.Start(textEdit1.EditValue.ToString());
//        }

//        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
//        {
//            if (keyData == (Keys.Q | Keys.Shift))
//            {
//                simpleButton1.PerformClick();
//                return true;
//            }
//            return base.ProcessCmdKey(ref msg, keyData);
//        }

//        private void simpleButton16_Click(object sender, EventArgs e)
//        {
//            Cmn.SaveText(SqlRepository.GetViewChangedSql(), textEdit1.EditValue.ToString(), Encoding.Unicode);

//            Process.Start(textEdit1.EditValue.ToString());

//        }

//        private void simpleButton17_Click(object sender, EventArgs e)
//        {
//            Cmn.SaveText(SqlRepository.GetViewForUpdSql(), textEdit1.EditValue.ToString(), Encoding.Unicode);

//            Process.Start(textEdit1.EditValue.ToString());
//        }

//        private void simpleButton18_Click(object sender, EventArgs e)
//        {
//            // Подключить проект reports.word
//            // using reports.word.XmlPrint;

//            //1. Создание отчета (выходного документа)
//            WordReport report1 = new WordReport();



//            //2. Получение шаблона

//            //Вариант А: 
//            //Открытие шаблона старого формата c динамической конвертацией
//            WordTemplate template1 = TemplatesConverter.GetWordTemplate(@"\\Emtsov\все_нужное_клади_сюда\Примеры_уведомлений\Old\pred_ogr_itogplat_kaz.xml");


//            ////Вариант B (быстрее): 
//            ////Предварительная конвертация (разовая , чтобы заменить старые шаблоны на новые)
//            //ConvertOptions convertOptions=new ConvertOptions();
//            //convertOptions.OutputFilePath=@"\\Emtsov\все_нужное_клади_сюда\Примеры_уведомлений\New\pred_ogr_itogplat_kaz.docx";
//            //TemplatesConverter.Convert(@"\\Emtsov\все_нужное_клади_сюда\Примеры_уведомлений\Old\pred_ogr_itogplat_kaz.xml", convertOptions);

//            ////Открытие шаблона нового формата
//            //WordTemplate template1 = new WordTemplate(@"\\Emtsov\все_нужное_клади_сюда\Примеры_уведомлений\New\pred_ogr_itogplat_kaz.docx");



//            //3. Получение DataSet и заполнение данных для печати
//            DataSet dataSet1 = template1.GetDataSet();

//            //DataSet отражает содержание шаблона
//            // - обязательно содержит таблицу "main" с индексом 0 с одной строкой с колонками соттветствующими  переменным из шаблона "@@NOM_TLG_PRED", "@@DAT_TLG_PRED" и т.д.
//            // - может содержать таблицы: 1 "table1", 2 "table2" и т.д , с колонками 1 "column1", 2 "column2"  и т.д  в зависимости от наличия таблиц в шаблоне.

//            // Задача вывода данных в отчет , сводится к заполнению данными этого DataSet

//            int i = 0;
//            foreach (DataColumn col in dataSet1.Tables["main"].Columns)
//            {
//                string varName = col.ColumnName; // Имя переменной "@@NOM_TLG_PRED", "@@DAT_TLG_PRED" и т.д.
//                dataSet1.Tables["main"].Rows[0][varName] = "Значение " + i.ToString(); // Установка значения переменной
//                i++;
//            }

//            DataRow row = dataSet1.Tables[2].Rows.Add(); //Добавление строки в таблицу 
//            row[0] = "Значение 1 в строке 1";  //Заполнение данных в строке
//            row[1] = "Значение 2 в строке 1"; //Заполнение данных в строке

//            row = dataSet1.Tables[2].Rows.Add(); //Добавление еще строки в таблицу 
//            row[0] = "Значение 1 в строке 2";//Заполнение данных в строке
//            row[1] = "Значение 2 в строке 2";//Заполнение данных в строке


//            // 4. Добавление шаблона с данными в отчет (выходной документ)
//            report1.Print(template1, dataSet1);

//            // 5. Сохранение выходного документа
//            report1.SaveToFile(@"C:\Временные\Док1.docx");


//            return;


//            //DataTable tt = db.ExecuteDataTable("select * from kr_dogovor where rownum<2", db.Connection);

//            //DataSet ds = new DataSet();

//            //tt.TableName = "a";

//            //ds.Tables.Add(tt);

//            //string docname = @"C:\Временные\Шаблон ФТС 2015 new .xml";
//            //XDocument xdoc = XDocument.Load(docname);
//            // WordPrintDocument doc = new WordPrintDocument(xdoc);
//            //doc.Print(docname.Replace(".xml", "1.xml"), ds);
//            // xdoc.Save();
//        }

//        private void btnQVUpdateDB_Click(object sender, EventArgs e)
//        {
//            QlikView.UpdateViews("appserv");
//        }

//        private void btnQVUpdateScripts_Click(object sender, EventArgs e)
//        {
//            XmlReports.LoadXml();
//            QlikView.UpdateScripts("appserv");
//        }

//        /// <summary>
//        /// создание вьюхи материализованной
//        /// </summary>
//        /// <param name="sender"></param>
//        /// <param name="e"></param>
//        private void simpleButton19_Click(object sender, EventArgs e)
//        {
//            var qname = lookUpEdit1.EditValue.ToString();
//            WaitUIHelper.LastUsedUIHelper.Show(String.Format("Генерация скрипта {0}", qname), WaitUIMode.WaitPanel);
//            //Wait.Show(String.Format("Генерация скрипта {0}", qname), form: this);
//            // XmlReports.loadXml(null, null, SqlBuilder.InputParams);
//            VQuery q = XmlReports.Environment.GetPrecompiledQuery(qname);
//            var qsname = string.Empty;
//            if (q.Attribute("stored") != null)
//                qsname = q.Attribute("stored").Value.ToString();
//            else
//                qsname = q.Attribute("name").Value.ToString();

//            var qry=new XElement(q);
//            qry.Attributes(TextConst.AName.Stored).Remove();
//            qry.Attributes(TextConst.AName.Name).Remove();
            
//            var cqry= Compiler.GetCompiledAndProcessedQuery(qry);
//           // var sql = Compiler.GetSql(XmlReports.getItemProcessedXml2("query", qname, false));
//            var sql = Compiler.GetSql(cqry);
//            string sqlbegin = string.Format(@"
//-- Start of DDL Script for Materialized View ASUSE.M_VIEW
//-- Generated {1}

//DROP MATERIALIZED VIEW {0} 
///

//CREATE MATERIALIZED VIEW {0} 
//--TABLESPACE  asuse_tbl
//--LOGGING
//--NOPARALLEL
//--BUILD IMMEDIATE 
//--REFRESH COMPLETE ON DEMAND 
//--ENABLE QUERY REWRITE 
//BUILD IMMEDIATE
//REFRESH START WITH ROUND(SYSDATE+1)
//   NEXT SYSDATE + 1
//AS ", qsname, DateTime.Now.ToString());

//            string sqlend = string.Format(@"/
//-- Synonym for View
//create public synonym {0} for {0}
///
//-- Grants for View
//GRANT SELECT ON {0} TO public 
///
//", qsname);


//            var idxstr = @"CREATE INDEX {0}_{1} ON {0} ({1}  ASC )
///";


//            foreach (string col in q.Elements(TextConst.EName.From).Descendants(TextConst.EName.Column)
//                .Where(e1 => Cmn.GetAttrValue(e1, TextConst.AName.Table) == TextConst.AVTable.Ths)
//                .Select(e2 => e2.Attribute(TextConst.AName.Column).Value).Distinct().ToList()
//                )
//            {
//               var  idxstr1 = string.Format(idxstr, qsname, col);
//               sqlend += idxstr1;
//            }

//            // создаем в том же каталоге скрипт для вьюхи
//            var namefile = string.Format("{0}{1}_{2}_{3}_dll.sql", Path.GetDirectoryName(textEdit1.EditValue.ToString()), DateTime.Now.ToString("yyMMdd"), Environment.MachineName, qsname);
//            using (var sw = new StreamWriter(new FileStream(namefile, FileMode.Create), Encoding.GetEncoding(1251)))
//            {
//                sw.Write(sqlbegin);

//                sw.Write(Cmn.ClearUndefined(sql));

//                sw.Write(sqlend);
//            }
//            WaitUIHelper.LastUsedUIHelper.Hide();
//            //Wait.Hide();
//            Process.Start(namefile);
//        }

//        private void simpleButton20_Click(object sender, EventArgs e)
//        {
//            var qname = lookUpEdit1.EditValue.ToString();

//            VQuery query = XmlReports.Environment.GetQuery(qname);
//            XElement qry = VQubeUtils.CreateQubeQuery(query, null);

//            qry.SetAttributeValue("noname", "1");
            
//            XElement compiledQuery = Compiler.GetCompiledAndProcessedQuery(qry);
//            string selectText = Compiler.GetQuerySelectStatmentFromCompiledQuery(compiledQuery);

//            string text = Cmn.ClearUndefined(selectText);

//            using (var sw = new StreamWriter(new FileStream(textEdit1.EditValue.ToString(), FileMode.Create), Encoding.GetEncoding(1251)))
//            {
//                sw.Write(Cmn.ClearUndefined(text));
//            }
            
//            Process.Start(textEdit1.EditValue.ToString());

//        }


//        private void saveElementInSourceFile(VSXElement el)
//        {


//            el.ParentName = el.Parent.Name.LocalName;
//            el.SourceFileName = ucQueryEditor.AddPathToFilename(el.AttrOrEmpty(AName.file));
//            el.SaveInSourceFile();
//        }

//        private void simpleButton21_Click(object sender, EventArgs e)
//        {

//            XmlReports.Environment.Manager.LoadProjectIfNeed("45258");
//            var list = XmlReports.Environment.GetElements(TextConst.EName.Queries).ToList().Cast<VQuery>().ToList();
//            foreach (var qry1 in list)
//            {
//                if (qry1.GetProjectName() == "45258")
//                {
//                    foreach (var col in qry1.Columns())
//                    {

//                        if (qry1.Elements(EName.from).Elements(EName.table).Any())
//                        {
//                            if (col.P_Column.ToUpper() == "NAME_FIDER_NUT")
//                            {
//                                col.P_SelfTitle = "Фидер";
//                            }
//                            if (col.P_Column.ToUpper() == "SRCNAME")
//                            {
//                                col.P_SelfTitle = "ТП";
//                            }
//                        }
//                        continue;
//                        col.P_SelfTitle = col.P_Comment;
//                        if (col.P_Comment.Contains("изм2:"))
//                        {
//                            col.P_SelfTitle = col.P_Comment.Replace("изм2:", "");
//                            var pref="";
//                            if (qry1.P_IdName.StartsWith("jr_rep_cons"))
//                            {
//                                pref = "t0";
//                            }
//                            if (qry1.P_IdName.StartsWith("jr_rep_cons_pnt1"))
//                            {
//                                pref = "t1";
//                            }

//                            if (qry1.P_IdName.StartsWith("jr_rep_cons_pnt2"))
//                            {
//                                pref = "t2";
//                            }
//                            pref += "_"+qry1.P_IdName[qry1.P_IdName.Length - 1]+"_";
//                            col.P_FactName = pref + col.P_Column;
//                            col.P_Aggregation = "sum";
//                        }

//                        if (col.P_Comment.Contains("изм1:"))
//                        {
//                            col.P_Dimension = col.P_Column;
//                            var qry2 = XmlReports.Environment.GetQueryByKeyDimensionName(col.P_Column);

//                            qry2.P_SelfTitle = col.P_Title;
//                            saveElementInSourceFile(qry2);

//                        }
                       
//                    }
                   
//                    saveElementInSourceFile(qry1);
//                }
               
               
                
//            }


//            //XmlReports.Environment.Manager.LoadProjectIfNeed("asuse2");
//            //var list = XmlReports.Environment.GetElements(TextConst.EName.Queries).ToList().Select(q1 => (VQuery)q1).ToList();
//            //foreach (var qry1 in list)
//            //{
//            //    bool changes = false;
//            //    foreach (var link in qry1.GetDescedantsApplyingParts(TextConst.EName.DimLink))
//            //    {
//            //        if (link.P_CalledQuery == "kodp_org")
//            //        {
//            //            link.P_CalledQuery = "kodp_bal";
//            //            changes = true;
//            //        }
//            //    }
//            //    if (changes)
//            //    {
//            //        saveElementInSourceFile(qry1);
//            //    }
//            //}
//            //var list = XmlReports.Environment.GetElements(TextConst.EName.Queries).ToList().Select(q1 => (VQuery)q1).ToList();

//            //var ddimqeries = new SortedList<string, List<string>>();
//            //foreach (var qry1 in list)
//            //{
//            //    var qry = (VQuery)qry1.GetMainE();

//            //    if (qry.XName == "ur_graf")
//            //    {
//            //    }
//            //    var dimnames = new List<string>();
//            //    var dims = new List<VSXElement>();
//            //    dims.AddRange( qry.EntityType.ParentDimensionLinks());
//            //    dims.AddRange(qry.EntityType.PrimaryExtDimensionLinks());
//            //    dims.AddRange(qry.EntityType.SecondaryExtDimensionLinks());
//            //    dims.AddRange(qry.EntityType.ColumnDimensionLinks());
//            //    foreach (var d in dims)
//            //    {
//            //        if (dimnames.Contains(d.P_Dimension))
//            //        {
//            //            if (!ddimqeries.ContainsKey(qry.XName))
//            //            {
//            //                ddimqeries.Add(qry.XName,new List<string>());
//            //            }
//            //            if (!ddimqeries[qry.XName].Contains(d.P_Dimension))
//            //            {
//            //                ddimqeries[qry.XName].Add(d.P_Dimension);
//            //            }
//            //        }
//            //        else
//            //        {
//            //            dimnames.Add(d.P_Dimension);
//            //        }

//            //    }
//            //    //.ColumnDimensionLinks();
//            //    //PrimaryExtDimensionLinks()

//            //}

//            //var s = "";
//            //foreach (var ii in ddimqeries)
//            //{
//            //    s += ii.Key+"(";
//            //    s += string.Join(",", ii.Value)+") ";
             
//            //}
           
//            //var s1 = s;
//            //var list = XmlReports.Environment.GetElements(TextConst.EName.Queries).ToList().Where(q => q.P_IsReport == TextConst.AVBool.True).Select(q1 => (VSXElement)q1).ToList();
//            //var list1 = XmlReports.Environment.GetElements(TextConst.EName.Forms).ToList().Select(q1 => (VSXElement)q1).ToList();
//            //list.AddRange(list1);

//            //list=list.Where(e1=>Cmn.GetAttrValue(e1,TextConst.AName.WithBehavior)!=TextConst.AVBool.False).ToList();
//            //foreach (VSXElement query in list)
//            //{
//            //    var main = query.GetMainParent();
//            //    if (main.GetAttrValue(TextConst.AName.File) != "")
//            //    {
//            //        query.SetAttributeValue(TextConst.AName.WithBehavior, TextConst.AVBool.False);

//            //        main.ParentName = main.Parent.Name.LocalName;
//            //        main.SourceFileName = ucQueryEditor.AddPathToFilename(main.GetAttrValue(TextConst.AName.File));
//            //        main.SaveInSourceFile();
//            //    }
//            //    else
//            //    {
//            //    }
//            //}


//            //foreach (VQuery query in XmlReports.Environment.GetElements(TextConst.EName.Queries).ToList())
//            //{
//            //    if (query.P_Extend == query.P_Name)
//            //    {
//            //        var newName = query.P_IdName + "_ext";

//            //        var qry = XmlReports.Environment.GetQuery(newName);
//            //        if (qry == null)
//            //        {
//            //            query.SavedKey = query.P_IdName;
//            //            query.P_IdName = newName;
//            //            var main = query.GetMainParent();

//            //            main.ParentName = main.Parent.Name.LocalName;
//            //            main.SourceFileName = ucQueryEditor.AddPathToFilename(main.GetAttrValue(TextConst.AName.File));
//            //            main.SaveInSourceFile();

//            //            query.SaveInSourceFile();
//            //        }
//            //        else
//            //        {
//            //        }
//            //    }
//            //}


//            //foreach (VDimension dim in XmlReports.Environment.GetElements(TextConst.EName.DimensionPackages).SelectMany(e1=>e1.GetElementsApplyingParts()).ToList())
//            //{

//            //    var qry = XmlReports.Environment.GetQueryByKeyDimensionName(dim.P_IdName);
//            //    if (qry != null)
//            //    {
//            //        dim.P_CalledQuery = qry.P_IdName;

//            //        var main = dim.GetMainParent();
//            //        main.ParentName = main.Parent.Name.LocalName;
//            //        main.SourceFileName = ucQueryEditor.AddPathToFilename(main.GetAttrValue(TextConst.AName.File));
//            //        main.SaveInSourceFile();
//            //    }
//            //}
//            //foreach (VSXElement form in XmlReports.Environment.SchemeNative.Descendants(TextConst.EName.Form).ToList().Select(e1 => VSXElement.Get(e1)).ToList())
//            //{
//            //    bool changed = false;
//            //    foreach (VSXElement grid in form.Descendants(TextConst.EName.Grid).ToList().Select(e1 => VSXElement.Get(e1)).ToList())
//            //    {
//            //        if (grid.Element(TextConst.EName.Columns) == null)
//            //        {
//            //            var content = grid.Elements().ToList();
//            //            content.Remove();
//            //            grid.Add(new XElement(TextConst.EName.Columns));
//            //            grid.Element(TextConst.EName.Columns).Add(content);
//            //            changed = true;
//            //        }
//            //    }
//            //    if (changed)
//            //    {

//            //        var main = form.RootQuery();
//            //        main.ParentName = main.Parent.Name.LocalName;
//            //        main.SourceFileName = ucQueryEditor.AddPathToFilename(main.GetAttrValue(TextConst.AName.File));
//            //        main.SaveInSourceFile();
//            //    }
//            //}
//        }

//        private void simpleButton22_Click(object sender, EventArgs e)
//        {
//            DataEditor.OpenForm("kido_lkk", "vc_user_login_list");
//            return;
//            //
//            var frm = new sql.builder.WinForms.frmMain();
//            if (!frm.Loaded)
//            {
//                //frm.ContextGroup = new SqlReportsContextGroup();
//                XmlReports.SetInputParameter("folder", "pris");
//                frm.Initialize();
//            }

//            frm.Show();
//            frm.Activate();

//            return;
            
            
//            // UIStatic.LoadProject("ipr");
//            var report = new ExpressReport();
//            report.Initialize("44212.44212");
          
           
//            report.GetParamField("p_kod_dog").SetValue(285793m);
//            report.ShowDialog("44212.44212");
//            return;


//            SqlBuilder.PreviewReport("41050", false);
//            return;
//            var cmd = new OracleCommand("select * from bav_test", db.Connection);
//            cmd.FetchSize = 1;
//            var reader = cmd.ExecuteReader();
    
//            int cnt = 0;
           
//            while (reader.Read())
//            {
//                cnt++;
//            }
           
            
//            VQuery q = XmlReports.Environment.GetQuery("ipr_fin_body_united");

//            var xcnt = new XElement("select");

//            foreach (VSXElement col in q.Columns())
//            {
//                if (Cmn.GetAttrValue(col, TextConst.AName.Stored) != "0" && col.P_AggregationS != "")
//                {
//                    xcnt.Add(new XElement(TextConst.EName.Column,

//                        new XAttribute(TextConst.AName.Table, "a"),
//                         new XAttribute(TextConst.AName.Column, col.XName)
//                        ));
//                }
//            }




//        }

//        void frm_Resize(object sender, EventArgs e)
//        {
//            // (sender as Form).SuspendLayout();
//            var btns = (sender as Form).Controls.Cast<Button>();
//            var cntInRow = 5;
//            var InRowCur = 0;
//            var top = 0;
//            var left = 0;
//            var width = (sender as Form).Width / cntInRow;
//            foreach (var btn in btns)
//            {

//                btn.Top = top;
//                btn.Left = left;
//                btn.Width = width;
//                InRowCur++;
//                left += width;
//                if (InRowCur > cntInRow)
//                {
//                    top += 30;
//                    left = 0;
//                    InRowCur = 0;
//                }
//            }
//            // (sender as Form).ResumeLayout();
//        }

//        private void simpleButton23_Click(object sender, EventArgs e)
//        {
//            var processedFiles = new List<string>();

//            var folder = @"C:\source\";
//            var commonName = "asuse2ipr";
//            var asuseName = "asuse2";
//            var iprName = "ipr";
//            var qrys = XmlReports.Environment.GetElements(TextConst.EName.Queries);
//            int i = 0;

//            foreach (VQuery q in qrys)
//            {
//                i++;
//                var fileName = q.AttrOrEmpty(AName.file);

//                if (!processedFiles.Contains(fileName))
//                {
//                    var wachedqueries = new List<int>();
//                    int t = 0;
//                    List<VQuery> queries = new List<VQuery>();
//                    queries.Add(q);
//                    while (queries.Count > 0 && t == 0) {
//                        foreach (var qry in queries)
//                        {
//                            wachedqueries.Add(qry.GetUniqueKey());
//                        }

//                        var customers = queries.SelectMany(e1 => e1.Customers()).ToList();
//                        var usesInReport = queries.SelectMany(e1 => e1.Uses_Report());

//                        var reports = usesInReport.Select(e1 => e1.GetMainParent() as VReport).Distinct().ToList();

//                        customers.AddRange(reports.SelectMany(e1 => e1.Customers()));


//                        if (customers.Count > 0)
//                        {

//                            if (customers[0].P_Customer == "17")
//                            {
//                                t = 1;

//                            }
//                            else
//                            {
//                                t = 2;

//                            }
//                        }
//                        else
//                        {
//                            var uses = queries.SelectMany(e1 => e1.Uses_QueryFrom());
//                            var queries1 = uses.Select(e1 => e1.GetMainParent() as VQuery).Distinct().ToList();

//                            queries = new List<VQuery>();

//                            foreach (var qry in queries1)
//                            {
//                                if (!wachedqueries.Contains(qry.GetUniqueKey()))
//                                {
//                                    queries.Add(qry);
//                                }

//                            }
//                        }
//                    }
//                    var fileName1 = fileName.Replace("\\asuse2", "");
//                    if (t > 0)
//                    {
//                        var name1 = folder + commonName + fileName1;
//                        var name2 = "";
//                        if (t == 1)
//                        {
//                            name2 = folder + iprName + fileName1;
//                        }
//                        else
//                        {
//                            name2 = folder + asuseName + fileName1;
//                        }
//                        Cmn.CreateFolder(name2);
//                        FileInfo f = new FileInfo(name1);
//                        f.MoveTo(name2);
//                        processedFiles.Add(fileName);

//                    }
//                    else
//                    {

//                    }

//                }
//                simpleButton23.Text = qrys.Count.ToString() + "/" + i.ToString() + "/" + processedFiles.Count.ToString();
//                Application.DoEvents();
//            }
//        }

//        private void btnKfAdress_Click(object sender, EventArgs e)
//        {
//            //var str = "kf_address";

//            //var reports = XmlReports.GetReportList("17", );
//            //var report_names = reports.AsEnumerable()
//            //    .Where(r => (r["item_type"].ToString() == "report" || r["item_type"].ToString() == "nogrid")
//            //             && (r["visible"].ToString() == "" || r["visible"].ToString() == "1"))
//            //    .Select(r => r["name"].ToString()).ToArray();

//            //Func<XElement, bool> checker = (xitem) =>
//            //{
//            //    return xitem.ToString(SaveOptions.DisableFormatting).Contains(str);
//            //};

//            //var res = Cmn.CheckReportsContent(report_names, checker);
//            //var result_path = Path.Combine(Path.GetTempPath(), "result.txt");
//            //using (var file = new FileStream(result_path, FileMode.Create))
//            //{
//            //    using (var writer = new StreamWriter(file))
//            //    {
//            //        writer.WriteLine(string.Format("Проверено {0} отчётов: {1}",
//            //            report_names.Length, string.Join(",", report_names)));
//            //        writer.WriteLine(string.Format("Найдено {0} вхождений \"{1}\" в отчётах: {2}",
//            //            res.Found.Count, str, string.Join(",", res.Found.Keys)));
//            //        writer.WriteLine(string.Format("Не удалось проверить {0} отчётов: {1}",
//            //            res.Errors.Count, string.Join(",", res.Errors.Keys)));
//            //        writer.WriteLine();
//            //        writer.WriteLine();

//            //        writer.WriteLine("++ Найдено:");
//            //        foreach (var found in res.Found)
//            //        {
//            //            writer.WriteLine("++ " + found.Key);
//            //            writer.WriteLine(found.Value);
//            //            writer.WriteLine();
//            //        }

//            //        writer.WriteLine("-- Ошибки:");
//            //        foreach (var error in res.Errors)
//            //        {
//            //            writer.WriteLine("-- " + error.Key);
//            //            writer.WriteLine(error.Value);
//            //            writer.WriteLine();
//            //        }
//            //    }
//            //}

//            //Process.Start(result_path);
//        }

//        private void btnUpdateInvProInstructs_Click(object sender, EventArgs e)
//        {
//            if (XmlReports.customerId != "17") return;

//            string[] root_folders = { "invpro", "invpro_ofz" };

//            var dt_reports = SqlBuilder.GetReportsDataTable();
//            var dt_ips = SqlMethods.Select("ips_s", null, db.Connection);

//            decimal kod_s_new = dt_ips.AsEnumerable().Max(r => Convert.ToDecimal(r["kod_s"])) + 1;

//            var root_folder_rows = dt_reports.AsEnumerable().Where(r => root_folders.Contains((string)r[TextConst.AName.Name]));
//            foreach (var root_folder_row in root_folder_rows)
//            {
//                // храним строка с отчётом + ips_s.kod_parent
//                var stack = new Stack<Tuple<DataRow, object>>();
//                stack.Push(new Tuple<DataRow, object>(root_folder_row, 27M));

//                while (stack.Count > 0)
//                {
//                    var rep = stack.Pop();
//                    var row_rep = rep.Item1;
//                    var kod_parent = rep.Item2;
//                    object kod_s = DBNull.Value;

//                    var row_ips = dt_ips.AsEnumerable().FirstOrDefault(r => r["KOD_NAME"].Equals(row_rep[TextConst.AName.Name]));
//                    if (row_ips == null)
//                    {
//                        var args = new[]
//                        {
//                            new SqlArg("kod_s", kod_s_new, SqlDestination.Insert, SqlType.Const),
//                            new SqlArg("name", row_rep[TextConst.AName.Title], SqlDestination.Insert, SqlType.String),
//                            new SqlArg("kod_name", row_rep[TextConst.AName.Name], SqlDestination.Insert, SqlType.String),
//                            new SqlArg("kod_parent", kod_parent, SqlDestination.Insert, SqlType.Const),
//                            new SqlArg("show_instr", 1, SqlDestination.Insert, SqlType.Const)
//                        };

//                        SqlMethods.Insert("ips_s", args, db.Connection);
//                        kod_s = kod_s_new++;
//                    }
//                    // заголовок отчёта изменился
//                    else if (!row_ips["name"].Equals(row_rep[TextConst.AName.Title]))
//                    {
//                        kod_s = row_ips["kod_s"];
//                        var args = new[]
//                        {
//                            new SqlArg("name", row_rep["title"], SqlDestination.Update, SqlType.String),
//                            new SqlArg("kod_s", kod_s, SqlDestination.Where, SqlType.Const)
//                        };

//                        SqlMethods.Update("ips_s", args, db.Connection);
//                    }

//                    var child_rep_rows = dt_reports.AsEnumerable()
//                        .Where(r => r[TextConst.AName.Folder].Equals(row_rep[TextConst.AName.Name]) && !r[TextConst.AName.Visible].Equals("0"));
//                    foreach (var r in child_rep_rows) stack.Push(new Tuple<DataRow, object>(r, kod_s));
//                }
//            }

//            db.Connection.Commit();
//        }

//        private string OraType(string type, bool with_precision)
//        {





//            switch (type)
//            {
//                case "string": return (with_precision) ? "VARCHAR2(4000)" : "VARCHAR2";
//                case "number": return "NUMBER";
//                case "date": return "DATE";
//                default: throw new ArgumentException(string.Format("Что за подозрительный тип {0}?", type));
//            }
//        }

//        // для корректного запуска скрипта через sqlplus
//        Encoding _encoding = Encoding.GetEncoding("Windows-1251");
//        private void btnPipelinedFunc_Click(object sender, EventArgs e)
//        {
//            XmlReports.LoadXml();
//            UIStatic.LoadProject("asuse2");
//            string qname = "deb_dolg_pr_on_dat";// lookUpEdit1.EditValue.ToString();
//            string filepath = textEdit1.EditValue.ToString();

//            string sql = SqlPipelined.Generate(qname, false);

//            // В.Емцов - так гарантированно освобождаются занятые ресурсы
//            using (var sw = new StreamWriter(new FileStream(filepath, FileMode.Create), _encoding))
//            {
//                sw.Write(sql);
//            }

//            Process.Start(filepath);
//        }
//        private void btnPipelinedFunc2_Click(object sender, EventArgs e)
//        {
//            XmlReports.LoadXml();

//            string qname = "deb_dolg_pr_on_dat";// lookUpEdit1.EditValue.ToString();
//            string filepath = textEdit1.EditValue.ToString();

//            string sql = SqlPipelined.Generate(qname, true);

//            // В.Емцов - так гарантированно освобождаются занятые ресурсы
//            using (var sw = new StreamWriter(new FileStream(filepath, FileMode.Create), _encoding))
//            {
//                sw.Write(sql);
//            }

//            Process.Start(filepath);
//        }

//        private void simpleButton24_Click(object sender, EventArgs e)
//        {
           
//            msbiViews("MSBI");

//        }
//        private string msbiUserName = null;


//        private void msbiAllSorceColumnsInfo()
//        {

//            XmlReports.Environment.Manager.GetNativeScheme().Descendants().Attributes(TextConst.AName.Materialize).Remove();
//            XmlReports.Environment.Manager.GetScheme().Descendants().Attributes(TextConst.AName.Materialize).Remove();
//            var msbiQueries = XmlReports.Environment.GetElements(TextConst.EName.Queries).Where(q => Cmn.GetAttrValue(q, TextConst.AName.Name).StartsWith("msbi_")).ToList();
//            var sql = new StringBuilder();
         


//               SortedList<string, List<string>> info = new SortedList<string, List<string>>();
//            List<string> tables = new List<string>();
//            foreach (VQuery query1 in msbiQueries)
//            {


//                var query = new XElement(query1);

//                XElement compiledQuery = Compiler.GetCompiledQuery(query);
             
//                foreach (XElement tbl in compiledQuery.Descendants(TextConst.EName.Table))
//                {
//                    var qry=tbl.Ancestors(TextConst.EName.Query).First();
//                    var xcolsAll = Compiler.getQueryColumnsWithGr(qry).ToList();
//                    var xcolsAllJoin = Compiler.getQueryJoinColumns(qry).ToList();
//                    xcolsAll.AddRange(xcolsAllJoin);
//                    var talias = tbl.Attribute(TextConst.AName.As).Value;
//                    var tname  = tbl.Attribute(TextConst.AName.Name).Value;
//                   var xcols= xcolsAll.Where(c => c.Attribute(TextConst.AName.Table).Value == talias).ToList() ;

//                   foreach (var xcol in xcols)
//                   {
//                       var cinfo = tname + "." + xcol.Attribute(TextConst.AName.Column).Value;
//                       if (!info.ContainsKey(cinfo))
//                       {
//                           info.Add(cinfo, new List<string>());

//                       }

//                       if (!info[cinfo].Contains(query1.XName))
//                       {
//                           info[cinfo].Add(query1.XName);
//                       }
//                   }
                   


                   
//                }


               
//            }

//            foreach (var i in info)
//            {
//                sql.AppendLine(i.Key + "\t" + string.Join(",", i.Value));
//            }
//            Cmn.SaveText(sql.ToString() + sql.ToString(), textEdit1.EditValue.ToString(), Encoding.Unicode);
//            Process.Start(textEdit1.EditValue.ToString());
//        }
//        private void msbiViews(string userName)
//        {
//           // UIStatic.LoadProject("ipr");
//            UIStatic.LoadProject("msbi");
//            msbiUserName = userName;
//            XmlReports.Environment.Manager.GetNativeScheme().Descendants().Attributes(TextConst.AName.Materialize).Remove();
//            XmlReports.Environment.Manager.GetScheme().Descendants().Attributes(TextConst.AName.Materialize).Remove();
//            var msbiQueries = XmlReports.Environment.GetElements(TextConst.EName.Queries).Where(q => Cmn.GetAttrValue(q, TextConst.AName.Name).StartsWith("msbi_")).ToList();
//            var sql = new StringBuilder();
//            var sqlRights = new StringBuilder();



//            List<string> tables = new List<string>();
//            foreach (VQuery query1 in msbiQueries)
//            {


//                var query = new XElement(query1);
//                sql.AppendLine("create or replace view");
//                sql.AppendLine(userName+ "." + query1.P_Name.Replace("msbi_", "msbi_"));
//                sql.AppendLine("as");

//                XElement compiledQuery = Compiler.GetCompiledAndProcessedQuery(query);

//                foreach (string tbl in compiledQuery.Descendants(TextConst.EName.Table).Attributes(TextConst.AName.Name).Select(a => a.Value).Distinct().ToList())
//                {
//                    if (!tables.Contains(tbl))
//                    {
//                        tables.Add(tbl);
//                        sqlRights.AppendLine("grant select on " + tbl + " to " + userName + "  WITH GRANT OPTION");
//                        sqlRights.AppendLine("/");
//                    }
//                }


//                string selectText = Compiler.GetQuerySelectStatmentFromCompiledQuery(compiledQuery);
//                sql.AppendLine(selectText);
//                sql.AppendLine("/");
//                sql.AppendLine("");
//                sql.AppendLine("grant select on " + query1.P_Name + " to public");
//                sql.AppendLine("/");
//                sql.AppendLine("");
//                sql.AppendLine("");
//                sql.AppendLine("");
//            }

//            sqlRights.AppendLine();
//            Cmn.SaveText(sqlRights.ToString() + sql.ToString(), textEdit1.EditValue.ToString(), Encoding.Unicode);
//            Process.Start(textEdit1.EditValue.ToString());
//        }

//        private void simpleButton25_Click(object sender, EventArgs e)
//        {
//            msbiMakeDoc(null, "msbi_info");
//        }

//        private void msbiMakeDoc(string repInfoStr, string templateName, bool getAll=false)
//        {
//            if (string.IsNullOrEmpty(msbiUserName)) msbiUserName = "MSBI_TEST";

//            var ds = new DataSet();

//            var tblInfo = new DataTable("tbl");
//            tblInfo.Columns.Add("name");
//            tblInfo.Columns.Add("title");
//            tblInfo.Columns.Add("key");
//            tblInfo.Columns.Add("treb_info");
//            tblInfo.Columns.Add("action_info");

//            var columnInfo = new DataTable("col");
//            columnInfo.Columns.Add("table");
//            columnInfo.Columns.Add("name");
//            columnInfo.Columns.Add("title");
//            //columnInfo.Columns.Add("comment");
//            columnInfo.Columns.Add("data_type");
//            columnInfo.Columns.Add("len", XmlReports.numberType);
//            columnInfo.Columns.Add("ref");
//            columnInfo.Columns.Add("treb_info");
//            ds.Tables.Add(tblInfo);
//            ds.Tables.Add(columnInfo);

//            columnInfo.ParentRelations.Add(new DataRelation("", tblInfo.Columns["name"], columnInfo.Columns["table"]));

//            var msbiQueries = XmlReports.Environment.GetElements(TextConst.EName.Queries).Where(q => Cmn.GetAttrValue(q, TextConst.AName.Name).StartsWith("msbi_")).Select(q1 => (VQuery)q1).OrderBy(q => q.P_Title).ToList();
//            var msbiQueriesAll = msbiQueries.ToList();
//            List<string> repInfo =null;
//            repInfo = repInfoStr.Split(',').ToList();
//            var inModNums = new List<int>();

//            foreach (string s in repInfo)
//            {
//				Int32 modNum;
//				Int32.TryParse(s.Replace("m", ""), out modNum);
//				//var modNum = Convert.ToInt32(s.Replace("m", ""));
//				if (modNum != null)
//				{
//					inModNums.Add(modNum);
//				}
//			}
//            if (repInfoStr != null && !getAll)
//            {
               
//                msbiQueries = msbiQueries.Where(
//                     e =>
//                          Cmn.GetAttrValue(e, TextConst.AName.Comment).Split(',').Any(s => repInfo.Contains(s.Split('.')[0]))
//                         ||
//                         e.Elements(TextConst.EName.Select).Elements().Where(
//                         e1 => Cmn.GetAttrValue(e1, TextConst.AName.Comment).Split(',').Any(s => repInfo.Contains(s.Split('.')[0]))

//                         ).Any()
//                     ).ToList();
//            }

//            var sql = new StringBuilder();
//            var colsTbl = db.ExecuteDataTable(
//            "select table_name,column_name,DATA_TYPE,DATA_LENGTH from all_tab_columns where table_name like 'MSBI_%' and owner='"+msbiUserName+"'" );

//            foreach (VQuery query in msbiQueries)
//            {
//                var keyCol = query.Columns().FirstOrDefault(c => Cmn.GetAttrValue(c, TextConst.AName.Key) == "1");

//                var keyName = "";
//                if (keyCol != null)
//                {
//                    keyName = (keyCol as VSXElement).XName;
//                }



//                var cols = query.Columns();
//                var r1=tblInfo.Rows.Add(query.P_Name.Replace("msbi_", "msbi_"), query.P_Title.Split('.')[1], keyName);
//                string tblTrebInfo = "";
             
//                if (repInfo != null && !getAll)
//                {
//                    cols = cols.Where(
//                             e1 => Cmn.GetAttrValue(query, TextConst.AName.Comment).Split(',').Any(s => repInfo.Contains(s.Split('.')[0])) ||
//                                   Cmn.GetAttrValue(e1, TextConst.AName.Comment).Split(',').Any(s => repInfo.Contains(s.Split('.')[0]))
//                             ).ToList();
//                }
//                bool isExtended = false;
//                foreach (VSXElement col in cols)
//                {
//                    object linkName = DBNull.Value;
//                    string title = col.P_Title;

//                    if (col.P_Key != "1")
//                    {
//                        linkName = col.P_CalledQuery;
//                        VQuery linkQuery = null;
//                        if (linkName == "")
//                        {
//                            linkQuery = msbiQueriesAll.Where(q => q.Columns().Any(c => c.P_Key == "1" && c.XName == col.XName)).FirstOrDefault();

//                        }
//                        else
//                        {
//                            linkQuery = msbiQueriesAll.Where(q => q.P_Name == linkName.ToString()).First();

//                        }
//                        if (linkQuery != null)
//                        {
//                            linkName = linkQuery.XName.Replace("msbi_", "msbi_");
//                            if (title == "")
//                            {
//                                title = linkQuery.P_Title.Split('.')[1];
//                            }
//                        }
//                    }
//                    else
//                    {

//                        if (title == "")
//                        {
//                            title = "ИД";
//                        }


//                    }

//                    var row = colsTbl.Rows.Cast<DataRow>().Where(r => r["TABLE_NAME"].ToString() == query.P_Name.ToUpper() && r["COLUMN_NAME"].ToString() == col.XName.ToUpper()).First();

//                    var r2 = columnInfo.Rows.Add(query.P_Name.Replace("msbi_", "msbi_"), col.XName, title, row["DATA_TYPE"], row["DATA_LENGTH"], linkName);
//                    if (repInfo != null)
//                    {
                       
//                        var modArr = col.P_Comment.Split(',').Select(s1 => s1.Split('.')[0] ).Distinct().ToList();
//                        bool isOld = false;
//                        foreach (string s in modArr)
//                        {
//                            var sm = s.Split('.')[0];
//                            var im = 0;
//                            if (sm.Contains("m"))
//                            {
//                                sm = sm.Replace("m", "");
//                                im = Convert.ToInt32(sm);
//                            }
//                            if (!inModNums.Any(i => i <= im)) { // модификация старше всех запрошенных
//                                isExtended = false;
                               
//                                break;
//                            }
//                            if (inModNums.Contains(im))
//                            {
//                                isExtended = true;
//                            }
//                        }


//                        var sitr_ar = col.P_Comment.Split(',').Where(s => repInfo.Contains(s.Split('.')[0]) && s.Split('.').Length > 1).Select(s1 => "(" + s1.Split('.')[1] + ")").Distinct().ToList();
                        
//                        sitr_ar.Sort();
//                        var sitr =
//                           string.Join("", sitr_ar

//                       );
//                        if (tblTrebInfo.IndexOf(sitr) == -1)
//                        {
//                            tblTrebInfo += sitr;
//                        }

//                        r2["treb_info"] = sitr;
//                    }

//                }

//                if (repInfoStr != null)
//                {
//                   var tblTrebInfoArr = query.P_Comment.Split(',').Where(s => repInfo.Contains(s.Split('.')[0])).ToArray();

//                    var tblTrebInfo1 = string.Join(",",
//                       tblTrebInfoArr.Where(s => s.Split('.').Length > 1).Select(s1 => "(" + s1.Split('.')[1] + ")").Distinct().ToArray()
//                       );
//                    if (tblTrebInfo.IndexOf(tblTrebInfo1) == -1)
//                    {
//                        tblTrebInfo += tblTrebInfo1;
//                    }
//                    if (tblTrebInfoArr.Length != 0 && isExtended) {
//                        r1["action_info"] = "Создать представление";
//                    }
//                    else if (isExtended)
//                    {
//                        r1["action_info"] = "Расширить представление";
//                    }
//                    else
//                    {
//                        // нет колонок для расширения
//                        r1["action_info"] = "Представление";
//                    }
                   
                       

//                }
//                r1["treb_info"] = tblTrebInfo;
//            }

//            //string fullPath = Printing.Print(ds, "msbi_info.xml", "Описание данных");
//            string fullPath = Printing.PrintWord(ds, templateName + ".docx", "Описание данных");

//            Cmn.OpenPrintedFile(fullPath);
//        }

//        private void simpleButton26_Click(object sender, EventArgs e)
//        {
//            msbiMakeDoc(textBox1.Text, "msbi_info_for_fts");
//        }

//        private void btnGenConstraintsExcel_Click(object sender, EventArgs e)
//        {
//            // структура такая
//            // начало со второй строки
//            // колонка 2 - родительская таблица 
//            // колонка 3 - зависимая таблица
//            // колонка 4 - имя констрэйнта
//            // колонка 5 - текущий признак on delete cascade (0 - нет, 1 - есть)
//            // колонка 6 - необходимый признак on delete cascade (0 - нет, 1 - есть)
//            // колонка 7 - имя колонки для связи
//            using (var dialog = new OpenFileDialog())
//            {
//                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
//                dialog.Filter = "(*.xlsx)|*.xlsx";
//                var result = dialog.ShowDialog();
//                if (result != DialogResult.OK) return;

//                using (var book = new Workbook())
//                {
//                    book.LoadDocument(dialog.FileName);
//                    var sheet = book.Worksheets.First();

//                    var sql = new StringBuilder();
//                    for (int i = 1; i <= sheet.Rows.LastUsedIndex; i++)
//                    {
//                        string ptable_name = sheet.Rows[i][1].DisplayText;
//                        if (string.IsNullOrEmpty(ptable_name)) break;

//                        string table_name = sheet.Rows[i][2].DisplayText;
//                        string constraint_name = sheet.Rows[i][3].DisplayText;
//                        string old_delete = sheet.Rows[i][4].DisplayText;
//                        string new_delete = sheet.Rows[i][5].DisplayText;
//                        string column_name = sheet.Rows[i][6].DisplayText;

//                        sql.AppendLine(string.Format("alter table {0} drop constraint {1};", table_name, constraint_name));
//                        sql.AppendLine("/");
//                        sql.AppendLine(string.Format("alter table {0} add constraint {1} foreign key ({2}) references {3}({2}){4};"
//                            ,table_name, constraint_name, column_name, ptable_name, (new_delete == "1" ? " on delete cascade" : "")));
//                        sql.AppendLine("/");
//                    }

//                    // В.Емцов - так гарантированно освобождаются занятые ресурсы
//                    using (var sw = new StreamWriter(new FileStream(textEdit1.EditValue.ToString(), FileMode.Create), Encoding.GetEncoding(1251)))
//                    {
//                        sw.Write(sql.ToString());
//                    }
//                    Process.Start(textEdit1.EditValue.ToString());
//                }
//            }
//        }

//        private void simpleButton27_Click(object sender, EventArgs e)
//        {
//            msbiViews("MSBI_TEST");
//        }

//        private void simpleButton28_Click(object sender, EventArgs e)
//        {
//            msbiMakeDoc(textBox1.Text, "msbi_info_for_fts",true);
//        }
        
//        private void btnScreenXlsx_Click(object sender, EventArgs e)
//        {
//            string filePath = null;
//            using (var dlg = new OpenFileDialog())
//            {
//                dlg.InitialDirectory = SqlBuilder.GetWorkFolderPath();
//                dlg.Filter = "(*.xlsx)|*.xlsx|(*.*)|*.*";
//                dlg.DefaultExt = "xlsx";

//                if (dlg.ShowDialog() != DialogResult.OK) return;

//                filePath = dlg.FileName;
//            }

//            if (string.IsNullOrEmpty(filePath)) return;

//            pbScreenXlsx.Position = 0;

//            var word = new Word.Application() {Visible = true};
//            word.Documents.Add();
//            word.ActiveWindow.ActivePane.View.Zoom.Percentage = 130;
//            word.Selection.PageSetup.Orientation = Word.WdOrientation.wdOrientLandscape;
//            var excel = new Excel.Application() {WindowState = Excel.XlWindowState.xlMaximized, DisplayFullScreen = false, Visible = false};
//            try
//            {
               
//                Excel.Workbook wb = excel.Workbooks.Open(filePath);
                
//                int colBegin = excel.ActiveWindow.ScrollColumn;
//                int rowBegin = excel.ActiveWindow.ScrollRow;
//                int colsStep = excel.ActiveWindow.VisibleRange.Columns.Count;
//                int rowsStep = excel.ActiveWindow.VisibleRange.Rows.Count;
//                int colsMax = excel.ActiveSheet.UsedRange.Columns.Count;
//                int rowsMax = excel.ActiveSheet.UsedRange.Rows.Count;

//                pbScreenXlsx.Properties.Maximum = colsMax*rowsMax;

//                for (int i = rowBegin; i < rowsMax + rowsStep; i += rowsStep)
//                {
//                    excel.ActiveWindow.ScrollRow = i;
//                    for (int j = colBegin; j < colsMax + colsStep; j += colsStep)
//                    {
//                        Clipboard.Clear();

//                        //string info = string.Format("{0}{1}:{2}{3}", ExcelUtils.GetColumnName(j), i, ExcelUtils.GetColumnName(j + colsStep - 1), (i + rowsStep - 1));
//                        //word.Selection.InsertAfter(info);
//                        //word.ActiveDocument.Range(word.Selection.End, word.Selection.End).Select();

//                        excel.ActiveWindow.ScrollColumn = j;
//                        var c1 = excel.ActiveWindow.VisibleRange.Column;
//                        var c2 = excel.ActiveWindow.VisibleRange.Column + excel.ActiveWindow.VisibleRange.Columns.Count - 1;
//                        excel.Visible = true;
               
//                        Excel.Range rr = excel.ActiveSheet.Range[excel.ActiveSheet.Cells[1, c1], excel.ActiveSheet.Cells[rowsMax, c2]];
//                      //  excel.ActiveWindow.VisibleRange.CopyPicture(Excel.XlPictureAppearance.xlScreen, Excel.XlCopyPictureFormat.xlBitmap);
//                        bool sucsess = false;
//                        while (!sucsess)
//                        {
//                            Application.DoEvents();
//                            try
//                            {
//                                rr.CopyPicture(Excel.XlPictureAppearance.xlScreen, Excel.XlCopyPictureFormat.xlBitmap);
//                                word.Activate();
//                                word.Selection.Paste();
//                                sucsess = true;
//                            }
//                            finally
//                            {
                               
//                            }
//                        }
                      
                      
//                        Application.DoEvents();


                     

//                        pbScreenXlsx.Position += rowsStep * colsStep;

//                        colsStep = excel.ActiveWindow.VisibleRange.Columns.Count;
//                        rowsStep = excel.ActiveWindow.VisibleRange.Rows.Count;

//                        if (pbScreenXlsx.Position > pbScreenXlsx.Properties.Maximum-1)
//                        {
//                            break;
//                        }
//                    }
//                    if (pbScreenXlsx.Position > pbScreenXlsx.Properties.Maximum-1)
//                    {
//                        break;
//                    }
//                }
//            }
//            finally
//            {
//                // вызов quit не убивает процесс excel.exe
//                Cmn.CloseExcel(ref excel);
//            }
//        }

//        private void simpleButton29_Click(object sender, EventArgs e)
//        {
//            string filePath = null;
//            using (var dlg = new OpenFileDialog())
//            {
//                dlg.InitialDirectory = SqlBuilder.GetWorkFolderPath();
//                dlg.Filter = "(*.xlsx)|*.xlsx|(*.*)|*.*";
//                dlg.DefaultExt = "xlsx";

//                if (dlg.ShowDialog() != DialogResult.OK) return;

//                filePath = dlg.FileName;

//                var excelDoc = new ExcelDocumentMM(filePath);
//                int rowIndex = 1;
//                try
//                {
//                    //string valueCol1 = "dfsfsdf";
//                    //string valueCol2 = "dfsdfsdf";
//                    //excelDoc.SetCellValue(valueCol1, rowIndex, 1);
//                    //excelDoc.SetCellValue(valueCol2, rowIndex, 2);
//                    excelDoc.UnMerge();
//                   // excelDoc.Merge_2();

//                }

//                catch (Exception error)
//                {
//                    excelDoc.Close();
//                    // обрабатываем саму ошибку
//                }
//                excelDoc.Visible = true;
//            }
//        }

//        private void simpleButton30_Click(object sender, EventArgs e)
//        {
//            string filePath = null;
//            using (var dlg = new OpenFileDialog())
//            {
//                dlg.InitialDirectory = SqlBuilder.GetWorkFolderPath();
//                dlg.Filter = "(*.xlsx)|*.xlsx|(*.*)|*.*";
//                dlg.DefaultExt = "xlsx";

//                if (dlg.ShowDialog() != DialogResult.OK) return;

//                filePath = dlg.FileName;

//                var excelDoc = new ExcelDocumentMM(filePath);
//                excelDoc.Visible = true;
//                int rowIndex = 1;
//                try
//                {
//                    //string valueCol1 = "dfsfsdf";
//                    //string valueCol2 = "dfsdfsdf";
//                    //excelDoc.SetCellValue(valueCol1, rowIndex, 1);
//                    //excelDoc.SetCellValue(valueCol2, rowIndex, 2);
//                    excelDoc.Merge();
//                    // excelDoc.Merge_2();

//                }

//                catch (Exception error)
//                {
//                    excelDoc.Close();
//                    // обрабатываем саму ошибку
//                }
               
//            }
//        }

//        private void simpleButton31_Click(object sender, EventArgs e)
//        {
//            msbiAllSorceColumnsInfo();
//        }

//        private void btnAllQueryTables_Click(object sender, EventArgs e)
//        {
//            var qname = lookUpEdit1.EditValue.ToString();
//            var xml = XmlReports.getItemProcessedXml2("query", qname, false);

//            var aa = xml.Descendants("table").Select(t => t.Attribute("name").Value).Distinct();

//            // В.Емцов - так гарантированно освобождаются занятые ресурсы
//            using (var sw = new StreamWriter(new FileStream(textEdit1.EditValue.ToString(), FileMode.Create), Encoding.GetEncoding(1251)))
//            {
//                sw.Write(string.Join("\r\n", aa));
//            }
            
//            Process.Start(textEdit1.EditValue.ToString());
//        }

//        private void simpleButton32_Click(object sender, EventArgs e)
//        {
//           // xlsoper_getRepNames();
//            // xlsoper_dimsReps()
//            //xlsoper_factsReps();
//            xlsoper_splitRepFiles();

//        }

//        private static void xlsoper_getRepNames()
//        {
//            var di = new DirectoryInfo(@"C:\Users\abelchenko\Desktop\МПЭП\Новая папка");

//            foreach (var fi in di.GetFiles())
//            {
//                var wb = new Workbook();
//                wb.LoadDocument(fi.FullName);
//                var sh = wb.Worksheets[0];
//                int i = 6;

//                bool ext = false;
//                while (!ext)
//                {
//                    var s = sh.Cells[i, 4].DisplayText;
//                    var s1 = sh.Cells[i, 5].DisplayText;

//                    if (string.IsNullOrEmpty(s))
//                    {
//                        ext = true;

//                    }
//                    else
//                    {
//                        db.ExecuteNonQuery("insert into s_rep (kod,name) values ('" + s + "','" + s1 + "')");
//                    }

//                    i++;
//                }
//                wb.Dispose();

//            }
//            db.Connection.Commit();
//        }
//        private static void xlsoper_splitRepFiles()
//        {
//            var di = new DirectoryInfo(@"C:\Users\abelchenko\Desktop\МПЭП\Новая папка");

//            foreach (var fi in di.GetFiles())
//            {
//                var wb = new Workbook();
//                wb.LoadDocument(fi.FullName);
                
//                foreach (var sh in wb.Worksheets)
//                {
//                    var wb1 = new Workbook();
//                    wb1.Worksheets[0].Name = sh.Name;
                 
//                    // Copy all information (content and formatting) to the newly created worksheet 
//                    // from the "Sheet1" worksheet.
//                    wb1.Worksheets[0].CopyFrom(sh);

//                    wb1.SaveDocument(Path.Combine(fi.Directory.FullName, sh.Name+".xlsx"));
//                    wb1.Dispose();
                    
//                }
                

//                wb.Dispose();

//            }
          
//        }

//        private static void xlsoper_dimsReps()
//        {
           
//                var wb = new Workbook();
//                wb.LoadDocument(@"C:\Users\abelchenko\Desktop\МПЭП\Анализ L+M+S+K.xlsx");
//                var sh = wb.Worksheets[0];
//                int i = 6;

//                bool ext = false;
//                int row = 1;
//                while (!ext)
//                {
//                    int col = 11;

//                    bool ext1 = false;
//                    var dimName = sh.Cells[row, 0].DisplayText;
//                    string dimId = db.ExecuteObject("select dim_id from s_dim where kod='" + dimName + "'").ToString();
                   
//                    while (!ext1)
//                    {

//                        var s = sh.Cells[row, col].DisplayText;
//                        if (!string.IsNullOrEmpty(s))
//                        {

//                            var ss = s.Split(',');
//                            foreach (var repName in ss)
//                            {
//                                string repId = db.ExecuteObject("select rep_id from s_rep where kod='" + repName + "'").ToString();
//                                db.ExecuteNonQuery("insert into s_rep_dim(rep_id,dim_id) values ('" + repId + "','" + dimId + "')");
//                            }
                            
//                        }
//                        col++;
//                        if (col == 62)
//                        {
//                            ext1 = true;
//                        }
//                    }
                   
//                    //if (string.IsNullOrEmpty(s))
//                    //{
//                    //    ext = true;

//                    //}
//                    //else
//                    //{
//                    //    db.ExecuteNonQuery("insert into s_rep (kod,name) values ('" + s + "','" + s1 + "')");
//                    //}

//                    row++;
//                    if (row == 24)
//                    {
//                        ext = true;
//                    }
//                }
//                wb.Dispose();

            
//            db.Connection.Commit();
//        }
//        private static void xlsoper_factsReps()
//        {

//            var wb = new Workbook();
//            wb.LoadDocument(@"C:\Users\abelchenko\Desktop\МПЭП\Анализ L+M+S+K.xlsx");
//            var sh = wb.Worksheets[0];
//            int i = 24;
//            db.ExecuteNonQuery("delete s_rep_msr");
//            db.ExecuteNonQuery("delete s_msr");
           
//            bool ext = false;
//            int row = 24;
//            int msrId = 0;
//            while (!ext)
//            {
                
//                var msrName = sh.Cells[row, 1].DisplayText;

//                List<Tuple<string, string, string, List<string>>> msrInfo = new List<Tuple<string, string, string, List<string>>>();

                
//                var ra1 = sh.Cells[row, 4].DisplayText;
//                var ra2 = sh.Cells[row, 5].DisplayText;
//                var ra3 = sh.Cells[row, 6].DisplayText;
//                var ra4 = sh.Cells[row, 7].DisplayText;
//                var rb1 = sh.Cells[row, 8].DisplayText;
//                var rb2 = sh.Cells[row, 9].DisplayText;
//                var rb3 = sh.Cells[row, 10].DisplayText;



//                var ra = new SortedList<int,string>();
//                if (!string.IsNullOrEmpty(ra1))
//                {
//                    ra.Add(1,ra1);
//                }
//                if (!string.IsNullOrEmpty(ra2))
//                {
//                    ra.Add(2, ra2);
//                }

//                if (!string.IsNullOrEmpty(ra3))
//                {
//                    ra.Add(3, ra3);
//                }

//                if (!string.IsNullOrEmpty(ra4))
//                {
//                    ra.Add(4, ra4);
//                }

//                var rb = new SortedList<int, string>();

//                if (!string.IsNullOrEmpty(rb1))
//                {
//                    rb.Add(1, rb1);
//                }
//                if (!string.IsNullOrEmpty(rb2))
//                {
//                    rb.Add(2, rb2);
//                }

//                if (!string.IsNullOrEmpty(rb3))
//                {
//                    rb.Add(3, rb3);
                    
//                }

//                int col = 4;

//                bool ext1 = false;
//                List<string> reps = new List<string>();
//                while (!ext1)
//                {

//                    var s = sh.Cells[row, col].DisplayText;
//                    var ss = s.Replace(" ", "").Split(',');
//                    if (!string.IsNullOrEmpty(s))
//                    {

//                        foreach (var repName1 in ss)
//                        {
//                            if (!reps.Contains(repName1))
//                            {
//                                reps.Add(repName1);
//                            }
//                        }

//                    }
//                    col++;
//                    if (col == 62)
//                    {
//                        ext1 = true;
//                    }
//                }

//                List<string> reps_a = new List<string>();
//                foreach (var v in ra)
//                {
//                    var ss = v.Value.Replace(" ", "").Split(',');
             
//                    foreach (var s in ss.Distinct().ToList())
//                    {
//                        if (!reps_a.Contains(s))
//                        {
//                            reps_a.Add(s);
//                        }
//                    }
//                }

//                List<string> reps_b = new List<string>();
//                foreach (var v in rb)
//                {
//                    var ss = v.Value.Replace(" ", "").Split(',');

//                    foreach (var s in ss.Distinct().ToList())
//                    {
//                        if (!reps_b.Contains(s))
//                        {
//                            reps_b.Add(s);
//                        }
//                    }
//                }

//                List<string> reps_an = new List<string>();
//                foreach (var s in reps)
//                {
//                    if (!reps_a.Contains(s))
//                    {
//                        reps_an.Add(s);
//                    }
//                }

               


//                List<string> reps_bn = new List<string>();
//                foreach (var s in reps)
//                {
//                    if (!reps_b.Contains(s))
//                    {
//                        reps_bn.Add(s);
//                    }
//                }
//                if (reps_an.Count != 0) {
//                    ra.Add(0, string.Join(",", reps_an));
//                }
//                if (reps_bn.Count != 0) {
//                    rb.Add(0, string.Join(",", reps_bn));
//                }
//                //if (!ra.Any())
//                //{
//                //    ra.Add(0, string.Join(",",reps));
//                //}
//                //if (!rb.Any())
//                //{
//                //    rb.Add(0, string.Join(",", reps));
//                //}
//                foreach (var va in ra)
//                {
//                    var vas = va.Key.ToString();
//                    if (vas == "0")
//                    {
//                        vas = "null";
//                    }
//                    foreach (var vb in rb)
//                    {
//                        var vbs = vb.Key.ToString();
//                        if (vbs == "0")
//                        {
//                            vbs = "null";
//                        }
//                        msrId++;
//                        var skod = msrId.ToString();

//                        skod = "M" + new string('0', 3-skod.Length)+skod;
//                        db.ExecuteNonQuery("insert into s_msr(msr_id,kod,name, planfact_id,vpc_id) values ('" + msrId.ToString() + "','" + skod + "','" + msrName + "'," + vas + "," + vbs + ")");

//                        var ssa = va.Value.Replace(" ", "").Split(',');
//                        var ssb = vb.Value.Replace(" ", "").Split(',');
                      
//                        foreach (var sa in ssa.Distinct().ToList())
//                        {

//                            if (ssb.Contains(sa))
//                            {
//                                string repId = db.ExecuteObject("select rep_id from s_rep where kod='" + sa + "'").ToString();
//                                db.ExecuteNonQuery("insert into s_rep_msr(rep_id,msr_id) values ('" + repId + "','" + msrId.ToString() + "')");
//                            }
//                        }
//                    }
//                }

//                //string dimId = db.ExecuteObject("select msr_id from s_msr where kod='" + msrName + "'").ToString();
//                //List<string> reps = new List<string>();
//                //while (!ext1)
//                //{
                 
//                //    var s = sh.Cells[row, col].DisplayText;
//                //      var ss = s.Replace(" ","").Split(',');
//                //    if (!string.IsNullOrEmpty(s))
//                //    {

//                //        foreach (var repName1 in ss)
//                //        {
//                //            if (!reps.Contains(repName1))
//                //            {
//                //                reps.Add(repName1);
//                //            }
//                //        }

//                //    }
//                //    col++;
//                //    if (col == 62)
//                //    {
//                //        ext1 = true;
//                //    }
//                //}

//                //foreach (var repName1 in reps)
//                //{
//                //    string repId = db.ExecuteObject("select rep_id from s_rep where kod='" + repName1 + "'").ToString();
//                //    db.ExecuteNonQuery("insert into s_rep_msr(rep_id,msr_id) values ('" + repId + "','" + dimId + "')");
//                //}
//                row++;
//                if (row == 91)
//                {
//                    ext = true;
//                }
//            }



            
//            wb.Dispose();


//            db.Connection.Commit();
//        }

   
//        private void simpleButton34_Click(object sender, EventArgs e)
//        {
//            MpedDoc.MpedList();
//        }

//        private void simpleButton35_Click(object sender, EventArgs e)
//        {
//            //var dic = new Dictionary<string, object>();
//            //dic.Add("date_s", new DateTime(2017, 10, 2));
//            //dic.Add("date_po", new DateTime(2017, 10, 14));
//            //var showPopupWaitForms = SqlBuilder.ShowPopupWaitForms;
//            //SqlBuilder.ShowPopupWaitForms = false;
//            //SqlBuilder.ExecReport("48300.48300", dic);
//            //SqlBuilder.ShowPopupWaitForms = showPopupWaitForms;
            
//            //ExcelLoadUtils.ExcelLoad();

//            var pars = new Dictionary<string, object>();
//            pars.Add("kod_zayav", (decimal)550506);
//            var showPopupWaitForms = SqlBuilder.ShowPopupWaitForms;
//            SqlBuilder.ShowPopupWaitForms = false;
//            SqlBuilder.ExecReportGetPath("48268.48268", pars);
//            SqlBuilder.ShowPopupWaitForms = showPopupWaitForms;
//        }

//        private void simpleButton36_Click(object sender, EventArgs e)
//        {
//            MP.Test.Test.Test4();

//            MessageBox.Show(DateTime.Now.ToString("T"));
//        }

//        private void simpleButton37_Click(object sender, EventArgs e)
//        {
//            var tableName = "bav_test_old_lk_info";
//            var s= ExcelLoadUtils.TableScriptFromExcel(@"C:\Users\abelchenko\Desktop\Выгрузка2.xlsx",tableName);
//            var fileName = Printing.outputFolder + @"\" + tableName + ".sql";
//            Cmn.SaveText(s, fileName, Encoding.Unicode);
//            Process.Start(fileName);
//        }

//		private void simpleButton38_Click(object sender, EventArgs e)
//		{
//			XmlReports.Environment.LoadProject("kido_asutp");
//			//XmlReports.Environment.LoadProject("asuse2");
//			//XmlReports.Environment.LoadProject("bar_ccb");
//			//XmlReports.Environment.LoadProject("mped");
//			StringBuilder sbTriggers = new StringBuilder();
//			string[] tables  = System.Array.ConvertAll(trHistTables.Text.Split(','), p => p.Trim());
//			foreach (string table in tables)
//			{
//				var query = (VQuery)XmlReports.Environment.GetElements(TextConst.EName.Queries).First(q => q.P_IdName == table);
//				sbTriggers.Append(SqlSchemeBuilder.GenerateHistoryTriggerScript(query));
//				sbTriggers.AppendLine();
//			}
//			var filename = Cmn.writeScriptFile("history_triggers", sbTriggers.ToString());
//			Process.Start(filename);
			
//		}


//        private string mpedDataPath = @"C:\TFS\root\main\all\plan.economic.analytics\ProjectData\va_data.xml";
//        private void simpleButton33_Click(object sender, EventArgs e1)
//        {

//            foreach (var p in XmlReports.Environment.Manager.GetAllProjects()) p.Unload();
//            XmlReports.Environment.Manager.LoadProjectIfNeed("mped");
//            var ds = new VDataSet();
//            foreach (VQuery q in XmlReports.Environment.GetElements(TextConst.EName.Queries).Where(q => q.P_Name.StartsWith("va_")))
//            {
//                var tbl = q.GetDescedantsP(EName.table).FirstOrDefault(e => e.P_Name.StartsWith("va_"));

//                if (tbl != null)
//                {
//                    var dtbl = db.ExecuteVDataTable("select * from " + tbl.P_Name);
//                    ds.Tables.Add(dtbl);
//                }
//            }
//            XElement xds = VDataSet.ToXml(ds, EName.root);
//            Cmn.SaveText(xds.ToString(), mpedDataPath, Encoding.UTF8);
//        }

//        private void simpleButton39_Click(object sender, EventArgs e)
//        {
//            var xd = XDocument.Load(mpedDataPath);
//            var ds = VDataSet.FromXml(xd.Root);

//            var sb = new StringBuilder();
//            foreach (VDataTable tbl in ds.Tables)
//            {
//                sb.Append(GenerateInsert(tbl));
//            }

//            var fname = Cmn.WriteFileToTemp("1.sql", sb.ToString());
//            Process.Start(fname);
//        }

//        private StringBuilder GenerateInsert(VDataTable tbl)
//        {
//            var sb = new StringBuilder();

//            var cCols = string.Join(",", tbl.Columns.Cast<DataColumn>().Select(c => c.ColumnName));
//            sb.AppendLine("begin");
//            sb.AppendLine("delete " + tbl.TableName + ";");
//            foreach (DataRow r in tbl.AsEnumerable())
//            {
//                var q = "";
//                  var sbVals = new StringBuilder();
//                foreach (VDataColumn col in tbl.Columns)
//                {
//                    var val = Cmn.ToOracleString(r[col]);
//                    sbVals.Append(q+val);
//                    q = ",";
//                }
              
//                sb.AppendLine(string.Format("insert into {0} ({1}) values({2});",tbl.TableName,cCols,sbVals));
//            }
//            sb.AppendLine("commit;");
//            sb.AppendLine("end;");
//            return sb;
//        }
//    }
//}
