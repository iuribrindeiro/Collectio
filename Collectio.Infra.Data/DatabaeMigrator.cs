using Collectio.Infra.CrossCutting.Services;
using Microsoft.EntityFrameworkCore;

namespace Collectio.Infra.Data
{
    public class DatabaeMigrator : IDatabaseMigrator
    {
        private readonly ApplicationContext _applicationContext;

        public DatabaeMigrator(ApplicationContext applicationContext) 
            => _applicationContext = applicationContext;

        public void Migrate() 
            => _applicationContext.Database.Migrate();
    }
}