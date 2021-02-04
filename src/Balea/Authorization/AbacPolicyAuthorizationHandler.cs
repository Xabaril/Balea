using Balea.DSL;
using Balea.DSL.Grammar;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Balea.Authorization
{
    internal class AbacPolicyAuthorizationHandler : AuthorizationHandler<AbacPolicyRequirement>
    {
        private readonly AbacAuthorizationContextFactory _abacAuthorizationContextFactory;

        public AbacPolicyAuthorizationHandler(AbacAuthorizationContextFactory abacAuthorizationContextFactory)
        {
            Ensure.Argument.NotNull(abacAuthorizationContextFactory, nameof(abacAuthorizationContextFactory));
            _abacAuthorizationContextFactory = abacAuthorizationContextFactory;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AbacPolicyRequirement requirement)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                //evaluate policy
                var abacContext = await  _abacAuthorizationContextFactory.Create(context);
                var abacPolicy = AbacAuthorizationPolicy.CreateFromGrammar(
@"policy example begin
    rule A (PERMIT) begin
        Subject.Role = ""customer"" AND Resource.Controller = ""Grades"" AND Parameters.Tenant = ""tenant1""    
    end
end", AllowedGrammars.Bal);

                if ( abacPolicy.IsSatisfied(abacContext))
                {
                    context.Succeed(requirement);
                    return;
                }
            }

            context.Fail();
        }
    }
}
