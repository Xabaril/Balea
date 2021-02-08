using Balea.Abstractions;
using Balea.Authorization.Abac.Context;
using Balea.Authorization.Abac.Grammars;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Balea.Authorization.Abac
{
    internal class AbacAuthorizationHandler : AuthorizationHandler<AbacRequirement>
    {
        private readonly AbacAuthorizationContextFactory _abacAuthorizationContextFactory;
        private readonly IRuntimeAuthorizationServerStore runtimeAuthorizationServerStore;

        public AbacAuthorizationHandler(
            AbacAuthorizationContextFactory abacAuthorizationContextFactory,
            IRuntimeAuthorizationServerStore runtimeAuthorizationServerStore)
        {
            Ensure.Argument.NotNull(abacAuthorizationContextFactory, nameof(abacAuthorizationContextFactory));
            Ensure.Argument.NotNull(runtimeAuthorizationServerStore, nameof(abacAuthorizationContextFactory));
            _abacAuthorizationContextFactory = abacAuthorizationContextFactory;
            this.runtimeAuthorizationServerStore = runtimeAuthorizationServerStore;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AbacRequirement requirement)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                var policy = await runtimeAuthorizationServerStore.GetPolicyAsync(requirement.Name);
                
                if (policy is null)
                {
                    context.Fail();
                    return;
                }

                var abacContext = await  _abacAuthorizationContextFactory.Create(context);
                var abacPolicy = AbacAuthorizationPolicy.CreateFromGrammar(policy.Content, WellKnownGrammars.Bal);

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
