using System;

namespace SlaChecker.Domain.Entities.HolidayApi
{
  public class HolidayApiRequests
  {
    public long Used { get; set; }
    public long Available { get; set; }
    public DateTimeOffset Resets { get; set; }
  }
}
