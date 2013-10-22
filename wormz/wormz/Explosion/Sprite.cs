using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace wormz.ui.Explosions
{
    class Sprite
    {
        /// <summary>
        /// The Members of the sprite
        /// </summary>
        #region Members
        public Texture2D Texture;
        protected List<Rectangle> frames = new List<Rectangle>();
        private int frameWidth = 0;
        private int frameHeight = 0;
        private int currentFrame;
        private float frameTime = 0.1f;
        private float timeForCurrentFrame = 0.0f;
        private Color tintColor = Color.White;
        private float rotation = 0.0f;
        public int CollisionRadius = 0;
        public int BoundingXPadding = 0;
        public int BoundingYPadding = 0;
        protected Vector2 location = Vector2.Zero;
        protected Vector2 velocity = Vector2.Zero;
        #endregion
        #region Construction
        public Sprite(Vector2 location, Texture2D texture, Rectangle initialFrame, Vector2 velocity) 
        {
            this.location = location;
            Texture = texture;
            this.velocity = velocity;
            frames.Add(initialFrame);
            frameWidth = initialFrame.Width;
            frameHeight = initialFrame.Height;
        }
        #endregion
        #region Sprite Properties
        #region Basic Sprite Properties
        public Vector2 Location
        {
            get { return location; }
            set { location = value; }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        public Color TintColor
        {
            get { return tintColor; }
            set { tintColor = value; }
        }
       
        #endregion
        #region Animation Sprite Properties
        /// <summary>
        /// MathHelper.Clamp() method used to ensure that when this property is set, the value passed does not
        /// exceed the number of animation frames available
        /// </summary>

        /// <summary>
        /// Allows the speed at which the animation plays to be updated. MathHelper.Max() used to ensure a valid value.
        /// A frameTime value of greater than or less than 0 creates an animation that updates during every Update() cycle
        /// </summary>
        public float FrameTime
        {
        get { return frameTime; }
            set { frameTime = MathHelper.Max(0, value); }
        }
        /// <summary>
        /// Returns the Rectangle associated with the current frame from the frames list.
        /// </summary>
        public Rectangle Source
        {
            get { return frames[currentFrame]; }
        }
        /// <summary>
        /// builds a new Rectangle based on the sprite's current screen location and the width and height of a frame.
        /// </summary>

        /// <summary>
        /// Returns the location member, offset by half of the width and height of the sprite (the center position if the sprite)
        /// </summary>
        public Vector2 Center
        {
            get
            {
                return location +
                new Vector2(frameWidth / 2, frameHeight / 2);
            }
        }

        #endregion
        #endregion
        #region Methods

        /// <summary>
        /// Updates the sprite in the game
        /// </summary>
        /// <param name="gameTime">The instance of this games gameTime</param>
        public virtual void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timeForCurrentFrame += elapsed;
            if (timeForCurrentFrame >= FrameTime)
            {
                currentFrame = (currentFrame + 1) % (frames.Count);
                timeForCurrentFrame = 0.0f;
            }
            location += (velocity * elapsed);
        }
        /// <summary>
        /// Draws the sprite in the game
        /// </summary>
        /// <param name="spriteBatch">The sprite batch used to draw this sprite</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Center, Source, tintColor, rotation, new Vector2(frameWidth / 2, frameHeight / 2),
                1.0f, SpriteEffects.None, 0.0f);
        }
        #endregion
    }
}
