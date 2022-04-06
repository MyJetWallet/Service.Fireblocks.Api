using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using MyJetWallet.Fireblocks.Client;
using MyJetWallet.Fireblocks.Domain.Models.SupportedAssets;
using MyJetWallet.Sdk.Service;
using Service.Fireblocks.Api.Grpc;
using Service.Fireblocks.Api.Grpc.Models.SupportedAssets;

namespace Service.Fireblocks.Api.Services
{
    public class SupportedAssetService : ISupportedAssetService
    {
        private readonly ILogger<SupportedAssetService> _logger;
        private readonly IClient _client;

        public SupportedAssetService(ILogger<SupportedAssetService> logger,
            IClient client)
        {
            _logger = logger;
            this._client = client;
        }

        public async IAsyncEnumerable<GetSupportedAssetResponse> GetSupportedAssetsAsync(GetSupportedAssetsRequest request)
        {
            bool isError = false;
            Response<List<AssetTypeResponse>> response = null;
            var context = request.ToJson();
            try
            {
                response = await _client.Supported_assetsAsync();

                _logger.LogInformation("GetSupportedAssetsAsync {context}", new
                {
                    Request = context,
                    Response = response.ToJson()
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error GetSupportedAssetsAsync {context}", context);
                isError = true;
            }

            if (isError)
            {
                yield return new GetSupportedAssetResponse()
                {
                    Error = new Grpc.Models.Common.ErrorResponse
                    {
                        ErrorCode = Grpc.Models.Common.ErrorCode.Unknown,
                    }
                };
            }

            var batchSize = request.BatchSize > 0 && request.BatchSize < 1000 ? request.BatchSize : 100;

            if (response.Result.Any())
            {
                var supportedAssets = response.Result.Select(x => new SupportedAsset()
                {
                    Id = x.Id,
                    Name = x.Name,
                    ContractAddress = x.ContractAddress,
                    Decimals = x.Decimals,
                    NativeAsset = x.NativeAsset,
                    Type = x.Type switch
                    {
                        AssetTypeResponseType.BASE_ASSET => AssetType.BASE_ASSET,
                        AssetTypeResponseType.ETH_CONTRACT => AssetType.ETH_CONTRACT,
                        AssetTypeResponseType.FIAT => AssetType.FIAT,
                        AssetTypeResponseType.ERC20 => AssetType.ERC20,
                        AssetTypeResponseType.BEP20 => AssetType.BEP20,
                        AssetTypeResponseType.COMPOUND => AssetType.COMPOUND,
                        AssetTypeResponseType.XLM_ASSET => AssetType.XLM_ASSET,
                        AssetTypeResponseType.ALGO_ASSET => AssetType.ALGO_ASSET,
                        AssetTypeResponseType.SOL_ASSET => AssetType.SOL_ASSET,

                        _ => AssetType.UNKNOWN,
                    }

                }).ToArray();

                foreach (var item in supportedAssets.Batch(batchSize))
                {
                    _logger.LogInformation("Streaming batch: {context}", new
                    {
                        Request = context,
                        Batch = item.ToJson()
                    });

                    yield return new GetSupportedAssetResponse
                    {
                        SupportedAssets = item,
                    };
                }
            }
            else
            {
                yield return new GetSupportedAssetResponse()
                {
                    Error = new Grpc.Models.Common.ErrorResponse
                    {
                        ErrorCode = Grpc.Models.Common.ErrorCode.DoesNotExist,
                    }
                };
            }
        }
    }
}
