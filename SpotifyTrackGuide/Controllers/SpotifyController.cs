using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;
using SpotifyTrackGuide.Auth;
using SpotifyTrackGuide.Helper;
using SpotifyTrackGuide.Models;

namespace SpotifyTrackGuide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpotifyController : ControllerBase
    {
        private Spotify _spotify;
        private readonly IMemoryCache _cache;

        public SpotifyController(IConfiguration config, IMemoryCache cache)
        {
            _spotify = new Spotify(config);
            _cache = cache;
        }

        [HttpGet("/api/playlist/{id}")]
        public ActionResult<TrackAverage> GetPlayList(string id)
        {
            var response = _spotify.GetPlayList(id);
            return response;

        }

        [HttpGet("/api/users/{Id}/stats")]
        public string GetUserPlayList(string Id)
        {
            var cacheEntry = _cache.GetOrCreate("CacheUserPlayList", entry =>
            {
                // Cache for 1 minute. Increase if needed
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60);

                var response = _spotify.GetPlatListStats(Id);
                return JsonConvert.SerializeObject(response);
            });

            return cacheEntry;
            
        }



        }

 }