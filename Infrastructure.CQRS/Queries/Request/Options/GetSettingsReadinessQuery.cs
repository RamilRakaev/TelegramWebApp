using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.CQRS.Queries.Request.Options
{
    public class GetSettingsReadinessQuery : IRequest<bool>
    {
    }
}
