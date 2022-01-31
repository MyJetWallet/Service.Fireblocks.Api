using System.Runtime.Serialization;

namespace Service.Fireblocks.Api.Grpc.Models.SupportedAssets
{
    [DataContract]
    public class GetSupportedAssetsRequest
    {
        [DataMember(Order = 1)]
        public int BatchSize { get; set; }
    }
}