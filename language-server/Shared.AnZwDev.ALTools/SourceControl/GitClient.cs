using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace AnZwDev.ALTools.SourceControl
{
    public static class GitClient
    {

        public static string[] GetModifiedFiles(string path, string fileExt)
        {
            List<string> files = new List<string>();
            bool hasExt = !String.IsNullOrWhiteSpace(fileExt);

            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.WorkingDirectory = path;
            startInfo.FileName = "git";
            startInfo.Arguments = "ls-files -m";
            process.StartInfo = startInfo;
            process.Start();

            StreamReader sr = process.StandardOutput;
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                if (!String.IsNullOrWhiteSpace(line))
                {
                    string fileName = Path.Combine(path, line.Trim());
                    if ((!hasExt) || (fileName.EndsWith(fileExt, StringComparison.CurrentCultureIgnoreCase)))
                        files.Add(fileName);
                }
            }

            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                string errorMessage = process.StandardError.ReadToEnd();
                process.Close();
                process.Dispose();
                throw new Exception(errorMessage);
            }

            process.Close();
            process.Dispose();

            if (Path.DirectorySeparatorChar != '/')
                for (int i = 0; i < files.Count; i++)
                    files[i] = files[i].Replace('/', Path.DirectorySeparatorChar);

            return files.ToArray();
        }

    }
}
