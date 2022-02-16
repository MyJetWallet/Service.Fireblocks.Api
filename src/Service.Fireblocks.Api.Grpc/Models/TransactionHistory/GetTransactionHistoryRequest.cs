using System.Runtime.Serialization;

namespace Service.Fireblocks.Api.Grpc.Models.TransactionHistory
{
    [DataContract]
    public class GetTransactionHistoryRequest
    {
        [DataMember(Order = 1)]
        public long BeforeUnixTime { get; set; }

        [DataMember(Order = 2)]
        public int Take { get; set; }
    }
}