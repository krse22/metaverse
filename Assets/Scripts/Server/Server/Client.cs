using System;
using System.Net.Sockets;
using UnityEngine;

public class Client 
{
    private ulong id; // Matches OwnerClientId 
    private TcpClient client;
    private NetworkStream stream;
    private byte[] receiveBuffer;
    private Packet receivedData;

    public Client(TcpClient client, ulong id)
    {
        this.client = client;
        this.id = id;

        receivedData = new Packet();
        stream = client.GetStream();
        receiveBuffer = new byte[TCPServer.dataBufferSize];
        stream.BeginRead(receiveBuffer, 0, receiveBuffer.Length, ReceiveCallback, null);
    }

    private void ReceiveCallback(IAsyncResult result)
    {
        try
        {

            int byteLength = stream.EndRead(result);
            if (byteLength <= 0)
            {
                // Server.clients[id].Disconnect();
                return;
            }

            byte[] data = new byte[byteLength];
            Array.Copy(receiveBuffer, data, byteLength);

            receivedData.Reset(HandleData(data));
            stream.BeginRead(receiveBuffer, 0, TCPServer.dataBufferSize, ReceiveCallback, null);
        }
        catch (Exception _ex)
        {
            Debug.Log($"Error receiving TCP data: {_ex}");
            // Server.clients[id].Disconnect();
        }
    }

    public void SendData(Packet _packet)
    {
        try
        {
            if (client != null)
            {
                _packet.WriteLength();
                stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
            }
        }
        catch (Exception _ex)
        {
            Debug.Log($"Error sending data to player {id} via TCP: {_ex}");
        }
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
                    TCPServer.ReceivedFromClient(id, _packet);
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


}
