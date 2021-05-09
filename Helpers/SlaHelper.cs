using System;
using System.Collections.Generic;
using System.Linq;
using SlaCheck;
using SlaCheck.Entities;
using SlaCheck.Services;

public static class SlaHelper
{
  private static List<Holiday> _holidays;
  private static int _whileCount;

  public static bool CheckSlaValidity(
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
    Console.WriteLine($"While Count: {_whileCount}");
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
      slaIntervalInHours,
      out remainingTime,
      out isBusinessPeriodOfDay,
      out isWeekend,
      out isHoliday
    );

    var slaExpirationOccursTheSameDay =
      slaExpirationDate.Day == initialDate.Day;

    var isValidDate =
      !isWeekend
      && !isHoliday
      && isBusinessPeriodOfDay
      && slaExpirationOccursTheSameDay;

    while (!isValidDate || remainingTime.TotalSeconds > 0)
    {
      _whileCount++;

      var nextBusinessPeriodBegin = new DateTime(
        slaExpirationDate.Year,
        slaExpirationDate.Month,
        slaExpirationDate.Day,
        businessPeriod.Start,
        0,
        0
      ).AddDays(1);

      slaExpirationDate = remainingTime.TotalSeconds > 0
        ? nextBusinessPeriodBegin.AddHours(remainingTime.Hours)
        : nextBusinessPeriodBegin.AddSeconds(slaIntervalInHours * 3600);

      CheckIsBusinessDate(
        businessPeriod,
        slaExpirationDate,
        slaIntervalInHours,
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
    int slaIntervalInHours,
    out TimeSpan remainingTime,
    out bool isBusinessPeriodOfDay,
    out bool isWeekend,
    out bool isHoliday
  )
  {
    isBusinessPeriodOfDay = CheckIsBusinessPeriod(
      slaExpirationDate,
      businessPeriod
    );

    if (isBusinessPeriodOfDay)
    {
      remainingTime = new TimeSpan();

      isWeekend = CheckIsWeekend(
        slaExpirationDate.Add(remainingTime)
      );

      isHoliday = CheckIsHoliday(
        slaExpirationDate.Add(remainingTime)
      );
    }
    else
    {
      isWeekend = false;
      isHoliday = false;

      var originalDate = (slaExpirationDate.AddHours(-slaIntervalInHours));

      var beginOfBusinessPeriod = new DateTime(
        originalDate.Year,
        originalDate.Month,
        originalDate.Day,
        businessPeriod.Start,
        0,
        0
      );

      var endOfBusinessPeriod = new DateTime(
        originalDate.Year,
        originalDate.Month,
        originalDate.Day,
        businessPeriod.End,
        0,
        0
      );

      var expirationBeginDiff = slaExpirationDate - beginOfBusinessPeriod;
      var expirationEndDiff = slaExpirationDate - endOfBusinessPeriod;

      remainingTime = expirationBeginDiff < expirationEndDiff
        ? new TimeSpan()
        : slaExpirationDate.Subtract(
            new TimeSpan(businessPeriod.End - businessPeriod.Start, 0, 0)
          ) - beginOfBusinessPeriod;
    }
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
    if (_holidays == null)
      _holidays = new HolidayApiService().GetHolidays();

    var dateToCompare = new DateTime(2020, date.Month, date.Day, 0, 0, 0);

    DateTimeOffset convertedDate = new DateTimeOffset(dateToCompare);

    return _holidays.Any(p => p.Date == convertedDate);
  }

  private static bool CheckIsBusinessPeriod(
    DateTime date,
    TimePeriod businessPeriod
  )
  {
    var isBusinessPeriod = true;

    var beginOfBusinessPeriod = new DateTime(
      date.Year,
      date.Month,
      date.Day,
      businessPeriod.Start,
      0,
      0
    );

    var endOfBusinessPeriod = new DateTime(
      date.Year,
      date.Month,
      date.Day,
      businessPeriod.End,
      0,
      0
    );

    isBusinessPeriod =
      beginOfBusinessPeriod <= date
      && date <= endOfBusinessPeriod;

    return isBusinessPeriod;
  }
}