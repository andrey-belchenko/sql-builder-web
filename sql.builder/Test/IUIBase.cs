//using System;
//using System.Collections.Generic;
//using System.Data;
////using System.Windows.Forms;
//using System.Xml.Linq;
//using DevExpress.XtraEditors;
//using infoenergo.core.Extensions;
//using sql.builder.DataApi;
//using sql.builder.UI;
//using Vertica.Data.Internal.DotNetDSI.DataEngine;

//namespace sql.builder.Test
//{
//    internal interface IUIBase
//    {
//        string GetFullName();
//        void SetFullName(string fullname);

//        string GetGroupTitle();
//        void SetGroupTitle(string grouptitle);

//        bool GetShowCheck();
//        void SetShowCheck(bool show_check);

//        bool GetAllowManualUsedSet();
//        void SetAllowManualUsedSet(bool allow);

//        string GetFieldName();
//        string GetCaption();
//        bool GetMandatory();
//        bool GetDefaultVisible();
//        string GetSpecialType();
//        string GetTableName();
//        string GetQueryName();
//        string GetQueryNameDefault();

//        void AddDependant(string name, IUIBase dep);
//        IEnumerable<KeyValuePair<string,IUIBase>> GetAllDependants();

//        void AddMaster(string name, IUIBase master);
//        IEnumerable<KeyValuePair<string,IUIBase>> GetAllMasters();

//        void Initialize(XElement xfield);
//    }
//    internal interface IDataControl
//    {
//        void ColumnChanged(DataRow row);
//        void ColumnEditableChanged(DataRow row);
//        void ColumnValidChanged(DataRow row);
//        void ColumnVisibleChanged(DataRow row);
//        void ColumnSelListChanged(DataRow row);
//        void ColumnFontColorChanged(DataRow row);
//        void ColumnTextChanged(DataRow row);
//        bool IsBool();

//        bool GetUsed();
//        void SetUsed(bool used);
//    }

//    internal interface IUIText : IUIBase
//    {
        
//    }

//    internal interface IUICheck : IUIBase
//    {

//    }

//    internal class UIBase : IUIBase, IDataControl
//    {       
//        public Control Ctrl { get; protected set; }
//        public XElement XField { get; protected set; }
//        public UIFormC Form;

//        Dictionary<string, UIBase> Dependants;
//        Dictionary<string, UIBase> Masters;

//        //public UIFormC.UseType UseType
//        //{
//        //    get
//        //    {
//        //        //if (Form.FormUseType == UIFormC.UseType.DataEditor && !IsColumn)
//        //        //{
//        //        //    return UIFormC.UseType.ParamEditor;
//        //        //}
//        //        //else
//        //        //{
//        //        //    return Form.FormUseType;
//        //        //}
//        //    }
//        //}
//        public bool isInGrid = false;

//        #region События
//        public delegate XElement NeedMasterValuesHandler(IUIBase sender);
//        public event NeedMasterValuesHandler NeedMasterValues;

//        public event EventHandler EditValueChanged;

//        public event Action<string, object> SpecialTypeChanged;
//        #endregion

//        #region IUIBase
//        public string GetFullName()
//        {
//            throw new System.NotImplementedException();
//        }
//        public void SetFullName(string fullname)
//        {
//            throw new System.NotImplementedException();
//        }
//        public string GetGroupTitle()
//        {
//            throw new System.NotImplementedException();
//        }
//        public void SetGroupTitle(string grouptitle)
//        {
//            throw new System.NotImplementedException();
//        }
//        public bool GetShowCheck()
//        {
//            throw new System.NotImplementedException();
//        }
//        public void SetShowCheck(bool show_check)
//        {
//            throw new System.NotImplementedException();
//        }
//        public bool GetAllowManualUsedSet()
//        {
//            throw new System.NotImplementedException();
//        }
//        public void SetAllowManualUsedSet(bool allow)
//        {
//            throw new System.NotImplementedException();
//        }
//        public string GetFieldName()
//        {
//            return XField.Attribute(TextConst.AName.Name).Value;
//        }
//        public string GetCaption()
//        {
//            return XField.AttrOrDef(TextConst.AName.Title, "");
//        }
//        public bool GetMandatory()
//        {
//            return (XField.AttrOrDef(TextConst.AName.Mandatory, TextConst.AVBool.False) == TextConst.AVBool.True);
//        }
//        public bool GetDefaultVisible()
//        {
//            return (XField.AttrOrDef(TextConst.AName.Visible, TextConst.AVBool.True) == TextConst.AVBool.True);
//        }
//        public bool GetNullAsUndefined()
//        {
//            return (XField.AttrOrDef(TextConst.AName.NullAsUndefined, TextConst.AVBool.True) == TextConst.AVBool.True);
//        }
//        public string GetSpecialType()
//        {
//            return XField.AttrOrDef(TextConst.AName.SpecialType, "no");
//        }
//        public string GetTableName()
//        {
//            return XField.AttrOrDef(TextConst.AName.Table, "");
//        }
//        public string GetQueryName()
//        {
//            throw new System.NotImplementedException();
//        }
//        public string GetQueryNameDefault()
//        {
//            throw new System.NotImplementedException();
//        }
//        public void AddDependant(string name, IUIBase dep)
//        {
//            throw new System.NotImplementedException();
//        }
//        public IEnumerable<KeyValuePair<string, IUIBase>> GetAllDependants()
//        {
//            throw new System.NotImplementedException();
//        }
//        public void AddMaster(string name, IUIBase master)
//        {
//            throw new System.NotImplementedException();
//        }
//        public IEnumerable<KeyValuePair<string, IUIBase>> GetAllMasters()
//        {
//            throw new System.NotImplementedException();
//        }
//        public virtual void Initialize(XElement xfield)
//        {
//            XField = xfield;
//        }
//        public bool GetUsed()
//        {
//            throw new System.NotImplementedException();
//        }
//        public virtual void SetUsed(bool used)
//        {
//                //if (UseType != UIFormC.UseType.ParamEditor) return;

