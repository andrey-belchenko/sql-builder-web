using System;
using System.Collections.Generic;
//using DevExpress.XtraLayout;
//using DevExpress.XtraEditors;
using sql.builder.DataApi;
//using DevExpress.XtraBars;
//using DevExpress.XtraLayout.Utils;
//using DevExpress.XtraEditors.Controls;
using System.Data;
using System.Linq;
using System.Xml.Linq;
//using System.Windows.Forms;

using sql.builder.Test;
using sql.builder.Controls;
namespace sql.builder.UI
{
    public partial class UIFormC : IForm
    {
        public bool IsModified()
        {
            // чтобы учитывались изменения в ячейке в фокусе

            if (!UIStatic.IsWeb())
            {
                //foreach (var grid in Grids.Values)
                //{
                //    var view = grid.GetGridControl();
                //    if (view == null) continue;
                //    view.EndEdit();
                //    //view.PostEditor();
                //    //view.UpdateCurrentRow();
                //}
            }
            return DataSource.Tables.Cast<VDataTable>().Any(t => t.HasUserChanges);
        }


        public bool IsModifiedSelfOrSub() 
        {
            return GetRelativeFormsAndSelf().Any(f => f.IsModified());
                //var stack = new Stack<UIFormC>();
                //stack.Push(this);

                //while (stack.Count > 0)
                //{
                //    var form = stack.Pop();
                //    if (form.IsModified) return true;

                //    if (form.SubForms != null) form.SubForms.ForEach(f => stack.Push(f));
                //}

                //return false;
        }

        public event EventHandler ChangeActionComplete = null;
        public void RaiseChangeActionComplete()
            // когда закончилось какое тодействие по изменению данных в форме
            // можно было бы завязаться на изменение dataSet, но не придумал как поймать конец массового изменения
            // обработаны не все случаи, измененине данных вручную не обработано.
        {
            if (ChangeActionComplete != null)
            {
                ChangeActionComplete(this, null);
            }
        }

        private void OnUserChangedData(object sender, DataColumnChangeEventArgs args)
        {
            //_is_modified = true;
            
            UpdateButtonsState();
            
        }
        internal void Data_OnTopTableRefreshed(object sender, EventArgs args)
        {
            //_is_modified = false;
            UpdateButtonsState();
        }
        internal void Data_OnTopTableCommited(object sender, EventArgs args)
        {
            //_is_modified = false;
            UpdateButtonsState();
        }

        //private SortedList<string, VariableStateDependence> variableStateDependenceList=null;

        //private SortedList<int, string> visiblilityControlVariableList = null; // для отолженной установки видимости 

        //private  class VariableStateDependence
        //{
        //    public List<ControlState> Editable = new List<ControlState>();
        //    public List<ControlState> Visible = new List<ControlState>();
        //    public List<ControlState> SelList = new List<ControlState>();
        //}

        //private  class ControlState
        //{
        //    public object Control;
        //    public object State=null;
        //}



        //public string AddVariableStateDependance(object control, string variable, string attr, bool invert=false) //!!! Тоже самое делается для полей по - другому . Продумать. Объединить.  Пока доделываю invert
        //{
        //    if (variable == TextConst.AVBool.True)// заплатка для стыковки нового и старого варианта
        //    {
        //        return null;
        //    }

        //    if (variable == TextConst.AVBool.False)
        //    {
        //        return null;
        //    }
        //    if (Form.FormUseType == UseType.ParamEditor) return null;
        //    if (invert)
        //    {
        //        variable = "!" + variable;
        //    }
        //    if (variableStateDependenceList == null)
        //    {
        //        variableStateDependenceList = new SortedList<string, VariableStateDependence>();
        //    }

        //    if (!variableStateDependenceList.ContainsKey(variable))
        //    {
        //        variableStateDependenceList.Add(variable, new VariableStateDependence());
        //    }

        //    var controlState = new ControlState();
        //    controlState.Control = control;
            

        //    List<ControlState> controlStateList = null;
          
        //    switch (attr)
        //    {
        //        case TextConst.AName.Editable:
        //            controlStateList = variableStateDependenceList[variable].Editable;
        //            break;
        //        case TextConst.AName.Visible:
        //            controlStateList = variableStateDependenceList[variable].Visible;
        //            break;
        //        case TextConst.EName.ListQuery:
        //            controlStateList = variableStateDependenceList[variable].SelList;
        //            break;
        //    }

