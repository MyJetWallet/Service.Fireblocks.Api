using Service.Fireblocks.Api.Grpc.Models.Addresses;
using Service.Fireblocks.Api.Grpc.Models.Common;
using Service.Fireblocks.Api.Grpc.Models.VaultAccounts;
using System.Runtime.Serialization;

namespace Service.Fireblocks.Api.Grpc.Models.VaultAssets
{
    [DataContract]
    public class CreateVaultAssetResponse
    {
        [DataMember(Order = 1)]
        public ErrorResponse Error { get; set; }

        [DataMember(Order = 2)]
        public VaultAsset VaultAsset { get; set; }

        [DataMember(Order = 3)]
        public VaultAddress VaultAddress { get; set; }
    }
}