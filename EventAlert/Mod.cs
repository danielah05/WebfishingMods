using GDWeave;

namespace EventAlert;

public class Mod : IMod {
    public Config Config;
    public Mod(IModInterface modInterface) {
        this.Config = modInterface.ReadConfig<Config>();

        if (Config.MeteorAlert) modInterface.RegisterScriptMod(new MeteorSpawnPatch());
        if (Config.RainAlert) modInterface.RegisterScriptMod(new RainCloudPatch());
    }

    public void Dispose() { }
}
