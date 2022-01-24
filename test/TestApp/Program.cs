using MyJetWallet.Sdk.Service;
using ProtoBuf.Grpc.Client;
using Service.Fireblocks.Api.Client;
using System;
using System.Threading.Tasks;

namespace TestApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            GrpcClientFactory.AllowUnencryptedHttp2 = true;

            Console.Write("Press enter to start");
            Console.ReadLine();


            var factory = new FireblocksApiClientFactory("http://localhost:5001");
            var client = factory.GetVaultAccountService();
            //var encryption = factory.GetEncryptionService();

            //var publicKey = await File.ReadAllTextAsync(@"C:\Users\O1\Desktop\fireblocks\fireblocks_api_key");
            //var privateKey = await File.ReadAllTextAsync(@"C:\Users\O1\Desktop\fireblocks\fireblocks_secret.key");

            //var x = await encryption.SetApiKeysAsync(new ()
            //{
            //    ApiKey = publicKey,
            //    PrivateKey = privateKey 
            //});


            var stream = client.GetBalancesForAssetAsync(new()
            {
                FireblocksAssetId = "ETH_TEST",
                NamePrefix = "client_",
                Threshold = 0.01m,
                BatchSize = 1
            });

            var count = 0;
            await foreach (var item in stream)
            {
                Console.WriteLine(count);
                Console.WriteLine(item?.ToJson());
                count++;
            }

            Console.WriteLine("End");
            Console.ReadLine();
        }
    }
}
