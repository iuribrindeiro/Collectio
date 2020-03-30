using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;
using Collectio.Infra.CrossCutting.Services.Interfaces;

namespace Collectio.Presentation.Filters
{
    public class CustomAsyncActionFilter : IAsyncActionFilter
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomAsyncActionFilter(IUnitOfWork unitOfWork) 
            => _unitOfWork = unitOfWork;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await next();
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
