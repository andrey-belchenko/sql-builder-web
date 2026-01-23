//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Drawing;
//using System.Data;
//using System.Linq;
//using System.Text;
////using System.Windows.Forms;
//using DevExpress.XtraEditors;
//using DevExpress.XtraBars.Docking;
//using System.Xml;
//using System.Xml.Linq;
//using DevExpress.XtraBars.Docking2010.Views;
//using DevExpress.XtraBars.Docking2010.Views.Tabbed;
//using sql.builder.DataApi;
//namespace sql.builder
//{
//    internal partial class ucQueryEditorContextManagers : ucBase
//    {
//        public ucQueryEditorContextManagers()
//        {
//            InitializeComponent();

//            cmChild.Info = "Дочерний";
//            cmNext.Info = "Следующий";
//            tabbedView1.Orientation = Orientation.Vertical;
//           var group= AddTab(tabbedView1,cmNext, cmNext.Info);
//            AddTab(tabbedView1,cmChild, cmChild.Info,group);

//        }


        


        

        


      

//        public void UpdateChildLists(VSXElement element)
//        {
            
//            cmChild.UpdateLists(element);
            
//        }

//        public void UpdateNextLists(VSXElement element)
//        {

//            cmChild.UpdateLists(element);

//        }

//        public event XElementEventHandler ChildItemSelected;


//        public void RaiseChildItemSelected(object sender, XElementEventArgs e)
//        {
//            if (ChildItemSelected != null)
//            {
//                ChildItemSelected(sender, e);
//            }
//        }

//        private void cmChild_ItemSelected(object sender, XElementEventArgs e)
//        {
//            RaiseChildItemSelected(sender, e);
//        }

       
//    }
//}
