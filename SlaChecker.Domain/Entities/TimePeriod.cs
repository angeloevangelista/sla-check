using System;

namespace SlaChecker.Domain.Entities
{
  public class TimePeriod
  {
    public TimePeriod(int start, int end)
    {
      Start = start;
      End = end;
    }

    public int Start { get; set; }
    public int End { get; set; }
  }
}