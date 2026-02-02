using System.Xml.Linq;

namespace sql.builder.Print.Xlsx
{
    public class ExcelBaseFile
    {
        #region ����
        protected readonly string file_path;
        protected XDocument xml;
        private XDocument _xmlChanged;
        #endregion
        #region ��������
        public string FilePath { get { return this.file_path; } }
        public XDocument XmlChanged {
            get {
                if (this._xmlChanged == null) {
                    this._xmlChanged = new XDocument(this.xml);
                }
                return this._xmlChanged;
            }
        }
        #endregion
        public ExcelBaseFile(string file_path)
        {
            this.file_path = file_path;
            this.xml = XDocument.Load(file_path);
        }
        public virtual void Save()
        {
            if (this._xmlChanged != null) {
                this._xmlChanged.Save(this.file_path);
            }
        }
    }
}