using MyJetWallet.Fireblocks.Domain.Models.SupportedAssets;
using MyJetWallet.Fireblocks.Domain.Models.VaultAccounts;
using Service.Fireblocks.Api.Grpc.Models.Common;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Service.Fireblocks.Api.Grpc.Models.SupportedAssets
{
    [DataContract]
    public class GetSupportedAssetResponse
    {
        [DataMember(Order = 1)]
        public ErrorResponse Error { get; set; }

        [DataMember(Order = 2)]
        public IReadOnlyCollection<SupportedAsset> SupportedAssets { get; set; }
    }
}