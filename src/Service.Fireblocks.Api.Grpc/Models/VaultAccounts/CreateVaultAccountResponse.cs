using Service.Fireblocks.Api.Grpc.Models.Common;
using System.Runtime.Serialization;

namespace Service.Fireblocks.Api.Grpc.Models.VaultAccounts
{
    [DataContract]
    public class CreateVaultAccountResponse
    {
        [DataMember(Order = 1)]
        public ErrorResponse Error { get; set; }

        [DataMember(Order = 2)]
        public VaultAccount VaultAccount { get; set; }
    }
}