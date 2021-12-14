using Service.Fireblocks.Api.Grpc.Models.VaultAssets;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Service.Fireblocks.Api.Grpc.Models.VaultAccounts
{
    [DataContract]
    public class VaultAccount
    {
        [DataMember(Order = 1)]
        public string Id { get; set; }

        [DataMember(Order = 2)]
        public string Name { get; set; }

        [DataMember(Order = 3)]
        public bool HiddenOnUI { get; set; }

        [DataMember(Order = 4)]
        public string CustomerRefId { get; set; }

        [DataMember(Order = 5)]
        public bool AutoFuel { get; set; }

        [DataMember(Order = 6)]
        public IReadOnlyCollection<VaultAsset> VaultAssets { get; set; }
    }
}