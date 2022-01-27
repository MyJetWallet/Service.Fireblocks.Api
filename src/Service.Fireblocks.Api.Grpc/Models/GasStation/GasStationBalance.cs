using System.Runtime.Serialization;

namespace Service.Fireblocks.Api.Grpc.Models.GasStation
{
    [DataContract]
    public class GasStationBalance
    {
        [DataMember(Order = 1)]
        public string FireblocksAssetId { get; set; }

        [DataMember(Order = 2)]
        public decimal Balance { get; set; }
    }
}