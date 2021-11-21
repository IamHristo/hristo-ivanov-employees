// <copyright file="MainViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WpfApp1.ViewModel
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Forms;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using WpfApp1.Models;

    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm.
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private Team team;
        private ObservableCollection<Employee> employees;
        private string selectedDateFormat;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel()
        {
            this.team = new Team();
        }

        /// <summary>
        /// Gets command for opening file dialog.
        /// </summary>
        public RelayCommand OpenFileDialogCommand { get => new RelayCommand(this.OnOpenFileDialog); }

        /// <summary>
        /// Gets command for showing longest period working employees pair.
        /// </summary>
        public RelayCommand ShowPairsCommand { get => new RelayCommand(this.OnShowPairs); }

        /// <summary>
        /// Gets or sets a collection of date formats.
        /// </summary>
        public List<string> DateFormats { get; set; } = new List<string>()
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
        };

        /// <summary>
        /// Gets or sets a value indicatin selected date format.
        /// </summary>
        public string SelectedDateFormat
        {
            get => this.selectedDateFormat;
            set
            {
                this.selectedDateFormat = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a collection of employees.
        /// </summary>
        public ObservableCollection<Employee> Employees
        {
            get => this.employees;
            set
            {
                this.employees = value;
                this.RaisePropertyChanged();
            }
        }

        private void OnShowPairs()
        {
            EmployeeHelper result = this.team.ShowBestProjectPartners();
            MessageBox.Show($"Employee with id {result.Employee1Id} and employee with id {result.Employee2Id} worked together longest period {result.DaysWorkedTogether} days.");
        }

        private void OnOpenFileDialog()
        {
            using (var openFileDialog = new System.Windows.Forms.OpenFileDialog())
            {
                openFileDialog.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    this.team.LoadTeamData(openFileDialog.FileName);
                    if (this.team.Employees != null)
                    {
                        this.Employees = new ObservableCollection<Employee>(this.team.Employees);
                    }
                }
            }
        }
    }
}
