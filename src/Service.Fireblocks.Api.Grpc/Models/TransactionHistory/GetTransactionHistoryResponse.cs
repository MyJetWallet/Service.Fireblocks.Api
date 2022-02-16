using Service.Fireblocks.Api.Grpc.Models.Common;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Service.Fireblocks.Api.Grpc.Models.TransactionHistory
{
    [DataContract]
    public class GetTransactionHistoryResponse
    {
        [DataMember(Order = 1)]
        public ErrorResponse Error { get; set; }

        [DataMember(Order = 2)]
        public IReadOnlyCollection<MyJetWallet.Fireblocks.Domain.Models.TransactionHistories.TransactionHistory> History { get; set; }
    }
}