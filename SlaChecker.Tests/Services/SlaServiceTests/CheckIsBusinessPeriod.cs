using Xunit;
using SlaChecker.Tests.Mocks.Services;
using SlaChecker.Implementation.Services;
using System;
using SlaChecker.Domain.Entities;

namespace SlaChecker.Tests.Services.SlaServiceTests
{
  public class CheckIsBusinessPeriod
  {
    [Fact]
    public void BetweenBusinessPeriod()
    {
      var holidayService = new HolidayApiServiceMock();
      var slaService = new SlaService(holidayService);

      var isBusinessPeriod = slaService.CheckIsBusinessPeriod(
        date: new DateTime(2020, 2, 10, 14, 0, 0),
        businessPeriod: new TimePeriod(9, 18)
      );

      Assert.True(
        isBusinessPeriod,
        "Should return TRUE given the hour is between begin and end of the business period."
      );
    }

    [Fact]
    public void ExactBeginOfBusinessPeriod()
    {
      var holidayService = new HolidayApiServiceMock();
      var slaService = new SlaService(holidayService);

      var isBusinessPeriod = slaService.CheckIsBusinessPeriod(
        date: new DateTime(2020, 2, 10, 9, 0, 0),
        businessPeriod: new TimePeriod(9, 18)
      );

      Assert.True(
        isBusinessPeriod,
        "Should return TRUE given the hour is exactly the begin of the business period."
      );
    }

    [Fact]
    public void ExactEndOfBusinessPeriod()
    {
      var holidayService = new HolidayApiServiceMock();
      var slaService = new SlaService(holidayService);

      var isBusinessPeriod = slaService.CheckIsBusinessPeriod(
        date: new DateTime(2020, 2, 10, 18, 0, 0),
        businessPeriod: new TimePeriod(9, 18)
      );

      Assert.True(
        isBusinessPeriod,
        "Should return TRUE given the hour is exactly the end of the business period."
      );
    }

    [Fact]
    public void BeforeBusinessPeriod()
    {
      var holidayService = new HolidayApiServiceMock();
      var slaService = new SlaService(holidayService);

      var isBusinessPeriod = slaService.CheckIsBusinessPeriod(
        date: new DateTime(2020, 2, 10, 8, 0, 0),
        businessPeriod: new TimePeriod(9, 18)
      );

      Assert.False(
        isBusinessPeriod,
        "Should return FALSE given the hour is before the business period."
      );
    }

    [Fact]
    public void AfterBusinessPeriod()
    {
      var holidayService = new HolidayApiServiceMock();
      var slaService = new SlaService(holidayService);

      var isBusinessPeriod = slaService.CheckIsBusinessPeriod(
        date: new DateTime(2020, 2, 10, 19, 0, 0),
        businessPeriod: new TimePeriod(9, 18)
      );

      Assert.False(
        isBusinessPeriod,
        "Should return FALSE given the hour is after the business period."
      );
    }
  }
}
