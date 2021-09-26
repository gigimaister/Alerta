using System.IO;

namespace GProof.Alerta.ExcelHelper.Entities
{
    public class InputFile
    {
        public string FileName { get; set; }

        public string FilePath { get; set; }

        public byte[] Content { get; set; }

        public Stream StreamContent()
        {
            return (Content == null ||  Content.Length == 0)? null : new MemoryStream(Content);
        }
    }
}
