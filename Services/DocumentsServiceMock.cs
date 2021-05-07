using System;
using System.Collections.Generic;
using SlaCheck.Entities;

namespace SlaCheck.Services
{
  public class DocumentsServiceMock
  {
    public List<Document> GetDocuments()
    {
      var list = new List<Document>();

      list.Add(
        new Document("DOC 01")
        {
          PublishDate = new DateTime(
            year: 2021,
            month: 5,
            day: 6,
            hour: 18, // <- This is the big hour >:D
            0,
            0
          )
        }
      );

      return list;
    }
  }
}