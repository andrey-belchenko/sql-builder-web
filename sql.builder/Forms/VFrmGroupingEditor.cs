//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Xml.Linq;

//using sql.builder.Controls;
//using sql.builder.DataApi;
//using sql.builder.FieldInfo;
//using sql.builder.Controls.FormFields;
//using sql.builder.Controls.Grids;
//using sql.builder.XmlHelpers;
//using sql.builder.Exceptions;
//using sql.builder;
//using sql.builder.UI;
//namespace sql.builder.VForms
//{
//    class VFrmGroupingEditor
//    {
//        public VFrmGroupingEditor(string reportName)
//        {
//            _reportName = reportName;
//        }


//        private VGrouping preset = null;
//        public void SetGroupInfo(XElement xgrouping)
//        {
//            if (xgrouping == null)
//            {
//                xgrouping=new XElement(TextConst.EName.Grouping);
//                xgrouping.Add(createEmptyGrset());
//            }
//            preset = VSXElement.Get<VGrouping>(xgrouping);
//            preset.VirtualParent = getQuery();
          
//            updateGrsetsIds(preset);
//        }

//        private void updateGrsetsIds(VGrouping gouping)
//        {

//            int i = 1;
//            foreach (var grset in gouping.GetDescedantsP(EName.grset))
//            {
//                grset.P_Alias="a"+i.ToString();
//                i++;
//            }
//            updateGrsetsNums(gouping, "");
//        }


//        private void updateGrsetsGrpInfo(VGrouping gouping)
//        {

            
//            foreach (var grset in gouping.GetDescedantsP(EName.grset))
//            {
//                var s = "";
//                var q = "";
//                foreach (var link in grset.Elements(TextConst.EName.Group).Elements(TextConst.EName.SourceLink).ToArray())
//                {
//                    s += q + link.Attribute(TextConst.AName.Title).Value;
//                    q = "; ";
//                }
//                grset.SetAttributeValue(TextConst.AName.Comment, s);
//            }
           
//        }

//        private void updateGrsetsInfo(VGrouping gouping)
//        {
//            updateGrsetsNums(gouping, "");
//            updateGrsetsGrpInfo(gouping);
//        }


//        private void updateGrsetsNums(XElement parent, string parentNum)
//        {

//            int i = 1;

//            if (parentNum != "")
//            {
//                parentNum += ".";
//            }

//            foreach (var grset in parent.Elements(TextConst.EName.Grset))
//            {
//                var num = parentNum + i.ToString();
//                grset.SetAttributeValue(TextConst.AName.Title, num);
//                updateGrsetsNums(grset, num);
//                i++;
//            }
//        }


//        private XElement createEmptyGrset()
//        {
//            var el = new XElement(TextConst.EName.Grset, new XElement(TextConst.AName.Group));
//            return el;
//        }
//        UIFormC _form = null;
//        public void Show()
//        {
          
//             UIStatic.LoadProject("system");
//             var form = UIStatic.CreateSystemForm("sys_grset", true);
//             if (_form != form)
//             {
//                 form.ChangeActionComplete += form_UserChangedData;
//                 form.OnButtonClick += form_OnButtonClick;
//                 form.DataSource.GetTable("a").CustomFill += ucMainReports_CustomFill_a;
//                 form.DataSource.GetTable("b").CustomFill += ucMainReports_CustomFill_b;
//                 form.DataSource.GetTable("c").CustomFill += ucMainReports_CustomFill_c;

//                 form.DataSource.GetTable("a").CustomRowSave_Added += FrmGroupingEditor_CustomRowSave_Added_a;

//                 form.DataSource.GetTable("a").CustomRowSave_Deleted += FrmGroupingEditor_CustomRowSave_Deleted_a;

//                 form.DataSource.GetTable("c").CustomRowSave_Added += FrmGroupingEditor_CustomRowSave_Added_c;

//                 form.DataSource.GetTable("c").CustomRowSave_Deleted += FrmGroupingEditor_CustomRowSave_Deleted_c;
//             }


//            form.LoadData(null);
//            form.ShowDialog();
//            _form = form;
//        }

//        void form_OnButtonClick(UIFormC form, XElement value)
//        {
//            if (Cmn.GetAttrValue(value, TextConst.AName.Name) == "apply")
//            {
//                commitSetting();
//            }
//        }
//        XElement _retPreset;
//        private void commitSetting()
//        {
//            updateGrsetsIds(preset);
//            _retPreset = preset;
//        }


//        public XElement GetGroupInfo()
//        {
//            return _retPreset;
//        }




//        private XElement getGrSetById(object id)
//        {
            
//            string sid = (string)Cmn.Nvl(id, "");
//            var grset = preset.Descendants(TextConst.EName.Grset).FirstOrDefault(e => Cmn.GetAttrValue(e, TextConst.AName.As) == sid);
//            return grset;
//        }

//        void FrmGroupingEditor_CustomRowSave_Added_a(DataRow row)
//        {
//            string parentId = (string)Cmn.Nvl(row["parent_id"], "");
//            XElement parent = preset;
//            if (parentId != "")
//            {
//                parent = getGrSetById(parentId);// preset.Descendants(TextConst.EName.Grset).First(e => Cmn.GetAttrValue(e, TextConst.AName.As) == parentId);
//            }
//            var grset = createEmptyGrset();
//            grset.SetAttributeValue(TextConst.AName.As, row["grset_id"].ToString());
//            parent.Add(grset);
//        }

