using AutoMapper;
using DeveloperStore.Sales.Application.Commands.Sales.Create;
using DeveloperStore.Sales.Application.Commands.Sales.Update;
using DeveloperStore.Sales.Application.DTOs;
using DeveloperStore.Sales.Application.Models;
using DeveloperStore.Sales.Domain.Entities;
using DeveloperStore.Sales.Domain.Models;

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
        
        CreateMap<UpdateSaleCommand, Sale>(MemberList.Destination)
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
            .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => src.Branch))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Products))
            .ReverseMap();

        CreateMap<CreateSaleCommand, SalesDto>(MemberList.Destination)
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CustomerId))
            .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => src.Branch))
            .ReverseMap();

        CreateMap<UpdateSaleCommand, SalesDto>(MemberList.Destination)
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CustomerId))
            .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => src.Branch))
            .ReverseMap();

        CreateMap<ProductRequest, ProductDto>(MemberList.Destination)
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ReverseMap();
       
        CreateMap<UpdateProductRequest, ProductDto>(MemberList.Destination)
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ReverseMap();

        CreateMap<ProductRequest, Item>(MemberList.Destination)
            .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ReverseMap();

        CreateMap<UpdateProductRequest, Item>(MemberList.Destination)
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ReverseMap();
        #endregion

        #region Get

        CreateMap<Sale, SalesModel>(MemberList.Destination)
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => new Customer { Id = src.CustomerId }))
            .ForMember(dest => dest.SalesDate, opt => opt.MapFrom(src => src.SalesDate))
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Items))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
            .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => src.Branch))
            .ForMember(dest => dest.IsCancelled, opt => opt.MapFrom(src => src.IsCancelled))
            .AfterMap((src, dest) =>
            {
                if (dest.Customer == null)
                {
                    dest.Customer = new();
                }

                dest.Customer.Id = src.CustomerId;
            })
            .ReverseMap();

        CreateMap<SalesModel, SalesDto>(MemberList.Destination)
            .ReverseMap();

        CreateMap<CustomerModel, CustomerDto>(MemberList.Destination)
            .ReverseMap();

        CreateMap<CustomerModel, Customer>(MemberList.Destination)
             .AfterMap((src, dest) =>
             {
                 if (dest.Name == null)
                 {
                     dest.Name = new Name();
                 }

                 dest.Name.Firstname = src.FirstName;
                 dest.Name.Lastname = src.LastName;
             });

        CreateMap<Customer, CustomerModel>(MemberList.Destination)
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Name.Firstname))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Name.Lastname))
            .ReverseMap();

        CreateMap<ProductModel, ProductDto>(MemberList.Destination)
            .ReverseMap();

        CreateMap<ProductModel, Product>(MemberList.Destination)
            .ReverseMap();

        CreateMap<Item, ProductModel>(MemberList.Destination)
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ItemId))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
            .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.Discount))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
            .ForMember(dest => dest.IsCancelled, opt => opt.MapFrom(src => src.IsCancelled))
            .ReverseMap();

        CreateMap<Product, Item>(MemberList.Destination)
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.Price))
            .ReverseMap();

        #endregion
    }
}
