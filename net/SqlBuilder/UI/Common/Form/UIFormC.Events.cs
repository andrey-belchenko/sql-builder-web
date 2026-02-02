using System;
using System.Xml.Linq;
using System.Linq;
//using System.Windows.Forms;
using System.Collections.Generic;
using System.Data;
//using DevExpress.XtraLayout;
//using DevExpress.XtraBars;
//using DevExpress.XtraLayout.Utils;
//using infoenergo.core.Extensions;
using sql.builder.DataApi;
using sql.builder.Controls;
//using sql.builder.Controls.Grids;

namespace sql.builder.UI
{
    public partial class UIFormC : IForm
    {
        public SortedList<string, VSXElement> eventsTags = null;
        public void AddEventTag(string eventName, VSXElement tag)
        {
            if (eventsTags == null)
            {
                eventsTags = new SortedList<string, VSXElement>();
            }
            eventsTags.Add(eventName, tag);
        }
        public void RaiseUIEvent(string name)
        {
            if (eventsTags != null)
            {
                if (eventsTags.ContainsKey(name))
                {
                    throw new NotImplementedException();
                    //VUseAction.ExecuteAction(this._form_name, (VUseAction)eventsTags[name], this.dataSource, this, null, null, null);
                }
            }
        }
        public event Action<UIFormC> CustomSave = null;
        public void RaiseCustomSave()
        {
            if (CustomSave != null)
            {
                CustomSave(this);
            }
        }
        public event Action<UIFormC, XElement> OnButtonClick = null;
        public void RaiseButtonClick(XElement xinfo)
        {
            if (OnButtonClick != null)
            {
                OnButtonClick(this, xinfo);
            }
        }
        public void UpdateEvents( XElement xevents)
        {
            if (xevents == null)
            {
                return;
            }
            foreach (XElement xcmd in xevents.Elements(TextConst.EName.UseAction))
            {
                var action = Cmn.GetActionInfo(xcmd);
                AddEventTag(xcmd.Attribute(TextConst.AName.EventName).Value, action);
            }
        }
    }
    public class UIEventArgs : EventArgs
    {
        public VSXElement ActionInfo;
        public DataRow Row;
        public VDataTable Table;
        public VDataColumn Column;
        public string EventName;
        public UIEventArgs(string eventName, VSXElement actionInfo, VDataTable table, DataRow row, VDataColumn column)
        {
            EventName = eventName;
            ActionInfo = actionInfo;
            Row = row;
            Table = table;
            Column = column;
        }         
    }
    public delegate bool UIEventHandler(object sender, UIEventArgs e);
}
