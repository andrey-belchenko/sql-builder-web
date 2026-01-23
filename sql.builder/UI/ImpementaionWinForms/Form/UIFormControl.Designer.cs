//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
////using System.Windows.Forms;
//using System.Xml.Linq;
//using DevExpress.DashboardCommon.Native;
//using DevExpress.XtraEditors;
//using DevExpress.XtraGrid.Views.Grid;

//using DevExpress.XtraBars;
//using DevExpress.XtraBars.Docking2010.Views;
//using DevExpress.XtraEditors.Controls;
//using infoenergo.core.Extensions;
//using sql.builder.Controls;
//using sql.builder.DataApi;
//using sql.builder.FieldInfo;
//using sql.builder.WinForms;
//using infoenergo.ui.win.Base;
//using sql.builder.Controls.FormFields;
//using sql.builder.Controls.Grids;
//using sql.builder.XmlHelpers;
//using sql.builder.Exceptions;

//namespace sql.builder.UI.WinForms
//{
//    internal partial class UIFormControl
//    {
//        /// <summary> 
//        /// ��������� ���������� ������������.
//        /// </summary>
//        private System.ComponentModel.IContainer components = null;

//        /// <summary> 
//        /// ���������� ��� ������������ �������.
//        /// </summary>
//        /// <param name="disposing">�������, ���� ����������� ������ ������ ���� ������; ����� �����.</param>
//        protected override void Dispose(bool disposing)
//        {
//            if (disposing && (components != null))
//            {
//                components.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//        #region ���, ������������� ��������� ������������� �����������

//        /// <summary> 
//        /// ������������ ����� ��� ��������� ������������ - �� ��������� 
//        /// ���������� ������� ������ ��� ������ ��������� ����.
//        /// </summary>
//        public void BeginInitialize()
//        {
//            this.components = new System.ComponentModel.Container();
//            this.tooltip = new DevExpress.Utils.ToolTipController(this.components);
//            this.barManager = new DevExpress.XtraBars.BarManager(this.components);
            //this.tbMain = new DevExpress.XtraBars.Bar();
//            this.tbMain = new VBar();
            //this.ButtonRefresh = new DevExpress.XtraBars.BarButtonItem();
            //this.ButtonSave = new DevExpress.XtraBars.BarButtonItem();
            //this.ButtonSaveAndClose = new DevExpress.XtraBars.BarButtonItem();
            //this.ButtonDelete = new DevExpress.XtraBars.BarButtonItem();
            //this.ButtonChoice = new DevExpress.XtraBars.BarButtonItem();
            //this.ButtonTest = new DevExpress.XtraBars.BarButtonItem();
            //this.barButtonItem4 = new DevExpress.XtraBars.BarButtonItem();
            //this.btnSaveSettings = new DevExpress.XtraBars.BarButtonItem();
            //this.btnLoadSettings = new DevExpress.XtraBars.BarButtonItem();
            //this.btnExtParams = new DevExpress.XtraBars.BarButtonItem();
//            this.tbSearch = new DevExpress.XtraBars.Bar();
//            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
         
//            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
//            this.rteSearch = new DevExpress.XtraEditors.Repository.RepositoryItemSearchControl();
//            this.popupMenu1 = new DevExpress.XtraBars.PopupMenu(this.components);
//            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rteSearch)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).BeginInit();
//            this.SuspendLayout();
            // 
            // tooltip
            // 
//            this.tooltip.Rounded = true;
//            this.tooltip.ToolTipLocation = DevExpress.Utils.ToolTipLocation.TopRight;
            // 
            // barManager
            // 
//            this.barManager.AllowCustomization = false;
//            this.barManager.AllowQuickCustomization = false;
//            this.barManager.AllowShowToolbarsPopup = false;
//            this.barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
//            this.tbMain,
//            this.tbSearch});
//            this.barManager.DockControls.Add(this.barDockControlTop);
//            this.barManager.DockControls.Add(this.barDockControlBottom);
//            this.barManager.DockControls.Add(this.barDockControlLeft);
//            this.barManager.DockControls.Add(this.barDockControlRight);
//            this.barManager.Form = this;

           

//        }

//        public void EndInitialize()
//        {
            
            //CreateBarButton(FormBarButtonType.Test, "Test");
            //CreateBarButton(FormBarButtonType.ViewTemp, "ViewTemp");


            //this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {

            //this.ButtonSave,
            //this.ButtonRefresh,
            //this.ButtonDelete,
            //this.ButtonTest,
            //this.ButtonSaveAndClose,
            //this.ButtonChoice,
            //this.barButtonItem4,
            //this.btnSaveSettings,
            //this.btnLoadSettings,
            //this.btnExtParams});
