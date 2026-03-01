using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using Oracle.ManagedDataAccess.Client;

namespace sql.builder.Clean
{
    /// <summary>
    /// Wrapper class for Oracle parameter collection - isolates Oracle.ManagedDataAccess dependency.
    /// Uses composition since OracleParameterCollection is sealed in ODP.NET.
    /// Implements DbParameterCollection for use with MoveParameters and AddRange.
    /// </summary>
    public class VOracleParameterCollection : DbParameterCollection
    {
        private readonly OracleParameterCollection _inner;

        public VOracleParameterCollection()
        {
            _inner = new OracleCommand().Parameters;
        }

        internal OracleParameterCollection Inner => _inner;

        public override int Count => _inner.Count;

        public override bool IsFixedSize => _inner.IsFixedSize;

        public override bool IsReadOnly => _inner.IsReadOnly;

        public override bool IsSynchronized => _inner.IsSynchronized;

        public override object SyncRoot => _inner.SyncRoot;

        public override int Add(object value)
        {
            var vp = value as VOracleParameter;
            var param = vp != null ? vp.Inner : (OracleParameter)value;
            _inner.Add(param);
            return _inner.Count - 1;
        }

        public override void AddRange(System.Array values)
        {
            if (values == null) return;
            foreach (object v in values)
            {
                var vp = v as VOracleParameter;
                var param = vp != null ? vp.Inner : (OracleParameter)v;
                _inner.Add(param);
            }
        }

        public override void Clear()
        {
            _inner.Clear();
        }

        public override bool Contains(object value)
        {
            var vp = value as VOracleParameter;
            var param = vp != null ? vp.Inner : value;
            return _inner.Contains(param);
        }

        public override bool Contains(string value)
        {
            return _inner.Contains(value);
        }

        public override void CopyTo(System.Array array, int index)
        {
            _inner.CopyTo(array, index);
        }

        public override IEnumerator GetEnumerator()
        {
            return _inner.GetEnumerator();
        }

        public override int IndexOf(object value)
        {
            var vp = value as VOracleParameter;
            var param = vp != null ? vp.Inner : value;
            return _inner.IndexOf(param);
        }

        public override int IndexOf(string parameterName)
        {
            return _inner.IndexOf(parameterName);
        }

        public override void Insert(int index, object value)
        {
            var vp = value as VOracleParameter;
            var param = vp != null ? vp.Inner : (OracleParameter)value;
            _inner.Insert(index, param);
        }

        public override void Remove(object value)
        {
            var vp = value as VOracleParameter;
            var param = vp != null ? vp.Inner : value;
            _inner.Remove(param);
        }

        public override void RemoveAt(int index)
        {
            _inner.RemoveAt(index);
        }

        public override void RemoveAt(string parameterName)
        {
            _inner.RemoveAt(parameterName);
        }

        protected override DbParameter GetParameter(int index)
        {
            return _inner[index];
        }

        protected override DbParameter GetParameter(string parameterName)
        {
            return _inner[parameterName];
        }

        protected override void SetParameter(int index, DbParameter value)
        {
            _inner[index] = (OracleParameter)value;
        }

        protected override void SetParameter(string parameterName, DbParameter value)
        {
            _inner[parameterName] = (OracleParameter)value;
        }
    }
}
