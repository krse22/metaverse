using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;

public class TCPClient : MonoBehaviour
{

    [SerializeField] private int port;

    private TcpClient client;
    private NetworkStream stream;
    private Packet receivedData;
    private byte[] receiveBuffer;


    private Dictionary<int, TCPServer.PacketHandler> packetHandlers;

    public void Connect()
    {
        client = new TcpClient()
        {
            ReceiveBufferSize = TCPServer.dataBufferSize,
            SendBufferSize = TCPServer.dataBufferSize,
        };

        InitializeClientData();

        client.BeginConnect("127.0.0.1", port, ConnectCallback, client);
    }

    private void ConnectCallback(IAsyncResult _result)
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

        using (Packet packet = new Packet())
        {
            packet.Write("Hello server!");

            SendData(packet);
        }

        stream.BeginRead(receiveBuffer, 0, TCPServer.dataBufferSize, ReceiveCallback, null);
    }

    private void ReceiveCallback(IAsyncResult result)
    {
        try
        {

            Debug.Log("received from serverini");
            int byteLength = stream.EndRead(result);
            if (byteLength <= 0)
            {
                // instance.Disconnect();
                return;
            }

            byte[] data = new byte[byteLength];
            Array.Copy(receiveBuffer, data, byteLength);

            receivedData.Reset(HandleData(data));
            stream.BeginRead(receiveBuffer, 0, TCPServer.dataBufferSize, ReceiveCallback, null);
        }
        catch
        {
           // Disconnect();
        }
    }

    public void WelcomeReceived(ulong fromClient, Packet packet)
    {
        string text = packet.ReadString();
        Debug.Log(text);
    }

    private bool HandleData(byte[] _data)
    {
        int _packetLength = 0;

        receivedData.SetBytes(_data);

        if (receivedData.UnreadLength() >= 4)
        {
            _packetLength = receivedData.ReadInt();
            if (_packetLength <= 0)
            {
                return true;
            }
        }

        while (_packetLength > 0 && _packetLength <= receivedData.UnreadLength())
        {
            byte[] _packetBytes = receivedData.ReadBytes(_packetLength);
            UnityThread.ExecuteInUnityThread(() =>
            {
                using (Packet _packet = new Packet(_packetBytes))
                {
                    ReceivedFromServer(_packet);
                }

            });

            _packetLength = 0;
            if (receivedData.UnreadLength() >= 4)
            {
                _packetLength = receivedData.ReadInt();
                if (_packetLength <= 0)
                {
                    return true;
                }
            }
        }

        if (_packetLength <= 1)
        {
            return true;
        }

        return false;
    }

    private void ReceivedFromServer(Packet packet)
    {
        string text = packet.ReadString();
        Debug.Log("Received from server: " + text);
    }

    private void InitializeClientData()
    {
        packetHandlers = new Dictionary<int, TCPServer.PacketHandler>()
        {
            { (int)ServerPackets.welcome, WelcomeReceived },
        };
        Debug.Log("Initialized packets.");
    }


    private void SendData(Packet packet)
    {
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

}
