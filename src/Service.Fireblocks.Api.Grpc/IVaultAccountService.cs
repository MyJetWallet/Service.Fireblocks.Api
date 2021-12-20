using System.ServiceModel;
using System.Threading.Tasks;
using Service.Fireblocks.Api.Grpc.Models.Addresses;
using Service.Fireblocks.Api.Grpc.Models.VaultAccounts;
using Service.Fireblocks.Api.Grpc.Models.VaultAssets;

namespace Service.Fireblocks.Api.Grpc
{
    [ServiceContract]
    public interface IVaultAccountService
    {
        [OperationContract]
        Task<CreateVaultAccountResponse> CreateVaultAccountAsync(CreateVaultAccountRequest request);

        [OperationContract]
        Task<GetVaultAccountResponse> GetVaultAccountAsync(GetVaultAccountRequest request);

        [OperationContract]
        Task<CreateVaultAssetResponse> CreateVaultAssetAsync(CreateVaultAssetRequest request);

        [OperationContract]
        Task<GetVaultAssetResponse> GetVaultAssetAsync(GetVaultAssetRequest request);

        [OperationContract]
        Task<CreateVaultAddressResponse> CreateVaultAddressAsync(CreateVaultAddressRequest request);

        [OperationContract]
        Task<GetVaultAddressResponse> GetVaultAddressAsync(GetVaultAddressRequest request);

        [OperationContract]
        Task<ValidateAddressResponse> ValidateAddressAsync(ValidateAddressRequest request);
    }
}