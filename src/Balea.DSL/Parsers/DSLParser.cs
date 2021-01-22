using Balea.DSL.Grammar;
using Balea.DSL.Parsers.Bal;
using System;
using System.Collections.Generic;

namespace Balea.DSL.Parsers
{
    internal static class DSLParser
    {
        private static List<IDSLParser> _parsers = new List<IDSLParser>()
        {
            new BalDSLParser()
        };

        public static DslAuthorizationPolicy Parse(string policy, AllowedGrammars grammar)
        {
            foreach (var parser in _parsers)
            {
                if (parser.CanParse(grammar))
                {
                    return parser.Parse(policy);
                }
            }

            throw new InvalidOperationException($"The grammar {Enum.GetName(typeof(AllowedGrammars), grammar)} does not contain any parser registered.");
        }
    }
}
