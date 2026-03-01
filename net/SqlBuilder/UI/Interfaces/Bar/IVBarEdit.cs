namespace sql.builder.UI
{
    public interface IVBarEditContainer : IVControl, IVBarItem
    {

        //void AddButton(IVBarItem button, bool beginGroup);
        void SetEdit(object edit);
        //var item = new BarEditItem()
        //{
        //    Caption = xcmd.Attribute(TextConst.AName.Title).Value,
        //    Manager = bm,
        //    Edit = rep,
        //    PaintStyle = BarItemPaintStyle.Caption
        //};
    }
}
