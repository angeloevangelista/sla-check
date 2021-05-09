using SlaCheck.Entities;
using SlaCheck.Services;

namespace SlaCheck
{
  class Program
  {
    static void Main(string[] args)
    {
      var businessPeriod = new TimePeriod(10, 18);
      var documents = new DocumentsServiceMock().GetDocuments();

      foreach (var document in documents)
        SlaHelper.CheckSlaValidity(
          document,
          businessPeriod,
          AppSettings.SlaInterval
        );
    }
  }
}
