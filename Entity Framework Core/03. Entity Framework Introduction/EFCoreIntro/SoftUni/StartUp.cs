using System;
using System.Linq;
using System.Text;
using SoftUni.Data;
using SoftUni.Models;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            var context = new SoftUniContext();
            //Console.WriteLine(GetEmployeesFullInformation(context));
            //Console.WriteLine(GetEmployeesWithSalaryOver50000(context));
            //Console.WriteLine(GetEmployeesFromResearchAndDevelopment(context));
            //Console.WriteLine(AddNewAddressToEmployee(context));
            //Console.WriteLine(GetEmployeesInPeriod(context));
            //Console.WriteLine(GetAddressesByTown(context));
            //Console.WriteLine(GetEmployee147(context));
            //Console.WriteLine(GetDepartmentsWithMoreThan5Employees(context));
            //Console.WriteLine(GetLatestProjects(context));
            //Console.WriteLine(IncreaseSalaries(context));
            //Console.WriteLine(GetEmployeesByFirstNameStartingWithSa(context));
            //Console.WriteLine(DeleteProjectById(context));
            Console.WriteLine(RemoveTown(context));
        }

        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var employeesInfo = context.Employees
                .OrderBy(x => x.EmployeeId)
                .ToList();

            StringBuilder result = new StringBuilder();
            foreach (var e in employeesInfo)
            {
                result.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:f2}");
            }

            return result.ToString().TrimEnd();
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var employeesInfo = context.Employees
                .Where(x => x.Salary > 50000)
                .OrderBy(x => x.FirstName)
                .Select(x => $"{x.FirstName} - {x.Salary:f2}")
                .ToList();

            StringBuilder result = new StringBuilder();
            foreach (var e in employeesInfo)
            {
                result.AppendLine(e);
            }

            return result.ToString().TrimEnd();
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var employeesInfo = context.Employees
                .Where(x => x.Department.Name == "Research and Development")
                .OrderBy(x => x.Salary)
                .ThenByDescending(x => x.FirstName)
                .Select(x => new { x.FirstName, x.LastName, x.Salary })
                .ToList();

            StringBuilder result = new StringBuilder();
            foreach (var e in employeesInfo)
            {
                result.AppendLine($"{e.FirstName} {e.LastName} from Research and Development - ${e.Salary:f2}");
            }

            return result.ToString().TrimEnd();
        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            var emp = context.Employees.FirstOrDefault(x => x.LastName == "Nakov");
            if (emp != null)
            {
                emp.Address = new Address() { AddressText = "Vitoshka 15", TownId = 4 };
            }

            context.SaveChanges();

            var addresses = context.Employees
                .OrderByDescending(x => x.AddressId)
                .Take(10)
                .Select(x => x.Address.AddressText)
                .ToList();

            StringBuilder result = new StringBuilder();
            foreach (var address in addresses)
            {
                result.AppendLine(address);
            }

            return result.ToString().TrimEnd();
        }

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var employeesInfo = context.Employees
                .Where(x => x.EmployeesProjects.Any(x => x.Project.StartDate.Year >= 2001 && x.Project.StartDate.Year <= 2003))
                .Select(x => new
                {
                    EmployeeFirstName = x.FirstName,
                    EmployeeLastName = x.LastName,
                    ManagerFirstName = x.Manager.FirstName,
                    ManagerLastName = x.Manager.LastName,
                    Projects = x.EmployeesProjects.Select(p => p.Project)
                })
                .Take(10)
                .ToList();

            StringBuilder result = new StringBuilder();
            foreach (var e in employeesInfo)
            {
                result.AppendLine($"{e.EmployeeFirstName} {e.EmployeeLastName} - Manager: {e.ManagerFirstName} {e.ManagerLastName}");
                foreach (var p in e.Projects)
                {
                    var startDate = $"{p.StartDate:M/d/yyyy h:mm:ss tt}";
                    var endDate = p.EndDate != null
                        ? $"{p.EndDate:M/d/yyyy h:mm:ss tt}"
                        : "not finished";
                    result.AppendLine($"--{p.Name} - {startDate} - {endDate}");
                }
            }

            return result.ToString().TrimEnd();
        }

        public static string GetAddressesByTown(SoftUniContext context)
        {
            var addresses = context.Addresses
                .OrderByDescending(x => x.Employees.Count())
                .ThenBy(x => x.Town.Name)
                .ThenBy(x => x.AddressText)
                .Select(x => new
                {
                    x.AddressText,
                    TownName = x.Town.Name,
                    EmployeeCount = x.Employees.Count()
                })
                .Take(10)
                .ToList();

            StringBuilder result = new StringBuilder();
            foreach (var a in addresses)
            {
                result.AppendLine($"{a.AddressText}, {a.TownName} - {a.EmployeeCount} employees");
            }

            return result.ToString().TrimEnd();
        }

        public static string GetEmployee147(SoftUniContext context)
        {
            var employee = context.Employees
                .Select(x => new
                {
                    x.EmployeeId,
                    x.FirstName,
                    x.LastName,
                    x.JobTitle,
                    Projects = x.EmployeesProjects
                    .Select(x => x.Project.Name)
                    .OrderBy(x => x)
                    .ToList()
                })
                .FirstOrDefault(x => x.EmployeeId == 147);

            StringBuilder result = new StringBuilder();
            result.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
            foreach (var project in employee.Projects)
            {
                result.AppendLine(project);
            }

            return result.ToString().TrimEnd();
        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var departments = context.Departments
                .Where(x => x.Employees.Count > 5)
                .OrderBy(x => x.Employees.Count)
                .ThenBy(x => x.Name)
                .Select(x => new
                {
                    x.Name,
                    ManagerFirstName = x.Manager.FirstName,
                    ManagerLastName = x.Manager.LastName,
                    Employees = x.Employees
                    .OrderBy(x => x.FirstName)
                    .ThenBy(x => x.LastName)
                    .ToList()
                })
                .ToList();

            StringBuilder result = new StringBuilder();
            foreach (var dep in departments)
            {
                result.AppendLine($"{dep.Name} - {dep.ManagerFirstName} {dep.ManagerLastName}");
                foreach (var emp in dep.Employees)
                {
                    result.AppendLine($"{emp.FirstName} {emp.LastName} - {emp.JobTitle}");
                }
            }

            return result.ToString().TrimEnd();
        }

        public static string GetLatestProjects(SoftUniContext context)
        {
            var projects = context.Projects
                .OrderByDescending(x => x.StartDate)
                .Take(10)
                .OrderBy(x => x.Name)
                .ToList();

            StringBuilder result = new StringBuilder();
            foreach (var project in projects)
            {
                result.AppendLine(project.Name);
                result.AppendLine(project.Description);
                result.AppendLine($"{project.StartDate:M/d/yyyy h:mm:ss tt}");
            }

            return result.ToString().TrimEnd();
        }

        public static string IncreaseSalaries(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(x => x.Department.Name == "Engineering" ||
                            x.Department.Name == "Tool Design" ||
                            x.Department.Name == "Marketing" ||
                            x.Department.Name == "Information Services")
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .ToList();


            StringBuilder result = new StringBuilder();
            foreach (var emp in employees)
            {
                emp.Salary *= 1.12m;
                result.AppendLine($"{emp.FirstName} {emp.LastName} (${emp.Salary:f2})");
            }

            context.SaveChanges();

            return result.ToString().TrimEnd();
        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(x => x.FirstName.ToLower().StartsWith("sa"))
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .ToList();

            StringBuilder result = new StringBuilder();
            foreach (var emp in employees)
            {
                result.AppendLine($"{emp.FirstName} {emp.LastName} - {emp.JobTitle} - (${emp.Salary:f2})");
            }

            return result.ToString().TrimEnd();
        }

        public static string DeleteProjectById(SoftUniContext context)
        {
            var empProjects = context.EmployeesProjects
                .Where(x => x.ProjectId == 2)
                .ToList();
            context.EmployeesProjects.RemoveRange(empProjects);

            var project = context.Projects
                .FirstOrDefault(x => x.ProjectId == 2);
            context.Projects.Remove(project);

            context.SaveChanges();

            StringBuilder result = new StringBuilder();
            var first10Projects = context.Projects.Take(10).ToList();
            foreach (var proj in first10Projects)
            {
                result.AppendLine(proj.Name);
            }

            return result.ToString().TrimEnd();
        }

        public static string RemoveTown(SoftUniContext context)
        {
            var townName = "Seattle";
            var employeesIsSeattle = context.Employees
                .Where(x => x.Address.Town.Name == townName)
                .ToList();

            foreach (var emp in employeesIsSeattle)
            {
                emp.AddressId = null;
            }

            var addresses = context.Addresses
                .Where(x => x.Town.Name == townName).ToList();
            var count = addresses.Count;

            foreach (var address in addresses)
            {
                context.Addresses.Remove(address);
            }

            var town = context.Towns.FirstOrDefault(x => x.Name == townName);
            if (town != null)
            {
                context.Towns.Remove(town);
            }

            context.SaveChanges();

            return $"{count} addresses in {townName} were deleted";
        }
    }
}
