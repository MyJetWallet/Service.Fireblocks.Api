using JetBrains.Annotations;
using MyJetWallet.Sdk.Grpc;
using Service.Fireblocks.Api.Grpc;

namespace Service.Fireblocks.Api.Client
{
    [UsedImplicitly]
    public class FireblocksApiClientFactory: MyGrpcClientFactory
    {
        public FireblocksApiClientFactory(string grpcServiceUrl) : base(grpcServiceUrl)
        {
        }

        public IVaultAccountService GetVaultAccountService() => CreateGrpcService<IVaultAccountService>();

        public IGasStationService GetGasStationService() => CreateGrpcService<IGasStationService>();

        public ISupportedAssetService GetSupportedAssetServiceService() => CreateGrpcService<ISupportedAssetService>();

        public IEncryptionService GetEncryptionService() => CreateGrpcService<IEncryptionService>();
    }
}
