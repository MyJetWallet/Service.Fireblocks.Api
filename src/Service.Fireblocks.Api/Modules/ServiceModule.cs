using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using MyJetWallet.Fireblocks.Client.Autofac;
using Service.Fireblocks.Api.Services;
using System.IO;

namespace Service.Fireblocks.Api.Modules
{
    public class ServiceModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var encryptionService = new SymmetricEncryptionService(Program.EnvSettings.GetEncryptionKey());
            builder.RegisterInstance(encryptionService);

            //var publicKey = File.ReadAllTextAsync(@"C:\Users\O1\Desktop\fireblocks\fireblocks_api_key").Result;
            //var x = encryptionService.Encrypt(publicKey);
            //var x1 = encryptionService.Decrypt(x);

            //var privateKey = File.ReadAllTextAsync(@"C:\Users\O1\Desktop\fireblocks\fireblocks_secret.key").Result;
            //var y = encryptionService.Encrypt(privateKey);
            //var y1 = encryptionService.Decrypt(y);

            var privateKey = Program.EnvSettings.GetFireblocksPrivateKey();
            privateKey = encryptionService.Decrypt(privateKey);
            privateKey = privateKey.Replace("-----BEGIN PRIVATE KEY-----", "");
            privateKey = privateKey.Replace("-----END PRIVATE KEY-----", "");


            var apiKey = Program.EnvSettings.GetFireblocksApiKey();
            apiKey = encryptionService.Decrypt(apiKey);

            builder.RegisterFireblocksClient(new MyJetWallet.Fireblocks.Client.ClientConfigurator()
            {
                ApiKey = apiKey,
                ApiPrivateKey = privateKey,
                BaseUrl = Program.Settings.FireblocksBaseUrl,
            });
        }
    }
}