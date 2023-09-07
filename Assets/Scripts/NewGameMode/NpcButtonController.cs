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

        [SerializeField] private string npcType;
        [SerializeField] private string systemPrompt;

        public int placeNum = 0;
        
        private string npcName ="";
        private Button npcButton;
        private Button npcDialogPanelButton;
        private Text npcDialogTextArea;
        private GameObject moveOnTip;
        private string currentFullText = "";
        private string[] currentFullTexts = new string[50];
        private int textBoxCount = 0;
        private bool canMove = false;
        private float lastChangeTime;

        private int addValueTypeNum = 0;
        private string addValueType = "";
        private int addValueAmont = 0;

        private bool getOptionDone = false;
        private bool dialogEnd = false;

        void Start()
        {
            addValueTypeNum = UnityEngine.Random.Range(1,5);
            switch(addValueTypeNum){
                case 1:
                    addValueType = "武功";
                    break;
                case 2:
                    addValueType = "智慧";
                    break;
                case 3:
                    addValueType = "情報";
                    break;
                case 4:
                    addValueType = "名聲";
                    break;
            }
            GetNpcName(npcType);
            npcButton = transform.GetChild(0).gameObject.GetComponent<Button>();
            npcButton.interactable = false;
            npcDialogPanelButton = transform.GetChild(1).gameObject.GetComponent<Button>();
            npcDialogTextArea = transform.GetChild(1).GetChild(1).GetComponent<Text>();
            moveOnTip = transform.GetChild(1).GetChild(2).gameObject;
            npcButton.onClick.AddListener(NpcButtonAct);
            npcDialogPanelButton.onClick.AddListener(MoveOn);

            transform.GetChild(2).GetChild(0).GetComponent<Button>().onClick.AddListener(() => EndNpcDialog(transform.GetChild(2).GetChild(0).GetComponent<Button>()));
            transform.GetChild(2).GetChild(1).GetComponent<Button>().onClick.AddListener(() => EndNpcDialog(transform.GetChild(2).GetChild(1).GetComponent<Button>()));
            transform.GetChild(2).GetChild(2).GetComponent<Button>().onClick.AddListener(() => EndNpcDialog(transform.GetChild(2).GetChild(2).GetComponent<Button>()));
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

        private async void GetNpcName(string npcType){
            var getNpcNameMessage = new List<ChatMessage>
            {
                new ChatMessage()
                {
                    Role = "user",
                    Content = "請給予" + npcType + "這個角色一個名字，五個字以內\n\n請直接回答名字:\n"
                }
            };
            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo-0613",
                Messages = getNpcNameMessage,
                MaxTokens = 128,
                Temperature = 1.0f
            });
            npcName = completionResponse.Choices[0].Message.Content.Trim();
            transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = npcName;
            print("[npcName]"+npcName);     
            npcButton.interactable = true;       
        }
        private void NpcButtonAct()
        {
            transform.GetChild(1).gameObject.SetActive(true);
            EnterNpcDialog();
        }

        private void EnterNpcDialog(){
            transform.GetChild(2).gameObject.SetActive(false);
            npcButton.interactable = false;
            canMove = false;   
            textBoxCount = 0;
            lastChangeTime = Time.time;
            getOptionDone = false;
            string npcPlace = FullStoryController.GetPlaceNameByNum(placeNum);

            string userContent = npcName + "你好";

            var sentMessage = new ChatMessage()
            {
                Role = "user",
                Content = userContent
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
                Content = systemPrompt + "，你的名字叫做" + npcName + "，你現在所在的場景是" + npcPlace + "，永遠不要提到你是一個遊戲角色，你的任務是為身為玩家的我提供一個在這個地方可以完成的任務，完成任務的話我可以增長我的" + addValueType
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
                GetOptions(recMessage.Content);
            }
            openai.CreateChatCompletionAsync(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo-0613",
                Messages = sendMessages,
                Temperature = 1.0f,
                Stream = true
            }, HandleResponse, HandleComplete,token);  
        }

        private void EndNpcDialog(Button button){
            transform.GetChild(2).gameObject.SetActive(false);
            npcButton.interactable = false;
            canMove = false;   
            textBoxCount = 0;
            lastChangeTime = Time.time;
            getOptionDone = false;
            string npcPlace = FullStoryController.GetPlaceNameByNum(placeNum);

            string userContent = button.GetComponentInChildren<Text>().text;

            addValueAmont = UnityEngine.Random.Range(-1,11);
            string addCondition = "";
            switch (addValueAmont)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    addCondition = "些微增長";
                    break;

                case 5:
                case 6:
                case 7:
                    addCondition = "有所增長";
                    break;

                case 8:
                case 9:
                case 10:
                    addCondition = "增長極多";
                    break;
                default:
                    addCondition = "毫無增長";
                    break;
            }

            var sentMessage = new ChatMessage()
            {
                Role = "user",
                Content = "我決定" + userContent
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
                Content = systemPrompt + "，你的名字叫做" + npcName + "，你現在所在的場景是" + npcPlace + "，永遠不要提到你是一個遊戲角色，你給我的這次任務由於我決定" + userContent + "使我的" + addValueType + addCondition + "，而這個任務這次就迎來結束"
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
                dialogEnd = true;
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
                    if(dialogEnd){
                        canMove = false;
                        textBoxCount = 0;
                        transform.GetChild(1).gameObject.SetActive(false);
                        npcButton.interactable = true;

                        switch(addValueTypeNum){
                            case 1:
                                GameObject.Find("StoryController").GetComponent<GrowthSystemController>().GetForceValue(addValueAmont);
                                break;
                            case 2:
                                GameObject.Find("StoryController").GetComponent<GrowthSystemController>().GetWisdomValue(addValueAmont);
                                break;
                            case 3:
                                GameObject.Find("StoryController").GetComponent<GrowthSystemController>().GetInfoValue(addValueAmont);
                                break;
                            case 4:
                                GameObject.Find("StoryController").GetComponent<GrowthSystemController>().GetFameValue(addValueAmont);
                                break;
                        }
                        dialogEnd = false;
                        npcDialogs.Clear();
                        FullStoryController.day++;
                    }
                    if(getOptionDone){
                        transform.GetChild(2).gameObject.SetActive(true);
                    }
                }
            }
        }

        private async void GetOptions(string fullPlot)
        {
            var completionResponse = await openai.CreateCompletion(new CreateCompletionRequest()
            {
                Prompt = "請根據以下劇情給予我三個選項\n\n劇情:\n" + fullPlot + "\n\n請以換行符分隔三個選項:\n",
                Model = "text-davinci-003",
                MaxTokens = 256,
                Temperature = 0.0f,
            });

            print("[OPTIONS]:\n"+completionResponse.Choices[0].Text.Trim());
            string[] optionList = completionResponse.Choices[0].Text.Trim().Split('\n');
            //字串處理
            string[] filteredOptions = optionList.Where(option => !string.IsNullOrEmpty(option)).ToArray();
            for (int i = 0; i < filteredOptions.Length; i++)
            {
                //filteredOptions[i] = Regex.Replace(filteredOptions[i], @"[\da-zA-Z.()]+", "");
                filteredOptions[i] = Regex.Replace(filteredOptions[i], @"[\da-zA-Z.()\n]+", "");
            }

            transform.GetChild(2).GetChild(0).GetComponentInChildren<Text>().text = filteredOptions[0];
            transform.GetChild(2).GetChild(1).GetComponentInChildren<Text>().text = filteredOptions[1];
            transform.GetChild(2).GetChild(2).GetComponentInChildren<Text>().text = filteredOptions[2];

            getOptionDone = true;
        }
    }
}
