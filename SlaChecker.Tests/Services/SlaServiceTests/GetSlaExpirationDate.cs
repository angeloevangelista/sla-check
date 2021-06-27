using System;
using SlaChecker.Domain.Entities;
using SlaChecker.Implementation.Services;
using SlaChecker.Tests.Mocks.Services;
using Xunit;

namespace SlaChecker.Tests.Services.SlaServiceTests
{
  public class GetSlaExpirationDate
  {
    [Fact]
    public void TwoSlaHoursOnSameDay()
    {
      var holidayService = new HolidayApiServiceMock();
      var slaService = new SlaService(holidayService);

      DateTime initialDate = new DateTime(2020, 2, 10, 9, 0, 0);

      var slaExpirationDate = slaService.GetSlaExpirationDate(
        initialDate,
        businessPeriod: new TimePeriod(9, 18),
        slaIntervalInHours: 2
      );

      var expectedDate = new DateTime(2020, 2, 10, 11, 0, 0);

      var datesMatch = slaExpirationDate.CompareTo(expectedDate) == 0;

      Assert.True(datesMatch, "Should calculate expiration date to same day.");
    }

    [Fact]
    public void FourSlaHoursExceedingOne()
    {
      var holidayService = new HolidayApiServiceMock();
      var slaService = new SlaService(holidayService);

      DateTime initialDate = new DateTime(2020, 2, 10, 15, 0, 0);

      var slaExpirationDate = slaService.GetSlaExpirationDate(
        initialDate,
        businessPeriod: new TimePeriod(9, 18),
        slaIntervalInHours: 4
      );

      var expectedDate = new DateTime(2020, 2, 11, 10, 0, 0);

      var datesMatch = slaExpirationDate.CompareTo(expectedDate) == 0;

      Assert.True(
        datesMatch,
        "Should calculate expiration date to the next day."
      );
    }

    [Fact]
    public void TwentySixSlaHoursExceedingTwo()
    {
      var holidayService = new HolidayApiServiceMock();
      var slaService = new SlaService(holidayService);

      DateTime initialDate = new DateTime(2020, 2, 10, 9, 0, 0);

      var slaExpirationDate = slaService.GetSlaExpirationDate(
        initialDate,
        businessPeriod: new TimePeriod(9, 18),
        slaIntervalInHours: 26
      );

      var expectedDate = new DateTime(2020, 2, 13, 11, 0, 0);

      var datesMatch = slaExpirationDate.CompareTo(expectedDate) == 0;

      Assert.True(
        datesMatch,
        "Should calculate expiration date to the 3 following days and 2 hours."
      );
    }

    [Fact]
    public void FourSlaHoursExceedingThreeToHoliday()
    {
      var holidayService = new HolidayApiServiceMock();
      var slaService = new SlaService(holidayService);

      DateTime initialDate = new DateTime(2020, 11, 14, 17, 0, 0);

      var slaExpirationDate = slaService.GetSlaExpirationDate(
        initialDate,
        businessPeriod: new TimePeriod(9, 18),
        slaIntervalInHours: 4
      );

      var expectedDate = new DateTime(2020, 11, 16, 12, 0, 0);

      var datesMatch = slaExpirationDate.CompareTo(expectedDate) == 0;

      Assert.True(
        datesMatch,
        "Should calculate expiration date to the following day, which is a holiday."
      );
    }

    [Fact]
    public void FourSlaHoursExceedingThreeToWeekend()
    {
      var holidayService = new HolidayApiServiceMock();
      var slaService = new SlaService(holidayService);

      DateTime initialDate = new DateTime(2020, 2, 7, 17, 0, 0);

      var slaExpirationDate = slaService.GetSlaExpirationDate(
        initialDate,
        businessPeriod: new TimePeriod(9, 18),
        slaIntervalInHours: 4
      );

      var expectedDate = new DateTime(2020, 2, 10, 12, 0, 0);

      var datesMatch = slaExpirationDate.CompareTo(expectedDate) == 0;

      Assert.True(
        datesMatch,
        "Should calculate expiration date to the following weekend."
      );
    }
  }
}
