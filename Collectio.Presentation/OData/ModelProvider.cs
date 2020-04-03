using Collectio.Application.ViewModels;
using Microsoft.AspNet.OData.Builder;
using Microsoft.OData.Edm;

namespace Collectio.Presentation.OData
{
    public class ModelProvider
    {
        public static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<ClienteViewModel>("Clientes");
            return builder.GetEdmModel();
        }
    }
}
