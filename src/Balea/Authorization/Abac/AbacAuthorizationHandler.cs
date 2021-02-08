using Balea.Authorization.Abac.Context;
using Balea.Authorization.Abac.Grammars;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Balea.Authorization.Abac
{
    internal class AbacAuthorizationHandler : AuthorizationHandler<AbacRequirement>
    {
        private readonly AbacAuthorizationContextFactory _abacAuthorizationContextFactory;

        public AbacAuthorizationHandler(AbacAuthorizationContextFactory abacAuthorizationContextFactory)
        {
            Ensure.Argument.NotNull(abacAuthorizationContextFactory, nameof(abacAuthorizationContextFactory));
            _abacAuthorizationContextFactory = abacAuthorizationContextFactory;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AbacRequirement requirement)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                //evaluate policy
                var abacContext = await  _abacAuthorizationContextFactory.Create(context);
                var abacPolicy = AbacAuthorizationPolicy.CreateFromGrammar(
@"policy example begin
    rule A (PERMIT) begin
        Subject.Role CONTAINS ""customer"" AND Resource.Controller = ""Grades"" AND Parameters.value > 6  
    end
end", WellKnownGrammars.Bal);

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
