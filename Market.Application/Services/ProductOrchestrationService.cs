using Market.Application.Interfaces;
using Market.Domain.Entities;
using Market.Domain.Enums;
using Market.Domain.Exceptions;
using Market.Domain.Extensions;
using Market.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Market.Application.Services
{
    public class ProductOrchestrationService(
        IHttpContextAccessor httpContextAccessor,
        IProductItemService productItemService,
        IUserService userService,
        IProductService productService) : IProductOrchestrationService
    {
        private HttpContext httpContext = httpContextAccessor.HttpContext
            ?? throw new ArgumentException("Http context accessor can not be null!");

        public async ValueTask<FullProduct> GetByCodeAsync(long code)
        {
            var userRole = httpContext.GetValueByClaimType(nameof(Role));
            var userId = Guid.Parse(httpContext.GetValueByClaimType(nameof(User.Id)));
            var user = await userService.GetByIdAsync(userId);
            var idForSearchingProductItems =
                userRole == nameof(Role.SuperAdmin)
                ? user.Id
                : user.CreatedBy;

            var productItems = await productItemService.GetAll(idForSearchingProductItems);

            var productItem = productItems.FirstOrDefault(productItem =>
                productItem.Code == code)
                    ?? throw new EntityNotFoundException(typeof(ProductItem));

            var product = await productService.GetByIdAsync(productItem.ProductId);

            return new FullProduct
            {
                Product = product,
                ProductItem = productItem,
            };
        }

        public async ValueTask<List<FullProduct>> GetByKeyAsync(string key)
        {
            var userId = Guid.Parse(httpContext.GetValueByClaimType(nameof(User.Id)));
            var user = await userService.GetByIdAsync(userId);
            var idForSearching = user.Role == Role.SuperAdmin
                ? user.Id : user.CreatedBy;

            var productItems = await productItemService.GetAll(idForSearching);

            var foundProductItems = productItems.ToList()
                .Where(productItem =>
                {
                    if (string.IsNullOrWhiteSpace(productItem.Title))
                        return false;

                    if (productItem.Title.Contains(key, StringComparison.OrdinalIgnoreCase))
                        return false;

                    return true;
                });

            var fullProducts = new List<FullProduct>();

            foreach (var productItem in foundProductItems)
            {
                var product = await productService.GetByIdAsync(productItem.ProductId);

                fullProducts.Add(new FullProduct
                {
                    Product = product,
                    ProductItem = productItem,
                });
            }

            return fullProducts;
        }
    }
}
