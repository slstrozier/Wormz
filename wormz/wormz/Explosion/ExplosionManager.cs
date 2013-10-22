using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using wormz.ui.Explosions;

namespace wormz.ui
{
    /// <summary>
    /// ExplosionManager handles the explosions through the game. Explosions are created by Particles. When a 
    /// new explosion is generated, a random number of the explosion images from the png file along with an additional number 
    /// of individual small dot-shaped particles to represent the explosion.
    /// </summary>
    class ExplosionManager
    {
        #region Members
        private Texture2D texture;
        private List<Rectangle> pieceRectangles = new List<Rectangle>();
        private Rectangle pointRectangle;

        private int minPieceCount = 3;
        private int maxPieceCount = 6;
        private int minPointCount = 20;
        private int maxPointCount = 30;
        private int durationCount = 90;
        private float explosionMaxSpeed = 30f;
        //pieceSpeedScale, pointSpeedMin, pointSpeedMax control how rapidly the explosion pieces and 
        //point sprites move away from the center of the explosion
        private float pieceSpeedScale = 6f;
        private int pointSpeedMin = 15;
        private int pointSpeedMax = 30;
        private Color initialColor = new Color(1.0f, 0.3f, 0f) * 0.5f;
        private Color finalColor = new Color(0f, 0f, 0f, 0f);
        Random rand = new Random();
        private List<Particle> ExplosionParticles = new List<Particle>();
        #endregion
        #region Construction
        /// <summary>
        /// Creates an ExplosionManager object
        /// </summary>
        /// <param name="texture"> The texture used to create the explosion</param>
        /// <param name="initialFrame">The initial frame used to create the explosion</param>
        /// <param name="pieceCount">The number of pieces used to create the explosion</param>
        /// <param name="pointRectangle"></param>
        public ExplosionManager(Texture2D texture, Rectangle initialFrame, int pieceCount, Rectangle pointRectangle)
        {
            this.texture = texture;
            for (int x = 0; x < pieceCount; x++)
            {
                pieceRectangles.Add(new Rectangle(
                initialFrame.X + (initialFrame.Width * x),
                initialFrame.Y,
                initialFrame.Width,
                initialFrame.Height));
            }
            this.pointRectangle = pointRectangle;
        }
        #endregion
        #region Methods
        /// <summary>
        /// Draws the explosion in the game
        /// </summary>
        /// <param name="location">The location of the explosion to be drawn</param>
        /// <param name="momentum">The speed at which the explosion moves (as if there were wind)</param>
        public void AddExplosion(Vector2 location, Vector2 momentum)
        {
            Vector2 pieceLocation = location -
            new Vector2(pieceRectangles[0].Width / 2,
            pieceRectangles[0].Height / 2);
            int pieces = rand.Next(minPieceCount, maxPieceCount + 1);
            for (int x = 0; x < pieces; x++)
            {
                ExplosionParticles.Add(new Particle(
                pieceLocation,
                texture,
                pieceRectangles[rand.Next(0, pieceRectangles.Count)],
                randomDirection(pieceSpeedScale) + momentum,
                Vector2.Zero,
                explosionMaxSpeed,
                durationCount,
                initialColor,
                finalColor));
            }
            int points = rand.Next(minPointCount, maxPointCount + 1);
            for (int x = 0; x < points; x++)
            {
                ExplosionParticles.Add(new Particle(
                location,
                texture,
                pointRectangle,
                randomDirection((float)rand.Next(
                pointSpeedMin, pointSpeedMax)) + momentum,
                Vector2.Zero,
                explosionMaxSpeed,
                durationCount,
                initialColor,
                finalColor));
            }
        }
        /// <summary>
        /// Updates the explosion
        /// </summary>
        /// <param name="gameTime">The instance of the Games gameTime</param>
        public void Update(GameTime gameTime)
        {
            for (int x = ExplosionParticles.Count - 1; x >= 0; x--)
            {
                if (ExplosionParticles[x].IsActive)
                {
                    ExplosionParticles[x].Update(gameTime);
                }
                else
                {
                    ExplosionParticles.RemoveAt(x);
                }
            }


        }
        /// <summary>
        /// Draws the spriteBatch
        /// </summary>
        /// <param name="spriteBatch">the spriteBatch to draw</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Particle particle in ExplosionParticles)
            {
                particle.Draw(spriteBatch);
            }
        }
        #endregion
        #region Helper Methods
        /// <summary>
        /// Generates a random vector at which to move the particle away from the center point of the explosion
        /// </summary>
        /// <param name="scale">The scale at which the random vector is created</param>
        /// <returns>A random Vector2 by the scale used as the parameter</returns>
        public Vector2 randomDirection(float scale)
        {
            Vector2 direction;
            do
            {
                direction = new Vector2(
                rand.Next(0, 101) - 50,
                rand.Next(0, 101) - 50);
            } while (direction.Length() == 0);
            direction.Normalize();
            direction *= scale;
            return direction;
        }
        #endregion
    }
}
