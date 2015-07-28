using Buddhabrot.Models;
using Livet;
using Livet.Commands;
using Livet.EventListeners;
using Livet.Messaging;
using Livet.Messaging.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;


namespace Buddhabrot.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        public void Initialize()
        {
            ColorR = 255;
            ColorG = 255;
            ColorB = 255;
            ColorAlpha = 3;
            SaveFileName = "buddhabrot";
            SaveMatrixName = "buddhabrot";
            ImagePixelWidth = 500;
            ImagePixelHeight = 500;
            Iteration = 10000;
            ColorAlphaThreshold = 0;
            PlotCount = 100000;
            DoAllPixel = false;
            IsFullScreen = false;
            IsRunning = false;

        }




        #region MainImage変更通知プロパティ
        private WriteableBitmap _MainImage;

        public WriteableBitmap MainImage
        {
            get
            { return _MainImage; }
            set
            {
                if (_MainImage == value)
                    return;
                _MainImage = value;
                RaisePropertyChanged("MainImage");
            }
        }
        #endregion


        #region IsFullScreen変更通知プロパティ
        private bool _IsFullScreen;

        public bool IsFullScreen
        {
            get
            { return _IsFullScreen; }
            set
            {
                if (_IsFullScreen == value)
                    return;
                _IsFullScreen = value;
                RaisePropertyChanged("IsFullScreen");
            }
        }
        #endregion

        #region IsRunning変更通知プロパティ
        private bool _IsRunning;

        public bool IsRunning
        {
            get
            { return _IsRunning; }
            set
            {
                if (_IsRunning == value)
                    return;
                _IsRunning = value;
                RaisePropertyChanged("IsRunning");
            }
        }
        #endregion



        #region SaveFileName変更通知プロパティ
        private string _SaveFileName;

        public string SaveFileName
        {
            get
            { return _SaveFileName; }
            set
            {
                if (_SaveFileName == value)
                    return;
                _SaveFileName = value;
                RaisePropertyChanged("SaveFileName");
            }
        }
        #endregion


        #region ImagePixelWidth変更通知プロパティ
        private int _ImagePixelWidth;

        public int ImagePixelWidth
        {
            get
            { return _ImagePixelWidth; }
            set
            {
                if (_ImagePixelWidth == value)
                    return;
                _ImagePixelWidth = value;
                RaisePropertyChanged("ImagePixelWidth");
            }
        }
        #endregion


        #region ImagePixelHeight変更通知プロパティ
        private int _ImagePixelHeight;

        public int ImagePixelHeight
        {
            get
            { return _ImagePixelHeight; }
            set
            {
                if (_ImagePixelHeight == value)
                    return;
                _ImagePixelHeight = value;
                RaisePropertyChanged("ImagePixelHeight");
            }
        }
        #endregion


        #region ColorR変更通知プロパティ
        private byte _ColorR;

        public byte ColorR
        {
            get
            { return _ColorR; }
            set
            {
                if (_ColorR == value)
                    return;
                _ColorR = value;
                RaisePropertyChanged("ColorR");
            }
        }
        #endregion


        #region ColorG変更通知プロパティ
        private byte _ColorG;

        public byte ColorG
        {
            get
            { return _ColorG; }
            set
            {
                if (_ColorG == value)
                    return;
                _ColorG = value;
                RaisePropertyChanged("ColorG");
            }
        }
        #endregion


        #region ColorB変更通知プロパティ
        private byte _ColorB;

        public byte ColorB
        {
            get
            { return _ColorB; }
            set
            {
                if (_ColorB == value)
                    return;
                _ColorB = value;
                RaisePropertyChanged("ColorB");
            }
        }
        #endregion


        #region ColorAlpha変更通知プロパティ
        private double _ColorAlpha;

        public double ColorAlpha
        {
            get
            { return _ColorAlpha; }
            set
            {
                if (_ColorAlpha == value)
                    return;
                _ColorAlpha = value;
                RaisePropertyChanged("ColorAlpha");
            }
        }
        #endregion


        #region ColorAlphaThreshold変更通知プロパティ
        private long _ColorAlphaThreshold;

        public long ColorAlphaThreshold
        {
            get
            { return _ColorAlphaThreshold; }
            set
            {
                if (_ColorAlphaThreshold == value)
                    return;
                _ColorAlphaThreshold = value;
                RaisePropertyChanged("ColorAlphaThreshold");
            }
        }
        #endregion


        #region SaveMatrixName変更通知プロパティ
        private string _SaveMatrixName;

        public string SaveMatrixName
        {
            get
            { return _SaveMatrixName; }
            set
            {
                if (_SaveMatrixName == value)
                    return;
                _SaveMatrixName = value;
                RaisePropertyChanged("SaveMatrixName");
            }
        }
        #endregion


        #region Iteration変更通知プロパティ
        private int _Iteration;

        public int Iteration
        {
            get
            { return _Iteration; }
            set
            {
                if (_Iteration == value)
                    return;
                _Iteration = value;
                RaisePropertyChanged("Iteration");
            }
        }
        #endregion


        #region DoAllPixel変更通知プロパティ
        private bool _DoAllPixel;

        public bool DoAllPixel
        {
            get
            { return _DoAllPixel; }
            set
            {
                if (_DoAllPixel == value)
                    return;
                _DoAllPixel = value;
                RaisePropertyChanged("DoAllPixel");
            }
        }
        #endregion


        #region PlotCount変更通知プロパティ
        private int _PlotCount;

        public int PlotCount
        {
            get
            { return _PlotCount; }
            set
            {
                if (_PlotCount == value)
                    return;
                _PlotCount = value;
                RaisePropertyChanged("PlotCount");
            }
        }
        #endregion



        #region SaveImageCommand
        private ViewModelCommand _SaveImageCommand;

        public ViewModelCommand SaveImageCommand
        {
            get
            {
                if (_SaveImageCommand == null)
                {
                    _SaveImageCommand = new ViewModelCommand(SaveImage, CanSaveImage);
                }
                return _SaveImageCommand;
            }
        }

        public bool CanSaveImage()
        {
            return true;
            /*            if (MainImage == null || IsRunning || string.IsNullOrWhiteSpace(SaveFileName))
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }*/
        }

        public async void SaveImage()
        {
            if (MainImage == null)
            {
                Messenger.Raise(new InformationMessage("保存する画像がありません。", "エラー", MessageBoxImage.Error, "Info"));
                return;
            }
            /*            else if (IsRunning)
                        {
                            Messenger.Raise(new InformationMessage("実行中は保存できません。", "エラー", MessageBoxImage.Error, "Info"));
                            return;
                        }
            */
            else if (string.IsNullOrWhiteSpace(SaveFileName))
            {
                Messenger.Raise(new InformationMessage("ファイル名を入力してください。", "エラー", MessageBoxImage.Error, "Info"));
                return;
            }

            await FileSaver.SaveImagePng(MainImage, SaveFileName + ".png");
            Messenger.Raise(new InformationMessage("画像を保存しました。", "", "Info"));
        }

        #endregion



        private long[,] pixelMatrix;





        #region SaveMatrixCommand
        private ViewModelCommand _SaveMatrixCommand;

        public ViewModelCommand SaveMatrixCommand
        {
            get
            {
                if (_SaveMatrixCommand == null)
                {
                    _SaveMatrixCommand = new ViewModelCommand(SaveMatrix, CanSaveMatrix);
                }
                return _SaveMatrixCommand;
            }
        }

        public bool CanSaveMatrix()
        {
            return true;
        }

        public async void SaveMatrix()
        {
            if (pixelMatrix == null)
            {
                Messenger.Raise(new InformationMessage("出力する行列のもととなる画像がありません。", "エラー", MessageBoxImage.Error, "Info"));
                return;
            }
            else if (string.IsNullOrWhiteSpace(SaveMatrixName))
            {
                Messenger.Raise(new InformationMessage("ファイル名を入力してください。", "エラー", MessageBoxImage.Error, "Info"));
                return;
            }

            await FileSaver.SaveMatrix(pixelMatrix, SaveMatrixName + ".csv");
            Messenger.Raise(new InformationMessage("行列を出力しました。", "", "Info"));
        }
        #endregion


        #region StartCalculateCommand
        private ViewModelCommand _StartCalculateCommand;

        public ViewModelCommand StartCalculateCommand
        {
            get
            {
                if (_StartCalculateCommand == null)
                {
                    _StartCalculateCommand = new ViewModelCommand(StartCalculate, CanStartCalculate);
                }
                return _StartCalculateCommand;
            }
        }

        public bool CanStartCalculate()
        {
            return true;
        }

        public void StartCalculate()
        {
            if (IsRunning)
            {
                Messenger.Raise(new InformationMessage("既に実行中です。", "エラー", MessageBoxImage.Error, "Info"));
                return;
            }

            IsRunning = true;
            Calculate();
            Messenger.Raise(new InformationMessage("処理を開始しました。", "", "Info"));
        }

        public async void Calculate()
        {
            MainImage = new WriteableBitmap(ImagePixelWidth, ImagePixelHeight, 96, 96, PixelFormats.Bgra32, null);
            var color = Color.FromRgb(ColorR, ColorG, ColorB);
            var p = new BuddhabrotPlotter(ImagePixelWidth, ImagePixelHeight, color);
            p.AlphaMagnification = ColorAlpha;

            if (DoAllPixel)
            {
                pixelMatrix = await p.Excute(Iteration);
                await Task.Factory.StartNew(() => MainImage = p.MatrixToImage(pixelMatrix).Result);
            }
            else
            {
                pixelMatrix = await p.ExcuteRandom(Iteration, PlotCount);
                await Task.Factory.StartNew(() => MainImage = p.MatrixToImage(pixelMatrix).Result);
            }
            IsRunning = false;
            Messenger.Raise(new InformationMessage("処理が終了しました。", "", "Info"));
        }


        #endregion


        #region WindowCommand
        private ListenerCommand<string> _WindowCommand;

        public ListenerCommand<string> WindowCommand
        {
            get
            {
                if (_WindowCommand == null)
                {
                    _WindowCommand = new ListenerCommand<string>(Window, CanWindow);
                }
                return _WindowCommand;
            }
        }

        public object Stringbuilder { get; private set; }

        public bool CanWindow()
        {
            return true;
        }

        public void Window(string parameter)
        {
            switch (parameter)
            {
                case "Normal":
                    Messenger.Raise(new WindowActionMessage(WindowAction.Normal, "MainWindow"));
                    IsFullScreen = false;
                    break;
                case "Maximize":
                    Messenger.Raise(new WindowActionMessage(WindowAction.Maximize, "MainWindow"));
                    IsFullScreen = false;
                    break;
                case "Minimize":
                    Messenger.Raise(new WindowActionMessage(WindowAction.Minimize, "MainWindow"));
                    IsFullScreen = false;
                    break;
                case "Close":
                    Messenger.Raise(new WindowActionMessage(WindowAction.Close, "MainWindow"));
                    IsFullScreen = false;
                    break;
                case "FullScreen":
                    Messenger.Raise(new WindowActionMessage(WindowAction.Maximize, "MainWindow"));
                    IsFullScreen = true;
                    break;
                default:
                    break;
            }
        }
        #endregion
    }

}