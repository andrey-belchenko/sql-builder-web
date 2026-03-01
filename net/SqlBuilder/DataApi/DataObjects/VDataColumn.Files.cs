using System.Collections.Generic;
using System.Data;
using Devart.Data.Oracle;
using System;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Linq;
using System.IO;
using SqlBuilderLib.DevTools;
using sql.builder.Clean;
namespace sql.builder.DataApi
{
    public partial class VDataColumn
    {

        //Пока нужен только один вариант хранения файлов , если понадобяться дугие сделать подгрузку этих данных их схемы в нестатические поля
        public static string FileTableName = "ur_scan_docs";
        public static string FileIdColumnName = "kod_scan";
        public static string FileNameColumnName = "file_name";
        public static string FileSizeColumnName = "file_size";
        public static string FileDataColumnName = "source";
        public Dictionary<object, VFileGetter> FileGetters = null;

        public void AddFileGetter(VFileGetter getter, DataRow row = null)
        {
            var table = GetTable();
            if (row == null)
            {
                row = table.CurrentRow;
            }
            if (row == null) return;
            if (FileGetters == null)
            {
                FileGetters = new Dictionary<object, VFileGetter>();
            }
            var rid = row[table.PrimaryKey[0]];
            FileGetters[rid] = getter;

            table.ManualUserChangedData();
        }

        public void RemoveFileGetter(DataRow row = null)
        {
            if (row == null)
            {
                row = GetTable().CurrentRow;
            }
            var rid = row[GetTable().PrimaryKey[0]];
            if (FileGetters != null)
            {
                if (FileGetters.ContainsKey(rid))
                {
                    FileGetters.Remove(rid);
                }
            }

        }

        public VFileGetter GetFileGetter(DataRow row = null)
        {
            if (row == null)
            {
                row = GetTable().CurrentRow;
            }
            if (row == null || row.RowState==DataRowState.Deleted) return null;
            if (FileGetters == null)
            {
                return null;
            }
            var rid = row[GetTable().PrimaryKey[0]];
            if (!FileGetters.ContainsKey(rid))
            {
                return null;
            }
            return FileGetters[rid];
        }

        public MemoryStream LoadFile(DataRow row = null)
        {
            if (row == null)
            {
                row = GetTable().CurrentRow;
            }
            if (row == null) return null;

            var fileId = row[this];
            var s = "";
            s += "select ";
            s += FileDataColumnName;
            s += " from ";
            s += FileTableName;
            s += " where ";
            s += FileIdColumnName + "=";
            s += TextConst.Pfx.Param + TextConst.DBParams.FileId;
            var cmd = new VOracleCommand(s, GetTable().GetConnection());
            var par = new VOracleParameter(TextConst.DBParams.FileId, VOracleDbType.Number, fileId, ParameterDirection.Input);
            cmd.Parameters.Add(par);
            DevUtilsProvider.Instance.AnalyzeExecSql(s);
            byte[] _buf = (byte[])cmd.ExecuteScalar();
            if (_buf == null)
            {
                return null;
            }
            var stream = new MemoryStream(_buf);
            return stream;

        }

        public void SaveFile(DataRow row)
        {
            var FileGetter = GetFileGetter(row);
            if (FileGetter == null) return;
            byte[] _buf = FileGetter.GetFile();


            var s = "";
            var fileId = row[this];
            s += "begin ";
            s += "null; ";
            if (fileId != DBNull.Value)
            {
                FileGetter.OldId = fileId;

            }

            if (row.RowState != DataRowState.Deleted)
            {
                s += "insert into ";
                s += FileTableName;
                s += " (";
                s += FileNameColumnName;
                s += "," + FileSizeColumnName;
                s += ") ";
                s += "values ( ";
                s += TextConst.Pfx.Param + TextConst.DBParams.FileName;
                s += "," + TextConst.Pfx.Param + TextConst.DBParams.FileSize;
                s += ") ";
                s += " returning   ";
                s += FileIdColumnName;
                s += " into   ";
                s += TextConst.Pfx.Param + TextConst.DBParams.FileId + ";";


                s += " end; ";
                var cmd = new VOracleCommand(s, GetTable().GetConnection());

                var par = new VOracleParameter(TextConst.DBParams.FileId, VOracleDbType.Number, ParameterDirection.Output);
                par.Value = fileId;
                cmd.Parameters.Add(par);

                par = new VOracleParameter(TextConst.DBParams.FileName, VOracleDbType.VarChar, FileGetter.Name(), ParameterDirection.Input);
                cmd.Parameters.Add(par);

                par = new VOracleParameter(TextConst.DBParams.FileSize, VOracleDbType.VarChar, _buf.Length, ParameterDirection.Input);
                cmd.Parameters.Add(par);

                DevUtilsProvider.Instance.AnalyzeExecSql(s);
                cmd.ExecuteNonQuery();

                fileId = cmd.Parameters[TextConst.DBParams.FileId].Value;

                s = "";
                s += "update ";
                s += FileTableName;
                s += " set ";
                s += FileDataColumnName;
                s += "=";
                s += TextConst.Pfx.Param + TextConst.DBParams.FileData;
                s += " where ";
                s += FileIdColumnName + "=";
                s += TextConst.Pfx.Param + TextConst.DBParams.FileId;


                cmd = new VOracleCommand(s, GetTable().GetConnection());

                par = new VOracleParameter(TextConst.DBParams.FileId, VOracleDbType.Number, fileId, ParameterDirection.Input);
                cmd.Parameters.Add(par);

                par = new VOracleParameter(TextConst.DBParams.FileData, VOracleDbType.Blob);
                par.Value = _buf;
                DevUtilsProvider.Instance.AnalyzeExecSql(s);
                cmd.ExecuteNonQuery();
                if (row.RowState != DataRowState.Deleted)
                {
                    row[this] = fileId;
                }
            }
            RemoveFileGetter(row);
        }


        public void DeleteOldFile(DataRow row)
        {
            var FileGetter = GetFileGetter(row);
            if (FileGetter == null) return;
            if (FileGetter.OldId == null) return;


            var s = "";
            var fileId = FileGetter.OldId;
            s += "begin ";
            s += "null; ";
            if (fileId != DBNull.Value)
            {

                s += "delete ";
                s += FileTableName;
                s += " where ";
                s += FileIdColumnName + "=";
                s += TextConst.Pfx.Param + TextConst.DBParams.FileId + ";";
            }
            s += " end; ";
            var cmd = new VOracleCommand(s, this.GetTable().GetConnection());
            cmd.Parameters.Add(new VOracleParameter(TextConst.DBParams.FileId, VOracleDbType.Number, fileId, ParameterDirection.InputOutput));


            var rid = row[GetTable().PrimaryKey[0]];
            FileGetters.Remove(rid);


        }
    }
}
