using System;
using SlaChecker.Domain.Entities;
using SlaChecker.Implementation.Services;
using SlaChecker.Tests.Mocks.Services;
using Xunit;

namespace SlaChecker.Tests.Services.SlaServiceTests
{
  public class CheckIsBusinessDate
  {
    [Fact]
    public void NormalDay()
    {
      var holidayService = new HolidayApiServiceMock();
      var slaService = new SlaService(holidayService);

      slaService.CheckIsBusinessDate(
        businessPeriod: new TimePeriod(9, 18),
        date: new DateTime(2020, 2, 10, 9, 0, 0),
        slaIntervalInHours: 2,
        out TimeSpan remainingTime,
        out bool isBusinessPeriodOfDay,
        out bool isWeekend,
        out bool isHoliday
      );

      var isBusinessDate = isBusinessPeriodOfDay && (!isWeekend && !isHoliday);

      Assert.True(
        isBusinessDate,
        "Should assert that date is a business date."
      );
    }

    [Fact]
    public void Holiday()
    {
      var holidayService = new HolidayApiServiceMock();
      var slaService = new SlaService(holidayService);

      slaService.CheckIsBusinessDate(
        businessPeriod: new TimePeriod(9, 18),
        date: new DateTime(2020, 1, 1, 9, 0, 0),
        slaIntervalInHours: 2,
        out TimeSpan remainingTime,
        out bool isBusinessPeriodOfDay,
        out bool isWeekend,
        out bool isHoliday
      );

      Assert.True(isHoliday, "Should assert that date is a holiday.");
    }

    [Fact]
    public void Weekend()
    {
      var holidayService = new HolidayApiServiceMock();
      var slaService = new SlaService(holidayService);

      slaService.CheckIsBusinessDate(
        businessPeriod: new TimePeriod(9, 18),
        date: new DateTime(2020, 2, 8, 9, 0, 0),
        slaIntervalInHours: 2,
        out TimeSpan remainingTime,
        out bool isBusinessPeriodOfDay,
        out bool isWeekend,
        out bool isHoliday
      );

      Assert.True(isWeekend, "Should assert that date is on weekend.");
    }

    [Fact]
    public void HolidayOnTheWeekend()
    {
      var holidayService = new HolidayApiServiceMock();
      var slaService = new SlaService(holidayService);

      slaService.CheckIsBusinessDate(
        businessPeriod: new TimePeriod(9, 18),
        date: new DateTime(2020, 11, 15),
        slaIntervalInHours: 2,
        out TimeSpan remainingTime,
        out bool isBusinessPeriodOfDay,
        out bool isWeekend,
        out bool isHoliday
      );

      var isHolidayOnWeekend = isWeekend && isHoliday;

      Assert.True(
        isHolidayOnWeekend,
        "Should assert that date is a holiday on weekend."
      );
    }

    [Fact]
    public void BeforeBusinessPeriod()
    {
      var holidayService = new HolidayApiServiceMock();
      var slaService = new SlaService(holidayService);

      slaService.CheckIsBusinessDate(
        businessPeriod: new TimePeriod(9, 18),
        date: new DateTime(2020, 2, 10, 6, 0, 0),
        slaIntervalInHours: 2,
        out TimeSpan remainingTime,
        out bool isBusinessPeriodOfDay,
        out bool isWeekend,
        out bool isHoliday
      );

      Assert.False(
        isBusinessPeriodOfDay,
        "Should assert that date is out of business period, before that."
      );
    }

    [Fact]
    public void AfterBusinessPeriod()
    {
      var holidayService = new HolidayApiServiceMock();
      var slaService = new SlaService(holidayService);

      slaService.CheckIsBusinessDate(
        businessPeriod: new TimePeriod(9, 18),
        date: new DateTime(2020, 2, 10, 20, 0, 0),
        slaIntervalInHours: 2,
        out TimeSpan remainingTime,
        out bool isBusinessPeriodOfDay,
        out bool isWeekend,
        out bool isHoliday
      );

      Assert.False(
        isBusinessPeriodOfDay,
        "Should assert that date is out of business period, after that."
      );
    }
  }
}
