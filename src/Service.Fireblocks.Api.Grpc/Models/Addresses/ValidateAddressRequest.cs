using System.Runtime.Serialization;

namespace Service.Fireblocks.Api.Grpc.Models.Addresses
{
    [DataContract]
    public class ValidateAddressRequest
    {
        [DataMember(Order = 1)]
        public string Address { get; set; }

        [DataMember(Order = 2)]
        public string AssetId { get; set; }
    }
}