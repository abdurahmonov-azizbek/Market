using Market.Application.Interfaces;
using Market.Domain.Entities;
using Market.Domain.Exceptions;
using Market.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Data;

namespace Market.Application.Services;

public class StatisticService(
    IOrderService orderService,
    IUserService userService,
    IProductService productService,
    IDebtService debtService) : IStatisticService
{
    public async ValueTask<OrderStatistics> Get(Guid userId, DateTime dateTime)
    {
        var date = dateTime.Date;
        var orders = (await orderService.GetAll(userId)).ToList()
            .Where(order => order.CreatedDate.Date == date);

        var cash = default(decimal);
        var card = default(decimal);

        var products = new List<ProductCount>();
        foreach (var order in orders)
        {
            if (order.PaymentType == Domain.Enums.PaymentType.Cash)
                cash += order.Price;
            else
                card += order.Price;

            var product = await productService.GetByIdAsync(order.ProductId)
                ?? throw new EntityNotFoundException(typeof(Product));

            products.Add(new ProductCount
            {
                Product = product,
                Count = order.Count
            });
        }

        return new OrderStatistics
        {
            Count = orders.Count(),
            Total = new Total { Card = card, Cash = cash, Sum = card + cash },
            Products = products
        };
    }

    public async ValueTask<OrderStatistics> Get(Guid userId)
    {
        var orders = (await orderService.GetAll(userId)).ToList();

        var products = new List<ProductCount>();

        var cash = default(decimal);
        var card = default(decimal);

        foreach (var order in orders)
        {
            if (order.PaymentType == Domain.Enums.PaymentType.Card)
                card += order.Price;
            else
                cash += order.Price;

            var product = await productService.GetByIdAsync(order.ProductId)
                ?? throw new EntityNotFoundException(typeof(Product));

            products.Add(new ProductCount
            {
                Product = product,
                Count = order.Count
            });
        }

        return new OrderStatistics
        {
            Count = orders.Count(),
            Total = new Total { Card = card, Cash = cash, Sum = card + cash },
            Products = products
        };
    }

    public async ValueTask<Statistics> GetFull(Guid userId, DateTime dateTime)
    {
        var date = dateTime.Date;

        //get all related orders
        var orders = (await orderService.GetAll(userId)).ToList();
        var admins = (await userService.GetAllAsync(userId)).ToList();
        foreach (var admin in admins)
        {
            var adminOrders = (await orderService.GetAll(admin.Id)).ToList();
            orders.AddRange(adminOrders);
        }
        orders = orders.Where(order => order.CreatedDate.Date == date).ToList();

        var profit = default(int);

        var cash = default(decimal);
        var card = default(decimal);

        foreach (var order in orders)
        {
            if (order.PaymentType == Domain.Enums.PaymentType.Card)
                card += order.Price;
            else
                cash += order.Price;

            var product = await productService.GetByIdAsync(order.ProductId)
                ?? throw new EntityNotFoundException(typeof(Product));

            profit += (product.SalePrice - product.IncomingPrice) * order.Count;
        }

        var debts = (await debtService.GetAll(userId)).ToList()
            .Where(debt => debt.CreatedDate.Date == date);

        decimal costsSum = 0;
        decimal debtsSum = 0;

        foreach (var debt in debts)
        {
            if (debt.Type == Domain.Enums.DebtType.Company)
            {
                costsSum += debt.Price;
            }
            else
            {
                debtsSum += debt.Price;
            }
        }

        return new Statistics
        {
            Orders = orders.Count(),
            Total = new Total
            {
                Card = card,
                Cash = cash,
                Sum = card + cash
            },
            Profit = profit,
            Debts = debtsSum,
            Costs = costsSum
        };

    }

    public async ValueTask<Statistics> GetFull(Guid userId)
    {
        var orders = (await orderService.GetAll(userId)).ToList();
        var admins = (await userService.GetAllAsync(userId)).ToList();
        foreach (var admin in admins)
        {
            var adminOrders = (await orderService.GetAll(admin.Id)).ToList();
            orders.AddRange(adminOrders);
        }

        var profit = default(int);

        var cash = default(decimal);
        var card = default(decimal);


        foreach (var order in orders)
        {
            if (order.PaymentType == Domain.Enums.PaymentType.Card)
                card += order.Price;
            else
                cash += order.Price;

            var product = await productService.GetByIdAsync(order.ProductId)
                ?? throw new EntityNotFoundException(typeof(Product));

            profit += (product.SalePrice - product.IncomingPrice) * order.Count;
        }

        var debts = (await debtService.GetAll(userId)).ToList();

        decimal costsSum = 0;
        decimal debtsSum = 0;

        foreach (var debt in debts)
        {
            if (debt.Type == Domain.Enums.DebtType.Company)
            {
                costsSum += debt.Price;
            }
            else
            {
                debtsSum += debt.Price;
            }
        }

        return new Statistics
        {
            Orders = orders.Count(),
            Total = new Total
            {
                Cash = cash,
                Card = card,
                Sum = cash + card
            },
            Profit = profit,
            Debts = debtsSum,
            Costs = costsSum
        };
    }
}
