using System;

namespace Oyster.Examples.ProtoBuf
{
    public static class RandomData
    {
        private static readonly Random Rnd = new Random();

        public static Office GenerateOffice(int employeeCount, int taskCount)
        {
            var office = new Office { Id = GenerateId(), Address = GenerateString() };
            while (employeeCount-- > 0)
            {
                office.Employees.Add(GenerateEmployee(office, taskCount));
            }
            return office;
        }

        private static Employee GenerateEmployee(Office office, int taskCount)
        {
            var employee = new Employee { Id = GenerateId(), Name = GenerateString(), Office = office };
            while (taskCount-- > 0)
            {
                employee.Tasks.Add(GenerateTask(employee));
            }
            return employee;
        }

        private static Task GenerateTask(Employee employee)
        {
            return new Task { Id = GenerateId(), Description = GenerateString(), Assignee = employee };
        }

        private static int GenerateId()
        {
            return Rnd.Next(1, 100000);
        }

        private static string GenerateString()
        {
            var bytes = new byte[Rnd.Next(5, 20)];
            Rnd.NextBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}
