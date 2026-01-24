using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Linq;
using System.IO;
namespace sql.builder.DataApi
{
    internal partial class VDataColumn
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
            var cmd = new OracleCommand(s, (OracleConnection)GetTable().GetConnection());
            var par = new OracleParameter(TextConst.DBParams.FileId, fileId);
            par.OracleDbType = OracleDbType.Decimal;
            cmd.Parameters.Add(par);
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
                var cmd = new OracleCommand(s, (OracleConnection)GetTable().GetConnection());

                var par = new OracleParameter(TextConst.DBParams.FileId, fileId);
                par.OracleDbType = OracleDbType.Decimal;
                par.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(par);

                par = new OracleParameter(TextConst.DBParams.FileName, FileGetter.Name());
                par.OracleDbType = OracleDbType.Varchar2;
                cmd.Parameters.Add(par);

                par = new OracleParameter(TextConst.DBParams.FileSize, _buf.Length);
                par.OracleDbType = OracleDbType.Varchar2;
                cmd.Parameters.Add(par);

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


                cmd = new OracleCommand(s, (OracleConnection)GetTable().GetConnection());

                par = new OracleParameter(TextConst.DBParams.FileId, fileId);
                par.OracleDbType = OracleDbType.Decimal;
                cmd.Parameters.Add(par);

                par = cmd.Parameters.Add(TextConst.DBParams.FileData, OracleDbType.Blob);
                par.Value = _buf;
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
            var cmd = new OracleCommand(s, this.GetTable().GetConnection());
            cmd.Parameters.Add(TextConst.DBParams.FileId, OracleDbType.Decimal, fileId, ParameterDirection.InputOutput);


            var rid = row[GetTable().PrimaryKey[0]];
            FileGetters.Remove(rid);


        }
    }
}
