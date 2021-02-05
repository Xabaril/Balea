using Antlr4.Runtime;
using Balea.Authorization.Abac.Context;
using Balea.Authorization.Abac.Grammars;
using Balea.Authorization.Abac.Grammars.BAL;
using Balea.DSL.Grammar.Bal;

namespace Balea.Authorization.Abac.Parsers
{
    internal class BALParser
        : IGrammarParser
    {
        public bool CanParse(WellKnownGrammars grammar)
        {
            return grammar == WellKnownGrammars.Bal;
        }

        public AbacAuthorizationPolicy Parse(string policy)
        {
            var inputStream = new AntlrInputStream(policy);
            var lexer = new BalLexer(inputStream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new BalParser(tokenStream);

            return new BalVisitor().Visit(parser.policy());
        }
    }
}
