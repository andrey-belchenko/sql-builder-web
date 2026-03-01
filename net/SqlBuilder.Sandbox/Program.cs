using System;

namespace SqlBuilder.Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            var dt = OracleExample.LoadRsEsys();
            Console.WriteLine($"Loaded {dt.Rows.Count} rows, {dt.Columns.Count} columns");
        }
    }
}
