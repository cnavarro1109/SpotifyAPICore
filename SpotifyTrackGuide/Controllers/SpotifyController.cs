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
using SpotifyTrackGuide.Helper;
using SpotifyTrackGuide.Models;

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
            #region OldCode
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
            #endregion

            FullTrack track = await _api.GetTrackAsync("3Hvu1pq89D4R0lyPBoujSv");
            // PrivateProfile profile = await _api.GetPrivateProfileAsync();

            return new string[] { "ERROR" };
        }

        //[HttpGet("{id}")]
        //public async Task<ActionResult<Paging<SimplePlaylist>>> GetSavedAlbumsAsync(int id)
        //{
        //    var userPlayList = await _api.GetUserPlaylistsAsync("cn1109", 20, 0);
        //    foreach (var list in userPlayList.Items)
        //    {
        //        var playList = await _api.GetPlaylistTracksAsync(list.Id, "", 100, 0, "");
        //        foreach (var track in playList.Items)
        //        {
        //            var response = await _api.GetTrackAsync(track.Track.Id);
        //            var dance = await _api.GetAudioFeaturesAsync(response.Id);
        //        }
        //    }
        //    return userPlayList;
        //    // savedAlbums.Items.ForEach(album => Console.WriteLine(album.Album.Name));
        //}

        [HttpGet("/api/playlist/{id}")]
        public async Task<ActionResult<TrackAverage>> GetPlayList(string id)
        {
            List<float> fDance = new List<float>();
            List<float> fEnergy = new List<float>();
            List<float> fValence = new List<float>();

            var playList = await _api.GetPlaylistTracksAsync(id, "", 100, 0, "");
            foreach(var list in playList.Items)
            {
                var song = _api.GetAudioFeaturesAsync(list.Track.Id);

                // Storing the values for later calculation
                fDance.Add(song.Result.Danceability);
                fEnergy.Add(song.Result.Energy);
                fValence.Add(song.Result.Valence);
            }


            TrackAverage average = new TrackAverage
            {
                danceabilityAverage = Calculate.GetAverageTotal(fDance),
                danceability = new Danceability
                {
                    min = Calculate.GetMinValue(fDance),
                    max = Calculate.GetMaxValue(fDance)
                },
                energyAverage = Calculate.GetAverageTotal(fEnergy),
                energy = new Energy
                {
                    min = Calculate.GetMinValue(fEnergy),
                    max = Calculate.GetMaxValue(fEnergy)
                },
                valenceAverage = Calculate.GetAverageTotal(fValence),
                valence = new Valence
                {
                    min = Calculate.GetMinValue(fValence),
                    max = Calculate.GetMaxValue(fValence)
                }

            };

            return average;
        }

        [HttpGet("/api/users/{Id}/stats")]
        public async Task<ActionResult<Paging<SimplePlaylist>>> GetUserPlayList(string Id)
        {
            var userPlayList = await _api.GetUserPlaylistsAsync(Id, 20, 0);
            return userPlayList;
        }



        }

 }