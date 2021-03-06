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
using Service.Fireblocks.Api.Grpc.Models.VaultAccounts;
using Service.Fireblocks.Api.Grpc.Models.VaultAssets;
using Service.Fireblocks.Api.Settings;

namespace Service.Fireblocks.Api.Services
{
    public class VaultAccountService : IVaultAccountService
    {
        private readonly ILogger<VaultAccountService> _logger;
        private readonly IVaultClient _vaultClient;
        private readonly IClient _client;
        private readonly IAccountsClient _accountsClient;
        private readonly ITransactionsClient _transactionsClient;

        public VaultAccountService(ILogger<VaultAccountService> logger,
            IVaultClient vaultClient,
            IClient client,
            IAccountsClient accountsClient,
            ITransactionsClient transactionsClient)
        {
            _logger = logger;
            this._vaultClient = vaultClient;
            this._client = client;
            this._accountsClient = accountsClient;
            this._transactionsClient = transactionsClient;
        }

        public async Task<CreateVaultAccountResponse> CreateVaultAccountAsync(CreateVaultAccountRequest request)
        {
            try
            {
                var idempotencyKey = $"account_{request.Name}";
                idempotencyKey = idempotencyKey.Substring(0, Math.Min(40, idempotencyKey.Length));
                var response = await _vaultClient.AccountsPostAsync(idempotencyKey, new Body
                {
                    AutoFuel = request.AutoFuel,
                    CustomerRefId = request.CustomerRefId,
                    Name = GetWithClientPrefix(request.Name),
                    HiddenOnUI = request.HiddenOnUI,
                });

                return new CreateVaultAccountResponse
                {
                    VaultAccount = new MyJetWallet.Fireblocks.Domain.Models.VaultAccounts.VaultAccount
                    {
                        AutoFuel = response.Result.AutoFuel,
                        CustomerRefId = response.Result.CustomerRefId,
                        HiddenOnUI = response.Result.HiddenOnUI,
                        Id = response.Result.Id,
                        Name = response.Result.Name,
                        VaultAssets = response.Result.Assets.Select(x => new MyJetWallet.Fireblocks.Domain.Models.VaultAssets.VaultAsset()
                        {
                            Id = x.Id,
                            Available = decimal.Parse(x.Available),
                            BlockHash = x.BlockHash,
                            BlockHeight = x.BlockHeight,
                            Frozen = decimal.Parse(x.Frozen),
                            LockedAmount = decimal.Parse(x.LockedAmount),
                            Pending = decimal.Parse(x.Pending),
                            Staked = decimal.Parse(x.Staked),
                            Total = decimal.Parse(x.Total)
                        }).ToArray(),
                    }
                };

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating VaultAccount {context}", request.ToJson());

                return new CreateVaultAccountResponse
                {
                    Error = new Grpc.Models.Common.ErrorResponse
                    {
                        ErrorCode = Grpc.Models.Common.ErrorCode.Unknown,
                        Message = e.Message
                    }
                };
            }
        }

