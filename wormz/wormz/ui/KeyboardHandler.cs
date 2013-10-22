using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace wormz.ui
{

    public delegate void KeyboardCommand();

    class KeyboardHandler : GameComponent
    {
        #region Fields
        private Dictionary<Keys, KeyboardCommand> keyMap;
        private wormz wormz; 
        #endregion

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="game">The game this keyboard handler is used for.</param>
        public KeyboardHandler(Game game) : base(game) { }
        #endregion

        #region XNA overrides
        /// <summary>
        /// Maps the keys of the game.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            wormz = ((wormz) Game);
            keyMap = new Dictionary<Keys, KeyboardCommand>();
            //increase the power.
            keyMap.Add(Keys.Up, new KeyboardCommand(doUp));
            //decrease the power.
            keyMap.Add(Keys.Down, new KeyboardCommand(doDown));
            //increase the power by 20.
            keyMap.Add(Keys.PageUp, new KeyboardCommand(doPageUp));
            //decrease the power by 20.
            keyMap.Add(Keys.PageDown, new KeyboardCommand(doPageDown));
            //rotate the cannon to the left.
            keyMap.Add(Keys.Left, new KeyboardCommand(doLeft));
            //rotate the cannon to the right.
            keyMap.Add(Keys.Right, new KeyboardCommand(doRight));
            //fire current weapon.
            keyMap.Add(Keys.Space, new KeyboardCommand(doSpace));
        }

        /// <summary>
        /// Does the actions of the current keys being pressed.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            KeyboardState state = Keyboard.GetState();

            foreach (Keys key in keyMap.Keys)
            {
                if (state.IsKeyDown(key))
                {
                    keyMap[key]();
                }
            }

        } 
        #endregion

        #region Command methods
        /// <summary>
        /// Increases the power of the current player.
        /// </summary>
        public void doUp()
        {
            wormz.increasePower(1);
        }

        /// <summary>
        /// Decreases the power of the current player.
        /// </summary>
        public void doDown()
        {
            wormz.decreasePower(1);
        }

        /// <summary>
        /// Increases the power of the current player quickly.
        /// </summary>
        public void doPageUp()
        {
            wormz.increasePower(20);
        }

        /// <summary>
        /// Decreases the power of the current player quickly.
        /// </summary>
        public void doPageDown()
        {
            wormz.decreasePower(20);
        }

        /// <summary>
        /// Decreases the Angle of the current player.
        /// </summary>
        public void doLeft()
        {
            wormz.decreaseAngle();
        }

        /// <summary>
        /// Increases the Angle of the current player.
        /// </summary>
        public void doRight()
        {
            wormz.increaseAngle();
        }

        /// <summary>
        /// Launches the rocket from the current player.
        /// </summary>
        public void doSpace()
        {
            wormz.fireWeapon();

        } 
        #endregion
    }
}
