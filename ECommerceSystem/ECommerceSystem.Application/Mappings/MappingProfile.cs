using AutoMapper;
using ECommerceSystem.Application.DTOs.Cart;
using ECommerceSystem.Application.DTOs.Category;
using ECommerceSystem.Application.DTOs.Order;
using ECommerceSystem.Application.DTOs.Product;
using ECommerceSystem.Application.DTOs.Review;
using ECommerceSystem.Application.DTOs.User;
using ECommerceSystem.Domain.Entities;

namespace ECommerceSystem.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User
            CreateMap<User, UserResponse>();

            // Category
            CreateMap<Category, CategoryResponse>();
            CreateMap<CreateCategoryRequest, Category>();
            CreateMap<UpdateCategoryRequest, Category>();

            // Product
            CreateMap<Product, ProductResponse>()
                .ForMember(dest => dest.SellerEmail, opt => opt.MapFrom(src => src.Seller != null ? src.Seller.Email : string.Empty))
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories));
            CreateMap<CreateProductRequest, Product>();
            CreateMap<UpdateProductRequest, Product>();

            // Review
            CreateMap<Review, ReviewResponse>();
            CreateMap<CreateReviewRequest, Review>();
            CreateMap<UpdateReviewRequest, Review>();

            // Cart
            CreateMap<CartItem, CartItemResponse>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.Subtotal, opt => opt.MapFrom(src => src.Product.Price * src.Quantity));
            CreateMap<AddCartItemRequest, CartItem>();

            // Order
            CreateMap<Order, OrderResponse>();
            CreateMap<OrderItem, OrderItemResponse>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));
            
            // CreateOrderItemRequest maps to OrderItem
            CreateMap<CreateOrderItemRequest, OrderItem>();
        }
    }
}
