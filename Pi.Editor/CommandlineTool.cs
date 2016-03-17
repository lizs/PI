using System;
using System.Diagnostics;
using System.IO;

namespace Pi.Editor
{
    public static class CommandlineTool
    {
        public static void Excecut(string exe, string args)
        {
            Process pro = null;
            StreamWriter sw = null;
            StreamReader sr = null;

            pro = new Process
            {
                StartInfo =
                {
                    FileName = exe,
                    Arguments = args,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                }
            };

            pro.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
            pro.ErrorDataReceived += (sender, e) => Console.WriteLine(e.Data);

            pro.Start();
            sw = pro.StandardInput;
            sw.AutoFlush = true;
            pro.BeginOutputReadLine();

            pro.WaitForExit();
        }
    }
}