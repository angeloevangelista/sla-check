using System;

namespace SlaChecker.Domain.Entities
{
  public class Holiday
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public string Country { get; set; }
  }
}
