using System.Collections.Generic;

namespace SlaChecker.Domain.Entities.HolidayApi
{
  public class HolidayApiResponse
  {
    public long Status { get; set; }
    public string Warning { get; set; }
    public string Error { get; set; }
    public HolidayApiRequests Requests { get; set; }
    public List<HolidayApiHoliday> Holidays { get; set; }
  }
}
