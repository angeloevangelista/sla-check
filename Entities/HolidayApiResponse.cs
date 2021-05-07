using System.Collections.Generic;

namespace SlaCheck.Entities
{
  public class HolidayApiResponse
  {
    public long Status { get; set; }
    public string Warning { get; set; }
    public Requests Requests { get; set; }
    public List<Holiday> Holidays { get; set; }
  }
}
