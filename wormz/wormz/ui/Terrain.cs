using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace wormz.ui
{
    public class Terrain : DrawableGameComponent
    {
        #region Fields
        private SpriteBatch spriteBatch;
        private Texture2D backgroundTexture;
        private Texture2D foregroundTexture;
        private Texture2D topTerrainTexture;
        private int[] terrainContour;
        private double sceneryDeterminer1;
        private double sceneryDeterminer2;
        private double sceneryDeterminer3;
        private Random randomizer;
        private wormz game;
        #endregion


        #region Properties
        public int[] Contour { get { return terrainContour; } }

        public Texture2D Foreground { get { return foregroundTexture; } }
        #endregion

        #region Constructors

        public Terrain(Game game, SpriteBatch batch)
            : base(game)
        {
            spriteBatch = batch;
            randomizer = new Random();
            this.game = (wormz) game;

            sceneryDeterminer1 = randomizer.NextDouble() + 1;
            sceneryDeterminer2 = randomizer.NextDouble() + 2;
            sceneryDeterminer3 = randomizer.NextDouble() + 3;

            GenerateScenery(); //necessary to generate the terrain to place players on
        }

        #endregion

        #region DrawableGameComponent Overrides

        /// <summary>
        /// Acquires the texture for the terrain and adds it to the content pipline.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            //loading the background and the map the players will sit on.
            backgroundTexture = Game.Content.Load<Texture2D>("Images/background");
            foregroundTexture = Game.Content.Load<Texture2D>("Images/terrain-merge");
            topTerrainTexture = Game.Content.Load<Texture2D>("Images/midGround");
        }

        /// <summary>
        /// Updates the terrain's aspect (does nothing right now).
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the terrain.
        /// </summary>
        /// <param name="gameTime">the current game time.</param>
        public override void Draw(GameTime gameTime)
        {
            CreateForeground();
            base.Draw(gameTime);
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Draws the background and foreground onto the screen.
        /// </summary>
        private void GenerateScenery()
        {
            terrainContour = new int[game.ScreenWidth];

            float offset = game.ScreenHeight / 2;
            float peakheight = 150;
            float flatness = 50;

            for (int x = 0; x < game.ScreenWidth; x++)
            {
                double height = peakheight / sceneryDeterminer1 * Math.Sin((float)x / flatness * sceneryDeterminer1 + sceneryDeterminer1);
                height += peakheight / sceneryDeterminer2 * Math.Sin((float)x / flatness * sceneryDeterminer2 + sceneryDeterminer2);
                height += peakheight / sceneryDeterminer3 * Math.Sin((float)x / flatness * sceneryDeterminer3 + sceneryDeterminer3);
                height += offset;
                terrainContour[x] = (int)height;
            }
        }

        /// <summary>
        /// This method draws the foreground from the data the Terrain contains.
        /// </summary>
        private void CreateForeground()
        {
            /**/Color[] foregroundColors = new Color[game.ScreenWidth * game.ScreenHeight];
            Color[] currentForeground = new Color[foregroundTexture.Width * foregroundTexture.Height];
            Color[] topOfTerrain = new Color[topTerrainTexture.Width * topTerrainTexture.Height];

            foregroundTexture.GetData(currentForeground);
            topTerrainTexture.GetData(topOfTerrain);

            for (int x = 0; x < game.ScreenWidth; x++)
            {
                int b = 0;
                for (int y = 0; y < game.ScreenHeight; y++)
                {
                    if (y > (terrainContour[x]+20))
                    {
                        foregroundColors[x + y * game.ScreenWidth] = currentForeground[x + y * game.ScreenWidth];
                    }
                    else if ( y > terrainContour[x] ){
                        foregroundColors[x + y * game.ScreenWidth] = topOfTerrain[x + y * game.ScreenWidth];
                    }
                    else
                    {
                        int use = x + y * game.ScreenWidth;
                        foregroundColors[use] = Color.Transparent;
                    }

                } 

            }

            foregroundTexture.SetData(foregroundColors);

            Rectangle screenRectangle = new Rectangle(0, 0, game.ScreenWidth, game.ScreenHeight);
            spriteBatch.Draw(backgroundTexture, screenRectangle, Color.White);
            spriteBatch.Draw(foregroundTexture, screenRectangle, Color.White);
        }

        /// <summary>
        /// This method flattens the terrain below the players so that they aren't standing in thin air or in the middle of a mountain.
        /// NOTE: This method needs to change if the player sprites are changed!
        /// </summary>
        /// <param name="players">Current list of players</param>
        public void FixTerrainBelowPlayers(ref List<Player> players)
        {
            foreach (Player currentPlayer in players)
            {
                if (currentPlayer.IsPlayerAlive())
                {
                    for (int x = 0; x < 40; x++)
                    {
                        terrainContour[(int) currentPlayer.getPosition().X + x] = 
                            terrainContour[(int) currentPlayer.getPosition().X];
                    }
                }
            }
        }

        #endregion
    }
}
