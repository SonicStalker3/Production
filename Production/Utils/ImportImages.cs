using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Production.DB;
using System.Windows;
using System.IO;

namespace Production.Utils
{
    public class ImportImages
    {
        public static void Start()
        {
            // Путь к папке с изображениями
            string imagesDirectory = @"C:\Users\Arseniy\Desktop\Учебная практика\src\Большая пачка\Вариант 1\Сессия 1\";
            ProductionEntities _context = DBContext.GetContext();

            // Путь к файлу с данными
            string dataFilePath = @"C:\Users\Arseniy\Desktop\Учебная практика\src\Большая пачка\Вариант 1\Сессия 1\Обработанные данные\materials_k_import.txt";

            // Чтение файла
            var lines = File.ReadAllLines(dataFilePath, Encoding.UTF8);
            var materials = _context.Materials.ToList();

            // Пропускаем заголовок
            foreach (var line in lines.Skip(1))
            {
                var columns = line.Split('\t'); // Разделяем по табуляции

                if (columns.Length < 3) continue; // Проверяем, что достаточно колонок

                string name = columns[0];
                string imagePath = columns[2];

                // Находим материал по имени
                var material = materials.FirstOrDefault(m => m.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

                if (material != null)
                {
                    // Загружаем изображение, если указано
                    if (!string.IsNullOrWhiteSpace(imagePath) && imagePath != "не указано" && imagePath != "отсутствует")
                    {
                        string fullImagePath = Path.Combine(imagesDirectory, imagePath.TrimStart('\\')); // Убираем начальный слэш
                        if (File.Exists(fullImagePath))
                        {
                            byte[] imageBytes = File.ReadAllBytes(fullImagePath);
                            material.Image = imageBytes; // Сохраняем изображение в базе данных
                        }
                        else
                        {
                            MessageBox.Show($"Изображение для материала '{name}' не найдено по пути: {fullImagePath}");
                        }
                    }
                }
                else
                {
                    MessageBox.Show($"Материал '{name}' не найден в базе данных.");
                }
            }

            // Сохраняем изменения в базе данных
            _context.SaveChanges();
        }
    }
}
