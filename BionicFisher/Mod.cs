using GDWeave;

namespace BionicFisher;

public class Mod : IMod {
    public Mod(IModInterface modInterface) {
        modInterface.RegisterScriptMod(new PlayerHudPatch());
        modInterface.RegisterScriptMod(new SteamNetworkPatch());
    }

    public void Dispose() { }
}
