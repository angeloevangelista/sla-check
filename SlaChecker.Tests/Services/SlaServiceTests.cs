using System;
using Xunit;
using SlaChecker.Domain.Entities;
using SlaChecker.Tests.Mocks.Services;
using SlaChecker.Implementation.Services;

namespace SlaChecker.Tests.Services
{
  public class SlaServiceTests
  {
    [Fact]
    public void Test1()
    {
      var holidayService = new HolidayApiServiceMock();
      var SlaService = new SlaService(holidayService);

      var slaIsValid = SlaService.CheckSlaValidity(
        initialDate: new DateTime(2020, 1, 1, 9, 0, 0),
        currentDate: new DateTime(2020, 1, 2, 12, 0, 0),
        businessPeriod: new TimePeriod(9, 18),
        slaIntervalInHours: 2
      );

      // It's holiday
      // One hour after expiration

      Assert.False(
        slaIsValid, 
        "Should return false given that SLA period has expired 1 hours ago."
      );
    }
  }
}
