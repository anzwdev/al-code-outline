using System;
using System.IO;
using System.Text;
using System.Threading;

namespace AnZwDev.System.IO
{
    public static class FileHelper
    {

        public static FileStream? OpenFileStreamWithRetry(string path, int openCount = 5, int failedOpenDelay = 1000)
        {
            while (openCount > 0)
            {
                try
                {
                    FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    return stream;
                }
                catch (Exception)
                {
                    openCount--;
                    if (openCount <= 0)
                        throw;
                    Thread.Sleep(failedOpenDelay);
                }
            }
            return null;
        }

        public static string ReadAllTextWithRetry(string path, int openCount = 5, int failedOpenDelay = 200)
        {
            using (var fileStream = OpenFileStreamWithRetry(path, openCount, failedOpenDelay))
                if (fileStream != null)
                    using (StreamReader reader = new StreamReader(fileStream))
                        return reader.ReadToEnd();
            return String.Empty;
        }

        public static string ReadAllTextWithRetry(string path, Encoding encoding, int openCount = 5, int failedOpenDelay = 200)
        {
            using (var fileStream = OpenFileStreamWithRetry(path, openCount, failedOpenDelay))
                if (fileStream != null)
                    using (StreamReader reader = new StreamReader(fileStream, encoding))
                        return reader.ReadToEnd();
            return String.Empty;
        }

    }
}
