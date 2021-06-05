using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace BennyKok.NotionAPI
{
    public class NotionAPI
    {
        private bool debug = true;
        private string apiKey;
        private readonly static string version = "v1";
        private readonly static string rootUrl = $"https://api.notion.com/{version}";
        private readonly static string urlDB = rootUrl + "/databases";
        private readonly static string urlUsers = rootUrl + "/users";
        private readonly static string apiVersion = "2021-05-13";

        public NotionAPI(string apiKey)
        {
            this.apiKey = apiKey;
        }

        enum RequestType
        {
            GET, POST, PATCH
        }

        private UnityWebRequest WebRequestWithAuth(string url, RequestType requestType, WWWForm form = null, string data = null)
        {
            UnityWebRequest request = null;
            switch (requestType)
            {
                case RequestType.GET:
                    request = UnityWebRequest.Get(url);
                    break;
                case RequestType.POST:
                    request = UnityWebRequest.Post(url, form);
                    break;
                case RequestType.PATCH:
                    request = UnityWebRequest.Put(url, data);
                    request.method = "PATCH";
                    break;
            }
            request.SetRequestHeader("Authorization", $"Bearer {apiKey}");
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Notion-Version", apiVersion);
            return request;
        }

        public IEnumerator GetJSON(string url, Action<string> callback)
        {
            if (debug) Debug.Log("GET Requesting: " + url);
            using (var request = WebRequestWithAuth(url, RequestType.GET))
            {
                yield return request.SendWebRequest();
                var data = request.downloadHandler.text;
                callback(data);
            }
        }

        public IEnumerator PostJSON(string url, Action<string> callback, WWWForm form)
        {
            if (debug) Debug.Log("POST Requesting: " + url);
            using (var request = WebRequestWithAuth(url, RequestType.POST, form))
            {
                yield return request.SendWebRequest();
                var data = request.downloadHandler.text;
                callback(data);
            }
        }

        public IEnumerator PatchJSON(string url, string data, Action<string> callback)
        {
            if (debug) Debug.Log("PATCH Requesting: " + url + data);
            using (var request = WebRequestWithAuth(url, RequestType.PATCH, null, data))
            {
                yield return request.SendWebRequest();
                var result = request.downloadHandler.text;
                callback(result);
            }
        }

        /// <summary>
        /// Get the Notion Database JSON object parsed with Unity's JsonUtility
        /// </summary>
        /// <param name="database_id">Database Id</param>
        /// <param name="callback"></param>
        /// <typeparam name="T">An serializable class containing all Property field for the Json parsing</typeparam>
        /// <returns></returns>
        public IEnumerator GetDatabase<T>(string database_id, Action<Database<T>> callback)
        {
            yield return GetDatabaseJSON(database_id, (json) =>
            {
                if (debug) Debug.Log(json);
                callback(JsonUtility.FromJson<Database<T>>(json));
            });
        }

        /// <summary>
        /// Return the entire Notion Database schema in raw JSON string
        /// </summary>
        /// <param name="database_id">Database Id</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public IEnumerator GetDatabaseJSON(string database_id, Action<string> callback)
        {
            var url = $"{urlDB}/{database_id}";
            yield return GetJSON(url, callback);
        }

        public IEnumerator QueryDatabase<T>(string database_id, Action<DatabaseQueryResponse<T>> callback)
        {
            yield return QueryDatabaseJSON(database_id, (json) =>
            {
                if (debug) Debug.Log(json);
                callback(JsonUtility.FromJson<DatabaseQueryResponse<T>>(json));
            });
        }

        public IEnumerator QueryDatabaseJSON(string database_id, Action<string> callback)
        {
            var url = $"{urlDB}/{database_id}/query";
            yield return PostJSON(url, callback, null);
        }

        public IEnumerator GetUsers(Action<DatabaseUsers> callback)
        {
            var url = $"{urlUsers}/";

            yield return GetJSON(url, (json) =>
            {
                if (debug) Debug.Log(json);
                callback(JsonUtility.FromJson<DatabaseUsers>(json));
            });
        }


        public IEnumerator PatchPageProperties(string pageID, string data, Action<string> callback)
        {
            var url = $"{rootUrl}/pages/{pageID}";
            yield return PatchJSON(url, data, callback);
        }

        public IEnumerator PatchPageProperties<T>(Page<T> page, Action<Page<T>> callback)
        {
            string data = JsonUtility.ToJson(page);
            yield return PatchPageProperties(page.id, data, (json) =>
            {
                if (debug) Debug.Log(json);
                callback?.Invoke(JsonUtility.FromJson<Page<T>>(json));
            });
        }
    }
}
