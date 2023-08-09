using AutoMapper;
using Testing.DomainModels;
using Testing.Profiles.AfterMaps;
using Data = Testing.Models;


namespace Testing.API.Profiles
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Data.Student, Student>()
                .ReverseMap();
            CreateMap<Data.Gender, Gender>()
                .ReverseMap();
            CreateMap<Data.Address, Address>()
                .ReverseMap();
            CreateMap<UpdateStudentRequest, Data.Student>()
                .AfterMap<UpdateStudentRequestAfterMap>();
            CreateMap<AddStudentRequest, Data.Student>()
                .AfterMap<AddStudentRequestAfterMap>();
        }
    }
}
