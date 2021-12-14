using System.ServiceModel;
using System.Threading.Tasks;
using Service.Fireblocks.Api.Grpc.Models.Encryption;

namespace Service.Fireblocks.Api.Grpc
{
    [ServiceContract]
    public interface IEncryptionService
    {
        [OperationContract]
        Task<EncryptionResponse> EncryptAsync(EncryptionRequest request);
    }
}