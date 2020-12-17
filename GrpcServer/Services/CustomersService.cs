using Grpc.Core;
using GrpcServer.Data;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcServer.Services
{
    public class CustomersService : Customer.CustomerBase
    {
        private readonly ILogger<CustomersService> _logger;
        private readonly ICustomerRepository _customerRepository;

        public CustomersService(ILogger<CustomersService> logger, ICustomerRepository customerRepository)
        {
            _logger = logger;
            _customerRepository = customerRepository;
        }

        public override async Task<CustomerModel> GetCustomer(CustomerLookupModel request, ServerCallContext context)
        {
            var customer = await _customerRepository.GetAsync(request.UserId);

            return await Task.FromResult(new CustomerModel
            {
                Age = customer.Age,
                EmailAddress = customer.EmailAddress,
                FirstName = customer.FirstName,
                Id = customer.Id,
                IsAlive = customer.IsAlive,
                LastName = customer.LastName
            });
        }

        public override async Task GetAll(NewCustomerRequest request, IServerStreamWriter<CustomerModel> responseStream, ServerCallContext context)
        {
            foreach (var customer in await _customerRepository.GetAllAsync())
            {

                await Task.Delay(1000, context.CancellationToken);
                await responseStream.WriteAsync(new CustomerModel
                {
                    Age = customer.Age,
                    EmailAddress = customer.EmailAddress,
                    FirstName = customer.FirstName,
                    Id = customer.Id,
                    IsAlive = customer.IsAlive,
                    LastName = customer.LastName
                });
            }
        }

        public override async Task<AddCustomerResponse> AddCustomer(IAsyncStreamReader<CustomerModel> requestStream, ServerCallContext context)
        {
            await foreach (var item in requestStream.ReadAllAsync())
            {
                await _customerRepository.AddAsync(new Data.Models.Customer
                {
                    Age = item.Age,
                    EmailAddress = item.EmailAddress,
                    FirstName = item.FirstName,
                    Id = item.Id,
                    IsAlive = item.IsAlive,
                    LastName = item.LastName
                });
            }
            var customers = await _customerRepository.GetAllAsync();
            return new AddCustomerResponse { Count = customers.Count};
        }
    }
}
