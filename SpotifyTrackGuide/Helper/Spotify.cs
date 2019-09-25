using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SpotifyAPI.Web;
using SpotifyTrackGuide.Auth;
using SpotifyTrackGuide.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyTrackGuide.Helper
{
    public class Spotify
    {
        private readonly IConfiguration _config;
        private SpotifyWebAPI _api;

        public Spotify(IConfiguration config)
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


        public TrackAverage GetPlayList(string id)
        {
            List<float> fDance = new List<float>();
            List<float> fEnergy = new List<float>();
            List<float> fValence = new List<float>();

            var playList = _api.GetPlaylistTracks(id);
            if (playList.Items != null)
            {
                foreach (var list in playList.Items)
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

            return null;
        }

        public List<Stats> GetPlatListStats(string Id)
        {
           List<Stats> statsList = new List<Stats>();
            var userPlayList = _api.GetUserPlaylists(Id, 20, 0);
            
            foreach(var list in userPlayList.Items)
            {
                var result = GetPlayList(list.Id);
                if (result != null)
                {
                    Stats stats = new Stats
                    {
                        playlistId = list.Id,
                        trackAverage = result
                    };
                    statsList.Add(stats);
                }
            }

            return statsList;
        }
    }
}
