using Market.Application.Interfaces;
using Market.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Market.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services
            .AddScoped<IUserService, UserService>()
            .AddScoped<ICategoryService, CategoryService>()
            .AddScoped<IOrderService, OrderService>()
            .AddScoped<IProductService, ProductService>()
            .AddScoped<IPasswordHasherService, PasswordHasherService>()
            .AddScoped<ITokenService, TokenService>()
            .AddScoped<IAuthService, AuthService>()
            .AddScoped<IAccountService, AccountService>()
            .AddScoped<IDebtService, DebtService>()
            .AddScoped<IReturnedProductService, ReturnedProductService>()
            .AddScoped<IStatisticService, StatisticService>();

        return services;
    }
}
