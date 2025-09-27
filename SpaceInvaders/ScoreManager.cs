using System;
using System.Collections.Generic;
using System.IO;

namespace SpaceInvaders
{
    // Quản lý bảng điểm: thêm, hiển thị, đọc/ghi file, sort (Bubble Sort)
    public static class ScoreManager
    {
        private static List<int> scores = new List<int>();

        // Thêm điểm -> gọi khi game over
        public static void AddScore(int score)
        {
            scores.Add(score);
        }

        // Hiển thị bảng điểm (có menu con: tìm kiếm, xóa, thống kê, xóa tất cả)
        public static void ShowScores()
        {
            LoadScoresFromFile("scores.txt", out List<int> loaded);
            if (loaded != null && loaded.Count > 0) scores = loaded;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== BẢNG ĐIỂM ===");

                if (scores.Count == 0)
                {
                    Console.WriteLine("Chưa có điểm nào.");
                }
                else
                {
                    for (int i = 0; i < scores.Count; i++)
                        Console.WriteLine($"{i + 1}. {scores[i]} điểm");
                }

                Console.WriteLine("\n1. Tìm kiếm điểm");
                Console.WriteLine("2. Xóa điểm");
                Console.WriteLine("3. Thống kê");
                Console.WriteLine("4. Sắp xếp điểm (giảm dần)");
                Console.WriteLine("5. Xóa toàn bộ bảng điểm");
                Console.WriteLine("0. Quay lại");
                Console.Write("Chọn: ");

                var choice = Console.ReadLine();
                if (choice == "1")
                {
                    Console.Write("Nhập điểm cần tìm: ");
                    if (int.TryParse(Console.ReadLine(), out int target))
                    {
                        int pos = SearchScore(target);
                        if (pos != -1)
                            Console.WriteLine($"✅ Tìm thấy {target} tại vị trí {pos + 1}");
                        else
                            Console.WriteLine("❌ Không tìm thấy!");
                    }
                    Console.ReadKey(true);
                }
                else if (choice == "2")
                {
                    Console.Write("Nhập số thứ tự muốn xóa: ");
                    if (int.TryParse(Console.ReadLine(), out int idx))
                    {
                        RemoveScore(idx - 1);
                        SaveScoresToFile("scores.txt");
                        Console.WriteLine("Đã xóa!");
                    }
                    Console.ReadKey(true);
                }
                else if (choice == "3")
                {
                    ShowStatistics();
                }
                else if (choice == "4")
                {
                    BubbleSortDescending(scores);
                    SaveScoresToFile("scores.txt");
                    Console.WriteLine("Đã sắp xếp theo điểm giảm dần!");
                    for (int i = 0; i < scores.Count; i++)
                        Console.WriteLine($"{i + 1}. {scores[i]} điểm");
                    Console.WriteLine("\nNhấn phím bất kỳ để quay lại...");
                    Console.ReadKey(true);
                }
                else if (choice == "5")
                {
                    ClearAllScores("scores.txt");
                    Console.WriteLine("Đã xóa toàn bộ bảng điểm!");
                    Console.ReadKey(true);
                }
                else if (choice == "0") break;
                else
                {
                    Console.WriteLine("Lựa chọn sai!");
                    Console.ReadKey(true);
                }
            }
        }

        // Lưu toàn bộ scores ra file, tạo file .bak trước khi ghi đè
        public static void SaveScoresToFile(string fileName)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    string backup = fileName + ".bak";
                    File.Copy(fileName, backup, true);
                }

                using (StreamWriter sw = new StreamWriter(fileName, false))
                {
                    foreach (var s in scores)
                        sw.WriteLine(s);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lưu file: " + ex.Message);
                Console.ReadKey(true);
            }
        }

        // Đọc file scores -> out parameter
        public static void LoadScoresFromFile(string fileName, out List<int> loadedScores)
        {
            loadedScores = new List<int>();
            try
            {
                if (!File.Exists(fileName)) return;
                string[] lines = File.ReadAllLines(fileName);
                foreach (var line in lines)
                {
                    if (int.TryParse(line.Trim(), out int s))
                        loadedScores.Add(s);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi đọc file: " + ex.Message);
                Console.WriteLine("Nhấn phím bất kỳ để tiếp tục...");
                Console.ReadKey(true);
            }
        }

        // Bubble Sort giảm dần
        private static void BubbleSortDescending(List<int> list)
        {
            int n = list.Count;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - 1 - i; j++)
                {
                    if (list[j] < list[j + 1])
                    {
                        int tmp = list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = tmp;
                    }
                }
            }
        }

        // Linear Search
        private static int SearchScore(int target)
        {
            for (int i = 0; i < scores.Count; i++)
            {
                if (scores[i] == target)
                    return i;
            }
            return -1;
        }

        // Xóa theo chỉ số
        private static void RemoveScore(int index)
        {
            if (index >= 0 && index < scores.Count)
                scores.RemoveAt(index);
        }

        // Xóa toàn bộ bảng điểm
        private static void ClearAllScores(string fileName)
        {
            scores.Clear();
            try
            {
                if (File.Exists(fileName))
                {
                    string backup = fileName + ".bak";
                    File.Copy(fileName, backup, true);
                }
                File.WriteAllText(fileName, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi xóa file: " + ex.Message);
                Console.ReadKey(true);
            }
        }

        // Thống kê số ván chơi, điểm cao nhất, điểm TB
        private static void ShowStatistics()
        {
            Console.Clear();
            Console.WriteLine("=== THỐNG KÊ ===");

            if (scores.Count == 0)
            {
                Console.WriteLine("Chưa có dữ liệu.");
            }
            else
            {
                int max = scores[0];
                int sum = 0;
                foreach (var s in scores)
                {
                    if (s > max) max = s;
                    sum += s;
                }
                double avg = (double)sum / scores.Count;

                Console.WriteLine($"Số ván đã chơi: {scores.Count}");
                Console.WriteLine($"Điểm cao nhất: {max}");
                Console.WriteLine($"Điểm trung bình: {avg:F2}");
            }

            Console.WriteLine("\nNhấn phím bất kỳ để quay lại...");
            Console.ReadKey(true);
        }
    }
}