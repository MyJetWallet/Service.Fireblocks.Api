using MyJetWallet.Fireblocks.Domain.Models.Addresses;
using Service.Fireblocks.Api.Grpc.Models.Common;
using System.Runtime.Serialization;

namespace Service.Fireblocks.Api.Grpc.Models.Addresses
{
    [DataContract]
    public class CreateVaultAddressResponse
    {
        [DataMember(Order = 1)]
        public ErrorResponse Error { get; set; }

        [DataMember(Order = 2)]
        public VaultAddress VaultAddress { get; set; }
    }
}