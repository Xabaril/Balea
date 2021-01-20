using Antlr4.Runtime;

namespace Balea.DSL.Grammar.Bal
{
    internal class BalDSLParser
        : IDSLParser
    {
        public bool CanParse(Grammars grammar)
        {
            return grammar == Grammars.Bal;
        }

        public DslAuthorizationPolicy Parse(string policy)
        {
            var inputStream = new AntlrInputStream(policy);
            var lexer = new BalLexer(inputStream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new BalParser(tokenStream);

            return new BalVisitor().Visit(parser.policy());
        }
    }
}
