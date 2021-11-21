using System;
using System.Globalization;

/// <summary>
/// 
/// </summary>
public class Employee
{
    private string dateTo;
    /// <summary>
    /// 
    /// </summary>
    public int EmpId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int ProjectId { get; set; }

    //[JsonConverter(typeof(CustomDateTimeConverter))]
    /// <summary>
    /// 
    /// </summary>
    public string DateFrom { get; set; }

    //[JsonConverter(typeof(CustomDateTimeConverter))]
    /// <summary>
    /// 
    /// </summary>
    public string DateTo
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
                this.dateTo = DateTime.Now.ToString("yyyy/dd/MM", CultureInfo.InvariantCulture);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public int DaysWorkedAtProject { get; set; }

    public void CalculatedDaysWorkedAtProject()
    {
        if (this.DateFrom != null && this.DateTo != null)
        {

            DateTime dateFrom = DateTime.ParseExact(this.DateFrom, "yyyy/dd/MM", null);
            DateTime dateTo = DateTime.ParseExact(this.DateTo, "yyyy/dd/MM", null);

            this.DaysWorkedAtProject = Convert.ToInt32((dateTo - dateFrom).TotalDays);
        }
    }
}
