using System.Runtime.Serialization;

namespace Service.Fireblocks.Api.Grpc.Models.VaultAssets
{
    [DataContract]
    public class GetVaultAssetRequest
    {
        [DataMember(Order = 1)]
        public string VaultAccountId { get; set; }

        [DataMember(Order = 2)]
        public string AsssetId { get; set; }
    }
}