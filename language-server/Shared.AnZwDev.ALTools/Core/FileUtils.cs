using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using AnZwDev.ALTools.Logging;

namespace AnZwDev.ALTools.Core
{
    public static class FileUtils
    {

        public static string ReadAllText(string path)
        {
            string content = "";
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))            
            using (StreamReader reader = new StreamReader(stream))
                    content = reader.ReadToEnd();
            return content;
        }

        public static string SafeReadAllText(string path)
        {
            //try to read file 4 times
            int retryCounter = 4;
            for (int i = 0; i < retryCounter; i++)
            {
                try
                {
                    return FileUtils.ReadAllText(path);
                }
                catch (FileNotFoundException fileNotFoundException)
                {
                    MessageLog.LogError(fileNotFoundException);
                    return "";
                }
                catch (Exception ex)
                {
                    if (i == (retryCounter - 1))
                        MessageLog.LogError(ex);
                    else
                        System.Threading.Thread.Sleep(200);
                }
            }
            return "";
        }

    }
}
