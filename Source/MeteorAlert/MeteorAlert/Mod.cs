using GDWeave;

namespace MeteorAlert;

public class Mod : IMod {
    public Mod(IModInterface modInterface) {
        modInterface.RegisterScriptMod(new MeteorSpawnPatch());
    }

    public void Dispose() { }
}
