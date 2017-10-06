using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace IdentityServerApp.ClientApp.Controllers
{
    [Route("[controller]")]
    public class IdentityController : Controller
    {

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //Buscar o token no IdentityServer
            var discovery =
                await DiscoveryClient.GetAsync("http://localhost:54098");

            var tokenClient = new TokenClient(discovery.TokenEndpoint,
                                              "clientApp",
                                              "secret");
            var tokenResponse =
                await tokenClient.RequestClientCredentialsAsync("apiApp");

            ViewData["tokenResult"] = tokenResponse.IsError
                                      ? tokenResponse.Error
                                      : tokenResponse.Json.ToString();

            //Dar get na api protegida
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            var apiResponse =
                await client.GetAsync("http://localhost:54288/api/identity");

            ViewData["apiResult"] = apiResponse.IsSuccessStatusCode
                                    ? await apiResponse.Content.ReadAsStringAsync()
                                    : apiResponse.StatusCode.ToString();

            return View();
        }

    }
}
