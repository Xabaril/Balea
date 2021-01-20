namespace Balea.DSL.Grammar
{
    public interface IDSLParser
    {
        bool CanParse(Grammars grammar);

        DslAuthorizationPolicy Parse(string policy);
    }
}
