using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace pleasework
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private PlayerCharacter player;
        private BeatManager beatManager;
        private AudioManager audioManager;
        private ScoreManager scoreManager;

        private Texture2D playerBaseTexture;
        private Texture2D playerMouthOpenTexture;
        private Texture2D beatTexture;
        private Vector2 centre;
        private SpriteFont scoreFont;


        private Song song;

        List<Beat> beats = new List<Beat>();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Get the current display resolution
            var displayMode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;

            // Set to native resolution
            graphics.PreferredBackBufferWidth = displayMode.Width;
            graphics.PreferredBackBufferHeight = displayMode.Height;

            // Enable fullscreen
            graphics.IsFullScreen = true;

            // Apply settings
            graphics.ApplyChanges();
        }


        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load textures
            playerBaseTexture = Content.Load<Texture2D>("PlayerBase");
            playerMouthOpenTexture = Content.Load<Texture2D>("PlayerMouthOpen");
            beatTexture = Content.Load<Texture2D>("Beat");

            //Load score font
            scoreFont = Content.Load<SpriteFont>("ScoreFont");

            // Load song
            song = Content.Load<Song>("Song");

            centre = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);

            // Initialize player
            player = new PlayerCharacter
            {
                BaseTexture = playerBaseTexture,
                MouthOpenTexture = playerMouthOpenTexture,
                Position = centre
            };

            // Initialize managers
            audioManager = new AudioManager();
            audioManager.Load(song);

            scoreManager = new ScoreManager();

            beatManager = new BeatManager(beats, player, audioManager, scoreManager);
            beatManager.LoadBeatmap("Content/beatmap.json");

            // Start the music
            audioManager.Play();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            player.Update(gameTime);
            beatManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            double currentTime = audioManager.GetPlaybackTime();

            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            player.Draw(spriteBatch);

            foreach (var beat in beats)
            {
                if (!beat.IsHit)
                {
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


            spriteBatch.DrawString(scoreFont, $"Score: {scoreManager.Score}", new Vector2(20, 20), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
