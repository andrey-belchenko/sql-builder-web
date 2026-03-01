using System;
using System.Collections;
using System.Data.Common;
using Devart.Data.Oracle;

namespace sql.builder.Clean
{
    /// <summary>
    /// Wrapper class for OracleDataReader - isolates Devart dependency.
    /// Uses composition since OracleDataReader is created by ExecuteReader().
    /// </summary>
    public class VOracleDataReader : DbDataReader
    {
        private readonly OracleDataReader _inner;

        public VOracleDataReader(OracleDataReader inner)
        {
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
        }

        public OracleDataReader Inner => _inner;

        public override int Depth => _inner.Depth;
        public override bool HasRows => _inner.HasRows;
        public override bool IsClosed => _inner.IsClosed;
        public override int RecordsAffected => _inner.RecordsAffected;
        public override int FieldCount => _inner.FieldCount;

        public override object this[int ordinal] => _inner[ordinal];
        public override object this[string name] => _inner[name];

        public override bool Read() => _inner.Read();
        public override bool NextResult() => _inner.NextResult();
        public override void Close() => _inner.Close();
        public override bool GetBoolean(int ordinal) => _inner.GetBoolean(ordinal);
        public override byte GetByte(int ordinal) => _inner.GetByte(ordinal);
        public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length) => _inner.GetBytes(ordinal, dataOffset, buffer, bufferOffset, length);
        public override char GetChar(int ordinal) => _inner.GetChar(ordinal);
        public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length) => _inner.GetChars(ordinal, dataOffset, buffer, bufferOffset, length);
        public override string GetDataTypeName(int ordinal) => _inner.GetDataTypeName(ordinal);
        public override DateTime GetDateTime(int ordinal) => _inner.GetDateTime(ordinal);
        public override decimal GetDecimal(int ordinal) => _inner.GetDecimal(ordinal);
        public override double GetDouble(int ordinal) => _inner.GetDouble(ordinal);
        public override Type GetFieldType(int ordinal) => _inner.GetFieldType(ordinal);
        public override float GetFloat(int ordinal) => _inner.GetFloat(ordinal);
        public override Guid GetGuid(int ordinal) => _inner.GetGuid(ordinal);
        public override short GetInt16(int ordinal) => _inner.GetInt16(ordinal);
        public override int GetInt32(int ordinal) => _inner.GetInt32(ordinal);
        public override long GetInt64(int ordinal) => _inner.GetInt64(ordinal);
        public override string GetName(int ordinal) => _inner.GetName(ordinal);
        public override int GetOrdinal(string name) => _inner.GetOrdinal(name);
        public override string GetString(int ordinal) => _inner.GetString(ordinal);
        public override object GetValue(int ordinal) => _inner.GetValue(ordinal);
        public override int GetValues(object[] values) => _inner.GetValues(values);
        public override bool IsDBNull(int ordinal) => _inner.IsDBNull(ordinal);
        public override IEnumerator GetEnumerator() => _inner.GetEnumerator();

        public VOracleLob GetOracleLob(int ordinal) => new VOracleLob(_inner.GetOracleLob(ordinal));

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _inner?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
