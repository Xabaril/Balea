using Balea.UI.Api;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomProblemDetails(this IServiceCollection services) =>
            services
                .AddProblemDetails(setup =>
                {
                    setup.Map<InvalidOperationException>(exception =>
                    {
                        return new ProblemDetails()
                        {
                            Detail = exception.Message,
                            Status = StatusCodes.Status400BadRequest
                        };
                    });
                })
                .Configure<ApiBehaviorOptions>(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Instance = context.HttpContext.Request.Path,
                            Status = StatusCodes.Status400BadRequest,
                            Type = $"https://httpstatuses.com/400",
                            Detail = Constants.Messages.ModelStateValidation
                        };

                        return new BadRequestObjectResult(problemDetails)
                        {
                            ContentTypes =
                            {
                                Constants.ContentTypes.ProblemJson,
                                Constants.ContentTypes.ProblemXml
                            }
                        };
                    };
                });
    }
}
