using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QBlizzard.Scene
{
   class Terrain : DrawableGameComponent
   {
      BasicEffect effectMountain, effectPeak;
      List<VertexPositionColor> listVertices = new List<VertexPositionColor>();
      List<VertexPositionColor> topVertices = new List<VertexPositionColor>();
      List<short> listIndices = new List<short>();
      List<short> topIndices = new List<short>();
      VertexDeclaration vertexDeclaration;
      IndexBuffer buffTopIndex;
      VertexBuffer buffTopVertex;
      IndexBuffer buffTerrainIndex;
      VertexBuffer buffTerrainVertex;

      private int minY = Int32.MinValue;
      private int maxY = Int32.MaxValue;

      public Terrain(Game g)
         : base(g)
      {
      }

      private void LoadEffects()
      {
         effectMountain = new BasicEffect(GraphicsDevice, null);
         effectMountain.VertexColorEnabled = true;
         effectMountain.Projection = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0, 0, 1);

         effectPeak = new BasicEffect(GraphicsDevice, null);
         effectPeak.Projection = effectMountain.Projection;
         effectPeak.VertexColorEnabled = false;
         effectPeak.AmbientLightColor = new Vector3(0, 0, 0);
         effectPeak.LightingEnabled = true;
         effectPeak.DiffuseColor = effectPeak.AmbientLightColor;
         effectPeak.SpecularColor = effectPeak.AmbientLightColor;

         vertexDeclaration = new VertexDeclaration(GraphicsDevice, VertexPositionColor.VertexElements);

         buffTopVertex = new VertexBuffer(GraphicsDevice, VertexPositionColor.SizeInBytes * topVertices.Count, BufferUsage.WriteOnly);
         buffTopVertex.SetData<VertexPositionColor>(topVertices.ToArray());

         buffTopIndex = new IndexBuffer(GraphicsDevice, typeof(short), topIndices.Count, BufferUsage.WriteOnly);
         buffTopIndex.SetData<short>(topIndices.ToArray());

         buffTerrainVertex = new VertexBuffer(GraphicsDevice, VertexPositionColor.SizeInBytes * listVertices.Count, BufferUsage.WriteOnly);
         buffTerrainVertex.SetData<VertexPositionColor>(listVertices.ToArray());

         buffTerrainIndex = new IndexBuffer(GraphicsDevice, typeof(short), listIndices.Count, BufferUsage.WriteOnly);
         buffTerrainIndex.SetData<short>(listIndices.ToArray());
      }

      private void LoadVertices()
      {
         List<Vector3> knots = new List<Vector3>();
         Random r = new Random(Environment.TickCount);

         // Setting up knots for the b-spline
         int y, xMax = 0;
         for (int x = 0; x <= 2500; x += xMax)
         {
            xMax = r.Next(Constants.MAX_PEAK_X) + Constants.MIN_PEAK_X;
            y = Constants.WIN_Y - (r.Next(Constants.MAX_PEAK_Y) + Constants.MIN_PEAK_Y);

            knots.Add(new Vector3(x, y, 0));

            if (y > minY)
               minY = y;

            if (y < maxY)
               maxY = y;
         }

         // Now actually creating the bspline.
         float backX = 0;
         bool atEdge = false;
         for (int i = 0; i < knots.Count - 3 && !atEdge; i++)
         {
            int subdivs = (int)(Math.Sqrt(Math.Pow((knots[i + 1].X - knots[i].X), 2) + Math.Pow((knots[i + 1].Y - knots[i].Y), 2)));
            List<Vector3> splineVerts = Common.generateSpline(knots[i], knots[i + 1], knots[i + 2], knots[i + 3], subdivs);
            int overVec = splineVerts.FindIndex(delegate(Vector3 v)
            {
               return v.X - backX > Constants.WIN_X;
            });

            if (overVec >= 0)
            {
               atEdge = true;
               overVec++;
               splineVerts.RemoveRange(overVec, splineVerts.Count - overVec);
            }

            for (int j = 0; j < splineVerts.Count - 1; j++)
            {
               Vector3 splinePoint = splineVerts[j];

               // We need to back up all vertices by the FIRST x. 
               if (topVertices.Count == 0)
                  backX = splinePoint.X;

               splinePoint.X -= backX;
               topVertices.Add(new VertexPositionColor(splinePoint, Color.Black));

               // Now find the lower vertex.
               listVertices.Add(new VertexPositionColor(splinePoint, Color.White));
               splinePoint.Y = Constants.WIN_Y;
               listVertices.Add(new VertexPositionColor(splinePoint, Color.Gray));
            }
         }
      }

      private void LoadIndices()
      {
         for (int i = 0; i < topVertices.Count - 1; i++)
         {
            topIndices.Add((short)(i));
            topIndices.Add((short)(i + 1));
         }

         for (int i = 0; i < listVertices.Count - 1; i += 2)
         {
            listIndices.Add((short)(i));
            listIndices.Add((short)(3 + i));
            listIndices.Add((short)(1 + i));
            listIndices.Add((short)(i));
            listIndices.Add((short)(2 + i));
            listIndices.Add((short)(3 + i));
         }
      }

      protected override void LoadContent()
      {
         base.LoadContent();
         LoadVertices();
         LoadIndices();
         LoadEffects();
      }

      public override void Draw(GameTime gameTime)
      {
         base.Draw(gameTime);

         // Draw top
         GraphicsDevice.VertexDeclaration = vertexDeclaration;
         GraphicsDevice.Vertices[0].SetSource(buffTopVertex, 0, VertexPositionColor.SizeInBytes);
         GraphicsDevice.Indices = buffTopIndex;

         effectPeak.Begin();
         effectPeak.CurrentTechnique.Passes[0].Begin();
         GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.LineStrip, 0, 0, topVertices.Count, 0, topIndices.Count - 1);
         effectPeak.CurrentTechnique.Passes[0].End();
         effectPeak.End();

         // Draw terrain
         GraphicsDevice.Vertices[0].SetSource(buffTerrainVertex, 0, VertexPositionColor.SizeInBytes);
         GraphicsDevice.Indices = buffTerrainIndex;

         effectMountain.Begin();
         effectMountain.CurrentTechnique.Passes[0].Begin();
         GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, listVertices.Count, 0, listIndices.Count / 3 );
         effectMountain.CurrentTechnique.Passes[0].End();
         effectMountain.End();


      }

      public Vector3[] GetLineSegment(float x)
      {
         Vector3[] segment = new Vector3[2];

         for (int i = 0; i < topVertices.Count - 1; i++)
         {
            if (topVertices[i + 1].Position.X >= x)
            {
               segment[0] = topVertices[i].Position;
               segment[1] = topVertices[i + 1].Position;
               break;
            }
         }

         return segment;
      }

      public int MaxY
      {
         get { return maxY; }
      }

      public int MinY
      {
         get { return minY; }
      }
   }
}
