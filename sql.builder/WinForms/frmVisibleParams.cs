//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
////using System.Windows.Forms;
////using DevExpress.XtraTreeList.Nodes;
//using infoenergo.ui.win.Forms;

//namespace sql.builder.WinForms
//{
//    internal partial class frmVisibleParams : FormBase
//    {

//        public DataTable ParamsTable
//        {
//            set 
//            { 
//                tlParamsVisible.DataSource = value; 
//                tlParamsVisible.ForceInitialize();
//                UpdateCheckColumnWidth();

//                tlParamsVisible.ExpandAll();
//            }
//        }

//        public frmVisibleParams()
//        {
//            InitializeComponent();
//        }

//        private void btnCancel_Click(object sender, EventArgs e)
//        {
//            ((DataTable)tlParamsVisible.DataSource).RejectChanges();
//            this.DialogResult = DialogResult.No;
//        }
//        private void btnAccept_Click(object sender, EventArgs e)
//        {
//            ((DataTable)tlParamsVisible.DataSource).AcceptChanges();
//            this.DialogResult = DialogResult.Yes;
//        }

//        private void rceParamVisible_EditValueChanged(object sender, EventArgs e)
//        {
//            tlParamsVisible.PostEditor();

//            var node = tlParamsVisible.FocusedNode;
//            if (node == null) return;

//            var selected_nodes = tlParamsVisible.Selection.Cast<TreeListNode>().ToArray().Reverse();
//            foreach (TreeListNode selected_node in selected_nodes)
//            {
//                selected_node["check"] = (bool)node["check"];
//            }

//            foreach (TreeListNode selected_node in selected_nodes)
//            {
//                UpdateNodesStatesUp(selected_node);
//                UpdateNodesStatesDown(selected_node);
//            }
//        }

//        private void UpdateNodesStatesUp(TreeListNode child_node)
//        {
//            // обходим дерево снизу вверх
//            if (child_node.ParentNode == null) return;

//            if (child_node.ParentNode.Nodes.Any(node => (bool) node["check"]))
//            {
//                child_node.ParentNode["check"] = true;
//            }
//            else if (child_node.ParentNode.Nodes.All(node => !(bool)node["check"]))
//            {
//                child_node.ParentNode["check"] = false;
//            }

//            UpdateNodesStatesUp(child_node.ParentNode);
//        }
//        private void UpdateNodesStatesDown(TreeListNode parent_node)
//        {
//            // обходим дерево сверху вниз
//            foreach (TreeListNode child_node in parent_node.Nodes)
//            {
//                if (!(bool) parent_node["check"])
//                {
//                    child_node["check"] = false;
//                }
//                UpdateNodesStatesDown(child_node);
//            }  
//        }

//        private void UpdateCheckColumnWidth()
//        {
//            if (tlParamsVisible.Nodes.Count == 0) return;

//            int step = 20;
//            int min_width = 50;

//            int width = 20 + step * (GetAllNodes().Max(n => n.Level) + 1);
//            colParamVisible.Width = width > min_width ? width : min_width;
//        }

//        #region Работа с деревом
//        /*private static TreeListNode GetRootNode(TreeListNode node)
//        {
//            TreeListNode root_node = node;
//            while (root_node.ParentNode != null) {
//                root_node = root_node.ParentNode;
//            }
//            return root_node;
//        }*/
//        private IEnumerable<TreeListNode> GetAllNodes()
//        {
//            return tlParamsVisible.Nodes.SelectMany(Cmn.GetNodeBranch);
//        }
//        #endregion
//    }
//}
