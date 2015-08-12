// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace PhotoStoreDemo
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Stack _undoStack;
        private RubberbandAdorner _cropSelector;
        public PhotoList Photos;
        public PrintList ShoppingCart;

        public MainWindow()
        {
            _undoStack = new Stack();
            InitializeComponent();
        }

        private void WindowLoaded(object sender, EventArgs e)
        {
            var layer = AdornerLayer.GetAdornerLayer(CurrentPhoto);
            _cropSelector = new RubberbandAdorner(CurrentPhoto) {Window = this};
            layer.Add(_cropSelector);
#if VISUALCHILD
            CropSelector.Rubberband.Visibility = Visibility.Hidden;
#endif
#if NoVISUALCHILD
            CropSelector.ShowRect = false;
#endif

            Photos = (PhotoList) (Application.Current.Resources["Photos"] as ObjectDataProvider)?.Data;
            Photos.Path = "..\\..\\Photos";
            ShoppingCart = (PrintList) (Application.Current.Resources["ShoppingCart"] as ObjectDataProvider)?.Data;
        }

        private void PhotoListSelection(object sender, RoutedEventArgs e)
        {
            var path = ((sender as ListBox)?.SelectedItem.ToString());
            BitmapSource img = BitmapFrame.Create(new Uri(path));
            CurrentPhoto.Source = img;
            ClearUndoStack();
            if (_cropSelector != null)
            {
#if VISUALCHILD
                if (Visibility.Visible == CropSelector.Rubberband.Visibility)
                    CropSelector.Rubberband.Visibility = Visibility.Hidden;
#endif
#if NoVISUALCHILD
                if (CropSelector.ShowRect)
                    CropSelector.ShowRect=false;
#endif
            }
            CropButton.IsEnabled = false;
        }

        private void AddToShoppingCart(object sender, RoutedEventArgs e)
        {
            if (PrintTypeComboBox.SelectedItem != null)
            {
                PrintBase item;
                switch (PrintTypeComboBox.SelectedIndex)
                {
                    case 0:
                        item = new Print(CurrentPhoto.Source as BitmapSource);
                        break;
                    case 1:
                        item = new GreetingCard(CurrentPhoto.Source as BitmapSource);
                        break;
                    case 2:
                        item = new Shirt(CurrentPhoto.Source as BitmapSource);
                        break;
                    default:
                        return;
                }
                ShoppingCart.Add(item);
                ShoppingCartListBox.ScrollIntoView(item);
                ShoppingCartListBox.SelectedItem = item;
                if (false == UploadButton.IsEnabled)
                    UploadButton.IsEnabled = true;
                if (false == RemoveButton.IsEnabled)
                    RemoveButton.IsEnabled = true;
            }
        }

        private void RemoveShoppingCartItem(object sender, RoutedEventArgs e)
        {
            if (null != ShoppingCartListBox.SelectedItem)
            {
                var item = ShoppingCartListBox.SelectedItem as PrintBase;
                ShoppingCart.Remove(item);
                ShoppingCartListBox.SelectedIndex = ShoppingCart.Count - 1;
            }
            if (0 == ShoppingCart.Count)
            {
                RemoveButton.IsEnabled = false;
                UploadButton.IsEnabled = false;
            }
        }

        private void Upload(object sender, RoutedEventArgs e)
        {
            if (ShoppingCart.Count > 0)
            {
                var scaleDuration = new TimeSpan(0, 0, 0, 0, ShoppingCart.Count*200);
                var progressAnimation = new DoubleAnimation(0, 100, scaleDuration, FillBehavior.Stop);
                UploadProgressBar.BeginAnimation(RangeBase.ValueProperty, progressAnimation);
                ShoppingCart.Clear();
                UploadButton.IsEnabled = false;
                if (RemoveButton.IsEnabled)
                    RemoveButton.IsEnabled = false;
            }
        }

        private void Rotate(object sender, RoutedEventArgs e)
        {
            if (CurrentPhoto.Source != null)
            {
                var img = (BitmapSource) (CurrentPhoto.Source);
                _undoStack.Push(img);
                var cache = new CachedBitmap(img, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                CurrentPhoto.Source = new TransformedBitmap(cache, new RotateTransform(90.0));
                if (false == UndoButton.IsEnabled)
                    UndoButton.IsEnabled = true;
                if (_cropSelector != null)
                {
#if VISUALCHILD
                    if (Visibility.Visible == CropSelector.Rubberband.Visibility)
                        CropSelector.Rubberband.Visibility = Visibility.Hidden;
#endif
#if NoVISUALCHILD
                if (CropSelector.ShowRect)
                    CropSelector.ShowRect=false;
#endif
                }
                CropButton.IsEnabled = false;
            }
        }

        private void BlackAndWhite(object sender, RoutedEventArgs e)
        {
            if (CurrentPhoto.Source != null)
            {
                var img = (BitmapSource) (CurrentPhoto.Source);
                _undoStack.Push(img);
                CurrentPhoto.Source = new FormatConvertedBitmap(img, PixelFormats.Gray8, BitmapPalettes.Gray256, 1.0);
                if (false == UndoButton.IsEnabled)
                    UndoButton.IsEnabled = true;
                if (_cropSelector != null)
                {
#if VISUALCHILD
                    if (Visibility.Visible == CropSelector.Rubberband.Visibility)
                        CropSelector.Rubberband.Visibility = Visibility.Hidden;
#endif
#if NoVISUALCHILD
                    if (CropSelector.ShowRect)
                        CropSelector.ShowRect = false;
#endif
                }
                CropButton.IsEnabled = false;
            }
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var anchor = e.GetPosition(CurrentPhoto);
            _cropSelector.CaptureMouse();
            _cropSelector.StartSelection(anchor);
            CropButton.IsEnabled = true;
        }

        private void Crop(object sender, RoutedEventArgs e)
        {
            if (CurrentPhoto.Source != null)
            {
                var img = (BitmapSource) (CurrentPhoto.Source);
                _undoStack.Push(img);
                var rect = new Int32Rect
                {
                    X = (int) (_cropSelector.SelectRect.X*img.PixelWidth/CurrentPhoto.ActualWidth),
                    Y = (int) (_cropSelector.SelectRect.Y*img.PixelHeight/CurrentPhoto.ActualHeight),
                    Width = (int) (_cropSelector.SelectRect.Width*img.PixelWidth/CurrentPhoto.ActualWidth),
                    Height = (int) (_cropSelector.SelectRect.Height*img.PixelHeight/CurrentPhoto.ActualHeight)
                };
                CurrentPhoto.Source = new CroppedBitmap(img, rect);
#if VISUALCHILD
                if (Visibility.Visible == CropSelector.Rubberband.Visibility)
                    CropSelector.Rubberband.Visibility = Visibility.Hidden;
#endif
#if NoVISUALCHILD
                if (CropSelector.ShowRect)
                    CropSelector.ShowRect = false;
#endif
                CropButton.IsEnabled = false;
                if (false == UndoButton.IsEnabled)
                    UndoButton.IsEnabled = true;
            }
        }

        private void Undo(object sender, RoutedEventArgs e)
        {
            if (_undoStack.Count > 0)
                CurrentPhoto.Source = (BitmapSource) _undoStack.Pop();
            if (0 == _undoStack.Count)
                UndoButton.IsEnabled = false;
#if VISUALCHILD
                if (Visibility.Visible == CropSelector.Rubberband.Visibility)
                    CropSelector.Rubberband.Visibility = Visibility.Hidden;
#endif
#if NoVISUALCHILD
            if (CropSelector.ShowRect)
                CropSelector.ShowRect = false;
#endif
        }

        private void ClearUndoStack()
        {
            _undoStack.Clear();
            UndoButton.IsEnabled = false;
        }
    }
}