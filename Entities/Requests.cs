using System;

namespace SlaCheck.Entities
{
  public class Requests
  {
    public long Used { get; set; }
    public long Available { get; set; }
    public DateTimeOffset Resets { get; set; }
  }
}
