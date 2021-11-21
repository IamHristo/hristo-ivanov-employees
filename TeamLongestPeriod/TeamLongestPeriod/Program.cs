using System;
using System.Collections.Generic;
using System.Linq;

namespace TeamLongestPeriod
{
    class Program
    {
        static void Main(string[] args)
        {
            Team team = new Team();
            List<EmployeeHelper> employeeHelperList = new List<EmployeeHelper>();
            team.LoadTeamData();
            team.RemoveEmployeesWorkingOnOnlyOneProject();

            var employeesGroupedByProject = team.Employees
                .GroupBy(u => u.ProjectId)
                .Select(grp => grp.OrderBy(o => o.DateFrom).ToList())
                .Where(lst => lst.Count > 1)
                .ToList();

            foreach (List<Employee> employeesInProject in employeesGroupedByProject)
            {
                for (int i = 0; i < employeesInProject.Count; i++)
                {
                    // ProjectId = 1
                    DateTime emp1DateFrom = DateTime.ParseExact(employeesInProject[i].DateFrom, "yyyy/dd/MM", null);
                    DateTime emp1DateTo = DateTime.ParseExact(employeesInProject[i].DateTo, "yyyy/dd/MM", null);

                    for (int j = i + 1; j < employeesInProject.Count; j++)
                    {
                        DateTime emp2DateFrom = DateTime.ParseExact(employeesInProject[i].DateFrom, "yyyy/dd/MM", null);
                        DateTime emp2DateTo = DateTime.ParseExact(employeesInProject[i].DateTo, "yyyy/dd/MM", null);

                        int daysWorkTogether = CalculatedDaysWorkedTogether(emp1DateFrom, emp2DateFrom, emp1DateTo, emp2DateTo);

                        if (daysWorkTogether > 0)
                        {
                            if (team.EmployeesIdsWorkingOnMoreThanOneProject.Any(x => x.EmpId == employeesInProject[i].EmpId) && team.EmployeesIdsWorkingOnMoreThanOneProject.Any(x => x.EmpId == employeesInProject[j].EmpId))
                            {
                                employeeHelperList.Add(new EmployeeHelper
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


            var workByPairs = employeeHelperList.
                GroupBy(gr => new { gr.Employee1Id, gr.Employee2Id, gr.ProjectId }).
                Select(g => new EmployeeHelper()
                {
                    Employee1Id = g.Key.Employee1Id,
                    Employee2Id = g.Key.Employee2Id,
                    DaysWorkedTogether = g.Sum(s => s.DaysWorkedTogether), // sum per project for pair of employees
                    ProjectId = g.First().ProjectId,
                }).OrderByDescending(o => o.DaysWorkedTogether).ToList();


            var longestWorkByPairs = workByPairs.GroupBy(gr => new { gr.Employee1Id, gr.Employee2Id }).Select(
                g => new
                {
                    ID1 = g.Key.Employee1Id,
                    ID2 = g.Key.Employee2Id,
                    Days = g.Sum(s => s.DaysWorkedTogether),
                    ProjectIDs = string.Join(",", g.Select(s => s.ProjectId)),
                }).OrderByDescending(o => o.Days).ToList();

            foreach (var item in longestWorkByPairs)
            {
                Console.WriteLine(item.ID1); 
                Console.WriteLine(item.ID2); 
                Console.WriteLine(item.ProjectIDs); 
                Console.WriteLine(item.Days); 
            }
            ;
            //int sumDaysWorkedTogether = 0;
            //int empId1 = 0;
            //int empId2 = 0;
            //int projectId = 0;
            //foreach (var empWorkedMoreThanOneProject in team.EmployeesIdsWorkingOnMoreThanOneProject)
            //{
            //    foreach (var emp in team.Employees)
            //    {
            //        if(empWorkedMoreThanOneProject.EmpId != emp.EmpId && team.EmployeesIdsWorkingOnMoreThanOneProject.Any(x => x.EmpId == emp.EmpId)) // Not same employee.
            //        {
            //            if(empWorkedMoreThanOneProject.ProjectId == emp.ProjectId) // Worked on same project.
            //            {
            //                if(sumDaysWorkedTogether < empWorkedMoreThanOneProject.DaysWorkedAtProject + emp.DaysWorkedAtProject)
            //                {
            //                    sumDaysWorkedTogether = empWorkedMoreThanOneProject.DaysWorkedAtProject + emp.DaysWorkedAtProject;
            //                    empId1 = empWorkedMoreThanOneProject.EmpId;
            //                    empId2 = emp.EmpId;
            //                    projectId = emp.ProjectId;
            //                }
            //            }
            //        }
            //    }
            //}
            //Console.WriteLine(empId1);
            //Console.WriteLine(empId2);
            //Console.WriteLine(projectId);
            //Console.WriteLine(sumDaysWorkedTogether);
        }

        private static int CalculatedDaysWorkedTogether(DateTime emp1DateFrom, DateTime emp2DateFrom, DateTime emp1DateTo, DateTime emp2DateTo)
        {
            int daysTogether = 0;

            if (emp1DateFrom <= emp2DateFrom && emp1DateTo <= emp2DateTo)
            {
                daysTogether = CalcDaysDiff(emp2DateFrom, emp1DateTo);
            }
            else if (emp1DateFrom >= emp2DateFrom && emp1DateTo >= emp2DateTo)
            {
                daysTogether = CalcDaysDiff(emp1DateFrom, emp2DateTo);
            }
            else if (emp1DateFrom >= emp2DateFrom && emp1DateTo <= emp2DateTo)
            {
                daysTogether = CalcDaysDiff(emp1DateFrom, emp1DateTo);
            }
            else if (emp1DateFrom <= emp2DateFrom && emp1DateTo >= emp2DateTo)
            {
                daysTogether = CalcDaysDiff(emp2DateFrom, emp2DateTo);
            }
            return daysTogether;
        }

        private static int CalcDaysDiff(DateTime start, DateTime end)
        {
            return (int)(end - start).TotalDays;
        }
    }
}
