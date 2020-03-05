using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Paint {
    public partial class Form1 : Form {

        private static Color color = Color.Black;
        private static Color backColor = Color.White;
        private static Color fillColor = Color.White;
        private static float width = 1;
        private Pen pen = new Pen(color, width);
        private Pen eraser = new Pen(backColor, width*10);
        private Brush solidBrush = new SolidBrush(fillColor);
        private bool paint = false;
        private Graphics g, gimg;
        private BufferedGraphicsContext bgc = BufferedGraphicsManager.Current;
        private BufferedGraphics bg;
        private enum Figure { Pen, Line, Ellipse, Rectangle, Eraser, Penta, Brush }
        private Figure figure = Figure.Pen;
        private Point currPoint;
        private Point startPoint;
        private Image img = null;
        

        public Form1() {
            InitializeComponent();
            g = workPanel.CreateGraphics();
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            eraser.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            eraser.EndCap = System.Drawing.Drawing2D.LineCap.Round;
        }


        private void workPanel_Paint(object sender, PaintEventArgs e) {
            if (img == null) {
                img = new Bitmap(
                    workPanel.Width, workPanel.Height,
                    workPanel.CreateGraphics()
                    );
                gimg = Graphics.FromImage(img);
                gimg.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                bg = bgc.Allocate(workPanel.CreateGraphics(),
                    new Rectangle(0, 0, workPanel.Width, workPanel.Height)
                    );
                gimg.Clear(Color.White);
                e.Graphics.DrawImage(img, 0, 0);
            }
            e.Graphics.DrawImage(img, 0, 0);
        }

        private void workPanel_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                paint = true;
            }
            startPoint = new Point(e.X, e.Y);
        }

        private void workPanel_MouseUp(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                switch (figure) {
                    case Figure.Pen: { break; }
                    case Figure.Ellipse: {
                            var sz = new Size(currPoint.X - startPoint.X, currPoint.Y - startPoint.Y);
                            gimg.DrawEllipse(pen, new Rectangle(startPoint, sz));
                            if (checkBox1.Checked) gimg.FillEllipse(solidBrush, new Rectangle(startPoint, sz));
                            bg.Render();
                            break;
                        }
                    case Figure.Line: {
                            gimg.DrawLine(pen, startPoint, currPoint);                            
                            bg.Render();
                            break;
                        }
                    case Figure.Rectangle: {
                            var sz = new Size(Math.Abs(currPoint.X - startPoint.X), Math.Abs(currPoint.Y - startPoint.Y));
                            if (currPoint.X < startPoint.X) {
                                if (currPoint.Y < startPoint.Y) {
                                    gimg.DrawRectangle(pen, new Rectangle(currPoint, sz));
                                    if (checkBox1.Checked) gimg.FillRectangle(solidBrush, new Rectangle(currPoint, sz));
                                } else {
                                    var point = new Point(x: currPoint.X, y: startPoint.Y);
                                    gimg.DrawRectangle(pen, new Rectangle(point, sz));
                                    if (checkBox1.Checked) gimg.FillRectangle(solidBrush, new Rectangle(point, sz));
                                }
                            } else {
                                if (currPoint.Y > startPoint.Y) {
                                    gimg.DrawRectangle(pen, new Rectangle(startPoint, sz));
                                    if (checkBox1.Checked) gimg.FillRectangle(solidBrush, new Rectangle(startPoint, sz));
                                } else {
                                    var point = new Point(x: startPoint.X, y: currPoint.Y);
                                    gimg.DrawRectangle(pen, new Rectangle(point, sz));
                                    if (checkBox1.Checked) gimg.FillRectangle(solidBrush, new Rectangle(point, sz));
                                }
                            }
                            bg.Render();
                            break;
                        }
                }
                paint = false;
                workPanel.CreateGraphics().DrawImage(img, 0, 0);
            }
        }

        private void workPanel_MouseMove(object sender, MouseEventArgs e) {
            if (img == null) return;
            var g = workPanel.CreateGraphics();
            //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            var bgg = bg.Graphics;
            //bgg.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            currPoint = new Point(e.X, e.Y);
            if (paint) {
                switch (figure) {
                    case Figure.Pen: {
                            gimg.DrawLine(pen, startPoint, currPoint);
                            bgg.DrawImage(img, 0, 0);
                            bgg.DrawLine(pen, startPoint, currPoint);
                            startPoint = currPoint;
                            bg.Render();
                            break;
                        }
                    case Figure.Brush: {
                            gimg.DrawLine(pen, startPoint, currPoint);
                            bgg.DrawImage(img, 0, 0);
                            bgg.DrawLine(pen, startPoint, currPoint);
                            startPoint = currPoint;
                            bg.Render();
                            break;
                        }
                    case Figure.Line: {
                            bgg.DrawImage(img, 0, 0);
                            bgg.DrawLine(pen, startPoint, currPoint);
                            bg.Render();
                            break;
                        }
                    case Figure.Ellipse: {
                            
                            var sz = new Size(currPoint.X - startPoint.X, currPoint.Y - startPoint.Y);
                            bgg.DrawImage(img, 0, 0);
                            bgg.DrawEllipse(pen, new Rectangle(startPoint, sz));
                            if (checkBox1.Checked) bgg.FillEllipse(solidBrush, new Rectangle(startPoint, sz));
                            bg.Render();
                            break;
                        }
                    case Figure.Rectangle: {
                            var sz = new Size(Math.Abs(currPoint.X - startPoint.X), Math.Abs(currPoint.Y - startPoint.Y));
                            bgg.DrawImage(img, 0, 0);
                            var rectangle = new Rectangle(startPoint, sz);
                            if (currPoint.X < startPoint.X) {
                                if (currPoint.Y < startPoint.Y) {
                                    bgg.DrawRectangle(pen, new Rectangle(currPoint, sz));
                                    if (checkBox1.Checked) bgg.FillRectangle(solidBrush, new Rectangle(currPoint, sz));
                                } else {
                                    var point = new Point(x: currPoint.X, y: startPoint.Y);
                                    bgg.DrawRectangle(pen, new Rectangle(point, sz));
                                    if (checkBox1.Checked) bgg.FillRectangle(solidBrush, new Rectangle(point, sz));
                                }
                            } else {
                                if (currPoint.Y > startPoint.Y) {
                                    bgg.DrawRectangle(pen, new Rectangle(startPoint, sz));
                                    if (checkBox1.Checked) bgg.FillRectangle(solidBrush, new Rectangle(startPoint, sz));
                                } else {
                                    var point = new Point(x: startPoint.X, y: currPoint.Y);
                                    bgg.DrawRectangle(pen, new Rectangle(point, sz));
                                    if (checkBox1.Checked) bgg.FillRectangle(solidBrush, new Rectangle(point, sz));
                                }
                            }
                            bg.Render();
                            break;
                        }
                    case Figure.Penta: {
                            var sz = new Size(Math.Abs(currPoint.X - startPoint.X), Math.Abs(currPoint.Y - startPoint.Y));
                            Pentagon penta = new Pentagon(startPoint, currPoint);               
                            bgg.DrawImage(img, 0, 0);
                            penta.DrawPentagon(pen, bgg, currPoint);
                            bg.Render();
                            break;
                        }
                    case Figure.Eraser: {
                            gimg.DrawLine(eraser, startPoint, currPoint);
                            bgg.DrawImage(img, 0, 0);
                            bgg.DrawLine(pen, startPoint, currPoint);
                            startPoint = currPoint;
                            bg.Render();
                            break;
                        }
                }
            }
        }

        private void colorPanel_Click(object sender, EventArgs e) {
            var mouseEvent = (MouseEventArgs)e;
            if (mouseEvent.Button == MouseButtons.Left) {
                var p = (Panel)sender;
                pen.Color = p.BackColor;
            }
            if (mouseEvent.Button == MouseButtons.Right) {
                var p = (Panel)sender;
                fillColor = p.BackColor;
                solidBrush = new SolidBrush(fillColor);
            }
        }



        private void saveButton_Click(object sender, EventArgs e) {
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.Filter = "Фаил картинки в формате JPG|*.jpg  |Фаил картинки в формате PNG|*.png";
            var res = saveFileDialog1.ShowDialog();
            if (res == DialogResult.OK) {
                img.Save($"{saveFileDialog1.FileName}", ImageFormat.Jpeg);
            }
        }

        private void Form1_Resize(object sender, EventArgs e) {
            if (img != null && this.WindowState != FormWindowState.Minimized) {
                g = workPanel.CreateGraphics();
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                var old_img = img;
                img = new Bitmap(
                        workPanel.Width, workPanel.Height,
                        workPanel.CreateGraphics()
                        );
                gimg = Graphics.FromImage(img);
                gimg.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                bg = bgc.Allocate(g, new Rectangle(0, 0, workPanel.Width, workPanel.Height));
                gimg.Clear(Color.White);
                gimg.DrawImage(old_img, 0, 0);
                g.DrawImage(img, 0, 0);
                bg = bgc.Allocate(g, new Rectangle(0, 0, workPanel.Width, workPanel.Height));
                 bg.Graphics.Clear(Color.White);
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e) {
            var menuItem = (ToolStripMenuItem)sender;
            pen.Width = float.Parse(menuItem.Text);
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e) {
            var res = openFileDialog1.ShowDialog();
            if (res == DialogResult.OK) {
                g.Clear(Color.White);
                img = new Bitmap(Image.FromFile(openFileDialog1.FileName));
                g = workPanel.CreateGraphics();
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                gimg = Graphics.FromImage(img);
                gimg.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.DrawImage(img, 0, 0);
                bg = bgc.Allocate(g, new Rectangle(0, 0, workPanel.Width, workPanel.Height));

            }
        }

        private void button3_Click(object sender, EventArgs e){
            var btn = (ToolStripButton)sender;
            switch (btn.Tag){
                case "Pen": { figure = Figure.Pen; break; }
                case "Line": { figure = Figure.Line; break; }
                case "Rectangle": { figure = Figure.Rectangle; break; }
                case "Ellipse": { figure = Figure.Ellipse; break; }
                case "Eraser": { figure = Figure.Eraser; break; }
                case "Brush": { figure = Figure.Brush; break; }
                case "Penta": { figure = Figure.Penta; break; }
                default: { break; }
            }
        }
    }
}

