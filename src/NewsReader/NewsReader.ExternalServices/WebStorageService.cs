using Jbe.NewsReader.Applications.Services;
using System;
using System.Collections.Generic;
using System.Composition;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Jbe.NewsReader.ExternalServices
{
    [Export(typeof(IWebStorageService)), Shared]
    internal class WebStorageService : IWebStorageService
    {
        public async Task<string> DownloadFileAsync(string fileName, Stream destination, string token, string eTag)
        {
            using (var client = CreateClient(token))
            {
                if (!string.IsNullOrEmpty(eTag)) { client.DefaultRequestHeaders.IfNoneMatch.Add(new EntityTagHeaderValue("\"" + eTag + "\"")); }
                var response = await client.GetAsync($"drive/special/approot:/{fileName}:/content").ConfigureAwait(false);
                if (response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == HttpStatusCode.NotModified)
                {
                    return null;
                }
                response.EnsureSuccessStatusCode();

                using (var webStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                {
                    await webStream.CopyToAsync(destination).ConfigureAwait(false);
                }
                return GetETag(response);
            }
        }

        public async Task<string> UploadFileAsync(Stream source, string fileName, string token)
        {
            using (var client = CreateClient(token))
            {
                var response = await client.PutAsync($"drive/special/approot:/{fileName}:/content", new StreamContent(source)).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                return GetETag(response);
            }
        }

        private string GetETag(HttpResponseMessage response)
        {
            IEnumerable<string> values;
            if (response.Headers.TryGetValues("ETag", out values))
            {
                return values.FirstOrDefault();
            }
            return null;
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
