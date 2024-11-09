using System.Text.Json.Serialization;

namespace EventAlert;

public class Config {
    [JsonInclude] public bool HideVanillaChatPrompts = true;
    [JsonInclude] public bool ShowLogs = true;
    [JsonInclude] public bool ShowSeconds = true;
    [JsonInclude] public bool MeteorAlert = true;
    [JsonInclude] public bool RainAlert = true;
    [JsonInclude] public bool VoidPortalAlert = true;
}