        //    controlStateList.Add(controlState);
        //    attachControlStateEvent();


        //    if (attr == TextConst.AName.Visible)
        //    {
        //        if (visiblilityControlVariableList == null)
        //        {
        //            visiblilityControlVariableList = new SortedList<int, string>();
        //            visiblilityControlVariableList[control.GetHashCode()] = variable;
        //        }
        //    }
        //    return variable;
            
        //}

      
        //private bool controlStateEventAttached=false;
        //internal void attachControlStateEvent()
        //{
        //    if (!controlStateEventAttached)
        //    {
        //        controlStateEventAttached = true;
        //        DataSource.VariableChanged += DataSource_OnVariableChanged;  
        //    }

        //}
        //internal void detachControlStateEvent()
        //{
        //    if (controlStateEventAttached)
        //    {
        //        controlStateEventAttached = false;
        //        DataSource.VariableChanged -= DataSource_OnVariableChanged;
        //    }

        //}

        private bool changeCompletedEventAttached = false;
        //private bool _is_modified;
        internal void attachChangeCompletedEvent()
        {
            if (!changeCompletedEventAttached)
            {
                changeCompletedEventAttached = true;
                DataSource.ChangeCompleted += DataSource_OnChangeCompleted;
            }

        }
        internal void detachChangeCompletedEvent()
        {
            if (changeCompletedEventAttached)
            {
                changeCompletedEventAttached = false;
                DataSource.ChangeCompleted -= DataSource_OnChangeCompleted;
            }

        }


        //private void DataSource_OnVariableChanged(object sender, DataColumnChangeEventArgs args)
        //{
        //    string varName = (args.Column as VDataColumn).VariableName;

        //    UpdateControlsStates(varName);


        //    foreach (VDataTable tbl in DataSource.Tables)
        //    {
        //        if (tbl.AutoRefresh)
        //        {
        //            var parsNames = tbl.GetParamsNames();
        //            if (parsNames.Contains(varName + " "))
        //            {
        //                tbl.Refresh();// !!! Выполняется несколько раз - отследить
        //            }
        //        }
        //    }


        //}

        private void DataSource_OnChangeCompleted(object sender, EventArgs args)
        {
            ApplyVisibitlity();
        }


        //internal void UpdateAllControlsStates()
        //{
        //    if (variableStateDependenceList == null) return;

        //    foreach (string varName in variableStateDependenceList.Keys)
        //    {
        //        UpdateControlsStates(varName);
        //    }
        //}

        //private void UpdateControlsStates(string varName)
        //{
        //    if (variableStateDependenceList == null) return;
        //    if (variableStateDependenceList.ContainsKey(varName))
        //    {
        //        foreach (ControlState controlState in variableStateDependenceList[varName].Editable)
        //        {
        //            SetControlEditable(controlState, varName);
        //        }
        //        foreach (ControlState controlState in variableStateDependenceList[varName].Visible)
        //        {
        //            SetControlVisible(controlState, varName);
        //        }
        //        foreach (ControlState controlState in variableStateDependenceList[varName].SelList)
        //        {
        //            RereshControlSelList(controlState, varName);
        //        }
        //    }
        //}

        //private  void SetControlEditable(ControlState controlState, string varName)
        //{
        //    object newVal = dataSource.GetVariableValue(varName);

        //    if (!newVal.Equals(controlState.State))
        //    {
        //         controlState.State=newVal;
        //        bool state=true;
        //        if (Cmn.Nvl(newVal, 0).ToString() == "0")
        //        {
        //            state = false;
        //        }
        //        SetControlEditable(controlState.Control, state);
        //    }
        //}

        //private void SetControlVisible(ControlState controlState, string varName)
        //{
        //    if (this.NoData) return;
        //    object newVal = dataSource.GetVariableValue(varName);

        //    if (!newVal.Equals(controlState.State))
        //    {
        //        controlState.State = newVal;
        //        bool state = true;
        //        if (Cmn.Nvl(newVal, 0).ToString() == "0")
        //        {
        //            state = false;
        //        }
        //        SetControlVisible(controlState.Control, state);
        //    }
        //}


        


