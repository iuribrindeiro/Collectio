﻿using System.Threading;
using System.Threading.Tasks;

namespace Collectio.Infra.CrossCutting.Services.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
