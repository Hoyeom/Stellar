using System;
using Cysharp.Threading.Tasks;
using Stellar.Runtime.Extension;
using UnityEngine;
using UnityEngine.Networking;

namespace Plugins.Stellar.Runtime
{
    public class Request
    {
        public readonly long Id;
        private readonly string _method;
        private readonly string _path;
        private readonly object _requestPayload;

        public Request(long id, string method, string path, object requestPayload)
        {
            Id = id;
            _method = method;
            _path = path;
            _requestPayload = requestPayload;
        }

        public async UniTask<ResponseDto<T>> Send<T>(string baseUrl, string accessToken = null,string secretKey = null)
        {
            using UnityWebRequest request = new();

            request.url = $"{baseUrl}/{_path}";
            request.method = _method;
            var requestPayloadString = JsonUtility.ToJson(_requestPayload);
            var buffer = new System.Text.UTF8Encoding().GetBytes(requestPayloadString);
            
            if (!string.IsNullOrWhiteSpace(secretKey))
            {
                var requestHmac = requestPayloadString.ComputeSHA256Hmac(secretKey);
                request.SetRequestHeader("bodyhmac", requestHmac);
            }

            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                request.SetRequestHeader("Authorization", accessToken);
            }

            request.SetRequestHeader("Content-Type", "application/json");

            request.uploadHandler = new UploadHandlerRaw(buffer);
            request.downloadHandler = new DownloadHandlerBuffer();
            
#if UNITY_EDITOR
            Debug.Log($"[Request Id: {Id}] Method: {_method}, Url: {request.url}, Payload: {requestPayloadString}".ToColorString(Color.green));
#endif
            
            try
            {
                await request.SendWebRequest();
            }
            catch (Exception e)
            {
                // ignored
            }

            ResponseDto<T> response;
            string message;

            if (request.result == UnityWebRequest.Result.Success)
            {
                message = request.downloadHandler.text;
                response = JsonUtility.FromJson<ResponseDto<T>>(message);
                
                string responseHmac = request.GetResponseHeader("responseHmac");
                string createResponseHmac = message.ComputeSHA256Hmac(secretKey);
                
                if (!responseHmac.Equals(createResponseHmac))
                {
                    response = new ResponseDto<T>()
                    {
                        Success = false,
                        Message = "Invalid Hmac"
                    };
                }
            }
            else
            {
                message = request.error;
                response = new ResponseDto<T>()
                {
                    Success = false,
                    Message = message,
                };
            }

#if UNITY_EDITOR
            Debug.Log($"[Request Id: {Id}] Method: {_method}, Url: {request.url}, Message: {message}".ToColorString(response.Success ? Color.green : Color.red));
#endif
            
            return response;
        }
    }
}