using System;
using System.Collections.Generic;
using System.Text;

namespace QBlizzard
{
   class Constants
   {
      public const int WIN_X = 800;
      public const int WIN_Y = 600;

      public const int START_X_MIN = -WIN_X;
      public const int START_X_MAX = WIN_X - 1;
      public const int START_Y = -30;
      public const int BOUNDARY_X = WIN_X + 13;

      public const int MELTING_TIME = 100;
      public const byte FLAKE_OPACITY = 200;
      public const int FLAKE_FADER = 2;
      public const int MAX_FLAKES = 200;

      public const int WIND_MIN = 22;
      public const int WIND_MAX = 25;

      public const int FALL_MIN = 90;
      public const int FALL_MAX = 100;

      public const int MAX_PEAK_X = 10;
      public const int MIN_PEAK_X = 100;
      public const int MIN_PEAK_Y = 40;
      public const int MAX_PEAK_Y = 120;

      public const float SPAWN_RATE = 1f / 50f;
      public const float COLLECT_RATE = 1f / 25f;
   }
}
