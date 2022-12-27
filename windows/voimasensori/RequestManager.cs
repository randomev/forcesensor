using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace voimasensori
{
    /// <summary>
    /// Reqeust manager encapsulates asynchronous HTTP GET and POST methods
    /// In conjunction with that, it internally caches requests made in the same minute
    /// 
    /// https://gist.github.com/olafurjohannsson/2ee0d44c0e6d55c836c6e62dea2aaf8a
    /// 
    /// </summary>
    public class RequestManager
    {
        private Dictionary<string, string> CustomHeaders = null;

        public RequestManager(Dictionary<string, string> customHeaders = null)
        {
            this.CustomHeaders = customHeaders;
        }

        /// <summary>
        /// Make an async HTTP POST request
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public async Task<string> POST(string url, Dictionary<string, string> data, string contentType = "application/json")
        {
            return await ConstructAndMakeRequest(url, HttpMethod.Post, contentType, data);
        }

        /// <summary>
        /// Make an async HTTP GET request
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<string> GET(string url)
        {
            return await ConstructAndMakeRequest(url, HttpMethod.Get, string.Empty, null);
        }

        /// <summary>
        /// Build up our request
        /// </summary>
        private async Task<string> ConstructAndMakeRequest(string url, HttpMethod method, string contentType, Dictionary<string, string> postData)
        {
            string data = string.Empty;

            string key = string.Concat(url, DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"));

            // Check the cache first
            if ((data = (Cache<string>.Get(key))) == null)
            {
                // POST or GET
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(3);

                    // we have custom headers
                    if (this.CustomHeaders != null && this.CustomHeaders.Count > 0)
                    {
                        // Add all our custom headers to our 
                        foreach (var header in this.CustomHeaders)
                        {
                            client.DefaultRequestHeaders.Add(header.Key, header.Value);
                        }
                    }

                    // POST method
                    if (method == HttpMethod.Post)
                    {
                        var response = await client.PostAsync(url, new FormUrlEncodedContent(postData));
                        data = await response.Content.ReadAsStringAsync();
                    }
                    else if (method == HttpMethod.Get)
                    {
                        data = await client.GetStringAsync(url);
                    }
                    else
                    {
                        throw new ArgumentException("Method not supported");
                    }
                }

                // Cache data in memory using key that expires each minute
                if (!string.IsNullOrEmpty(data) && Cache<string>.Get(key) == null)
                {
                    Cache<string>.Set(key, data);
                }
            }


            return data;
        }
    }

    /// <summary>
    /// Very simple in memory cache
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal static class Cache<T>
    {
        static MemoryCache memoryCache;

        static Cache()
        {
            if (memoryCache == null)
            {
                memoryCache = new MemoryCache("memCache");
            }
        }

        public static T Get(string key)
        {
            return (T)memoryCache.Get(key);
        }

        public static void Set(string key, T value)
        {
            // evicted after 24hrs
            memoryCache.Set(key, value, new DateTimeOffset(DateTime.UtcNow.AddMinutes(5)));
        }
    }
}
