using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public class PuzzlePiece : Control
    {
        private Bitmap image;
        private bool isClicked;
        private int offsetX, offsetY;

        public PuzzlePiece(Bitmap image)
        {
            this.image = image;
            Cursor = Cursors.Hand;

            float aspectRatio = (float)image.Width / image.Height;
            int newWidth = 100;
            int newHeight = (int)(newWidth / aspectRatio);
            Size = new Size(newWidth, newHeight);

            MouseDown += OnMouseDown;
            MouseMove += OnMouseMove;
            MouseUp += OnMouseUp;
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            isClicked = true;
            offsetX = e.X;
            offsetY = e.Y;
            Console.WriteLine("down");
        }

        private const int SnapGridSize = 10;

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (isClicked)
            {
                int newX = (Location.X + e.X - offsetX) / SnapGridSize * SnapGridSize;
                int newY = (Location.Y + e.Y - offsetY) / SnapGridSize * SnapGridSize;

                Location = new Point(newX, newY);
                Console.WriteLine("move");
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            isClicked = false;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawImage(image, ClientRectangle);
        }
    }

    public partial class Form1 : Form
    {
        private PuzzlePiece[] puzzlePieces;
        private string[] puzzleFolders = { "eiffel", "pisa", "pomoika", "svobody" };

        public Form1()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            AddPuzzlePieces(24);
        }

        private void AddPuzzlePieces(int count)
        {
            puzzlePieces = new PuzzlePiece[count];

            Random random = new Random();
            string selectedFolder = puzzleFolders[random.Next(puzzleFolders.Length)];

            for (int i = 0; i < count; i++)
            {
                string filePath = $"../../../{selectedFolder}/{i + 1}.jpg";

                try
                {
                    Bitmap puzzleImage = new Bitmap(filePath);
                    puzzleImage.MakeTransparent();

                    puzzlePieces[i] = new PuzzlePiece(puzzleImage)
                    {
                        Location = new Point(10, 10),
                    };

                    Controls.Add(puzzlePieces[i]);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error {filePath}: {ex.Message}");
                   
                }
            }
        }
    }

}
