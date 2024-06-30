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

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CarSetupData
    {
        public byte m_frontWing;                // Front wing aero
        public byte m_rearWing;                 // Rear wing aero
        public byte m_onThrottle;               // Differential adjustment on throttle (percentage)
        public byte m_offThrottle;              // Differential adjustment off throttle (percentage)
        public float m_frontCamber;             // Front camber angle (suspension geometry)
        public float m_rearCamber;              // Rear camber angle (suspension geometry)
        public float m_frontToe;                // Front toe angle (suspension geometry)
        public float m_rearToe;                 // Rear toe angle (suspension geometry)
        public byte m_frontSuspension;          // Front suspension
        public byte m_rearSuspension;           // Rear suspension
        public byte m_frontAntiRollBar;         // Front anti-roll bar
        public byte m_rearAntiRollBar;          // Front anti-roll bar
        public byte m_frontSuspensionHeight;    // Front ride height
        public byte m_rearSuspensionHeight;     // Rear ride height
        public byte m_brakePressure;            // Brake pressure (percentage)
        public byte m_brakeBias;                // Brake bias (percentage)
        public float m_rearLeftTyrePressure;    // Rear left tyre pressure (PSI)
        public float m_rearRightTyrePressure;   // Rear right tyre pressure (PSI)
        public float m_frontLeftTyrePressure;   // Front left tyre pressure (PSI)
        public float m_frontRightTyrePressure;  // Front right tyre pressure (PSI)
        public byte m_ballast;                  // Ballast
        public float m_fuelLoad;                // Fuel load
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PacketCarSetupData
    {
        public PacketHeader m_header;           // Header

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
        public CarSetupData[] m_carSetups;      // Array of CarSetupData
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CarTelemetryData
    {
        public ushort m_speed;                    // Speed of car in kilometres per hour
        public float m_throttle;                  // Amount of throttle applied (0.0 to 1.0)
        public float m_steer;                     // Steering (-1.0 (full lock left) to 1.0 (full lock right))
        public float m_brake;                     // Amount of brake applied (0.0 to 1.0)
        public byte m_clutch;                     // Amount of clutch applied (0 to 100)
        public sbyte m_gear;                      // Gear selected (1-8, N=0, R=-1)
        public ushort m_engineRPM;                // Engine RPM
        public byte m_drs;                        // 0 = off, 1 = on
        public byte m_revLightsPercent;           // Rev lights indicator (percentage)
        public ushort m_revLightsBitValue;        // Rev lights (bit 0 = leftmost LED, bit 14 = rightmost LED)

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public ushort[] m_brakesTemperature;      // Brakes temperature (celsius)

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] m_tyresSurfaceTemperature;  // Tyres surface temperature (celsius)

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] m_tyresInnerTemperature;    // Tyres inner temperature (celsius)

        public ushort m_engineTemperature;        // Engine temperature (celsius)

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] m_tyresPressure;           // Tyres pressure (PSI)

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] m_surfaceType;              // Driving surface, see appendices
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PacketCarTelemetryData
    {
        public PacketHeader m_header;             // Header

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
        public CarTelemetryData[] m_carTelemetryData; // Array of CarTelemetryData

        public byte m_mfdPanelIndex;              // Index of MFD panel open - 255 = MFD closed
                                                  // Single player, race – 0 = Car setup, 1 = Pits
                                                  // 2 = Damage, 3 =  Engine, 4 = Temperatures
                                                  // May vary depending on game mode
        public byte m_mfdPanelIndexSecondaryPlayer; // See above
        public sbyte m_suggestedGear;             // Suggested gear for the player (1-8)
                                                  // 0 if no gear suggested
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CarStatusData
    {
        public byte m_tractionControl;                // Traction control - 0 = off, 1 = medium, 2 = full
        public byte m_antiLockBrakes;                 // 0 (off) - 1 (on)
        public byte m_fuelMix;                        // Fuel mix - 0 = lean, 1 = standard, 2 = rich, 3 = max
        public byte m_frontBrakeBias;                 // Front brake bias (percentage)
        public byte m_pitLimiterStatus;               // Pit limiter status - 0 = off, 1 = on
        public float m_fuelInTank;                    // Current fuel mass
        public float m_fuelCapacity;                  // Fuel capacity
        public float m_fuelRemainingLaps;             // Fuel remaining in terms of laps (value on MFD)
        public ushort m_maxRPM;                       // Cars max RPM, point of rev limiter
        public ushort m_idleRPM;                      // Cars idle RPM
        public byte m_maxGears;                       // Maximum number of gears
        public byte m_drsAllowed;                     // 0 = not allowed, 1 = allowed
        public ushort m_drsActivationDistance;        // 0 = DRS not available, non-zero - DRS will be available in [X] metres
        public byte m_actualTyreCompound;             // F1 Modern - 16 = C5, 17 = C4, 18 = C3, 19 = C2, 20 = C1, 21 = C0, 7 = inter, 8 = wet
                                                      // F1 Classic - 9 = dry, 10 = wet
                                                      // F2 – 11 = super soft, 12 = soft, 13 = medium, 14 = hard, 15 = wet
        public byte m_visualTyreCompound;             // F1 visual (can be different from actual compound)
                                                      // 16 = soft, 17 = medium, 18 = hard, 7 = inter, 8 = wet
                                                      // F1 Classic – same as above
                                                      // F2 ‘19, 15 = wet, 19 – super soft, 20 = soft, 21 = medium, 22 = hard
        public byte m_tyresAgeLaps;                   // Age in laps of the current set of tyres
        public sbyte m_vehicleFiaFlags;               // -1 = invalid/unknown, 0 = none, 1 = green, 2 = blue, 3 = yellow
        public float m_enginePowerICE;                // Engine power output of ICE (W)
        public float m_enginePowerMGUK;               // Engine power output of MGU-K (W)
        public float m_ersStoreEnergy;                // ERS energy store in Joules
        public byte m_ersDeployMode;                  // ERS deployment mode, 0 = none, 1 = medium, 2 = hotlap, 3 = overtake
        public float m_ersHarvestedThisLapMGUK;       // ERS energy harvested this lap by MGU-K
        public float m_ersHarvestedThisLapMGUH;       // ERS energy harvested this lap by MGU-H
        public float m_ersDeployedThisLap;            // ERS energy deployed this lap
        public byte m_networkPaused;                  // Whether the car is paused in a network game
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PacketCarStatusData
    {
        public PacketHeader m_header;                 // Header

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
        public CarStatusData[] m_carStatusData;       // Array of CarStatusData
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FinalClassificationData
    {
        public byte m_position;                      // Finishing position
        public byte m_numLaps;                       // Number of laps completed
        public byte m_gridPosition;                  // Grid position of the car
        public byte m_points;                        // Number of points scored
        public byte m_numPitStops;                   // Number of pit stops made
        public byte m_resultStatus;                  // Result status - 0 = invalid, 1 = inactive, 2 = active
                                                     // 3 = finished, 4 = didnotfinish, 5 = disqualified
                                                     // 6 = not classified, 7 = retired
        public uint m_bestLapTimeInMS;               // Best lap time of the session in milliseconds
        public double m_totalRaceTime;               // Total race time in seconds without penalties
        public byte m_penaltiesTime;                 // Total penalties accumulated in seconds
        public byte m_numPenalties;                  // Number of penalties applied to this driver
        public byte m_numTyreStints;                 // Number of tyre stints up to maximum
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] m_tyreStintsActual;            // Actual tyres used by this driver
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] m_tyreStintsVisual;            // Visual tyres used by this driver
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] m_tyreStintsEndLaps;           // The lap number stints end on
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PacketFinalClassificationData
    {
        public PacketHeader m_header;                       // Header

        public byte m_numCars;                             // Number of cars in the final classification

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
        public FinalClassificationData[] m_classificationData;  // Array of FinalClassificationData
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LobbyInfoData
    {
        public byte m_aiControlled;                         // Whether the vehicle is AI (1) or Human (0) controlled
        public byte m_teamId;                               // Team id - see appendix (255 if no team currently selected)
        public byte m_nationality;                          // Nationality of the driver
        public byte m_platform;                             // 1 = Steam, 3 = PlayStation, 4 = Xbox, 6 = Origin, 255 = unknown
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48)]
        public string m_name;                               // Name of participant in UTF-8 format – null terminated
                                                            // Will be truncated with ... (U+2026) if too long
        public byte m_carNumber;                            // Car number of the player
        public byte m_readyStatus;                          // 0 = not ready, 1 = ready, 2 = spectating
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PacketLobbyInfoData
    {
        public PacketHeader m_header;                       // Header

        public byte m_numPlayers;                           // Number of players in the lobby data

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
        public LobbyInfoData[] m_lobbyPlayers;              // Array of LobbyInfoData structs
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CarDamageData
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] m_tyresWear;                   // Tyre wear (percentage)

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] m_tyresDamage;                  // Tyre damage (percentage)

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] m_brakesDamage;                 // Brakes damage (percentage)

        public byte m_frontLeftWingDamage;            // Front left wing damage (percentage)
        public byte m_frontRightWingDamage;           // Front right wing damage (percentage)
        public byte m_rearWingDamage;                 // Rear wing damage (percentage)
        public byte m_floorDamage;                    // Floor damage (percentage)
        public byte m_diffuserDamage;                 // Diffuser damage (percentage)
        public byte m_sidepodDamage;                  // Sidepod damage (percentage)
        public byte m_drsFault;                       // Indicator for DRS fault, 0 = OK, 1 = fault
        public byte m_ersFault;                       // Indicator for ERS fault, 0 = OK, 1 = fault
        public byte m_gearBoxDamage;                  // Gear box damage (percentage)
        public byte m_engineDamage;                   // Engine damage (percentage)
        public byte m_engineMGUHWear;                 // Engine wear MGU-H (percentage)
        public byte m_engineESWear;                   // Engine wear ES (percentage)
        public byte m_engineCEWear;                   // Engine wear CE (percentage)
        public byte m_engineICEWear;                  // Engine wear ICE (percentage)
        public byte m_engineMGUKWear;                 // Engine wear MGU-K (percentage)
        public byte m_engineTCWear;                   // Engine wear TC (percentage)
        public byte m_engineBlown;                    // Engine blown, 0 = OK, 1 = fault
        public byte m_engineSeized;                   // Engine seized, 0 = OK, 1 = fault
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PacketCarDamageData
    {
        public PacketHeader m_header;                 // Header

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
        public CarDamageData[] m_carDamageData;       // Array of CarDamageData structs
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LapHistoryData
    {
        public uint m_lapTimeInMS;              // Lap time in milliseconds
        public ushort m_sector1TimeInMS;        // Sector 1 time in milliseconds
        public byte m_sector1TimeMinutes;       // Sector 1 whole minute part
        public ushort m_sector2TimeInMS;        // Sector 2 time in milliseconds
        public byte m_sector2TimeMinutes;       // Sector 2 whole minute part
        public ushort m_sector3TimeInMS;        // Sector 3 time in milliseconds
        public byte m_sector3TimeMinutes;       // Sector 3 whole minute part
        public byte m_lapValidBitFlags;         // Bit flags indicating lap and sector validity
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TyreStintHistoryData
    {
        public byte m_endLap;                   // Lap the tyre usage ends on (255 for current tyre)
        public byte m_tyreActualCompound;       // Actual tyres used by this driver
        public byte m_tyreVisualCompound;       // Visual tyres used by this driver
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PacketSessionHistoryData
    {
        public PacketHeader m_header;                    // Header

        public byte m_carIdx;                            // Index of the car this lap data relates to
        public byte m_numLaps;                           // Number of laps in the data (including current partial lap)
        public byte m_numTyreStints;                     // Number of tyre stints in the data

        public byte m_bestLapTimeLapNum;                 // Lap the best lap time was achieved on
        public byte m_bestSector1LapNum;                 // Lap the best Sector 1 time was achieved on
        public byte m_bestSector2LapNum;                 // Lap the best Sector 2 time was achieved on
        public byte m_bestSector3LapNum;                 // Lap the best Sector 3 time was achieved on

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
        public LapHistoryData[] m_lapHistoryData;        // Array of LapHistoryData structs (max 100 laps)

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public TyreStintHistoryData[] m_tyreStintsHistoryData; // Array of TyreStintHistoryData structs (max 8 stints)
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TyreSetData
    {
        public byte m_actualTyreCompound;     // Actual tyre compound used
        public byte m_visualTyreCompound;     // Visual tyre compound used
        public byte m_wear;                   // Tyre wear (percentage)
        public byte m_available;              // Whether this set is currently available
        public byte m_recommendedSession;     // Recommended session for tyre set
        public byte m_lifeSpan;               // Laps left in this tyre set
        public byte m_usableLife;             // Max number of laps recommended for this compound
        public short m_lapDeltaTime;          // Lap delta time in milliseconds compared to fitted set
        public byte m_fitted;                 // Whether the set is fitted or not
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PacketTyreSetsData
    {
        public PacketHeader m_header;         // Header

        public byte m_carIdx;                 // Index of the car this data relates to

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public TyreSetData[] m_tyreSetData;  // Array of TyreSetData structs (13 dry + 7 wet)

        public byte m_fittedIdx;              // Index into array of fitted tyre
    }

    /**
     * Motion ex data intentionnaly left out
     */
}
