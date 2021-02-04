using Microsoft.AspNetCore.Authorization;
using System;

namespace Balea.Authorization
{
    internal class AbacPolicyRequirement : IAuthorizationRequirement
    {
        public AbacPolicyRequirement(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(nameof(name));
            }

            Name = name;
        }

        public string Name { get; }
    }
}