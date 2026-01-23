//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Xml.Linq;
////using System.Windows.Forms;
//using Devart.Data.Oracle;
//using DevExpress.XtraBars;
//using DevExpress.XtraEditors;
//using DevExpress.XtraEditors.Controls;
//using DevExpress.XtraGrid.Views.Grid;
//using infoenergo.core.Extensions;
//using sql.builder.Controls.Grids;
//using sql.builder.Controls.Grids.ReportViewModes;
//using sql.builder.Controls.Grids.ReportViewModes.Dashboard;
//using sql.builder.DataApi;
//using sql.builder.UI;
//using sql.builder.XmlHelpers;

//namespace sql.builder.Controls
//{
//    internal partial class ucTableViewerContainer : /* IReportGrid,*/ IControlWithTableSource
//    {
//        private VVariableDepandantceController _variableDepandantceController = null;
//        public VVariableDepandantceController GetVariableDepandantceController()
//        {
//            if (_variableDepandantceController == null)
//            {
//                _variableDepandantceController = new VVariableDepandantceController();
//                _variableDepandantceController.Grid = this;//.getControl();

//                // _variableDepandantceController.Form = this;
//            }
//            _variableDepandantceController.attachControlStateEvent();// на случай смены datasource вызывается каждый раз,влечет изменение одного поля - повторение не страшно

//            return _variableDepandantceController;
//        }

//        public string AddVariableStateDependance(object control, string variable, string attr, bool invert = false) //!!! Тоже самое делается для полей по - другому . Продумать. Объединить.  Пока доделываю invert
//        {
//            return GetVariableDepandantceController().AddVariableStateDependance(control, variable, attr, invert);

//        }
//    }

    
//}
