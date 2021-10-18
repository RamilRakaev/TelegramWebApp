using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Model;
using Infrastructure.CQRS.Queries.Request.ApplicationUsers;

namespace Infrastructure.CQRS.Queries.Handlers.ApplicationUsers
{
    public class GetUsersHandler : IRequestHandler<GetUsersQuery, ApplicationUser[]>
    {
        private readonly UserManager<ApplicationUser> _db;

        public GetUsersHandler(UserManager<ApplicationUser> db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(UserManager<ApplicationUser>));
        }

        public async Task<ApplicationUser[]> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = _db.Users.Include(u => u.Role).AsNoTracking();
            return await users.ToArrayAsync();
        }
    }
}
