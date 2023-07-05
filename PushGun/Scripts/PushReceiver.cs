using Newtonsoft.Json.Linq;

public class PushReceiver : Singleton<PushReceiver>
{
	public delegate void OnMessage(string iconBase64, string title, string message, string source_user_iden);


	private OnMessage onMessage = null;
	private KyoWebSocket webSocketForPush;

	private bool IsInitialized = false;
	private bool IsPaused = false;

	private string accessToken = string.Empty;

	public string AccessToken => accessToken;

	public PushReceiver()
	{
		IsInitialized = false;
		IsPaused = false;
	}


	public void Init(string accessToken, OnMessage onMessage)
	{
		this.onMessage = onMessage;
		this.accessToken = accessToken;

        if (webSocketForPush == null)
			webSocketForPush= new KyoWebSocket("wss://stream.pushbullet.com/websocket/" + accessToken);

		webSocketForPush.OnMessage += OnMessageReceived;
		var result = webSocketForPush.ConnectAsync();

		IsInitialized = true;
	}

	private void OnMessageReceived(string message)
	{
		if (IsPaused)
			return; // when user paused receiving

        if (message.Equals("{\"type\":\"nop\"}"))
			return; // when data is garbage data


        JObject jObject = JObject.Parse(message);
		JToken token = jObject["push"];

		string type = token["type"].ToString();

		if (type.Equals("dismissal"))
			return; // when the data is not mirroring notification

        string applicationName = string.Empty;
		string icon = string.Empty;
		string title = string.Empty;
		string body = string.Empty;
		string sourceUserIden = string.Empty;

        if (type.Equals("sms_changed"))    // When it's just Text Message
        {
            applicationName = "Message";
            JToken noti = token["notifications"][0];
            title = noti["title"].ToString();
            body = noti["body"].ToString();
			sourceUserIden = noti["source_user_iden"].ToString();
        }
		else
        {
            applicationName = token["application_name"].ToString();
			icon = token["icon"].ToString();
            title = token["title"].ToString();
            body = token["body"].ToString();
            sourceUserIden = token["source_user_iden"].ToString();
        }

        if (Utilizer.IsProgramShouldBeIgnored(applicationName))
            return; // when application is on ignoreList.

        onMessage?.Invoke(icon, $"[{applicationName}] {title}", body, sourceUserIden);
    }

	public void StartGetting()
	{
		if (!IsInitialized) return;

		IsPaused = false;
	}

	public void StopGetting()
	{
		if (!IsInitialized) return;

		IsPaused = true;
	}
}