using AutoMapper;
using Collectio.Application.Base;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Collectio.Application.Base.Commands;
using Collectio.Application.Profiles;

namespace Collectio.Infra.CrossCutting.Ioc
{
    public static class DependencyResolver
    {
        public static void RegisterDependencies(this IServiceCollection serviceCollectio)
        {
            serviceCollectio.AddMediatR(typeof(AbstractCommandHandler<,>));
            serviceCollectio.AddAutoMapper(e => e.DisableConstructorMapping(), Assembly.GetAssembly(typeof(ClienteProfile)));
            serviceCollectio.AddScoped<ICommandQuerySender, CommandQuerySender>();
        }
    }
}
