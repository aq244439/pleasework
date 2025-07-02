using Microsoft.Xna.Framework.Media;
using System;

namespace pleasework
{
    public class AudioManager
    {
        private Song song;
        private double startTime;
        private bool playing = false;

        public void Load(Song song)
        {
            this.song = song;
        }

        public void Play()
        {
            MediaPlayer.Play(song);
            startTime = TimeSpan.FromTicks(DateTime.Now.Ticks).TotalSeconds;
            playing = true;
        }

        public double GetPlaybackTime()
        {
            if (!playing)
                return 0;
            return TimeSpan.FromTicks(DateTime.Now.Ticks).TotalSeconds - startTime;
        }
    }
}

