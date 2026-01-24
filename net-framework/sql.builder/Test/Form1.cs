using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
//using System.Windows.Forms;
using System.Xml.Linq;
using Devart.Data.Oracle;
//using DevExpress.XtraPrinting.Native;
using sql.builder.DataApi;

namespace sql.builder.Test
{
    public partial class Form1 : Form
    {
        const string PROJECT_NAME = "asuse2";

        private VSXElement _xroot;
        private Dictionary<string, Record> _dict;
        private HashSet<string> _asuse_queries;
        private HashSet<string> _ipr_queries;

        public Form1()
        {
            InitializeComponent();
            _dict = new Dictionary<string, Record>();
        }

        public void Init()
        {
            XmlReports.Environment.Manager.LoadProjectIfNeed(PROJECT_NAME);
            _xroot = XmlReports.Environment.Manager.GetAllProjects().First(p => p.Name == PROJECT_NAME).SchemeNative;

            var xitems = _xroot.GetElementsApplyingParts()
                .SelectMany(a => a.GetElementsApplyingParts())//.Where(e => e.AttrOrDef("name", "").StartsWith("us_zased")))
                .OrderBy(e => e.Attribute("file").Value);

            foreach (VSXElement xitem in xitems)
            {
                var rec = new Record(xitem);
                _dict.Add(rec.ID, rec);
            }
            gridControl1.DataSource = _dict.Values;

            Directory.CreateDirectory("Form1");
            _asuse_queries = new HashSet<string>(Tools.GetAsuseQueries());
            _ipr_queries = new HashSet<string>(Tools.GetIprQueries());
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Init();
        }

        private void Process()
        {
            foreach (var rec in _dict.Values)
            {
                var xcustomers = rec.XItem.Element("customers");
                if (xcustomers != null)
                {
                    if (xcustomers.Elements("customer").Any(e => e.Attribute("id").Value == "17"))
                    {
                        rec.IprRelsCount++;
                        rec.Processed = true;
                    }

                    if (xcustomers.Elements("customer").Any(e => e.Attribute("id").Value != "17"))
                    {
                        rec.AsuseRelsCount++;
                        rec.Processed = true;
                    }
                }

                if (rec.Type == "dimension-package")
                {
                    var ipr_names = new[] { "33535", "34397", "41050", "ipr", "ipr_fbu", "ipr_fin_kap", "ipr_struct", "kido" };
                    if (ipr_names.Contains(rec.XItem.Attribute("name").Value))
                    {
                        rec.IprRelsCount++;
                        rec.Processed = true;
                    }
                    else
                    {
                        rec.AsuseRelsCount++;
                        rec.Processed = true;
                    }
                }

                if (rec.Type == "query")// && rec.XItem.Descendants("table").Any())
                {
                    if (_ipr_queries.Contains(rec.Name))
                    {
                        rec.IprRelsCount++;
                        rec.Processed = true;
                    }
                    if (_asuse_queries.Contains(rec.Name))
                    {
                        rec.AsuseRelsCount++;
                        rec.Processed = true;
                    }
                }
            }

            foreach (var rec in _dict.Values)
            {
                ProcessItem(rec);
            }

            gridView1.RefreshData();
        }

        private void ProcessItem(Record rec)
        {
            rec.Processed = true;

            var useless_elements = new[] { "qube", "dimset", "template", "select", "where", "from" };

            var xchildren = rec.XItem.GetDescedantsApplyingParts()
                .Where(e => !useless_elements.Contains(e.Name.LocalName))
                .SelectMany(e =>
                {
                    try
                    {
                        return e.GetUsedElements();
                    }
                    catch
                    {
                        return Enumerable.Empty<VSXElement>();
                    }
                })
                .Where(e1 => e1 != null)
                .Select(e2 => e2.BaseElementOrSelf())
                .Where(e1 => e1 != null).ToArray();

            foreach (var xchild in xchildren)
            {
                string name = Tools.GetElementName(xchild);
                string id = xchild.Name.LocalName + ":" + name;

                Record child_rec = null;
                if (!_dict.TryGetValue(id, out  child_rec))
                {
                    continue;
                }

                if (!child_rec.Processed) ProcessItem(child_rec);

                rec.AsuseRelsCount += child_rec.AsuseRelsCount;
                rec.IprRelsCount += child_rec.IprRelsCount;
            }
        }

        //HashSet<VSXElement> hs = new HashSet<VSXElement>();
        //private void ProcessItem(VSXElement xitem, Record rec)
        //{
        //    //if (hs.Contains(xitem)) return;
        //    //hs.Add(xitem);

        //    if (rec != null) rec.Processed = true;

        //    //var useless_elements = new[] { "qube", "dimset", "template", "select", "where", "from" };
        //    var useless_elements = new[]{"template"};

