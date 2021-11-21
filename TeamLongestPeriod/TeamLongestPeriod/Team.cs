using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TeamLongestPeriod
{
    /// <summary>
    /// 
    /// </summary>
    public class Team
    {
        private List<Employee> employees;

        /// <summary>
        /// 
        /// </summary>
        public List<Employee> Employees
        {
            get => this.employees;
            set
            {
                this.employees = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<Employee> EmployeesIdsWorkingOnMoreThanOneProject { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public void LoadTeamData()
        {
            this.Employees = JsonConvert.DeserializeObject<List<Employee>>(File.ReadAllText(@"D:\Interviews\Sirma 2021\TeamLongestPeriod\TeamLongestPeriod\Data.json"));

            foreach (Employee emp in this.Employees)
            {
                emp.CalculatedDaysWorkedAtProject();
                Console.WriteLine($"EmpId = {emp.EmpId}");
                Console.WriteLine($"ProjectId = {emp.ProjectId}");
                Console.WriteLine($"DaysWorked = {emp.DaysWorkedAtProject}");
                Console.WriteLine();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveEmployeesWorkingOnOnlyOneProject()
        {
            this.EmployeesIdsWorkingOnMoreThanOneProject = this.Employees.
                    GroupBy(x => x.EmpId).
                    SelectMany(g => g.Skip(1))
                    .ToList();
        }



        /// <summary>
        /// Show top 2 employees working on same project longest time.
        /// </summary>
        private void ShowBestProjectPartners()
        {

        }


    }
}
