using AutoMapper;
using Blog_Api.DataModel.Entities;
using Blog_Api.DataModel.Requests;
using Blog_Api.DataModel.Responses;

namespace Blog_Api.Configuration;

public class AutomapperProfile : Profile
{
    public AutomapperProfile()
    {
        CreateMap<AddPostRequestModel,Post>();
        CreateMap<Post, AddPostResponseModel>();
    }
}