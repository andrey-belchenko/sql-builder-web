using System;
using System.Diagnostics;
using System.IO;

namespace sql.builder.MP.Tools
{
    public class CommandPrompt
    {
        Process _cmd;

        public string OutputText { get; private set; }
        public string ErrorText { get; private set; }

        public void Execute(string command)
        {
            Start();

            _cmd.StandardInput.WriteLine(command);
            _cmd.StandardInput.Flush();

            Close();
        }

        private void Cmd_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (ErrorText == null)
            {
                ErrorText = e.Data;
            }
            else
            {
                ErrorText += Environment.NewLine + e.Data;
            }
        }

        private void Cmd_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (OutputText == null)
            {
                OutputText = e.Data;
            }
            else
            {
                OutputText += Environment.NewLine + e.Data;
            }
        }

        private void Start()
        {
            OutputText = null;
            ErrorText = null;

            _cmd = new Process();
            _cmd.StartInfo.FileName = "cmd.exe";
            _cmd.StartInfo.WorkingDirectory = Path.GetTempPath();
            _cmd.StartInfo.RedirectStandardInput = true;
            _cmd.StartInfo.RedirectStandardOutput = true;
            _cmd.StartInfo.RedirectStandardError = true;
            _cmd.StartInfo.CreateNoWindow = true;
            _cmd.StartInfo.UseShellExecute = false;
            //_cmd.OutputDataReceived += Cmd_OutputDataReceived;
            _cmd.ErrorDataReceived += Cmd_ErrorDataReceived;
            _cmd.EnableRaisingEvents = true;
            _cmd.Start();
            _cmd.BeginErrorReadLine();
            _cmd.BeginOutputReadLine();
            // чтобы корректно обрабатывались пути с кирилицей
          //  _cmd.StandardInput.WriteLine("cd "+Path.GetTempPath());
            _cmd.StandardInput.WriteLine("chcp 1251 >nul");
        }

        private void Close()
        {
            _cmd.StandardInput.Close();
            _cmd.WaitForExit();

            //_cmd.OutputDataReceived -= Cmd_OutputDataReceived;
            _cmd.ErrorDataReceived -= Cmd_ErrorDataReceived;
            _cmd.Close();
        }
    }
}