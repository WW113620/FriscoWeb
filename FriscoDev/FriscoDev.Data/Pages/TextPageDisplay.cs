using FriscoTab;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FriscoDev.Data.Page
{
    public enum FontType
    {
        Regular = 0,
        Large = 1,
        Bold = 2
    }

    public class Character
    {
        public byte[,] charDots = null;
        public PMDDisplaySize[] displaySizeList = null;
        public char alphabit;
        public int width;
        public int height;
        public FontType fontType;
        public int yshift = 0;
        public int charSpacing = 1;
        public Character()
        {
            displaySizeList = new PMDDisplaySize[1];
            displaySizeList[0] = PMDDisplaySize.AllSize;
            alphabit = ' ';
            width = 1;
            height = 7;
            fontType = FontType.Regular;
            yshift = 0;
            charSpacing = 1;
            charDots = new byte[1, 7];
        }
        public Character(string description)
        {
            string name = string.Empty, value = string.Empty;
            int i, rowIdx = 0;
            string[] segList = Regex.Split(description, Environment.NewLine);

            //
            // Default is that the font will apply to all panel size
            //
            displaySizeList = new PMDDisplaySize[1];
            displaySizeList[0] = PMDDisplaySize.AllSize;

            try
            {
                for (i = 0; i < segList.Length; i++)
                {
                    if (segList[i].Contains("Character="))
                    {
                        Utils.GetNameValue(segList[i], ref name, ref value);
                        if (value.Length > 0)
                            alphabit = value[0];
                        else
                            alphabit = ' ';
                    }
                    else if (segList[i].Contains("DisplayType="))
                    {
                        Utils.GetNameValue(segList[i], ref name, ref value);

                        string[] panelSizeList = value.Split(',');

                        displaySizeList = new PMDDisplaySize[panelSizeList.Length];

                        for (int k = 0; k < displaySizeList.Length; k++)
                            displaySizeList[k] = (PMDDisplaySize)Int32.Parse(panelSizeList[k]);

                    }
                    else if (segList[i].Contains("FontType="))
                    {
                        Utils.GetNameValue(segList[i], ref name, ref value);
                        fontType = (FontType)Int32.Parse(value);
                    }
                    else if (segList[i].Contains("Dimention="))
                    {
                        Utils.GetNameValue(segList[i], ref name, ref value);

                        string[] dims = value.Split('x');

                        if (dims.Length == 2)
                        {
                            width = Int32.Parse(dims[0]);
                            height = Int32.Parse(dims[1]);

                            charDots = new byte[width, height];
                        }
                        else
                        {
                            Console.WriteLine("Dimension Format Error: " + value);
                            return;
                        }
                    }
                    else if (segList[i].Contains("CharSpacing="))
                    {
                        Utils.GetNameValue(segList[i], ref name, ref value);

                        charSpacing = Int32.Parse(value);
                    }
                    else if (segList[i].Contains("YShift="))
                    {
                        Utils.GetNameValue(segList[i], ref name, ref value);

                        yshift = Int32.Parse(value);
                    }
                    else
                    {
                        // Dots portion
                        string line = segList[i];

                        if (line.Length > 0)
                        {
                            for (int x = 0; x < width; x++)
                            {
                                if (x < line.Length && line[x] == 'o')
                                    charDots[x, rowIdx] = 1;
                                else
                                    charDots[x, rowIdx] = 0;

                            }
                            rowIdx++;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
        }
    }

    public class DisplayFont
    {
        private List<Character> charList = new List<Character>();
        private Character unsupportedChar = null;
        public DisplayFont(string fontFilename)
        {
            int i, k;
            int startIdx = -1;
            int endIdx = -1;
            string charData = string.Empty;

            //
            // We will display non-supported char display
            // 
            unsupportedChar = new Character();

            try
            {
                string readText = File.ReadAllText(fontFilename);
                string[] segList = Regex.Split(readText, Environment.NewLine);

                for (i = 0; i < segList.Length; i++)
                {
                    if (segList[i].Contains("<Char"))
                        startIdx = i;
                    if (segList[i].Contains("</Char"))
                        endIdx = i;

                    if (endIdx != -1 && startIdx != -1 && endIdx > startIdx)
                    {
                        charData = string.Empty;

                        for (k = startIdx + 1; k < endIdx; k++)
                            charData += (segList[k].Trim() + Environment.NewLine);

                        Character current = new Character(charData);

                        charList.Add(current);

                        startIdx = -1;
                        endIdx = -1;
                    }
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message + "," + charData);
                return;
            }
        }

        public Character getCharacter(char c, PMDDisplaySize displaySize, FontType fontType)
        {
            for (int i = 0; i < charList.Count; i++)
            {
                if (charList[i].fontType == fontType &&
                    charList[i].alphabit == c)
                {
                    for (int k = 0; k < charList[i].displaySizeList.Length; k++)
                    {
                        if (charList[i].displaySizeList[k] == PMDDisplaySize.AllSize)
                            return charList[i];

                        if (displaySize == charList[i].displaySizeList[k])
                            return charList[i];
                    }
                }
            }

            return unsupportedChar;
        }

        public string getDescription()
        {
            string s = string.Empty;

            s += ("Total chars = " + charList.Count + Environment.NewLine);

            for (int i = 0; i < charList.Count; i++)
            {
                string panels = string.Empty;

                for (int k = 0; k < charList[i].displaySizeList.Length; k++)
                {
                    panels += charList[i].displaySizeList[k] + " ";
                }


                s += ((i + 1) + " = [" + charList[i].alphabit + "], Display = " +
                      panels + ", Type=" +
                      charList[i].fontType.ToString() + Environment.NewLine);
            }

            return s;
        }
    }

    public class TextPageDisplay
    {
        DisplayFont fonts = null;

        public TextPageDisplay(string fontFilename)
        {
            fonts = new DisplayFont(fontFilename);
        }

        //
        // dotSize --> Diamter of each Light in pixel
        // spacing --> Distance between each dot in pixel
        // margin  --> Distance between edge dot to the boundary in pixel
        //
        public Bitmap[] getTextDisplayBitmap(PMDDisplaySize displayType, string line1, string line2,
                                              FontType fontType,
                                              TextPageScrollType scrollType,
                                              TextPageScrollStart scrollStart,
                                              TextPageScrollEnd scrollEnd,
                                              int dotSize = 5, int spacing = 15, int margin = 8)
        {
            int i, idx;
            int bitmapWidth, bitmapHeight;
            Bitmap bit;

            #region No Scroll Display
            if (scrollType == TextPageScrollType.No_Scrolling)
            {
                byte[,] canvas = getNoScrollingDisplayOutputData(displayType, fontType, line1, line2);

                Bitmap[] images = new Bitmap[1];

                bit = createBitmapFromData(canvas, dotSize, spacing, margin);

                images[0] = bit;
                return images;
            }
            #endregion

            #region Scrolling Display
            else
            {
                int width, height;

                if (displayType == PMDDisplaySize.EighteenInchPMD)
                {
                    width = 48;
                    height = 31;
                }
                else if (displayType == PMDDisplaySize.FifteenInchPMD)
                {
                    width = 42;
                    height = 26;
                }
                else
                {
                    width = 36;
                    height = 21;
                }

                byte[,] canvas = new byte[width, height];

                bitmapWidth = (width - 1) * spacing + 2 * margin + dotSize;
                bitmapHeight = (height - 1) * spacing + 2 * margin + dotSize;

                List<Bitmap> imageList = new List<Bitmap>();
                int numFrames = 0;

                byte[,] virtualCanvas;
                int virtualCanvasWidth;
                int virtualCanvasHeight;

                virtualCanvas = getScrollingDisplayOutputData(displayType, fontType, line1);

                virtualCanvasWidth = virtualCanvas.GetLength(0);
                virtualCanvasHeight = virtualCanvas.GetLength(1);

                #region
                if (scrollStart == TextPageScrollStart.Full_Field &&
                    scrollEnd == TextPageScrollEnd.Full_Field)
                {
                    numFrames = virtualCanvasWidth - width + 1;
                    if (numFrames <= 0)
                        numFrames = 1;

                    for (i = 0; i < numFrames; i++)
                    {
                        if (scrollType == TextPageScrollType.Right_to_Left)
                            idx = i;
                        else
                            idx = virtualCanvasWidth - width - i;

                        copyArrayData(virtualCanvas, idx, canvas, 0, width);

                        bit = createBitmapFromData(canvas, dotSize, spacing, margin);

                        imageList.Add(bit);
                    }

                    return imageList.ToArray();
                }
                else if (scrollStart == TextPageScrollStart.Full_Field &&
                         scrollEnd == TextPageScrollEnd.Scroll_Out)
                {
                    numFrames = virtualCanvasWidth;

                    for (i = 0; i < numFrames; i++)
                    {
                        if (scrollType == TextPageScrollType.Right_to_Left)
                            idx = i;
                        else
                            idx = virtualCanvasWidth - width - i;

                        copyArrayData(virtualCanvas, idx, canvas, 0, width);
                        bit = createBitmapFromData(canvas, dotSize, spacing, margin);

                        imageList.Add(bit);
                    }

                    // Create last empty frame
                    resetCanvas(ref canvas);
                    bit = createBitmapFromData(canvas, dotSize, spacing, margin);
                    imageList.Add(bit);

                    return imageList.ToArray();

                }
                else if (scrollStart == TextPageScrollStart.Scroll_In &&
                            scrollEnd == TextPageScrollEnd.Full_Field)
                {
                    // 1st Empty frame
                    resetCanvas(ref canvas);
                    bit = createBitmapFromData(canvas, dotSize, spacing, margin);
                    imageList.Add(bit);

                    numFrames = virtualCanvasWidth;

                    int srcIndex = 0;
                    int desIndex = 0;

                    for (i = 0; i < numFrames; i++)
                    {
                        if (scrollType == TextPageScrollType.Right_to_Left)
                        {
                            desIndex = width - 1 - i;
                            srcIndex = i - width + 1;

                            if (srcIndex < 0)
                                srcIndex = 0;

                            if (desIndex < 0)
                                desIndex = 0;

                            //desIndex = width - 1 - i;
                            //srcIndex = i - width + 1;

                            //if (desIndex < 0)
                            //    desIndex = 0;
                        }
                        else
                        {
                            srcIndex = virtualCanvasWidth - i - 1;
                            desIndex = 0;
                        }

                        copyArrayData(virtualCanvas, srcIndex, canvas, desIndex, width);

                        bit = createBitmapFromData(canvas, dotSize, spacing, margin);

                        imageList.Add(bit);
                    }

                    return imageList.ToArray();
                }
                else if (scrollStart == TextPageScrollStart.Scroll_In &&
                            scrollEnd == TextPageScrollEnd.Scroll_Out)
                {
                    // 1st Empty frame
                    resetCanvas(ref canvas);
                    bit = createBitmapFromData(canvas, dotSize, spacing, margin);
                    imageList.Add(bit);

                    numFrames = virtualCanvasWidth + width;

                    int srcIndex = 0;
                    int desIndex = 0;

                    for (i = 0; i < numFrames; i++)
                    {
                        if (scrollType == TextPageScrollType.Right_to_Left)
                        {
                            desIndex = width - 1 - i;
                            srcIndex = i - width + 1;

                            if (desIndex < 0)
                                desIndex = 0;

                            if (srcIndex < 0)
                                srcIndex = 0;
                        }
                        else
                        {
                            srcIndex = virtualCanvasWidth - i - 1;
                            desIndex = 0;
                        }

                        copyArrayData(virtualCanvas, srcIndex, canvas, desIndex, width);

                        bit = createBitmapFromData(canvas, dotSize, spacing, margin);

                        imageList.Add(bit);
                    }

                    // Create last empty frame
                    resetCanvas(ref canvas);
                    bit = createBitmapFromData(canvas, dotSize, spacing, margin);
                    imageList.Add(bit);

                    return imageList.ToArray();
                }
                #endregion

                #region Scroll Right to Left
                {
                    //
                    // Scroll Direction: <------ (Right To Left)
                    //
                    if (scrollStart == TextPageScrollStart.Full_Field &&
                        scrollEnd == TextPageScrollEnd.Full_Field)
                    {
                        numFrames = virtualCanvasWidth - width + 1;
                        if (numFrames <= 0)
                            numFrames = 1;

                        for (i = 0; i < numFrames; i++)
                        {
                            copyArrayData(virtualCanvas, i, canvas, 0, width);

                            bit = createBitmapFromData(canvas, dotSize, spacing, margin);

                            imageList.Add(bit);
                        }

                        return imageList.ToArray();
                    }
                    else if (scrollStart == TextPageScrollStart.Full_Field &&
                             scrollEnd == TextPageScrollEnd.Scroll_Out)
                    {
                        numFrames = virtualCanvasWidth;

                        for (i = 0; i < numFrames; i++)
                        {
                            copyArrayData(virtualCanvas, i, canvas, 0, width);
                            bit = createBitmapFromData(canvas, dotSize, spacing, margin);

                            imageList.Add(bit);
                        }

                        // Create last empty frame
                        resetCanvas(ref canvas);
                        bit = createBitmapFromData(canvas, dotSize, spacing, margin);
                        imageList.Add(bit);

                        return imageList.ToArray();
                    }
                    else if (scrollStart == TextPageScrollStart.Scroll_In &&
                             scrollEnd == TextPageScrollEnd.Full_Field)
                    {
                        // 1st Empty frame
                        resetCanvas(ref canvas);
                        bit = createBitmapFromData(canvas, dotSize, spacing, margin);
                        imageList.Add(bit);

                        numFrames = virtualCanvasWidth;

                        int desIndex = 0;
                        int srcIndex = 0;

                        for (i = 0; i < numFrames; i++)
                        {
                            desIndex = width - 1 - i;
                            srcIndex = i - width + 1;

                            copyArrayData(virtualCanvas, srcIndex, canvas, desIndex, width);

                            bit = createBitmapFromData(canvas, dotSize, spacing, margin);

                            imageList.Add(bit);
                        }

                        return imageList.ToArray();
                    }
                    else if (scrollStart == TextPageScrollStart.Scroll_In &&
                             scrollEnd == TextPageScrollEnd.Scroll_Out)
                    {
                        // 1st Empty frame
                        resetCanvas(ref canvas);
                        bit = createBitmapFromData(canvas, dotSize, spacing, margin);
                        imageList.Add(bit);

                        numFrames = virtualCanvasWidth + width;

                        int desIndex = 0;
                        int srcIndex = 0;

                        for (i = 0; i < numFrames; i++)
                        {
                            desIndex = width - 1 - i;
                            if (desIndex < 0)
                                desIndex = 0;

                            srcIndex = i - width + 1;
                            if (srcIndex < 0)
                                srcIndex = 0;

                            copyArrayData(virtualCanvas, srcIndex, canvas, desIndex, width);

                            bit = createBitmapFromData(canvas, dotSize, spacing, margin);

                            imageList.Add(bit);
                        }

                        // Create last empty frame
                        resetCanvas(ref canvas);
                        bit = createBitmapFromData(canvas, dotSize, spacing, margin);
                        imageList.Add(bit);

                        return imageList.ToArray();
                    }
                }
                #endregion

                return null;

            }
            #endregion
        }

        private Bitmap createBitmapFromData(byte[,] canvas, int dotSize, int spacing, int margin)
        {
            int xpos, ypos;
            int bitmapWidth = (canvas.GetLength(0) - 1) * spacing + 2 * margin + dotSize;
            int bitmapHeight = (canvas.GetLength(1) - 1) * spacing + 2 * margin + dotSize;

            Bitmap bit = new Bitmap(bitmapWidth, bitmapHeight);

            SolidBrush lightOnBrush = new SolidBrush(Color.Gold);
            SolidBrush lightOffBrush = new SolidBrush(Color.FromArgb(30, 30, 30));
            Rectangle rect;

            using (Graphics g = Graphics.FromImage(bit))
            {
                // Fill background
                using (SolidBrush brush = new SolidBrush(Color.Black))
                {
                    g.FillRectangle(brush, 0, 0, bitmapWidth, bitmapHeight);
                }

                for (int x = 0; x < canvas.GetLength(0); x++)
                {
                    for (int y = 0; y < canvas.GetLength(1); y++)
                    {
                        xpos = margin + x * spacing;
                        ypos = margin + y * spacing;

                        if (canvas[x, y] == 0)
                        {
                            rect = new Rectangle(xpos, ypos, dotSize, dotSize);
                            g.FillEllipse(lightOffBrush, rect);
                        }
                        else
                        {
                            rect = new Rectangle(xpos - 1, ypos - 1, dotSize + 2, dotSize + 2);
                            g.FillEllipse(lightOnBrush, rect);
                        }
                    }
                }
            }

            return bit;
        }
        private byte[,] getNoScrollingDisplayOutputData(PMDDisplaySize type, FontType fontType,
                                                        string line1, string line2)
        {
            int i;
            int w = 0, h = 0;
            Boolean has2Lines = false;
            int line1YStart = 0, line2YStart = 0;
            int x, y;

            if (line2 != string.Empty && line2.Length > 0)
                has2Lines = true;

            if (type == PMDDisplaySize.TwelveInchPMD)
            {
                w = 36; h = 21;

                if (has2Lines)
                {
                    line1YStart = 2;
                    line2YStart = 12;
                }
                else
                {
                    if (fontType == FontType.Large)
                        line1YStart = 5;
                    else
                        line1YStart = 7;
                }
            }
            else if (type == PMDDisplaySize.FifteenInchPMD)
            {
                w = 42; h = 26;

                if (has2Lines)
                {
                    line1YStart = 4;
                    line2YStart = 15;
                }
                else
                {
                    if (fontType == FontType.Large)
                        line1YStart = 8;
                    else
                        line1YStart = 10;
                }

            }
            else if (type == PMDDisplaySize.EighteenInchPMD)
            {
                w = 48; h = 31;

                if (has2Lines)
                {
                    line1YStart = 6;
                    line2YStart = 19;
                }
                else
                {
                    line1YStart = 12;
                }

            }

            byte[,] canvas = new byte[w, h];

            //
            // First, we calculate the width for line1 and line2
            //
            int line1Width = 0;
            int line2Width = 0;

            for (i = 0; i < line1.Length; i++)
            {
                Character c = fonts.getCharacter(line1[i], type, fontType);

                line1Width += c.width;

                if (i != 0)
                    line1Width += c.charSpacing;
            }

            for (i = 0; i < line2.Length; i++)
            {
                Character c = fonts.getCharacter(line2[i], type, fontType);

                line2Width += c.width;

                if (i != 0)
                    line2Width += c.charSpacing;
            }

            // 
            // Generate data for line 1
            //
            byte[,] textData = new byte[line1Width, h];
            int xstartPosition;
            int xStart = 0;

            for (i = 0; i < line1.Length; i++)
            {
                Character c = fonts.getCharacter(line1[i], type, fontType);

                if (i != 0)
                    xStart += c.charSpacing;

                for (x = 0; x < c.width; x++)
                {
                    for (y = 0; y < c.height; y++)
                    {
                        if (xStart + x < line1Width && line1YStart + c.yshift + y < h)
                            textData[xStart + x, line1YStart + c.yshift + y] = c.charDots[x, y];
                    }
                }

                xStart += c.width;
            }

            xstartPosition = (int)(0.5 + (float)(w - line1Width) / 2.0);
            // xstartPosition = (int)((float)(w - line1Width) / 2.0);

            writeTextDataToCanvas(textData, ref canvas, xstartPosition);

            if (has2Lines)
            {
                textData = new byte[line2Width, h];

                xStart = 0;
                for (i = 0; i < line2.Length; i++)
                {
                    Character c = fonts.getCharacter(line2[i], type, fontType);

                    if (i != 0)
                        xStart += c.charSpacing;

                    for (x = 0; x < c.width; x++)
                    {
                        for (y = 0; y < c.height; y++)
                        {
                            if (xStart + x < line2Width && line2YStart + c.yshift + y < h)
                                textData[xStart + x, line2YStart + c.yshift + y] = c.charDots[x, y];
                        }
                    }

                    xStart += c.width;
                }

                xstartPosition = (int)(0.5 + (float)(w - line2Width) / 2.0);
                // xstartPosition = (int)((float)(w - line2Width) / 2.0);

                writeTextDataToCanvas(textData, ref canvas, xstartPosition);
            }

            return canvas;
        }

        private byte[,] getScrollingDisplayOutputData(PMDDisplaySize type, FontType fontType, string line1)
        {
            int i, h = 0;
            int line1YStart = 0;
            int x, y;

            if (type == PMDDisplaySize.TwelveInchPMD)
            {
                h = 21;
                line1YStart = 7;
            }
            else if (type == PMDDisplaySize.FifteenInchPMD)
            {
                h = 26;
                line1YStart = 10;
            }
            else if (type == PMDDisplaySize.EighteenInchPMD)
            {
                h = 31;
                line1YStart = 12;
            }

            //
            // First, we calculate the width for line1
            //
            int line1Width = 0;

            for (i = 0; i < line1.Length; i++)
            {
                Character c = fonts.getCharacter(line1[i], type, fontType);

                line1Width += c.width;

                if (i != 0)
                    line1Width += c.charSpacing;
            }

            byte[,] canvas = new byte[line1Width, h];

            // 
            // Generate data for line 1
            //
            byte[,] textData = new byte[line1Width, h];
            int xStart = 0;

            for (i = 0; i < line1.Length; i++)
            {
                Character c = fonts.getCharacter(line1[i], type, fontType);

                if (i != 0)
                    xStart += c.charSpacing;

                for (x = 0; x < c.width; x++)
                {
                    for (y = 0; y < c.height; y++)
                    {
                        canvas[xStart + x, line1YStart + y + c.yshift] = c.charDots[x, y];
                    }
                }

                xStart += c.width;
            }

            return canvas;
        }

        private void writeTextDataToCanvas(byte[,] textData, ref byte[,] canvas, int xstartPosition)
        {
            int x, y;
            int xpos;

            for (x = 0; x < textData.GetLength(0); x++)
            {
                xpos = x + xstartPosition;

                if (xpos < 0 || xpos >= canvas.GetLength(0))
                    continue;

                for (y = 0; y < textData.GetLength(1); y++)
                {
                    if (textData[x, y] == 1)
                        canvas[xpos, y] = 1;
                }
            }
        }


        private void copyArrayData(byte[,] sourceArray, int sourceIndex,
                                   byte[,] destinationArray, int destinationIndex,
                                   int length)
        {
            int i, y;

            resetCanvas(ref destinationArray);

            for (i = 0; i < length; i++)
            {
                if (destinationIndex + i >= destinationArray.GetLength(0) ||
                    sourceIndex + i >= sourceArray.GetLength(0))
                    continue;

                if (sourceIndex + i < 0 | destinationIndex + i < 0)
                    continue;

                for (y = 0; y < destinationArray.GetLength(1); y++)
                    destinationArray[destinationIndex + i, y] = sourceArray[sourceIndex + i, y];
            }
        }
        private void resetCanvas(ref byte[,] canvas)
        {
            int x, y;

            for (x = 0; x < canvas.GetLength(0); x++)
            {
                for (y = 0; y < canvas.GetLength(1); y++)
                {
                    canvas[x, y] = 0;
                }
            }
        }

        private void writeTextToBitmap(ref Bitmap bmp, string s)
        {
            RectangleF rectf = new RectangleF(0, 0, bmp.Width, bmp.Height);

            Graphics g = Graphics.FromImage(bmp);
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.DrawString(s, new Font("Tahoma", 15), Brushes.White, rectf, sf);
        }
        public string getDescription()
        {
            return fonts.getDescription();
        }


    }

}
