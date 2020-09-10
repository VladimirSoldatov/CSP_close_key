using System;
using System.IO;
using System.Diagnostics;
using System.Text;

namespace cmd_csp
{
    class Program
    {
        static void Main()
        {
            string text = "@echo off\n SetLocal EnableExtensions EnableDelayedExpansion\n copy \"C:\\Program Files\\Crypto Pro\\CSP\\csptest.exe\" >nul\n chcp 1251\n set path=\"C:\\Program Files\\Crypto Pro\\CSP\"\n" +
                "if exist %computername%.txt del /f /q %computername%.txt\n if exist temp.txt del /f /q temp.txt\n set NameK=\"\" \n " +
                "for /f \"usebackq tokens=3,4* delims=\\\" %%a in (`csptest -keyset -enum_cont -fqcn -verifycontext` ) do (\n set NameK=%%a\n;csptest -passwd -showsaved -container \"!NameK!\" >> temp.txt\n )\n" +
                "del /f /q csptest.exe\n set/a $ai=-1\n set/a $bi=2\n for /f \"usebackq delims=\" %%a in (\"temp.txt\") do @(set \"$a=%%a\"\n if \"!$a:~,14!\"==\"AcquireContext\" echo:!$a! >> %computername%.txt\n" +
                "if \"!$a:~,8!\"==\"An error\" echo:Увы, ключевой носитель отсутствует или пароль не был сохранен. >> %computername%.txt & echo: >> %computername%.txt\n if \"!$a:~,5!\"==\"Saved\" set/a $ai=1\n" +
                "if !$ai! geq 0 set/a $ai-=1 & set/a $bi-=1 & echo:!$a! >> %computername%.txt\n if !$bi!==0 echo: >> %computername%.txt & set/a $bi=2\n)\n del /f /q temp.txt\n EndLocal\n echo on";
            using (FileStream fstream = new FileStream(@"temp.cmd", FileMode.OpenOrCreate))
            {
                // преобразуем строку в байты
                byte[] input = Encoding.Default.GetBytes(text);
                // запись массива байтов в файл
                fstream.Write(input, 0, input.Length);

            }
                Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = " /c " + text;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            StreamReader reader = process.StandardOutput;
            string output = reader.ReadToEnd();

            Console.WriteLine(output);

            process.WaitForExit();
            process.Close();
            System.IO.File.Delete(@"temp.cmd");

        }
    }
}