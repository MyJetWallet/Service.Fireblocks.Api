using MyJetWallet.Fireblocks.Domain.Models.VaultAssets;
using Service.Fireblocks.Api.Grpc.Models.Common;
using Service.Fireblocks.Api.Grpc.Models.VaultAccounts;
using System.Runtime.Serialization;

namespace Service.Fireblocks.Api.Grpc.Models.VaultAssets
{
    [DataContract]
    public class GetVaultAssetResponse
    {
        [DataMember(Order = 1)]
        public ErrorResponse Error { get; set; }

        [DataMember(Order = 2)]
        public VaultAsset VaultAsset { get; set; }
    }
}