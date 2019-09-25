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
        private readonly IConfiguration _config;
        private Spotify _spotify;
        private readonly IMemoryCache _cache;

        public SpotifyController(IConfiguration config, IMemoryCache cache)
        {
            _spotify = new Spotify(config);
            _cache = cache;
        }

        //// GET api/values
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<string>>> GetAsync()
        //{
        //    #region OldCode
        //    //PrivateProfile profile = await _api.GetPrivateProfileAsync();
        //    //if (!profile.HasError())
        //    //{
        //    //    return new string[] { profile.DisplayName };
        //    //}

        //    //CredentialsAuth auth = new CredentialsAuth("d2b2ac4c97184c698277356646f044d0", "e7cf547744434a4fbfeb130f60281ef1");
        //    //Token token = await auth.GetToken();
        //    //_api = new SpotifyWebAPI()
        //    //{
        //    //    AccessToken = token.AccessToken,
        //    //    TokenType = token.TokenType,

        //    //};
        //    #endregion

        //    FullTrack track = await _api.GetTrackAsync("3Hvu1pq89D4R0lyPBoujSv");

        //    return new string[] { "ERROR" };
        //}

        [HttpGet("/api/playlist/{id}")]
        public async Task<ActionResult<TrackAverage>> GetPlayList(string id)
        {
            var response = _spotify.GetPLayList(id);
            return response;

            #region older code
            //List<float> fDance = new List<float>();
            //List<float> fEnergy = new List<float>();
            //List<float> fValence = new List<float>();

            //var playList = await _api.GetPlaylistTracksAsync(id, "", 100, 0, "");
            //foreach(var list in playList.Items)
            //{
            //    var song = _api.GetAudioFeaturesAsync(list.Track.Id);

            //    // Storing the values for later calculation
            //    fDance.Add(song.Result.Danceability);
            //    fEnergy.Add(song.Result.Energy);
            //    fValence.Add(song.Result.Valence);
            //}


            //TrackAverage average = new TrackAverage
            //{
            //    danceabilityAverage = Calculate.GetAverageTotal(fDance),
            //    danceability = new Danceability
            //    {
            //        min = Calculate.GetMinValue(fDance),
            //        max = Calculate.GetMaxValue(fDance)
            //    },
            //    energyAverage = Calculate.GetAverageTotal(fEnergy),
            //    energy = new Energy
            //    {
            //        min = Calculate.GetMinValue(fEnergy),
            //        max = Calculate.GetMaxValue(fEnergy)
            //    },
            //    valenceAverage = Calculate.GetAverageTotal(fValence),
            //    valence = new Valence
            //    {
            //        min = Calculate.GetMinValue(fValence),
            //        max = Calculate.GetMaxValue(fValence)
            //    }

            //};

            //return average;
            #endregion
        }

        [HttpGet("/api/users/{Id}/stats")]
        public string GetUserPlayList(string Id)
        {
            var cacheEntry = _cache.GetOrCreate("CacheUserPlayList", entry =>
            {
                // Cache for 1 minutes. Increase if needed
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60);

                var response = _spotify.GetPlatListStats(Id);
                return JsonConvert.SerializeObject(response);
            });

            return cacheEntry;
            
        }



        }

 }