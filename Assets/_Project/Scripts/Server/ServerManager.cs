using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class ServerManager : LocalSingleton<ServerManager>
{
    private int requestsInProgress = 0;
    private const string GET = "GET";
    private const string PUT = "PUT";
    private const string POST = "POST";

#if UNITY_EDITOR
    [SerializeField] private bool SHOW_DEBUGS = false;
#endif

    public void SendGetRequest(string url, Action<string> onSuccessCallback = null, Action onErrorCallback = null, bool showRequestBlocker = false)
    {
        UnityWebRequest request = CreateUnityWebRequest(url, GET);

        StartCoroutine(SendRequest(request, onSuccessCallback, onErrorCallback, showRequestBlocker));
    }

    public void SendPutRequest(string url, Action<string> onSuccessCallback = null, Action onErrorCallback = null, bool showRequestBlocker = false)
    {
        UnityWebRequest request = CreateUnityWebRequest(url, PUT);

        StartCoroutine(SendRequest(request, onSuccessCallback, onErrorCallback, showRequestBlocker));
    }

    public void SendPostRequest(string url, string bodyData = null, Action<string> onSuccessCallback = null, Action onErrorCallback = null, bool showRequestBlocker = false)
    {
        UnityWebRequest request;

        if (string.IsNullOrEmpty(bodyData))
        {
            request = CreateUnityWebRequest(url, POST);
        }
        else
        {
            request = CreateUnityWebRequest(url, POST);
            request.SetRequestHeader("Content-Type", "application/json");
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes($"{bodyData}");
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        }

        StartCoroutine(SendRequest(request, onSuccessCallback, onErrorCallback, showRequestBlocker));
    }

    private UnityWebRequest CreateUnityWebRequest(string url, string method, string postForm = "")
    {
        UnityWebRequest request = null;

        switch (method)
        {
            case UnityWebRequest.kHttpVerbGET:
                {
                    request = UnityWebRequest.Get(url);
                    break;
                }
            case UnityWebRequest.kHttpVerbPUT:
                {
                    request = UnityWebRequest.Put(url, "");
                    break;
                }
            case UnityWebRequest.kHttpVerbPOST:
                {
                    request = UnityWebRequest.Post(url, postForm);
                    break;
                }
            default:
                break;
        }

        return request;
    }

    private IEnumerator SendRequest(UnityWebRequest request, Action<string> onSuccessCallback, Action onErrorCallback, bool showRequestBlocker = false)
    {
        if (requestsInProgress == 0 && showRequestBlocker)
        {
            EventsManager.Publish(EventsManager.onStartRequest);
        }

        requestsInProgress++;

#if UNITY_EDITOR
        if (SHOW_DEBUGS)
        {
            Debug.Log($"From Unity - {request.method}\nEndpoint: {request.url}");
        }
#endif

        UnityWebRequestAsyncOperation requestAsyncOps = request.SendWebRequest();

        while (!requestAsyncOps.isDone)
        {
            yield return null;
        }

        OnGetRequestResponse(request, onSuccessCallback, onErrorCallback);

        // request.result
        // UnityWebRequest.Result.ConnectionError: //Server ERROR
        // UnityWebRequest.Result.DataProcessingError: //Response ERROR
        // UnityWebRequest.Result.ProtocolError: //HTTP ERROR
    }

    private void OnGetRequestResponse(UnityWebRequest request, Action<string> onSuccessCallback = null, Action onErrorCallback = null)
    {
        requestsInProgress--;

        if (requestsInProgress == 0)
        {
            EventsManager.Publish(EventsManager.onEndAllRequests);
        }

        if (request.result == UnityWebRequest.Result.Success)
        {
            OnRequestSuccess(request, onSuccessCallback);
            return;
        }

        OnRequestError(request, onErrorCallback);
    }

    private void OnRequestSuccess(UnityWebRequest request, Action<string> onSuccessCallback = null)
    {
#if UNITY_EDITOR
        if (SHOW_DEBUGS)
        {
            Debug.Log($"From Server - {request.method}\nEndpoint: {request.url}\nResponse: {request.downloadHandler.text}");
        }
#endif
        onSuccessCallback?.Invoke(request.downloadHandler.text);
    }

    private void OnRequestError(UnityWebRequest request, Action onErrorCallback = null)
    {
#if UNITY_EDITOR
        if (SHOW_DEBUGS)
        {
            Debug.Log($"From Server - {request.method}\nEndpoint: {request.url}\nError: {request.error}");
        }
#endif
        onErrorCallback?.Invoke();
    }
}
