using System.Runtime.Serialization;

namespace Service.Fireblocks.Api.Grpc.Models.Addresses
{
    [DataContract]
    public class CreateVaultAddressRequest
    {
        [DataMember(Order = 1)]
        public string VaultAccountId { get; set; }

        [DataMember(Order = 2)]
        public string CustomerRefId { get; set; }

        [DataMember(Order = 3)]
        public string AssetId { get; set; }

        [DataMember(Order = 4)]
        public string Name { get; set; }
    }
}