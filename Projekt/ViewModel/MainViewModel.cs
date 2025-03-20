using System;
using Model;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows;
using System.Threading;
using System.Timers;

namespace ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private BoardModel boardModel;

        private CancellationTokenSource cancellationTokenSource; // Dodajemy pole przechowujące referencję do tokenu anulowania

        private int numberOfBalls;

        public ModelAbstractApi ModelAPI { get; set; }

        public MainViewModel(ModelAbstractApi? API = null)
        {
            if (API == null)
            {
                this.ModelAPI = ModelAbstractApi.CreateApi();
            }
            else
            {
                this.ModelAPI = API;
            }

            NewGameCommand = new RelayCommand(param => NewGame());
            LoadGameCommand = new RelayCommand(param => LoadGame());
            SaveGameCommand = new RelayCommand(param => SaveGame());
            ExitCommand = new RelayCommand(param => Exit());
            SetNumberOfBallsCommand = new RelayCommand(param => SetNumberOfBalls());
            boardModel = ModelAPI.createBoard(600, 800);
        }

        public int NumberOfBalls
        {
            get { return numberOfBalls; }
            set
            {
                numberOfBalls = value;
                OnPropertyChanged(nameof(NumberOfBalls));
            }
        }

        public ObservableCollection<BallModel> Balls
        {
            get { return boardModel.Balls; }
            set
            {
                if (boardModel.Balls != value)
                {
                    boardModel.Balls = value;
                    OnPropertyChanged("Balls");
                }
            }
        }

        public ICommand NewGameCommand { get; private set; }
        public ICommand LoadGameCommand { get; private set; }
        public ICommand SaveGameCommand { get; private set; }
        public ICommand ExitCommand { get; private set; }

        public ICommand SetNumberOfBallsCommand { get; private set; }

        public MainViewModel() : this(null) { }

       

        //private void TimerCallback(object? sender, ElapsedEventArgs e)
        //{
        //    Console.WriteLine("XC VM: " + Balls[0].XCordinate);
        //    Console.WriteLine("YC VM: " + Balls[0].YCordinate);
        //}

        private void NewGame()
        {
            ModelAPI.CreateBallsList(NumberOfBalls);
            Balls = ModelAPI.GetBallsList();
            ModelAPI.newGame();
        }


        void SetNumberOfBalls()
        { }

        public int Height
        {
            get { return boardModel.Height; }
            set
            {
                if (boardModel.Height != value)
                {
                    boardModel.Height = value;
                    OnPropertyChanged("Height");
                }
            }
        }

        public int Width
        {
            get { return boardModel.Width; }
            set
            {
                if (boardModel.Width != value)
                {
                    boardModel.Width = value;
                    OnPropertyChanged("Width");
                }
            }
        }

        public ICommand MakeMoveCommand { get; private set; }

        private bool CanMakeMove()
        {
            // Polecenie "MakeMove" może być wykonane, jeśli plansza ma co najmniej jedną piłkę
            return Balls.Count > 0;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void LoadGame()
        {
            // Kod do wczytania gry
        }

        private void SaveGame()
        {
            // Kod do zapisania gry
        }

        private void Exit()
        {
            // Kod do wyjścia z aplikacji
            Exit();
        }
    }
}