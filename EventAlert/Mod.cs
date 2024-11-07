using GDWeave;

namespace EventAlert;

public class Mod : IMod {
    public static Config Config = null!;
    public Mod(IModInterface modInterface) {
        Config = modInterface.ReadConfig<Config>();

        if (Config.MeteorAlert) modInterface.RegisterScriptMod(new MeteorSpawnPatch());
        if (Config.RainAlert) modInterface.RegisterScriptMod(new RainCloudPatch());
    }

    public void Dispose() { }
}
