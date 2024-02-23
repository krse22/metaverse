using System;

public class DataHandler
{
    public static bool HandleData(byte[] data, Packet receivedData, Action<Packet> onPacketHandled)
    {
        int packetLength = 0;

        receivedData.SetBytes(data);

        if (receivedData.UnreadLength() >= 4)
        {
            packetLength = receivedData.ReadInt();
            if (packetLength <= 0)
            {
                return true;
            }
        }

        while (packetLength > 0 && packetLength <= receivedData.UnreadLength())
        {
            byte[] packetBytes = receivedData.ReadBytes(packetLength);
            UnityThread.ExecuteInUnityThread(() =>
            {
                using (Packet packet = new Packet(packetBytes))
                {
                    onPacketHandled(packet);
                }
            });

            packetLength = 0;
            if (receivedData.UnreadLength() >= 4)
            {
                packetLength = receivedData.ReadInt();
                if (packetLength <= 0)
                {
                    return true;
                }
            }
        }

        if (packetLength <= 1)
        {
            return true;
        }

        return false;
    }
}
