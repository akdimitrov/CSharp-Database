namespace MiniORM.App
{
    using System.Linq;
    using Data;
    using Data.Entities;

    public class StartUp
    {
        static void Main(string[] args)
        {
            var dbContext = new SoftUniDbContext("Server=.;Database=MiniORM;Integrated Security=True");
            dbContext.Employees.Add(new Employee
            {
                FirstName = "Gosho",
                LastName = "Inserted",
                DepartmentId = dbContext.Departments.First().Id,
                IsEmployed = true
            });

            var employee = dbContext.Employees.Last();
            employee.FirstName = "Modified";

            dbContext.SaveChanges();
        }
    }
}
