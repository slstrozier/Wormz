using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wormz.sound;

namespace wormz.state
{
    public class ProjectileFlightState : GameState
    {
        /// <summary>
        /// Determines what game logic to use when increasing
        /// the angle of the player's weapon slightly.
        /// </summary>
        /// <param name="toUse">The game to reference for game logic.</param>
        public void increaseAngle(wormz toUse)
        {
        }

        /// <summary>
        /// Determines the game logic to use when decreasing
        /// the angle of the player's weapon slightly.
        /// </summary>
        /// <param name="toUse">The game to reference for game logic.</param>
        public void decreaseAngle(wormz toUse)
        {
        }

        /// <summary>
        /// Determines the game logic to use when increasing
        /// the power of the player's weapon.
        /// </summary>
        /// <param name="toUse">The game to reference for game logic.</param>
        public void increasePower(wormz toUse, int amount)
        {
        }

        /// <summary>
        /// Determines the game logic to use when decreasing
        /// the power of the player's weapon.
        /// </summary>
        /// <param name="toUse">The game to reference for game logic.</param>
        public void decreasePower(wormz toUse, int amount)
        {
        }

        /// <summary>
        /// Determines what occurs when a weapon is fired.
        /// </summary>
        /// <param name="toUse">The game to reference for game logic.</param>
        public void fireWeapon(wormz toUse)
        {
        }

        /// <summary>
        /// Determines what occurs when a projectile collides with something.
        /// </summary>
        /// <param name="toUse">The game to reference for game logic.</param>
        public void collide(wormz toUse)
        {
            toUse.startExplosion();
            toUse.disposeOfRocket();
            try
            {
                SoundManager.PlayRocketExplosion();
                //TODO: Move this to where ever the rocket's animation will be handled.
            }
            catch (Exception e)
            {
            }
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