//            this.barManager.MaxItemId = 16;
//            this.barManager.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
//            this.repositoryItemTextEdit1,
//            this.rteSearch});
            // 
            // tbMain
            // 
//            this.tbMain.BarAppearance.Normal.BackColor = System.Drawing.Color.Transparent;
//            this.tbMain.BarAppearance.Normal.Options.UseBackColor = true;
//            this.tbMain.BarName = "���������������� 1";
//            this.tbMain.DockCol = 0;
//            this.tbMain.DockRow = 0;
//            this.tbMain.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            //this.tbMain.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            //new DevExpress.XtraBars.LinkPersistInfo(this.ButtonRefresh),
            //new DevExpress.XtraBars.LinkPersistInfo(this.ButtonSave),
            //new DevExpress.XtraBars.LinkPersistInfo(this.ButtonSaveAndClose),
            //new DevExpress.XtraBars.LinkPersistInfo(this.ButtonDelete),
            //new DevExpress.XtraBars.LinkPersistInfo(this.ButtonChoice),
            //new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.None, false, this.ButtonTest, false),
            //new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem4),
            //new DevExpress.XtraBars.LinkPersistInfo(this.btnSaveSettings),
            //new DevExpress.XtraBars.LinkPersistInfo(this.btnLoadSettings),
            //new DevExpress.XtraBars.LinkPersistInfo(this.btnExtParams)});
//            this.tbMain.OptionsBar.AllowQuickCustomization = false;
//            this.tbMain.OptionsBar.DrawBorder = false;
//            this.tbMain.OptionsBar.DrawDragBorder = false;
//            this.tbMain.OptionsBar.UseWholeRow = true;
//            this.tbMain.Text = "���������������� 1";
//            this.tbMain.Visible = false;
            // 
            // ButtonRefresh
            // 
            //this.ButtonRefresh.Caption = "��������";
            //this.ButtonRefresh.Id = 4;
            //this.ButtonRefresh.Name = "ButtonRefresh";
            //this.ButtonRefresh.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            // 
            // ButtonSave
            // 
            //this.ButtonSave.Caption = "���������";
            //this.ButtonSave.Id = 3;
            //this.ButtonSave.Name = "ButtonSave";
            //this.ButtonSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            // 
            // ButtonSaveAndClose
            // 
            //this.ButtonSaveAndClose.Caption = "��������� � �������";
            //this.ButtonSaveAndClose.Id = 7;
            //this.ButtonSaveAndClose.Name = "ButtonSaveAndClose";
            //this.ButtonSaveAndClose.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
//            //// 
//            //// ButtonDelete
//            //// 
            //this.ButtonDelete.Caption = "�������";
            //this.ButtonDelete.Id = 5;
            //this.ButtonDelete.Name = "ButtonDelete";
            //this.ButtonDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
//            //// 
//            //// ButtonChoice
//            //// 
            //this.ButtonChoice.Caption = "�������";
            //this.ButtonChoice.Id = 10;
            //this.ButtonChoice.Name = "ButtonChoice";
            //this.ButtonChoice.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
//            //// 
//            //// ButtonTest
//            //// 
            //this.ButtonTest.Caption = "test";
            //this.ButtonTest.Id = 6;
            //this.ButtonTest.Name = "ButtonTest";
            //this.ButtonTest.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
//            //// 
//            //// barButtonItem4
//            //// 
            //this.barButtonItem4.Caption = "barButtonItem4";
            //this.barButtonItem4.Id = 11;
            //this.barButtonItem4.Name = "barButtonItem4";
            //this.barButtonItem4.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
//            //// 
//            //// btnSaveSettings
//            //// 
            //this.btnSaveSettings.Caption = "��������� ������";
            //this.btnSaveSettings.Id = 12;
            //this.btnSaveSettings.Name = "btnSaveSettings";
            //this.btnSaveSettings.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
//            //// 
//            //// btnLoadSettings
//            //// 
            //this.btnLoadSettings.Caption = "��������� ������";
            //this.btnLoadSettings.Id = 13;
            //this.btnLoadSettings.Name = "btnLoadSettings";
            //this.btnLoadSettings.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
