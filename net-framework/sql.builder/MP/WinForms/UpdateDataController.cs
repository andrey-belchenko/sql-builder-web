using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
//using infoenergo.framework;

using sql.builder.MP.Tools;
namespace sql.builder.MP.Forms
{

    public class TableLoadTask
    {
        public TableLoadTask(string sheetName, string tableName, MPColumn[] columns, int firstDataRowIndex=2, int firstDataColumnIndex=1)
        {
            SheetName = sheetName;
            TableName = tableName;
            DestTableName = tableName;
            Columns = columns;
            FirstDataColumnIndex = firstDataColumnIndex;
            FirstDataRowIndex = firstDataRowIndex;
        }
        public string SheetName;
        public string TableName;
        public MPColumn[] Columns;
        public int FirstDataRowIndex = 1;
        public int FirstDataColumnIndex = 1;
        public string DestTableName;

    }
    public class UpdateDataController
    {
        private CancellationTokenSource _tokenSource;

        public delegate void EventHandlerUIMode(object sender, UIMode args);
        public event EventHandlerUIMode ChangeUIMode = delegate { };

        //public string DestTableName { get; set; }
        //public string SrcTableName { get; set; }
        //public MPColumn[] Columns { get; private set; }

        public TableLoadTask[] Tasks;
        public Announcer Announcer { get; set; }
        public DataTable LogTable { get; private set; }

        //public UpdateDataController(string destTableName, string srcTableName, MPColumn[] columns, Announcer announcer)
        //{
        //    DestTableName = destTableName;
        //    SrcTableName = srcTableName;
        //    Columns = columns;
        //    Announcer = announcer;

        //    LogTable = new DataTable();
        //    LogTable.Columns.AddRange(new[]
        //    {
        //        new DataColumn("status", typeof(int)),
        //        new DataColumn("time", typeof(DateTime)),
        //        new DataColumn("text", typeof(string))
        //    });

        //    announcer.SomethingHappened += announcer_SomethingHappened;
        //}

        public UpdateDataController(TableLoadTask[] tasks, Announcer announcer)
        {

            Create(tasks, announcer);
        }

        public UpdateDataController(TableLoadTask task, Announcer announcer)
        {
            Create(new TableLoadTask[] {task}, announcer);
           
           
        }

        private void Create(TableLoadTask[] tasks, Announcer announcer)
        {
            Tasks = tasks;
            Announcer = announcer;

            LogTable = new DataTable();
            LogTable.Columns.AddRange(new[]
            {
                new DataColumn("status", typeof(int)),
                new DataColumn("time", typeof(DateTime)),
                new DataColumn("text", typeof(string))
            });

            announcer.SomethingHappened += announcer_SomethingHappened;  
        }

        //public async void UpdateTableFromExcelAsync(string filePath, int firstDataRowIndex = 1)
        //{
            
        //    _tokenSource = new CancellationTokenSource();

        //    ChangeUIMode(this, UIMode.InProgress);

        //    try
        //    {
        //        await Task.Factory.StartNew(() => ConvertDataFromExcel(filePath, firstDataRowIndex), _tokenSource.Token);
        //        await Task.Factory.StartNew(() => FillTempTable(), _tokenSource.Token);
        //        await Task.Factory.StartNew(() => UpdateMainTable(), _tokenSource.Token);


        //    }
        //    catch (TaskCanceledException ex)
        //    {
        //        //
        //    }

        //    ChangeUIMode(this, UIMode.Ready);
        //}
        private IExcelEnvironment _excelEnv;
        private ExcelDataReader _reader;
        public void OpenFile(string filePath)
        {
            //_excelEnv = new ExcelMSEnvironment();
            _reader = new ExcelDataReader(_excelEnv, filePath);
        }

        public void CloseFile()
        {
           _excelEnv.Dispose();
        }


        public object GetCellValue(string sheetName, int rowIndex, int columnIndex)
        {
            return _reader.GetCellValue(sheetName, rowIndex, columnIndex);
        }

