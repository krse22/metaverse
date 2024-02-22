using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEditor.PackageManager;
using UnityEngine;

public class TCPServer : MonoBehaviour
{
    public static int dataBufferSize = 1024;

    [SerializeField] private int port;
    private static TcpListener listener;
    private Thread listenerThread;

    public delegate void PacketHandler(ulong _fromClient, Packet _packet);
    public static Dictionary<int, PacketHandler> packetHandlers;

    private static Dictionary<ulong, Client> clients;

    // For local testing only
    private static ulong idIncrementor = 0;

    private void Start()
    {

        UnityThread.Init();

        listenerThread = new Thread(StartServer);
        listenerThread.Start();
    }

    void StartServer()
    {
        InitializeServerData();
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

    public static void WelcomeReceived(ulong _fromClient, Packet _packet)
    {
        string username = _packet.ReadString();
        Debug.Log(username);

    }

    private static void InitializeServerData()
    {
        packetHandlers = new Dictionary<int, PacketHandler>()
            {
                { (int)ClientPackets.welcomeReceived, WelcomeReceived },
            };
  
    }

    public void Broadcast()
    {
        foreach(ulong key in clients.Keys)
        {
            Packet packet = new Packet();
            packet.InsertInt(1);
            packet.Write("Hello from server ;))))");
            clients[key].SendData(packet);
        }
    }

    private void OnApplicationQuit()
    {
        listener.Stop();
        listenerThread.Join();
    }


}
