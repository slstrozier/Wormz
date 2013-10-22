using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using wormz.ui;
using wormz.sound;
using wormz.state;

namespace wormz
{
    /// <summary>
    /// A strategy game in which worms fight for survival.
    /// 
    /// http://www.riemers.net/eng/Tutorials/XNA/Csharp/Series2D/Starting_a_project.php
    /// </summary>
    public class wormz : Microsoft.Xna.Framework.Game
    {
        #region Game Variables
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private GraphicsDevice device;
        private GameState state;
        private ExplosionManager explosionManager;
        private Texture2D spriteSheet;
        #endregion

        #region GameState Constants
        public static PlayerMovementState PLAYERMOVEMENT = new PlayerMovementState();
        public static ProjectileFlightState PROJECTILEFLIGHT = new ProjectileFlightState();
        public static ExplosionState EXPLOSION = new ExplosionState();
        #endregion

        #region Component Variables
        // Screen dimensions
        private int screenWidth;        
        private int screenHeight;

        // Scenery and background
        private Terrain battlefield;

        //player management
        private List<Player> players;
        private int currentPlayer = 0;
        private Rocket rocket;
        
        //other
        private Random randomizer = new Random();
        #endregion

        #region Properties
        /// <summary>
        /// Gets the current player in control.
        /// </summary>
        public Player CurrentPlayer
        {
            get { return this.players[currentPlayer]; }
            // set;
        }

        /// <summary>
        /// Gets this games sprite batch.
        /// </summary>
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }

        public int ScreenWidth { get { return screenWidth; } }
        public int ScreenHeight { get { return screenHeight; } }
        #endregion

        #region Constructors
        /// <summary>
        /// This sets the graphics device and the content pipeline.
        /// </summary>
        public wormz()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            state = PLAYERMOVEMENT;
        }
        #endregion

        #region DrawableGameComponent Methods
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //setting the window of the game.
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "Wormz";

            screenWidth = graphics.GraphicsDevice.PresentationParameters.BackBufferWidth;
            screenHeight = graphics.GraphicsDevice.PresentationParameters.BackBufferHeight;

            this.battlefield = new Terrain(this, spriteBatch);//creates the terrain to be used
            Components.Add(battlefield);

            this.players = Player.CreatePlayers(this, spriteBatch, 7, screenWidth, battlefield);
            foreach (Player player in players)
            {
                Components.Add(player);
            }

            battlefield.FixTerrainBelowPlayers(ref players);//fixes the terrain to place players properly

            Components.Add(new Hud(this));
            Components.Add(new KeyboardHandler(this));

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteSheet = Content.Load<Texture2D>(@"Textures\SpriteSheet");
            explosionManager = new ExplosionManager(spriteSheet, new Rectangle(0, 0, 50, 50), 1, new Rectangle(0, 450, 2, 2));
            SoundManager.Initialize(Content);
            device = graphics.GraphicsDevice;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit from a xbox controller.
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.Exit();
            }
            // Allows the game to exit from a keyboard.
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            //TODO: Move this to wherever the explosion animation is handled.
            this.changePlayer();
            explosionManager.Update(gameTime);
            base.Update(gameTime);


        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            //DrawScenery();

            // draw any components attached to this game
            base.Draw(gameTime);
            explosionManager.Draw(spriteBatch);
            spriteBatch.End();
        }
        #endregion

        #region Private Helper methods

        /// <summary>
        /// Creates the rocket object and launches it based on the currentplayer's power and angle.
        /// </summary>
        internal void launchRocket()
        {
            if (rocket == null)
            {
                SoundManager.PlayCannonFire();
                rocket = new Rocket(this, spriteBatch, CurrentPlayer, battlefield.Foreground);
                Components.Add(rocket);
                SoundManager.PlayRocketTrail();
                
            }

        }

        /// <summary>
        /// Resets the rocket once it collides with something.
        /// </summary>
        internal void disposeOfRocket()
        {
            rocket.Dispose();
            rocket = null;
        }
        /// <summary>
        /// Creates the explosion of the rocket colliding with the terrian
        /// </summary>
        internal void CreateExplosion()
        {
            explosionManager.AddExplosion(Collision.ReturnCollisionPoint(), new Vector2(0,0));
        }
        /// <summary>
        /// Changes the current player, cycling from 0 to players.Count - 1
        /// </summary>
        internal void changeCurrentPlayer()
        {
            currentPlayer = (currentPlayer + 1) % players.Count;
        }

        /// <summary>
        /// Increases the current player's power by the given amount. Can be negative.
        /// </summary>
        /// <param name="toChangeBy">The amount to increase the player's power by.</param>
        internal void increasePlayerPower(int toChangeBy)
        {
            CurrentPlayer.Power += toChangeBy;
        }

        /// <summary>
        /// Increases the current player's angle by the given amount. Can be negative.
        /// </summary>
        /// <param name="toChangeBy">The amount to increase the player's angle by (in radians).</param>
        internal void increasePlayerAngle(float toChangeBy)
        {
            CurrentPlayer.Angle += toChangeBy;
        }

        #endregion

        #region GameState methods

        /// <summary>
        /// Sets the current state to the ProjectileFlight State.
        /// </summary>
        internal void pauseForFlight()
        {
            state = PROJECTILEFLIGHT;
        }

        /// <summary>
        /// Sets the current state to the Explosion State.
        /// </summary>
        internal void startExplosion()
        {
            state = EXPLOSION;
        }

        /// <summary>
        /// Sets the current state to the PlayerMovement State.
        /// </summary>
        internal void allowPlayerMovement()
        {
            state = PLAYERMOVEMENT;
        }

        /// <summary>
        /// Activates the current game state's reaction to attempting to increase the player's power.
        /// </summary>
        public void increasePower(int amount)
        {
            state.increasePower(this, amount);
        }

        /// <summary>
        /// Activates the current game state's reaction to attempting to decrease the player's power.
        /// </summary>
        public void decreasePower(int amount)
        {
            state.decreasePower(this, amount);
        }

        /// <summary>
        /// Activates the current game state's reaction to attempting to increase the player's angle.
        /// </summary>
        public void increaseAngle()
        {
            state.increaseAngle(this);
        }

        /// <summary>
        /// Activates the current game state's reaction to attempting to decrease the player's angle.
        /// </summary>
        public void decreaseAngle()
        {
            state.decreaseAngle(this);
        }

        /// <summary>
        /// Activates the current game state's reaction to attempting to fire the player's weapon.
        /// </summary>
        public void fireWeapon()
        {
            state.fireWeapon(this);
        }

        /// <summary>
        /// Activates the current game state's reaction to the weapon collidiing with something.
        /// </summary>
        public void collide()
        {
            state.collide(this);
            CreateExplosion();
        }

        /// <summary>
        /// Activates the current game state's reaction to changing the current player.
        /// </summary>
        public void changePlayer()
        {
            state.changePlayer(this);
        }

        #endregion
    }
}
