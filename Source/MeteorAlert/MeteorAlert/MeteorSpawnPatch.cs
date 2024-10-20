using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace MeteorAlert;

public class MeteorSpawnPatch : IScriptMod {
    public bool ShouldRun(string path) => path == "res://Scenes/Entities/MeteorSpawn/meteor_spawn.gdc";

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
                // found match
                yield return token;

                // add custom notification code
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("_send_notification");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("a meteor has landed!"));
                yield return new Token(TokenType.ParenthesisClose);
                new Token(TokenType.Newline, 1);
            } else {
                // return to original token
                yield return token;
            }
        }
    }
}