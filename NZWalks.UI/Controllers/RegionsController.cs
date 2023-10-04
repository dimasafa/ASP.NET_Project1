using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models;
using NZWalks.UI.Models.DTO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace NZWalks.UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            // Get All Regions from Web Api

            List<RegionDto> response = new List<RegionDto>();

            try
            {
                // Erstellen neu httpClient, um Api zu benutzen.
                var client = httpClientFactory.CreateClient();

                // Anwenden Get Method aus unsere API mit richtige url
                var httpResponeMessage = await client.GetAsync("https://localhost:7202/api/regions");

                // Prüfen, ob unsere Anfrage efolgsreich wurde. Wenn es false wird, dann wird eine Exception schicken. Deswegen liegen wir den Code in try-catch
                httpResponeMessage.EnsureSuccessStatusCode();

                // Da es in try, dann es bedeuted, dass httpResponeMethod - true ist. Read die daten aus api. await httpResponeMessage.Content.ReadAsStringAsync(); - from aussen api
                response.AddRange(await httpResponeMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>());

                // Liegen die daten in Viewbag, und diese Viewbag schon in front in index.cshtml zu zeigen.
                //ViewBag.stringResponseBody = ResponseBody;

            }
            catch (Exception ex)
            {
                // Log the Exception

            }
            return View(response);

        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Add(AddRegionViewModel model)
        {
            // Erstellen den Client
            var client = httpClientFactory.CreateClient();

            // Message von httpRequest
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7202/api/regions"),
                Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json")
            };

            // Schicken async die daten mit request
            var httpResponseMessage = await client.SendAsync(httpRequestMessage);

            httpResponseMessage.EnsureSuccessStatusCode();

            var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDto>();

            if (response is not null)
            {
                return RedirectToAction("Index", "Regions");
            }

            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            // Erstellen den Client
            var client = httpClientFactory.CreateClient();

            var response = await client.GetFromJsonAsync<RegionDto>($"https://localhost:7202/api/regions/{id.ToString()}");

            if (response is not null)
            {
                return View(response);
            }

            return View(null);
        }

        [HttpPut]

        public async Task<IActionResult> Edit(RegionDto request)
        {
            // Erstellen den Client
            var client = httpClientFactory.CreateClient();

            // Message von httpRequest
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"https://localhost:7202/api/regions/{request.id}"),
                Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
            };

            var httpResponseMessage = await client.SendAsync(httpRequestMessage);

            httpResponseMessage.EnsureSuccessStatusCode();

            var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDto>();

            if (response is not null)
            {
                return RedirectToAction("Edit", "Regions");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(RegionDto request)
        {


            try
            {           
                // Erstellen den Client
                var client = httpClientFactory.CreateClient();

                var httpResponseMessage = await client.DeleteAsync($"https://localhost:7202/api/regions/{request.id}");

                httpResponseMessage.EnsureSuccessStatusCode();

                return RedirectToAction("Index", "Regions");
            }
            catch (Exception ex)
            {

            }

            return View("Edit");

        }

    }
}
