using Service.Fireblocks.Api.Grpc.Models.SupportedAssets;
using System.Collections.Generic;
using System.ServiceModel;

namespace Service.Fireblocks.Api.Grpc
{
    [ServiceContract]
    public interface ISupportedAssetService
    {
        [OperationContract]
        IAsyncEnumerable<GetSupportedAssetResponse> GetSupportedAssetsAsync(GetSupportedAssetsRequest request);
    }
}