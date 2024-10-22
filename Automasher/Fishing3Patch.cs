using GDWeave.Godot;
using GDWeave.Modding;

namespace Automasher;

public class Fishing3Patch : IScriptMod { 
    // load the fishing3.gdc file
    public bool ShouldRun(string path) => path == "res://Scenes/Minigames/Fishing3/fishing3.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens) {
        var modified = false;
        foreach (var token in tokens) {
            // look for the first instance of "is_action_just_pressed"
            if (!modified && token is IdentifierToken {Name: "is_action_just_pressed"} identifierToken) {
                // replace it with "is_action_pressed", removing the need of having to mash
                identifierToken.Name = "is_action_pressed";
                modified = true;
            }
                    
            yield return token;
        }
    }
}