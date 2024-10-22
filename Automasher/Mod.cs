using GDWeave;

namespace Automasher;

public class Mod : IMod {
    public Mod(IModInterface modInterface) {
        modInterface.RegisterScriptMod(new Fishing3Patch());
    }

    public void Dispose() { }
}
