using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Diagnostics;
using AName_ = sql.builder.DataApi.AName;

namespace sql.builder.DataApi
{
    /// <summary>
    /// &lt;usereport project="" report="" title="" /&gt;
    /// </summary>
    internal sealed class VUseReport : VSXElement
    {
        internal VUseReport()
            : base(EName.usereport)
        {
        }
        private static VSXElement GetReport(string report_name)
        {
            XElement report = XmlReports.Environment.Manager.GetNativeScheme().Elements(EName.reports).Elements(EName.report).SearchByAttribute(AName_.name, report_name);
            if (report == null) {
                report = XmlReports.Environment.Manager.GetNativeScheme().Elements(EName.queries).Elements(EName.query).Where(EPredicate.IsReport).SearchByAttribute(AName_.name, report_name);
            }
            if (report != null) {
                return VSXElement.Get(report);
            } else {
                return null;
            }
        }
        #region Report
        public override string P_Report {
            get {
                return base.P_Report;
            }
            set {
                base.P_Report = value;
                VSXElement rep = GetReport(value);
                if (rep != null) {
                    this.P_Invisible = rep.P_Invisible;
                    this.P_SelfTitle = rep.P_SelfTitle;
                    this.P_SecurityId = rep.P_SecurityId;
                    this.P_Project = Cmn.ExtractProjectName(rep.Attribute(AName_.file).Value);
                }
            }
        }
        public override bool P_Report_Exists()
        {
            return true;
        }
        public void P_Report_List(VDataTable table)
        {
            table.AddColumn("id", "Отчёт");
            table.AddColumn("title", "Заголовок");
        }
        public void P_Report_ListRefresh(VDataTable table)
        {
            table.Rows.Clear();
            HashSet<string> names = new HashSet<string>();
            IList<XElement> list = XmlReports.Environment.Manager.GetNativeScheme().Elements(EName.reports).Elements(EName.report).ToList();
            int index;
            string name;
            for (index = 0; index < list.Count; index++) {
                XElement report = list[index];
                name = report.AttrOrEmpty(AName_.name);
                if (names.Contains(name)) {
                    Debug.WriteLine("Отчёт c именем \"" + name + "\" дублируется, используйте XPath //reports/report[@name=\"" + name + "\"] для поиска дублей.");
                } else {
                    table.AddRow(name, report.AttrOrEmpty(AName_.title));
                    names.Add(name);
                }
            }
            list = XmlReports.Environment.Manager.GetNativeScheme().Elements(EName.queries).Elements(EName.query).Where(EPredicate.IsReport).ToList();
            for (index = 0; index < list.Count; index++) {
                XElement query = list[index];
                name = query.AttrOrEmpty(AName_.name);
                if (names.Contains(name)) {
                    Debug.WriteLine("Отчёт c именем \"" + name + "\" дублируется, используйте XPath //queries/query[@name=\"" + name + "\"] для поиска дублей.");
                } else {
                    table.AddRow(name, query.AttrOrEmpty(AName_.title));
                    names.Add(name);
                }
            }
        }
        #endregion
        #region NodeText
        public override string GetNodeInfo()
        {
            string report = this.P_Report;
            string s = this.P_NodeName + " " + Bold(report);
            VSXElement rep = GetReport(report);
            if (rep != null) {
                s += " " + Italic(rep.P_SelfTitle);
            }
            return s;
        }
        #endregion
        #region SelfTitle
        public override bool P_SelfTitle_Exists()
        {
            return true;
        }
        #endregion
        #region Invisible
        public override bool P_Invisible_Exists()
        {
            return true;
        }
        #endregion
        #region SecurityId
        public override bool P_SecurityId_Exists()
        {
            return true;
        }
        #endregion
        #region Project
        public override bool P_Project_Exists()
        {
            return true;
        }
        #endregion
    }
}