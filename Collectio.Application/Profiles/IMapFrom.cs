using AutoMapper;

namespace Collectio.Application.Profiles
{
    public interface IMapFrom<T>
    {
        void Mapping(Profile profile) 
            => profile.CreateMap(typeof(T), GetType());
    }

    public interface IMapTo<T>
    {
        void Mapping(Profile profile);
    }
}