        //public  void SetControlEditable(object control, bool val)
        //{
        //    if (control is LayoutControlGroup)
        //    {
        //        (control as LayoutControlGroup).Enabled = val;
        //    }

        //    else if (control is BarButtonItem)
        //    {
        //        (control as BarButtonItem).Enabled = val;

        //    }

        //    else if (control is SimpleButton)
        //    {
        //        (control as SimpleButton).Enabled = val;
        //    }

        //    else if (control is EditorButton)
        //    {
        //        (control as EditorButton).Enabled = val;
        //    }
        //    else
        //    {
        //        throw new System.InvalidOperationException("Не описано поведение для "+ control.GetType().Name);
        //    }
        //}

        //public bool SetControlVisible_GroupNew(object control, bool val) // new to do
        //{
        //    var group = control as VLayoutGroupInfo;
        //    if (group != null)
        //    {
        //        group.SetVisibility(val);
                
        //        return true;
        //    }
        //    return false;
        //}

        //public void SetControlVisible(object control, bool val) // new to do
        //{
        //    if (SetControlVisible_GroupNew(control, val))
        //    {
        //        return;
        //    }

        //    if (control is BarItem)
        //    {
        //        var ctrl = (BarItem)control;
        //        if (val)
        //        {
        //            ctrl.Visibility = BarItemVisibility.Always;
        //        }
        //        else
        //        {
        //            ctrl.Visibility = BarItemVisibility.Never;
        //        }
        //        return;
        //    }

        //    if (control is Control)
        //    {
        //        var itemInfo = Layout.GetItemByControl(control);
        //        itemInfo.SetVisibility(val);
        //    }

        //    if (control is EditorButton)
        //    {
        //        (control as EditorButton).Visible = val;
        //    }
        //}


        


        ////!!! Желательно перенести в DataSet, подумать как   обеспечмить общую логику для контролов (нет соотв колонки в dataset) и полей (есть колонка в dataset)
        //private static void RereshControlSelList(ControlState controlState, string varName)
        //{
        //    if (controlState.Control is UIBase)
        //    {
        //        (controlState.Control as UIBase).RefreshData();
        //    }
        //}

        

    }

    public partial class UIFormC
    {
        private VVariableDepandantceController _variableDepandantceController = null;
        private VVariableDepandantceController getVariableDepandantceController()
        {
            if (_variableDepandantceController == null)
            {
                _variableDepandantceController = new VVariableDepandantceController();
                _variableDepandantceController.Form = this;
            }
            return _variableDepandantceController;
        }

        public string AddVariableStateDependance(object control, string variable, string attr, bool invert = false) //!!! Тоже самое делается для полей по - другому . Продумать. Объединить.  Пока доделываю invert
        {
            return getVariableDepandantceController().AddVariableStateDependance(control, variable, attr, invert);

        }


        
        internal void attachControlStateEvent()
        {
            getVariableDepandantceController().attachControlStateEvent();

        }
        internal void detachControlStateEvent()
        {
            getVariableDepandantceController().detachControlStateEvent();

        }

        internal void UpdateAllControlsStates()
        {

            getVariableDepandantceController().UpdateAllControlsStates();
            
        }

        public void SetControlVisible(object control, bool val) // new to do
        {
            getVariableDepandantceController().SetControlVisible(control,val);
        }

      
    }
    public partial class VVariableDepandantceController
    {

        private SortedList<string, VariableStateDependence> variableStateDependenceList = null;

        private SortedList<int, string> visiblilityControlVariableList = null; // для отолженной установки видимости 

        private class VariableStateDependence
        {
            public List<ControlState> Editable = new List<ControlState>();
            public List<ControlState> Visible = new List<ControlState>();
            //public List<ControlState> Exists = new List<ControlState>();
            public List<ControlState> SelList = new List<ControlState>();
        }

        private class ControlState
        {
            public object Control;
            public object State = null;
        }

