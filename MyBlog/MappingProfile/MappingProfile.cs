using AutoMapper;
using WEB.Models.User;
using System;
using System.Collections.Generic;
using System.Text;
using Core.Entity;
using Core.Models.User;
using Core.Models.Post;

namespace BLL.MappingProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<(User s1, UserProfile s2), EditViewModel>()
                .ForMember(dst => dst.Id, option => option.MapFrom(src => src.s1.Id))
                .ForMember(dst => dst.Username, option => option.MapFrom(src => src.s1.Login))
                .ForMember(dst => dst.FirstName, option => option.MapFrom(src => src.s2.FirstName))
                .ForMember(dst => dst.LastName, option => option.MapFrom(src => src.s2.LastName))
                .ForMember(dst => dst.Picture, option => option.MapFrom(src => src.s2.Picture)).ReverseMap();

            CreateMap<Post, PostViewModel>()
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.Text, opt => opt.MapFrom(src => src.Text))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();
        }
    }
}
