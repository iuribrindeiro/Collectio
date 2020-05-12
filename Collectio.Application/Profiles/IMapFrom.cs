using AutoMapper;

namespace Collectio.Application.Profiles
{
    public interface IMapFrom<T>
    {
        void Mapping(Profile profile);
    }
}