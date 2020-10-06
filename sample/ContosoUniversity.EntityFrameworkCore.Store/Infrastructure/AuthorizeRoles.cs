using Microsoft.AspNetCore.Authorization;
using System;

namespace ContosoUniversity.EntityFrameworkCore.Store.Infrastructure
{
    public class AuthorizeRoles : AuthorizeAttribute
    {
        public AuthorizeRoles(params string [] roles)
        {
            Roles = String.Join(",", roles);
        }
    }
}
