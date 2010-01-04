using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QBlizzard.Scene
{
   class Square : DrawableGameComponent
   {
      BasicEffect effect;
      List<VertexPositionColor> listVertices = new List<VertexPositionColor>();
      List<short> listIndices = new List<short>();
      VertexDeclaration vertexDeclaration;
      IndexBuffer buffIndex;
      VertexBuffer buffVertex;
      int xMax = 100;
      int y = 100;

      public Square(Game g)
         : base(g)
      {
      }

      protected override void LoadContent()
      {
         base.LoadContent();

         effect = new BasicEffect(GraphicsDevice, null);
         effect.VertexColorEnabled = true;
         effect.Projection = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0, 0, 1);

         vertexDeclaration = new VertexDeclaration(GraphicsDevice, VertexPositionColor.VertexElements);

         Color c = Color.Red;

         for (int x = 0; x <= Constants.WIN_X; x += xMax)
         {
            listVertices.Add(new VertexPositionColor(new Vector3(x, Constants.WIN_Y - y, 0), Color.Red));
            listVertices.Add(new VertexPositionColor(new Vector3(x, Constants.WIN_Y, 0), Color.Blue));
         }

         for (int i = 0; i < listVertices.Count - 1; i+=2)
         {
            listIndices.Add((short)(i));
            listIndices.Add((short)(3 + i));
            listIndices.Add((short)(1 + i));
            listIndices.Add((short)(i));
            listIndices.Add((short)(2 + i));
            listIndices.Add((short)(3 + i));
         }

         buffVertex = new VertexBuffer(GraphicsDevice, VertexPositionColor.SizeInBytes * listVertices.Count, BufferUsage.WriteOnly);
         buffVertex.SetData<VertexPositionColor>(listVertices.ToArray());

         buffIndex = new IndexBuffer(GraphicsDevice, typeof(short), listIndices.Count, BufferUsage.WriteOnly);
         buffIndex.SetData<short>(listIndices.ToArray());
      }

      public override void Draw(GameTime gameTime)
      {
         base.Draw(gameTime);

         GraphicsDevice.VertexDeclaration = vertexDeclaration;
         GraphicsDevice.Vertices[0].SetSource(buffVertex, 0, VertexPositionColor.SizeInBytes);
         GraphicsDevice.Indices = buffIndex;

         effect.Begin();
         effect.CurrentTechnique.Passes[0].Begin();

            GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, listVertices.Count, 0, listIndices.Count / 3);


         effect.CurrentTechnique.Passes[0].End();
         effect.End();
      }
   }
}
