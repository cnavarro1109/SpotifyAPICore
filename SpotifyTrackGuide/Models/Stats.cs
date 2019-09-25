using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyTrackGuide.Models
{
    public class Stats
    {
        public string playlistId { get; set; }
        public TrackAverage trackAverage { get; set; }
    }
}
