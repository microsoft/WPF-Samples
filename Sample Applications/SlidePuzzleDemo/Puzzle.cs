// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;

namespace SlidePuzzleDemo
{
    /// <summary>
    ///     Interaction logic for Puzzle.xaml
    /// </summary>
    public partial class Puzzle : Window
    {
        private void PuzzleSourceChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = (ComboBox) sender;
            var item = (ComboBoxItem) cb.SelectedValue;
            var tag = (string) item.Tag;

            _stylingPuzzle = true;

            if (_masterVideoElement != null)
            {
                _masterVideoElement.Stop();
                _masterVideoElement = null;
            }

            switch (tag)
            {
                case "Untemplated":
                    _stylingPuzzle = false;
                    break;

                case "StaticBitmap":
                {
                    var masterImage = (Image) Resources["MasterImage"];
                    _elementToChopUp = masterImage;
                    var bitmap = (BitmapSource) masterImage.Source;
                    _puzzleSize = new Size(bitmap.PixelWidth/1.5, bitmap.PixelHeight/1.5);
                }
                    break;

                case "StaticVectorContent":
                {
                    _elementToChopUp = new VectorContent();
                    _puzzleSize = new Size(500, 500);
                }
                    break;

                case "VideoContent":
                {
                    _elementToChopUp = PrepareVideoElement(out _puzzleSize);
                }
                    break;

                case "AnimateVectorContent":
                {
                    var vc = new VectorContent();

                    // Must explicitly start storyboards that only appear in VisualBrushes
                    vc.BeginStoryboard(vc.MyStoryboard);

                    _elementToChopUp = vc;
                    _puzzleSize = new Size(500, 500);
                }

                    break;

                case "Document":
                {
                    FrameworkElement elt = new Document();
                    _elementToChopUp = elt;
                    _puzzleSize = new Size(elt.Width, elt.Height);
                }
                    break;

                case "FormsContent":
                {
                    FrameworkElement elt = new FormContent();
                    _elementToChopUp = elt;
                    _puzzleSize = new Size(700, 500);
                }
                    break;

                case "SpinningCube":
                {
                    var vc = new VectorContent();
                    vc.BeginStoryboard(vc.MyStoryboard);
                    // Must explicitly start storyboards that only appear in VisualBrushes
                    var masterImage = (Image) Resources["TableImage"];

                    var sc = new SpinningCube
                    {
                        CubeMaterial = {Brush = new ImageBrush(masterImage.Source)},
                        CubeMaterial2 = {Brush = new VisualBrush(vc)}
                    };


                    var myStoryboard = (Storyboard) sc.Resources["MyStoryboard"];
                    sc.BeginStoryboard(myStoryboard);
                    // Must explicitly start storyboards that only appear in VisualBrushes
                    _elementToChopUp = sc;
                    _puzzleSize = new Size(500, 500);
                }

                    break;

                default:
                    Debug.Assert(false, "Unexpected Puzzle Source");
                    break;
            }

            NewPuzzleGrid();
        }

        private UIElement PrepareVideoElement(out Size resultingSize)
        {
            _masterVideoElement = (MediaElement) Resources["MasterVideo"];
            _masterVideoElement.UnloadedBehavior = MediaState.Manual;


            resultingSize = new Size(_masterVideoElement.Width, _masterVideoElement.Height);

            _masterVideoElement.Play();

            return _masterVideoElement;
        }

        private void OnMoveMade(object sender, HandledEventArgs e)
        {
            // Blur or unblur based on whether the move was a valid one.
            var blur = (DropShadowEffect)StatusLabel.Effect;
            if (blur != null)
            {
                if (e.Handled)
                {
                    blur.BlurRadius = 0.0;
                    StatusLabel.Content = "";
                }
                else
                {
                    if (blur.BlurRadius >= 4.0) blur.BlurRadius = 2.0; else blur.BlurRadius = 4.0;
                    StatusLabel.Content = "Bad Move!";
                }
            }
        }

        private void NewPuzzleGrid()
        {
            if (_puzzleGrid != null)
            {
                PuzzleHostingPanel.Children.Remove(_puzzleGrid);
            }
            _puzzleGrid = new PuzzleGrid();
            _puzzleGrid.PuzzleWon += delegate { StatusLabel.Content = "Got it!!!"; };

            _puzzleGrid.MoveMade += OnMoveMade;

            _puzzleGrid.IsApplyingStyle = _stylingPuzzle;
            _puzzleGrid.NumRows = _numRows;

            _puzzleGrid.ElementToChopUp = _elementToChopUp;
            _puzzleGrid.PuzzleSize = _puzzleSize;

            _puzzleGrid.ShowNumbers(ChkShowNumbers.IsChecked.Value);

            _puzzleGrid.ShouldAnimateInteractions = ChkShowAnimations.IsChecked.Value;

            PuzzleHostingPanel.Children.Add(_puzzleGrid);
            StatusLabel.Content = "New " + _numRows + "x" + _numRows + " game";
        }

        #region Less Consequential Event Handlers

        private void OnMixUp(object sender, RoutedEventArgs e)
        {
            _puzzleGrid.MixUpPuzzle();
        }

        private void OnResetPuzzle(object sender, RoutedEventArgs e)
        {
            NewPuzzleGrid();
        }

        private void OnShowHideNumbers(object sender, RoutedEventArgs e)
        {
            var cb = (CheckBox) sender;
            _puzzleGrid.ShowNumbers(cb.IsChecked.Value);
        }

        private void OnChkAnimation(object sender, RoutedEventArgs e)
        {
            var cb = (CheckBox) sender;
            _puzzleGrid.ShouldAnimateInteractions = cb.IsChecked.Value;
        }

        private void PuzzleDimensionsChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = (ComboBox) sender;
            var item = (ComboBoxItem) cb.SelectedValue;
            var tag = (string) item.Tag;
            _numRows = int.Parse(tag);
            NewPuzzleGrid();
        }

        #endregion

        #region Private Data and Boilerplate

        public Puzzle()
        {
            InitializeComponent();
            var dropShadow = new DropShadowEffect();
            dropShadow.ShadowDepth = 0;
            dropShadow.BlurRadius = 0;
            dropShadow.Color = Colors.Red;
            StatusLabel.Effect = dropShadow;
        }

        private PuzzleGrid _puzzleGrid;
        private UIElement _elementToChopUp;
        private Size _puzzleSize;
        private bool _stylingPuzzle;
        private int _numRows;

        private MediaElement _masterVideoElement;

        #endregion
    }
}