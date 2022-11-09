using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

namespace Lunity
{
    public static class LWeb
    {
        private static readonly HttpClient Client = new HttpClient();
        
        public static async Task<string> GetAsync(string uri, Action<string> onError = null)
        {
            Debug.Log("Fetching at url " + uri);
            var response = await Client.GetAsync(uri);
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
    }
}