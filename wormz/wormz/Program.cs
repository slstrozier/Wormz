using System;

namespace wormz
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (wormz game = new wormz())
            {
                game.Run();
            }
        }
    }
#endif
}

