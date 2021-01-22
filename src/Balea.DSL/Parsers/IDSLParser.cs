using Balea.DSL.Grammar;

namespace Balea.DSL.Parsers
{
    interface IDSLParser
    {
        bool CanParse(AllowedGrammars grammar);
        DslAuthorizationPolicy Parse(string policy);
    }
}
