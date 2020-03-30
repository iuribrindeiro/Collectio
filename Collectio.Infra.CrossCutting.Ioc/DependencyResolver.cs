using AutoMapper;
using Collectio.Application.Base;
using Collectio.Application.Base.Commands;
using Collectio.Application.Profiles;
using Collectio.Infra.CrossCutting.Services.Interfaces;
using Collectio.Infra.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Collectio.Infra.CrossCutting.Ioc
{
    public static class DependencyResolver
    {
        public static void RegisterDependencies(this IServiceCollection serviceCollectio, IConfiguration configuration)
        {
            serviceCollectio.AddMediatR(typeof(AbstractCommandHandler<,>));
            serviceCollectio.AddAutoMapper(e => e.DisableConstructorMapping(), Assembly.GetAssembly(typeof(ClienteProfile)));
            serviceCollectio.AddScoped<ICommandQuerySender, CommandQuerySender>();
            serviceCollectio.AddDbContext<ApplicationContext>(e => e.UseSqlServer(configuration.GetConnectionString("Default")));
            serviceCollectio.AddScoped<IUnitOfWork, ApplicationContext>(e => e.GetService<ApplicationContext>());
        }
    }
}
