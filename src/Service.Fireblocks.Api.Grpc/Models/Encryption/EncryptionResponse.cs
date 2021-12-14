using System.Runtime.Serialization;

namespace Service.Fireblocks.Api.Grpc.Models.Encryption
{
    [DataContract]
    public class EncryptionResponse
    {
        [DataMember(Order = 1)]
        public string EncryptedData { get; set; }
    }
}
