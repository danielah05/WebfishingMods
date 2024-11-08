using System.Text.Json.Serialization;

namespace EventAlert;

public class Config {
    [JsonInclude] public bool HideChatPrompts = true;
    [JsonInclude] public bool MeteorAlert = true;
    [JsonInclude] public bool RainAlert = true;
    [JsonInclude] public bool VoidPortalAlert = true;
}
