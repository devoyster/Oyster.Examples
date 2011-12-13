using System;
using System.Collections.Generic;
using ProtoBuf;

namespace Oyster.Examples.ProtoBuf
{
    // Explicit proto contract
    [Serializable, ProtoContract]
    public class Employee
    {
        public Employee()
        {
            Tasks = new List<Task>();
        }

        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        public string Name { get; set; }

        [ProtoMember(3)]
        public Office Office { get; set; }

        [ProtoMember(4)]
        public ICollection<Task> Tasks { get; set; }
    }

    // Plain serializable types
    [Serializable]
    public class Office
    {
        public Office()
        {
            Employees = new List<Employee>();
        }

        public int Id { get; set; }

        public string Address { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }

    [Serializable]
    public class Task
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public Employee Assignee { get; set; }
    }
}
