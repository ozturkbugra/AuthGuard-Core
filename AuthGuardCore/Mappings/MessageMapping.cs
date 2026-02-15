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

            CreateMap<Message, MessageWithReceiverInfoViewModel>()
               .ForMember(dest => dest.ReceiverName,
                   opt => opt.MapFrom(src => src.Receiver.Name))
               .ForMember(dest => dest.ReceiverSurname,
                   opt => opt.MapFrom(src => src.Receiver.Surname))
               .ForMember(dest => dest.CategoryName,
                   opt => opt.MapFrom(src => src.Category.Name));

        }
    }
}
