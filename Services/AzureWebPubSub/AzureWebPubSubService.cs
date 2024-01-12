using Azure;
using Azure.Messaging.WebPubSub;
using Azure.Messaging.WebPubSub.Clients;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Mug.Services.AzureWebPubSub
{
    public class AzureWebPubSubService
    {
        private static readonly int _regenerateHours = 6;
        private static readonly string[] roles = ["webpubsub.joinLeaveGroup", "webpubsub.sendToGroup"];

        private readonly string _endpoint;
        private readonly string _key;


        private DateTime _lastUriGenerationTime;
        private WebPubSubServiceClient _serviceClient;
        private WebPubSubClient? _client;

        public AzureWebPubSubService(string endpoint, string key)
        {
            _endpoint = endpoint;
            _key = key;
            _serviceClient = new WebPubSubServiceClient(new Uri(_endpoint), "Hub", new AzureKeyCredential(_key));
        }

        public WebPubSubServiceClient ServiceClient {  get { return _serviceClient; } }

        public async Task<WebPubSubClient?> GetClient()
        {
            if ((DateTime.UtcNow - _lastUriGenerationTime).TotalHours >= _regenerateHours)
            {
                if (_client != null)
                {
                    try
                    {
                        await _client.StopAsync();
                    }
                    catch (ObjectDisposedException)
                    {
                        // already disposed
                    }
                }

                var uri = await _serviceClient.GetClientAccessUriAsync(roles: roles, expiresAfter: TimeSpan.FromHours(24));
                _lastUriGenerationTime = DateTime.Now;
                _client = new WebPubSubClient(uri);
                await _client.StartAsync();
            }
            return _client;
        }
    }
}
