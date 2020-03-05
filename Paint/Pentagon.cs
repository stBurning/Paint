using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paint {
    class Pentagon {

        private Point startPoint;
        private Point currPoint;
        private Size sz;
        Point[] points;
        public Pentagon(Point startPoint,Point currPoint) {
            this.startPoint = startPoint;
            this.currPoint = currPoint;
            points = new Point[5];
            sz = new Size(Math.Abs(currPoint.X - startPoint.X), Math.Abs(currPoint.Y - startPoint.Y));
            if (currPoint.X < startPoint.X) {
                if (currPoint.Y < startPoint.Y) {
                  
                } else {
                    
                }
            } else {
                if (currPoint.Y > startPoint.Y) {
                    points[0] = new Point(startPoint.X, startPoint.Y + Math.Abs(currPoint.Y - startPoint.Y)/5*2);
                    points[1] = new Point(startPoint.X + Math.Abs(currPoint.X - startPoint.X) / 2, startPoint.Y);
                    points[2] = new Point(currPoint.X, startPoint.Y + Math.Abs(currPoint.Y - startPoint.Y) / 5 * 2);
                    points[3] = new Point(startPoint.X + (Math.Abs(currPoint.X - startPoint.X) / 4) * 3, currPoint.Y);
                    points[4] = new Point(startPoint.X + (Math.Abs(currPoint.X - startPoint.X) / 4), currPoint.Y);


                } else {
                    
                }
            }

        }

        public void DrawPentagon(Pen pen, Graphics g, Point currPoint) {
            g.DrawPolygon(pen, points);
        }
    }
}
