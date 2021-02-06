using AutoMapper;
using Domain.DTO;
using Domain.Entities;
using Domain.Views;

namespace Application.AutoMapper
{
    public class DataMappingProfile : Profile
    {
        public DataMappingProfile()
        {
            CreateMap<Lawsuit, LawsuitDTO>().ReverseMap();
            CreateMap<Lawsuit, LawsuitView>().ReverseMap();
        }
    }
}
