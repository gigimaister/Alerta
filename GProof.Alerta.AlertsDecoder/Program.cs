using System;
using System.Text;
using System.Threading.Tasks;

namespace GProof.Alerta.AlertsDecoder
{
    public static class Program
    {
        static void Main(string[] args)
        {
            MainAsync().Wait();
        }

        public async static Task MainAsync()
        {
            EncodingProvider encodingProvider = CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(encodingProvider);
            Console.OutputEncoding = Constants.HebrewEncoding;
            DataRetriever dataRetriever = new DataRetriever();

            await new Executor().Execute();

            Console.ReadLine();
        }

    }
}
