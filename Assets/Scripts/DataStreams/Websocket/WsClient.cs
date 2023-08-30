using System;
using UnityEngine;
using UnityEngine.Events;
using WebSocketSharp;

public class WsClient : MonoBehaviour
{
    #region SingletonPattern
    private static WsClient _instance;
    public static WsClient Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("The WsClient is Null");
            }
            return _instance;
        }
    }
    private void Awake()
    {
        _instance = this;
    }
    #endregion

    public WebSocket ws;
    public bool hasWsConnection = false;
    private float timeoutLength = 2.0f;
    private float timeoutTimer = 0.0f;
    [SerializeField] private string ip = "localhost";
    [SerializeField] private string port = "8080";


    [System.Serializable] public class WsEvent : UnityEvent<string> { }
    public UnityEvent wsMsgReceived;
    public class WSHelloworld
    {
        public string connectMessage;
        public string _time;
        public WSHelloworld(string connectmsg)
        {
            connectMessage = connectmsg;
            _time = System.DateTime.Now.ToString("f");
        }
        public string HelloMessage()
        {
            return JsonUtility.ToJson(this);
        }
    }
    private WSHelloworld wsHello;


    private void Start()
    {
        wsHello = new WSHelloworld("Hello from " + ip);
        //wsToDebug = this.GetComponent<Ws_to_debug>();
        ws = new WebSocket("ws://" + ip + ":" + port);
        ws.ConnectAsync();

        ws.OnOpen += (sender, e) =>
        {
            hasWsConnection = true;
            var wsmsg = wsHello.HelloMessage();
            SendWSMessage(wsmsg);
            wsMsgReceived?.Invoke();

        };

        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("Message Received from " + ((WebSocket)sender).Url + ", Data : " + e.Data);
            HandleIncomingMessage(e.Data);
        };
        ws.OnClose += (sender, e) =>
        {
            //Debug.Log("the connection did close");
            hasWsConnection = false;
        };
    }

    private void Update()
    {
        // we check for a websocket connection and otherwise try to reconnect every <timeoutLength> seconds.
        if (ws == null)
        {
            return;
        }
        if (hasWsConnection == false)
        {
            timeoutTimer += Time.deltaTime;
            if (timeoutTimer > timeoutLength)
            {
                ws.ConnectAsync();
                timeoutTimer = 0;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ws.Send("test");
            }
        }
    }

    private void HandleIncomingMessage(string message)
    {
        //Debug.Log("INVOKING " + message);
        message = message.Replace("\'", "\""); // python websocket uses single quotes?
        WebsocketMessage res = JsonUtility.FromJson<WebsocketMessage>(message);
        switch (res.type)
        {
            case "player":
                break;
            default:
                Debug.LogWarning("unknown command");
                break;
        }
        Debug.Log(message);
        //wsMsgReceived.Invoke(message);
        // TODO
    }


    public void SendWSMessage(string message)
    {
        //Debug.Log("sending " + message);
        DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        int cur_time = (int)(DateTime.UtcNow - epochStart).TotalSeconds;
        if (hasWsConnection)
        {
            ws.Send("time: " + cur_time + ", " + message);
        }
    }
    public void SetDebug(string message)
    {
        Debug.Log("printing " + message);
        if (message is null)
        {
            throw new ArgumentNullException(nameof(message));
        }

    }

    private void OnApplicationQuit()
    {

        DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        int cur_time = (int)(DateTime.UtcNow - epochStart).TotalSeconds;
        SendWSMessage("time: " + cur_time + ", message: Android application is closing ");
        ws.CloseAsync(CloseStatusCode.Normal);
    }


}


[Serializable]
public class WebsocketMessage
{
    public string type;
    public string data;
    //public PlayerVals playerValues;
    //TODO
}