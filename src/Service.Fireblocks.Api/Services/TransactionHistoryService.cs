using Microsoft.Extensions.Logging;
using MyJetWallet.Fireblocks.Client;
using MyJetWallet.Fireblocks.Domain.Models.AssetMappngs;
using MyJetWallet.Fireblocks.Domain.Models.TransactionHistories;
using MyJetWallet.Sdk.Service;
using MyNoSqlServer.Abstractions;
using Service.Blockchain.Wallets.MyNoSql.AssetsMappings;
using Service.Fireblocks.Api.Grpc;
using Service.Fireblocks.Api.Grpc.Models.TransactionHistory;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Fireblocks.Api.Services
{
    public class TransactionHistoryService : ITransactionHistoryService
    {
        private readonly IClient _client;
        private readonly ILogger<TransactionHistoryService> _logger;
        private readonly IMyNoSqlServerDataReader<AssetMappingNoSql> _myNoSqlServerDataReader;

        public TransactionHistoryService(
            IClient client, 
            ILogger<TransactionHistoryService> logger,
            IMyNoSqlServerDataReader<AssetMappingNoSql> myNoSqlServerDataReader)
        {
            _client = client;
            _logger = logger;
            _myNoSqlServerDataReader = myNoSqlServerDataReader;
        }

        public async Task<GetTransactionHistoryResponse> GetTransactionHistoryAsync(GetTransactionHistoryRequest request)
        {
            try
            {
                var assetsDict = _myNoSqlServerDataReader.Get()
                    .Select(x => x.AssetMapping)
                    .ToDictionary(x => x.FireblocksAssetId);
                var take = request.Take > 0 && request.Take <= 500 ? request.Take : 200;

                var transactions = await _client.TransactionsGetAsync(
                    before: request.BeforeUnixTime.ToString(),
                    orderBy: OrderBy.LastUpdated,
                    status: "COMPLETED",
                    limit: request.Take);

                if (transactions.StatusCode >= 200 && transactions.StatusCode < 400)
                {
                    if (transactions.Result.Any())
                        return new GetTransactionHistoryResponse
                        {
                            History = transactions.Result.Select(x =>
                            {
                                var sourceType = GetTransferPeerType(x.Source.Type);
                                var destinationType = GetTransferPeerType(x.Destination.Type);
                                AssetMapping asset = null;
                                AssetMapping feeAsset = null;

                                assetsDict.TryGetValue(x.AssetId, out asset);
                                assetsDict.TryGetValue(x.FeeCurrency, out feeAsset);

                                return new TransactionHistory
                                {
                                    Id = x.Id,
                                    Amount = x.Amount,
                                    FireblocksAssetId = x.AssetId,
                                    CreatedDateUnix = x.CreatedAt,
                                    UpdatedDateUnix = x.LastUpdated,
                                    Destination = new MyJetWallet.Fireblocks.Domain.Models.TransactionHistories.TransferPeerPath
                                    {
                                        Id = x.Source.Id,
                                        Name = x.Source.Name,
                                        Type = destinationType,
                                    },
                                    DestinationAddress = x.DestinationAddress,
                                    Fee = x.NetworkFee,
                                    FireblocksFeeAssetId = x.FeeCurrency,
                                    Source = new MyJetWallet.Fireblocks.Domain.Models.TransactionHistories.TransferPeerPath
                                    {
                                        Id = x.Source.Id,
                                        Name = x.Source.Name,
                                        Type = sourceType,
                                    },
                                    SourceAddress = x.SourceAddress,
                                    Status = TransactionHistoryStatus.COMPLETED,
                                    TxHash = x.TxHash,
                                    AssetNetwork = asset?.NetworkId,
                                    AssetSymbol = asset?.AssetId,
                                    FeeAssetNetwork = feeAsset?.AssetId,
                                    FeeAssetSymbol = feeAsset?.NetworkId,
                                };
                            }).ToArray(),
                        };

                    return new GetTransactionHistoryResponse
                    {
                        History = new MyJetWallet.Fireblocks.Domain.Models.TransactionHistories.TransactionHistory[0]
                    };
                }
                else
                {
                    return new GetTransactionHistoryResponse
                    {
                        Error = new Grpc.Models.Common.ErrorResponse
                        {
                            ErrorCode = Grpc.Models.Common.ErrorCode.ApiIsNotAvailable,
                            Message = "Retry later, Api is not available!",
                        }
                    };
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error during GetTransactionHistoryAsync", request.ToJson());
                return new GetTransactionHistoryResponse
                {
                    Error = new Grpc.Models.Common.ErrorResponse
                    {
                        ErrorCode = Grpc.Models.Common.ErrorCode.Unknown,
                        Message = e.Message,
                    }
                };
            }
        }

        private static MyJetWallet.Fireblocks.Domain.Models.TransactionHistories.TransferPeerPathType GetTransferPeerType(
            MyJetWallet.Fireblocks.Client.TransferPeerPathType type)
        {
            return type switch
            {
                MyJetWallet.Fireblocks.Client.TransferPeerPathType.VAULT_ACCOUNT => MyJetWallet.Fireblocks.Domain.Models.TransactionHistories.TransferPeerPathType.VAULT_ACCOUNT,
                MyJetWallet.Fireblocks.Client.TransferPeerPathType.EXCHANGE_ACCOUNT => MyJetWallet.Fireblocks.Domain.Models.TransactionHistories.TransferPeerPathType.EXCHANGE_ACCOUNT,
                MyJetWallet.Fireblocks.Client.TransferPeerPathType.INTERNAL_WALLET =>
                MyJetWallet.Fireblocks.Domain.Models.TransactionHistories.TransferPeerPathType.INTERNAL_WALLET,
                MyJetWallet.Fireblocks.Client.TransferPeerPathType.EXTERNAL_WALLET => MyJetWallet.Fireblocks.Domain.Models.TransactionHistories.TransferPeerPathType.EXTERNAL_WALLET,
                MyJetWallet.Fireblocks.Client.TransferPeerPathType.NETWORK_CONNECTION =>
                MyJetWallet.Fireblocks.Domain.Models.TransactionHistories.TransferPeerPathType.NETWORK_CONNECTION,
                MyJetWallet.Fireblocks.Client.TransferPeerPathType.FIAT_ACCOUNT =>
                MyJetWallet.Fireblocks.Domain.Models.TransactionHistories.TransferPeerPathType.FIAT_ACCOUNT,
                MyJetWallet.Fireblocks.Client.TransferPeerPathType.COMPOUND =>
                MyJetWallet.Fireblocks.Domain.Models.TransactionHistories.TransferPeerPathType.COMPOUND,
                MyJetWallet.Fireblocks.Client.TransferPeerPathType.GAS_STATION =>
                MyJetWallet.Fireblocks.Domain.Models.TransactionHistories.TransferPeerPathType.GAS_STATION,
                MyJetWallet.Fireblocks.Client.TransferPeerPathType.ONE_TIME_ADDRESS =>
                MyJetWallet.Fireblocks.Domain.Models.TransactionHistories.TransferPeerPathType.ONE_TIME_ADDRESS,
                MyJetWallet.Fireblocks.Client.TransferPeerPathType.UNKNOWN => MyJetWallet.Fireblocks.Domain.Models.TransactionHistories.TransferPeerPathType.UNKNOWN,

                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
            };
        }
    }
}
