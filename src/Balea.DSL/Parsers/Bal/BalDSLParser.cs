using Antlr4.Runtime;
using Balea.DSL.Grammar;
using Balea.DSL.Grammar.Bal;
using Balea.DSL.Grammars.Bal;

namespace Balea.DSL.Parsers.Bal
{
    internal class BalDSLParser
        : IDSLParser
    {
        public bool CanParse(AllowedGrammars grammar)
        {
            return grammar == AllowedGrammars.Bal;
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
