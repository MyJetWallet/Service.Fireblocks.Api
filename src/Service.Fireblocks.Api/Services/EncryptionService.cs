using MyJetWallet.Fireblocks.Client.Auth;
using MyNoSqlServer.Abstractions;
using Service.Fireblocks.Api.Grpc;
using Service.Fireblocks.Api.Grpc.Models.Encryption;
using Service.Fireblocks.Api.NoSql;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Service.Fireblocks.Api.Services
{
    public class EncryptionService : IEncryptionService
    {
        private readonly SymmetricEncryptionService _symmetricEncryptionService;
        private readonly IMyNoSqlServerDataWriter<FireblocksApiKeysNoSql> _myNoSqlServerDataReader;
        private readonly KeyActivator _keyActivator;

        public EncryptionService(SymmetricEncryptionService symmetricEncryptionService,
            IMyNoSqlServerDataWriter<FireblocksApiKeysNoSql> myNoSqlServerDataReader,
            KeyActivator keyActivator)
        {
            this._symmetricEncryptionService = symmetricEncryptionService;
            this._myNoSqlServerDataReader = myNoSqlServerDataReader;
            this._keyActivator = keyActivator;
        }

        public Task<EncryptionResponse> EncryptAsync(EncryptionRequest request)
        {
            var result = _symmetricEncryptionService.Encrypt(request.Data.Trim());

            return Task.FromResult(new EncryptionResponse
            {
                EncryptedData = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(result.Trim()))
            });
        }

        public async Task<SetApiKeyResponse> SetApiKeysAsync(SetApiKeyRequest request)
        {
            var apiKey = _symmetricEncryptionService.Encrypt(request.ApiKey);
            var privateKey = request.PrivateKey.Replace("-----BEGIN PRIVATE KEY-----", "");
            privateKey = privateKey.Replace("-----END PRIVATE KEY-----", "");
            var privateKeyEnc = _symmetricEncryptionService.Encrypt(privateKey);

            await _myNoSqlServerDataReader.InsertOrReplaceAsync(FireblocksApiKeysNoSql.Create(apiKey, privateKeyEnc));

            _keyActivator.ActivateKeys(request.ApiKey, privateKey);

            return new SetApiKeyResponse { };
        }
    }
}
