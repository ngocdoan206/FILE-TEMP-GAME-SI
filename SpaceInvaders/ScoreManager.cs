using System;
using System.Collections.Generic;
using System.IO;

namespace SpaceInvaders
{
    // Quản lý bảng điểm: thêm, hiển thị, đọc/ghi file, sort
    public static class ScoreManager
    {
        private static List<int> scores = new List<int>();
        private const string FileName = "scores.txt";

        // Thêm điểm -> gọi khi game over
        public static void AddScore(int score)
        {
            scores.Add(score);
            SaveScoreToFile(score); // lưu ngay điểm vừa đạt được
        }

        // Lưu 1 điểm mới (append, không ghi đè)
        private static void SaveScoreToFile(int score)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(FileName, true)) // append
                {
                    sw.WriteLine(score);
                }

                // ✅ Tạo bản backup
                File.Copy(FileName, FileName + ".bak", true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lưu điểm: " + ex.Message);
                Console.ReadKey(true);
            }
        }

        // Tải toàn bộ điểm từ file vào list
        private static void LoadScores()
        {
            scores.Clear();
            if (!File.Exists(FileName)) return;

            try
            {
                string[] lines = File.ReadAllLines(FileName);
                foreach (var line in lines)
                {
                    if (int.TryParse(line.Trim(), out int s))
                        scores.Add(s);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi đọc file: " + ex.Message);
                Console.ReadKey(true);
            }
        }

        // Hiển thị bảng điểm (có menu con)
        public static void ShowScores()
        {
            LoadScores();

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
                        OverwriteFile();
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
                    // Tạo 1 bản copy để sắp xếp, không ảnh hưởng danh sách gốc
                    var sortedScores = new List<int>(scores);
                    BubbleSortDescending(sortedScores);
                    Console.WriteLine("=== Điểm sắp xếp giảm dần ===");
                    for (int i = 0; i < sortedScores.Count; i++)
                        Console.WriteLine($"{i + 1}. {sortedScores[i]} điểm");
                    Console.WriteLine("\nNhấn phím bất kỳ để quay lại...");
                    Console.ReadKey(true);
                }
                else if (choice == "5")
                {
                    ClearAllScores();
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

        // Ghi đè lại toàn bộ list vào file (dùng sau khi xóa hoặc sắp xếp)
        private static void OverwriteFile()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(FileName, false)) // ghi đè
                {
                    foreach (var s in scores)
                        sw.WriteLine(s);
                }

                // ✅ Tạo bản backup
                File.Copy(FileName, FileName + ".bak", true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi ghi file: " + ex.Message);
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
        private static void ClearAllScores()
        {
            scores.Clear();
            OverwriteFile();
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