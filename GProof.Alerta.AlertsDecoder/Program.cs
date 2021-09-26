using System;
using System.Text;

namespace GProof.Alerta.AlertsDecoder
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            EncodingProvider encodingProvider = CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(encodingProvider);
            Console.OutputEncoding = Encoding.GetEncoding("Windows-1255");
        }
    }
}
