using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DevOpsResourceCreator.Repository
{
    public class DevOpsRestApi : IDevOpsRestApi
    {
        private readonly string baseUri;
        public DevOpsRestApi(IConfiguration configuration)
        {
            baseUri = configuration.GetSection("DevOps").GetSection("TargetBaseAddress").Value;
        }
        public string Create(string uri, string content, string token)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpContent httpContent = new StringContent(content, Encoding.UTF8, "application/json");
                try 
                { 
                    var response = Task.Run(async () =>
                    {
                        try
                        {
                            using (HttpResponseMessage response = await client.PostAsync(uri, httpContent))
                            {
                                response.EnsureSuccessStatusCode();
                                return (await response.Content.ReadAsStringAsync());
                            }
                        }
                        catch(Exception ex)
                        {
                            var ex1 = ex;
                            throw ex;
                        }
                    });
                    return response.Result;
                }
                catch (AggregateException ae)
                {
                    foreach (var e in ae.InnerExceptions)
                    {
                        throw e;
                    }
                }
                return "";
            }
        }

        public string GetAll(string url, string token)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = Task.Run(async () =>
                {
                    using HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    return (await response.Content.ReadAsStringAsync());
                });
                return response.Result;
            }
        }

        public string Get(string url, string token)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = Task.Run(async () =>
                {
                    using (HttpResponseMessage response = await client.GetAsync(url))
                    {
                        response.EnsureSuccessStatusCode();
                        return (await response.Content.ReadAsStringAsync());
                    }
                });
                return response.Result;
            }
        }

        public string Update(string url, string content, string token)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpContent httpContent = new StringContent(content, Encoding.UTF8, "application/json");

                var response = Task.Run(async () =>
                {
                    using HttpResponseMessage response = await client.PatchAsync(url, httpContent);
                    response.EnsureSuccessStatusCode();
                    return (await response.Content.ReadAsStringAsync());
                });
                return response.Result;
            }
        }

        public string Delete(string url, string token)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = Task.Run(async () =>
                {
                    using HttpResponseMessage response = await client.DeleteAsync(url);
                    response.EnsureSuccessStatusCode();
                    return (await response.Content.ReadAsStringAsync());
                });
                return response.Result;
            }
        }
    }
}
