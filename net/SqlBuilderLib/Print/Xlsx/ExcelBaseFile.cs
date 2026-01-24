using System.Xml.Linq;

namespace sql.builder.Print.Xlsx
{
    internal class ExcelBaseFile
    {
        #region Поля
        protected readonly string file_path;
        protected XDocument xml;
        private XDocument _xmlChanged;
        #endregion
        #region Свойства
        internal string FilePath { get { return this.file_path; } }
        internal XDocument XmlChanged {
            get {
                if (this._xmlChanged == null) {
                    this._xmlChanged = new XDocument(this.xml);
                }
                return this._xmlChanged;
            }
        }
        #endregion
        internal ExcelBaseFile(string file_path)
        {
            this.file_path = file_path;
            this.xml = XDocument.Load(file_path);
        }
        internal virtual void Save()
        {
            if (this._xmlChanged != null) {
                this._xmlChanged.Save(this.file_path);
            }
        }
    }
}