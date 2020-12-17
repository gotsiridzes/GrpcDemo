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
            var channel = GrpcChannel.ForAddress("https://localhost:5001/");
            Console.WriteLine("Hello Service Request");
            
            var helloRequest = new HelloRequest { Name = "saba" };
            var helloClient = new Greeter.GreeterClient(channel);
            var helloResponse = await helloClient.SayHelloAsync(helloRequest);
            
            Console.WriteLine(helloResponse.Message);
            
            Console.WriteLine();
            Console.WriteLine("Customer Service\n***");
            
            var customerClient = new Customer.CustomerClient(channel);
            var customerRequest = new CustomerLookupModel { UserId = 1 };
            var customerResponse = await customerClient.GetCustomerInfoAsync(customerRequest);
            
            Console.WriteLine("First Customer:");
            Console.WriteLine($"\t{customerResponse.FirstName} {customerResponse.LastName} {customerResponse.Age}");
            Console.WriteLine();

            Console.WriteLine("New Customers\n***\n");

            using (var call = customerClient.GetNewCustomer(new NewCustomerRequest()))
            {

                await foreach (var item in call.ResponseStream.ReadAllAsync())
                {
                    var currentCustomer = call.ResponseStream.Current;
                    Console.WriteLine($"\t{currentCustomer.Id} - {currentCustomer.FirstName} {currentCustomer.LastName}: {currentCustomer.EmailAddress}");
                }

                //while (await call.ResponseStream.MoveNext())
                //{
                //    var currentCustomer = call.ResponseStream.Current;
                //    Console.WriteLine($"\t{currentCustomer.Id} - {currentCustomer.FirstName} {currentCustomer.LastName}: {currentCustomer.EmailAddress}");
                //}
            }
            Console.ReadLine();
        }
    }
}
