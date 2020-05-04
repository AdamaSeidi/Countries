
namespace Countries.Services
{
    using Countries;
    using Countries.Models;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    public class ApiService
    {
        public async Task<Response> GetCountries(string baseUrl, string controller)
		{
			try
			{
				var client = new HttpClient();

				client.BaseAddress = new Uri(baseUrl);

				var status = await client.GetAsync(controller);

				var result = await status.Content.ReadAsStringAsync();

				if (!status.IsSuccessStatusCode)
				{
					return new Response
					{
						IsSuccess = true,
						Message = result,
					};
				}

				var countries = JsonConvert.DeserializeObject<List<Country>>(result);
				return new Response
				{
					IsSuccess = true,
					Result = countries,
				};
			}
			catch (Exception e)
			{
				return new Response
				{
					IsSuccess = false,
					Message = e.Message,
				};
			}
		}
    }
}
