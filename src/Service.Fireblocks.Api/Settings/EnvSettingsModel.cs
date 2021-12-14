using System;
using System.Text;

namespace Service.Fireblocks.Api.Settings
{
    public class EnvSettingsModel
    {
        public string ENCRYPTION_KEY { get; set; }

        public string FIREBLOCKS_PRIVATE_KEY { get; set; }

        public string FIREBLOCKS_API_KEY { get; set; }

        public string GetEncryptionKey()
        {
            return ENCRYPTION_KEY.Trim();
        }

        public string GetFireblocksPrivateKey()
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(FIREBLOCKS_PRIVATE_KEY)).Trim();
        }

        public string GetFireblocksApiKey()
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(FIREBLOCKS_API_KEY)).Trim();
        }
    }
}
