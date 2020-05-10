namespace Collectio.Infra.CrossCutting.Services
{
    public interface IDatabaseMigrator
    {
        void Migrate();
    }
}