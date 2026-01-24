using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using sql.builder.XmlHelpers;

namespace sql.builder.TFS
{
    /// <summary>
    /// Работа с TFS через командную строку. Лучше использовать класс TFSServer (кроме checkout)
    /// </summary>
    internal static class TFSHelper
    {
        static int check_interval = 500;
        static int check_times = 10;
        public static bool CheckOutFile(string file_path)
        {
            bool success = false;

            ExecuteTFSCommand(string.Format(@"tf checkout ""{0}""", file_path));

            // ждём пока тфс сделает чек аут и сам снимет флаг ReadOnly с файла
            for (int i = 0; i < check_times; i++)
            {
                if (!(new FileInfo(file_path)).IsReadOnly)
                {
                    success = true;
                    break;
                }

                Thread.Sleep(check_interval);
            }

            return success;
        }

        public static bool CheckInFile(string file_path)
        {
            ExecuteTFSCommand(string.Format(@"tf checkin ""{0}""", file_path));

            // ждём пока тфс сделает чек ин и сам поставит флаг ReadOnly на файл
            bool success = false;
            for (int i = 0; i < check_times; i++)
            {
                if ((new FileInfo(file_path)).IsReadOnly)
                {
                    success = true;
                    break;
                }
                Thread.Sleep(check_interval);
            }

            return success;
        }
        public static void AddFileToSource(string file_path)
        {
            if (File.Exists(file_path))
            {
                ExecuteTFSCommand(string.Format(@"tf add ""{0}""", file_path));
            }
            else
            {
                throw new ArgumentException(string.Format("Попытка добавить отсутствующий файл в TFS - не надо так делать \"{0}\"", file_path));
            }
        }

        static void ExecuteTFSCommand(string command_text)
        {
            // checkout работает только для workspace с типом server (local не работает!)

            // через командную строку VS
            var cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.RedirectStandardError = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.OutputDataReceived += Cmd_OutputDataReceived;
            cmd.ErrorDataReceived += Cmd_ErrorDataReceived;
            cmd.EnableRaisingEvents = true;
            cmd.Start();
            cmd.BeginErrorReadLine();
            cmd.BeginOutputReadLine();

            // должна быть установлена VS 2012
            var pf_folder = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            cmd.StandardInput.WriteLine("chcp 1251"); // чтобы верно обрабатывались пути с кирилицей
            cmd.StandardInput.WriteLine(@"%comspec% /k ""{0}\Microsoft Visual Studio 11.0\VC\vcvarsall.bat""", pf_folder);
            cmd.StandardInput.WriteLine(command_text);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();

            cmd.OutputDataReceived -= Cmd_OutputDataReceived;
            cmd.ErrorDataReceived -= Cmd_ErrorDataReceived;
            cmd.Close();
        }

        private static void Cmd_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Debug.WriteLine(e.Data);
        }

        private static void Cmd_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Debug.WriteLine(e.Data);
        }
        
    }
}
