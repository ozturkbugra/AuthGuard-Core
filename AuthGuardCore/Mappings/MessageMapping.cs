using AuthGuardCore.Entities;
using AuthGuardCore.Models;
using AutoMapper;

namespace AuthGuardCore.Mappings
{
    public class MessageMapping: Profile
    {
        public MessageMapping() {

            CreateMap<Message, MessageWithSenderInfoViewModel>()
               .ForMember(dest => dest.SenderName,
                   opt => opt.MapFrom(src => src.Sender.Name))
               .ForMember(dest => dest.SenderSurname,
                   opt => opt.MapFrom(src => src.Sender.Surname))
               .ForMember(dest => dest.CategoryName,
                   opt => opt.MapFrom(src => src.Category.Name));
        }
    }
}
