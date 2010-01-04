using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QBlizzard.Scene
{
   public class SquareIndexBuffer : DrawableGameComponent
   {
      // a basic effect, which contains the shaders that we will use to draw our
      // primitives.
      BasicEffect basicEffect;

      // an array of the vertices that have to be drawn
      VertexPositionColor[] vertices = new VertexPositionColor[4];

      // the vertex declaration that will be set on the device for drawing. this is 
      // created automatically using VertexPositionColor's vertex elements.
      VertexDeclaration vertexDeclaration;

      // Create a Index Buffer
      IndexBuffer indexbuffer;
      // Create a Vertex Buffer
      VertexBuffer vertexbuffer;

      public SquareIndexBuffer(Game game)
         : base(game) { }

      protected override void LoadGraphicsContent(bool loadAllContent)
      {
         base.LoadGraphicsContent(loadAllContent);
         // set up a new basic effect, and enable vertex colors.
         basicEffect = new BasicEffect(GraphicsDevice, null);
         basicEffect.VertexColorEnabled = true;

         // projection uses CreateOrthographicOffCenter to create 2d projection
         // matrix with 0,0 in the upper left.
         basicEffect.Projection = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width,
             GraphicsDevice.Viewport.Height, 0,
             0, 1);

         // create a vertex declaration, which tells the graphics card what kind of
         // data to expect during a draw call. We're drawing using
         // VertexPositionColors, so we'll use those vertex elements.
         vertexDeclaration = new VertexDeclaration(GraphicsDevice, VertexPositionColor.VertexElements);

         // create our square by setting up the vertices
         // because we are using an indexbuffer, we only need 4 vertices in stead of 6 (2 triangles)
         vertices[0].Position = new Vector3(250, 100, 0);
         vertices[0].Color = Color.Red;

         vertices[1].Position = new Vector3(350, 200, 0);
         vertices[1].Color = Color.Blue;

         vertices[2].Position = new Vector3(250, 200, 0);
         vertices[2].Color = Color.Green;

         vertices[3].Position = new Vector3(350, 100, 0);
         vertices[3].Color = Color.Yellow;

         vertexbuffer = new VertexBuffer(GraphicsDevice, VertexPositionColor.SizeInBytes * vertices.Length, BufferUsage.WriteOnly); 
         vertexbuffer.SetData<VertexPositionColor>(vertices);

         // set up our indices
         // because a square consist of 2 triangles, we need 6 points in our indexbuffer
         short[] indices = new short[6];

         indices[0] = 0;
         indices[1] = 1;
         indices[2] = 2;
         indices[3] = 0;
         indices[4] = 3;
         indices[5] = 1;

         indexbuffer = new IndexBuffer(GraphicsDevice, typeof(short), indices.Length, BufferUsage.WriteOnly);
         indexbuffer.SetData(indices);
      }

      public override void Draw(GameTime gameTime)
      {
         base.Draw(gameTime);
         // prepare the graphics device for drawing by setting the vertex declaration and the vertices
         GraphicsDevice.VertexDeclaration = vertexDeclaration;
         GraphicsDevice.Vertices[0].SetSource(vertexbuffer, 0, VertexPositionColor.SizeInBytes);
         // prepare the graphics device for drawing by setting the indices
         GraphicsDevice.Indices = indexbuffer;

         // tell our basic effect to begin.
         basicEffect.Begin();
         basicEffect.CurrentTechnique.Passes[0].Begin();

         GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 4, 0, 2);

         // tell basic effect that we're done.
         basicEffect.CurrentTechnique.Passes[0].End();
         basicEffect.End();
      }
   }
}
