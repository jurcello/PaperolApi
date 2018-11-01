using System;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;

namespace Paperol
{

    public class PaperolApiConnector : MonoBehaviour
    {
        [System.Serializable]
        public class LoginData
        {
            public string username;
            public string password;
        }

        [System.Serializable]
        public class LoginResponse
        {
            public string key;
        }

        public string apiEndPoint = "https://paperol.thecodingartist.nl/api/v1/";

        public delegate void ConnectSucces();
        public delegate void GetListCallback(PaperolDataList list);
        public delegate void GetItemCallback(PaperolData loadedData);

        protected string token;
        protected const int timeout = 10;

        public void Connect(string username, string password, ConnectSucces successCallback)
        {

            string connectUrl = apiEndPoint + "rest-auth/login/";
            LoginData data = new LoginData();
            data.password = password;
            data.username = username;
            Debug.Log("Starting connection");
            string jsonBody = JsonUtility.ToJson(data);
            RestClient.Request(new RequestHelper
            {
                Uri = connectUrl,
                Method = "POST",
                Timeout = timeout,
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json"}
                },
                BodyString = jsonBody
            }).Then(response =>
            {
                LoginResponse answer = JsonUtility.FromJson<LoginResponse>(response.Text);
                token = answer.key;
                successCallback();
                Debug.Log(response.Data.ToString());
            }).Catch(err =>
            {
                Debug.LogError(err.Message);
            });
        }

        public bool IsConnected()
        {
            return this.token != null;
        }

        public void GetList(GetListCallback callback)
        {
            if (this.token != null) { 
                string connectUrl = apiEndPoint + "paperol_save/";
                RequestHelper request = BuildDefaultRequestHelper(connectUrl);
                RestClient.Request(request).Then( response =>
                {
                    // We need to create an array of data in order to be able to deserialize.
                    string json = "{ \"data\": " + response.Text + "}";
                    PaperolDataList list = JsonUtility.FromJson<PaperolDataList>(json);
                    callback(list);
                }).Catch(err =>
                {
                    Debug.LogError(err.Message);
                });
            }
        }

        public void GetItem(int id, GetItemCallback callback)
        {
            if (this.token != null)
            {
                string connectUrl = apiEndPoint + String.Format("paperol_save/{0}/", id);
                RequestHelper request = BuildDefaultRequestHelper(connectUrl);
                RestClient.Request(request).Then(response =>
                {
                    PaperolData item = JsonUtility.FromJson<PaperolData>(response.Text);
                    callback(item);
                }).Catch(err => { Debug.LogError(err.Message); });
            }
        }

        protected RequestHelper BuildDefaultRequestHelper(string url, string method = "GET")
        {
            return new RequestHelper
            {
                Uri = url,
                Method = method,
                Timeout = timeout,
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json"},
                    { "Authorization", "Token " + this.token }
                },
            };
        }
    }

}
