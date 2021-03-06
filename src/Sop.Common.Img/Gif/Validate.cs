﻿using System;
using System.Collections.Generic;
using System.DrawingCore;
using System.IO;

namespace Sop.Common.Img.Gif

{
    public class Validate
    {

        private AnimatedGifEncoder coder = new AnimatedGifEncoder();
        private Stream stream = new MemoryStream();

        private char[] _identifyingCode;
        private int _defaultIdentifyingCodeLen = 4;
        private string _availableLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        /// <summary>
        /// 
        /// </summary>
        private Random random = new Random();


        private int _frameCount = 4;
        private int _delay = 900;
        private int _noiseCount = 15;

        public int Width { get; } = 150;

        public int Height { get; } = 60;

        public string IdentifyingCode
        {
            get { return new string(_identifyingCode); }
        }

        public Validate(int width, int height)
        {
            Width = width < 1 ? 1 : width;
            Height = height < 1 ? 1 : height;
            coder.SetSize(Width, Height);
            coder.SetRepeat(0);
        }

        private void GenerateIdentifyingCode(int codeLength)
        {
            if (codeLength < 1)
                codeLength = _defaultIdentifyingCodeLen;

            List<char> codes = new List<char>();

            for (int i = 0; i < codeLength; i++)
            {
                codes.Add(_availableLetters[random.Next(0, _availableLetters.Length)]);
            }

            _identifyingCode = new char[codes.Count];

            codes.CopyTo(_identifyingCode);
        }

        public Stream Create(Stream _stream)
        {
            coder.Start(_stream);
            Process();
            return stream;
        }
        /// <summary>
        /// 用它的这个方法,比用 stream 生成的要大!
        /// </summary>
        /// <param name="path"></param>
        public void Create(string path)
        {

            FileStream fs = new FileStream(path, FileMode.Create);
            coder.Start(fs);
            Process();
            fs.Close();
        }



        /// <summary>
        /// 
        /// </summary>
        private void Process()
        {

            GenerateIdentifyingCode(_defaultIdentifyingCodeLen);

            Brush br = Brushes.White;
            Rectangle rect = new Rectangle(0, 0, Width, Height);
            Font f = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold);

            for (int i = 0; i < _frameCount; i++)
            {
                System.DrawingCore.Image im = new Bitmap(Width, Height);
                Graphics ga = Graphics.FromImage(im);

                ga.FillRectangle(br, rect);

                int fH = (int)f.GetHeight();
                int fW = (int)ga.MeasureString(IdentifyingCode, f).Width;

                AddNoise(ga);

                ga.DrawString(IdentifyingCode, f, SystemBrushes.Desktop, new PointF(random.Next(1, Width - 1 - fW), random.Next(1, Height - 1 - fH)));
                ga.Flush();
                //coder.Delay= _delay;
                coder.SetDelay(_delay);
                coder.AddFrame(im);
                im.Dispose();
            }
            coder.Finish();
        }

        private void AddNoise(Graphics ga)
        {

            Pen pen = new Pen(SystemColors.GrayText);

            Point[] ps = new Point[_noiseCount];
            for (int i = 0; i < _noiseCount; i++)
            {
                ps[i] = new Point(random.Next(1, Width - 1), random.Next(1, Height - 1));
            }

            ga.DrawLines(pen, ps);
        }

    }
}
