using System.Text;

namespace AnZwDev.System.IO
{
    public interface IFile
    {

        Encoding Encoding { get; }
        string FullPath { get; }
        string ReadAllText();
        void WriteAllText(string content);

    }
}
