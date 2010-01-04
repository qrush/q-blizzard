using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace QBlizzard.Scene
{
   class Background : DrawableGameComponent
   {
      private SpriteBatch background;
      private Texture2D texture;

      public Background(Game g) : base(g)
      {
      }

      public override void Initialize()
      {
         background = new SpriteBatch(Game.GraphicsDevice);
         base.Initialize();
      }

      protected override void LoadContent()
      {
         texture = Game.Content.Load<Texture2D>("background");
         base.LoadContent();
      }

      public override void Draw(GameTime gameTime)
      {
         background.Begin();
         background.Draw(texture, Vector2.Zero, Color.White);
         background.End();
      }
   }
}
