using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Stellar.Runtime.Extension;
using UnityEngine;
using UnityEngine.Networking;

namespace Plugins.Stellar.Runtime
{
    public abstract class WebApplicationServer
    {
        private readonly string _baseUrl;
        private string _accessToken;
        private string _secretKey;
        private Queue<Request> _requestQueue;
        private long _createId;
        
        public WebApplicationServer(string baseUrl)
        {
            _baseUrl = baseUrl;
            _requestQueue = new Queue<Request>();
            _createId = 0;
        }

        public void SetSecretKey(string salt)
        {
            _secretKey = salt;
        }
        
        public void SetAccessToken(string accessToken)
        {
            _accessToken = accessToken;
        }

        protected async UniTask<ResponseDto<T>> Post<T>(string path = null, object payload = null)
        {
            Request request = CreateRequest(UnityWebRequest.kHttpVerbPOST, path, payload);
            return await Send<T>(request);
        }

        protected async UniTask<ResponseDto<T>> Get<T>(string path = null, object payload = null)
        {
            Request request = CreateRequest(UnityWebRequest.kHttpVerbGET, path, payload);
            return await Send<T>(request);
        }
        
        protected async UniTask<ResponseDto<T>> Put<T>(string path = null, object payload = null)
        {
            Request request = CreateRequest(UnityWebRequest.kHttpVerbPUT, path, payload);
            return await Send<T>(request);
        }
        
        protected async UniTask<ResponseDto<T>> Create<T>(string path = null, object payload = null)
        {
            Request request = CreateRequest(UnityWebRequest.kHttpVerbCREATE, path, payload);
            return await Send<T>(request);
        }
        
        protected async UniTask<ResponseDto<T>> Delete<T>(string path = null, object payload = null)
        {
            Request request = CreateRequest(UnityWebRequest.kHttpVerbDELETE, path, payload);
            return await Send<T>(request);
        }
       
        protected async UniTask<ResponseDto<T>> Head<T>(string path = null, object payload = null)
        {
            Request request = CreateRequest(UnityWebRequest.kHttpVerbHEAD, path, payload);
            return await Send<T>(request);
        }
        
        protected async UniTask<ResponseDto<T>> OrderedPost<T>(string path = null, object payload = null)
        {
            Request request = CreateRequest(UnityWebRequest.kHttpVerbPOST, path, payload);
            return await OrderedSend<T>(request);
        }

        protected async UniTask<ResponseDto<T>> OrderedGet<T>(string path = null, object payload = null)
        {
            Request request = CreateRequest(UnityWebRequest.kHttpVerbGET, path, payload);
            return await OrderedSend<T>(request);
        }
        
        protected async UniTask<ResponseDto<T>> OrderedPut<T>(string path = null, object payload = null)
        {
            Request request = CreateRequest(UnityWebRequest.kHttpVerbPUT, path, payload);
            return await OrderedSend<T>(request);
        }
        
        protected async UniTask<ResponseDto<T>> OrderedCreate<T>(string path = null, object payload = null)
        {
            Request request = CreateRequest(UnityWebRequest.kHttpVerbCREATE, path, payload);
            return await OrderedSend<T>(request);
        }
        
        protected async UniTask<ResponseDto<T>> OrderedDelete<T>(string path = null, object payload = null)
        {
            Request request = CreateRequest(UnityWebRequest.kHttpVerbDELETE, path, payload);
            return await OrderedSend<T>(request);
        }
       
        protected async UniTask<ResponseDto<T>> OrderedHead<T>(string path = null, object payload = null)
        {
            Request request = CreateRequest(UnityWebRequest.kHttpVerbHEAD, path, payload);
            return await OrderedSend<T>(request);
        }

        private Request CreateRequest(string method, string path, object payload)
        {
            return new Request(_createId++, method, path, payload);
        }
        
        private async UniTask<ResponseDto<T>> OrderedSend<T>(Request request)
        {
            _requestQueue.Enqueue(request);
            await UniTask.WaitUntil(() => _requestQueue.Peek().Id == request.Id);
            _requestQueue.Dequeue();
            return await Send<T>(request);
        }

        private async UniTask<ResponseDto<T>> Send<T>(Request request)
        {
            return await request.Send<T>(_baseUrl, _accessToken, _secretKey);
        }
    }
}
