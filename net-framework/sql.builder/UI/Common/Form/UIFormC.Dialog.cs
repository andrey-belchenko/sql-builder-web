using System;

//using System.Windows.Forms;

using sql.builder.WinForms;


namespace sql.builder.UI
{

   
    public partial class UIFormC : IForm
    {


        private void ButtonRefresh_ItemClick(object sender)
        {
        }

        private void DoRefresh()
        {
            RefreshData();
            dataSource.WasRefresh = true;
            RefreshSource(null, false);
        }

        #region old
        //private void ButtonRefresh_ItemClick(object sender)
        //{
        //    RefreshSourceWithCheckModified();
        //}




        public bool RefreshSourceWithCheckModified(object[] new_pars = null, bool isCreation = false, bool firstRefresh = false)
        {
            if (!SaveDataWithCheckModified()) return false;
            if (firstRefresh)
            {
                dataSource.WasRefresh = false;
            }
            RefreshData();
            dataSource.WasRefresh = true;
            RefreshSource(new_pars, isCreation);

            return true;
        }

        public bool SaveDataWithCheckModified()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
