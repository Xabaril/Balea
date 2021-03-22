using Balea.DSL.Grammar;

namespace Balea.DSL.Parsers
{
    interface IDSLParser
    {
        bool CanParse(AllowedGrammars grammar);
        AbacAuthorizationPolicy Parse(string policy);
    }
}
