using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcServer.Services
{
    public class CustomersService : Customer.CustomerBase
    {
        private readonly ILogger<CustomersService> _logger;
        public List<CustomerModel> Customers 
        {
            get
            {
                return new List<CustomerModel>
                {
                    new CustomerModel
                    {
                        Age = 22,
                        FirstName = "saba",
                        LastName = "gotsiridze",
                        IsAlive = true,
                        EmailAddress = "gotsiridze.s@outlook.com",
                        Id = 1
                    },
                    new CustomerModel
                    {
                        Age = 22,
                        FirstName = "saba",
                        LastName = "gotsiridze",
                        IsAlive = true,
                        EmailAddress = "gotsiridze.s@outlook.com",
                        Id = 2
                    },
                    new CustomerModel
                    {
                        Age = 22,
                        FirstName = "saba",
                        LastName = "gotsiridze",
                        IsAlive = true,
                        EmailAddress = "gotsiridze.s@outlook.com",
                        Id = 3
                    },
                    new CustomerModel
                    {
                        Age = 22,
                        FirstName = "saba",
                        LastName = "gotsiridze",
                        IsAlive = true,
                        EmailAddress = "gotsiridze.s@outlook.com",
                        Id = 4
                    }
                };
            }
        } 

        public CustomersService(ILogger<CustomersService> logger)
        {
            _logger = logger;
        }

        public override Task<CustomerModel> GetCustomerInfo(CustomerLookupModel request, ServerCallContext context)
        {

            return Task.FromResult(Customers.Where(x => x.Id == request.UserId).FirstOrDefault());
        }

        public override async Task GetNewCustomer(NewCustomerRequest request, IServerStreamWriter<CustomerModel> responseStream, ServerCallContext context)
        {
            foreach (var customer in Customers)
            {
                await Task.Delay(1000);
                await responseStream.WriteAsync(customer);
            }
        }
    }
}
