using Collectio.Infra.CrossCutting.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace Collectio.Presentation.Filters
{
    public class CustomAsyncActionFilter : IAsyncActionFilter
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomAsyncActionFilter(IUnitOfWork unitOfWork) 
            => _unitOfWork = unitOfWork;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var result = await next();
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
