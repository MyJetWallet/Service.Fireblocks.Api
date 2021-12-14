using System.Runtime.Serialization;

namespace Service.Fireblocks.Api.Grpc.Models.VaultAccounts
{
    [DataContract]
    public class GetVaultAccountRequest
    {
        [DataMember(Order = 1)]
        public string Name { get; set; }

        [DataMember(Order = 2)]
        public string VaultAccountId { get; set; }
    }
}