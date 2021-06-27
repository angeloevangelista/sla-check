using System;
using Xunit;
using SlaChecker.Domain.Entities;
using SlaChecker.Tests.Mocks.Services;
using SlaChecker.Implementation.Services;

namespace SlaChecker.Tests.Services.SlaServiceTests
{
  public class CheckSlaValidity
  {
    [Fact]
    public void CommonValidPeriod()
    {
      var holidayService = new HolidayApiServiceMock();
      var slaService = new SlaService(holidayService);

      var slaIsValid = slaService.CheckSlaValidity(
        initialDate: new DateTime(2020, 2, 1, 10, 0, 0),
        currentDate: new DateTime(2020, 2, 1, 14, 0, 0),
        businessPeriod: new TimePeriod(9, 18),
        slaIntervalInHours: 6
      );

      Assert.True(
        slaIsValid,
        "Should return TRUE given that SLA period has 2 hours resting."
      );
    }

    [Fact]
    public void CommonExpiredPeriod()
    {
      var holidayService = new HolidayApiServiceMock();
      var slaService = new SlaService(holidayService);

      var slaIsValid = slaService.CheckSlaValidity(
        initialDate: new DateTime(2020, 2, 10, 10, 0, 0),
        currentDate: new DateTime(2020, 2, 10, 15, 0, 0),
        businessPeriod: new TimePeriod(9, 18),
        slaIntervalInHours: 4
      );

      Assert.False(
        slaIsValid,
        "Should return FALSE given that SLA period has expired 2 hours ago."
      );
    }

    [Fact]
    public void ValidPeriodWithSlaIntervalLargerThanBusinessPeriod()
    {
      var holidayService = new HolidayApiServiceMock();
      var slaService = new SlaService(holidayService);

      var slaIsValid = slaService.CheckSlaValidity(
        initialDate: new DateTime(2020, 2, 10, 10, 0, 0),
        currentDate: new DateTime(2020, 2, 13, 13, 0, 0),
        businessPeriod: new TimePeriod(9, 18),
        slaIntervalInHours: 24 + 6
      );

      Assert.True(
        slaIsValid,
        "Should return TRUE given that SLA period has 2 hours remaining, after 3 days of SLA."
      );
    }

    [Fact]
    public void ExpiredPeriodWithSlaIntervalLargerThanBusinessPeriod()
    {
      var holidayService = new HolidayApiServiceMock();
      var slaService = new SlaService(holidayService);

      var slaIsValid = slaService.CheckSlaValidity(
        initialDate: new DateTime(2020, 2, 10, 10, 0, 0),
        currentDate: new DateTime(2020, 2, 13, 13, 0, 0),
        businessPeriod: new TimePeriod(9, 18),
        slaIntervalInHours: 24 + 2
      );

      Assert.False(
        slaIsValid,
        "Should return FALSE given that SLA period has expired 2 hours ago, after 3 days of SLA."
      );
    }

    [Fact]
    public void ValidPeriodAfterWeekend()
    {
      var holidayService = new HolidayApiServiceMock();
      var slaService = new SlaService(holidayService);

      var slaIsValid = slaService.CheckSlaValidity(
        initialDate: new DateTime(2020, 1, 31, 18, 0, 0),
        currentDate: new DateTime(2020, 2, 3, 10, 0, 0),
        businessPeriod: new TimePeriod(9, 18),
        slaIntervalInHours: 2
      );

      Assert.True(
        slaIsValid,
        "Should return TRUE given that SLA period has 1 remaining hour after weekend."
      );
    }

    [Fact]
    public void ExpiredPeriodAfterWeekend()
    {
      var holidayService = new HolidayApiServiceMock();
      var slaService = new SlaService(holidayService);

      var slaIsValid = slaService.CheckSlaValidity(
        initialDate: new DateTime(2020, 1, 31, 18, 0, 0),
        currentDate: new DateTime(2020, 2, 3, 14, 0, 0),
        businessPeriod: new TimePeriod(9, 18),
        slaIntervalInHours: 2
      );

      Assert.False(
        slaIsValid,
        "Should return FALSE given that SLA period has expired one hour ago, after a weekend."
      );
    }

    [Fact]
    public void ExactExpirationPeriod()
    {
      var holidayService = new HolidayApiServiceMock();
      var slaService = new SlaService(holidayService);

      var slaIsValid = slaService.CheckSlaValidity(
        initialDate: new DateTime(2020, 2, 1, 10, 0, 0),
        currentDate: new DateTime(2020, 2, 1, 14, 0, 0),
        businessPeriod: new TimePeriod(9, 18),
        slaIntervalInHours: 4
      );

      Assert.True(
        slaIsValid,
        "Should return TRUE given that SLA period has not expired yet."
      );
    }

    [Fact]
    public void ExpiredOneHourAfterHoliday()
    {
      var holidayService = new HolidayApiServiceMock();
      var slaService = new SlaService(holidayService);

      var slaIsValid = slaService.CheckSlaValidity(
        initialDate: new DateTime(2020, 1, 1, 9, 0, 0),
        currentDate: new DateTime(2020, 1, 2, 12, 0, 0),
        businessPeriod: new TimePeriod(9, 18),
        slaIntervalInHours: 2
      );

      Assert.False(
        slaIsValid,
        "Should return FALSE given that SLA period has expired 1 hours ago."
      );
    }

    [Fact]
    public void YetValidForOneHourAfterHoliday()
    {
      var holidayService = new HolidayApiServiceMock();
      var slaService = new SlaService(holidayService);

      var slaIsValid = slaService.CheckSlaValidity(
        initialDate: new DateTime(2020, 1, 1, 9, 0, 0),
        currentDate: new DateTime(2020, 1, 2, 10, 0, 0),
        businessPeriod: new TimePeriod(9, 18),
        slaIntervalInHours: 2
      );

      Assert.True(
        slaIsValid,
        "Should return TRUE given that SLA period has 1 hour resting."
      );
    }
  }
}
