using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class TCPServer : MonoBehaviour
{
    public static int dataBufferSize = 1024;

    private static int port;
    private static TcpListener listener;
    private static Thread listenerThread;

    public delegate void PacketHandler(ulong _fromClient, Packet _packet);

    private static Dictionary<ulong, Client> clients;

    // For local testing only
    private static ulong idIncrementor = 0;

    public static void InitializeServer(int portNumber)
    {
        port = portNumber;

        UnityThread.Init();

        listenerThread = new Thread(StartServer);
        listenerThread.Start();
    }

    private static void StartServer()
    {
        clients = new Dictionary<ulong, Client>();

        listener = new TcpListener(IPAddress.Any, port);
        listener.Start();
        listener.BeginAcceptTcpClient(TCPConnectCallback, null);
    }

    private static void TCPConnectCallback(IAsyncResult result)
    {
        TcpClient tcpCLient = listener.EndAcceptTcpClient(result);
        listener.BeginAcceptTcpClient(TCPConnectCallback, null);

        Client client = new Client(tcpCLient, idIncrementor);
        clients[idIncrementor] = client;

        Debug.Log($"Incoming connection from {tcpCLient.Client.RemoteEndPoint}...");

    }

    public static void ReceivedFromClient(ulong from, Packet packet)
    {
        byte requestId = packet.ReadByte();
        byte type = packet.ReadByte();
        string route = packet.ReadString();
        string jsonData = packet.ReadString();

        Router.InvokeRoutes(requestId, type, route, jsonData);

        Debug.Log($"Received type {type} from {from} on route {route} text: {jsonData}");

        Packet returnPacket = new Packet();
        returnPacket.Write((byte)ServerPacketType.Response);
        returnPacket.Write(requestId);
        returnPacket.Write("Response for api/character");

        clients[from].SendData(returnPacket);

        returnPacket.Dispose();
    }

    public void Broadcast()
    {
        foreach(ulong key in clients.Keys)
        {
            Packet packet = new Packet();
            packet.Write("Hello from server ;))))");
            clients[key].SendData(packet);
        }
    }

    public static void DisconnectClient(ulong id)
    {
        clients.Remove(id);
    }

    private void OnApplicationQuit()
    {
        if (listener != null)
        { 
            listener.Stop();
            listenerThread.Join();
        }
    }


}
