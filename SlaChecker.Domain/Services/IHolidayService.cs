using System.Collections.Generic;
using SlaChecker.Domain.Entities;

namespace SlaChecker.Domain.Services
{
  public interface IHolidayService
  {
    List<Holiday> ListHolidays(int year);
  }
}