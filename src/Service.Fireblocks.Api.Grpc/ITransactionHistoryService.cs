using System.ServiceModel;
using System.Threading.Tasks;
using Service.Fireblocks.Api.Grpc.Models.VaultAccounts;

namespace Service.Fireblocks.Api.Grpc
{
    [ServiceContract]
    public interface ITransactionHistoryService
    {
        [OperationContract]
        Task<GetVaultAccountResponse> GetVaultAccountAsync(GetVaultAccountRequest request);
    }
}