        public UIFormC Form = null;
        //internal ucTableViewerContainer Grid = null;
        //public  Form = null;
        public string AddVariableStateDependance(object control, string variable, string attr, bool invert = false) //!!! Тоже самое делается для полей по - другому . Продумать. Объединить.  Пока доделываю invert
        {
            //if (variable.Contains("decision") && invert)
            //{
            //}
            if (variable == TextConst.AVBool.True)// заплатка для стыковки нового и старого варианта
            {
                return null;
            }

            if (variable == TextConst.AVBool.False)
            {
                return null;
            }
            if (Form != null)
            {
                if (Form.FormUseType == UIFormC.UseType.ParamEditor) return null;
            }
            if (invert)
            {
                variable = "!" + variable;
            }
            if (variableStateDependenceList == null)
            {
                variableStateDependenceList = new SortedList<string, VariableStateDependence>();
            }

            if (!variableStateDependenceList.ContainsKey(variable))
            {
                variableStateDependenceList.Add(variable, new VariableStateDependence());
            }

            var controlState = new ControlState();
            controlState.Control = control;


            List<ControlState> controlStateList = null;

            switch (attr)
            {
                case TextConst.AName.Editable:
                    controlStateList = variableStateDependenceList[variable].Editable;
                    SetControlEditable(control, false);
                    break;
                case TextConst.AName.Visible:
                    controlStateList = variableStateDependenceList[variable].Visible;
                    break;
                //case TextConst.AName.Exists:
                //    controlStateList = variableStateDependenceList[variable].Exists;
                //    break;
                case TextConst.EName.ListQuery:
                    controlStateList = variableStateDependenceList[variable].SelList;
                    break;
            }

            controlStateList.Add(controlState);
            attachControlStateEvent();


            if (attr == TextConst.AName.Visible)
            {
                if (visiblilityControlVariableList == null)
                {
                    visiblilityControlVariableList = new SortedList<int, string>();
                    visiblilityControlVariableList[control.GetHashCode()] = variable;
                }
            }
            return variable;

        }

        //private bool controlStateEventAttached = false;

       
        internal VDataSet DataSource
        {

            get
            {
                if (Form != null)
                {
                    return Form.DataSource;
                }
                else
                {
                    //return Grid.DataSource;
                    throw new InvalidOperationException();
                }
            }
            
        }
        internal void attachControlStateEvent()
        {
            //if (!controlStateEventAttached)
            //{
                //controlStateEventAttached = true;
                //DataSource.VariableChanged += DataSource_OnVariableChanged;
            if (DataSource != null)
            {
                DataSource.VariableDepandantceController = this;
            }
            //}

        }
        internal void detachControlStateEvent()
        {
            //if (controlStateEventAttached)
            //{
                //controlStateEventAttached = false;
            if (DataSource != null)
            {
                DataSource.VariableDepandantceController = null;
            }
                //DataSource.VariableChanged -= DataSource_OnVariableChanged;
            //}

        }
        internal void DataSourceVariableChanged(string variableName)
        {
            string varName = variableName;
            this.UpdateControlsStates(varName);
            VDataSet ds = this.DataSource;
            if (ds.WasRefresh) {
                for (int index = 0; index < ds.Tables.Count; index++) {
                    VDataTable tbl = (VDataTable)ds.Tables[index];
                    if (tbl.AutoRefresh) {
                        HashSet<string> parsNames = tbl.GetParamsNames();
                        if (parsNames.Contains(varName)) {
                            //tbl.Refresh();// !!! Выполняется несколько раз - отследить
                            //tbl.RaiseCurrentRowChanged();
                            tbl.EnqueueRefresh();
                        }
                    }
                }
            }
        }
        internal void UpdateAllControlsStates()
        {
            if (variableStateDependenceList == null) return;

            foreach (string varName in variableStateDependenceList.Keys)
            {
                UpdateControlsStates(varName);
            }
        }

