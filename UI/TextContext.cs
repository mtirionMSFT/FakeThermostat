using nanoFramework.Presentation.Media;
using nanoFramework.UI;

namespace FakeThermostat.UI
{
    public class TextContext
    {
        private Screen _screen;
        private Font _font;
        //private Bitmap _bitmap;

        public TextContext(Screen screen, short id)
        {
            _screen = screen;
            _font = Resource.GetFont((Resource.FontResources)id);
            //_bitmap = new Bitmap(_font.MaxWidth + 1, _font.Height);
        }

        public void DisplayText(string text, Color color = Color.White,
                                HorizontalTextAlignment horizontalAlignment = HorizontalTextAlignment.Left,
                                VerticalTextAlignment verticalAlignment = VerticalTextAlignment.Top,
                                ushort marginLeft = 0, ushort marginTop = 0, ushort marginRight = 0, ushort marginBottom = 0,
                                bool newLine = false)
        {
            ushort startX = GetHorizontalPosition(GetTextWidth(text), horizontalAlignment, marginLeft, marginRight);
            ushort startY = GetVerticalPosition(verticalAlignment, marginTop, marginBottom);
            ushort width = (ushort)(_screen.Width - startX - marginRight);
            ushort height = (ushort)(_screen.Height - startY - marginBottom);

            //char[] chars = text.ToCharArray();
            //int posX = startX;
            //int posY = startY;
            //foreach (char c in chars)
            //{
            //    _bitmap.Clear();
            //    _bitmap.DrawText(c.ToString(), _font, color, 0, 0);

            //    int width = _font.CharWidth(c);
            //    if (newLine && (posX + width) >= _screen.HorizontalBreakOf)
            //    {
            //        // move to beginning of next line
            //        posX = startX;
            //        posY += _font.Height;
            //    }

            //    _bitmap.Flush(posX, posY, _bitmap.Width, _bitmap.Height);
            //    posX += width;
            //}

            DisplayControl.Write(text, startX, startY, width, height, _font, color, Color.Black);
        }

        public int GetTextWidth(string text)
        {
            int length = 0;
            char[] chars = text.ToCharArray();
            foreach (char c in chars)
            {
                length += _font.CharWidth(c);
            }
            return length;
        }

        public int GetTextHeight(string text)
        {
            return _font.Height;
        }

        private ushort GetHorizontalPosition(int textWidth, HorizontalTextAlignment alignment, ushort marginLeft = 0, ushort marginRight = 0)
        {
            ushort frameWidth = (ushort)(_screen.Width - (marginLeft + marginRight));
            switch (alignment)
            {
                case HorizontalTextAlignment.Center:
                    return (ushort)(marginLeft + ((frameWidth - textWidth) / 2));
                case HorizontalTextAlignment.Right:
                    return (ushort)(marginLeft + ((frameWidth - textWidth) - marginRight));
                case HorizontalTextAlignment.Left:
                default:
                    return marginLeft;
            }
        }

        private ushort GetVerticalPosition(VerticalTextAlignment alignment, ushort marginTop = 0, ushort marginBottom = 0)
        {
            ushort frameHeight = (ushort)(_screen.Height - (marginTop + marginBottom));
            switch (alignment)
            {
                case VerticalTextAlignment.Center:
                    return (ushort)(marginTop + (frameHeight - _font.Height) / 2);
                case VerticalTextAlignment.Bottom:
                    return (ushort)(marginTop + (frameHeight - _font.Height));
                case VerticalTextAlignment.Top:
                default:
                    return marginTop;
            }
        }
    }
}
