using AutoMapper;
using Bib.Application.Features.Publisher.Command.CreatePublisher;
using Bib.Application.Features.Publisher.Queries.Common;
using Bib.Domain.Entities;
using System;

namespace Bib.Application.Common.Mappings
{
    public class PublisherProfile : Profile
    {
        public PublisherProfile()
        {
            CreateMap<CreatePublisherCommand, Publisher>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Site, opt => opt.MapFrom(src => src.Site))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));


            CreateMap<Publisher, PublisherDto>();

            CreateMap<Publisher, Publisher>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                    srcMember != null && !srcMember.Equals(dest)));
        }
    }
}
