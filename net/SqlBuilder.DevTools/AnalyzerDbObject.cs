using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlBuilderLib.DevTools
{
    public class AnalyzerDbObject
    {
        public string ObjectName;
        public DbObjectType? ObjectType;
        public bool Processed;
    }
}
