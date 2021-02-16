using Balea.Abstractions;
using Balea.Authorization.Abac.Context;
using Balea.Authorization.Abac.Grammars;
using Balea.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Balea.Authorization.Abac
{
    internal class AbacAuthorizationHandler : AuthorizationHandler<AbacRequirement>
    {
        private readonly AbacAuthorizationContextFactory _abacAuthorizationContextFactory;
        private readonly IRuntimeAuthorizationServerStore _runtimeAuthorizationServerStore;
        private readonly ILogger<AbacAuthorizationHandler> _logger;

        public AbacAuthorizationHandler(
            AbacAuthorizationContextFactory abacAuthorizationContextFactory,
            IRuntimeAuthorizationServerStore runtimeAuthorizationServerStore,
            ILogger<AbacAuthorizationHandler> logger)
        {
            Ensure.Argument.NotNull(abacAuthorizationContextFactory, nameof(abacAuthorizationContextFactory));
            Ensure.Argument.NotNull(runtimeAuthorizationServerStore, nameof(abacAuthorizationContextFactory));
            Ensure.Argument.NotNull(logger, nameof(logger));
            _abacAuthorizationContextFactory = abacAuthorizationContextFactory;
            _runtimeAuthorizationServerStore = runtimeAuthorizationServerStore;
            _logger = logger;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AbacRequirement requirement)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                try
                {
                    var policy = await _runtimeAuthorizationServerStore.GetPolicyAsync(requirement.Name);

                    if (policy is object)
                    {
                        Log.AbacAuthorizationHandlerIsEvaluatingPolicy(_logger, policy.Name, policy.Content);

                        var abacContext = await _abacAuthorizationContextFactory.Create(context);
                        var abacPolicy = AbacAuthorizationPolicy.CreateFromGrammar(policy.Content, WellKnownGrammars.Bal);

                        if (abacPolicy.IsSatisfied(abacContext))
                        {
                            context.Succeed(requirement);
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.AbacAuthorizationHandlerThrow(_logger, ex);
                }
            }

            context.Fail();
        }
    }
}
