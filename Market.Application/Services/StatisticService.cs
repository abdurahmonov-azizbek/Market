using Market.Application.Interfaces;
using Market.Domain.Entities;
using Market.Domain.Exceptions;
using Market.Domain.Models;
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

        var products = new List<ProductCount>();
        foreach (var order in orders)
        {
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
            Total = orders.Sum(order => order.Price),
            Products = products
        };
    }

    public async ValueTask<OrderStatistics> Get(Guid userId)
    {
        var orders = (await orderService.GetAll(userId)).ToList();

        var products = new List<ProductCount>();
        foreach (var order in orders)
        {
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
            Total = orders.Sum(order => order.Price),
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
        foreach (var order in orders)
        {
            var product = await productService.GetByIdAsync(order.ProductId)
                ?? throw new EntityNotFoundException(typeof(Product));

            profit += product.SalePrice - product.IncomingPrice;
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
            Total = orders.Sum(order => order.Price),
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
        foreach (var order in orders)
        {
            var product = await productService.GetByIdAsync(order.ProductId)
                ?? throw new EntityNotFoundException(typeof(Product));

            profit += product.SalePrice - product.IncomingPrice;
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
            Total = orders.Sum(order => order.Price),
            Profit = profit,
            Debts = debtsSum,
            Costs = costsSum
        };
    }
}
