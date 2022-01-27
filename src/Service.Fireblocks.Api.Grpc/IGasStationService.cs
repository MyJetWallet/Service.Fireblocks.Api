using Service.Fireblocks.Api.Grpc.Models.GasStation;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Service.Fireblocks.Api.Grpc
{
    [ServiceContract]
    public interface IGasStationService
    {
        [OperationContract]
        Task<GetGasStationResponse> GetGasStationAsync(GetGasStationRequest request);

        [OperationContract]
        Task<SetGasStationResponse> SetGasStationAsync(SetGasStationRequest request);
    }
}