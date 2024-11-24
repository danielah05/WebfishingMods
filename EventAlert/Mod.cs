using GDWeave;

namespace EventAlert;

public class Mod : IMod {
    public static Config Config = null!;
    public Mod(IModInterface modInterface) {
        Config = modInterface.ReadConfig<Config>();

        modInterface.RegisterScriptMod(new MeteorSpawnPatch());
        modInterface.RegisterScriptMod(new RainCloudPatch());
        modInterface.RegisterScriptMod(new VoidPortalPatch());
    }

    public void Dispose() { }
}
