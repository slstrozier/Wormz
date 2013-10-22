using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wormz.state
{
    /// <summary>
    /// The current actions that occur when an event is triggered by the game.
    /// </summary>
    interface GameState
    {
        /// <summary>
        /// Determines what game logic to use when increasing
        /// the angle of the player's weapon slightly.
        /// </summary>
        /// <param name="toUse">The game to reference for game logic.</param>
        void increaseAngle(wormz toUse);

        /// <summary>
        /// Determines the game logic to use when decreasing
        /// the angle of the player's weapon slightly.
        /// </summary>
        /// <param name="toUse">The game to reference for game logic.</param>
        void decreaseAngle(wormz toUse);

        /// <summary>
        /// Determines the game logic to use when increasing
        /// the power of the player's weapon.
        /// </summary>
        /// <param name="toUse">The game to reference for game logic.</param>
        void increasePower(wormz toUse, int amount);

        /// <summary>
        /// Determines the game logic to use when decreasing
        /// the power of the player's weapon.
        /// </summary>
        /// <param name="toUse">The game to reference for game logic.</param>
        void decreasePower(wormz toUse, int amount);

        /// <summary>
        /// Determines what occurs when a weapon is fired.
        /// </summary>
        /// <param name="toUse">The game to reference for game logic.</param>
        void fireWeapon(wormz toUse);

        /// <summary>
        /// Determines what occurs when a projectile collides with something.
        /// </summary>
        /// <param name="toUse">The game to reference for game logic.</param>
        void collide(wormz toUse);

        /// <summary>
        /// Determines what occures when the game changes players.
        /// </summary>
        /// <param name="toUse">The game to reference for game logic.</param>
        void changePlayer(wormz toUse);
    }
}
