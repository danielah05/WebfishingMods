using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace AutoclickerTweaks;

public class Fishing3Patch : IScriptMod { 
    public bool ShouldRun(string path) => path == "res://Scenes/Minigames/Fishing3/fishing3.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens) {
        var readyMatch = new MultiTokenWaiter([
            t => t is IdentifierToken {Name: "_ready"},
            t => t.Type is TokenType.ParenthesisOpen,
            t => t.Type is TokenType.ParenthesisClose,
            t => t.Type is TokenType.Colon,
            t => t.Type is TokenType.Newline
        ]);

        foreach (var token in tokens) {
            if (readyMatch.Check(token)) {
                yield return token;

                // $autoclick_timer.wait_time = AutoclickerSpeed
                yield return new Token(TokenType.Dollar);
                yield return new IdentifierToken("autoclick_timer");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("wait_time");
                yield return new Token(TokenType.OpAssign);
                // fail check in case someone just tries to do something smaller than the smallest possible wait_time
                if (Mod.Config.AutoclickerSpeed < 0.001f) {
                    yield return new ConstantToken(new RealVariant(0.001));
                } else {
                    yield return new ConstantToken(new RealVariant(Mod.Config.AutoclickerSpeed));
                }

                yield return token;
            } else {
                yield return token;
            }
        }
    }
}
