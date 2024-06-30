using dashboard_host;
using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace dashboard_host
{
    public class UdpListener
    {
        private UdpClient udpClient;
        private IPEndPoint remoteEndPoint;
        private const int port = 20777;

        public UdpListener()
        {
            udpClient = new UdpClient(port);
            remoteEndPoint = new IPEndPoint(IPAddress.Any, port);
        }

        public void StartReceiving()
        {
            while (true)
            {
                byte[] data = udpClient.Receive(ref remoteEndPoint);
                PacketHeader header = ByteArrayToStructure<PacketHeader>(data);

                switch (header.m_packetId)
                {
                    case 0:
                        Console.WriteLine("Motion packet");
                        break;
                    case 1:
                        Console.WriteLine("Session packet");
                        break;
                    case 2:
                        Console.WriteLine("Lap packet");
                        break;
                    case 3:
                        Console.WriteLine("Event packet");
                        break;
                    case 4:
                        Console.WriteLine("Participants packet");
                        break;
                    case 5:
                        Console.WriteLine("Car set ups packet");
                        break;
                    case 6:
                        Console.WriteLine("Car telemetry packet");
                        break;
                    case 7:
                        Console.WriteLine("Car status packet");
                        break;
                    case 8:
                        Console.WriteLine("Final classification packet");
                        break;
                    case 9:
                        Console.WriteLine("Lobby info packet");
                        break;
                    case 10:
                        Console.WriteLine("Car damage packet");
                        break;
                    case 11:
                        Console.WriteLine("Session history packet");
                        break;
                    case 12:
                        Console.WriteLine("Tire history packet");
                        break;
                    case 13:
                        Console.WriteLine("Motion ex packet");
                        break;
                    default:
                        break;
                }
            }
        }

        private T ByteArrayToStructure<T>(byte[] bytes) where T : struct
        {
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            try
            {
                return (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            }
            finally
            {
                handle.Free();
            }
        }
    }
}