using System.Runtime.Serialization;

namespace Service.Fireblocks.Api.Grpc.Models.GasStation
{
    [DataContract]
    public class SetGasStationRequest
    {
        [DataMember(Order = 2)]
        public decimal GasThreshold { get; set; }

        [DataMember(Order = 3)]
        public decimal GasCap { get; set; }

        [DataMember(Order = 4)]
        public decimal MaxGasPrice { get; set; }
    }
}