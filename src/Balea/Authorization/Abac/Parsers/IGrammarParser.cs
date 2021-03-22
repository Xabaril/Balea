using Balea.Authorization.Abac.Context;
using Balea.Authorization.Abac.Grammars;
using System;
using System.Collections.Generic;

namespace Balea.Authorization.Abac.Parsers
{

    interface IGrammarParser
    {
        bool CanParse(WellKnownGrammars grammar);
        AbacAuthorizationPolicy Parse(string policy);
    }

    internal static class DefaultParser
    {
        private static List<IGrammarParser> _parsers = new List<IGrammarParser>()
        {
            new BALParser()
        };

        public static AbacAuthorizationPolicy Parse(string policy, WellKnownGrammars grammar)
        {
            foreach (var parser in _parsers)
            {
                if (parser.CanParse(grammar))
                {
                    return parser.Parse(policy);
                }
            }

            throw new InvalidOperationException($"The grammar {Enum.GetName(typeof(WellKnownGrammars), grammar)} does not contain any parser registered.");
        }
    }
}
