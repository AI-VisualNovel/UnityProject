using System.Collections;
using System;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.Text.RegularExpressions;


namespace OpenAI
{
    public class NpcButtonController : MonoBehaviour
    {
        private OpenAIApi openai = new OpenAIApi();
        private CancellationTokenSource token = new CancellationTokenSource();
        private List<ChatMessage> npcDialogs = new List<ChatMessage>();

        [SerializeField] private string npcName;
        [SerializeField] private string systemPrompt;
        
        private Button npcButton;
        private Button npcDialogPanelButton;
        private Text npcDialogTextArea;
        private GameObject moveOnTip;
        private string currentFullText = "";
        private string[] currentFullTexts = new string[50];
        private int textBoxCount = 0;
        private bool canMove = false;
        private float lastChangeTime;


        void Start()
        {
            npcButton = transform.GetChild(0).gameObject.GetComponent<Button>();
            npcDialogPanelButton = transform.GetChild(1).gameObject.GetComponent<Button>();
            npcDialogTextArea = transform.GetChild(1).GetChild(1).GetComponent<Text>();
            moveOnTip = transform.GetChild(1).GetChild(2).gameObject;
            transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = npcName;
            npcButton.onClick.AddListener(NpcButtonAct);
            npcDialogPanelButton.onClick.AddListener(MoveOn);
        }

        // Update is called once per frame
        void Update()
        {
            currentFullTexts = currentFullText.Split("\n");
            if(textBoxCount >= 0 && textBoxCount < currentFullTexts.Length && currentFullTexts[textBoxCount] != null){
                //監測字串變化
                if(npcDialogTextArea.text != currentFullTexts[textBoxCount]){
                    npcDialogTextArea.text = currentFullTexts[textBoxCount];
                    lastChangeTime = Time.time;
                    canMove = false;
                }
                
                if(Time.time - lastChangeTime >= 1f && !canMove){
                    canMove = true;
                }
            }

            if(canMove){
                moveOnTip.SetActive(true);
            }else{
                moveOnTip.SetActive(false);
            }            
        }
        private void NpcButtonAct()
        {
            transform.GetChild(1).gameObject.SetActive(true);
            EnterNpcDialog();
        }

        private void EnterNpcDialog(){
            npcButton.interactable = false;
            canMove = false;   
            textBoxCount = 0;
            lastChangeTime = Time.time;

            var sentMessage = new ChatMessage()
            {
                Role = "user",
                Content = "哈囉!"
            };
            var recMessage = new ChatMessage()
            {
                Role = "assistant",
                Content = ""
            };
            npcDialogs.Add(sentMessage);

            List<ChatMessage> sendMessages = new List<ChatMessage>(npcDialogs);
            var systemMessage = new ChatMessage()
            {
                Role = "system",
                Content = systemPrompt
            };
            sendMessages.Add(systemMessage);
            foreach(ChatMessage m in sendMessages){
                print("[SEND]" + m.Role + ":" + m.Content);
            }            

            void HandleResponse(List<CreateChatCompletionResponse> responses){
                currentFullText = string.Join("", responses.Select(r => r.Choices[0].Delta.Content));
            }
            void HandleComplete(){
                recMessage.Content = currentFullText;
                npcDialogs.Add(recMessage);
            }
            openai.CreateChatCompletionAsync(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo-0613",
                Messages = sendMessages,
                Temperature = 0.5f,
                Stream = true
            }, HandleResponse, HandleComplete,token);  
        }

        private void MoveOn(){
            if(canMove){
                textBoxCount++;
                if(textBoxCount < currentFullTexts.Length && currentFullTexts[textBoxCount] == ""){
                    textBoxCount++;
                }
                if(textBoxCount != 0 && textBoxCount >= currentFullTexts.Length){
                    canMove = false;
                    textBoxCount = 0;
                    transform.GetChild(1).gameObject.SetActive(false);
                    npcButton.interactable = true;
                }
            }
        }
    }
}