//                //if (!(this is UICheck)) ceUsed.Checked = value;
//                //bool value1 = ceUsed.Checked;
//                //if (!ShowCheck)
//                //{
//                //    // (Form.DataSource.ParamsTable.Columns[FieldName] as VDataColumn).ParamUsed = true;

//                //}

//                //else if (TypeMetadata.SourceType == BaseSourceType.Simple)
//                //{
//                //    if (UseType == UIFormC.UseType.ParamEditor)
//                //    {
//                //        //(Form.DataSource.ParamsTable.Columns[FieldName] as VDataColumn).ParamUsed = value;
//                //    }
//                //}
//                //else if (TypeMetadata.SourceType == BaseSourceType.SimpleRange)
//                //{
//                //    if (UseType == UIFormC.UseType.ParamEditor)
//                //    {
//                //        //(Form.DataSource.ParamsTable.Columns[FieldName + "1"] as VDataColumn).ParamUsed = value;
//                //        //(Form.DataSource.ParamsTable.Columns[FieldName + "2"] as VDataColumn).ParamUsed = value;
//                //    }
//                //}
//                //else if (TypeMetadata.SourceType == BaseSourceType.Array)
//                //{
//                //    //if (Form.DataSource.ParamsTable.Columns.Contains(FieldName))
//                //    //{

//                //    //    (Form.DataSource.ParamsTable.Columns[FieldName] as VDataColumn).ParamUsed = value;
//                //    //}
//                //    Form.DataSource.ArrayValueTable(FieldName).ParamUsed = value1;
//                //}
//            }
//        public void ColumnChanged(DataRow row)
//        {
//            throw new System.NotImplementedException();
//        }
//        public void ColumnEditableChanged(DataRow row)
//        {
//            throw new System.NotImplementedException();
//        }
//        public void ColumnValidChanged(DataRow row)
//        {
//            throw new System.NotImplementedException();
//        }
//        public void ColumnVisibleChanged(DataRow row)
//        {
//            throw new System.NotImplementedException();
//        }
//        public void ColumnSelListChanged(DataRow row)
//        {
//            throw new System.NotImplementedException();
//        }
//        public void ColumnFontColorChanged(DataRow row)
//        {
//            throw new System.NotImplementedException();
//        }
//        public void ColumnTextChanged(DataRow row)
//        {
//            throw new System.NotImplementedException();
//        }
//        public virtual bool IsBool()
//        {
//            return false;
//        }
//        #endregion
//    }

//    internal class UIText : UIBase, IUIText
//    {
//        public override void Initialize(XElement xitem)
//        {
//            base.Initialize(xitem);
//            Ctrl = ControlsFactory.Get<UIText>();
//        }
//    }

//    internal class UICheck : UIBase, IUICheck
//    {
//        public override void Initialize(XElement xitem)
//        {
//            base.Initialize(xitem);
//            Ctrl = ControlsFactory.Get<UICheck>();
//        }
//    }

//    internal static class ControlsFactory
//    {
//        internal static Control Get<T>()
//        {
//            Control ctrl = new XtraUserControl();
//            if (typeof(T) == typeof(UIText))
//            {
//                ctrl.Controls.Add(new CheckEdit() {Dock = DockStyle.Left});
//                ctrl.Controls.Add(new TextEdit() {Dock = DockStyle.Fill});
//            }

//            return ctrl;
//        }
//    }
//}
