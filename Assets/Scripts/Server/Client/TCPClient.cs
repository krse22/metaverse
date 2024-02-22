using System;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

enum ServerPacketType
{
    Response = 0,
    Message = 1,
}

public class TCPClient : MonoBehaviour
{
    [SerializeField] private int port;

    private static TCPClient instance;

    private void Awake()
    {
        instance = this;
    }

    private static TcpClient client;
    private static NetworkStream stream;
    private static Packet receivedData;
    private static byte[] receiveBuffer;
    private static byte requestId;

    private static Dictionary<byte, Response> responses;

    public void Connect()
    {
        ConnectToServer();
    }

    private static void ConnectToServer()
    {
        client = new TcpClient()
        {
            ReceiveBufferSize = TCPServer.dataBufferSize,
            SendBufferSize = TCPServer.dataBufferSize,
        };

        requestId = 0;
        responses = new Dictionary<byte, Response>();

        client.BeginConnect("127.0.0.1", instance.port, ConnectCallback, client);
    }

    private static void ConnectCallback(IAsyncResult _result)
    {
        client.EndConnect(_result);
        stream = client.GetStream();

        if (!client.Connected)
        {
            return;
        }

        stream = client.GetStream();
        receivedData = new Packet();
        receiveBuffer = new byte[TCPServer.dataBufferSize];

        stream.BeginRead(receiveBuffer, 0, TCPServer.dataBufferSize, ReceiveCallback, null);
    }

    private static void ReceiveCallback(IAsyncResult result)
    {
        try
        {
            Debug.Log("received from serverini");
            int byteLength = stream.EndRead(result);
            if (byteLength <= 0)
            {
                Disconnect();
                return;
            }

            byte[] data = new byte[byteLength];
            Array.Copy(receiveBuffer, data, byteLength);

            receivedData.Reset(DataHandler.HandleData(data, receivedData, ReceivedFromServer));
            stream.BeginRead(receiveBuffer, 0, TCPServer.dataBufferSize, ReceiveCallback, null);
        }
        catch
        {
            Disconnect();
        }
    }

    private static void ReceivedFromServer(Packet packet)
    {
        ServerPacketType type = (ServerPacketType)packet.ReadByte();
        byte requestId = packet.ReadByte();
        string response = packet.ReadString();

        responses[requestId].body = response;
        responses[requestId].onResponse(responses[requestId]);
    }

    public static void Post(string route, string jsonBody, Action<Response> onResponse)
    {
        SendData(RequestType.Post, route, jsonBody, onResponse);
    }

    public static void Get(string route, string jsonBody, Action<Response> onResponse)
    {
        SendData(RequestType.Get, route, jsonBody, onResponse);
    }

    public static void Put(string route, string jsonBody, Action<Response> onResponse)
    {
        SendData(RequestType.Put, route, jsonBody, onResponse);
    }

    public static void Delete(string route, string jsonBody, Action<Response> onResponse)
    {
        SendData(RequestType.Delete, route, jsonBody, onResponse);
    }

    public static void SendData(RequestType type, string route, string jsonBody, Action<Response> onResponse)
    {

        Packet packet = new Packet();
        packet.Write(requestId);
        packet.Write((byte)type);
        packet.Write(route);
        packet.Write(jsonBody);

        responses[requestId] = new Response()
        {
            ResponseId = requestId,
            onResponse = onResponse
        };

        requestId++;

        try
        {
            if (client != null)
            {
                packet.WriteLength();
                stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
            }
        }
        catch (Exception _ex)
        {
            Debug.Log($"Error sending data to server via TCP: {_ex}");
        }
    }

    private static void Disconnect()
    {
        client.Close();
    }

}
