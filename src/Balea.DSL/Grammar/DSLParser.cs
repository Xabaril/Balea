using Balea.DSL.Grammar.Bal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Balea.DSL.Grammar
{
    internal static class DSLParser
    {
        private static List<IDSLParser> _parsers = new List<IDSLParser>()
        {
            new BalDSLParser()
        };

        public static DslAuthorizationPolicy Parse(string policy, Grammars grammar)
        {
            foreach (var parser in _parsers)
            {
                if (parser.CanParse(grammar))
                {
                    return parser.Parse(policy);
                }
            }

            throw new InvalidOperationException($"The grammar {Enum.GetName(typeof(Grammars), grammar)} does not contain any parser registered.");
        }
    }
}
