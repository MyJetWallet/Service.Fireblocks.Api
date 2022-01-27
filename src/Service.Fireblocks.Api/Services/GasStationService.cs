using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyJetWallet.Fireblocks.Client;
using MyJetWallet.Fireblocks.Domain.Models.Addresses;
using MyJetWallet.Sdk.Service;
using Service.Fireblocks.Api.Grpc;
using Service.Fireblocks.Api.Grpc.Models.Addresses;
using Service.Fireblocks.Api.Grpc.Models.Balances;
using Service.Fireblocks.Api.Grpc.Models.GasStation;
using Service.Fireblocks.Api.Grpc.Models.VaultAccounts;
using Service.Fireblocks.Api.Grpc.Models.VaultAssets;
using Service.Fireblocks.Api.Settings;

namespace Service.Fireblocks.Api.Services
{
    public class GasStationService : IGasStationService
    {
        private readonly ILogger<VaultAccountService> _logger;
        private readonly IClient _client;
        private readonly IGas_stationClient _gasStationClient;

        public GasStationService(ILogger<VaultAccountService> logger,
            IClient client,
            IGas_stationClient gasStationClient)
        {
            _logger = logger;
            _client = client;
            _gasStationClient = gasStationClient;
        }
        public async Task<GetGasStationResponse> GetGasStationAsync(GetGasStationRequest request)
        {
            try
            {
                var gasStation = await _client.Gas_stationAsync();

                var balances = gasStation.Result.Balance.Select(x =>
                {
                    decimal.TryParse(x.Value, out var balance);

                    return new GasStationBalance()
                    {
                        Balance = balance,
                        FireblocksAssetId = x.Key
                    };
                }).ToArray();
                decimal.TryParse(gasStation.Result.Configuration.GasCap, out var gasCap);
                decimal.TryParse(gasStation.Result.Configuration.GasThreshold, out var gasThreshold);
                decimal.TryParse(gasStation.Result.Configuration.MaxGasPrice, out var maxGasPrice);

                return new GetGasStationResponse()
                {
                    Balances = balances,
                    GasCap = gasCap,
                    GasThreshold = gasThreshold,
                    MaxGasPrice = maxGasPrice
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error GetGasStationAsync {context}", request.ToJson());
                
                return new GetGasStationResponse()
                {
                    Error = new Grpc.Models.Common.ErrorResponse
                    {
                        ErrorCode = Grpc.Models.Common.ErrorCode.Unknown,
                        Message = "Internal error"
                    }
                };
            }
        }

        public async Task<SetGasStationResponse> SetGasStationAsync(SetGasStationRequest request)
        {
            try
            {
                var gasStation = await _gasStationClient.ConfigurationAsync(new GasStationConfiguration
                {
                    GasCap = request.GasCap.ToString(),
                    GasThreshold = request.GasThreshold.ToString(),
                    MaxGasPrice = request.MaxGasPrice.ToString(),
                });

                return new SetGasStationResponse()
                {
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error GetGasStationAsync {context}", request.ToJson());

                return new SetGasStationResponse()
                {
                    Error = new Grpc.Models.Common.ErrorResponse
                    {
                        ErrorCode = Grpc.Models.Common.ErrorCode.Unknown,
                        Message = "Internal error"
                    }
                };
            }
        }
    }
}
