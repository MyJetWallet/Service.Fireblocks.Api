using System.ServiceModel;
using System.Threading.Tasks;
using Service.Fireblocks.Api.Grpc.Models.TransactionHistory;

namespace Service.Fireblocks.Api.Grpc
{
    [ServiceContract]
    public interface ITransactionHistoryService
    {
        [OperationContract]
        Task<GetTransactionHistoryResponse> GetTransactionHistoryAsync(GetTransactionHistoryRequest request);
    }
}