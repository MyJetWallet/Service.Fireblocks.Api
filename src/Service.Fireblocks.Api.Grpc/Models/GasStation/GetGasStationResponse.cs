using Service.Fireblocks.Api.Grpc.Models.Common;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Service.Fireblocks.Api.Grpc.Models.GasStation
{
    [DataContract]
    public class GetGasStationResponse
    {
        [DataMember(Order = 1)]
        public ErrorResponse Error { get; set; }

        [DataMember(Order = 2)]
        public decimal GasThreshold { get; set; }

        [DataMember(Order = 3)]
        public decimal GasCap { get; set; }

        [DataMember(Order = 4)]
        public decimal MaxGasPrice { get; set; }

        [DataMember(Order = 5)]
        public IReadOnlyCollection<GasStationBalance> Balances { get; set; }
    }

}