// this is a model that holds every data in the game

using System.Collections.Generic;
using OpenAI;

[System.Serializable]
public class GameData 
{
    public string Time;  // the time that the save button been pressed
    public string Story; // save gpt's response
    // public List<ChatMessage> ChatMessage;
    public string ChatMessage;
    // options
    public string LatestOption1;
    public string LatestOption2;
    public string LatestOption3;
    // image path
    public string BackgroundImg; 
    public string Location; // which button saves the screenshot 
}
