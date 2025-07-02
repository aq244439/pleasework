using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace pleasework
{
    public class BeatManager
    {
        public List<Beat> Beats { get; private set; } = new List<Beat>();

        private ScoreManager scoreManager;
        private AudioManager audioManager;
        private PlayerCharacter player;
        private List<Beat> beats;
        private const float AngleTolerance = 45f;

        public BeatManager(List<Beat> beats, PlayerCharacter player, AudioManager audioManager, ScoreManager scoreManager)
        {
            this.beats = beats;
            this.player = player;
            this.audioManager = audioManager;
            this.scoreManager = scoreManager;
        }


        public void LoadBeatmap(string filePath)
        {
            string json = File.ReadAllText(filePath);
            var beatData = JsonSerializer.Deserialize<List<BeatDataModel>>(json);

            foreach (var data in beatData)
            {
                if (Enum.TryParse(data.corner, out Beat.SpawnCorner corner))
                {
                    Beats.Add(new Beat(corner, data.arrivalTime));
                }
            }
            Console.WriteLine($"Loaded {Beats.Count} beats");

        }

        public void Update(GameTime gameTime)
        {
            double currentTime = audioManager.GetPlaybackTime();
            KeyboardState kstate = Keyboard.GetState();

            foreach (var beat in Beats)
            {
                if (beat.IsHit || beat.IsMissed)
                    continue;

                // Mark missed if too late
                if (currentTime > beat.ArrivalTime + 0.2)
                {
                    beat.IsMissed = true;
                    continue;
                }

                if (kstate.IsKeyDown(Keys.Space))
                {
                    Vector2 beatPos = beat.GetPosition(currentTime, player.Position);

                    float angleToBeat = MathHelper.ToDegrees(
                        (float)Math.Atan2(beatPos.Y - player.Position.Y, beatPos.X - player.Position.X)
                    );

                    angleToBeat = (angleToBeat + 360f) % 360f;
                    float playerAngle = (player.RotationAngle + 360f) % 360f;

                    float angleDiff = Math.Abs(MathHelper.WrapAngle(
                        MathHelper.ToRadians(angleToBeat - playerAngle)
                    ) * MathHelper.ToDegrees(1));

                    if (angleDiff <= AngleTolerance)
                    {
                        double timeDiff = Math.Abs(currentTime - beat.ArrivalTime);
                        if (timeDiff <= 0.1)
                        {
                            beat.IsHit = true;
                            scoreManager.RegisterHit();
                        }
                    }
                }
            }
        }


        public void Draw(SpriteBatch spriteBatch, Texture2D beatTexture, Vector2 centre)
        {
            double currentTime = audioManager.GetPlaybackTime();

            foreach (var beat in Beats)
            {
                if (beat.IsHit || beat.IsMissed)
                    continue;

                Vector2 pos = beat.GetPosition(currentTime, centre);

                spriteBatch.Draw(
                    beatTexture,
                    pos,
                    null,
                    Color.White,
                    0f,
                    new Vector2(beatTexture.Width / 2, beatTexture.Height / 2),
                    1f,
                    SpriteEffects.None,
                    0f);
            }
        }
    }
}
