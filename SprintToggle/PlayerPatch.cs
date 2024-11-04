using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace SprintToggle;

public class PlayerPatch : IScriptMod {
    private const string SprintToggle = "sprint_toggle";

    public bool ShouldRun(string path) => path == "res://Scenes/Entities/Player/player.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens) {
        var extendsWaiter = new MultiTokenWaiter([
            t => t.Type is TokenType.PrExtends,
            t => t.Type is TokenType.Newline,
        ], allowPartialMatch: true);
        var mouselookMatch = new MultiTokenWaiter([
            t => t is IdentifierToken {Name: "mouse_look"},
            t => t.Type is TokenType.OpAssign,
            t => t is IdentifierToken {Name: "Input"},
            t => t.Type is TokenType.Period,
            t => t is IdentifierToken {Name: "is_action_pressed"},
            t => t.Type is TokenType.ParenthesisOpen,
            t => t is ConstantToken {Value: StringVariant {Value: "mouse_look"}},
            t => t.Type is TokenType.ParenthesisClose,
        ]);
        var newlineConsumer = new TokenConsumer(t => t.Type is TokenType.Newline);
        var sprintMatch = new MultiTokenWaiter([
            t => t is IdentifierToken {Name: "sprinting"},
            t => t.Type is TokenType.OpAssign,
            t => t.Type is TokenType.OpNot,
            t => t is IdentifierToken {Name: "Input"},
            t => t.Type is TokenType.Period,
            t => t is IdentifierToken {Name: "is_action_pressed"},
            t => t.Type is TokenType.ParenthesisOpen,
            t => t is ConstantToken {Value: StringVariant {Value: "move_sneak"}},
            t => t.Type is TokenType.ParenthesisClose
        ]);

        foreach (var token in tokens) {
            if (extendsWaiter.Check(token)) {
                yield return token;
                // var SprintToggle = false
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken(SprintToggle);
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new BoolVariant(false));
                yield return new Token(TokenType.Newline);
            } else if (newlineConsumer.Check(token)) {
                // this kills code lol #swag
                continue;
            } else if (mouselookMatch.Check(token)) {
                yield return token;

                yield return new Token(TokenType.Newline, 1);

                // if Input.is_action_just_pressed("move_sprint"):
                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken("Input");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("is_action_just_pressed");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("move_sprint"));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 2);

                // SprintToggle = !SprintToggle
                yield return new IdentifierToken(SprintToggle);
                yield return new Token(TokenType.OpAssign);
                yield return new Token(TokenType.OpNot);
                yield return new IdentifierToken(SprintToggle);
                yield return new Token(TokenType.Newline, 1);
            } else if (sprintMatch.Check(token)) {
                yield return token;

                // this turns "sprinting = not Input.is_action_pressed("move_sneak") and Input.is_action_pressed("move_sprint")" into "sprinting = not Input.is_action_pressed("move_sneak") and sprint_toggle"
                yield return new Token(TokenType.OpAnd);
                yield return new IdentifierToken(SprintToggle);

                newlineConsumer.SetReady();
            } else {
                yield return token;
            }
        }
    }
}
