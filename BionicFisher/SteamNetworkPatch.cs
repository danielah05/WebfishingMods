using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace BionicFisher;

// patching the steam network probably sounds scary but this is just to implement the bionic reader to the chat client sided

public class SteamNetworkPatch : IScriptMod {
    private const string SplitText = "split_text";
    private const string CombineText = "combine_text";
    private const string BBDetect = "bbdetect";
    private const string BBDetect2 = "bbdetect2";
    private const string DrunkDetect = "drunkdetect";
    private const string Offset = "offset";
    private const string LetterCount = "lettercount";
    private const string BionicEnd = "bionicend";
    private const string ForN = "n";
    private const string ForL = "l";
    private const string ForC = "c";

    public bool ShouldRun(string path) => path == "res://Scenes/Singletons/SteamNetwork.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens) {
        var updatechatMatch = new MultiTokenWaiter([
            t => t is IdentifierToken {Name: "_update_chat"},
            t => t.Type is TokenType.ParenthesisOpen,
            t => t is IdentifierToken {Name: "text"},
            t => t.Type is TokenType.Comma,
            t => t is IdentifierToken {Name: "local"},
            t => t.Type is TokenType.OpAssign,
            t => t is ConstantToken {Value: BoolVariant {Value: false}},
            t => t.Type is TokenType.ParenthesisClose,
            t => t.Type is TokenType.Colon,
            t => t.Type is TokenType.Newline
        ]);

