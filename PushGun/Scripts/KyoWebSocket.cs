using System;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


public class KyoWebSocket
{
    public delegate void OnMessageDelegate(string message);

    private ClientWebSocket client = null;
    private Thread receiveProcess;
    private Uri uri = null;

    public event OnMessageDelegate OnMessage;

    public KyoWebSocket(string uriString)
    {
        client = new ClientWebSocket();
        uri = new Uri(uriString);
    }

    public async Task ConnectAsync()
    {
        await client.ConnectAsync(uri, new CancellationToken());

        if (receiveProcess != null)
        {
            receiveProcess.Join();
            receiveProcess = null;
        }

        receiveProcess = new Thread(ProcessMessage);
        receiveProcess.Start();
    }

    // 메시지 프로세서
    private void ProcessMessage()
    {
        byte[] recvData = new byte[16384];
        while (client.State == WebSocketState.Open)
        {
            var result = client.ReceiveAsync(new Memory<byte>(recvData), new CancellationToken()).Result;
            string message = Encoding.UTF8.GetString(recvData, 0, result.Count);
            Task.Run(() => { OnMessage?.Invoke(message); });
        }
    }
}
