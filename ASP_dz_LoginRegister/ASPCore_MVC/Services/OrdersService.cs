using ASPCore_MVC.DbContext;
using ASPCore_MVC.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPCore_MVC.Services
{
    public interface IOrdersService
    {
        public Task<Order?> Create(Order order);
        public Task<IEnumerable<Order>?> Read();
        public Task<Order?> Read(int id);
        public Task<Order?> ReadByProductId(int id);
        public Task<Order?> ReadByUserId(string id);
        public Task<Order?> Update(Order order);
        public Task<bool> Delete(int id);

    }
    public class OrdersService : IOrdersService
    {
        private readonly OrdersContext? Context;

        public OrdersService(OrdersContext context)
        {
            Context = context;
        }
        public async Task<Order?> Create(Order order)
        {
            await Context.Orders.AddAsync(order);
            await Context?.SaveChangesAsync();
            foreach(var p in Context.Orders)
            {
                Console.WriteLine($"Buyer: {p.UserId} Product: {p.ProductId} Count: {p.Count}");
            }
            return order;
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                Context?.Orders?.Remove(Context?.Orders?.Find(id));
                await Context?.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<Order>?> Read()
        {
            return await Context?.Orders?.ToListAsync();
        }

        public async Task<Order?> Read(int id)
        {
            return await Context?.Orders?.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Order?> ReadByProductId(int id)
        {
            return await Context?.Orders?.FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task<Order?> ReadByUserId(string id)
        {
            return await Context?.Orders?.FirstOrDefaultAsync(p => p.UserId == id);
        }

        public async Task<Order?> Update(Order order)
        {
            Context?.Orders?.Update(order);
            await Context?.SaveChangesAsync();

            return order;
        }
    }
}
