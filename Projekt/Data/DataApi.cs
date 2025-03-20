using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Data
{
    internal sealed class DataApi : DataAbstractApi
    {
        private static object lockObject = new object();
        private boardData board;
        private CancellationTokenSource cancellationTokenSource; // Dodajemy pole przechowujące referencję do tokenu anulowania
        private BuforDanych bufor = null;

        internal BuforDanych Bufor { get => bufor; set => bufor = value; }

        public override void DodajDaneKolizjeKul(string daneTekstowe)
        {
            bufor.DodajDaneKolizjeKul(daneTekstowe);
        }

        public override void zapisz_bufor()
        {
            bufor.ZapiszDaneDoPliku(null, null);
        }

        public override void DodajDaneKolizjeZeScianami(string daneTekstowe)
        {
            bufor.DodajDaneKolizjeZeScianami(daneTekstowe);
        }

        public override void resetBufor()
        {
            this.Bufor = new BuforDanych();
        }

        public override boardData getBoard()
        {
            return board;
        }

        public override object getCommonLock()
        {
            //test
            //bufor.DodajDane("11 12 13 14 15 16 17 18 19 20");
            return DataApi.lockObject;
        }

        public override ballData addBall(int R = 0, int xCord = -5, int yCord = -5, int mass = -5)
        {
            ballData kula = createBall(R, xCord, yCord, mass);
            board.Balls.Add(kula);
            return kula;
        }

        public override boardData createBoard(int height, int width)
        {
            boardData myBoard = new boardData(height, width);
            board = myBoard;
            return myBoard;
        }

        public override void  newGame<T>(Action<int, int, object, T> makeMoveAction, T ball, int height, int width, object lockObject, CancellationToken token)
        {
            Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    //List<T> ballsCollisions = checkCollisionsFunc(ball);
                    
                    makeMoveAction(height, width, lockObject, ball);
                    await Task.Delay(30, token); // Częstotliwość aktualizacji ruchu (10ms)
                }
            }, token);

        }


        public override void CreateBallsList(int balls)
        {
            board.Balls.Clear(); // Wyczyść obecne kule na planszy
            // Dodaj daną ilość nowych kulek o losowych współrzędnych
            for (int i = 0; i < balls; i++)
            {
                ballData kulka = createBall();
                board.Balls.Add(kulka);
                
                Random random = new Random();
                int velocityX = random.Next(-10, 10);
                int velocityY = random.Next(-10, 10);

                while (velocityX >= -2 && velocityX <= 2)
                {
                    velocityX = random.Next(-10, 10);
                }

                while (velocityY >= -2 && velocityY <= 2)
                {
                    velocityY = random.Next(-10, 10);
                }
                board.Balls.ElementAt(i).SetverticalMove(velocityY);
                board.Balls.ElementAt(i).SethorizontalMove(velocityX);
            }
        }

        public override ballData createBall(int R = 0, int xCord = -5, int yCord = -5, int mass = -5)
        {
            if (R != 0 && R > 0)
            {
                if (xCord < 0 || yCord < 0)
                {
                    Random random = new Random();
                    
                    int radius = R; // Przykładowy promień
                    int x = random.Next(0, board.Width - radius); // Losowa współrzędna X
                    int y = random.Next(0, board.Height - radius); // Losowa współrzędna Y

                    if (mass > 0)
                        return new ballData(x, y, radius, mass);
                    else
                    {
                        int masa = random.Next(10, 30); // Przykładowy masa
                        return new ballData(x, y, radius, masa);
                    }
                }
                else
                {
                    int radius = R; // Przykładowy promień
                    Random random = new Random();
                    
                    int x = xCord;
                    int y = yCord;

                    if (mass > 0)
                        return new ballData(x, y, radius, mass);
                    else
                    {
                        int masa = random.Next(10, 30); // Przykładowy masa
                        return new ballData(x, y, radius, masa);
                    }
                        
                }
            }
            else
            {
                Random random = new Random();
                if (xCord < 0 || yCord < 0)
                {
                    int radius = random.Next(10, 30); // Przykładowy promień
                    
                    int x = random.Next(0, board.Width - radius); // Losowa współrzędna X
                    int y = random.Next(0, board.Height - radius); // Losowa współrzędna Y

                    if (mass > 0)
                        return new ballData(x, y, radius, mass);
                    else
                    {
                        int masa = random.Next(10, 30); // Przykładowy masa
                        return new ballData(x, y, radius, masa);
                    }
                }
                else
                {
                    int radius = random.Next(10, 30); // Przykładowy promień
                    
                    int x = xCord;
                    int y = yCord;

                    if (mass > 0)
                        return new ballData(x, y, radius, mass);
                    else
                    {
                        int masa = random.Next(10, 30); // Przykładowy masa
                        return new ballData(x, y, radius, masa);
                    }
                }
            }
        }

        public override void LoadGame(string saveName)
        {
            // Utwórz obiekt blokady
            object lockObject = getCommonLock();

            // Wejdź do sekcji krytycznej
            Monitor.Enter(lockObject);

            try
            {
                // Kod do wczytania gry
                string folderPath = "saves/"+saveName;
                try
                {
                    int fileCount = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories).Length;
                    Console.WriteLine("Liczba plików w folderze: " + fileCount);

                    board.Balls.Clear();

                    for (global::System.Int32 i = 0; i < fileCount; i++)
                    {
                        string name = "ball" + i + ".json";
                        board.Balls.Add(LoadBallData(name, folderPath));
                    }


                }
                catch (Exception e)
                {
                    Console.WriteLine("Wystąpił błąd: " + e.Message);
                }
            }
            finally
            {
                Monitor.Exit(lockObject);
            }
        }

        public override void SaveGame(string saveName)
        {
            // Utwórz obiekt blokady
            object lockObject = getCommonLock();

            // Wejdź do sekcji krytycznej
            Monitor.Enter(lockObject);

            try
            {
                // Kod do zapisania gry
                string folderPath = "saves/"+saveName;
                try
                {
                    DirectoryInfo di = new DirectoryInfo(folderPath);

                    // Dodaj obsługę tworzenia katalogu, gdy nie istnieje
                    if (!di.Exists)
                    {
                        di.Create();
                        Console.WriteLine("Utworzono katalog: " + folderPath);
                    }

                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                    Console.WriteLine("Wszystkie pliki zostały usunięte.");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Wystąpił błąd: " + e.Message);
                }


                int i = 0;
                foreach (var ball in board.Balls)
                {
                    string name = "ball" + i + ".json";
                    SaveBallData(ball, name, saveName);
                    i++;
                }
            }
            finally 
            { 
                Monitor.Exit(lockObject); 
            }
        }

        public override void SaveBallData(ballData data, string fileName, string saveName)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            string jsonString = JsonSerializer.Serialize(data, options);

            string folderPath = "saves/" + saveName;
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string filePath = Path.Combine(folderPath, fileName);

            File.WriteAllText(filePath, jsonString);
        }

        public override ballData LoadBallData(string fileName, string saveName)
        {
            string folderPath = saveName;
            if (!Directory.Exists(folderPath))
            {
                throw new DirectoryNotFoundException($"Folder {folderPath} does not exist.");
            }

            string filePath = Path.Combine(saveName, fileName);

            string jsonString = File.ReadAllText(filePath);

            ballData data = JsonSerializer.Deserialize<ballData>(jsonString);

            return data;
        }

        internal class BuforDanych
        {
            private MemoryStream _buforKolizjeKul;
            private MemoryStream _buforKolizjeZeScianami;
            private string nameKolizjeKul;
            private string nameKolizjeZeScianami;
            private string Path;

            public MemoryStream BuforKolizjeKul { get => _buforKolizjeKul; set => _buforKolizjeKul = value; }
            public MemoryStream BuforKolizjeZeScianami { get => _buforKolizjeZeScianami; set => _buforKolizjeZeScianami = value; }

            public BuforDanych()
            {
                string format = "dd_MM_yyyy_HH_mm_ss_fff";
                string filename = DateTime.Now.ToString(format);
                
                string filename1 = "KolizjeKul_" + filename + ".json";
                string filename2 = "KolizjeZeScianami_" + filename + ".json";

                string folderPath = "StatisticData/" + filename;
                
                Path = folderPath;

                nameKolizjeKul = filename1;
                nameKolizjeZeScianami = filename2;

                BuforKolizjeKul = new MemoryStream();
                BuforKolizjeZeScianami = new MemoryStream();
                AppDomain.CurrentDomain.ProcessExit += ZapiszDaneDoPliku;
            }

            public void DodajDaneKolizjeKul(string daneTekstowe)
            {
                // Rozdzielenie ciągu tekstowego na liczby
                var liczbyTekstowe = daneTekstowe.Split(' ');
                if (liczbyTekstowe.Length != 12)
                {
                    throw new ArgumentException("Expected exactly 12 values in the input string.");
                }

                var Mass_A = double.Parse(liczbyTekstowe[0], CultureInfo.InvariantCulture);
                var Mass_B = double.Parse(liczbyTekstowe[1], CultureInfo.InvariantCulture);
                var OldVelocityX_A = double.Parse(liczbyTekstowe[2], CultureInfo.InvariantCulture);
                var OldVelocityY_A = double.Parse(liczbyTekstowe[3], CultureInfo.InvariantCulture);
                var OldVelocityX_B = double.Parse(liczbyTekstowe[4], CultureInfo.InvariantCulture);
                var OldVelocityY_B = double.Parse(liczbyTekstowe[5], CultureInfo.InvariantCulture);
                var NewVelocityX_A = double.Parse(liczbyTekstowe[6], CultureInfo.InvariantCulture);
                var NewVelocityY_A = double.Parse(liczbyTekstowe[7], CultureInfo.InvariantCulture);
                var NewVelocityX_B = double.Parse(liczbyTekstowe[8], CultureInfo.InvariantCulture);
                var NewVelocityY_B = double.Parse(liczbyTekstowe[9], CultureInfo.InvariantCulture);
                var BilaNo1 = double.Parse(liczbyTekstowe[10], CultureInfo.InvariantCulture);
                var BilaNo2 = double.Parse(liczbyTekstowe[11], CultureInfo.InvariantCulture);


                var kolizjaZKula = new
                {
                    OldVelocityX_A,
                    OldVelocityY_A,
                    NewVelocityX_A,
                    NewVelocityY_A,
                    OldVelocityX_B,
                    OldVelocityY_B,
                    NewVelocityX_B,
                    NewVelocityY_B,
                    Mass_A,
                    Mass_B,
                    BilaNo1,
                    BilaNo2
                };

                var dane = new
                {
                    kolizjaZKula
                };


                // Serializacja danych do bufora
                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(dane, options);
                var bytes = Encoding.UTF8.GetBytes(json);
                BuforKolizjeKul.Write(bytes, 0, bytes.Length);
            }

            public void DodajDaneKolizjeZeScianami(string daneTekstowe)
            {
                // Rozdzielenie ciągu tekstowego na liczby
                var liczbyTekstowe = daneTekstowe.Split(' ');
                if (liczbyTekstowe.Length != 10)
                {
                    throw new ArgumentException("Expected exactly 10 values in the input string.");
                }

                var Mass = double.Parse(liczbyTekstowe[0], CultureInfo.InvariantCulture);
                var OldVelocityX = double.Parse(liczbyTekstowe[1], CultureInfo.InvariantCulture);
                var OldVelocityY = double.Parse(liczbyTekstowe[2], CultureInfo.InvariantCulture);
                var oldX = double.Parse(liczbyTekstowe[5], CultureInfo.InvariantCulture);
                var oldY = double.Parse(liczbyTekstowe[6], CultureInfo.InvariantCulture);
                var NewVelocityX = double.Parse(liczbyTekstowe[3], CultureInfo.InvariantCulture);
                var NewVelocityY = double.Parse(liczbyTekstowe[4], CultureInfo.InvariantCulture);
                var newX = double.Parse(liczbyTekstowe[7], CultureInfo.InvariantCulture);
                var newY = double.Parse(liczbyTekstowe[8], CultureInfo.InvariantCulture);
                var BilaNo = double.Parse(liczbyTekstowe[9], CultureInfo.InvariantCulture);



                var kolizjaZeScianami = new
                {
                    Mass,
                    OldVelocityX,
                    OldVelocityY,
                    oldX,
                    oldY,
                    NewVelocityX,
                    NewVelocityY,
                    newX,
                    newY,
                    BilaNo
                };

                var dane = new
                {
                    kolizjaZeScianami
                };


                // Serializacja danych do bufora
                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(dane, options);
                var bytes = Encoding.UTF8.GetBytes(json);
                BuforKolizjeZeScianami.Write(bytes, 0, bytes.Length);
            }

            ~BuforDanych()
            {
                // Zapis danych do pliku przy zamykaniu programu
                ZapiszDaneDoPliku(null, null);
            }

            public void ZapiszDaneDoPliku(object sender, EventArgs e)
            {
                if (!Directory.Exists(Path))
                {
                    Directory.CreateDirectory(Path);
                }

                // Zapis bufora do pliku
                File.WriteAllBytes(Path + "/" + nameKolizjeKul, BuforKolizjeKul.ToArray());
                File.WriteAllBytes(Path + "/" + nameKolizjeZeScianami, BuforKolizjeZeScianami.ToArray());
            }
        }

    }
}