        //    var xelements = xitem.GetElementsApplyingParts().Where(e => !useless_elements.Contains(e.Name.LocalName)).ToArray();
        //    var xused = xelements.SelectMany(e =>
        //    {
        //        try
        //        {
        //            return e.GetUsedElements();
        //        }
        //        catch
        //        {
        //            return Enumerable.Empty<VSXElement>();
        //        }
        //    })
        //    .Where(e1 => e1 != null)
        //    .Select(e2 => e2.BaseElementOrSelf())
        //    .Where(e1 => e1 != null).ToArray();

        //    var xchildren = xelements.Concat(xused).ToArray();

        //    if (xchildren.Any())
        //    {

        //    }

        //    foreach (var xchild in xchildren)
        //    {
        //        if (hs.Contains(xchild))
        //        {

        //        }
        //        hs.Add(xchild);
        //        string name = Tools.GetElementName(xchild);
        //        string id = xchild.Name.LocalName + ":" + name;

        //        Record child_rec = null;
        //        _dict.TryGetValue(id, out child_rec);

        //        if (child_rec == null || !child_rec.Processed)
        //        {
        //            ProcessItem(xchild, child_rec ?? rec);
        //        }

        //        if(rec != null && child_rec != null)
        //        {
        //            rec.AsuseRelsCount += child_rec.AsuseRelsCount;
        //            rec.IprRelsCount += child_rec.IprRelsCount;   
        //        }
        //    }
        //}

        private void gridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName.EndsWith("RelsCount"))
            {
                var rec = gridView1.GetRow(e.RowHandle) as Record;
                if (rec == null) return;

                e.Appearance.BackColor = Tools.GetRowColor(rec.AsuseRelsCount, rec.IprRelsCount, rec.CustomAsuse, rec.CustomIpr);
            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Process();
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            var row = gridView1.GetRow(e.FocusedRowHandle);
            if (row == null) return;

            var rec = row as Record;
            if (rec.XItem == null) return;

            memoEdit1.Text = rec.XItem.ToString();
        }

        private void gridView1_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                var rec = gridView1.GetRow(e.RowHandle) as Record;
                if (rec == null) return;

