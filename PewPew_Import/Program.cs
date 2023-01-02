using System;

namespace PewPew
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Root game = new Root())
            {
                //game.IsMouseVisible = true;
                game.Run();
            }
        }
    }
}

