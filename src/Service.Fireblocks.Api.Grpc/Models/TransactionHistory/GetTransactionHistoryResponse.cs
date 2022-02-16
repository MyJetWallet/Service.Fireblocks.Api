using MyJetWallet.Fireblocks.Domain.Models.VaultAccounts;
using Service.Fireblocks.Api.Grpc.Models.Common;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Service.Fireblocks.Api.Grpc.Models.VaultAccounts
{
    [DataContract]
    public class GetTransactionHistoryResponse
    {
        [DataMember(Order = 1)]
        public ErrorResponse Error { get; set; }

        [DataMember(Order = 2)]
        public IReadOnlyCollection<TransactionHistory> History { get; set; }
    }
}