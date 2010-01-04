using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using QBlizzard.Particles;
using QBlizzard.Scene;

namespace QBlizzard
{
   /// <summary>
   /// This is the main type for your game
   /// </summary>
   public class QBlizzard : Game
   {
      GraphicsDeviceManager graphics;
      Snowblower sb;
      Background bg;
      Terrain m; 
     
      public QBlizzard()
      {
         graphics = new GraphicsDeviceManager(this);
         graphics.PreferredBackBufferWidth = Constants.WIN_X;
         graphics.PreferredBackBufferHeight = Constants.WIN_Y;
         graphics.PreferMultiSampling = true;

         Content.RootDirectory = "Content";
         bg = new Background(this);
         m = new Terrain(this);
         sb = new Snowblower(this, m);
      }

      protected override void Initialize()
      {
         base.Components.Add(sb);
         base.Components.Add(bg);
         base.Components.Add(m);
         base.Initialize();
      }

      protected override void LoadContent()
      {
         base.LoadContent();
      }

      protected override void UnloadContent()
      {
         base.UnloadContent();
      }

      protected override void Update(GameTime gameTime)
      {
         // Allows the default game to exit on Xbox 360 and Windows
         if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            this.Exit();

         base.Update(gameTime);
      }

      protected override void Draw(GameTime gameTime)
      {
         graphics.GraphicsDevice.Clear(Color.White);

         base.Draw(gameTime);
      }
   }
}