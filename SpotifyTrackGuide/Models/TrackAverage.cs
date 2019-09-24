using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyTrackGuide.Models
{
    public class TrackAverage
    {
        public Danceability danceability { get; set; }
        public double overall { get; set; }
        public double energy { get; set; }
        public double valence { get; set; }
    }

}
