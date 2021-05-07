using System;

namespace SlaCheck.Entities
{
  public class Document
  {
    public Document(string title)
    {
      Title = title;
    }

    public string Title { get; set; }
    public bool Validated { get; set; }
    public DateTime PublishDate { get; set; }
  }
}