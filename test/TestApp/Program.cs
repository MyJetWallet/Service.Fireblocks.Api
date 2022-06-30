using MyJetWallet.Fireblocks.Domain.Models.Addresses;
using MyJetWallet.Fireblocks.Domain.Models.TransactionHistories;
using MyJetWallet.Sdk.Service;
using ProtoBuf.Grpc.Client;
using Service.Fireblocks.Api.Client;
using Service.Fireblocks.Api.Grpc.Models.Addresses;
using Service.Fireblocks.Api.Grpc.Models.VaultAccounts;
using Service.Fireblocks.Api.Grpc.Models.VaultAssets;
using System;
using System.Collections.Generic;
using System.Linq;
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


            var factory = new FireblocksApiClientFactory("http://fireblocks-api.spot-services.svc.cluster.local:80");//new FireblocksApiClientFactory("http://localhost:5001");
            var vaultAccountService = factory.GetVaultAccountService();
            var assetsClient = factory.GetSupportedAssetServiceService();
            var transactionHistoryClient = factory.GetTransactionHistoryService();

            var createVaultRequest = new CreateVaultAccountRequest()
            {
                AutoFuel = true,
                Name = "aave_test",
                CustomerRefId = "aave_test",
                //We can hide it on ui of fireblocks
                HiddenOnUI = false
            };

            var vault = await vaultAccountService.CreateVaultAccountAsync(createVaultRequest);

            if (vault.Error != null)
            {
                Console.WriteLine(vault.Error.ToJson());
            }

            VaultAddress vaultAddress = null;

           CreateVaultAssetRequest request = new()
            {
                AsssetId = "AAVE",
                VaultAccountId = vault.VaultAccount.Id,
            };

            var vaultAsset = await vaultAccountService.CreateVaultAssetAsync(request);

            vaultAddress = vaultAsset.VaultAddress;

            if (vaultAddress == null)
            {
                GetVaultAddressRequest request1 = new()
                {
                    AssetId = "AAVE",
                    VaultAccountId = vault.VaultAccount.Id,
                };

                var existing = await vaultAccountService.GetVaultAddressAsync(request1);

                vaultAddress = existing?.VaultAddress?.FirstOrDefault();
            }

            if (vaultAddress == null)
            {
            }

            //var currentUnixTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            //var transactionList = new List<TransactionHistory>();
            //var transactionHashes = new HashSet<string>();
            //do
            //{
            //    var transactions = await transactionHistoryClient.GetTransactionHistoryAsync(
            //        new Service.Fireblocks.Api.Grpc.Models.TransactionHistory.GetTransactionHistoryRequest()
            //        {
            //            BeforeUnixTime = currentUnixTime,
            //            Take = 20
            //        });

            //    if (transactions.Error != null)
            //        break;

            //    if (transactions.History == null || !transactions.History.Any())
            //        break;

            //    var count = 0;
            //    foreach (var item in transactions.History)
            //    {
            //        if (transactionHashes.Contains(item.TxHash))
            //            continue;

            //        // WITHDRAWALS, SETTLEMENTS and ALL INTERNAL TRANSFERS EXCEPT GAS STATION TRANSACTIONS
            //        // TRANSACTIONS FROM GAS STATION CURRENTLY HAVE UNKNWON TYPE
            //        if (item.Source != null && (item.Source.Type == TransferPeerPathType.VAULT_ACCOUNT ||
            //                                    item.Source.Type == TransferPeerPathType.GAS_STATION))
            //        {
            //            transactionHashes.Add(item.TxHash);
            //            transactionList.Add(item);
            //            count++;
            //        }
            //        else
            //        {
            //            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(item));
            //        }
            //    }

            //    if (count == 0)
            //        break;

            //    currentUnixTime = transactions.History.Last().CreatedDateUnix;
            //} while (currentUnixTime != long.MaxValue);

            //Console.WriteLine();
            //Console.WriteLine();

            //Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(transactionList));

            //var encryption = factory.GetEncryptionService();

            //var publicKey = await File.ReadAllTextAsync(@"C:\Users\O1\Desktop\fireblocks\fireblocks_api_key");
            //var privateKey = await File.ReadAllTextAsync(@"C:\Users\O1\Desktop\fireblocks\fireblocks_secret.key");

            //var x = await encryption.SetApiKeysAsync(new ()
            //{
            //    ApiKey = publicKey,
            //    PrivateKey = privateKey 
            //});

            //var streamAssets = assetsClient.GetSupportedAssetsAsync(new()
            //{
            //    BatchSize = 30
            //});

            //var count = 0;
            // await foreach (var item in streamAssets)
            // {
            //     Console.WriteLine(count);
            //     Console.WriteLine(item?.ToJson());
            //     count++;
            // }

            //var stream = client.GetBalancesForAssetAsync(new()
            //{
            //    FireblocksAssetId = "ETH_TEST",
            //    NamePrefix = "client_",
            //    Threshold = 0.01m,
            //    BatchSize = 1
            //});

            //count = 0;
            //await foreach (var item in stream)
            //{
            //    Console.WriteLine(count);
            //    Console.WriteLine(item?.ToJson());
            //    count++;
            //}

            Console.WriteLine("End");
            Console.ReadLine();
        }
    }
}
