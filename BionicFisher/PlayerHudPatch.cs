using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace BionicFisher;

// patching the steam network probably sounds scary but this is just to implement the bionic reader to the chat client sided

public class PlayerHudPatch : IScriptMod {
    private const string Notif = "notif";
    private const string NotifSound = "notifsound";

    public bool ShouldRun(string path) => path == "res://Scenes/HUD/playerhud.gdc";

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

                // load custom font and override
                // gamechat.add_font_override("normal_font", ResourceLoader.load("res://BionicFisher/Fonts/arial_normal.tres"))
                yield return new IdentifierToken("gamechat");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("add_font_override");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("normal_font"));
                yield return new Token(TokenType.Comma);
                yield return new IdentifierToken("ResourceLoader");
                yield return new Token(TokenType.Period);
                yield return new Token(TokenType.BuiltInFunc, 76);
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("res://BionicFisher/Fonts/arial_normal.tres"));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);
                // gamechat.add_font_override("bold_font", ResourceLoader.load("res://BionicFisher/Fonts/arial_bold.tres"))
                yield return new IdentifierToken("gamechat");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("add_font_override");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("bold_font"));
                yield return new Token(TokenType.Comma);
                yield return new IdentifierToken("ResourceLoader");
                yield return new Token(TokenType.Period);
                yield return new Token(TokenType.BuiltInFunc, 76);
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("res://BionicFisher/Fonts/arial_bold.tres"));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);
                yield return token;
            } else {
                // return to original token
                yield return token;
            }
        }
    }
}
