using System;
using System.Collections.Generic;
using System.Threading;

namespace SpaceInvaders
{
    class Game
    {
        static int FrameCount = 0;
        static Config.Difficulty currentDifficulty = Config.Difficulty.Medium; // mặc định

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            
            // load dữ liệu cũ trước khi vào menu
            ScoreManager.LoadScoresFromFile("scores.txt", out _);
            PlayerManager.LoadPlayersFromFile("players.txt");

            while (true)
            {
                Console.Clear();
                Console.SetCursorPosition(0, 0); // đảm bảo menu bắt đầu từ đầu màn hình
                Console.WriteLine("=== " + Config.GameTitle + " ===");
                Console.WriteLine("1. Chơi game");
                Console.WriteLine("2. Xem bảng điểm");
                Console.WriteLine("3. Xem Top 10");
                Console.WriteLine("4. Thoát");
                Console.Write("Chọn: ");

                var choice = Console.ReadLine();

                if (choice == "1")
                {
                    ChooseDifficulty();  // gọi chọn độ khó trước khi chơi
                    StartGame();
                }
                else if (choice == "2") ScoreManager.ShowScores();
                else if (choice == "3") PlayerManager.ShowTop10();
                else if (choice == "4") break;
                else
                {
                    Console.WriteLine("Lựa chọn không hợp lệ. Nhấn phím bất kỳ để tiếp tục...");
                    Console.ReadKey();
                }
            }
        }

        // menu chọn độ khó
        static void ChooseDifficulty()
        {
            Console.Clear();
            Console.WriteLine("=== CHỌN ĐỘ KHÓ ===");
            Console.WriteLine("1. Dễ");
            Console.WriteLine("2. Trung bình");
            Console.WriteLine("3. Khó");
            Console.Write("Chọn: ");

            var choice = Console.ReadLine();
            if (choice == "1") currentDifficulty = Config.Difficulty.Easy;
            else if (choice == "2") currentDifficulty = Config.Difficulty.Medium;
            else if (choice == "3") currentDifficulty = Config.Difficulty.Hard;
            else currentDifficulty = Config.Difficulty.Medium;
        }

        static void StartGame()
        {
            RenderWelcomeScreen();
            var gameState = new GameState();

            while (true)
            {
                var initialTimeStamp = DateTime.Now;

                // handles the updated gamestate
                var state = HandleFrame(gameState);
                if (state.CheckGameOver())
                {
                    RenderGameOverScreen(state);
                    ScoreManager.AddScore(state.GameScore); // lưu điểm vào bảng điểm
                    ScoreManager.SaveScoresToFile("scores.txt"); // ghi file

                    var key = Console.ReadKey(true).Key;

                    while (true)
                    {
                        if (key == ConsoleKey.Q || key == ConsoleKey.Enter)
                            break;
                        else
                            key = Console.ReadKey(true).Key;
                    }

                    if (key == ConsoleKey.Q)
                    {
                        break;
                    }
                    else if (key == ConsoleKey.Enter)
                    {
                        state.EscapedInvaderCount = 0;
                        state.GameScore = 0;
                        state.Hero = new Hero(38, 30);
                        state.Invaders = new List<Invader>();
                        state.Bullets = new List<Bullet>();
                    }
                }

                //Draws State to console
                Render(state);

                gameState = state;

                //Extra sleep time to maintain uniform speed
                int napTime = GetNapTime(initialTimeStamp);
                FrameCount++;
                Thread.Sleep(napTime);
            }
        }

        static GameState HandleFrame(GameState state)
        {
            state.GetKeyStrokes();
            state.UpdateBulletLocation();

            if (FrameCount % 10 == 0)
            {
                state.UpdateInvaderLocation();
            }

            // áp dụng độ khó khi spawn invader
            int spawnRate = Config.GetSpawnRate(currentDifficulty);
            if (FrameCount % spawnRate == 0)
            {
                state.GenerateInvader();
            }
            return state;
        }

        static void Render(GameState state)
        {
            var board = new GameBoard();

            board.SetBoardSize(80, 35);
            board.ClearScreen();
            board.RenderHero(state.Hero);
            board.RenderInvaders(state.Invaders);
            board.RenderBullets(state.Bullets);
            board.DisplayGameScore(state.GameScore);
            board.DisplayEscapedInvaderCount(state.EscapedInvaderCount);
            board.HideCursor();
        }

        //time for the thread to sleep to maintain the game speed. 
        static int GetNapTime(DateTime initialTime)
        {
            var finalTimeStamp = DateTime.Now;
            var timeDifference = (finalTimeStamp - initialTime).TotalMilliseconds;
            int naptime = 0;
            if (timeDifference < 33.3)
            {
                naptime = (int)(33.3 - timeDifference);
            }
            return naptime > 0 ? naptime : 0;
        }

        static void RenderGameOverScreen(GameState state)
        {
            Console.Clear();
            Console.SetCursorPosition(30, 14);
            Console.Write("Game Over");
            Console.SetCursorPosition(28, 20);
            Console.Write(String.Format("Your Score: {0}", state.GameScore));

    //  Thêm nhập tên tại đây
            Console.SetCursorPosition(25, 22);
            Console.Write("Nhập tên của bạn: ");
            string playerName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(playerName))
            playerName = "Người chơi ẩn danh";

    // Lưu vào PlayerManager
            PlayerManager.AddGame(playerName, state.GameScore);
            PlayerManager.SavePlayersToFile("players.txt");
    //  Quan trọng: từ đây menu "Xem Top 10" sẽ có dữ liệu

    Thread.Sleep(500);
    Console.SetCursorPosition(15, 32);
    Console.Write("Press Enter key to play again. Press Q to quit the game.");
}

        static void RenderWelcomeScreen()
        {
            var welcomeScreen = new WelcomeScreen();
            welcomeScreen.RenderWelcomeScreen();
        }
    }
}