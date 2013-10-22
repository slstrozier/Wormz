using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace wormz.state
{
    
    public class PlayerMovementState : GameState
    {
        
        /// <summary>
        /// Determines what game logic to use when increasing
        /// the angle of the player's weapon slightly.
        /// </summary>
        /// <param name="toUse">The game to reference for game logic.</param>
        public void increaseAngle(wormz toUse)
        {
            toUse.increasePlayerAngle(0.01f);
        }

        /// <summary>
        /// Determines the game logic to use when decreasing
        /// the angle of the player's weapon slightly.
        /// </summary>
        /// <param name="toUse">The game to reference for game logic.</param>
        public void decreaseAngle(wormz toUse)
        {
            toUse.increasePlayerAngle(-0.01f);
        }

        /// <summary>
        /// Determines the game logic to use when increasing
        /// the power of the player's weapon.
        /// </summary>
        /// <param name="toUse">The game to reference for game logic.</param>
        public void increasePower(wormz toUse, int amount)
        {
            toUse.increasePlayerPower(amount);            
        }

        /// <summary>
        /// Determines the game logic to use when decreasing
        /// the power of the player's weapon.
        /// </summary>
        /// <param name="toUse">The game to reference for game logic.</param>
        public void decreasePower(wormz toUse, int amount)
        {
            toUse.increasePlayerPower(-amount);
        }

        /// <summary>
        /// Determines what occurs when a weapon is fired.
        /// </summary>
        /// <param name="toUse">The game to reference for game logic.</param>
        public void fireWeapon(wormz toUse)
        {
            toUse.pauseForFlight();
            toUse.launchRocket();
        }

        /// <summary>
        /// Determines what occurs when a projectile collides with something.
        /// </summary>
        /// <param name="toUse">The game to reference for game logic.</param>
        public void collide(wormz toUse)
        {
            
        }

        /// <summary>
        /// Determines what occures when the game changes players.
        /// </summary>
        /// <param name="toUse">The game to reference for game logic.</param>
        public void changePlayer(wormz toUse)
        {
        }
    }
}
