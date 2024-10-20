using System.Text.Json.Serialization;

namespace MeteorAlert;

public class Config {
    [JsonInclude] public bool SomeSetting = true;
}
