using GDWeave;

namespace AutoclickerTweaks;

public class Mod : IMod {
    public static Config Config = null!;
    public Mod(IModInterface modInterface) {
        Config = modInterface.ReadConfig<Config>();
        modInterface.RegisterScriptMod(new Fishing3Patch());
    }

    public void Dispose() { }
}
