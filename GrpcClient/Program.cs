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
            using (var channel = GrpcChannel.ForAddress("https://localhost:5001/"))
            {
                Console.WriteLine("Hello Service Request");
            
                var helloRequest = new HelloRequest { Name = "saba" };
                var helloClient = new Greeter.GreeterClient(channel);
                var helloResponse = await helloClient.SayHelloAsync(helloRequest);
            
                Console.WriteLine(helloResponse.Message);
            
                Console.WriteLine();
                Console.WriteLine("Customer Service\n***");
            
                var customerClient = new Customer.CustomerClient(channel);
                var customerRequest = new CustomerLookupModel { UserId = 1 };
                var customerResponse = await customerClient.GetCustomerAsync(customerRequest);
            
                Console.WriteLine("First Customer:");
                Console.WriteLine($"{customerResponse.FirstName} {customerResponse.LastName} {customerResponse.Age}");
                Console.WriteLine();

                Console.WriteLine("All Customers\n***\n");
                Console.WriteLine("Server Streaming ...");
                using (var call = customerClient.GetAll(new NewCustomerRequest()))
                {

                    await foreach (var item in call.ResponseStream.ReadAllAsync())
                    {
                        var currentCustomer = call.ResponseStream.Current;
                        Console.WriteLine($"{currentCustomer.Id} - {currentCustomer.FirstName} {currentCustomer.LastName}: {currentCustomer.EmailAddress}");
                    }

                    //while (await call.ResponseStream.MoveNext())
                    //{
                    //    var currentCustomer = call.ResponseStream.Current;
                    //    Console.WriteLine($"\t{currentCustomer.Id} - {currentCustomer.FirstName} {currentCustomer.LastName}: {currentCustomer.EmailAddress}");
                    //}
                }

                Console.WriteLine("Adding Customers");
                Console.WriteLine("Client Streaming");

                var call2 = customerClient.AddCustomer();

                for (int i = 0; i < 10000; i++)
                {
                    var rnd = new Random();
                    await call2.RequestStream.WriteAsync(
                        new CustomerModel
                        {
                            Id = rnd.Next(),
                            Age = rnd.Next(20, 100),
                            EmailAddress = $"test{rnd.Next()}@gmail.com"
                        });

                }
                await call2.RequestStream.CompleteAsync();

                
                Console.WriteLine(call2.ResponseAsync.Result.Count);
                Console.WriteLine("Press any key to continue...");

            }

            Console.ReadKey();
        }
    }
}
