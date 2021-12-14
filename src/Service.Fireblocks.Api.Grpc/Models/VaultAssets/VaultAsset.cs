using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Service.Fireblocks.Api.Grpc.Models.VaultAssets
{
    [DataContract]
    public class VaultAsset
    {
        [DataMember(Order = 1)]
        public string Id { get; set; }

        [DataMember(Order = 2)]
        public decimal Total { get; set; }

        [DataMember(Order = 3)]
        public decimal Available { get; set; }

        [DataMember(Order = 4)]
        public decimal Pending { get; set; }

        [DataMember(Order = 5)]
        public decimal Staked { get; set; }

        [DataMember(Order = 6)]
        public decimal Frozen { get; set; }

        [DataMember(Order = 7)]
        public decimal LockedAmount { get; set; }

        [DataMember(Order = 8)]
        public string BlockHeight { get; set; }

        [DataMember(Order = 9)]
        public string BlockHash { get; set; }
    }
}
