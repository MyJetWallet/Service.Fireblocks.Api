using MyJetWallet.Sdk.Service;
using MyYamlParser;

namespace Service.Fireblocks.Api.Settings
{
    public class SettingsModel
    {
        [YamlProperty("FireblocksApi.SeqServiceUrl")]
        public string SeqServiceUrl { get; set; }

        [YamlProperty("FireblocksApi.ZipkinUrl")]
        public string ZipkinUrl { get; set; }

        [YamlProperty("FireblocksApi.ElkLogs")]
        public LogElkSettings ElkLogs { get; set; }

        [YamlProperty("FireblocksApi.BaseUrl")]
        public string FireblocksBaseUrl { get; set; }

        [YamlProperty("FireblocksApi.MyNoSqlWriterUrl")]
        public string MyNoSqlWriterUrl { get; internal set; }

        [YamlProperty("FireblocksApi.MyNoSqlReaderHostPort")]
        public string MyNoSqlReaderHostPort { get; internal set; }
    }
}
