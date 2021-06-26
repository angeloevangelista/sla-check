using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using SlaChecker.Domain.Entities;
using SlaChecker.Domain.Entities.HolidayApi;
using SlaChecker.Domain.Services;

namespace SlaChecker.Implementation.Services
{
  public class HolidayApiService : IHolidayService
  {
    private readonly string _apiKey;
    private readonly HttpClient _httpClient;

    public HolidayApiService(string holidayApiKey)
    {
      _apiKey = holidayApiKey;
      _httpClient = new HttpClient();
      _httpClient.BaseAddress = new Uri("https://holidayapi.com");
    }

    public List<Holiday> ListHolidays(int year)
    {
      var response = _httpClient.GetAsync(
        $"v1/holidays?key={_apiKey}&country=BR&year={year}"
      )
      .GetAwaiter()
      .GetResult()
      .Content
      .ReadAsStringAsync()
      .GetAwaiter()
      .GetResult();

      var holidayApiResponse = JsonConvert
        .DeserializeObject<HolidayApiResponse>(
          response
        );

      if (holidayApiResponse.Status != 200)
        throw new Exception(
          $"HolidayApi Exception: {holidayApiResponse.Error}"
        );

      var holidays = ConvertHolidayToInternType(holidayApiResponse);

      return holidays;
    }

    private static List<Holiday> ConvertHolidayToInternType(
      HolidayApiResponse apiResponse
    )
    {
      var holidays = apiResponse.Holidays.Select(p => new Holiday()
      {
        Id = p.Uuid,
        Name = p.Name,
        Date = p.Date.DateTime,
        Country = p.Country
      })
      .ToList();

      return holidays;
    }
  }
}