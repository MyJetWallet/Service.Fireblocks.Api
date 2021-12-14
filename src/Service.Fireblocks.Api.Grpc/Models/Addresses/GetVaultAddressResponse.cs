using Service.Fireblocks.Api.Grpc.Models.Common;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Service.Fireblocks.Api.Grpc.Models.Addresses
{
    [DataContract]
    public class GetVaultAddressResponse
    {
        [DataMember(Order = 1)]
        public ErrorResponse Error { get; set; }

        [DataMember(Order = 2)]
        public IReadOnlyCollection<VaultAddress> VaultAddress { get; set; }
    }
}