                e.Appearance.BackColor = Tools.GetRowColor(rec.AsuseRelsCount, rec.IprRelsCount, rec.CustomAsuse, rec.CustomIpr);
            }
            else
            {
                Hashtable ht = gridView1.GetGroupSummaryValues(e.RowHandle);
                if (ht == null) return;

                decimal asuse = Convert.ToDecimal(ht[gridView1.GroupSummary[0]]);
                decimal ipr = Convert.ToDecimal(ht[gridView1.GroupSummary[1]]);
                bool? asuse1 = (bool?)(ht[gridView1.GroupSummary[2]]);
                bool? ipr1 = (bool?)(ht[gridView1.GroupSummary[3]]);

                e.Appearance.BackColor = Tools.GetRowColor(asuse, ipr, asuse1, ipr1);
            }
        }

        private void repositoryItemCheckEdit1_EditValueChanged(object sender, EventArgs e)
        {
            gridView1.PostEditor();
            gridView1.RefreshData();
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var serializer = new DataContractSerializer(typeof(Record[]));
            using (var stream = new FileStream(@"Form1\records.xml", FileMode.Create))
            {
                serializer.WriteObject(stream, _dict.Values.ToArray());
            }
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs args)
        {
            var serializer = new DataContractSerializer(typeof(Record[]));
            using (var stream = new FileStream(@"Form1\records.xml", FileMode.Open))
            {
                var records = serializer.ReadObject(stream) as Record[];
                _dict = new Dictionary<string, Record>();
                foreach (var r in records)
                {
                    _dict.Add(r.ID, r);
                }
            }

            XmlReports.Environment.Manager.LoadProjectIfNeed(PROJECT_NAME);
            _xroot = XmlReports.Environment.Manager.GetAllProjects().First(p => p.Name == PROJECT_NAME).SchemeNative;

            var xitems = _xroot.GetElementsApplyingParts()
                .SelectMany(a => a.GetElementsApplyingParts())//.Where(e => e.AttrOrDef("name", "").StartsWith("us_zased")))
                .OrderBy(e => e.Attribute("file").Value);

            foreach (var xitem in xitems)
            {
                string id = xitem.Name.LocalName + ":" + Tools.GetElementName(xitem);
                Record rec = null;
                if(_dict.TryGetValue(id, out rec))
                {
                    rec.XItem = xitem;
                }
                else
                {
                    rec = new Record(xitem);
                    _dict.Add(rec.ID, rec);
                }
            }


            gridControl1.DataSource = _dict.Values;
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string root_old = XmlReports.getProjectPath() + @"sql.builder\source";
            //string root = Application.StartupPath + @"\Form1\source_new";
            string root = XmlReports.getProjectPath() + @"sql.builder\source2";

            string asuse2_folder = "asuse2";
            string ipr_folder = "ipr";
            string asuse2_ipr_folder = "asuse2_ipr";

            if (Directory.Exists(root)) Directory.Delete(root, true);
            Directory.CreateDirectory(root);

            string cur_folder = null;
            foreach (var group in _dict.Values.GroupBy(r => r.File))
            {
                bool asuse = group.Any(r => r.CustomAsuse == true || (!r.CustomAsuse.HasValue && r.AsuseRelsCount > 0));
                bool ipr = group.Any(r => r.CustomIpr == true || (!r.CustomIpr.HasValue && r.IprRelsCount > 0));

                if (ipr && !asuse) cur_folder = ipr_folder;
                else if (!ipr && asuse) cur_folder = asuse2_folder;
                else cur_folder = asuse2_ipr_folder;

                var path1 = root_old + group.Key;
                var path2 = root + @"\" + cur_folder + group.Key.Substring(7, group.Key.Length - 7);
                Directory.CreateDirectory(Path.GetDirectoryName(path2));
                File.Copy(path1, path2);
            }

            var dirs = Directory.EnumerateDirectories(root_old).Where(f => !f.EndsWith(PROJECT_NAME));
            foreach (var dir in dirs)
            {
                string new_dir = dir.Replace(root_old, root);
                Tools.CopyFilesRecursively(new DirectoryInfo(dir), new DirectoryInfo(new_dir));
            }

            var xprojects = XElement.Parse(_xprojects);
            xprojects.Save(root + @"\common\projects.xml");

            System.Diagnostics.Process.Start(root);
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.RowHandle < 0 && e.Button == MouseButtons.Right)
            {
                popupMenu1.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GetChildRows().ForEach(r => r.CustomAsuse = true);
            gridView1.RefreshData();
        }

        private IEnumerable<Record> GetChildRows()
        {
            var list = new List<Record>();

            var inds = gridView1.GetSelectedRows().Concat(new[] { gridView1.FocusedRowHandle }).Where(i => i < 0).Distinct();
            foreach (var groupRowHandle in inds)
            {
                int childRowCount = gridView1.GetChildRowCount(groupRowHandle);
                for (int i = 0; i < childRowCount; i++)
                {
                    int rowHandle = gridView1.GetChildRowHandle(groupRowHandle, i);
                    list.Add(gridView1.GetRow(rowHandle) as Record);
                }
            }

            return list;
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GetChildRows().ForEach(r => r.CustomAsuse = false);
            gridView1.RefreshData();
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GetChildRows().ForEach(r => r.CustomAsuse = null);
            gridView1.RefreshData();
        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GetChildRows().ForEach(r => r.CustomIpr = true);
            gridView1.RefreshData();
        }

        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GetChildRows().ForEach(r => r.CustomIpr = false);
            gridView1.RefreshData();
        }

        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GetChildRows().ForEach(r => r.CustomIpr = null);
            gridView1.RefreshData();
        }

        const string _xprojects =
            @"<root>
  <projects>
    <project name=""common"">
      <references/>
    </project>
    <project name=""asuse1"">
      <references>
        <reference project=""common""/>
      </references>
    </project>
    <project name=""asuse2_ipr"">
      <references>
        <reference project=""common""/>
      </references>
    </project>
	<project name=""asuse2"">
      <references>
        <reference project=""common""/>
		<reference project=""asuse2_ipr""/>
      </references>
    </project>
	<project name=""ipr"">
      <references>
        <reference project=""common""/>
		<reference project=""asuse2_ipr""/>
      </references>
    </project>
    <project name=""psk"">
      <references>
        <reference project=""common""/>
      </references>
    </project>
    <project name=""servicedesk"">
      <references>
        <reference project=""common""/>
      </references>
    </project>
    <project name=""teptes"">
      <references>
        <reference project=""common""/>
      </references>
    </project>
  </projects>
