using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace wormz.ui
{
    /// <summary>
    /// Represents the Hud of the game, includes power/angle.
    /// </summary>
    class Hud : DrawableGameComponent
    {
        #region Variables
        private SpriteFont font;
        private wormz wormz;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new hud
        /// </summary>
        /// <param name="game">The game this hud will be placed on.</param>
        public Hud(Game game)
            : base(game)
        {
            this.wormz = (wormz) game;
        }

        #endregion

        #region DrawableGameComponent Overrides
        /// <summary>
        /// loads this hud into the game's content
        /// </summary>
        protected override void LoadContent()
        {
            this.font = Game.Content.Load<SpriteFont>("Fonts/hud");
            base.LoadContent();
        }

        /// <summary>
        /// Draws the hud based on the current player's power and angle.
        /// </summary>
        /// <param name="gameTime">The current gametime.</param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            wormz.SpriteBatch.DrawString(font, wormz.CurrentPlayer.ToString(), 
                new Vector2(20, 20), wormz.CurrentPlayer.Color);
        }

        #endregion
    }
}
