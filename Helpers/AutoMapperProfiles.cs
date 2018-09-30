using AutoMapper;
using JobTracker.API.Dtos;
using JobTracker.API.Models;

namespace JobTracker.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UserForRegisterDto, User>();
            CreateMap<Brand, BrandForDetailedDto>();
            CreateMap<Job, JobDetailedDto>();
            CreateMap<JobForCreationDto, Job>();
            CreateMap<JobForUpdateDto, Job>();
            CreateMap<Job, JobForUpdateDto>();
            CreateMap<User, UserForAuthenticationDto>();
            CreateMap<Job, JobToEditDto>();
            CreateMap<MessageForCreationDto, Message>().ReverseMap();
            CreateMap<Message, MessageToReturnDto>();
            CreateMap<User, UserForDetailedDto>();
            CreateMap<PhotoForCreationDto, Photo>();
            CreateMap<Photo, PhotoToReturnDto>();
            
        } 
    }
}