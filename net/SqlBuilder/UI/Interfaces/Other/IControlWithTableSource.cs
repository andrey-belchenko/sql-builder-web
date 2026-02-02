using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using sql.builder.DataApi;
namespace sql.builder.UI
{
    public interface IControlWithTableSource : IControlWithTableSourcePubl
    {
        VDataTable GetDataTable();
        void BeginUpdate();
        void EndUpdate();
        VLayoutControlContainerInfo LayoutContainer {get; set; }
        bool IsVisibleInLayout();
        void UpdateDataSourceSelectedRows();
        void SetFocusedCell(string columnName, DataRow row);
        void SetAllowMerge();
        bool IsCheckBoxSelection();
        bool IsTree();
    }

    public interface IControlWithTableSourcePubl
    {
        void SetVisibleInLayout(bool value);
    }
   
}
