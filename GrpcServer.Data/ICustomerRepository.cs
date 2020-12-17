using GrpcServer.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GrpcServer.Data
{
    public interface ICustomerRepository
    {
        Task<Customer> GetAsync(int userId);
        Task<List<Customer>> GetAllAsync();
        Task AddAsync(Customer customer);
    }
}
