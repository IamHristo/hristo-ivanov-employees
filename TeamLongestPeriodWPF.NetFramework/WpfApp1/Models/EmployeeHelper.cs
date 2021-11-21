// <copyright file="EmployeeHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WpfApp1.Models
{
    /// <summary>
    /// This class contains employees working on same project and number of days worked together.
    /// </summary>
    public class EmployeeHelper
    {
        /// <summary>
        /// Gets or sets a value indicating first employee id.
        /// </summary>
        public int Employee1Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating second employee id.
        /// </summary>
        public int Employee2Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating project id.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Gets or sets a value days employee 1 and employee 2 worked together.
        /// </summary>
        public int DaysWorkedTogether { get; set; }
    }
}
