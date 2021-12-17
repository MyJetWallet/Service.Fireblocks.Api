using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using MyJetWallet.Fireblocks.Client.Autofac;
using Service.Fireblocks.Api.Services;
using System.IO;
using MyJetWallet.Sdk.NoSql;
using Service.Fireblocks.Api.NoSql;

namespace Service.Fireblocks.Api.Modules
{
    public class ServiceModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            System.Console.WriteLine(Program.EnvSettings.GetEncryptionKey());
            var encryptionService = new SymmetricEncryptionService(Program.EnvSettings.GetEncryptionKey());
            builder.RegisterInstance(encryptionService);

            builder.RegisterFireblocksClient(new MyJetWallet.Fireblocks.Client.ClientConfigurator()
            {
                //ApiKey = ,
                //ApiPrivateKey = ,
                BaseUrl = Program.Settings.FireblocksBaseUrl,
            });

            builder.RegisterMyNoSqlWriter<FireblocksApiKeysNoSql>(() => Program.Settings.MyNoSqlWriterUrl, FireblocksApiKeysNoSql.TableName);
        }
    }
}