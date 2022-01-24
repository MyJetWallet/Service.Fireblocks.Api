using System.Runtime.Serialization;

namespace Service.Fireblocks.Api.Grpc.Models.Balances
{
    [DataContract]
    public class GetVaultAccountBalancesRequest
    {
        [DataMember(Order = 1)]
        public string NamePrefix { get; set; }

        [DataMember(Order = 2)]
        public string FireblocksAssetId { get; set; }

        [DataMember(Order = 3)]
        public decimal Threshold { get; set; }

        [DataMember(Order = 4)]
        public int BatchSize { get; set; }
    }
}