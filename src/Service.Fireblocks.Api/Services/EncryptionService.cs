using Service.Fireblocks.Api.Grpc;
using Service.Fireblocks.Api.Grpc.Models.Encryption;
using Service.Fireblocks.Api.Utils;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Service.Fireblocks.Api.Services
{
    public class EncryptionService : IEncryptionService
    {
        private readonly SymmetricEncryptionService _symmetricEncryptionService;

        public EncryptionService(SymmetricEncryptionService symmetricEncryptionService)
        {
            this._symmetricEncryptionService = symmetricEncryptionService;
        }

        public Task<EncryptionResponse> EncryptAsync(EncryptionRequest request)
        {
            var result = _symmetricEncryptionService.Encrypt(request.Data);

            return Task.FromResult(new EncryptionResponse
            {
                EncryptedData = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(result))
            });
        }
    }
}
