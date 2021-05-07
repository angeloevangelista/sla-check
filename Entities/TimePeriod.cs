namespace SlaCheck.Entities
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