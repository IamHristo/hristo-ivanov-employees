// <copyright file="Employee.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WpfApp1.Models
{
    using System;
    using System.Globalization;

    /// <summary>
    /// This class contains properties and methods for Employee model.
    /// </summary>
    public class Employee
    {
        private DateTime dateTo;

        /// <summary>
        /// Gets or sets a value indicating employee id.
        /// </summary>
        public int EmpId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating employee project id.
        /// </summary>
        public int ProjectId { get; set; }

        // [JsonConverter(typeof(CustomDateTimeConverter))]

        /// <summary>
        /// Gets or sets a value indicating employee project date started.
        /// </summary>
        public DateTime DateFrom { get; set; }

        // [JsonConverter(typeof(CustomDateTimeConverter))]

        /// <summary>
        /// Gets or sets a value indicating employee project date ended.
        /// </summary>
        public DateTime DateTo
        {
            get => this.dateTo;
            set
            {
                if (value != null)
                {
                    this.dateTo = value;
                }
                else
                {
                    this.dateTo = DateTime.Now;
                }
            }
        }
    }
}
