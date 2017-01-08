using Jbe.NewsReader.Applications.Services;
using System;
using System.Composition;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Jbe.NewsReader.ExternalServices
{
    [Export(typeof(IWebStorageService)), Shared]
    internal class WebStorageService : IWebStorageService
    {
        public async Task<bool> DownloadFileAsync(string fileName, Stream destination, string token)
        {
            using (var client = CreateClient(token))
            {
                var result = await client.GetAsync($"drive/special/approot:/{fileName}:/content").ConfigureAwait(false);
                if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return false;
                }
                result.EnsureSuccessStatusCode();
                
                using (var webStream = await result.Content.ReadAsStreamAsync().ConfigureAwait(false))
                {
                    await webStream.CopyToAsync(destination).ConfigureAwait(false);
                }
                return true;
            }
        }

        public async Task UploadFileAsync(Stream source, string fileName, string token)
        {
            using (var client = CreateClient(token))
            {
                var result = await client.PutAsync($"drive/special/approot:/{fileName}:/content", new StreamContent(source)).ConfigureAwait(false);
                result.EnsureSuccessStatusCode();
            }
        }

        private HttpClient CreateClient(string token)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(@"https://api.onedrive.com/v1.0/");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }
    }
}
