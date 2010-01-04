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
   enum FlakeStatus
   {
      Created,
      Falling,
      Fallen,
      Melting,
      Melted
   }

   class Snowflake : DrawableGameComponent, IComparable<Snowflake>
   {
      private Texture2D texture;
      private SpriteBatch sbFlake;
      private float rotationFactor;
      private Vector2 origin = Vector2.Zero;
      private Vector2 scale = Vector2.One;
      private Vector2 position;
      private int fallSpeed;
      private int rotationSpeed;
      private Color tintColor = Color.White;
      private int meltCount;
      private byte opacity;
      private byte tint;
      private float startX;
      private int amplitudeX;
      private int jitterX;
      private FlakeStatus status = FlakeStatus.Created;
      private float windX;
      private Terrain _Mountains;

      public Snowflake(Game g, Terrain m) : base(g)
      {
         _Mountains = m;
      }

      public override void Initialize()
      {
         sbFlake = new SpriteBatch(Game.GraphicsDevice);
         base.Initialize();
      }

      public void Create(Random rGen)
      {
         startX = rGen.Next(Constants.START_X_MIN, Constants.START_X_MAX);
         windX = rGen.Next(Constants.WIND_MIN, Constants.WIND_MAX) / 10;
         position.Y = Constants.START_Y;

         scale.X = scale.Y = (float) (rGen.NextDouble() / 20) + 0.02f;

         fallSpeed = rGen.Next(Constants.FALL_MIN, Constants.FALL_MAX);
         rotationSpeed = rGen.Next(1, 4);

         amplitudeX = rGen.Next(-90, 90);
         jitterX = rGen.Next(-10, 10);
     
         opacity = Constants.FLAKE_OPACITY;
         tint = (byte)rGen.Next(200, 255);
         tintColor = new Color(tint, tint, tint, opacity);

         meltCount = 0;

         status = FlakeStatus.Falling;
      }

      protected override void LoadContent()
      {
         texture = Game.Content.Load<Texture2D>("snowflake");
         origin = new Vector2(texture.Width / 2, texture.Height / 2);
         base.LoadContent();
      }

      public override void Update(GameTime gameTime)
      {
         switch (status)
         {
            case FlakeStatus.Falling:
               rotationFactor += -(float)(gameTime.ElapsedGameTime.TotalSeconds * rotationSpeed);

               startX += windX;
               position.X = startX + ((float)Math.Cos(gameTime.TotalRealTime.TotalSeconds + jitterX) * amplitudeX);
               position.Y += (float)(gameTime.ElapsedGameTime.TotalSeconds * fallSpeed);

               // need to check here if we're reached our mountains.
               if(position.Y >= _Mountains.MaxY)
               {
                  // Now we need to see which line segment we're under. 
                  if (position.X < 0)
                     status = FlakeStatus.Melted;
                  else if (position.Y >= _Mountains.MinY)
                     status = FlakeStatus.Fallen;
                  else
                  {
                     Vector3[] points = _Mountains.GetLineSegment(position.X);

                     float slope = (points[0].Y - points[1].Y) / (points[0].X - points[1].X);
                     float lineY = slope * (position.X - points[0].X) + points[0].Y;

                     if( position.Y >= lineY )
                        status = FlakeStatus.Fallen;
                  }

               }

               if (position.X >= Constants.BOUNDARY_X)
                  status = FlakeStatus.Melted;

               break;

            case FlakeStatus.Fallen:

               meltCount++;

               if (meltCount > Constants.MELTING_TIME)
                  status = FlakeStatus.Melting;

               break;

            case FlakeStatus.Melting:

               if (opacity - 1 > 0)
               {
                  opacity -= Constants.FLAKE_FADER;
                  tintColor = new Color(tint, tint, tint, opacity);
               }
               else
                  status = FlakeStatus.Melted;
               break;
         }
      }

      public override void Draw(GameTime gameTime)
      {
         sbFlake.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.FrontToBack, SaveStateMode.None, Matrix.Identity);
         sbFlake.Draw(texture, position, null, tintColor, rotationFactor, origin, scale, SpriteEffects.None, 0);
         sbFlake.End();
      }

      public float X
      {
         get { return position.X; }
         set { position.X = value; }
      }

      public float Y
      {
         get { return position.Y; }
         set { position.Y = value; }
      }

      public FlakeStatus Status
      {
         get { return status; }
         set { status = value; }
      }

      public int CompareTo(Snowflake other)
      {
         return this.Status.CompareTo(other.Status);

         //if (this.Status == FlakeStatus.Created && other.Status == FlakeStatus.Created)
         //   return 0;
         //else if (this.Status == FlakeStatus.Created)
         //   return -1;
         //else if (other.Status == FlakeStatus.Created)
         //   return 1;
         //else
         //   return 0;

      }
   }
}
