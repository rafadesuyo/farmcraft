using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace DreamQuiz
{
    public class QuestionCreationRequestHandler : MonoBehaviour
    {
        private const string baseUrl = "https://dreamquiztest.ocarinastudio.com/api/player/question/review";
        private const string createQuestionSuffix = "/create";
        private const string getRandomQuestionSuffix = "/random";
        private const string reviewQuestionSuffix = "/send/review";
        private const string reportQuestionSuffix = "/reject/question";

        private const int connectionTimeout = 30;
        private QuestionCreationDTO questionCreationDTO = null;
        private string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOiIyODY1N2MyNC02ZDVlLTQ0MTctOGU4OC01NWY1N2Q0YTM5NmEiLCJqdGkiOiJmMGE1NjI4Zi02ZWU5LTRjOTItYTdlOC04MTk4Zjc3YTVlMzkiLCJyb2xlIjoiUGxheWVyVXNlciIsIm5iZiI6MTY5NTMxNzQ1NSwiZXhwIjoxNzI2OTM5ODU1LCJpYXQiOjE2OTUzMTc0NTV9.uju3BKyVRKRWFO7ukZCLwQk27wssCRM6aVi1AGYdNdE";
        //TO DO - Using a temporary token Bruno created for me on Swagger. Change to the correct token once it's implemented. https://ocarinastudios.atlassian.net/browse/DQG-2313?atlOrigin=eyJpIjoiNTQ5OGYzY2Q1ZDM5NDhiZmJjZDI2YTIyY2VkODI0MjkiLCJwIjoiaiJ9

        public static event Action<bool> OnQuestionCreationDataSent;
        public static event Action<QuestionReviewDTO> OnRandomQuestionReceived;

        private void OnEnable()
        {
            ReviewAQuestionScreen.OnReadyToReviewQuestion += SendGetQuestionRequest;
            QuestionAnswersInputScreen.OnPressedCreateQuestionButton += SendCreatedQuestionRequest;
            ReviewAQuestionScreen.OnLikeOrDislikedQuestion += SendReviewQuestionRequest;
            RemoveReportScreen.OnQuestionReported += SendReportQuestionRequest;
        }

        private void OnDisable()
        {
            ReviewAQuestionScreen.OnReadyToReviewQuestion -= SendGetQuestionRequest;
            QuestionAnswersInputScreen.OnPressedCreateQuestionButton -= SendCreatedQuestionRequest;
            ReviewAQuestionScreen.OnLikeOrDislikedQuestion -= SendReviewQuestionRequest;
            RemoveReportScreen.OnQuestionReported -= SendReportQuestionRequest;
        }

        #region CREATE QUESTION
        private UnityWebRequest POSTWebRequest()
        {
            string questionCreationDTOJson = JsonConvert.SerializeObject(questionCreationDTO);
            byte[] body = new System.Text.UTF8Encoding().GetBytes($"{questionCreationDTOJson}");

            UnityWebRequest request = UnityWebRequest.Post(baseUrl + createQuestionSuffix, questionCreationDTOJson);
            request.timeout = connectionTimeout;
            request.uploadHandler = new UploadHandlerRaw(body);
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {token}"); //TO DO - Using a temporary token Bruno created for me on Swagger. Change to the correct token once it's implemented. https://ocarinastudios.atlassian.net/browse/DQG-2313?atlOrigin=eyJpIjoiNTQ5OGYzY2Q1ZDM5NDhiZmJjZDI2YTIyY2VkODI0MjkiLCJwIjoiaiJ9

            return request;
        }

        public void SendCreatedQuestionRequest(QuestionCreationDTO dto) //POST
        {
            questionCreationDTO = dto;
            StartCoroutine(SendCreatedQuestionRequest_Coroutine());
        }

        IEnumerator SendCreatedQuestionRequest_Coroutine()
        {
            UnityWebRequest request = POSTWebRequest();

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                OnQuestionCreationDataSent?.Invoke(false);
                Debug.LogWarning("QUEST CREATION REQUEST ERROR! " + request.error + "/// respondeCode: " + request.responseCode);
            }
            else
            {
                OnQuestionCreationDataSent?.Invoke(true);
                Debug.LogWarning("Question created successfully!" + request.downloadHandler.text);
            }
        }
        #endregion

        #region GET QUESTION
        private UnityWebRequest GETWebRequest()
        {
            UnityWebRequest request = UnityWebRequest.Get(baseUrl + getRandomQuestionSuffix);
            request.timeout = connectionTimeout;
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {token}"); //TO DO - Using a temporary token Bruno created for me on Swagger. Change to the correct token once it's implemented. https://ocarinastudios.atlassian.net/browse/DQG-2313?atlOrigin=eyJpIjoiNTQ5OGYzY2Q1ZDM5NDhiZmJjZDI2YTIyY2VkODI0MjkiLCJwIjoiaiJ9

            return request;
        }

        private void SendGetQuestionRequest()
        {
            StartCoroutine(SendGetQuestionRequestCoroutine());
        }

        IEnumerator SendGetQuestionRequestCoroutine()
        {
            UnityWebRequest request = GETWebRequest();

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"ERROR! RespondeCode: {request.responseCode} ");
            }
            else
            {
                try
                {
                    string questionDTOJson = request.downloadHandler.text;
                    QuestionReviewDTO responseDTO = JsonConvert.DeserializeObject<QuestionReviewDTO>(questionDTOJson);


                    if (responseDTO != null)
                    {
                        Debug.LogWarning("Random question received!");
                        OnRandomQuestionReceived?.Invoke(responseDTO);
                    }
                    else
                    {
                        Debug.LogWarning("Failed to deserialize JSON response.");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"Error deserializing JSON: {e.Message}");
                }
            }
        }
        #endregion

        #region REVIEW QUESTION
        public UnityWebRequest ReviewWebRequest(string questionReviewID, int result)
        {
            string url = $"{baseUrl}{reviewQuestionSuffix}?playerQuestionReviewID={questionReviewID}&reviewValue={result}";
            UnityWebRequest request = UnityWebRequest.Put(url,"");
            request.timeout = connectionTimeout;
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {token}"); //TO DO - Using a temporary token Bruno created for me on Swagger. Change to the correct token once it's implemented. https://ocarinastudios.atlassian.net/browse/DQG-2313?atlOrigin=eyJpIjoiNTQ5OGYzY2Q1ZDM5NDhiZmJjZDI2YTIyY2VkODI0MjkiLCJwIjoiaiJ9

            return request;
        }

        private void SendReviewQuestionRequest(string questionReviewID, int result)
        {
            StartCoroutine(SendReviewQuestionRequestCoroutine(questionReviewID, result));
        }

        private IEnumerator SendReviewQuestionRequestCoroutine(string questionReviewID, int result)
        {
            UnityWebRequest request = ReviewWebRequest(questionReviewID, result);

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogWarning("ERROR TRYING TO REVIEW QUESTION! " + request.error + "/// respondeCode: " + request.responseCode);
            }

            else
            {
                Debug.LogWarning("Question REVIEWED successfully!" + request.downloadHandler.text);
            }
        }

        #endregion

        #region REPORT QUESTION
        public UnityWebRequest ReportWebRequest(string questionReviewID, List<int> reasonsList, string customComment)
        {
            string url = $"{baseUrl}{reportQuestionSuffix}?playerQuestionReviewID={questionReviewID}&customComment={customComment}";
            Debug.LogWarning("URL => " + url);
            string reasonsListJSON = JsonConvert.SerializeObject(reasonsList);
            byte[] body = new System.Text.UTF8Encoding().GetBytes($"{reasonsListJSON}");
            UnityWebRequest request = UnityWebRequest.Put(url, reasonsListJSON);
            request.timeout = connectionTimeout;
            request.uploadHandler = new UploadHandlerRaw(body);
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {token}"); //TO DO - Using a temporary token Bruno created for me on Swagger. Change to the correct token once it's implemented. https://ocarinastudios.atlassian.net/browse/DQG-2313?atlOrigin=eyJpIjoiNTQ5OGYzY2Q1ZDM5NDhiZmJjZDI2YTIyY2VkODI0MjkiLCJwIjoiaiJ9

            return request;
        }

        private void SendReportQuestionRequest(string questionReviewID, List<int> reasonsList, string customComment)
        {
            StartCoroutine(SendReportQuestionRequestCoroutine(questionReviewID, reasonsList, customComment));
        }

        IEnumerator SendReportQuestionRequestCoroutine(string questionReviewID, List<int> reasonsList, string customComment)
        {
            UnityWebRequest request = ReportWebRequest(questionReviewID, reasonsList, customComment);

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogWarning("ERROR TRYING TO REPORT A QUESTION! " + request.error + "/// respondeCode: " + request.responseCode);
            }
            else
            {
                Debug.LogWarning("Question REPORTED successfully!" + request.downloadHandler.text);
            }
        }
        #endregion
    }
}
