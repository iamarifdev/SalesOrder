using AutoMapper;
using SalesOrder.Common.DTO.Element;
using SalesOrder.Common.DTO.Order;
using SalesOrder.Common.DTO.Window;
using SalesOrder.Common.Models;
using SalesOrder.Data.Models;

namespace SalesOrder.API.AutoMappingProfiles;

public class ModelMappingProfile : Profile
{
    public ModelMappingProfile()
    {
        CreateMap<PaginatedList<Order>, PaginatedList<OrderDto>>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
        CreateMap<OrderCreateDto, Order>()
            .ForMember(dest => dest.Windows, opt => opt.MapFrom(src => src.Windows))
            .ReverseMap();
        CreateMap<OrderUpdateDto, Order>().ReverseMap();
        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.Windows, opt => opt.MapFrom(src => src.Windows))
            .ReverseMap();


        CreateMap<PaginatedList<Window>, PaginatedList<WindowDto>>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
        CreateMap<WindowCreateDto, Window>()
            .ForMember(dest => dest.SubElements, opt => opt.MapFrom(src => src.SubElements))
            .ForMember(dest => dest.TotalSubElements, opt => opt.MapFrom(src => src.SubElements.Count))
            .ReverseMap();
        CreateMap<WindowUpdateDto, Window>().ReverseMap();
        CreateMap<Window, WindowDto>()
            .ForMember(dest => dest.SubElements, opt => opt.MapFrom(src => src.SubElements))
            .ReverseMap();

        CreateMap<PaginatedList<SubElement>, PaginatedList<SubElementDto>>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
        CreateMap<SubElementCreateDto, SubElement>().ReverseMap();
        CreateMap<SubElementUpdateDto, SubElement>().ReverseMap();
        CreateMap<SubElementDto, SubElement>().ReverseMap();
    }
}