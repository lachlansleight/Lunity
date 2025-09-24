using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using UnityEngine;

namespace Lunity
{
    public class AsyncHttp
    {
        private static readonly HttpClient Client = new HttpClient();
        
        public static async void Send(HttpRequestMessage message, Action<string> onSuccess = null, Action<string> onFailure = null)
        {
            try {
                var response = await Client.SendAsync(message);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                onSuccess?.Invoke(responseContent);
            } catch (HttpRequestException e) {
                onFailure?.Invoke(e.Message);
            }
        }
        
        public static async Task<HttpResponseMessage> SendAsync(HttpRequestMessage message, Action<string> onFailure = null)
        {
            try {
                var response = await Client.SendAsync(message);
                response.EnsureSuccessStatusCode();
                return response;
            } catch (HttpRequestException e) {
                onFailure?.Invoke(e.Message);
                return default;
            }
        }
        
        public static async void Get(string uri, Action<string> onSuccess = null, Action<string> onFailure = null)
        {
            try {
                var response = await Client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                onSuccess?.Invoke(responseContent);
            } catch (HttpRequestException e) {
                onFailure?.Invoke(e.Message);
            }
        }

        public static async Task<string> GetAsync(string uri, Action<string> onFailure = null)
        {
            try {
                var response = await Client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            } catch (HttpRequestException e) {
                onFailure?.Invoke(e.Message);
                return "";
            }
        }

        public static async void Post(string uri, string data, Action<string> onSuccess = null, Action<string> onFailure = null)
        {
            try {
                var response = await Client.PostAsync(uri, new StringContent(data));
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                onSuccess?.Invoke(responseContent);
            } catch (HttpRequestException e) {
                onFailure?.Invoke(e.Message);
            }
        }
        
        public static async Task<string> PostAsync(string uri, string data, Action<string> onFailure = null)
        {
            try {
                var response = await Client.PostAsync(uri, new StringContent(data));
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            } catch (HttpRequestException e) {
                onFailure?.Invoke(e.Message);
                return "";
            }
        }
        
        public static async void Patch(string uri, string data, Action<string> onSuccess = null, Action<string> onFailure = null)
        {
            try {
                var response = await Client.PatchAsync(uri, new StringContent(data));
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                onSuccess?.Invoke(responseContent);
            } catch (HttpRequestException e) {
                Debug.Log(e.Message);
                onFailure?.Invoke(e.Message);
            }
        }
        
        public static async Task<string> PatchAsync(string uri, string data, Action<string> onFailure = null)
        {
            try {
                var response = await Client.PatchAsync(uri, new StringContent(data));
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            } catch (HttpRequestException e) {
                onFailure?.Invoke(e.Message);
                return "";
            }
        }
        
        public static async void Put(string uri, string data, Action<string> onSuccess = null, Action<string> onFailure = null)
        {
            try {
                var response = await Client.PutAsync(uri, new StringContent(data));
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                onSuccess?.Invoke(responseContent);
            } catch (HttpRequestException e) {
                onFailure?.Invoke(e.Message);
            }
        }
        
        public static async Task<string> PutAsync(string uri, string data, Action<string> onFailure = null)
        {
            try {
                var response = await Client.PutAsync(uri, new StringContent(data));
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            } catch (HttpRequestException e) {
                onFailure?.Invoke(e.Message);
                return "";
            }
        }
        
        public static async void Delete(string uri, Action<string> onSuccess = null, Action<string> onFailure = null)
        {
            try {
                var response = await Client.DeleteAsync(uri);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                onSuccess?.Invoke(responseContent);
            } catch (HttpRequestException e) {
                onFailure?.Invoke(e.Message);
            }
        }
        
        public static async Task<string> DeleteAsync(string uri, Action<string> onFailure = null)
        {
            try {
                var response = await Client.DeleteAsync(uri);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            } catch (HttpRequestException e) {
                onFailure?.Invoke(e.Message);
                return "";
            }
        }
    }
}