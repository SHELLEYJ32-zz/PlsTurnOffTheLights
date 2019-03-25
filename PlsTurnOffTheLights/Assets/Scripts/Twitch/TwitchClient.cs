using System.Collections;
using System.Collections.Generic;
using TwitchLib.Unity;          //import libraries from TwitchLib so we can talk to the twitch API
using TwitchLib.Client.Models;
using UnityEngine;

public class TwitchClient : MonoBehaviour
{

    public Client client;
    public string channelName;
    public GameObject CatControllerObject;
    private float helpTimer;
    private bool helpFlag;

    private CatController catController;
    // Start is called before the first frame update
    void Start()
    {
        channelName = Global.Instance.twitchName; //get the name of the twitch channel as set in the options menu

        ConnectionCredentials credentials = new ConnectionCredentials("mrtwitchboto", Secrets.Instance.accessToken); //set up the twitch bots credentials
        client = new Client();
        client.Initialize(credentials, channelName); //Initialize the Bot

        client.OnMessageReceived += CommandListen; //Set up Trigger to fire whenever a message is sent in twitch chat

        client.Connect(); //connect the bot to the twitch chat

        catController = CatControllerObject.GetComponent<CatController>(); //set up the cat controller
        helpTimer = 40.0f; //set the delay between bot sending help messages to avoid getting banned
    }

    void Update()
    {
        if (helpFlag == true) //runs to see if the help flag has been flipped, and resets it when needed
        {
            helpTimer = helpTimer - Time.deltaTime;
            if (helpTimer <= 0.0f)
            {
                helpFlag = false;
                helpTimer = 40.0f;
            }
        }
    }

    //Checks to see if messages in twitch chat are commands, and if so, executes them
    private void CommandListen(object sender, TwitchLib.Client.Events.OnMessageReceivedArgs e)
    {
        if (e.ChatMessage.Message == "!help" && helpFlag == false) //sends help message
        {
            Help();
            helpFlag = true;
        }
        else if (e.ChatMessage.Message == "!Up" || e.ChatMessage.Message == "!up" || e.ChatMessage.Message == "!Down" || e.ChatMessage.Message == "!down"
            || e.ChatMessage.Message == "!Left" || e.ChatMessage.Message == "!left" || e.ChatMessage.Message == "!Right" || e.ChatMessage.Message == "!right"
            || e.ChatMessage.Message == "!u" || e.ChatMessage.Message == "!d" || e.ChatMessage.Message == "!l" || e.ChatMessage.Message == "!r")
        {
            catController.ChatMoveCommand(e.ChatMessage.Message, e.ChatMessage.Username);
        }
        /*else
        {
            client.SendMessage(client.JoinedChannels[0],"Command not recognized, please use !Help to get the list of commands.");
        }*/
    }

    /*private void ChatListen(object sender, TwitchLib.Client.Events.OnMessageReceivedArgs e)
    {
        Debug.Log("Someone just sent a message in Twitch Chat");
        Debug.Log(e.ChatMessage.Username + ": " + e.ChatMessage.Message);
    }*/

    //the help message
    private void Help()
    {
        client.SendMessage(client.JoinedChannels[0], "Welcome to Herding Chats! You can use chat commands to help the cats avoid the player!" +
            " The valid commands are: !Up, !Down, !Left, !Right. Alternatively, you can use !u, !d, !l, !r");
    }
}
