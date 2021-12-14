using System.Runtime.Serialization;

namespace Service.Fireblocks.Api.Grpc.Models.VaultAccounts
{
    [DataContract]
    public class CreateVaultAccountRequest
    {
        [DataMember(Order = 1)]
        public string Name { get; set; }

        [DataMember(Order = 2)]
        public string CustomerRefId { get; set; }

        [DataMember(Order = 3)]
        public bool AutoFuel { get; set; }

        [DataMember(Order = 4)]
        public bool HiddenOnUI { get; set; }
    }
}