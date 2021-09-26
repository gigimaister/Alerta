using System;
using System.IO;

namespace GProof.Alerta.ExcelHelper.Entities
{
    public class DataFile
    {
        public string Filename { get; set; }
        public Func<Stream> StreamFactory { get; set; }
    }
}
