// <copyright file="Team.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WpfApp1.Models
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using Newtonsoft.Json;
    using WpfApp1.Converter;

    /// <summary>
    /// This class contains team with employees.
    /// </summary>
    public class Team
    {
        private List<Employee> employees;
        private List<EmployeeHelper> employeeHelperList;

        /// <summary>
        /// Gets or sets List of all employees.
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
        /// Gets or sets List of working on more than one project employees.
        /// </summary>
        public List<Employee> EmployeesIdsWorkingOnMoreThanOneProject { get; set; }

        /// <summary>
        /// Load data for <see cref="Employees"/> from given path to json file.
        /// </summary>
        /// <param name="path">Path to json file.</param>
        public void LoadTeamData(string path)
        {
            var settings = new JsonSerializerSettings();
            settings.DateParseHandling = DateParseHandling.None;
            settings.Converters.Add(new MultiFormatDateTimeConverter(new List<string>()
        {
            "yyyy-MM-dd",
            "yyyy/MM/dd",
            "yyyy.MM.dd",
            "yyyy-dd-MM",
            "yyyy/dd/MM",
            "yyyy.dd.MM",
            "MM-dd-yyyy",
            "MM/dd/yyyy",
            "MM.dd.yyyy",
            "MMM-dd-yyyy",
            "MMM/dd/yyyy",
            "MMM.dd.yyyy",
            "MMMM-dd-yyyy",
            "MMMM/dd/yyyy",
            "MMMM.dd.yyyy",
        }));
            this.Employees = JsonConvert.DeserializeObject<List<Employee>>(File.ReadAllText(path), settings);

            this.RemoveEmployeesWorkingOnOnlyOneProject();
        }

        /// <summary>
        /// Show top 2 employees working on same project longest time.
        /// </summary>
        /// <returns>EmployeeHelper of most time worked together employee pairs.</returns>
        public EmployeeHelper ShowBestProjectPartners()
        {
            employeeHelperList = new List<EmployeeHelper>();
            var employeesGroupedByProject = this.Employees
                .GroupBy(u => u.ProjectId)
                .Select(grp => grp.OrderBy(o => o.DateFrom).ToList())
                .Where(lst => lst.Count > 1)
                .ToList();

            foreach (List<Employee> employeesInProject in employeesGroupedByProject)
            {
                for (int i = 0; i < employeesInProject.Count; i++)
                {
                    // ProjectId = 1
                    var ci = new CultureInfo("en-US");
                    var formats = new[]
                    {
                        "yyyy-MM-dd",
                        "yyyy/MM/dd",
                        "yyyy.MM.dd",
                        "yyyy-dd-MM",
                        "yyyy/dd/MM",
                        "yyyy.dd.MM",
                        "MM-dd-yyyy",
                        "MM/dd/yyyy",
                        "MM.dd.yyyy",
                        "MMM-dd-yyyy",
                        "MMM/dd/yyyy",
                        "MMM.dd.yyyy",
                        "MMMM-dd-yyyy",
                        "MMMM/dd/yyyy",
                        "MMMM.dd.yyyy",
                    }
                                        .Union(ci.DateTimeFormat.GetAllDateTimePatterns()).ToArray();

                    DateTime emp1DateFrom = employeesInProject[i].DateFrom;
                    DateTime emp1DateTo = employeesInProject[i].DateTo;

                    for (int j = i + 1; j < employeesInProject.Count; j++)
                    {
                        DateTime emp2DateFrom = employeesInProject[i + 1].DateFrom;
                        DateTime emp2DateTo = employeesInProject[i + 1].DateTo;

                        int daysWorkTogether = this.CalculatedDaysWorkedTogether(emp1DateFrom, emp2DateFrom, emp1DateTo, emp2DateTo);

                        if (daysWorkTogether > 0)
                        {
                            if (this.EmployeesIdsWorkingOnMoreThanOneProject.Any(x => x.EmpId == employeesInProject[i].EmpId) && this.EmployeesIdsWorkingOnMoreThanOneProject.Any(x => x.EmpId == employeesInProject[j].EmpId))
                            {
                                this.employeeHelperList.Add(new EmployeeHelper
                                {
                                    Employee1Id = employeesInProject[i].EmpId,
                                    Employee2Id = employeesInProject[j].EmpId,
                                    ProjectId = employeesInProject[i].ProjectId,
                                    DaysWorkedTogether = daysWorkTogether,
                                });
                            }
                        }
                    }
                }
            }

            var workByPairs = this.employeeHelperList.
                GroupBy(gr => new { gr.Employee1Id, gr.Employee2Id, gr.ProjectId }).
                Select(g => new EmployeeHelper()
                {
                    Employee1Id = g.Key.Employee1Id,
                    Employee2Id = g.Key.Employee2Id,
                    DaysWorkedTogether = g.Sum(s => s.DaysWorkedTogether), // sum per project for pair of employees
                    ProjectId = g.First().ProjectId,
                }).OrderByDescending(o => o.DaysWorkedTogether).ToList();

            EmployeeHelper longestWorkByPairs = workByPairs.GroupBy(gr => new { gr.Employee1Id, gr.Employee2Id }).Select(
                g => new EmployeeHelper
                {
                    Employee1Id = g.Key.Employee1Id,
                    Employee2Id = g.Key.Employee2Id,
                    DaysWorkedTogether = g.Sum(s => s.DaysWorkedTogether),
                }).OrderByDescending(o => o.DaysWorkedTogether).FirstOrDefault();

            return longestWorkByPairs;
        }

        /// <summary>
        /// Remove employees working on only one project from <see cref="Employees"/> collection.
        /// </summary>
        private void RemoveEmployeesWorkingOnOnlyOneProject()
        {
            this.EmployeesIdsWorkingOnMoreThanOneProject = this.Employees.
                    GroupBy(x => x.EmpId).
                    SelectMany(g => g.Skip(1))
                    .ToList();
        }

        private int CalculatedDaysWorkedTogether(DateTime emp1DateFrom, DateTime emp2DateFrom, DateTime emp1DateTo, DateTime emp2DateTo)
        {
            int daysTogether = 0;

            if (emp1DateFrom <= emp2DateFrom && emp1DateTo <= emp2DateTo)
            {
                daysTogether = this.CalcDaysDiff(emp2DateFrom, emp1DateTo);
            }
            else if (emp1DateFrom >= emp2DateFrom && emp1DateTo >= emp2DateTo)
            {
                daysTogether = this.CalcDaysDiff(emp1DateFrom, emp2DateTo);
            }
            else if (emp1DateFrom >= emp2DateFrom && emp1DateTo <= emp2DateTo)
            {
                daysTogether = this.CalcDaysDiff(emp1DateFrom, emp1DateTo);
            }
            else if (emp1DateFrom <= emp2DateFrom && emp1DateTo >= emp2DateTo)
            {
                daysTogether = this.CalcDaysDiff(emp2DateFrom, emp2DateTo);
            }

            return daysTogether;
        }

        private int CalcDaysDiff(DateTime start, DateTime end)
        {
            return (int)(end - start).TotalDays;
        }
    }
}