        public async Task<CreateVaultAddressResponse> CreateVaultAddressAsync(CreateVaultAddressRequest request)
        {
            try
            {
                var idempotencyKey = $"addr_{request.Name}";
                idempotencyKey = idempotencyKey.Substring(0, Math.Min(40, idempotencyKey.Length));
                var response = await _accountsClient.AddressesPostAsync(idempotencyKey,
                    request.VaultAccountId,
                    request.AssetId,
                    new Body6
                    {
                        CustomerRefId = request.CustomerRefId,
                        Description = GetWithClientPrefix(request.Name),
                    });

                return new CreateVaultAddressResponse
                {
                    VaultAddress = new MyJetWallet.Fireblocks.Domain.Models.Addresses.VaultAddress
                    {
                        Address = response.Result.Address,
                        Bip44AddressIndex = response.Result.Bip44AddressIndex,
                        EnterpriseAddress = response.Result.EnterpriseAddress,
                        LegacyAddress = response.Result.LegacyAddress,
                        Tag = response.Result.Tag,
                    }
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating VaultAddress {context}", request.ToJson());

                return new CreateVaultAddressResponse
                {
                    Error = new Grpc.Models.Common.ErrorResponse
                    {
                        ErrorCode = Grpc.Models.Common.ErrorCode.Unknown,
                        Message = e.Message
                    }
                };
            }
        }

        public async Task<Grpc.Models.VaultAssets.CreateVaultAssetResponse> CreateVaultAssetAsync(CreateVaultAssetRequest request)
        {
            try
            {
                var idempotencyKey = $"wallet_{request.VaultAccountId}_{request.AsssetId}";
                idempotencyKey = idempotencyKey.Substring(0, Math.Min(40, idempotencyKey.Length));
                var response = await _vaultClient.AccountsPostAsync(idempotencyKey, request.VaultAccountId, request.AsssetId,
                    new Body5
                    {
                        EosAccountName = request.EosAccountName
                    });

                return new Grpc.Models.VaultAssets.CreateVaultAssetResponse
                {
                    VaultAsset = new MyJetWallet.Fireblocks.Domain.Models.VaultAssets.VaultAsset()
                    {
                        Id = response.Result.Id,
                    },
                    VaultAddress = new VaultAddress
                    {
                        Address = response.Result.Address,
                        EnterpriseAddress = response.Result.EnterpriseAddress,
                        LegacyAddress = response.Result.LegacyAddress,
                        Tag = response.Result.Tag,
                    }
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating VaultAddress {context}", request.ToJson());

                return new Grpc.Models.VaultAssets.CreateVaultAssetResponse
                {
                    Error = new Grpc.Models.Common.ErrorResponse
                    {
                        ErrorCode = Grpc.Models.Common.ErrorCode.Unknown,
                        Message = e.Message
                    }
                };
            }
        }

        public async Task<GetVaultAccountResponse> GetVaultAccountAsync(GetVaultAccountRequest request)
        {
            try
            {
                if (!string.IsNullOrEmpty(request.VaultAccountId))
                {
                    var response = await _vaultClient.AccountsGetAsync(request.VaultAccountId, default);

                    return new GetVaultAccountResponse
                    {
                        VaultAccount = new MyJetWallet.Fireblocks.Domain.Models.VaultAccounts.VaultAccount[] { new () {
                            AutoFuel = response.Result.AutoFuel,
                            CustomerRefId = response.Result.CustomerRefId,
                            HiddenOnUI = response.Result.HiddenOnUI,
                            Id = response.Result.Id,
                            Name = response.Result.Name,
                            VaultAssets = response.Result.Assets.Select(x => {

                                decimal.TryParse(x.Available, out var available);
                                decimal.TryParse(x.Frozen, out var frozen);
                                decimal.TryParse(x.LockedAmount, out var lockedAmount);
                                decimal.TryParse(x.Pending, out var pending);
                                decimal.TryParse(x.Staked, out var staked);
                                decimal.TryParse(x.Total, out var total);

                                return new MyJetWallet.Fireblocks.Domain.Models.VaultAssets.VaultAsset()
                                {
                                    Id = x.Id,
                                    BlockHash = x.BlockHash,
                                    BlockHeight = x.BlockHeight,
                                    Available = available,
                                    Frozen = frozen,
                                    LockedAmount = lockedAmount,
                                    Pending = pending,
                                    Staked = staked,
                                    Total = total
                                };}).ToArray()
                        } },
                    };
                }

                {
                    var response = await _vaultClient.AccountsGetAsync(namePrefix: request.Name);

                    if (response.Result.Any())
                    {
                        return new GetVaultAccountResponse
                        {
                            VaultAccount = response.Result.Select(x => new MyJetWallet.Fireblocks.Domain.Models.VaultAccounts.VaultAccount()
                            {
                                AutoFuel = x.AutoFuel,
                                CustomerRefId = x.CustomerRefId,
                                HiddenOnUI = x.HiddenOnUI,
                                Id = x.Id,
                                Name = x.Name,
                                VaultAssets = x.Assets.Select(x =>
                                {
                                    decimal.TryParse(x.Available, out var available);
                                    decimal.TryParse(x.Frozen, out var frozen);
                                    decimal.TryParse(x.LockedAmount, out var lockedAmount);
                                    decimal.TryParse(x.Pending, out var pending);
                                    decimal.TryParse(x.Staked, out var staked);
                                    decimal.TryParse(x.Total, out var total);

                                    return new MyJetWallet.Fireblocks.Domain.Models.VaultAssets.VaultAsset()
                                    {
                                        Id = x.Id,
                                        Available = available,
                                        BlockHash = x.BlockHash,
                                        BlockHeight = x.BlockHeight,
                                        Frozen = frozen,
                                        LockedAmount = lockedAmount,
                                        Pending = pending,
                                        Staked = staked,
                                        Total = total
                                    };
                                }).ToArray()
                            }).ToArray(),
                        };
                    }

                    return new GetVaultAccountResponse()
                    {
                        Error = new Grpc.Models.Common.ErrorResponse
                        {
                            ErrorCode = Grpc.Models.Common.ErrorCode.DoesNotExist,
                        }
                    };
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating VaultAddress {context}", request.ToJson());

                return new GetVaultAccountResponse
                {
                    Error = new Grpc.Models.Common.ErrorResponse
                    {
                        ErrorCode = Grpc.Models.Common.ErrorCode.Unknown,
                        Message = e.Message
                    }
                };
            }
        }

        public async Task<GetVaultAddressResponse> GetVaultAddressAsync(GetVaultAddressRequest request)
        {
            try
            {
                var response = await _accountsClient.AddressesGetAsync(request.VaultAccountId, request.AssetId, default);

                return new GetVaultAddressResponse
                {
                    VaultAddress = response.Result.Select(x => new VaultAddress
                    {
                        Address = x.Address,
                        Bip44AddressIndex = x.Bip44AddressIndex,
                        EnterpriseAddress = x.EnterpriseAddress,
                        LegacyAddress = x.LegacyAddress,
                        Tag = x.Tag,
                    }).ToArray(),
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating VaultAddress {context}", request.ToJson());

                return new GetVaultAddressResponse
                {
                    Error = new Grpc.Models.Common.ErrorResponse
                    {
                        ErrorCode = Grpc.Models.Common.ErrorCode.Unknown,
                        Message = e.Message
                    }
                };
            }
        }

        public async Task<GetVaultAssetResponse> GetVaultAssetAsync(GetVaultAssetRequest request)
        {
            try
            {
                var response = await _vaultClient.AccountsGetAsync(request.VaultAccountId, request.AsssetId, default);

                return new GetVaultAssetResponse
                {
                    VaultAsset = new MyJetWallet.Fireblocks.Domain.Models.VaultAssets.VaultAsset()
                    {
                        Id = response.Result.Id,
                        Available = decimal.Parse(response.Result.Available),
                        BlockHash = response.Result.BlockHash,
                        BlockHeight = response.Result.BlockHeight,
                        Frozen = decimal.Parse(response.Result.Frozen),
                        LockedAmount = decimal.Parse(response.Result.LockedAmount),
                        Pending = decimal.Parse(response.Result.Pending),
                        Staked = decimal.Parse(response.Result.Staked),
                        Total = decimal.Parse(response.Result.Total)
                    },
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating VaultAddress @{context}", request);

                return new GetVaultAssetResponse
                {
                    Error = new Grpc.Models.Common.ErrorResponse
                    {
                        ErrorCode = Grpc.Models.Common.ErrorCode.Unknown,
                        Message = e.Message
                    }
                };
            }
        }

        public static string GetWithClientPrefix(string clientId)
        {
            return $"client_{clientId}";
        }

        public async Task<Grpc.Models.Addresses.ValidateAddressResponse> ValidateAddressAsync(ValidateAddressRequest request)
        {
            try
            {
                var response = await _transactionsClient.Validate_addressAsync(request.AssetId, request.Address);

                return new Grpc.Models.Addresses.ValidateAddressResponse
                {
                    Address = request.Address,
                    IsValid = response.Result.IsValid,
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error validating address @{context}", request.ToJson());

                return new Grpc.Models.Addresses.ValidateAddressResponse
                {
                    Error = new Grpc.Models.Common.ErrorResponse
                    {
                        ErrorCode = Grpc.Models.Common.ErrorCode.Unknown,
                        Message = e.Message
                    }
                };
            }
        }

        public async IAsyncEnumerable<GetVaultAccountBalancesResponse> GetBalancesForAssetAsync(GetVaultAccountBalancesRequest request)
        {
            bool isError = false;
            Response<List<VaultAccount>> response = null;
            var context = request.ToJson();
            try
            {
                response = await _vaultClient.AccountsGetAsync(
                    namePrefix: request.NamePrefix,
                    minAmountThreshold: request.Threshold.ToString(),
                    assetId: request.FireblocksAssetId);
                _logger.LogInformation("GetBalancesForAssetAsync {context}", new
                {
                    Request = context,
                    Response = response.ToJson()
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error GetBalancesForAssetAsync {context}", context);
                isError = true;
            }

            if (isError)
            {
                yield return new GetVaultAccountBalancesResponse()
                {
                    Error = new Grpc.Models.Common.ErrorResponse
                    {
                        ErrorCode = Grpc.Models.Common.ErrorCode.Unknown,
                    }
                };
            }

            var batchSize =request.BatchSize > 0 && request.BatchSize < 1000 ? request.BatchSize : 100;

            if (response.Result.Any())
            {
                var vaultAccounts = response.Result.Select(x => new MyJetWallet.Fireblocks.Domain.Models.VaultAccounts.VaultAccount()
                {
                    AutoFuel = x.AutoFuel,
                    CustomerRefId = x.CustomerRefId,
                    HiddenOnUI = x.HiddenOnUI,
                    Id = x.Id,
                    Name = x.Name,
                    VaultAssets = x.Assets.Select(x =>
                    {
                        decimal.TryParse(x.Available, out var available);
                        decimal.TryParse(x.Frozen, out var frozen);
                        decimal.TryParse(x.LockedAmount, out var lockedAmount);
                        decimal.TryParse(x.Pending, out var pending);
                        decimal.TryParse(x.Staked, out var staked);
                        decimal.TryParse(x.Total, out var total);

                        return new MyJetWallet.Fireblocks.Domain.Models.VaultAssets.VaultAsset()
                        {
                            Id = x.Id,
                            Available = available,
                            BlockHash = x.BlockHash,
                            BlockHeight = x.BlockHeight,
                            Frozen = frozen,
                            LockedAmount = lockedAmount,
                            Pending = pending,
                            Staked = staked,
                            Total = total
                        };
                    }).ToArray()
                }).ToArray();

                foreach (var item in vaultAccounts.Batch(batchSize))
                {
                    _logger.LogInformation("Streaming batch: {context}", new
                    {
                        Request = context,
                        Batch = item.ToJson()
                    });

                    yield return new GetVaultAccountBalancesResponse
                    {
                        VaultAccounts = item,
                    };
                }
            }
            else
            {
                yield return new GetVaultAccountBalancesResponse()
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
