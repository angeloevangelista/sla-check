using System;
using System.Linq;
using SlaChecker.Domain.Entities;
using SlaChecker.Domain.Services;

namespace SlaChecker.Implementation.Services
{
  public class SlaService : ISlaService
  {
    private readonly IHolidayService _holidayService;

    public SlaService(IHolidayService holidayService)
    {
      this._holidayService = holidayService;
    }

    public bool CheckSlaValidity(
      DateTime initialDate,
      DateTime currentDate,
      TimePeriod businessPeriod,
      int slaIntervalInHours
    )
    {
      bool slaIsValid;

      var slaExpirationDate = GetSlaExpirationDate(
        initialDate,
        businessPeriod,
        slaIntervalInHours
      );

      slaIsValid = currentDate < slaExpirationDate;

      return slaIsValid;
    }

    public DateTime GetSlaExpirationDate(
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

      var currentRemainingTime = remainingTime;

      while (!isValidDate || currentRemainingTime.TotalSeconds > 0)
      {
        var nextBusinessPeriodBegin = new DateTime(
          slaExpirationDate.Year,
          slaExpirationDate.Month,
          slaExpirationDate.Day,
          businessPeriod.Start,
          0,
          0
        ).AddDays(1);

        slaExpirationDate = currentRemainingTime.TotalSeconds > 0
          ? nextBusinessPeriodBegin.AddHours(currentRemainingTime.Hours)
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

        if (remainingTime.TotalSeconds > 0)
          currentRemainingTime = remainingTime;

        if (isValidDate)
          currentRemainingTime = new TimeSpan();
      }

      return slaExpirationDate;
    }

    public void CheckIsBusinessDate(
      TimePeriod businessPeriod,
      DateTime date,
      int slaIntervalInHours,
      out TimeSpan remainingTime,
      out bool isBusinessPeriodOfDay,
      out bool isWeekend,
      out bool isHoliday
    )
    {
      isBusinessPeriodOfDay = CheckIsBusinessPeriod(
        date,
        businessPeriod
      );

      if (isBusinessPeriodOfDay)
        remainingTime = new TimeSpan();
      else
      {
        var originalDate = (date.AddHours(-slaIntervalInHours));

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

        var expirationBeginDiff = date - beginOfBusinessPeriod;
        var expirationEndDiff = date - endOfBusinessPeriod;

        remainingTime = expirationBeginDiff < expirationEndDiff
          ? new TimeSpan()
          : date.Subtract(
              new TimeSpan(businessPeriod.End - businessPeriod.Start, 0, 0)
            ) - beginOfBusinessPeriod;
      }

      isWeekend = CheckIsWeekend(
        date.Add(remainingTime)
      );

      isHoliday = CheckIsHoliday(
        date.Add(remainingTime)
      );
    }

    public bool CheckIsBusinessPeriod(DateTime date, TimePeriod businessPeriod)
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

    private static bool CheckIsWeekend(DateTime date)
    {
      var weekendDays = new DayOfWeek[] {
        DayOfWeek.Saturday,
        DayOfWeek.Sunday
      };

      return weekendDays.Contains(date.DayOfWeek);
    }

    private bool CheckIsHoliday(DateTime date)
    {
      var holidays = this._holidayService.ListHolidays(date.Year);

      return holidays.Any(p => p.Date.Date == date.Date);
    }
  }
}