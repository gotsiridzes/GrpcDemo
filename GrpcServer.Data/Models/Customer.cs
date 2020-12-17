using System;
using System.Collections.Generic;
using System.Text;

namespace GrpcServer.Data.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public int Age { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public bool IsAlive { get; set; }
    }
}
