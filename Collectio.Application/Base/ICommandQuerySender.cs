﻿using Collectio.Application.Base.Commands;
using Collectio.Application.Base.Queries;
using System.Threading.Tasks;
using MediatR;

namespace Collectio.Application.Base
{
    public interface ICommandQuerySender
    {
        Task<R> Send<R>(ICommand<R> command);
        Task<QueryResult<R>> Send<Q, R>(Q query) 
            where Q : Query<R>
            where R : class;
    }
}