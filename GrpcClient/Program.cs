using Grpc.Core;
using Grpc.Net.Client;
using GrpcServer;
using System;
using System.Threading.Tasks;

namespace GrpcClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello Service Request");
            
            var helloRequest = new HelloRequest { Name = "saba" };
            var helloChannel = GrpcChannel.ForAddress("https://localhost:5001/");
            var helloClient = new Greeter.GreeterClient(helloChannel);
            var helloResponse = await helloClient.SayHelloAsync(helloRequest);
            
            Console.WriteLine(helloResponse.Message);
            
            Console.WriteLine();
            Console.WriteLine("Customer Service\n***");
            
            var customerChannel = GrpcChannel.ForAddress("https://localhost:5001/");
            var customerClient = new Customer.CustomerClient(customerChannel);
            var customerRequest = new CustomerLookupModel { UserId = 1 };
            var customerResponse = await customerClient.GetCustomerInfoAsync(customerRequest);
            
            Console.WriteLine("First Customer:");
            Console.WriteLine($"\t{customerResponse.FirstName} {customerResponse.LastName} {customerResponse.Age}");
            Console.WriteLine();

            Console.WriteLine("New Customers\n***\n");

            using (var call = customerClient.GetNewCustomer(new NewCustomerRequest()))
            {
                while (await call.ResponseStream.MoveNext())
                {
                    var currentCustomer = call.ResponseStream.Current;
                    Console.WriteLine($"\t{currentCustomer.Id} - {currentCustomer.FirstName} {currentCustomer.LastName}: {currentCustomer.EmailAddress}");

                }
            }
            Console.ReadLine();
        }
    }
}
