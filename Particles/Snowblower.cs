using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using QBlizzard.Scene;

namespace QBlizzard.Particles
{
   class Snowblower : GameComponent
   {
      private List<Snowflake> _Flakes;
      private float respawnTimer = 10;
      private float collectTimer = 10;
      private Random rGen;
      private Terrain _Mountains;

      public Snowblower(Game g, Terrain m)
         : base(g)
      {
         _Mountains = m;
         _Flakes = new List<Snowflake>(Constants.MAX_FLAKES);
         rGen = new Random((int)DateTime.Now.Ticks);
      }

      public override void Initialize()
      {
         for (int i = 0; i < Constants.MAX_FLAKES; i++)
            _Flakes.Add(new Snowflake(Game, _Mountains));

         base.Initialize();
      }

      public override void Update(GameTime gameTime)
      {
         respawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
         collectTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
         if (respawnTimer >= Constants.SPAWN_RATE)
         {
            respawnTimer = 0f;
            bool foundFlake = false;

            for (int i = 0; i < _Flakes.Count - 1; i++)
            {
               if (!foundFlake && _Flakes[i].Status == FlakeStatus.Created)
               {
                  _Flakes[i].Create(rGen);
                  Game.Components.Add(_Flakes[i]);
                  foundFlake = true;
               }
               else if (_Flakes[i].Status == FlakeStatus.Melted)
               {
                  Game.Components.Remove(_Flakes[i]);
                  _Flakes[i].Status = FlakeStatus.Created;
               }
            }
         }

         if (collectTimer >= Constants.COLLECT_RATE)
         {
            collectTimer = 0f;
            GC.Collect();
         }

         base.Update(gameTime);
      }
   }
}