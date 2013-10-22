using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace wormz.ui
{
    public class Player : DrawableGameComponent
    {
        #region Static initialization
        /// <summary>
        /// This forces players to start at exact locations based on forground.png
        /// </summary>
        /// <summary>
        /// This chooses a different color for each of the players. Is not random.
        /// </summary> 
        private static Color[] playerColors = new Color[] {
            Color.Red,
            Color.Green,
            Color.Blue,
            Color.Purple,
            Color.Orange,
            Color.Indigo,
            Color.Yellow,
            Color.SaddleBrown,
            Color.Tomato,
            Color.Turquoise,
        };

        private static Texture2D carriageTexture = null;
        private static Texture2D cannonTexture = null;
        private static float playerScaling;
        private static Random numberRandomizer = new Random();
        #endregion

        #region Properties
        /// <summary>
        /// returns and sets the current angle of this player.
        /// </summary>
        public float Angle
        {
            get
            {
                return this.angle;
            }

            set
            {
                this.angle = value;

                if (this.angle > MathHelper.PiOver2)
                {
                    this.angle = -MathHelper.PiOver2;
                }
                else if (this.angle < -MathHelper.PiOver2)
                {
                    this.angle = MathHelper.PiOver2;
                }
            }
        }
        /// <summary>
        /// returns and sets the power of this player.
        /// </summary>
        public float Power
        {
            get
            {
                return this.power;
            }

            set
            {
                this.power = value;

                this.power = MathHelper.Clamp(this.Power, 0, 1000);
            }
        }

        /// <summary>
        /// gets and sets the color of this player.
        /// </summary>
        public Color Color { get { return this.color; } }
        /// <summary>
        /// gets and sets the current position of this player.
        /// </summary>
        public Vector2 Position { get { return position; } }
        #endregion

        #region Variables
        private Vector2 position;
        public Vector2 getPosition() { return position; }
        private bool isAlive;
        public bool IsPlayerAlive() { return isAlive; }
        private Color color;
        private float angle;
        private float power;
        private wormz scorch;
        private SpriteBatch spriteBatch;
        #endregion

        #region Constructions

        /// <summary>
        /// Creates a new player.
        /// </summary>
        /// <param name="game">The game this player will be participating in</param>
        /// <param name="batch">the Sprite Batch that will be drawing this class.</param>
        public Player(Game game, SpriteBatch batch)
            : base(game)
        {
            this.scorch = (wormz) game;
            spriteBatch = batch;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates the players and sets their variables. 
        /// </summary>
        /// <param name="game">The game this player will be participating in</param>
        /// <param name="batch">the Sprite Batch that will be drawing this class.</param>
        /// <param name="numberOfPlayers">the total number of players in this game.</param>
        /// <returns>The list of all the players for this game.</returns>
        public static List<Player> CreatePlayers(Game game, SpriteBatch batch,
            int numberOfPlayers, int screenWidth, Terrain battlefield)
        {
            List<Player> players = new List<Player>();
            bool[] playerPositions = new bool[numberOfPlayers];

            for (int i = 0; i < numberOfPlayers; i++)
            {
                Player player = new Player(game, batch);

                player.position = new Vector2();

                int randomPosition = 0;
                bool isPositionTaken = true;
                while (isPositionTaken)//randomizes player order
                {
                    randomPosition = numberRandomizer.Next(0, numberOfPlayers);
                    if (!playerPositions[randomPosition])
                    {
                        playerPositions[randomPosition] = true;
                        isPositionTaken = false;
                    }
                }

                player.position.X = screenWidth / (numberOfPlayers + 1) * (randomPosition + 1);
                player.position.Y = battlefield.Contour[(int) player.position.X];
                player.isAlive = true;
                player.color = playerColors[i];
                player.angle = MathHelper.ToRadians(45);
                player.power = 100;

                players.Add(player);
            }

            return players;
        }

        #endregion

        #region DrawableGameComponent Overrides

        /// <summary>
        /// Loads the parts of the current player.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            if (carriageTexture == null)
            {
                carriageTexture = Game.Content.Load<Texture2D>("Images/carriage");
                cannonTexture = Game.Content.Load<Texture2D>("Images/cannon");

                playerScaling = 40.0f / (float) carriageTexture.Width;
            }
        }

        /// <summary>
        /// Draws the player from the loaded content.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            int xPos = (int) position.X;
            int yPos = (int) position.Y;
            Vector2 cannonOrigin = new Vector2(11, 50);

            spriteBatch.Draw(cannonTexture, new Vector2(xPos + 20, yPos - 10),
                null, color, angle, cannonOrigin, playerScaling,
                SpriteEffects.None, 1);

            spriteBatch.Draw(carriageTexture, position, null,
                this.color, 0, new Vector2(0, carriageTexture.Height),
                playerScaling, SpriteEffects.None, 0);
        }

        #endregion

        #region Object Overrides
        public override string ToString()
        {
            //returns the data from the current power and angle for the spriteFont.
            return String.Format("angle: {0}°\npower: {1}", (int) MathHelper.ToDegrees(this.angle), this.power);
        }
        #endregion
    }
}
