using System.Xml.Linq;
using sql.builder.DataApi;

namespace sql.builder.Test
{
    interface ITree : IXmlParseble, ISelectInfo, ILevelChangeble
    {
        bool AddNext();
        bool AddSibling();
        bool Delete();

        void ShowFooter(bool show);

        void SetMode(Mode mode);
        void SetDataSource(VDataSet source);
    }


    interface IDataView
    {
        
    }

    interface IXmlParseble
    {
        void LoadFromXml(XElement xroot);
        void SaveToXml(XElement xroot);
    }

    interface IViewChangeble
    {
        void SetCurrentView(string id);
        string GetCurrentView();
    }

    interface ILevelChangeble
    {
        void SetCurrentLevel(string level);
        string GetCurrentLevel();
    }

    interface ISelectInfo
    {

    }

    internal enum Mode
    {
        None,
        View,
        Edit,
        Select
    }
}
