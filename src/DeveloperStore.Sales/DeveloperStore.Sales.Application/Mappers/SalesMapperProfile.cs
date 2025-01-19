using AutoMapper;
using DeveloperStore.Sales.Application.Commands.Sales.Create;
using DeveloperStore.Sales.Application.DTOs;
using DeveloperStore.Sales.Domain.Entities;

namespace DeveloperStore.Sales.Application.Mappers;

public class SalesMapperProfile : Profile
{
    public SalesMapperProfile()
    {

        #region Create

        CreateMap<CreateSaleCommand, Sale>(MemberList.Destination)
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
            .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => src.Branch))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Products))
            .ReverseMap();

        CreateMap<CreateSaleCommand, SalesDto>(MemberList.Destination)
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CustomerId))
            .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => src.Branch))
            .ReverseMap();

        CreateMap<ProductRequest, ProductDto>(MemberList.Destination)
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ReverseMap();

        CreateMap<ProductRequest, Item>(MemberList.Destination)
            .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ReverseMap();
        #endregion

        #region Get

        CreateMap<Sale, SalesDto>(MemberList.Destination)
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.SalesDate, opt => opt.MapFrom(src => src.SalesDate))
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Items))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
            .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => src.Branch))
            .ForMember(dest => dest.IsCancelled, opt => opt.MapFrom(src => src.IsCancelled))
            .ReverseMap();

        CreateMap<Item, ProductDto>(MemberList.Destination)
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ItemId))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
            .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.Discount))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
            .ForMember(dest => dest.IsCancelled, opt => opt.MapFrom(src => src.IsCancelled))
            .ReverseMap();

        #endregion
    }
}