        public void ExecuteLoad()
        {

            try
            {
                //using (IExcelEnvironment excelEnv = new ExcelMSEnvironment())
                //{

                Announcer.Announce("Чтение файла Excel", MessageStatus.Normal);
                //var reader = new ExcelDataReader(_excelEnv, filePath);

                //IMPDataReader reader = new ExcelDataReader(excelEnv, task.Columns, filePath, firstDataRowIndex, fircsDataColumnIndex);
                foreach (var task in Tasks)
                {
                    Announcer.Announce("Преобразование данных " + task.SheetName, MessageStatus.Normal);
                    _reader.SetOptions(task.SheetName, task.Columns, task.FirstDataRowIndex, task.FirstDataColumnIndex);
                    IMPDataStore saver = new MPDataToFile();
                    saver.SaveData(_reader);
                    Announcer.Announce("Преобразование данных " + task.SheetName + " успешно", MessageStatus.Normal);
                    FillTempTable(task);



                }

                // }
            }
            //catch (Exception ex)
            //{
            //    Announcer.Announce("Ошибка: " + ex.Message, MessageStatus.Error);
            //    _tokenSource.Cancel();
            //}
            finally
            {
                CloseFile();
            }
            
        }
        //public  void UpdateTempTableFromExcel(string filePath, int firstDataRowIndex = 2)// нумерация с 1
        //{

        //    _tokenSource = new CancellationTokenSource();
        //    ChangeUIMode(this, UIMode.InProgress);
        //    ConvertDataFromExcel(filePath, firstDataRowIndex);
        //    FillTempTable();
        //    //UpdateMainTable();
            

        //    ChangeUIMode(this, UIMode.Ready);
        //}

        //public void ConvertDataFromExcel(string filePath, int firstDataRowIndex,int fircsDataColumnIndex=1)
        //{
        //    Announcer.Announce("Преобразование данных из Excel", MessageStatus.Normal);
        //    try
        //    {
        //        using (IExcelEnvironment excelEnv = new ExcelMSEnvironment())
        //        {
        //            IMPDataReader reader = new ExcelDataReader(excelEnv, Columns, filePath, firstDataRowIndex,fircsDataColumnIndex);
        //            IMPDataStore saver = new MPDataToFile();
        //            saver.SaveData(reader);
        //        }
        //        Announcer.Announce("Преобразование успешно", MessageStatus.Normal);
        //    }
        //    catch (Exception ex)
        //    {
        //        Announcer.Announce("Ошибка: " + ex.Message, MessageStatus.Error);
        //        _tokenSource.Cancel();
        //    }
        //}
        public void FillTempTable(TableLoadTask task)
        {
            Announcer.Announce("Загрузка данных во временную таблицу " + task.TableName, MessageStatus.Normal);
            //try
            //{
                IMPDataLoader loader = new MPSqlLoader() { Announcer = Announcer };
                bool success = loader.FillTable(task.TableName, task.Columns);
                if (success) Announcer.Announce("Данные успешно загружены", MessageStatus.Normal);
                else _tokenSource.Cancel();
            //}
            //catch (Exception ex)
            //{
            //    Announcer.Announce("Ошибка: " + ex.Message, MessageStatus.Error);
            //    _tokenSource.Cancel();
            //}
        }

        //private void UpdateMainTable(TableLoadTask task)
        //{
        //    Announcer.Announce("Обновление данных в основной таблице " + task.DestTableName, MessageStatus.Normal);

        //    try
        //    {
        //        IMPTableFiller filler = new MPTableFiller(task.TableName, task.Columns);
        //        bool success = filler.FillTable(task.DestTableName);
        //        if (success)
        //        {
        //            Announcer.Announce("Данные успешно обновлены", MessageStatus.Normal);
        //        }
        //        else
        //        {
        //            Announcer.Announce("Не удалось обновить данные", MessageStatus.Error);
        //            _tokenSource.Cancel();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Announcer.Announce("Ошибка: " + ex.Message, MessageStatus.Error);
        //        _tokenSource.Cancel();
        //    }
        //}

        void announcer_SomethingHappened(object sender, AnnouncerEventArgs e)
        {
            if (e.Status == MessageStatus.Error)
            {
                ChangeUIMode(this, UIMode.Ready);
            }

            ThreadHelper.RunInUIThread(() =>
            {
                var row = LogTable.NewRow();
                row["status"] = (e.Status == MessageStatus.Error) ? 1 : 0;
                row["time"] = e.Time;
                row["text"] = e.Message;
                LogTable.Rows.InsertAt(row, 0);
            });
        }
    }

    public enum UIMode
    {
        Ready,
        InProgress
    }


}