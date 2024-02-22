using System;
using System.IO;
using System.Net.Sockets;
using UnityEngine;
using static Unity.Networking.Transport.Utilities.ReliableUtility;

public class TCPClient : MonoBehaviour
{

    [SerializeField] private int port;

    private TcpClient client;
    private NetworkStream stream;

    public void Connect()
    {
        client = new TcpClient()
        {
            ReceiveBufferSize = TCPServer.dataBufferSize,
            SendBufferSize = TCPServer.dataBufferSize,
        };


        client.BeginConnect("127.0.0.1", port, ConnectCallback, client);
    }

    private void ConnectCallback(IAsyncResult _result)
    {
        client.EndConnect(_result);
        stream = client.GetStream();

        using (Packet packet = new Packet())
        {
            packet.InsertInt(1);
            packet.Write("Hello server!");
            packet.WriteLength();

            SendData(packet);
        }

        if (!client.Connected)
        {
            return;
        }
   
    }

    private void SendData(Packet packet)
    {
        try
        {
            if (client != null)
            {
                stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
            }
        }
        catch (Exception _ex)
        {
            Debug.Log($"Error sending data to server via TCP: {_ex}");
        }
    }

}
