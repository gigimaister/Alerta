using System;
using System.Text;

namespace GProof.Alerta.Server
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            EncodingProvider encodingProvider = CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(encodingProvider);
            Console.OutputEncoding = Encoding.GetEncoding("Windows-1255");

            Console.WriteLine("Start");
            Console.WriteLine("....ממתין לצבע אדום");

            new Runner().Run();

            Console.WriteLine("End");
            Console.ReadLine();
        }
    }
}
