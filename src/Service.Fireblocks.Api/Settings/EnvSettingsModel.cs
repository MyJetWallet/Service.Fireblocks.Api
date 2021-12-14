using System;
using System.Text;

namespace Service.Fireblocks.Api.Settings
{
    public class EnvSettingsModel
    {
        public string ENCRYPTION_KEY { get; set; }

        public string GetEncryptionKey()
        {
            return ENCRYPTION_KEY.Trim();
        }

    }
}