        foreach (var token in tokens) {
            if (updatechatMatch.Check(token)) {
                // found match
                yield return token;

                // var split_text = text.split(" ")
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken(SplitText);
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("text");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("split");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant(" "));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);
                // var combine_text = ""
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken(CombineText);
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant(""));
                yield return new Token(TokenType.Newline, 1);
                // for n in split_text.size():
                yield return new Token(TokenType.CfFor);
                yield return new IdentifierToken(ForN);
                yield return new Token(TokenType.OpIn);
                yield return new IdentifierToken(SplitText);
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("size");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 2);
                // var bbdetect = false
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken(BBDetect);
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new BoolVariant(false));
                yield return new Token(TokenType.Newline, 2);
                // var bbdetect2 = false
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken(BBDetect2);
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new BoolVariant(false));
                yield return new Token(TokenType.Newline, 2);
                // var drunkdetect = false
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken(DrunkDetect);
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new BoolVariant(false));
                yield return new Token(TokenType.Newline, 2);
                // var offset = 1
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken(Offset);
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new IntVariant(1));
                yield return new Token(TokenType.Newline, 2);
                // var lettercount = 0
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken(LetterCount);
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new IntVariant(0));
                yield return new Token(TokenType.Newline, 2);
                // var bionicend = 0
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken(BionicEnd);
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new IntVariant(0));
                yield return new Token(TokenType.Newline, 2);
                // for l in split_text[n]:
                yield return new Token(TokenType.CfFor);
                yield return new IdentifierToken(ForL);
                yield return new Token(TokenType.OpIn);
                yield return new IdentifierToken(SplitText);
                yield return new Token(TokenType.BracketOpen);
                yield return new IdentifierToken(ForN);
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 3);
                // if l == "[" and bbdetect == true:
                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken(ForL);
                yield return new Token(TokenType.OpEqual);
                yield return new ConstantToken(new StringVariant("["));
                yield return new Token(TokenType.OpAnd);
                yield return new IdentifierToken(BBDetect);
                yield return new Token(TokenType.OpEqual);
                yield return new ConstantToken(new BoolVariant(true));
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 4);
                // bbdetect2 = true
                yield return new IdentifierToken(BBDetect2);
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new BoolVariant(true));
                yield return new Token(TokenType.Newline, 3);
                // if l == "[":
                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken(ForL);
                yield return new Token(TokenType.OpEqual);
                yield return new ConstantToken(new StringVariant("["));
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 4);
                // offset = 17
                yield return new IdentifierToken(Offset);
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new IntVariant(17));
                yield return new Token(TokenType.Newline, 4);
                // bbdetect = true
                yield return new IdentifierToken(BBDetect);
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new BoolVariant(true));
                yield return new Token(TokenType.Newline, 3);
                // if l == "/" and bbdetect == true:
                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken(ForL);
                yield return new Token(TokenType.OpEqual);
                yield return new ConstantToken(new StringVariant("/"));
                yield return new Token(TokenType.OpAnd);
                yield return new IdentifierToken(BBDetect);
                yield return new Token(TokenType.OpEqual);
                yield return new ConstantToken(new BoolVariant(true));
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 4);
                // drunkdetect = true
                yield return new IdentifierToken(DrunkDetect);
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new BoolVariant(true));
                yield return new Token(TokenType.Newline, 3);
                // if bbdetect2 == true:
                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken(BBDetect2);
                yield return new Token(TokenType.OpEqual);
                yield return new ConstantToken(new BoolVariant(true));
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 4);
                // bionicend = floor((split_text[n].length() - offset - offset) / 2.5)
                yield return new IdentifierToken(BionicEnd);
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("floor");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken(SplitText);
                yield return new Token(TokenType.BracketOpen);
                yield return new IdentifierToken(ForN);
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("length");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.OpSub);
                yield return new IdentifierToken(Offset);
                yield return new Token(TokenType.OpSub);
                yield return new IdentifierToken(Offset);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.OpDiv);
                yield return new ConstantToken(new RealVariant(2.5));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 3);
                // else:
                yield return new Token(TokenType.CfElse);
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 4);
                // bionicend = floor((split_text[n].length() - offset) / 2.5)
                yield return new IdentifierToken(BionicEnd);
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("floor");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken(SplitText);
                yield return new Token(TokenType.BracketOpen);
                yield return new IdentifierToken(ForN);
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("length");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.OpSub);
                yield return new IdentifierToken(Offset);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.OpDiv);
                yield return new ConstantToken(new RealVariant(2.5));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 3);
                // if bbdetect == false:
                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken(BBDetect);
                yield return new Token(TokenType.OpEqual);
                yield return new ConstantToken(new BoolVariant(false));
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 4);
                // lettercount += 1
                yield return new IdentifierToken(LetterCount);
                yield return new Token(TokenType.OpAssignAdd);
                yield return new ConstantToken(new IntVariant(1));
                yield return new Token(TokenType.Newline, 2);
                // if not split_text[n] == "":
                yield return new Token(TokenType.CfIf);
                yield return new Token(TokenType.OpNot);
                yield return new IdentifierToken(SplitText);
                yield return new Token(TokenType.BracketOpen);
                yield return new IdentifierToken(ForN);
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.OpEqual);
                yield return new ConstantToken(new StringVariant(""));
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 3);
                // if bbdetect == true and lettercount > 0:
                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken(BBDetect);
                yield return new Token(TokenType.OpEqual);
                yield return new ConstantToken(new BoolVariant(true));
                yield return new Token(TokenType.OpAnd);
                yield return new IdentifierToken(LetterCount);
                yield return new Token(TokenType.OpGreater);
                yield return new ConstantToken(new IntVariant(0));
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 4);
                // split_text[n] = split_text[n].insert(lettercount, "[/b]")
                yield return new IdentifierToken(SplitText);
                yield return new Token(TokenType.BracketOpen);
                yield return new IdentifierToken(ForN);
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken(SplitText);
                yield return new Token(TokenType.BracketOpen);
                yield return new IdentifierToken(ForN);
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("insert");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken(LetterCount);
                yield return new Token(TokenType.Comma);
                yield return new ConstantToken(new StringVariant("[/b]"));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 3);
                // elif drunkdetect == false:
                yield return new Token(TokenType.CfElif);
                yield return new IdentifierToken(DrunkDetect);
                yield return new Token(TokenType.OpEqual);
                yield return new ConstantToken(new BoolVariant(false));
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 4);
                // split_text[n] = split_text[n].insert(bionicend + offset, "[/b]")
                yield return new IdentifierToken(SplitText);
                yield return new Token(TokenType.BracketOpen);
                yield return new IdentifierToken(ForN);
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken(SplitText);
                yield return new Token(TokenType.BracketOpen);
                yield return new IdentifierToken(ForN);
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("insert");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken(BionicEnd);
                yield return new Token(TokenType.OpAdd);
                yield return new IdentifierToken(Offset);
                yield return new Token(TokenType.Comma);
                yield return new ConstantToken(new StringVariant("[/b]"));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 3);
                // if not split_text[n][0] == "[":
                yield return new Token(TokenType.CfIf);
                yield return new Token(TokenType.OpNot);
                yield return new IdentifierToken(SplitText);
                yield return new Token(TokenType.BracketOpen);
                yield return new IdentifierToken(ForN);
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.BracketOpen);
                yield return new ConstantToken(new IntVariant(0));
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.OpEqual);
                yield return new ConstantToken(new StringVariant("["));
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 4);
                // split_text[n] = split_text[n].insert(0, "[b]")
                yield return new IdentifierToken(SplitText);
                yield return new Token(TokenType.BracketOpen);
                yield return new IdentifierToken(ForN);
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken(SplitText);
                yield return new Token(TokenType.BracketOpen);
                yield return new IdentifierToken(ForN);
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("insert");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new IntVariant(0));
                yield return new Token(TokenType.Comma);
                yield return new ConstantToken(new StringVariant("[b]"));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 3);
                // elif split_text[n][0] == "[" and drunkdetect == false:
                yield return new Token(TokenType.CfElif);
                yield return new IdentifierToken(SplitText);
                yield return new Token(TokenType.BracketOpen);
                yield return new IdentifierToken(ForN);
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.BracketOpen);
                yield return new ConstantToken(new IntVariant(0));
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.OpEqual);
                yield return new ConstantToken(new StringVariant("["));
                yield return new Token(TokenType.OpAnd);
                yield return new IdentifierToken(DrunkDetect);
                yield return new Token(TokenType.OpEqual);
                yield return new ConstantToken(new BoolVariant(false));
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 4);
                // split_text[n] = split_text[n].insert(offset, "[b]")
                yield return new IdentifierToken(SplitText);
                yield return new Token(TokenType.BracketOpen);
                yield return new IdentifierToken(ForN);
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken(SplitText);
                yield return new Token(TokenType.BracketOpen);
                yield return new IdentifierToken(ForN);
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("insert");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken(Offset);
                yield return new Token(TokenType.Comma);
                yield return new ConstantToken(new StringVariant("[b]"));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);
                // for c in split_text.size():
                yield return new Token(TokenType.CfFor);
                yield return new IdentifierToken(ForC);
                yield return new Token(TokenType.OpIn);
                yield return new IdentifierToken(SplitText);
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("size");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 2);
                // combine_text = combine_text + split_text[c] + " "
                yield return new IdentifierToken(CombineText);
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken(CombineText);
                yield return new Token(TokenType.OpAdd);
                yield return new IdentifierToken(SplitText);
                yield return new Token(TokenType.BracketOpen);
                yield return new IdentifierToken(ForC);
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.OpAdd);
                yield return new ConstantToken(new StringVariant(" "));
                yield return new Token(TokenType.Newline, 1);
                // text = combine_text
                yield return new IdentifierToken("text");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken(CombineText);
                yield return new Token(TokenType.Newline, 1);

                yield return token;
            } else {
                // return to original token
                yield return token;
            }
        }
    }
}
