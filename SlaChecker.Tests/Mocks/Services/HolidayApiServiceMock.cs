using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SlaChecker.Domain.Entities;
using SlaChecker.Domain.Services;

namespace SlaChecker.Tests.Mocks.Services
{
  public class HolidayApiServiceMock : IHolidayService
  {
    private readonly IReadOnlyCollection<Holiday> _holidays;

    public HolidayApiServiceMock()
    {
      this._holidays = new List<Holiday>()
      {
        new Holiday()
          {
            Id = new Guid("b58254f9-b38b-42c1-8b30-95a095798b0c"),
            Name = "New Year's Day",
            Date = new DateTime(2020, 01, 01),
            Country = "BR"
          },
          new Holiday()
          {
            Id = new Guid("26346ac8-b1c7-4dfb-bda2-64c1ca445cc9"),
            Name = "Shrove Monday",
            Date = new DateTime(2020, 02, 24),
            Country = "BR"
          },
          new Holiday()
          {
            Id = new Guid("4a764c30-0b8e-4182-b89d-edb419213c7b"),
            Name = "Shrove Tuesday",
            Date = new DateTime(2020, 02, 25),
            Country = "BR"
          },
          new Holiday()
          {
            Id = new Guid("2190c592-1eef-4448-a2ce-2498f7ffdefd"),
            Name = "World Wildlife Day",
            Date = new DateTime(2020, 03, 03),
            Country = "BR"
          },
          new Holiday()
          {
            Id = new Guid("709c2831-1a6c-4ed6-a671-9104fa983980"),
            Name = "March Equinox",
            Date = new DateTime(2020, 03, 20),
            Country = "BR"
          },
          new Holiday()
          {
            Id = new Guid("669c4067-8115-4148-830f-a3a5f1be4d56"),
            Name = "Good Friday",
            Date = new DateTime(2020, 04, 10),
            Country = "BR"
          },
          new Holiday()
          {
            Id = new Guid("3fb4f766-2f02-4a6b-8fd1-ac8250755e24"),
            Name = "Easter",
            Date = new DateTime(2020, 04, 12),
            Country = "BR"
          },
          new Holiday()
          {
            Id = new Guid("72614f29-7826-4e89-83d5-b7563ed89576"),
            Name = "Tiradentes",
            Date = new DateTime(2020, 04, 21),
            Country = "BR"
          },
          new Holiday()
          {
            Id = new Guid("9fcb2d8b-5b6c-4b9f-b654-24661a47c1b0"),
            Name = "Labor Day",
            Date = new DateTime(2020, 05, 01),
            Country = "BR"
          },
          new Holiday()
          {
            Id = new Guid("b3f263a6-1442-42c9-ae2e-a0bef6cc2899"),
            Name = "Mother's Day",
            Date = new DateTime(2020, 05, 10),
            Country = "BR"
          },
          new Holiday()
          {
            Id = new Guid("725dde73-304e-4214-b75a-cf951fe400b0"),
            Name = "Feast of Corpus Christi",
            Date = new DateTime(2020, 06, 11),
            Country = "BR"
          },
          new Holiday()
          {
            Id = new Guid("92743aca-67d8-446b-8702-beb3ad4a1547"),
            Name = "June Solstice",
            Date = new DateTime(2020, 06, 20),
            Country = "BR"
          },
          new Holiday()
          {
            Id = new Guid("902eadef-c017-4e7a-aeb3-8a1bdeb48447"),
            Name = "International Asteroid Day",
            Date = new DateTime(2020, 06, 30),
            Country = "BR"
          },
          new Holiday()
          {
            Id = new Guid("d179c577-45f9-427e-9822-2c886860025e"),
            Name = "Nelson Mandela International Day",
            Date = new DateTime(2020, 07, 18),
            Country = "BR"
          },
          new Holiday()
          {
            Id = new Guid("110c2bb2-d637-4328-9890-e5b0a95f7678"),
            Name = "Father's Day",
            Date = new DateTime(2020, 08, 09),
            Country = "BR"
          },
          new Holiday()
          {
            Id = new Guid("c93d54ac-4b68-4198-bdd3-dfa28b59ab9f"),
            Name = "Independence Day",
            Date = new DateTime(2020, 09, 07),
            Country = "BR"
          },
          new Holiday()
          {
            Id = new Guid("658b5d7d-a8ad-48bb-b7c1-b69a88ce8cfb"),
            Name = "September Equinox",
            Date = new DateTime(2020, 09, 22),
            Country = "BR"
          },
          new Holiday()
          {
            Id = new Guid("70aed7b0-efd0-4997-bf3d-b9f1936c8ae8"),
            Name = "Our Lady of Aparecida",
            Date = new DateTime(2020, 10, 12),
            Country = "BR"
          },
          new Holiday()
          {
            Id = new Guid("12fec325-5fa6-4618-9262-0b21ca902405"),
            Name = "Day of the Dead",
            Date = new DateTime(2020, 11, 02),
            Country = "BR"
          },
          new Holiday()
          {
            Id = new Guid("bcbc4805-b8b4-434b-bbf4-b7b22ff4a4a8"),
            Name = "Proclamation of the Republic",
            Date = new DateTime(2020, 11, 15),
            Country = "BR"
          },
          new Holiday()
          {
            Id = new Guid("56f8dff3-1daa-4e15-83e0-e7a1f3fab585"),
            Name = "December Solstice",
            Date = new DateTime(2020, 12, 21),
            Country = "BR"
          },
          new Holiday()
          {
            Id = new Guid("6d608483-6fc8-4386-8e09-1cb522707532"),
            Name = "Christmas Eve",
            Date = new DateTime(2020, 12, 24),
            Country = "BR"
          },
          new Holiday()
          {
            Id = new Guid("9e92052c-9f89-4a76-bb46-5adca4180b9a"),
            Name = "Christmas Day",
            Date = new DateTime(2020, 12, 25),
            Country = "BR"
          },
          new Holiday()
          {
            Id = new Guid("5965525a-03a7-4b65-ac79-6a461ce61162"),
            Name = "New Year's Eve",
            Date = new DateTime(2020, 12, 31),
            Country = "BR"
          }
      };
    }

    public List<Holiday> ListHolidays(int year)
    {
      return _holidays
        .Where(p => p.Date.Year == year)
        .ToList();
    }
  }
}