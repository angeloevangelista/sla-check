using System;
using SlaChecker.Domain.Entities;

namespace SlaChecker.Domain.Services
{
  public interface ISlaService
  {
    bool CheckSlaValidity(
      DateTime initialDate,
      DateTime currentDate,
      TimePeriod businessPeriod,
      int slaIntervalInHours
    );

    DateTime GetSlaExpirationDate(
      DateTime initialDate,
      TimePeriod businessPeriod,
      int slaIntervalInHours
    );

    void CheckIsBusinessDate(
      TimePeriod businessPeriod,
      DateTime slaExpirationDate,
      int slaIntervalInHours,
      out TimeSpan remainingTime,
      out bool isBusinessPeriodOfDay,
      out bool isWeekend,
      out bool isHoliday
    );

    bool CheckIsBusinessPeriod(
      DateTime date,
      TimePeriod businessPeriod
    );
  }
}