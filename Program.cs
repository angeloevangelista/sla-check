using System;
using System.Collections.Generic;
using System.Linq;
using SlaCheck.Entities;
using SlaCheck.Services;

namespace SlaCheck
{
  class Program
  {
    static List<Holiday> holidays;
    static int whileCount = 0;

    static void Main(string[] args)
    {
      var businessPeriod = new TimePeriod(10, 18);

      var documents = new DocumentsServiceMock().GetDocuments();

      foreach (var document in documents)
      {
        var slaIsValid = CheckSlaValidity(
          document, 
          businessPeriod, 
          AppSettings.SlaInterval
        );

        whileCount = 0;
      }
    }

    private static bool CheckSlaValidity(
      Document document,
      TimePeriod businessPeriod,
      int slaIntervalInHours
    )
    {
      bool slaIsValid;

      var slaExpirationDate = GetSlaExpirationDate(
        document.PublishDate,
        businessPeriod,
        slaIntervalInHours
      );

      slaIsValid = AppSettings.ServerDateTime < slaExpirationDate;

      var formatedDocumentDate = document.PublishDate.ToString();
      var formatedSlaExpirationDate = slaExpirationDate.ToString();

      Console.WriteLine();
      Console.WriteLine($"Documento: {document.Title}");
      Console.WriteLine($"Data de publicação: {formatedDocumentDate}");
      Console.WriteLine($"Vencimento SLA: {formatedSlaExpirationDate}");
      Console.WriteLine($"SLA válido: {(slaIsValid ? "✅" : "❌")}");
      Console.WriteLine($"While Count: {whileCount}");
      Console.WriteLine();
      Console.WriteLine("----------");

      return slaIsValid;
    }

    private static DateTime GetSlaExpirationDate(
      DateTime initialDate,
      TimePeriod businessPeriod,
      int slaIntervalInHours
    )
    {
      var slaExpirationDate = initialDate
        .AddSeconds(slaIntervalInHours * 3600);

      TimeSpan remainingTime;
      bool isBusinessPeriodOfDay, isWeekend, isHoliday;

      CheckIsBusinessDate(
        businessPeriod,
        slaExpirationDate,
        out remainingTime,
        out isBusinessPeriodOfDay,
        out isWeekend,
        out isHoliday
      );

      var isValidDate = !isWeekend && !isHoliday && isBusinessPeriodOfDay;

      while (!isValidDate || remainingTime.TotalSeconds > 0)
      {
        whileCount++;

        var nextBusinessPeriodBegin = new DateTime(
          slaExpirationDate.Year,
          slaExpirationDate.Month,
          slaExpirationDate.Day + 1,
          businessPeriod.Start,
          0,
          0
        );

        slaExpirationDate = remainingTime.TotalSeconds > 0
          ? nextBusinessPeriodBegin.Add(remainingTime)
          : nextBusinessPeriodBegin.AddSeconds(slaIntervalInHours * 3600);

        CheckIsBusinessDate(
          businessPeriod,
          slaExpirationDate,
          out remainingTime,
          out isBusinessPeriodOfDay,
          out isWeekend,
          out isHoliday
        );

        isValidDate = !isWeekend && !isHoliday && isBusinessPeriodOfDay;
      }

      return slaExpirationDate;
    }

    private static void CheckIsBusinessDate(
      TimePeriod businessPeriod,
      DateTime slaExpirationDate,
      out TimeSpan remainingTime,
      out bool isBusinessPeriodOfDay,
      out bool isWeekend,
      out bool isHoliday
    )
    {
      isBusinessPeriodOfDay = CheckIsBusinessPeriod(
        slaExpirationDate,
        businessPeriod,
        out remainingTime
      );

      isWeekend = CheckIsWeekend(
        slaExpirationDate.Add(remainingTime)
      );

      isHoliday = CheckIsHoliday(
        slaExpirationDate.Add(remainingTime)
      );
    }

    private static bool CheckIsWeekend(DateTime date)
    {
      return new DayOfWeek[] {
        DayOfWeek.Saturday,
        DayOfWeek.Sunday
      }.Contains(date.DayOfWeek);
    }

    private static bool CheckIsHoliday(DateTime date)
    {
      if (holidays == null)
        holidays = new HolidayApiService().GetHolidays();

      var dateToCompare = new DateTime(2020, date.Month, date.Day, 0, 0, 0);

      DateTimeOffset convertedDate = new DateTimeOffset(dateToCompare);

      return holidays.Any(p => p.Date == convertedDate);
    }

    private static bool CheckIsBusinessPeriod(
      DateTime dateToCheck,
      TimePeriod businessPeriod,
      out TimeSpan remainingTime
    )
    {
      var isBusinessPeriod = true;

      var beginOfBusinessPeriod = new DateTime(
        dateToCheck.Year,
        dateToCheck.Month,
        dateToCheck.Day,
        businessPeriod.Start,
        0,
        0
      );

      var endOfBusinessPeriod = new DateTime(
        dateToCheck.Year,
        dateToCheck.Month,
        dateToCheck.Day,
        businessPeriod.End,
        0,
        0
      );

      isBusinessPeriod =
        beginOfBusinessPeriod <= dateToCheck
        && dateToCheck <= endOfBusinessPeriod;

      if(isBusinessPeriod) {
        remainingTime = new TimeSpan();
      } else {
        remainingTime = (endOfBusinessPeriod.Hour - dateToCheck.Hour) < 0
          ? new TimeSpan()
          : (endOfBusinessPeriod - dateToCheck).Duration();
      }

      return isBusinessPeriod;
    }
  }
}
