using Market.Application.Interfaces;
using Market.Domain.Entities;
using Market.Domain.Exceptions;
using Market.Domain.Models;
using System.Data;

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

        //add current user's orders to orders collection
        var currentUsersOrders = await orderService.GetAll(userId);
        foreach (var order in currentUsersOrders)
        {
            if (order.CreatedDate.Date == date)
                orders.Add(order);
        }

        //add users' admins' orders
        foreach (var admin in admins)
        {
            var adminOrders = await orderService.GetAll(admin.Id);
            foreach (var order in orders)
            {
                if (order.CreatedDate.Date == date)
                {
                    orders.Add(order);
                }
            }
        }

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

        var debts = await debtService.GetAll(userId);
        var foundDebts = debts.ToList().Where(debt =>
        {
            if (debt.CreatedDate.Year == date.Year &&
                debt.CreatedDate.Month == date.Month &&
                debt.CreatedDate.Day == date.Day)
            {
                return true;
            }

            return false;
        });

        int costs = 0;
        int debtsB = 0;

        foreach (var debt in foundDebts)
        {
            if (debt is null)
                continue;

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
