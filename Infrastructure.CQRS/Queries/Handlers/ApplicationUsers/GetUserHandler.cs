using Domain.Model;
using Infrastructure.CQRS.Queries.Request.ApplicationUsers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.CQRS.Queries.Handlers.ApplicationUsers
{
    public class GetUserHandler : IRequestHandler<GetUserQuery, ApplicationUser>
    {
        private readonly UserManager<ApplicationUser> _db;

        public GetUserHandler(UserManager<ApplicationUser> db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(UserManager<ApplicationUser>));
        }

        public async Task<ApplicationUser> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            return await _db.FindByIdAsync(request.Id.ToString());
        }
    }
}
