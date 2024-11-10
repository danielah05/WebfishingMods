using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace EventAlert;

public class MeteorSpawnPatch : IScriptMod {
    private const string Time = "time";
    private const string Notif = "notif";
    private const string NotifSound = "notifsound";

    public bool ShouldRun(string path) => path == "res://Scenes/Entities/MeteorSpawn/meteor_spawn.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens) {
        var newlineConsumer = new TokenConsumer(t => t.Type is TokenType.Newline);
        var readyMatch = new MultiTokenWaiter([
            t => t is IdentifierToken {Name: "_ready"},
            t => t.Type is TokenType.ParenthesisOpen,
            t => t.Type is TokenType.ParenthesisClose,
            t => t.Type is TokenType.Colon,
            t => t.Type is TokenType.Newline
        ]);

        foreach (var token in tokens) {
            if (newlineConsumer.Check(token)) {
                continue;
            } else if (readyMatch.Check(token)) {
                // found match
                yield return token;

                // add chat log
                if (Mod.Config.ShowLogs) {
                    // var time = Time.get_time_string_from_system()
                    yield return new Token(TokenType.PrVar);
                    yield return new IdentifierToken(Time);
                    yield return new Token(TokenType.OpAssign);
                    yield return new IdentifierToken("Time");
                    yield return new Token(TokenType.Period);
                    yield return new IdentifierToken("get_time_string_from_system");
                    yield return new Token(TokenType.ParenthesisOpen);
                    yield return new Token(TokenType.ParenthesisClose);
                    yield return new Token(TokenType.Newline, 1);
                    // hide seconds
                    if (!Mod.Config.ShowSeconds) {
                        // time.erase(time.length() - 3, 3)
                        yield return new IdentifierToken(Time);
                        yield return new Token(TokenType.Period);
                        yield return new IdentifierToken("erase");
                        yield return new Token(TokenType.ParenthesisOpen);
                        yield return new IdentifierToken(Time);
                        yield return new Token(TokenType.Period);
                        yield return new IdentifierToken("length");
                        yield return new Token(TokenType.ParenthesisOpen);
                        yield return new Token(TokenType.ParenthesisClose);
                        yield return new Token(TokenType.OpSub);
                        yield return new ConstantToken(new IntVariant(3));
                        yield return new Token(TokenType.Comma);
                        yield return new ConstantToken(new IntVariant(3));
                        yield return new Token(TokenType.ParenthesisClose);
                        yield return new Token(TokenType.Newline, 1);
                    }
                    // time = time.lstrip(0)
                    yield return new IdentifierToken(Time);
                    yield return new Token(TokenType.OpAssign);
                    yield return new IdentifierToken(Time);
                    yield return new Token(TokenType.Period);
                    yield return new IdentifierToken("lstrip");
                    yield return new Token(TokenType.ParenthesisOpen);
                    yield return new ConstantToken(new IntVariant(0));
                    yield return new Token(TokenType.ParenthesisClose);
                    yield return new Token(TokenType.Newline, 1);
                    // Network._update_chat("[color=#1e814e](" + time + " Meteor)[/color] a meteor has landed!")
                    yield return new IdentifierToken("Network");
                    yield return new Token(TokenType.Period);
                    yield return new IdentifierToken("_update_chat");
                    yield return new Token(TokenType.ParenthesisOpen);
                    yield return new ConstantToken(new StringVariant("[color=#1e814e]("));
                    yield return new Token(TokenType.OpAdd);
                    yield return new IdentifierToken(Time);
                    yield return new Token(TokenType.OpAdd);
                    yield return new ConstantToken(new StringVariant(" Meteor)[/color] a meteor has landed!"));
                    yield return new Token(TokenType.ParenthesisClose);
                    yield return new Token(TokenType.Newline, 1);
                }

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
                // var notifsound = load("res://mods/EventAlert/Assets/drip3.ogg")
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken(NotifSound);
                yield return new Token(TokenType.OpAssign);
                yield return new Token(TokenType.BuiltInFunc, 76);
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("res://mods/EventAlert/Assets/drip3.ogg"));
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
                // notif.volume_db = -16
                yield return new IdentifierToken(Notif);
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("volume_db");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new IntVariant(-16));
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

                // remove the "what was that" text from the chat, its not needed
                if (Mod.Config.HideVanillaChatPrompts) newlineConsumer.SetReady();
                else yield return token;
            } else {
                // return to original token
                yield return token;
            }
        }
    }
}
