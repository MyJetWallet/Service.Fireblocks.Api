using Autofac;
using Service.Fireblocks.Api.Grpc;

// ReSharper disable UnusedMember.Global

namespace Service.Fireblocks.Api.Client
{
    public static class AutofacHelper
    {
        public static void RegisterFireblocksApiClient(this ContainerBuilder builder, string grpcServiceUrl)
        {
            var factory = new FireblocksApiClientFactory(grpcServiceUrl);

            builder.RegisterInstance(factory.GetVaultAccountService()).As<IVaultAccountService>().SingleInstance();
            builder.RegisterInstance(factory.GetGasStationService()).As<IGasStationService>().SingleInstance();
            builder.RegisterInstance(factory.GetSupportedAssetServiceService()).As<ISupportedAssetService>().SingleInstance();
            builder.RegisterInstance(factory.GetTransactionHistoryService()).As<ITransactionHistoryService>().SingleInstance();
        }
    }
}
