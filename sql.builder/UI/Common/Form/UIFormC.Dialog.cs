using System;

//using System.Windows.Forms;

using sql.builder.WinForms;


namespace sql.builder.UI
{

   
    public partial class UIFormC : IForm
    {


        private void ButtonRefresh_ItemClick(object sender)
        {
            ////if (!SaveDataWithCheckModified()) return ;
            //Action yesAction = () =>
            //{
            //    if (SaveData(false))
            //    {
            //        DoRefresh();
            //    }
            //};
            //Action noAction = () =>
            //{
            //    DoRefresh();
            //};
            //if (IsModifiedSelfOrSub())
            //{

            //    UIStatic.GetControlsfactory().GetDialogManager(null).ShowQuestion("Имеются несохраненные изменения", "Сохранить изменения?", yesAction, noAction);
              
            //}
            //else
            //{
            //    DoRefresh();
            //}

          
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
            //if (IsModifiedSelfOrSub())
            //{

            //    var result = ShowMessage.Show(ShowMessage.MType.UnsavedChangesQuestion);
            //    if (result == DialogResult.Cancel)
            //    {
            //        return false;
            //    }
            //    else if (result == DialogResult.Yes)
            //    {
            //        if (!SaveData(false)) return false;
            //    }
            //}

            //return true;
        }
        #endregion
    }
}