        private void UpdateControlsStates(string varName)
        {
            if (variableStateDependenceList == null) return;
            var list = new List<string>();


            if (variableStateDependenceList.ContainsKey(varName))
            {
                list.Add(varName);
            }
            if (variableStateDependenceList.ContainsKey("!" + varName))
            {
                list.Add("!"+ varName);
            }
            foreach (var varName1 in list)
            {
                //if (variableStateDependenceList.ContainsKey(varName))
                //{
                foreach (ControlState controlState in variableStateDependenceList[varName1].Editable)
                {
                    SetControlEditable(controlState, varName1);
                }
                //foreach (ControlState controlState in variableStateDependenceList[varName].Exists)
                //{
                //    SetControlExists(controlState, varName);
                //}
                foreach (ControlState controlState in variableStateDependenceList[varName1].Visible)
                {
                    SetControlVisible(controlState, varName1);
                }
                foreach (ControlState controlState in variableStateDependenceList[varName1].SelList)
                {
                    RereshControlSelList(controlState, varName1);
                }
            }
        }
        private void SetControlEditable(ControlState controlState, string varName)
        {
            object newVal = this.DataSource.GetVariableValue(varName);
            if (!newVal.Equals(controlState.State)) {
                controlState.State = newVal;
                bool state;
                if (Cmn.IsNullOrDBNull(newVal)) {
                    state = false;
                } else {
                    state = newVal.ToString() != TextConst.AVBool.False;
                }
                SetControlEditable(controlState.Control, state);
            }
        }
        //private void SetControlExists(ControlState controlState, string varName)
        //{
        //    object newVal = DataSource.GetVariableValue(varName);
        //    if (!newVal.Equals(controlState.State))
        //    {
        //        controlState.State = newVal;
        //        bool state = true;
        //        if (Cmn.Nvl(newVal, 0).ToString() == "0")
        //        {
        //            state = false;
        //        }
        //        //SetControlVisible(controlState.Control, state);
        //    }
        //}
        private void SetControlVisible(ControlState controlState, string varName)
        {
            object newVal = DataSource.GetVariableValue(varName);
            if (!newVal.Equals(controlState.State)) {
                controlState.State = newVal;
                bool state;
                if (Cmn.IsNullOrDBNull(newVal)) {
                    state = false;
                } else {
                    state = newVal.ToString() != TextConst.AVBool.False;
                }
                SetControlVisible(controlState.Control, state);
            }
        }
        private static void SetControlEditable(object control, bool val)
        {

            var ec = (control as IVEnabledControl);
            ec.SetEnabled(val);
            //if (control is LayoutControlGroup)
            //{
            //    (control as LayoutControlGroup).Enabled = val;
            //}

            //else if (control is BarButtonItem)
            //{
            //    (control as BarButtonItem).Enabled = val;

            //}

            //else if (control is SimpleButton)
            //{
            //    (control as SimpleButton).Enabled = val;
            //}

            //else if (control is EditorButton)
            //{
            //    (control as EditorButton).Enabled = val;
            //}
            //else
            //{
            //    throw new System.InvalidOperationException("Не описано поведение для " + control.GetType().Name);
            //}
        }

        public bool SetControlVisible_GroupNew(object control, bool val) // new to do
        {
            var group = control as VLayoutGroupInfo;
            if (group != null)
            {
                group.SetVisibility(val);

                return true;
            }
            return false;
        }

        public void SetControlVisible(object control, bool val) // new to do
        {
            //if (SetControlVisible_GroupNew(control, val))
            //{
            //    return;
            //}

            //if (control is BarItem)
            //{
            //    var ctrl = (BarItem)control;
            //    if (val)
            //    {
            //        ctrl.Visibility = BarItemVisibility.Always;
            //    }
            //    else
            //    {
            //        //ctrl.Visibility = BarItemVisibility.Always;
            //        ctrl.Visibility = BarItemVisibility.Never;
            //    }
            //    return;
            //}

            //if (control is Control  || control is IVNormalControl)
            //{
            //    var itemInfo = Form.Layout.GetItemByControl(control);
            //    itemInfo.SetVisibility(val);
            //}

            //if (control is EditorButton)
            //{
            //    (control as EditorButton).Visible = val;
            //}
        }


        //!!! Желательно перенести в DataSet, подумать как   обеспечмить общую логику для контролов (нет соотв колонки в dataset) и полей (есть колонка в dataset)
        private static void RereshControlSelList(ControlState controlState, string varName)
        {
            if (controlState.Control is UIBase)
            {
                (controlState.Control as UIBase).RefreshData();
            }
        }





        public event ValueChangeEventHandler ChangeActionComplete = null;

    }



}
