using dashboard_host;
using System;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Runtime.InteropServices;

namespace dashboard_host
{
    public class UdpListener
    {
        private UdpClient udpClient;
        private IPEndPoint remoteEndPoint;
        private const int port = 20777;
        private GameData gameData = new GameData();
        private WebSocketServer webSocketServer;

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
                        PacketMotionData motionData = ByteArrayToStructure<PacketMotionData>(data);
                        break;
                    case 1:
                        PacketSessionData sessionData = ByteArrayToStructure<PacketSessionData>(data);
                        HandleSessionData(sessionData);
                        break;
                    case 2:
                        PacketLapData lapData = ByteArrayToStructure<PacketLapData>(data);
                        HandleLapData(lapData);
                        break;
                    case 4:
                        PacketParticipantsData participantsData = ByteArrayToStructure<PacketParticipantsData>(data);
                        HandleParticipantsData(participantsData);
                        break;
                    case 5:
                        PacketCarSetupData carSetupData = ByteArrayToStructure<PacketCarSetupData>(data);
                        break;
                    case 6:
                        PacketCarTelemetryData carTelemetryData = ByteArrayToStructure<PacketCarTelemetryData>(data);
                        HandleCarTelemetryData(carTelemetryData);
                        break;
                    case 7:
                        PacketCarStatusData carStatusData = ByteArrayToStructure<PacketCarStatusData>(data);
                        HandleCarStatusData(carStatusData);
                        break;
                    case 8:
                        PacketFinalClassificationData finalClassificationData = ByteArrayToStructure<PacketFinalClassificationData>(data);
                        break;
                    case 9:
                        PacketLobbyInfoData lobbyInfoData = ByteArrayToStructure<PacketLobbyInfoData>(data);
                        break;
                    case 10:
                        PacketCarDamageData carDamageData = ByteArrayToStructure<PacketCarDamageData>(data);
                        break;
                    case 11:
                        PacketSessionHistoryData sessionHistoryData = ByteArrayToStructure<PacketSessionHistoryData>(data);
                        break;
                    case 12:
                        PacketTyreSetsData tyreSetsData = ByteArrayToStructure<PacketTyreSetsData>(data);
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

        private int GetPlayerIndex(PacketHeader header)
        {
            return header.m_playerCarIndex;
        }

        private void HandleLapData(PacketLapData lapData)
        {
            int playerIndex = GetPlayerIndex(lapData.m_header);
            gameData.lapTime = (int)lapData.m_lapData[playerIndex].m_currentLapTimeInMS;
            gameData.lastLapTime = (int)lapData.m_lapData[playerIndex].m_lastLapTimeInMS;
            gameData.s1 = (int)lapData.m_lapData[playerIndex].m_sector1TimeInMS;
            gameData.s2 = Math.Max((int)lapData.m_lapData[playerIndex].m_sector2TimeInMS - gameData.s1, 0);
            gameData.s3 = Math.Max((int)lapData.m_lapData[playerIndex].m_currentLapTimeInMS - gameData.s1 - gameData.s2, 0);
            gameData.currentLap = (int)lapData.m_lapData[playerIndex].m_currentLapNum;
        }

        private void HandleCarTelemetryData(PacketCarTelemetryData carTelemetryData)
        {
            int playerIndex = GetPlayerIndex(carTelemetryData.m_header);
            gameData.rpm = carTelemetryData.m_carTelemetryData[playerIndex].m_engineRPM;
            gameData.speed = carTelemetryData.m_carTelemetryData[playerIndex].m_speed;
            gameData.gear = carTelemetryData.m_carTelemetryData[playerIndex].m_gear;
            if (carTelemetryData.m_carTelemetryData[playerIndex].m_drs == 1)
            {
                gameData.drsState = "Active";
            }
            else if (gameData.drsState == "Active" && carTelemetryData.m_carTelemetryData[playerIndex].m_drs == 0)
            {
                gameData.drsState = "None";
            }
            gameData.recommendedGear = carTelemetryData.m_suggestedGear;
            gameData.shiftLights = carTelemetryData.m_carTelemetryData[playerIndex].m_revLightsBitValue;
        }

        private void HandleCarStatusData(PacketCarStatusData carStatusData)
        {
            int playerIndex = GetPlayerIndex(carStatusData.m_header);
            gameData.ersCharge = (decimal)carStatusData.m_carStatusData[playerIndex].m_ersStoreEnergy;
            gameData.isOvertake = carStatusData.m_carStatusData[playerIndex].m_ersDeployMode == 3;
            if (gameData.drsState != "Active" && carStatusData.m_carStatusData[playerIndex].m_drsActivationDistance != 0) 
                gameData.drsState = "" + carStatusData.m_carStatusData[playerIndex].m_drsActivationDistance;
        }

        private void HandleSessionData(PacketSessionData sessionData)
        {
            gameData.totalLaps = (int)sessionData.m_totalLaps;
            gameData.speedUnit = sessionData.m_speedUnitsLeadPlayer == 0 ? "mph" : "km/h";
        }

        private void HandleParticipantsData(PacketParticipantsData participantsData)
        {
            gameData.position = participantsData.m_participants[GetPlayerIndex(participantsData.m_header)].m_raceNumber;
            gameData.totalPositions = participantsData.m_numActiveCars;
        }
    }
}