//        void FrmGroupingEditor_CustomRowSave_Deleted_a(DataRow row)
//        {
//            string id = (string)Cmn.Nvl(row["grset_id",DataRowVersion.Original], "");
//            var grset = getGrSetById(id);// preset.Descendants(TextConst.EName.Grset).First(e => Cmn.GetAttrValue(e, TextConst.AName.As) == id);
//            grset.Remove();
//        }


//        void FrmGroupingEditor_CustomRowSave_Added_c(DataRow row)
//        {
//            var grset = getGrSetById(row["grset_id"]);
//            var grp = grset.Element(TextConst.AName.Group);
//            var link = new XElement(TextConst.EName.SourceLink);
//            var title = row.Table.DataSet.Tables["b"].Rows.Find(row["query_link_id"].ToString())["title"];
//            link.SetAttributeValue(TextConst.AName.Table,row["query_link_id"].ToString());
//            link.SetAttributeValue(TextConst.AName.Title, title);
//            grp.Add(link);

//            row["title"] = title;

//            //throw new NotImplementedException();
//        }

//        void FrmGroupingEditor_CustomRowSave_Deleted_c(DataRow row)
//        {
//            var grset = getGrSetById(row["grset_id", DataRowVersion.Original]);
//            var grp = grset.Element(TextConst.AName.Group);
//            var link = grp.Elements(TextConst.EName.SourceLink).First(e => Cmn.GetAttrValue(e, TextConst.AName.Table) == row["query_link_id", DataRowVersion.Original].ToString());
//            link.Remove();
//        }

//        void form_UserChangedData(object sender, EventArgs e)
//        {

//            (sender as UIFormC).SaveData(false);
//            var ds = (sender as UIFormC).DataSource;
//            //fillPresetFromDS(ds);
//            updateGrsetsInfo(preset);
//            updateDSFromPreset(ds);
//            (sender as UIFormC).SaveData(false);
//        }


//        private void updateDSFromPreset(VDataSet ds)
//        {
//            ds.GetTable("a").SuppressChangeEvent();
//            foreach (var grset in preset.GetDescedantsP(EName.grset))
//            {
//                var row = ds.GetTable("a").Rows.Find(grset.Attribute(AName.@as).Value);
//                row["title"] = grset.P_SelfTitle;
//                row["info"] = grset.P_Comment;
//            }
//            ds.GetTable("a").UnsuppressChangeEvent();
//        }

//        //private void fillPresetFromDS(VDataSet ds)
//        //{
//        //    var list1 = new SortedList<string, List< DataRow>>();
//        //    foreach (var row in ds.GetTable("a").AsEnumerable())
//        //    {
//        //        string parentId = (string)Cmn.Nvl(row["parent_id"], "");
               
//        //        if (!list1.ContainsKey(parentId))
//        //        {
//        //            list1.Add(parentId, new List<DataRow>());
//        //        }
//        //        list1[parentId].Add(row);
//        //    }
//        //    preset.Elements().Remove();
//        //    fillPresetFromDS_addChildGrsets(preset, list1, "");
//        //    updateGrsetsInfo(preset);
//        //}
//        //private void fillPresetFromDS_addChildGrsets(XElement parent, SortedList<string, List<DataRow>> pcList, string parentId)
//        //{
//        //    if (!pcList.ContainsKey(parentId)) return;
//        //    var childRows = pcList[parentId];

//        //    foreach (var row in childRows)
//        //    {
//        //        var grset = createEmptyGrset();
//        //        parent.Add(grset);
              
//        //        string id = (string) row["grset_id"];
//        //        grset.SetAttributeValue(TextConst.AName.Id, id);
//        //        fillPresetFromDS_addChildGrsets(grset, pcList, id);
//        //    }
//        //}

//        private string _reportName = null;

//        void ucMainReports_CustomFill_a(VDataTable table)
//        {

//            foreach (var grset in preset.GetDescedantsP(EName.grset))
//            {
//                var row = table.NewRow();
//                row["grset_id"] = grset.P_Alias;
//                row["title"] = grset.P_SelfTitle;
//                row["info"] = grset.P_Comment;
//                var par= grset.GetParent();
//                if  (par is VGrset)
//                {
//                    row["parent_id"] = par.P_Alias;
//                }
               
//                table.Rows.Add(row);
//            }
//            // List<DataRow> rows = new List<DataRow>();
           
//        }
//        private VQuery getQuery()
//        {

//            var qry = XmlReports.Environment.GetQuery(_reportName);
//            return qry;
//        }
//        void ucMainReports_CustomFill_b(VDataTable table)
//        {
//            var qry = getQuery();

//            foreach (var src in qry.AllSources())
//            {
//                if (src is VLink)
//                {
//                    var row = table.NewRow();
//                    row["query_link_id"] = src.XName;
//                    row["title"] = src.XTitle;
//                    table.Rows.Add(row);
//                }
//            }

//            // List<DataRow> rows = new List<DataRow>();

//        }

//        void ucMainReports_CustomFill_c(VDataTable table)
//        {
//            var parentId = table.GetDataSet().GetTable("a").CurrentRow["grset_id"];

//            var grset = getGrSetById(parentId);
//            if (grset == null) return;
//            foreach (var link in grset.Elements(TextConst.EName.Group).Elements(TextConst.EName.SourceLink).ToArray())
//            {
//                var row = table.NewRow();
//                row["query_link_id"] = link.Attribute(TextConst.AName.Table).Value;
//                row["title"] = link.Attribute(TextConst.AName.Title).Value;
//                row["grset_id"] = parentId;
//                table.Rows.Add(row);
//            }

//        }
//    }
//}
