using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace QBlizzard
{
   class Common
   {
      // http://www.c-sharpcorner.com/uploadfile/apundit/drawingcurves11182005012515am/drawingcurves.aspx?login=true&user=DoctorNick
      public static List<Vector3> generateSpline(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, int subdivs)
      {
         List<Vector3> vertices = new List<Vector3>();
         float[] a = new float[4];
         float[] b = new float[4];

         a[0] = (-p1.X + 3f * p2.X - 3f * p3.X + p4.X) / 6.0f;
         a[1] = (3f * p1.X - 6f * p2.X + 3 * p3.X) / 6.0f;
         a[2] = (-3f * p1.X + 3f * p3.X) / 6.0f;
         a[3] = (p1.X + 4f * p2.X + p3.X) / 6.0f;

         b[0] = (-p1.Y + 3f * p2.Y - 3f * p3.Y + p4.Y) / 6.0f;
         b[1] = (3 * p1.Y - 6f * p2.Y + 3f * p3.Y) / 6.0f;
         b[2] = (-3 * p1.Y + 3f * p3.Y) / 6.0f;
         b[3] = (p1.Y + 4f * p2.Y + p3.Y) / 6.0f;

         vertices.Add(new Vector3(a[3], b[3], 0));

         for (int i = 1; i < subdivs; i++)
         {
            float t = ((float)i) / ((float)subdivs);

            float x = a[3] + t * (a[2] + t * (a[1] + t * a[0]));
            float y = b[3] + t * (b[2] + t * (b[1] + t * b[0]));
            vertices.Add(new Vector3(x, y, 0));
         }

         return vertices;
      }

   }
}
