using GrpcServer.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcServer.Data
{
    public class CustomerRepository : ICustomerRepository
    {
        private static List<Customer> customers = new List<Customer>
        {
            new Customer
            {
                Age = 22,
                FirstName = "saba",
                LastName = "gotsiridze",
                IsAlive = true,
                EmailAddress = "gotsiridze.s@outlook.com",
                Id = 1
            },
            new Customer
            {
                Age = 22,
                FirstName = "test",
                LastName = "testadze",
                IsAlive = true,
                EmailAddress = "test.s@outlook.com",
                Id = 2
            },
            new Customer
            {
                Age = 22,
                FirstName = "katleti",
                LastName = "adsasdasd",
                IsAlive = true,
                EmailAddress = "adsasdas@ss.com",
                Id = 3
            }
        };

        public Task<List<Customer>> GetAllAsync() => Task.FromResult(customers);

        public Task<Customer> GetAsync(int userId) => Task.FromResult(customers.Where(x => x.Id == userId).FirstOrDefault());

        public Task AddAsync(Customer customer)
        {
            customers.Add(customer);
            return Task.FromResult(true);
        }
    }
}
