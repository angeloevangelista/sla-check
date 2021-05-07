using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using SlaCheck.Entities;

namespace SlaCheck.Services
{
  public class HolidayApiService
  {
    private readonly string _apiKey = AppSettings.HolidayApiKey;
    private readonly string _baseUrl = "https://holidayapi.com";
    private readonly HttpClient _httpClient;

    public HolidayApiService()
    {
      _httpClient = new HttpClient();
      _httpClient.BaseAddress = new Uri(_baseUrl);
    }

    public List<Holiday> GetHolidays()
    {
      var response = _httpClient.GetAsync(
        $"v1/holidays?key={_apiKey}&country=BR&year=2020"
      ).GetAwaiter().GetResult();

      var content = JsonConvert.DeserializeObject<HolidayApiResponse>(
        response.Content.ReadAsStringAsync().GetAwaiter().GetResult()
      );

      return content.Holidays;
    }
  }
}