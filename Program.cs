using System;

namespace QBlizzard
{
   static class Program
   {
      /// <summary>
      /// The main entry point for the application.
      /// </summary>
      static void Main(string[] args)
      {
         using (QBlizzard game = new QBlizzard())
         {
            game.Run();
         }
      }
   }
}

