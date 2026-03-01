using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Contract = System.Diagnostics.Contracts.Contract;

namespace sql.builder.Print.Xlsx
{
    public static class ExcelUtils
    {
        public static string GetPrintDirectory()
        {
            int postfix = 1;
            string print_directory;
            do
            {

                print_directory = Path.Combine(sql.builder.Clean.Settings.GetInstance().TempPath, "sql.builder.printing", "report" + postfix++);
            } while (Directory.Exists(print_directory));

            //Directory.CreateDirectory(print_directory);
            return print_directory;
        }

        static Dictionary<int, string> _cash1 = new Dictionary<int, string>();
        static Dictionary<string, int> _cash2 = new Dictionary<string, int>();
        public static string GetColumnName(int index)
        {
            string name;
            if (_cash1.TryGetValue(index, out name))
            {
                return name;
            }

            const string letters = "ZABCDEFGHIJKLMNOPQRSTUVWXY";

            int NextPos = (index / 26);
            int LastPos = (index % 26);
            if (LastPos == 0) NextPos--;

            if (index > 26)
                name = GetColumnName(NextPos) + letters[LastPos];
            else
                name = letters[LastPos] + "";

            _cash1.Add(index, name);
            return name;
        }
        public static int GetColumnNumber(string name)
        {
            int number = 0;
            if (_cash2.TryGetValue(name, out number))
            {
                return number;
            }

            number = 0;
            int pow = 1;
            for (int i = name.Length - 1; i >= 0; i--)
            {
                number += (name[i] - 'A' + 1) * pow;
                pow *= 26;
            }

            _cash2.Add(name, number);

            return number;
        }
        public static void ParseCellName(string cell_name, out int row_id, out int column_id, out string column_name)
        {
            Contract.Assert(!string.IsNullOrEmpty(cell_name));
            int index = 0;
            column_id = 0;
            while (true)
            {
                if (index == cell_name.Length)
                {
                    throw new ArgumentOutOfRangeException("cell_name");
                }
                char ch = cell_name[index];
                if (ch >= '0' && ch <= '9')
                {
                    break;
                }
                else if (ch < 'A' || ch > 'Z')
                {
                    throw new ArgumentOutOfRangeException("cell_name");
                }
                column_id = column_id * 26 + (ch - 'A' + 1);
                index++;
            }
            if (index == 0)
            {
                throw new ArgumentOutOfRangeException("cell_name");
            }
            row_id = int.Parse(cell_name.Substring(index));
            column_name = cell_name.Substring(0, index);
            Contract.Assume(ExcelUtils.GetColumnNumber(column_name) == column_id);
        }
        // TODO: ����� �������������� �������-������ �� ������ ����?
        public static string CorrectFormulaReferences(string formula, int colDelta = 0, int rowDelta = 0)
        {
            if (colDelta == 0 && rowDelta == 0) return formula;

            MatchCollection matches = Regex.Matches(formula, "([A-Z]+)([0-9]+)");

            int ind = 0;
            StringBuilder sb = new StringBuilder();
            foreach (Match match in matches)
            {
                sb.Append(formula.Substring(ind, match.Index - ind));
                string cellName = match.Groups[1].Value;
                // ������������ ��� ������ � �������, ���� ������� ��������� � �������� ������
                if (colDelta != 0)
                {
                    cellName = GetColumnName(GetColumnNumber(cellName) + colDelta);
                }

                int rowId = int.Parse(match.Groups[2].Value);
                int rowIdNew = rowId + rowDelta;
                sb.Append(cellName + rowIdNew);

                ind = match.Index + match.Length;
            }

            if (formula.Length > ind)
            {
                sb.Append(formula.Substring(ind, formula.Length - ind));
            }

            return sb.ToString();
        }

        public static IEnumerable<string> GetVariablesNames(string text)
        {
            foreach (Match match in Regex.Matches(text, @"\[:(([a-zA-Z0-9_]+\.)*)([a-zA-Z0-9_]+)\]"))
            {
                string tableName = "a";
                if (match.Groups[1].Value != "")
                {
                    tableName = match.Groups[1].Value.Split('.').First().ToLower();
                }

                yield return tableName + "." + match.Groups[3].Value.ToLower();
            }
        }
        public static IEnumerable<string> GetVariablesNames(DataTable table)
        {
            foreach (DataColumn col in table.Columns)
            {
                string tableName = "a";
                if (table.TableName != "" && table.TableName != "Table1")
                {
                    tableName = table.TableName.ToLower();
                }

                yield return tableName + "." + col.ColumnName.ToLower();
            }
        }
    }
}