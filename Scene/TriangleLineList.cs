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
   class TriangleLineList : DrawableGameComponent
   {
      BasicEffect effect;
      VertexPositionColor[] vertices;
      VertexDeclaration vertexDeclaration;

      public TriangleLineList(Game g)
         : base(g)
      {
         vertices = new VertexPositionColor[6];
      }

      protected override void LoadContent()
      {
         effect = new BasicEffect(GraphicsDevice, null);
         effect.VertexColorEnabled = true;
         effect.Projection = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0, 0, 1);

         vertexDeclaration = new VertexDeclaration(GraphicsDevice, VertexPositionColor.VertexElements);
         vertices[0].Position = new Vector3(50, 100, 0);
         vertices[0].Color = Color.Red;

         vertices[1].Position = new Vector3(50, 200, 0);
         vertices[1].Color = Color.Red;

         vertices[2].Position = new Vector3(50, 200, 0);
         vertices[2].Color = Color.Red;


         base.LoadContent();
      }

      public override void Draw(GameTime gameTime)
      {
         base.Draw(gameTime);

         GraphicsDevice.VertexDeclaration = vertexDeclaration;
         effect.Begin();
         effect.CurrentTechnique.Passes[0].Begin();

         GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, vertices, 0, 3);

         effect.CurrentTechnique.Passes[0].End();
         effect.End();
      }


   }
}