</root>";
    }

    [DataContract]
    internal class Record
    {
        private decimal _asuseRelsCount;

        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public string File { get; set; }

        public VSXElement XItem { get; set; }
        [DataMember]
        public bool Processed { get; set; }

        [DataMember]
        public decimal AsuseRelsCount
        {
            get { return _asuseRelsCount; }
            set
            {
                _asuseRelsCount = value;
            }
        }

        [DataMember]
        public decimal IprRelsCount { get; set; }
        [DataMember]
        public bool? CustomAsuse { get; set; }
        [DataMember]
        public bool? CustomIpr { get; set; }

        public Record(VSXElement xitem)
        {
            XItem = xitem;
            Type = xitem.Name.LocalName;
            Name = Tools.GetElementName(xitem);
            File = xitem.Attribute("file").Value;
            ID = Type + ":" + Name;
        }

        public override string ToString()
        {
            return ID;
        }

        public override bool Equals(object obj)
        {
            return ID == ((Record)obj).ID;
        }
    }

    internal static class Tools
    {
        internal static string GetElementName(VSXElement xitem)
        {
            //if (xitem.Parent == null || xitem.Parent.Parent == null || xitem.Parent.Parent.Name.LocalName != "root")
            //{
            //    return null;
            //}

            var parent = xitem.Parent;
            if (parent != null && parent.Attribute("key-name") != null)
            {
                return xitem.Attribute(parent.Attribute("key-name").Value).Value;
            }
            else
            {
                return xitem.ToString();
            }
            //else if (xitem.Name == "part" || xitem.Name == "field")
            //{
            //    return xitem.Attribute("id").Value;
            //}
            //else if (xitem.Name == "navigationitem")
            //{
            //    return xitem.Attribute("title").Value;
            //}
            //else if (xitem.Name == "call")
            //{
            //    return xitem.Attribute("function").Value;
            //}
            //else if (xitem.Attribute("name") != null)
            //{
            //    return xitem.Attribute("name").Value;
            //}
            //else
            //{
            //    return null;
            //}
        }

        public static string[] GetAsuseQueries()
        {
            string file = @"Form1\asuse_queries.txt";
            if (File.Exists(file))
            {
                return File.ReadAllLines(file);
            }
            else
            {
                const string connectionString = "data source=asuse;user id=asuse;password=kl0pik";
                var temp = db.Connection;
                string[] queries = null;
                using (db.Connection = new OracleConnection(connectionString))
                {
                    db.Connection.Open();
                    var a1 = db.GetTables("asuse").AsEnumerable().Select(r => ((string)r["TABLE_NAME"]).ToLower());
                    var a2 = db.GetViews("asuse").AsEnumerable().Select(r => ((string)r["VIEW_NAME"]).ToLower());
                    queries = a1.Concat(a2).ToArray();
                }

                db.Connection = temp;

                File.WriteAllLines(file, queries);

                return queries;
            }
        }

        public static string[] GetIprQueries()
        {
            string file = @"Form1\ipr_queries.txt";
            if (File.Exists(file))
            {
                return File.ReadAllLines(file);
            }
            else
            {
                const string connectionString = "data source=alpha;user id=plan;password=qwaser";
                var temp = db.Connection;
                string[] queries = null;
                using (db.Connection = new OracleConnection(connectionString))
                {
                    db.Connection.Open();
                    var a1 = db.GetTables("plan").AsEnumerable().Select(r => ((string)r["TABLE_NAME"]).ToLower());
                    var a2 = db.GetTables("pres").AsEnumerable().Select(r => ((string)r["TABLE_NAME"]).ToLower());
                    var a3 = db.GetTables("msbi_test").AsEnumerable().Select(r => ((string)r["TABLE_NAME"]).ToLower());
                    var a4 = db.GetViews("plan").AsEnumerable().Select(r => ((string)r["VIEW_NAME"]).ToLower());
                    var a5 = db.GetViews("pres").AsEnumerable().Select(r => ((string)r["VIEW_NAME"]).ToLower());
                    var a6 = db.GetViews("msbi_test").AsEnumerable().Select(r => ((string)r["VIEW_NAME"]).ToLower());
                    queries = a1.Concat(a2).Concat(a3).Concat(a4).Concat(a5).Concat(a6).ToArray();
                }

                db.Connection = temp;

                File.WriteAllLines(file, queries);

                return queries;
            }
        }

        public static Color GetRowColor(decimal asuse, decimal ipr, bool? asuse1, bool? ipr1)
        {
            if (asuse1.HasValue)
            {
                asuse = (asuse1.Value) ? 1 : 0;
            }

            if (ipr1.HasValue)
            {
                ipr = (ipr1.Value) ? 1 : 0;
            }

            var color = Color.Transparent;
            if (asuse == 0 && ipr > 0) color = Color.LightPink;
            else if (asuse > 0 && ipr == 0) color = Color.LightGreen;
            else if (asuse > 0 && ipr > 0) color = Color.Khaki;

            return color;
        }

        public static void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target)
        {
            foreach (DirectoryInfo dir in source.GetDirectories())
                CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name));
            foreach (FileInfo file in source.GetFiles())
            {
                Directory.CreateDirectory(Path.Combine(target.FullName));
                file.CopyTo(Path.Combine(target.FullName, file.Name));
            }
        }

    }
}
