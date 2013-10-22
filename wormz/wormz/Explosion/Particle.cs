using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace wormz.ui.Explosions
{
    /// <summary>
    /// Extends the Sprite class. A particles is a short-lived sprite that may be generated in large quantities.
    /// </summary>
    class Particle:Sprite
    {
        #region Members
        private Vector2 acceleration;
        private float maxSpeed;
        //initialDuration and remainingDuration are used to determing how far along the particle's lifespan it is at any given time.
        private int initialDuration;
        private int remainingDuration;
        //initialColor and finalColor determine the tint color that will be used in the SpriteBatch.Draw() call
        private Color initialColor;
        private Color finalColor;
        #endregion
        #region Properties
        public int ElapsedDuration
        {
        get
        {
        return initialDuration - remainingDuration;
        }
        }
        /// <summary>
        /// represents the current position along the particle's lifespan.
        /// </summary>
        public float DurationProgress
        {
            get
            {
                return (float)ElapsedDuration / 
                (float)initialDuration;
            }
        }
        /// <summary>
        /// returns false if the remainingDuration has reached zero.
        /// </summary>
        public bool IsActive
        {
            get
            {
                return (remainingDuration > 0);
            }
        }
        #endregion
        #region Construction
        /// <summary>
        /// Creates a Particle
        /// </summary>
        /// <param name="location">The Location of the particle</param>
        /// <param name="texture">The texture used to draw the particle</param>
        /// <param name="initialFrame">The frame at which the particle begins</param>
        /// <param name="velocity">The speed at which the particle moves</param>
        /// <param name="acceleration">The acceleration of the particle</param>
        /// <param name="maxSpeed">The maximum velocity of the Particle</param>
        /// <param name="duration">How long the Particle lives</param>
        /// <param name="initialColor">The initial color of the Particle</param>
        /// <param name="finalColor">The final color of the Particle</param>
        public Particle(
                Vector2 location,
                Texture2D texture,
                Rectangle initialFrame,
                Vector2 velocity,
                Vector2 acceleration,
                float maxSpeed,
                int duration,
                Color initialColor,
                Color finalColor)
            : base(location, texture, initialFrame, velocity)
        {
            initialDuration = duration;
            remainingDuration = duration;
            this.acceleration = acceleration;
            this.initialColor = initialColor;
            this.maxSpeed = maxSpeed;
            this.finalColor = finalColor;
        }
        #endregion
        #region Methods
        /// <summary>
        /// Updates the particle during the game
        /// </summary>
        /// <param name="gameTime">The instance of the game</param>
        public override void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                velocity += acceleration;
                if (velocity.Length() > maxSpeed)
                {
                    velocity.Normalize();
                    velocity *= maxSpeed;
                }
                TintColor = Color.Lerp(
                initialColor,
                finalColor,
                DurationProgress);
                remainingDuration--;
                base.Update(gameTime);
            }
        }
        /// <summary>
        /// Draws the particle in the game
        /// </summary>
        /// <param name="spriteBatch">the sprite batch used to draw the particle/sprite</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive)
            {
                base.Draw(spriteBatch);
            }
        }
        #endregion
    }
}
