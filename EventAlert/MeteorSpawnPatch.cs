using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace EventAlert;

public class MeteorSpawnPatch : IScriptMod {
    private const string Notif = "notif";
    private const string NotifSound = "notifsound";

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

                // play sound effect
                // var notif = AudioStreamPlayer.new()
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken(Notif);
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("AudioStreamPlayer");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("new");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);
                // var notifsound = load("res://Sounds/store_bell.ogg")
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken(NotifSound);
                yield return new Token(TokenType.OpAssign);
                yield return new Token(TokenType.BuiltInFunc, 76);
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("res://Sounds/store_bell.ogg"));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);
                // add_child(notif)
                yield return new IdentifierToken("add_child");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken(Notif);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);
                // notif.set_stream(notifsound)
                yield return new IdentifierToken(Notif);
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("set_stream");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken(NotifSound);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);
                // notif.volume_db = -4
                yield return new IdentifierToken(Notif);
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("volume_db");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new IntVariant(-4));
                yield return new Token(TokenType.Newline, 1);
                // notif.pitch_scale = 1
                yield return new IdentifierToken(Notif);
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("pitch_scale");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new IntVariant(1));
                yield return new Token(TokenType.Newline, 1);
                // notif.bus = "SFX"
                yield return new IdentifierToken(Notif);
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("bus");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant("SFX"));
                yield return new Token(TokenType.Newline, 1);
                // notif.play()
                yield return new IdentifierToken(Notif);
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("play");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);

                // add custom notification code
                // PlayerData._send_notification("a meteor has landed!", 1)
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("_send_notification");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("a meteor has landed!"));
                yield return new Token(TokenType.Comma);
                yield return new ConstantToken(new IntVariant(1));
                yield return new Token(TokenType.ParenthesisClose);
                yield return token;
            } else {
                // return to original token
                yield return token;
            }
        }
    }
}
