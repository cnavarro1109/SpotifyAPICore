using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;
using SpotifyTrackGuide.Auth;

namespace SpotifyTrackGuide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpotifyController : ControllerBase
    {
        private readonly IConfiguration _config;
        private SpotifyWebAPI _api;

        public SpotifyController(IConfiguration config)
        {
            _config = config;

            SpotifyAuthentication token = new SpotifyAuthentication(_config);
            var bearer = JsonConvert.DeserializeObject<Dictionary<string, string>>(token.GetClientCredentialsAuthToken());
            _api = new SpotifyWebAPI
            {
                AccessToken = bearer["access_token"],
                TokenType = bearer["token_type"]
            };
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetAsync()
        {

            //PrivateProfile profile = await _api.GetPrivateProfileAsync();
            //if (!profile.HasError())
            //{
            //    return new string[] { profile.DisplayName };
            //}

            //CredentialsAuth auth = new CredentialsAuth("d2b2ac4c97184c698277356646f044d0", "e7cf547744434a4fbfeb130f60281ef1");
            //Token token = await auth.GetToken();
            //_api = new SpotifyWebAPI()
            //{
            //    AccessToken = token.AccessToken,
            //    TokenType = token.TokenType,
                
            //};

            FullTrack track = _api.GetTrack("3Hvu1pq89D4R0lyPBoujSv");
            // PrivateProfile profile = await _api.GetPrivateProfileAsync();

            return new string[] { "ERROR" };
        }

 
    }
}