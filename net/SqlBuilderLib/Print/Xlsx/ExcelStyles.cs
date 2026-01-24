using System;
using Contract = System.Diagnostics.Contracts.Contract;
using System.Collections.Generic;
using System.Xml.Linq;

namespace sql.builder.Print.Xlsx
{
    internal class ExcelStyles : ExcelBaseFile
    {
        private Dictionary<string, ExcelStyle> _styles;
        internal ExcelStyles(string file_path)
            : base(file_path)
        {
            Contract.Assert(this.xml.Root.Name == ns.Main.styleSheet);
            this._styles = new Dictionary<string, ExcelStyle>();
            int i = 0;
            foreach (XElement xstyle in this.xml.Root.Element(ns.Main.cellXfs).Elements(ns.Main.xf)) {
                this._styles.Add(i.ToString(), new ExcelStyle(xstyle));
                i++;
            }
        }
        internal ExcelStyle GetStyle(string styleId)
        {
            if (string.IsNullOrEmpty(styleId)) {
                return null;
            } else {
                return this._styles[styleId];
            }
        }
    }
}