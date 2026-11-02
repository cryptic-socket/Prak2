using System;
using System.Drawing;
using System.Windows.Forms;

namespace Prak2
{
    public partial class Form1 : Form
    {
        // Deklarasi variabel komponen UI
        TextBox txtGX1, txtGY1, txtGX2, txtGY2;
        TextBox txtLX, txtLY, txtLR;
        PictureBox picGDasar, picGDDA, picGBres;
        PictureBox picLDasar, picLPolar, picLBres;
        Button btnGaris, btnLingkaran;

        public Form1()
        {
            InitializeComponent();
            SetupUIOtomatis(); // Panggil fungsi pembuat UI
        }

        // Fungsi untuk menggambar UI otomatis tanpa perlu tab Design
        private void SetupUIOtomatis()
        {
            this.Size = new Size(800, 600);
            this.Text = "Algoritma Penggambaran Objek Primitif";

            // --- UI BAGIAN GARIS ---
            this.Controls.Add(new Label() { Text = "PENGGAMBARAN GARIS (X1, Y1) -> (X2, Y2)", Location = new Point(20, 10), Width = 300, Font = new Font("Arial", 9, FontStyle.Bold) });

            txtGX1 = new TextBox() { Location = new Point(20, 40), Width = 40, Text = "10" };
            txtGY1 = new TextBox() { Location = new Point(70, 40), Width = 40, Text = "10" };
            txtGX2 = new TextBox() { Location = new Point(20, 70), Width = 40, Text = "180" };
            txtGY2 = new TextBox() { Location = new Point(70, 70), Width = 40, Text = "180" };

            btnGaris = new Button() { Text = "Gambar", Location = new Point(20, 100), Width = 90 };
            btnGaris.Click += BtnGaris_Click;

            this.Controls.Add(txtGX1); this.Controls.Add(txtGY1);
            this.Controls.Add(txtGX2); this.Controls.Add(txtGY2);
            this.Controls.Add(btnGaris);

            picGDasar = new PictureBox() { Location = new Point(130, 40), Size = new Size(200, 200), BorderStyle = BorderStyle.FixedSingle, BackColor = Color.White };
            picGDDA = new PictureBox() { Location = new Point(340, 40), Size = new Size(200, 200), BorderStyle = BorderStyle.FixedSingle, BackColor = Color.White };
            picGBres = new PictureBox() { Location = new Point(550, 40), Size = new Size(200, 200), BorderStyle = BorderStyle.FixedSingle, BackColor = Color.White };

            this.Controls.Add(picGDasar); this.Controls.Add(picGDDA); this.Controls.Add(picGBres);

            // --- UI BAGIAN LINGKARAN ---
            this.Controls.Add(new Label() { Text = "PENGGAMBARAN LINGKARAN (X, Y, Radius)", Location = new Point(20, 260), Width = 300, Font = new Font("Arial", 9, FontStyle.Bold) });

            txtLX = new TextBox() { Location = new Point(20, 290), Width = 40, Text = "100" };
            txtLY = new TextBox() { Location = new Point(70, 290), Width = 40, Text = "100" };
            txtLR = new TextBox() { Location = new Point(20, 320), Width = 90, Text = "80" };

            btnLingkaran = new Button() { Text = "Gambar", Location = new Point(20, 350), Width = 90 };
            btnLingkaran.Click += BtnLingkaran_Click;

            this.Controls.Add(txtLX); this.Controls.Add(txtLY);
            this.Controls.Add(txtLR); this.Controls.Add(btnLingkaran);

            picLDasar = new PictureBox() { Location = new Point(130, 290), Size = new Size(200, 200), BorderStyle = BorderStyle.FixedSingle, BackColor = Color.White };
            picLPolar = new PictureBox() { Location = new Point(340, 290), Size = new Size(200, 200), BorderStyle = BorderStyle.FixedSingle, BackColor = Color.White };
            picLBres = new PictureBox() { Location = new Point(550, 290), Size = new Size(200, 200), BorderStyle = BorderStyle.FixedSingle, BackColor = Color.White };

            this.Controls.Add(picLDasar); this.Controls.Add(picLPolar); this.Controls.Add(picLBres);
        }

        // --- FUNGSI ALGORITMA PENGGAMBARAN ---
        private void DrawPixel(Bitmap bmp, int x, int y)
        {
            if (x >= 0 && x < bmp.Width && y >= 0 && y < bmp.Height)
                bmp.SetPixel(x, y, Color.Black);
        }

        private void DrawLineDasar(Bitmap bmp, int x1, int y1, int x2, int y2)
        {
            int dx = x2 - x1; int dy = y2 - y1;
            if (dx == 0)
            {
                for (int y = Math.Min(y1, y2); y <= Math.Max(y1, y2); y++) DrawPixel(bmp, x1, y);
                return;
            }
            float m = (float)dy / dx; float c = y1 - m * x1;
            for (int x = Math.Min(x1, x2); x <= Math.Max(x1, x2); x++)
                DrawPixel(bmp, x, (int)Math.Round(m * x + c));
        }

