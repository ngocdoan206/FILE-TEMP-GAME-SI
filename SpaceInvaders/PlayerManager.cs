using System;
using System.Collections.Generic;
using System.IO;

namespace SpaceInvaders
{
    public static class PlayerManager
    {
        // Danh sách lưu tất cả (tên + điểm)
        private static List<(string Name, int Score)> players = new List<(string, int)>();

        // Thêm một game vào danh sách
        public static void AddGame(string name, int score)
        {
            players.Add((name, score));
        }

        // Lưu vào file
        public static void SavePlayersToFile(string fileName)
        {
            try
            {
                List<string> lines = new List<string>();
                foreach (var p in players)
                {
                    lines.Add($"{p.Name},{p.Score}");
                }
                File.WriteAllLines(fileName, lines);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lưu người chơi: " + ex.Message);
            }
        }

        // Đọc từ file
        public static void LoadPlayersFromFile(string fileName)
        {
            players.Clear();
            try
            {
                if (!File.Exists(fileName)) return;

                string[] lines = File.ReadAllLines(fileName);
                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 2 && int.TryParse(parts[1], out int score))
                    {
                        players.Add((parts[0], score));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi đọc file người chơi: " + ex.Message);
            }
        }

        // Hiển thị Top 10
        public static void ShowTop10()
        {
            Console.Clear();
            Console.WriteLine("=== TOP 10 NGƯỜI CHƠI ===");

            if (players.Count == 0)
            {
                Console.WriteLine("Chưa có dữ liệu.");
            }
            else
            {
                // Sắp xếp giảm dần theo điểm
                players.Sort((a, b) => b.Score.CompareTo(a.Score));

                for (int i = 0; i < players.Count && i < 10; i++)
                {
                    Console.WriteLine($"{i + 1}. {players[i].Name} - {players[i].Score} điểm");
                }
            }

            Console.WriteLine("\nNhấn phím bất kỳ để quay lại menu...");
            Console.ReadKey(true);
        }
    }
} 