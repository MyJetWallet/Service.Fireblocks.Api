using Service.Fireblocks.Api.Grpc.Models.Common;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Service.Fireblocks.Api.Grpc.Models.GasStation
{
    [DataContract]
    public class SetGasStationResponse
    {
        [DataMember(Order = 1)]
        public ErrorResponse Error { get; set; }
    }
}