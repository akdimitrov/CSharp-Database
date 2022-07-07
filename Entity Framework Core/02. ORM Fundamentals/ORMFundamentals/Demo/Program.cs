using System;
using System.Linq;
using Demo.Models;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new SoftUniContext();
            Console.WriteLine(db.Employees.Count());
        }
    }
}
