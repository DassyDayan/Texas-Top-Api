using Pickpong.Entities;
using Pickpong.Models;

namespace Pickpong.Dto.Classes
{
    public class AutoMapperProfile : AutoMapper.Profile
    {

        public AutoMapperProfile()
        {
            CreateMap<TboardSetting, SizeOptionsDto>()
                 .ForMember(dest => dest.idSize, opt => opt.MapFrom(src => src.IIdSize))
                 .ForMember(dest => dest.idShape, opt => opt.MapFrom(src => src.IIdShape))
                 .ForMember(dest => dest.parameterA, opt => opt.MapFrom(src => src.DLength))
                 .ForMember(dest => dest.parameterB, opt => opt.MapFrom(src => src.DHeight))
                 .ForMember(dest => dest.price, opt => opt.MapFrom(src => src.DPrice))
                 .ForMember(dest => dest.maxPlayers, opt => opt.MapFrom(src => src.IMaxPlayers));

            CreateMap<Tcustomize, CustomizeSettingDTO>()
                .ForMember(dest => dest.IIdSize, opt => opt.MapFrom(src => src.IIdCost));

            CreateMap<TcartDetail, CartDetails>();

            CreateMap<carpetDetailModel, TcarpetDetail>();

            CreateMap<newOrderModel, TcustomerDetail>();
            CreateMap<newOrderModel, Torder>();

            CreateMap<Settings, TboardSetting>();

            CreateMap<SettingOfCustomize, Tcustomize>();

        }
    }
}