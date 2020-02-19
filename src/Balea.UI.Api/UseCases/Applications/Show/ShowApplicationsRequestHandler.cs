using Balea.EntityFrameworkCore.Store.DbContexts;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Balea.UI.Api.UseCases.Applications.Show
{
    public class ShowApplicationsRequestHandler : IRequestHandler<ShowApplicationsRequest, ShowApplicationsResponse>
    {
        private readonly StoreDbContext dbContext;

        public ShowApplicationsRequestHandler(StoreDbContext dbContext)
        {
            Ensure.Argument.NotNull(dbContext, nameof(dbContext));
            this.dbContext = dbContext;
        }

        public Task<ShowApplicationsResponse> Handle(ShowApplicationsRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
