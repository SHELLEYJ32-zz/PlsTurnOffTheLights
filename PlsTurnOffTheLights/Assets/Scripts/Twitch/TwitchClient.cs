using TwitchLib.Unity;          //import libraries from TwitchLib so we can talk to the twitch API
using TwitchLib.Client.Models;
using UnityEngine;

public class TwitchClient : MonoBehaviour
{

    public Client client;
    public string channelName = "shelleyj16";
    private float helpTimer;
    private bool helpFlag;
    private MonsterTwitchListener twitchListener;

    void Start()
    {
        Application.runInBackground = true;

        //change
        //channelName = Global.Instance.twitchName; //get the name of the twitch channel as set in the options menu

        ConnectionCredentials credentials = new ConnectionCredentials("gatekeeperlightsbot", Secrets.accessToken); //set up the twitch bots credentials
        client = new Client();
        client.Initialize(credentials, channelName); //Initialize the Bot

        client.OnMessageReceived += CommandListen; //Set up Trigger to fire whenever a message is sent in twitch chat

        client.Connect(); //connect the bot to the twitch chat

        twitchListener = GetComponent<MonsterTwitchListener>();

        helpTimer = 31.0f;
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
        if (e.ChatMessage.Message == "!help" && !helpFlag) //sends help message
        {
            Help();
            helpFlag = true;
        }
        else if (e.ChatMessage.Message == "!u" || e.ChatMessage.Message == "!d" || e.ChatMessage.Message == "!l" || e.ChatMessage.Message == "!r")
        {
            twitchListener.CollectVotes(e.ChatMessage.Message);
        }
    }

    //the help message
    private void Help()
    {
        client.SendMessage(client.JoinedChannels[0], "How to play with the streamer? Control monsters by typing in !u, !d, !l, !r." +
        "This message will not prompt again within 30 seconds.");
    }
}
