using System;

namespace SlaCheck.Entities
{
  public class Holiday
  {
    public string Name { get; set; }
    public DateTimeOffset Date { get; set; }
    public DateTimeOffset Observed { get; set; }
    public bool Public { get; set; }
    public string Country { get; set; }
    public Guid Uuid { get; set; }
    public Weekday Weekday { get; set; }
  }
}
