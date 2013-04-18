using System;
using System.Drawing;

public class captcha
{
    #region captcha text for check

        private string txt;
        public string text
        {
            get
            {
                return txt;
            }
        }

    #endregion

    #region public methods

        /// <summary>
        /// Create an image-captcha.
        /// </summary>
        /// <param name="Width">Width of creating image.</param>
        /// <param name="Height">Height of creating image.</param>
        /// <returns>Bitmap. Generated image-captcha.</returns>
        public Bitmap CreateImage(int Width, int Height)
        {
            Random rnd = new Random();

            Bitmap result = new Bitmap(Width, Height);

            //text position
            int Xpos = rnd.Next(0, Width - 80);
            int Ypos = rnd.Next(15, Height - 20);

            Brush[] colors = { Brushes.Black,
                        Brushes.Red,
                        Brushes.RoyalBlue,
                        Brushes.Green };

            Pen[] colorsP = { Pens.Black,
                        Pens.Red,
                        Pens.RoyalBlue,
                        Pens.Green };

            Graphics g = Graphics.FromImage((Image)result);

            //background color
            g.Clear(Color.FromArgb(255, 200, 200, 200));

            //generate text
            txt = String.Empty;
            string ALF = "1234567890QWERTYUIOPASDFGHJKLZXCVBNM";
            for (int i = 0; i < 5; ++i)
                txt += ALF[rnd.Next(ALF.Length)];

            int textColor = rnd.Next(colors.Length);
            g.DrawString(text,
                            new Font("Arial", 15),
                            colors[textColor],
                            new PointF(Xpos, Ypos));

            //interferences
            for (int i = 0; i < Width; ++i)
                for (int j = 0; j < Height; ++j)
                    if (rnd.Next() % 5 == 0)
                        result.SetPixel(i, j, colorsP[textColor].Color);

            return result;
        }

    #endregion
}
