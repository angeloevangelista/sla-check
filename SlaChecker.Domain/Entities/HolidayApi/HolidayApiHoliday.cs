using System;

namespace SlaChecker.Domain.Entities.HolidayApi
{
  public class HolidayApiHoliday
  {
    public string Name { get; set; }
    public DateTimeOffset Date { get; set; }
    public DateTimeOffset Observed { get; set; }
    public bool Public { get; set; }
    public string Country { get; set; }
    public Guid Uuid { get; set; }
    public HolidayApiWeekday Weekday { get; set; }
  }
}
