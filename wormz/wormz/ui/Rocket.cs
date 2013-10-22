using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace wormz.ui
{
    class Rocket : DrawableGameComponent
    {
        #region Constant Variables
        private static Vector2 gravity = new Vector2(0, 1);
        private static Vector2 offset = new Vector2(20.0f, -10.0f);
        private static float scaling = 0.1f;
        #endregion

        #region Variablies
        private Texture2D texture;
        private Texture2D smokeTexture;
        private List<Vector2> smokeList;
        private Vector2 position;
        private float angle;
        private Vector2 velocity;
        private Color color;
        private SpriteBatch spriteBatch;
        private Random randomizer;
        private Texture2D foregroundTexture;
        private int screenHeight;
        private int screenWidth;
        private bool rocketFlying;
        #endregion

        #region Porperties
        public Texture2D rocketTexture
        {
            get { return texture; }
            set { texture = value; }
        }

        public float Scaling
        {
            get { return scaling; }
            set { scaling = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public float Angle
        {
            get { return angle; }
            set { angle = value; }
        }

        public bool RocketFlying
        {
            get { return rocketFlying; }
            set { rocketFlying = value; }
        }

        public List<Vector2> SmokeList
        {
            get { return smokeList; }
            set { smokeList = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new rocket based on the player,angle and power.
        /// </summary>
        /// <param name="game">The current game</param>
        /// <param name="batch">the spritebatch to draw the rocket</param>
        /// <param name="player">the player for this rocket.</param>
        public Rocket(Game game, SpriteBatch batch, Player player, Texture2D foregroundTexture)
            : base(game)
        {
            
            spriteBatch = batch;

            position = player.Position + offset;
            angle = player.Angle;
    
            Vector2 up = new Vector2(0, -1);
            Matrix rotationMatrix = Matrix.CreateRotationZ(angle);
            velocity = Vector2.Transform(up, rotationMatrix);
            velocity *= player.Power / 50.0f;

            color = player.Color;
            rocketFlying = true;

            smokeList = new List<Vector2>();
            randomizer = new Random();

            this.foregroundTexture = foregroundTexture;
        }
        #endregion

        #region DrawableGameComponent Overrides
        /// <summary>
        /// Acquires the texture for the rocket and adds it to the content pipline.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            texture = Game.Content.Load<Texture2D>("Images/rocket");
            smokeTexture = Game.Content.Load<Texture2D>("Images/smoke");
            Collision.setUpCollision(this, foregroundTexture);
        }

        /// <summary>
        /// Updates the velocity and position of the rocket.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (rocketFlying)
            {
                if (position.Y > (((wormz)Game).ScreenHeight + 100)) //checks if the rocket is below the screen
                {
                    base.Dispose();//if so, remove the rocket from the game, it matters no more.
                    
                }
                else
                {
                   
                    velocity += gravity / 10.0f;
                    position += velocity;
                    angle = (float)Math.Atan2(velocity.X, -velocity.Y);
                    for (int i = 0; i < 5; i++)
                    {
                        Vector2 smokePos = position;
                        smokePos.X += randomizer.Next(10) - 5;
                        smokePos.Y += randomizer.Next(10) - 5;
                        smokeList.Add(smokePos);
                    }
                    
                    if (Collision.CheckCollisions(gameTime))
                    {
                        this.RocketFlying = false;
                        this.SmokeList = new List<Vector2>();
                        
                        ((wormz)Game).collide();
                    }
                    
                    
                }
            }
        }

        /// <summary>
        /// Draws the rocket based on its angle and position.
        /// </summary>
        /// <param name="gameTime">the current game time.</param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            spriteBatch.Draw(texture, position, null, color, angle, new Vector2(42, 240), scaling, SpriteEffects.None, 1);
            DrawSmoke();
            
        }

        private void DrawSmoke()
        {
            foreach (Vector2 smokePos in smokeList)
                spriteBatch.Draw(smokeTexture, smokePos, null, Color.White, 0, new Vector2(40, 35), 0.2f, SpriteEffects.None, 1);
          
        }
        #endregion
    }
}
