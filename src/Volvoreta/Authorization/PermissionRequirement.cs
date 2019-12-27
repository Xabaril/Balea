using Microsoft.AspNetCore.Authorization;
using System;

namespace Volvoreta.Authorization
{
    internal class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement(string name)
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