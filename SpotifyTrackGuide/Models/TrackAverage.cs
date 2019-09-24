using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyTrackGuide.Models
{
    public class TrackAverage
    {
        public Danceability danceability { get; set; }
        public double danceabilityAverage { get; set; }

        public Energy energy { get; set; }
        public double energyAverage { get; set; }

        public Valence valence { get; set; }
        public double valenceAverage { get; set; }
    }

    public class Energy
    {
        public double min { get; set; }
        public double max { get; set; }
    }

    public class Valence
    {
        public double min { get; set; }
        public double max { get; set; }
    }
}
