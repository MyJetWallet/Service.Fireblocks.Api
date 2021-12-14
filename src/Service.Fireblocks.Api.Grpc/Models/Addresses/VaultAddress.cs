using Service.Fireblocks.Api.Grpc.Models.VaultAssets;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Service.Fireblocks.Api.Grpc.Models.Addresses
{
    [DataContract]
    public class VaultAddress
    {
        [DataMember(Order = 1)]
        public string Address { get; set; }

        [DataMember(Order = 2)]
        public string LegacyAddress { get; set; }

        [DataMember(Order = 3)]
        public string EnterpriseAddress { get; set; }

        [DataMember(Order = 4)]
        public string Tag { get; set; }

        [DataMember(Order = 5)]
        public decimal Bip44AddressIndex { get; set; }
    }
}