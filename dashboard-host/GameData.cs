using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace dashboard_host
{
    [Serializable]
    public record class GameData
    {
        public int lapTime { get; set; }
        public int lastLapTime { get; set; }
        public int expectedDelta { get; set; }
        public int position { get; set; }
        public int totalPositions { get; set; }
        public int currentLap { get; set; }
        public int totalLaps { get; set; }
        public int rpm { get; set; } = 10000;
        public int speed { get; set; }
        public string speedUnit { get; set; } = "";
        public int gear { get; set; }
        public int recommendedGear { get; set; }
        public string drsState { get; set; } = "None";
        public int s1 { get; set; }
        public int s2 { get; set; }
        public decimal ersCharge { get; set; }
        public bool isOvertake { get; set; }
        public long shiftLights { get; set; }

        public string ToJsonString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
