using AutoMapper;
using WebApplication1.Data;
using WebApplication1.Model;

namespace WebApplication1.AutoMapperConfigurations
{
    public class AutoMapperConfig:Profile
    {
        public AutoMapperConfig() {
            // This will create to and fro mapping between StudentDTO and Student
            CreateMap<StudentDTO, Student>()
            .ReverseMap()
            .ForMember(
            dest => dest.address,
            opt => opt.MapFrom(src => string.IsNullOrEmpty(src.address) ? "No Address" : src.address))
            .AddTransform<DateTime>(n=>n.Date); 
        }
    }
}
