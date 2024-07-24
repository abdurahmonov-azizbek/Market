using Market.Application.Interfaces;
using Market.Domain.Entities;
using Market.Domain.Exceptions;
using Market.Domain.Models;

namespace Market.Application.Services;

public class StatisticService(
    IOrderService orderService,
    IUserService userService,
    IProductItemService productItemService,
    IProductService productService,
    IDebtService debtService) : IStatisticService
{
    public async ValueTask<Statistics> Get(Guid userId, DateTime dateTime)
    {
        var date = dateTime.Date;

        var admins = await userService.GetAllAsync(userId);

        var orders = new List<Order>();

        foreach (var admin in admins)
            orders.AddRange((await orderService.GetAll(admin.Id)).Where(x => x.CreatedDate.Date == date));

        orders.AddRange((await orderService.GetAll(userId)).Where(x => x.CreatedDate.Date == date));

        var ordersCount = orders.Count();

        int total = 0;
        int profit = 0;

        foreach (var order in orders)
        {
            var productItem = await productItemService.GetByIdAsync(order.ProductItemId)
                ?? throw new EntityNotFoundException(typeof(ProductItem));

            var product = await productService.GetByIdAsync(productItem.ProductId)
                ?? throw new EntityNotFoundException(typeof(Product));

            total += product.SalePrice;
            profit += product.SalePrice - product.IncomingPrice;
        }

        var debts = (await debtService.GetAll(userId))
            .Where(d => d.CreatedDate.Date == date);

        int costs = 0;
        int debtsB = 0;

        foreach (var debt in debts)
        {
            if (debt.Type == Domain.Enums.DebtType.Company)
            {
                costs += Convert.ToInt32(debt.Price);
            }
            else
            {
                debtsB += Convert.ToInt32(debt.Price);
            }
        }

        return new Statistics
        {
            Orders = ordersCount,
            Total = total,
            Profit = profit,
            Debts = debtsB,
            Costs = costs
        };
    }
}
