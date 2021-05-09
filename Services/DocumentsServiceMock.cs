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
            month: 1,
            day: 1,
            hour: 10, // <- This is the big hour >:D
            0,
            0
          )
        }
      );

      return list;
    }
  }
}