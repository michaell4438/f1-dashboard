using dashboard_host;
using System;
using System.Diagnostics;
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
        private Timer timer;
        private volatile GameData gameData = new GameData();
        private WebSocketServer webSocketServer;

        public UdpListener()
        {
            udpClient = new UdpClient(port);
            remoteEndPoint = new IPEndPoint(IPAddress.Any, port);

            webSocketServer = new WebSocketServer("http://localhost:5174/");
        }

        public void StartReceiving()
        {
            var cts = new CancellationTokenSource();

            _ = Task.Run(() => webSocketServer.StartAsync(cts.Token));
            timer = new Timer(BroadcastGameData, null, 0, 50);

            StartDashboardServer();

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

        private void StartDashboardServer()
        {
            // Run the command "npm run dev -- --host" in the f1-dashboard directory
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd",
                RedirectStandardInput = true,
                UseShellExecute = false
            };
            Process process = new Process
            {
                StartInfo = startInfo
            };
            process.Start();
            process.StandardInput.WriteLine("cd f1-dashboard");
            process.StandardInput.WriteLine("npm run dev -- --host");
        }

        private void BroadcastGameData(object state)
        {
            try
            {
                webSocketServer.BroadcastMessageAsync(gameData.ToJsonString()).Wait();
            }
            catch (Exception ex)
            { }
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
            gameData.s2 = (int)lapData.m_lapData[playerIndex].m_sector2TimeInMS;
            gameData.currentLap = (int)lapData.m_lapData[playerIndex].m_currentLapNum;
            gameData.position = (int)lapData.m_lapData[playerIndex].m_carPosition;
        }

        private void HandleCarTelemetryData(PacketCarTelemetryData carTelemetryData)
        {
            int playerIndex = GetPlayerIndex(carTelemetryData.m_header);
            gameData.rpm = carTelemetryData.m_carTelemetryData[playerIndex].m_engineRPM;
            gameData.speed = carTelemetryData.m_carTelemetryData[playerIndex].m_speed;
            if (gameData.speedUnit == "mph")
            {
                gameData.speed = (int)(gameData.speed * 0.621371);
            }
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
            gameData.shiftLights = carTelemetryData.m_carTelemetryData[playerIndex].m_revLightsPercent;
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
            gameData.totalPositions = participantsData.m_numActiveCars;
        }
    }
}