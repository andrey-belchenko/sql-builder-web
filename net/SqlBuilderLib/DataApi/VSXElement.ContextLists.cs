using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.IO;
//using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using sql.builder.FieldInfo;
using System.Reflection;
namespace sql.builder.DataApi
{
    internal partial class VSXElement : VXElement
    {


        #region Вспомогательные вызовы
        public virtual void MakeChildContextLists(XElement xlists)
        {

            
        }

        public virtual void MakeFunctionList(List<XElement> list, string typ)
        {
           
            foreach (XElement el in FunctionsListForType(typ))
            {
                list.Add(
                    new XElement("call", new XAttribute("function", Cmn.GetAttrValue(el, "name")))
                    );
            }
          
        }


        public virtual void MakeColumnsList(List<XElement> list,bool doNameCheck)
        {
            NameCheck nameCheck = null;
            if (doNameCheck)
            {
                nameCheck = new NameCheck(RootQuery().Columns().SelectAsArray(e => e.XName));
            }
           
            foreach (VQueryCall qry in RootQuery().AllSources())
            {
                VQuery query = qry.Query();
                if (query != null)
                {
                    foreach (VSXElement col in query.Columns())
                    {
                        XElement newCol = new XElement("column", new XAttribute("table", qry.XName), new XAttribute("column", col.XName));

                        if (doNameCheck)
                        {
                            string alias = nameCheck.GetAlias(newCol.Attribute("column").Value);

                            if (alias != "")
                            {
                                newCol.SetAttributeValue("as", alias);
                            }
                        }
                        list.Add(
                            newCol
                           );
                    }
                }
            }
        }

        public XElement CreateContextListsElement()
        {

            return new XElement("lists");

        }

        public XElement CreateContextListElement(XElement xlists, string name)
        {

            var xlist = new XElement("list", new XAttribute("name",name));
            xlists.Add(xlist);
            return xlist;
            

        }


        #endregion


        #region Основные вызовы



        public VContextListsInfo GetContextListInfo(VContextListsType listType)
        {
            VContextListsInfo info = null;
            switch (listType)
            {
                case VContextListsType.Column: info = CL_Column_Info();
                    break;
                case VContextListsType.Call: info = CL_Call_Info();
                    break;
                default: info= null;
                    break;
            }
            info.ListType = listType;
            info.Element = this;
            return info;
        }
        public VDataTable GetContextListContent(VContextListsType listType)
        {
            List<XElement> list = new List<XElement>();
            switch (listType) {
                case VContextListsType.Column:
                    CL_Column_Content(list);
                    break;
                case VContextListsType.Call:
                    CL_Call_Content(list);
                    break;
                default:
                    break;
            }
            VDataTable table = new VDataTable();
            for (int id = 0; id < list.Count; id++) {
                VSXElement item = VSXElement.Get(list[id]);
                item.VirtualParent = this;
                //DataRow row = item1.ToDataRowSingle(table, true, i);
                DataRow row = table.Rows.Add();
                IList<string> names = this.GetPropNames();
                for (int index = 0; index < names.Count; index++) {
                    string name = names[index];
                    string property = PropPfx + name;
                    AddSpecColumns(table);
                    if (VFieldInfo.Exists(item, property)) {
                        VDataColumn col = (VDataColumn)table.Columns[name];
                        if (col == null) {
                            col = item.CreateColumn(name);
                            table.Columns.Add(col);
                        }
                        col.Visible = VFieldInfo.VisibleInTable(item, property);
                        if (col.Visible) {
                            row[col] = VFieldInfo.GetValue(item, property);
                        }
                        row["id"] = id.ToString();
                    }
                }
                row["node"] = item;
                item.Row = row;
            }
            return table;
        }
        public  virtual List<VContextListsType> ContextListAllowedTypes()
        {
            return new List<VContextListsType>();
        }

        public virtual VContextListsInfo CL_Column_Info()
        {
            return new VContextListsInfo(TextConst.EName.Column, TextConst.EName.Column);
        }
        public virtual void CL_Column_Content(List<XElement> list)
        {
            
        }
        public virtual VContextListsInfo CL_Call_Info()
        {
            return new VContextListsInfo(TextConst.EName.Call, TextConst.EName.Call);
        }

        public virtual void CL_Call_Content(List<XElement> list)
        {

        }


        #endregion


    }

    internal class VContextListsInfo
    {

        public VContextListsInfo(string name, string title)
        {
            Name = name;
            Title = title;
           
        }
        
        public VContextListsType ListType;
        public VSXElement Element;
        public string Name;
        public string Title;


        public VDataTable GetContent()
        {
            var tbl= Element.GetContextListContent(ListType);
            tbl.TableName = Name;
            return tbl;
        }
    }

    internal enum VContextListsType
    {
        Column,
        Call
    }

    
}
