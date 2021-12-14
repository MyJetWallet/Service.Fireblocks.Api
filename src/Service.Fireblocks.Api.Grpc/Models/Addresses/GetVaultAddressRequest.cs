using System.Runtime.Serialization;

namespace Service.Fireblocks.Api.Grpc.Models.Addresses
{
    [DataContract]
    public class GetVaultAddressRequest
    {
        [DataMember(Order = 1)]
        public string VaultAccountId { get; set; }

        [DataMember(Order = 2)]
        public string AssetId { get; set; }
    }
}