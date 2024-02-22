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
        byte requestid = packet.ReadByte();
        byte type = packet.ReadByte();
        string route = packet.ReadString();
        string jsonData = packet.ReadString();
        Debug.Log($"Received type {type} from {from} on route {route} text: {jsonData}");

        Packet returnPacket = new Packet();
        returnPacket.Write((byte)ServerPacketType.Response);
        returnPacket.Write(requestid);
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
        listener.Stop();
        listenerThread.Join();
    }


}
