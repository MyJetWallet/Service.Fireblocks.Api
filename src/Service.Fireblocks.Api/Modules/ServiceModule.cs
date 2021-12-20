using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using MyJetWallet.Fireblocks.Client.Autofac;
using Service.Fireblocks.Api.Services;
using System.IO;
using MyJetWallet.Sdk.NoSql;
using Service.Fireblocks.Api.NoSql;
using MyJetWallet.Fireblocks.Client.DelegateHandlers;
using Microsoft.Extensions.Logging;

namespace Service.Fireblocks.Api.Modules
{
    public class ServiceModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var logger = Program.LogFactory.CreateLogger<LoggerMiddleware>();
            var encryptionService = new SymmetricEncryptionService(Program.EnvSettings.GetEncryptionKey());
            builder.RegisterInstance(encryptionService);

            builder.RegisterFireblocksClient(new MyJetWallet.Fireblocks.Client.ClientConfigurator()
            {
                //ApiKey = ,
                //ApiPrivateKey = ,
                BaseUrl = Program.Settings.FireblocksBaseUrl,
            }, new LoggerMiddleware(logger));

            builder.RegisterMyNoSqlWriter<FireblocksApiKeysNoSql>(() => Program.Settings.MyNoSqlWriterUrl, FireblocksApiKeysNoSql.TableName);
        }
    }
}