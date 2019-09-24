using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyTrackGuide.Auth
{
    public class SpotifyAuthentication
    {
        private readonly IConfiguration _config;

        public SpotifyAuthentication(IConfiguration config)
        {
            _config = config;
        }

        public string GetClientCredentialsAuthToken()
        {
            var spotifyClient = _config["ClientId"];
            var spotifySecret = _config["ClientSecret"];

            var webClient = new WebClient();

            var postparams = new NameValueCollection();
            postparams.Add("grant_type", "client_credentials");

            var authHeader = Convert.ToBase64String(Encoding.Default.GetBytes($"{spotifyClient}:{spotifySecret}"));
            webClient.Headers.Add(HttpRequestHeader.Authorization, "Basic " + authHeader);

            var tokenResponse = webClient.UploadValues("https://accounts.spotify.com/api/token", postparams);

            return Encoding.UTF8.GetString(tokenResponse);
        }

        
    }
}
