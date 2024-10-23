using System.Text.Json.Serialization;

namespace EventAlert;

public class Config {
    [JsonInclude] public bool MeteorAlert = true;
    [JsonInclude] public bool RainAlert = true;
}
