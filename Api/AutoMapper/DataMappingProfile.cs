using AutoMapper;
using Domain.DTO;
using Domain.Entities;

namespace Application.AutoMapper
{
    public class DataMappingProfile : Profile
    {
        public DataMappingProfile()
        {
          
            CreateMap<Lawsuit, LawsuitDTO>().ReverseMap();

        }
    }
}
