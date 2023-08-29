// using System;
// using UnityEngine;
// using System.Linq;
// using UnityEngine.UI;
// using System.Collections.Generic;
// using System.Threading;
// using UnityEngine.Networking;
// using System.Threading.Tasks;


// namespace OpenAI
// {
//     public class ChatMassageLoading : MonoBehaviour
//     {

//         [SerializeField] private RectTransform sent;
//         [SerializeField] public RectTransform received; // Lai
//         private OpenAIApi openai = new OpenAIApi("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");
//         private CancellationTokenSource token = new CancellationTokenSource();
//         [SerializeField] private ScrollRect scroll;


//         private SemaphoreSlim semaphore;
//         private float heightSpeed = 0;
//         private float height = 0;



//         public async void LoadingFormerProgress(){
//             string readFromFilePath = Application.streamingAssetsPath + "/Json/08-25-23-21-59-19.json";
//             List<ChatMessage> chat_massage = OpenAI.SaveLoad.GetChatMassage(readFromFilePath);
//             // foreach (ChatMessage m in chat_massage){
//                 try{
//                     // foreach (ChatMessage m in chat_massage){

//                         // var sentMessage = new ChatMessage()
//                         // {
//                         //     Role = m.Role,
//                         //     Content = m.Content
//                         // };
//                         var recItem = AppendMessage(chat_massage);

                       
//                         // Complete the prompt
//                         heightSpeed = 0;
//                         semaphore = new SemaphoreSlim(0);
//                         openai.CreateChatCompletionAsync(new CreateChatCompletionRequest()
//                         {
//                             Model = "gpt-3.5-turbo-0613",
//                             Messages = chat_massage,
//                             Stream = true
//                         },(responses) => HandleResponse(responses,chat_massage,recItem),HandleComplete,token);
//                         await semaphore.WaitAsync();

//                         // scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
//                         // recItem.anchoredPosition = new Vector2(0, -height);
//                         // LayoutRebuilder.ForceRebuildLayoutImmediate(recItem);
//                         // height += recItem.sizeDelta.y;
//                         // scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
//                         // scroll.verticalNormalizedPosition = 0;

//                         //省點錢         
//                         // ***************************************************************************************8
//                         // recMessage.Content = recItem.GetChild(0).GetChild(0).GetComponent<Text>().text;
//                         // currentFullText = recMessage.Content;  
//                         // messages.Add(recMessage);   
//                         // SaveLoad.CreateTextFile(currentFullText);
                
//                         // SaveLoad.SaveChatMassage(messages); // testing
//                         // SaveLoad.SaveStoryToList(currentFullText);


//                         //GetOptions();
//                         //SendImageRequest();

//                         //button.enabled = true;
//                         //inputField.enabled = true;
//                         // optionChoicing.SetActive(true);
//                     // };
//                 }
//                 catch(Exception ex)
//                 {
//                     Debug.LogError("An error occurred: " + ex.Message);
//                     // WrongApiPanel.SetActive(true);
//                 }
//         }

//         private void HandleComplete(){
//             semaphore.Release();
//         }

//         private void HandleResponse(List<CreateChatCompletionResponse> responses, ChatMessage message,RectTransform item)
//         {
//                 scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);

//                 message.Content = string.Join("", responses.Select(r => r.Choices[0].Delta.Content));
//                 item.GetChild(0).GetChild(0).GetComponent<Text>().text = message.Content;

//                 item.anchoredPosition = new Vector2(0, -height);
//                 LayoutRebuilder.ForceRebuildLayoutImmediate(item);
//                 // height += item.sizeDelta.y;
//                 scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height+heightSpeed);
//                 scroll.verticalNormalizedPosition = 0;

//                 heightSpeed += 0.45f;
//         }

//          private RectTransform AppendMessage(List<ChatMessage> message)
//         {
            
//             foreach(ChatMessage m in message){
//                 scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);

//                 var item = Instantiate(m.Role == "user" ? sent : received, scroll.content);
//                 item.GetChild(0).GetChild(0).GetComponent<Text>().text = m.Content;
//                 item.anchoredPosition = new Vector2(0, -height);
//                 LayoutRebuilder.ForceRebuildLayoutImmediate(item);
//                 height += item.sizeDelta.y;
//                 scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
//                 scroll.verticalNormalizedPosition = 0;

//                 return item;

//             }
            
//         }
//     }
// }