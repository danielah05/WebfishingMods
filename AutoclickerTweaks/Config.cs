using System.Text.Json.Serialization;

namespace AutoclickerTweaks;

public class Config {
    [JsonInclude] public float AutoclickerSpeed = 0.11f;
}
