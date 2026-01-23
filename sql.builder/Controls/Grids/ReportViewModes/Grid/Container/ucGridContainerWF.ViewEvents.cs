//using System.Collections.Generic;
//using System.Linq;
//using DevExpress.Data;
//using DevExpress.XtraEditors;
//using DevExpress.XtraGrid.Views.Grid;
//using sql.builder.DataApi;
//using sql.builder.DataApi.TableDataAccessor.Adapters;
//using sql.builder.UI;

//namespace sql.builder.Controls.Grids.ReportViewModes
//{
//    internal partial class ucGridContainerWF
//    {
//        bool eventsAttached = false;
//        public void AttachViewEvents()
//        {
          
           
            
//            if (!eventsAttached)
//            {

//                GetGrid().ViewSelectionChanged += view_SelectionChanged;
//            }

//            eventsAttached = true;
            
           
//        }

        
//        public void DetachViewEvents()
//        {
//            if (eventsAttached)
//            {
//                GetGrid().ViewSelectionChanged -= view_SelectionChanged;
//            }

//            eventsAttached = false;
           
//        }



//        public event SimpleEventHandler SelectionChanged;
//        private object _selectedView = null;
//        void view_SelectionChanged(object sender)
//        {
//            _selectedView = sender;
//            if (SelectionChanged != null)
//            {
//                SelectionChanged();
//            }
//        }





//        public IVTableDataAdapter GetSelection()
//        {
//            if (_selectedView == null)
//            {
//                return null;
//            }
//            if (_selectedView is GridView)
//            {
//                return new VTDAGridView((GridView)_selectedView);
//            }
//            else
//            {
//                return new VTDATreeList((DevExpress.XtraTreeList.TreeList)_selectedView);
//            }
//        }
//    }
//}
