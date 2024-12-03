using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Lunity
{
    public static class LWeb
    {
        private static readonly HttpClient Client = new HttpClient();
        
        public static async Task<string> GetAsync(string uri, Action<string> onError = null)
        {
            if(Application.isEditor) Debug.Log("GET: " + uri);
            var response = await Client.GetAsync(uri);
            try {
                return await response.Content.ReadAsStringAsync();
            } catch (HttpRequestException e) {
                onError?.Invoke(e.Message);
                return "";
            }
        }
        
        public static async Task<byte[]> GetBytesAsync(string uri, Action<string> onError = null)
        {
            if(Application.isEditor) Debug.Log("GET: " + uri);
            var response = await Client.GetAsync(uri);
            try {
                return await response.Content.ReadAsByteArrayAsync();
            } catch (HttpRequestException e) {
                onError?.Invoke(e.Message);
                return new byte[0];
            }
        }
        
        public static async Task<string> PostAsync(string uri, string body, Action<string> onError = null)
        {
            if(Application.isEditor) Debug.Log("POST: " + uri + "\n" + body);
            var response = await Client.PostAsync(uri, new StringContent(body, Encoding.UTF8, "application/json"));
            try {
                return await response.Content.ReadAsStringAsync();
            } catch (HttpRequestException e) {
                onError?.Invoke(e.Message);
                return "";
            }
        }

        public static IEnumerator GetCoroutine(string uri, Action<string> onResponse, Action<string> onError = null)
        {
            var task = GetAsync(uri, onError);
            yield return new WaitUntil(() => task.IsCompleted);
            onResponse?.Invoke(task.Result);
        }
        
        public static IEnumerator GetBytesCoroutine(string uri, Action<byte[]> onResponse, Action<string> onError = null)
        {
            var task = GetBytesAsync(uri, onError);
            yield return new WaitUntil(() => task.IsCompleted);
            onResponse?.Invoke(task.Result);
        }
        
        public static IEnumerator PostCoroutine(string uri, string body, Action<string> onResponse, Action<string> onError = null)
        {
            var task = PostAsync(uri, body, onError);
            yield return new WaitUntil(() => task.IsCompleted);
            onResponse?.Invoke(task.Result);
        }
    }
}