        private void DrawLineDDA(Bitmap bmp, int x1, int y1, int x2, int y2)
        {
            int dx = x2 - x1; int dy = y2 - y1;
            int steps = Math.Max(Math.Abs(dx), Math.Abs(dy));
            float xInc = (float)dx / steps, yInc = (float)dy / steps;
            float x = x1, y = y1;
            for (int i = 0; i <= steps; i++)
            {
                DrawPixel(bmp, (int)Math.Round(x), (int)Math.Round(y));
                x += xInc; y += yInc;
            }
        }

        private void DrawLineBresenham(Bitmap bmp, int x1, int y1, int x2, int y2)
        {
            int dx = Math.Abs(x2 - x1), dy = Math.Abs(y2 - y1);
            int sx = x1 < x2 ? 1 : -1, sy = y1 < y2 ? 1 : -1;
            int err = dx - dy;
            while (true)
            {
                DrawPixel(bmp, x1, y1);
                if (x1 == x2 && y1 == y2) break;
                int e2 = 2 * err;
                if (e2 > -dy) { err -= dy; x1 += sx; }
                if (e2 < dx) { err += dx; y1 += sy; }
            }
        }

        private void Draw8Points(Bitmap bmp, int xc, int yc, int x, int y)
        {
            DrawPixel(bmp, xc + x, yc + y); DrawPixel(bmp, xc - x, yc + y);
            DrawPixel(bmp, xc + x, yc - y); DrawPixel(bmp, xc - x, yc - y);
            DrawPixel(bmp, xc + y, yc + x); DrawPixel(bmp, xc - y, yc + x);
            DrawPixel(bmp, xc + y, yc - x); DrawPixel(bmp, xc - y, yc - x);
        }

        private void DrawCircleDasar(Bitmap bmp, int xc, int yc, int r)
        {
            for (int x = 0; x <= r; x++)
            {
                int y = (int)Math.Round(Math.Sqrt(r * r - x * x));
                Draw8Points(bmp, xc, yc, x, y);
            }
        }

        private void DrawCirclePolar(Bitmap bmp, int xc, int yc, int r)
        {
            float dtheta = 1.0f / r;
            for (float theta = 0; theta <= Math.PI / 4; theta += dtheta)
            {
                int x = (int)Math.Round(r * Math.Cos(theta));
                int y = (int)Math.Round(r * Math.Sin(theta));
                Draw8Points(bmp, xc, yc, x, y);
            }
        }

        private void DrawCircleBresenham(Bitmap bmp, int xc, int yc, int r)
        {
            int x = 0, y = r, p = 3 - 2 * r;
            Draw8Points(bmp, xc, yc, x, y);
            while (x <= y)
            {
                x++;
                if (p < 0) p = p + 4 * x + 6;
                else { y--; p = p + 4 * (x - y) + 10; }
                Draw8Points(bmp, xc, yc, x, y);
            }
        }

        // --- EVENT HANDLER TOMBOL ---
        private void BtnGaris_Click(object sender, EventArgs e)
        {
            int x1 = int.Parse(txtGX1.Text), y1 = int.Parse(txtGY1.Text);
            int x2 = int.Parse(txtGX2.Text), y2 = int.Parse(txtGY2.Text);

            Bitmap bmp1 = new Bitmap(picGDasar.Width, picGDasar.Height);
            Bitmap bmp2 = new Bitmap(picGDDA.Width, picGDDA.Height);
            Bitmap bmp3 = new Bitmap(picGBres.Width, picGBres.Height);

            DrawLineDasar(bmp1, x1, y1, x2, y2);
            DrawLineDDA(bmp2, x1, y1, x2, y2);
            DrawLineBresenham(bmp3, x1, y1, x2, y2);

            picGDasar.Image = bmp1; picGDDA.Image = bmp2; picGBres.Image = bmp3;
        }

        private void BtnLingkaran_Click(object sender, EventArgs e)
        {
            int xc = int.Parse(txtLX.Text), yc = int.Parse(txtLY.Text), r = int.Parse(txtLR.Text);

            Bitmap bmp1 = new Bitmap(picLDasar.Width, picLDasar.Height);
            Bitmap bmp2 = new Bitmap(picLPolar.Width, picLPolar.Height);
            Bitmap bmp3 = new Bitmap(picLBres.Width, picLBres.Height);

            DrawCircleDasar(bmp1, xc, yc, r);
            DrawCirclePolar(bmp2, xc, yc, r);
            DrawCircleBresenham(bmp3, xc, yc, r);

            picLDasar.Image = bmp1; picLPolar.Image = bmp2; picLBres.Image = bmp3;
        }
    }
}