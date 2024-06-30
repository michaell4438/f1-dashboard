using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace dashboard_host
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PacketHeader
    {
        public ushort m_packetFormat;
        public byte m_gameYear;
        public byte m_gameMajorVersion;
        public byte m_gameMinorVersion;
        public byte m_packetVersion;
        public byte m_packetId;
        public ulong m_sessionUID;
        public float m_sessionTime;
        public uint m_frameIdentifier;
        public uint m_overallFrameIdentifier;
        public byte m_playerCarIndex;
        public byte m_secondaryPlayerCarIndex;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CarMotionData
    {
        public float m_worldPositionX;
        public float m_worldPositionY;
        public float m_worldPositionZ;
        public float m_worldVelocityX;
        public float m_worldVelocityY;
        public float m_worldVelocityZ;
        public short m_worldForwardDirX;
        public short m_worldForwardDirY;
        public short m_worldForwardDirZ;
        public short m_worldRightDirX;
        public short m_worldRightDirY;
        public short m_worldRightDirZ;
        public float m_gForceLateral;
        public float m_gForceLongitudinal;
        public float m_gForceVertical;
        public float m_yaw;
        public float m_pitch;
        public float m_roll;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PacketMotionData
    {
        public PacketHeader m_header;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
        public CarMotionData[] m_carMotionData;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MarshalZone
    {
        public float m_zoneStart;
        public sbyte m_zoneFlag;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct WeatherForecastSample
    {
        public byte m_sessionType;
        public byte m_timeOffset;
        public byte m_weather;
        public byte m_trackTemperature;
        public byte m_trackTemperatureChange;
        public byte m_airTemperature;
        public byte m_airTemperatureChange;
        public byte m_rainPercentage;
    }

    using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PacketSessionData
    {
        public PacketHeader m_header;                   // Header

        public byte m_weather;                          // Weather - 0 = clear, 1 = light cloud, 2 = overcast
                                                        // 3 = light rain, 4 = heavy rain, 5 = storm
        public sbyte m_trackTemperature;                // Track temp. in degrees celsius
        public sbyte m_airTemperature;                  // Air temp. in degrees celsius
        public byte m_totalLaps;                    // Total number of laps in this race
        public ushort m_trackLength;                // Track length in metres
        public byte m_sessionType;                  // 0 = unknown, 1 = P1, 2 = P2, 3 = P3, 4 = Short P
                                                    // 5 = Q1, 6 = Q2, 7 = Q3, 8 = Short Q, 9 = OSQ
                                                    // 10 = R, 11 = R2, 12 = R3, 13 = Time Trial
        public sbyte m_trackId;                         // -1 for unknown, see appendix
        public byte m_formula;                          // Formula, 0 = F1 Modern, 1 = F1 Classic, 2 = F2,
                                                        // 3 = F1 Generic, 4 = Beta, 5 = Supercars
                                                        // 6 = Esports, 7 = F2 2021
        public ushort m_sessionTimeLeft;                // Time left in session in seconds
        public ushort m_sessionDuration;              // Session duration in seconds
        public byte m_pitSpeedLimit;                    // Pit speed limit in kilometres per hour
        public byte m_gamePaused;                     // Whether the game is paused – network game only
        public byte m_isSpectating;                 // Whether the player is spectating
        public byte m_spectatorCarIndex;                // Index of the car being spectated
        public byte m_sliProNativeSupport;          // SLI Pro support, 0 = inactive, 1 = active
        public byte m_numMarshalZones;                // Number of marshal zones to follow
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)]
        public MarshalZone[] m_marshalZones;           // List of marshal zones – max 21
        public byte m_safetyCarStatus;                // 0 = no safety car, 1 = full
                                                      // 2 = virtual, 3 = formation lap
        public byte m_networkGame;                    // 0 = offline, 1 = online
        public byte m_numWeatherForecastSamples;      // Number of weather samples to follow
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 56)]
        public WeatherForecastSample[] m_weatherForecastSamples; // Array of weather forecast samples
        public byte m_forecastAccuracy;          // 0 = Perfect, 1 = Approximate
        public byte m_aiDifficulty;              // AI Difficulty rating – 0-110
        public uint m_seasonLinkIdentifier;      // Identifier for season - persists across saves
        public uint m_weekendLinkIdentifier;     // Identifier for weekend - persists across saves
        public uint m_sessionLinkIdentifier;     // Identifier for session - persists across saves
        public byte m_pitStopWindowIdealLap;     // Ideal lap to pit on for current strategy (player)
        public byte m_pitStopWindowLatestLap;    // Latest lap to pit on for current strategy (player)
        public byte m_pitStopRejoinPosition;     // Predicted position to rejoin at (player)
        public byte m_steeringAssist;            // 0 = off, 1 = on
        public byte m_brakingAssist;             // 0 = off, 1 = low, 2 = medium, 3 = high
        public byte m_gearboxAssist;             // 1 = manual, 2 = manual & suggested gear, 3 = auto
        public byte m_pitAssist;                 // 0 = off, 1 = on
        public byte m_pitReleaseAssist;          // 0 = off, 1 = on
        public byte m_ERSAssist;                 // 0 = off, 1 = on
        public byte m_DRSAssist;                 // 0 = off, 1 = on
        public byte m_dynamicRacingLine;         // 0 = off, 1 = corners only, 2 = full
        public byte m_dynamicRacingLineType;     // 0 = 2D, 1 = 3D
        public byte m_gameMode;                  // Game mode id - see appendix
        public byte m_ruleSet;                   // Ruleset - see appendix
        public uint m_timeOfDay;                 // Local time of day - minutes since midnight
        public byte m_sessionLength;             // 0 = None, 2 = Very Short, 3 = Short, 4 = Medium
                                                 // 5 = Medium Long, 6 = Long, 7 = Full
        public byte m_speedUnitsLeadPlayer;             // 0 = MPH, 1 = KPH
        public byte m_temperatureUnitsLeadPlayer;       // 0 = Celsius, 1 = Fahrenheit
        public byte m_speedUnitsSecondaryPlayer;        // 0 = MPH, 1 = KPH
        public byte m_temperatureUnitsSecondaryPlayer;  // 0 = Celsius, 1 = Fahrenheit
        public byte m_numSafetyCarPeriods;              // Number of safety cars called during session
        public byte m_numVirtualSafetyCarPeriods;       // Number of virtual safety cars called
        public byte m_numRedFlagPeriods;                // Number of red flags called during session
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LapData
    {
        public uint m_lastLapTimeInMS;           // Last lap time in milliseconds
        public uint m_currentLapTimeInMS;        // Current time around the lap in milliseconds
        public ushort m_sector1TimeInMS;        // Sector 1 time in milliseconds
        public byte m_sector1TimeMinutes;       // Sector 1 whole minute part
        public ushort m_sector2TimeInMS;        // Sector 2 time in milliseconds
        public byte m_sector2TimeMinutes;       // Sector 2 whole minute part
        public ushort m_deltaToCarInFrontInMS;  // Time delta to car in front in milliseconds
        public ushort m_deltaToRaceLeaderInMS;  // Time delta to race leader in milliseconds
        public float m_lapDistance;          // Distance vehicle is around current lap in metres – could
                                             // be negative if line hasn’t been crossed yet
        public float m_totalDistance;            // Total distance travelled in session in metres – could
                                                 // be negative if line hasn’t been crossed yet
        public float m_safetyCarDelta;          // Delta in seconds for safety car
        public byte m_carPosition;               // Car race position
        public byte m_currentLapNum;             // Current lap number
        public byte m_pitStatus;                 // 0 = none, 1 = pitting, 2 = in pit area
        public byte m_numPitStops;               // Number of pit stops taken in this race
        public byte m_sector;                    // 0 = sector1, 1 = sector2, 2 = sector3
        public byte m_currentLapInvalid;         // Current lap invalid - 0 = valid, 1 = invalid
        public byte m_penalties;                 // Accumulated time penalties in seconds to be added
        public byte m_totalWarnings;             // Accumulated number of warnings issued
        public byte m_cornerCuttingWarnings;     // Accumulated number of corner cutting warnings issued
        public byte m_numUnservedDriveThroughPens;  // Num drive through pens left to serve
        public byte m_numUnservedStopGoPens;        // Num stop go pens left to serve
        public byte m_gridPosition;              // Grid position the vehicle started the race in
        public byte m_driverStatus;              // Status of driver - 0 = in garage, 1 = flying lap
                                                 // 2 = in lap, 3 = out lap, 4 = on track
        public byte m_resultStatus;              // Result status - 0 = invalid, 1 = inactive, 2 = active
                                                 // 3 = finished, 4 = didnotfinish, 5 = disqualified
                                                 // 6 = not classified, 7 = retired
        public byte m_pitLaneTimerActive;        // Pit lane timing, 0 = inactive, 1 = active
        public ushort m_pitLaneTimeInLaneInMS;   // If active, the current time spent in the pit lane in ms
        public ushort m_pitStopTimerInMS;        // Time of the actual pit stop in ms
        public byte m_pitStopShouldServePen;     // Whether the car should serve a penalty at this stop
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PacketLapData
    {
        public PacketHeader m_header;                   // Header

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
        public LapData[] m_lapData;                     // Lap data for all cars on track
    }

    /**
     * Event data intentionnaly left out
     */

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ParticipantData
    {
        public byte m_aiControlled;      // Whether the vehicle is AI (1) or Human (0) controlled
        public byte m_driverId;          // Driver id - see appendix, 255 if network human
        public byte m_networkId;         // Network id – unique identifier for network players
        public byte m_teamId;            // Team id - see appendix
        public byte m_myTeam;            // My team flag – 1 = My Team, 0 = otherwise
        public byte m_raceNumber;        // Race number of the car
        public byte m_nationality;       // Nationality of the driver
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 48)]
        public byte[] m_name;            // Name of participant in UTF-8 format – null terminated
                                         // Will be truncated with … (U+2026) if too long
        public byte m_yourTelemetry;     // The player's UDP setting, 0 = restricted, 1 = public
        public byte m_showOnlineNames;   // The player's show online names setting, 0 = off, 1 = on
        public byte m_platform;          // 1 = Steam, 3 = PlayStation, 4 = Xbox, 6 = Origin, 255 = unknown
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PacketParticipantsData
    {
        public PacketHeader m_header;                   // Header

        public byte m_numActiveCars;                 // Number of active cars in the data – should match number of
                                                     // cars on HUD
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
        public ParticipantData[] m_participants;      // Data for all cars
    }   
}