//            //// 
//            //// btnExtParams
//            //// 
            //this.btnExtParams.Caption = "�������������� ���������";
            //this.btnExtParams.Id = 14;
            //this.btnExtParams.Name = "btnExtParams";
            //this.btnExtParams.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            // 
            // tbSearch
            // 
//            this.tbSearch.BarName = "����� ���������";
//            this.tbSearch.DockCol = 0;
//            this.tbSearch.DockRow = 1;
//            this.tbSearch.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
//            this.tbSearch.OptionsBar.AllowQuickCustomization = false;
//            this.tbSearch.OptionsBar.DrawBorder = false;
//            this.tbSearch.OptionsBar.DrawDragBorder = false;
//            this.tbSearch.OptionsBar.UseWholeRow = true;
//            this.tbSearch.Text = "����� ���������";
//            this.tbSearch.Visible = false;
            // 
            // barDockControlTop
            // 
//            this.barDockControlTop.CausesValidation = false;
//            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
//            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
//            this.barDockControlTop.Size = new System.Drawing.Size(1077, 58);
            // 
            // barDockControlBottom
            // 
//            this.barDockControlBottom.CausesValidation = false;
//            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
//            this.barDockControlBottom.Location = new System.Drawing.Point(0, 391);
//            this.barDockControlBottom.Size = new System.Drawing.Size(1077, 0);
            // 
            // barDockControlLeft
            // 
//            this.barDockControlLeft.CausesValidation = false;
//            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
//            this.barDockControlLeft.Location = new System.Drawing.Point(0, 58);
//            this.barDockControlLeft.Size = new System.Drawing.Size(0, 333);
            // 
            // barDockControlRight
            // 
//            this.barDockControlRight.CausesValidation = false;
//            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
//            this.barDockControlRight.Location = new System.Drawing.Point(1077, 58);
//            this.barDockControlRight.Size = new System.Drawing.Size(0, 333);

            // 
            // repositoryItemTextEdit1
            // 
//            this.repositoryItemTextEdit1.AutoHeight = false;
//            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // rteSearch
            // 
//            this.rteSearch.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
//            new DevExpress.XtraEditors.Repository.ClearButton(),
//            new DevExpress.XtraEditors.Repository.SearchButton()});
//            this.rteSearch.Name = "rteSearch";
//            this.rteSearch.Sorted = true;
            // 
            // popupMenu1
            // 
            //this.popupMenu1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem2, DevExpress.XtraBars.BarItemPaintStyle.Caption)});
//            this.popupMenu1.Manager = this.barManager;
//            this.popupMenu1.Name = "popupMenu1";
            // 
            // UIFormControl
            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.Controls.Add(this.barDockControlLeft);
//            this.Controls.Add(this.barDockControlRight);
//            this.Controls.Add(this.barDockControlBottom);
//            this.Controls.Add(this.barDockControlTop);
//            this.DoubleBuffered = true;
//            this.Name = "UIFormControl";
//            this.Size = new System.Drawing.Size(1077, 391);

//            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rteSearch)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).EndInit();
//            this.ResumeLayout(false);
//            this.PerformLayout();

//        }
        
        

//        #endregion

//        private DevExpress.Utils.ToolTipController tooltip;
//        internal DevExpress.XtraBars.BarManager barManager;
//        private DevExpress.XtraBars.BarDockControl barDockControlTop;
//        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
//        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
//        private DevExpress.XtraBars.BarDockControl barDockControlRight;

//        private DevExpress.XtraBars.PopupMenu popupMenu1;

        //internal DevExpress.XtraBars.Bar tbMain;

//        internal VBar tbMain;
        //internal DevExpress.XtraBars.BarButtonItem ButtonRefresh;
        //internal DevExpress.XtraBars.BarButtonItem ButtonSave;
        //public DevExpress.XtraBars.BarButtonItem ButtonDelete;

        //internal DevExpress.XtraBars.BarButtonItem ButtonTest;
        //internal DevExpress.XtraBars.BarButtonItem ButtonSaveAndClose;
//        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
//        private DevExpress.XtraBars.Bar tbSearch;
//        private DevExpress.XtraEditors.Repository.RepositoryItemSearchControl rteSearch;
        //internal DevExpress.XtraBars.BarButtonItem ButtonChoice;
        //private DevExpress.XtraBars.BarButtonItem barButtonItem4;
        //internal DevExpress.XtraBars.BarButtonItem btnSaveSettings;
        //internal DevExpress.XtraBars.BarButtonItem btnLoadSettings;
        //internal DevExpress.XtraBars.BarButtonItem btnExtParams;
//    }
//}
