﻿using FriscoTab;
using System;
using System.Drawing;
using System.IO;

namespace FriscoDev.UI.Utils
{
    public class ImageHelper
    {
        public string savaFile = System.Web.HttpContext.Current.Server.MapPath("~/Images/Graphic");
        private int mGraphicRows = 21;
        private int mGraphicColumns = 36;

        private const int DEFAULT_GRIPHIC_GRID_MARGIN = 15;
        private const int DEFAULT_GRAPHIC_GRID_SIZE = 25;
        private const int DEFAULT_GRAPHIC_CELL_MARGIN = 0;

        private int mGraphicGridMargin = DEFAULT_GRIPHIC_GRID_MARGIN;
        private int mGraphicGridSize = DEFAULT_GRAPHIC_GRID_SIZE;
        private int mGraphicCellMargin = DEFAULT_GRAPHIC_CELL_MARGIN;
        private byte[,] mBitmapData = null;
        public Graphics g = null;
        private Bitmap mGraphicDrawArea = null;

        public void MakeImg(string fileName, byte[,] mBitmap, FriscoTab.PMDDisplaySize type)
        {
            mBitmapData = mBitmap;
            if (type == PMDDisplaySize.TwelveInchPMD)
            {
                mGraphicColumns = 36; mGraphicRows = 21;
            }
            else if (type == PMDDisplaySize.FifteenInchPMD)
            {
                mGraphicColumns = 42; mGraphicRows = 26;
            }
            else if (type == PMDDisplaySize.EighteenInchPMD)
            {
                mGraphicColumns = 48; mGraphicRows = 31;
            }

            int Width = 584;
            int Height = 330;

            mGraphicGridMargin = 5;
            mGraphicGridSize = (int)(((float)(Width - 2 * mGraphicGridMargin) / (float)mGraphicColumns));

            mGraphicDrawArea = new Bitmap(Width, Height);
            g = System.Drawing.Graphics.FromImage(mGraphicDrawArea);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.Clear(Color.White);

            drawGridLine();

            int x, y;

            for (x = 0; x < mGraphicColumns; x++)
            {
                for (y = 0; y < mGraphicRows; y++)
                {
                    if (mBitmapData[x, y] == 1)
                    {
                        drawCell(x + 1, y + 1);
                    }
                }
            }


            if (!Directory.Exists(savaFile))
                Directory.CreateDirectory(savaFile);

            var fileSaveUrl = Path.Combine(savaFile, fileName);

            if (System.IO.File.Exists(fileSaveUrl))
            {
                System.IO.File.SetAttributes(fileSaveUrl, FileAttributes.Normal);
                System.IO.File.Delete(fileSaveUrl);
            }

            mGraphicDrawArea.Save(fileSaveUrl, System.Drawing.Imaging.ImageFormat.Png);
        }

        private void drawGridLine()
        {
            int row, col;

            for (row = 1; row <= mGraphicRows; row++)
            {
                for (col = 1; col <= mGraphicColumns; col++)
                {
                    drawCircle(col, row);
                }
            }

        }

        private void drawCell(int cellX, int cellY)
        {
            int x, y;

            x = mGraphicGridMargin + (cellX - 1) * mGraphicGridSize + mGraphicCellMargin;
            y = mGraphicGridMargin + (cellY - 1) * mGraphicGridSize + mGraphicCellMargin;

            SolidBrush brush;
            Boolean redrawCircle = false;

            if (mBitmapData[cellX - 1, cellY - 1] == 1)
                brush = new SolidBrush(Color.Blue);
            else
            {
                brush = new SolidBrush(Color.White);
                redrawCircle = true;
            }

            Rectangle rect =
                new Rectangle(x, y, mGraphicGridSize - 2 * mGraphicCellMargin,
                              mGraphicGridSize - 2 * mGraphicCellMargin);

            g.FillEllipse(brush, rect);
            if (redrawCircle)
                drawCircle(cellX, cellY);
        }

        private void drawCircle(int cellX, int cellY)
        {
            int x, y;

            x = mGraphicGridMargin + (cellX - 1) * mGraphicGridSize + mGraphicCellMargin;
            y = mGraphicGridMargin + (cellY - 1) * mGraphicGridSize + mGraphicCellMargin;

            Pen mypen = new Pen(Brushes.DarkCyan);
            mypen.Width = 1;

            SolidBrush brush = new SolidBrush(Color.Black);

            Rectangle rect =
                new Rectangle(x, y, mGraphicGridSize - 2 * mGraphicCellMargin,
                              mGraphicGridSize - 2 * mGraphicCellMargin);

            g.DrawEllipse(mypen, rect);
        }


    }
}