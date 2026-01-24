using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
namespace sql.builder.UI
{
    public interface IVControl
    {
          
    }

    public interface IVEnabledControl
    {
         void SetEnabled(bool value);

    }


    public interface IVMaskControl
    {
        void SetMask(string value);
        void SetMaxLength(int value);

    }

    public interface IVTagControl
    {
        object Tag { get; set; } 

    }

    public interface IVVisibleControl
    {
         void SetVisible(bool value);

    }

    public interface IVNormalControl
    {

    }

    public delegate void ValueChangeEventHandler(object value);
    public delegate void CellChangeEventHandler(int row,string column,object value);
    public delegate void CellEventHandler(string view ,int row, string column);
    public delegate void RowEventHandler(int row);

    public delegate void XmlValueChangeEventHandler(XElement value);
    //public delegate void SizeChangeEventHandler(int width, int height);
    public delegate void SimpleEventHandler();
  
    public delegate void CustomProcessEventHandler (TableViewerEventArgs viewEventArgs,CustomProcessEventArgs customProcessArgs);
    public class CustomProcessEventArgs
    {
        public bool Handled = false;
        public bool Cancel = false;
        public bool BoolValue = false;

        
    }
    public class TableViewerEventArgs
    {
        
        public object Value = null;
        public object Row;
       
        public object Column = null;
        
    }
}
