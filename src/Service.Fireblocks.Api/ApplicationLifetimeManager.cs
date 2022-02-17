using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyJetWallet.Fireblocks.Client.Auth;
using MyJetWallet.Sdk.NoSql;
using MyJetWallet.Sdk.Service;
using MyNoSqlServer.Abstractions;
using Service.Fireblocks.Api.NoSql;
using Service.Fireblocks.Api.Services;

namespace Service.Fireblocks.Api
{
    public class ApplicationLifetimeManager : ApplicationLifetimeManagerBase
    {
        private readonly ILogger<ApplicationLifetimeManager> _logger;
        private readonly IMyNoSqlServerDataWriter<FireblocksApiKeysNoSql> _myNoSqlServerData;
        private readonly KeyActivator _keyActivator;
        private readonly SymmetricEncryptionService _symmetricEncryptionService;
        private readonly MyNoSqlClientLifeTime _noSqlTcpClient;

        public ApplicationLifetimeManager(
            IHostApplicationLifetime appLifetime, 
            ILogger<ApplicationLifetimeManager> logger,
            IMyNoSqlServerDataWriter<FireblocksApiKeysNoSql> myNoSqlServerData,
            KeyActivator keyActivator,
            SymmetricEncryptionService symmetricEncryptionService,
            MyNoSqlClientLifeTime noSqlTcpClient)
            : base(appLifetime)
        {
            _logger = logger;
            _myNoSqlServerData = myNoSqlServerData;
            _keyActivator = keyActivator;
            _symmetricEncryptionService = symmetricEncryptionService;
            _noSqlTcpClient = noSqlTcpClient;
        }

        protected override void OnStarted()
        {
            _logger.LogInformation("OnStarted has been called.");
            var key = _myNoSqlServerData.GetAsync(FireblocksApiKeysNoSql.GeneratePartitionKey(), FireblocksApiKeysNoSql.GenerateRowKey()).Result;

            if (key != null)
            {
                try
                {
                    var apiKey = _symmetricEncryptionService.Decrypt(key.ApiKey);
                    var privateKey = _symmetricEncryptionService.Decrypt(key.PrivateKey);
                    _keyActivator.ActivateKeys(apiKey, privateKey);
                }
                catch (System.Exception e)
                {
                    _logger.LogError(e, "PLS< SET UP KEYS FOR API");
                }
            }

            _noSqlTcpClient.Start();
        }

        protected override void OnStopping()
        {
            _logger.LogInformation("OnStopping has been called.");
            _noSqlTcpClient.Stop();
        }

        protected override void OnStopped()
        {
            _logger.LogInformation("OnStopped has been called.");
        }
    }
}
