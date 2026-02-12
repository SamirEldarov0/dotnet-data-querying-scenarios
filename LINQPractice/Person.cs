using System;
using System.Collections.Generic;
using System.Text;

namespace LINQPractice
{
    public class Person
    {
        public int Id { get; set; }
        public string FullName { get; set; } = "";
        public string Country { get; set; } = "";
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<Order> Orders { get; set; } = new();
    }

}