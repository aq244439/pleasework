using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace pleasework
{
    public class PlayerCharacter
    {
        public float RotationAngle { get; private set; }
        public Texture2D BaseTexture { get; set; }
        public Texture2D MouthOpenTexture { get; set; }
        public Vector2 Position { get; set; }

        private const float RotationSpeed = 180f;
        private bool isMouthOpen = false;

        public void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardState kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.Left))
                RotationAngle -= RotationSpeed * delta;
            if (kstate.IsKeyDown(Keys.Right))
                RotationAngle += RotationSpeed * delta;

            RotationAngle %= 360f;
            if (RotationAngle < 0) RotationAngle += 360f;

            isMouthOpen = kstate.IsKeyDown(Keys.Space);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw base texture
            spriteBatch.Draw(
                BaseTexture,
                Position,
                null,
                Color.White,
                MathHelper.ToRadians(RotationAngle),
                new Vector2(BaseTexture.Width / 2, BaseTexture.Height / 2),
                1f,
                SpriteEffects.None,
                0f);

            // Draw mouth open texture overlay if space is pressed
            if (isMouthOpen)
            {
                spriteBatch.Draw(
                    MouthOpenTexture,
                    Position,
                    null,
                    Color.White,
                    MathHelper.ToRadians(RotationAngle),
                    new Vector2(MouthOpenTexture.Width / 2, MouthOpenTexture.Height / 2),
                    1f,
                    SpriteEffects.None,
                    0f);
            }
        }
    }
}
