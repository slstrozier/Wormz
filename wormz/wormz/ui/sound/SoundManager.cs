using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;

namespace wormz.sound
{
    /// <summary>
    /// This class manages all the sound throughout the game
    /// </summary>
    public static class SoundManager
    {
        #region Fields
        private static SoundEffect cannonFire;
        private static SoundEffect rocketExplosion;
        private static SoundEffect rocketTrail;
        #endregion

        #region Construction
        /// <summary>
        /// Uses the ContentManager to load SoundEffect Objects
        /// </summary>
        /// <param name="content">The content pipeline to use for the SoundManager</param>
        public static void Initialize(ContentManager content)
        {
            try
            {
                cannonFire = content.Load<SoundEffect>(@"Sounds\CannonFire");
                rocketExplosion = content.Load<SoundEffect>(@"Sounds\RocketExplosion");
                rocketTrail = content.Load<SoundEffect>(@"Sounds\RocketTrail");
            }
            catch
            {
                Debug.Write("SoundManager Initialization Failed");
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Plays the sound of the cannon blast
        /// </summary>
        public static void PlayCannonFire()
        {
            try
            {
                cannonFire.Play();
            }
            catch
            {
                Debug.Write("PlayCannonFire() Failed");
            }
        }
        /// <summary>
        /// Plays the sound of the rocket exploding
        /// </summary>
        public static void PlayRocketExplosion()
        {
            try
            {
                rocketExplosion.Play();
            }
            catch
            {
                Debug.Write("PlayRocketExplosion() Failed");
            }
        }
        /// <summary>
        /// Plays the sound of a rocked flying in the air
        /// </summary>
        public static void PlayRocketTrail()
        {
            try
            {
                rocketTrail.Play();
            }
            catch
            {
                Debug.Write("PlayRocketTrail() Failed");
            }
        }
        #endregion
    }
}
