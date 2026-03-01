using System.Xml.Linq;
using sql.builder.DataApi;

namespace sql.builder.Print.Xlsx
{
    public class ExcelCell : ExcelBaseItem
    {
        public static bool IsHeadMarker(ExcelCell cell)
        {
            return cell.text == TextConst.ExcelMarks.HeadMarker;
        }
        private readonly ExcelRow row;
        private readonly ExcelCellInfo cell_info;
        private readonly string style_id;
        private string value;
        private string text;
        private bool has_shared_string;
        private bool has_formula;
        private bool has_shared_formula;
        private ExcelFormula formula;
        private ExcelRefToken formula_ref;
        public ExcelCell(XElement xml, ExcelPrintEnv env, ExcelRow row)
            : base(xml, env)
        {
            XElement xv = xml.Element(ns.Main.v);
            XElement xf = xml.Element(ns.Main.f);
            XAttribute at = xml.Attribute(ns.None.t);
            this.ID = xml.Attribute(ns.None.r).Value;
            this.row = row;
            this.style_id = xml.AttrOrDefault(ns.None.s, string.Empty);
            this.has_shared_string = at != null && at.Value == "s";
            if (this.has_shared_string)
            {
                this.value = xv.Value;
                this.text = env.SharedStrings.GetStringByIndex(this.value);
            }
            else if (xv != null)
            {
                this.value = xv.Value;
                this.text = xv.Value;
            }
            else
            {
                this.value = string.Empty;
                this.text = string.Empty;
            }
            this.has_formula = (xf != null);
            // чтобы значения пересчитались при открытии
            if (this.has_formula && xv != null)
            {
                xv.Remove();
            }
            this.cell_info = new ExcelCellInfo(this.ID);
            if (this.has_formula)
            {
                at = xf.Attribute(ns.None.t);
                this.has_shared_formula = (at != null && at.Value == "shared");
                if (this.has_shared_formula)
                {
                    XAttribute aref = xf.Attribute(ns.None.ref_);
                    if (aref != null)
                    {
                        this.formula_ref = new ExcelRefToken(aref.Value);
                    }
                }
                if (this.formula_ref != null || !this.has_shared_formula)
                {
                    this.formula = new ExcelFormula(xf.Value, this);
                }
            }
        }
        public ExcelRow Row { get { return this.row; } }
        public ExcelCellInfo CellInfo { get { return this.cell_info; } }
        public string Value { get { return this.value; } }
        public string Text { get { return this.text; } }
        public string StyleID { get { return this.style_id; } }
        public bool HasSharedString { get { return this.has_shared_string; } }
        public bool HasFormula { get { return this.has_formula; } }
        //public bool HasRefFormula { get { return this.formula_ref != null; } }
        public ExcelRefToken FormulaRef { get { return this.formula_ref; } }
        public ExcelFormula Formula { get { return this.formula; } }
        public ExcelCell Copy(ExcelRow row, string column_name)
        {
            XElement xml = new XElement(this.Xml);
            // пока формулы будут теряться - не придумал как сделать по-нормальному
            if (this.has_formula)
            {
                // реализовать copy для ExcelFormula!!
                xml.Element(ns.Main.f).Value = this.formula.GetText();
            }
            ExcelCell cell = new ExcelCell(xml, this.Env, row);
            cell.ChangeColumnName(column_name);
            return cell;
        }
        public void ChangeColumnName(string colNameNew)
        {
            // теперь не нужно
            //if (HasFormula)
            //{
            //int delta = ExcelUtils.GetColumnNumber(colNameNew) - ExcelUtils.GetColumnNumber(ColumnName);
            //Formula.Move(delta);

            //var xf = Xml.Element(ns.main + "f");
            //xf.Value = ExcelUtils.CorrectFormulaReferences(xf.Value, delta, 0);
            //}
            this.cell_info.ColumnName = colNameNew;
            this.ID = this.cell_info.CellName;
            this.Xml.Attribute(ns.None.r).Value = this.ID;
        }
        //public void CopyFormula(ExcelCell cellSource)
        //{
        //    Xml.Element(ns.main + "f").ReplaceWith(cellSource.Xml.Element(ns.main + "f"));
        //    HasSharedFormula = cellSource.HasSharedFormula;
        //    HasFormula = cellSource.HasFormula;
        //    HasRefFormula = cellSource.HasRefFormula;
        //    FormulaRef = cellSource.FormulaRef;
        //    if (FormulaRef.Changed) Xml.Element(ns.main + "f").SetAttributeValue("ref", FormulaRef.GetText());
        //}
        public void SetFormula(string formula)
        {
            XElement xf = this.Xml.Element(ns.Main.f);
            if (xf == null)
            {
                xf = new XElement(ns.Main.f);
                this.Xml.Add(xf);
            }
            xf.SetValue(formula);
            this.has_formula = true;
            this.formula = new ExcelFormula(formula, this);
            if (this.has_shared_formula)
            {
                xf.RemoveAttribute(ns.None.t);
                this.has_shared_formula = false;
            }
            // чтобы значения пересчитались при открытии
            this.Xml.RemoveElement(ns.Main.v);
        }
        public void CopyValue(ExcelCell cellSource)
        {
            XElement xv = this.Xml.Element(ns.Main.v);
            XElement xv2 = cellSource.Xml.Element(ns.Main.v);
            if (xv != null)
            {
                xv.ReplaceWith(xv2);
            }
            else
            {
                this.Xml.Add(xv2);
            }
            this.value = cellSource.Value;
            this.text = cellSource.Text;
            this.Xml.SetAttributeValue(ns.None.t, cellSource.Xml.AttrOrDefault(ns.None.t, null));
            this.has_shared_string = cellSource.has_shared_string;
        }
        public void SetValue(string value)
        {
            this.text = value;
            if (this.has_shared_string)
            {
                int index = this.Env.SharedStrings.InternStringAndGetIndex(value);
                this.Xml.Element(ns.Main.v).Value = index.ToString();
                this.value = index.ToString();
            }
            else
            {
                // возможно нужна проверка, что значение число
                this.Xml.Element(ns.Main.v).Value = value;
                this.value = value;
            }
        }
        public void DeleteFormulaRef()
        {
            if (this.formula_ref != null)
            {
                XElement xf = this.Xml.Element(ns.Main.f);
                xf.RemoveAttribute(ns.None.ref_);
                xf.RemoveAttribute(ns.None.t);
                this.formula_ref = null;
                this.has_shared_formula = false;
            }
        }
    }
}