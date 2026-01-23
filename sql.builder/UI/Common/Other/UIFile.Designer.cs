//using System;
//namespace sql.builder.UI
//{
//    internal partial class UIFile
//    {


//        private IVFileEdit beControl;

//        protected new void BeginHandInitialize()
//        {
//            BeginInitializeBase();
//            beControl = UIStatic.GetControlsfactory().CreateFileEdit();
//        }
//        protected override void initializeComponent()
//        {
//            base.initializeComponent();
//            BeginHandInitialize();

//            SetEditor(this.beControl);
//            this.beControl.FileOpenPressed += beControl_FileOpenPressed;
//            this.beControl.FileSelected += beControl_FileSelected;
//            this.beControl.Cleared += beControl_Cleared;
//            EndHandInitialize();
//        }

        
//        protected new void EndHandInitialize()
//        {
//            beControl.EndInit();
//            EndInitializeBase();
//        }
        //protected new void BeginHandInitialize()
        //{
        //    //base.BeginHandInitialize();
        //    BeginInitializeBase();
        //    this.beControl = new DevExpress.XtraEditors.ButtonEdit();
        //    this.openfile = new System.Windows.Forms.OpenFileDialog();
        //    openfile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
       
            
        //    //   ((System.ComponentModel.ISupportInitialize)(this.ceUsed.Properties)).BeginInit();
        //    //((System.ComponentModel.ISupportInitialize)(this.pSettings)).BeginInit();
        //    //this.pSettings.SuspendLayout();
        //    //((System.ComponentModel.ISupportInitialize)(this.pEditors)).BeginInit();
        //    //this.pEditors.SuspendLayout();
        //    ((System.ComponentModel.ISupportInitialize)(this.beControl.Properties)).BeginInit();
        //    //cRoot.SuspendLayout();
        //}
        //protected override void initializeComponent()
        //{
        //    base.initializeComponent();
        //    BeginHandInitialize();

        //    //base.InitializeComponent();

        //    DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
        //    DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
        //    DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
        //    DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
        //    DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject5 = new DevExpress.Utils.SerializableAppearanceObject();
        //    DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject6 = new DevExpress.Utils.SerializableAppearanceObject();
        //    DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject7 = new DevExpress.Utils.SerializableAppearanceObject();
        //    DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject8 = new DevExpress.Utils.SerializableAppearanceObject();
        //    // 
        //    // pEditors
        //    // 
        //    AddPart(beControl);
        //    // 
        //    // beControl
        //    // 
        //    this.beControl.Dock = System.Windows.Forms.DockStyle.Fill;
        //    this.beControl.Location = new System.Drawing.Point(0, 0);
        //    this.beControl.Name = "beControl";
        //    this.beControl.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
        //    new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Search, "", -1, true, true, true, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, serializableAppearanceObject2, serializableAppearanceObject3, serializableAppearanceObject4, "Открыть файл", null, null, true),
        //    new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "Выбрать", -1, true, true, true, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject5, serializableAppearanceObject6, serializableAppearanceObject7, serializableAppearanceObject8, "Выбрать файл", null, null, true)});
        //    this.beControl.Properties.ReadOnly = true;
        //    this.beControl.Properties.UseReadOnlyAppearance = false;
        //    this.beControl.Size = new System.Drawing.Size(201, 20);
        //    this.beControl.TabIndex = 0;
        //    this.beControl.ButtonPressed += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beControl_ButtonPressed);
        //    //this.beControl.EditValueChanged += new System.EventHandler(this.beControl_EditValueChanged);
        //    this.beControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.beControl_KeyDown);
        //    // 
        //    // openfile
        //    // 
        //    this.openfile.RestoreDirectory = true;
        //    // 
        //    // UIFile
        //    // 
        //    //cRoot.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        //    //cRoot.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        //    //cRoot.Name = "UIFile";

        //    EndHandInitialize();
        //}
        //protected new void EndHandInitialize()
        //{
        //  //  ((System.ComponentModel.ISupportInitialize)(this.ceUsed.Properties)).EndInit();
        //    //((System.ComponentModel.ISupportInitialize)(this.pSettings)).EndInit();
        //    //this.pSettings.ResumeLayout(false);
        //    //((System.ComponentModel.ISupportInitialize)(this.pEditors)).EndInit();
        //    //this.pEditors.ResumeLayout(false);
        //    ((System.ComponentModel.ISupportInitialize)(this.beControl.Properties)).EndInit();
        //    //cRoot.ResumeLayout(false);
        //    EndInitializeBase();
        //    //base.EndHandInitialize();
        //}

        //private DevExpress.XtraEditors.ButtonEdit beControl;
        //private System.Windows.Forms.OpenFileDialog openfile;

        
//    }
//}
