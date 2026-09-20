using System;

namespace Lab1Music.Models
{
    /// <summary>
    /// Класс-сущность «Трек».
    /// Инварианты:
    ///  1) длительность от 1 до 3600 секунд;
    ///  2) число прослушиваний может только расти;
    ///  3) название непустое;
    ///  4) номер трека задаётся один раз и не меняется.
    /// </summary>
    public class Track
    {
        // ---------- Статические члены (Задание 2) ----------
        private static int _tracksCreated = 0;
        public static int TracksCreated => _tracksCreated;

        public const int MAX_DURATION = 3600;   // статическая константа

        // ---------- Поля (все private — инкапсуляция) ----------
        private readonly int _number;            // номер задаётся один раз
        private string _title;
        private string _artist;
        private int _durationSeconds;
        private int _playCount;

        // ---------- Конструкторы ----------
        /// <summary>Полный конструктор.</summary>
        public Track(int number, string title, string artist, int durationSeconds)
        {
            if (number <= 0)
                throw new ArgumentException("Номер трека должен быть положительным.");
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Название трека не может быть пустым.");
            if (string.IsNullOrWhiteSpace(artist))
                throw new ArgumentException("Исполнитель не может быть пустым.");
            if (durationSeconds < 1 || durationSeconds > MAX_DURATION)
                throw new ArgumentException(
                    $"Длительность должна быть от 1 до {MAX_DURATION} секунд.");

            _number = number;
            _title = title.Trim();
            _artist = artist.Trim();
            _durationSeconds = durationSeconds;
            _playCount = 0;

            _tracksCreated++;
        }

        /// <summary>Упрощённый конструктор (Задание 1: минимум два конструктора).</summary>
        public Track(string title, string artist)
            : this(++_tracksCreated, title, artist, 180) { }

        // ---------- Геттеры (только к нужным полям) ----------
        public int Number => _number;                       // read-only
        public string Title => _title;
        public string Artist => _artist;
        public int DurationSeconds => _durationSeconds;
        public int PlayCount => _playCount;

        // ---------- Методы предметной области (вместо сеттеров) ----------
        /// <summary>Проиграть один раз.</summary>
        public void Play() => Play(1);

        /// <summary>Проиграть N раз. Перегрузка (Задание 2): короткая вызывает полную.</summary>
        public void Play(int times)
        {
            if (times <= 0)
                throw new ArgumentException("Количество прослушиваний должно быть > 0.");
            _playCount += times;
        }

        /// <summary>Переименовать трек с проверкой.</summary>
        public void Rename(string newTitle)
        {
            if (string.IsNullOrWhiteSpace(newTitle))
                throw new ArgumentException("Новое название не может быть пустым.");
            _title = newTitle.Trim();
        }

        // ---------- toString() ----------
        public override string ToString()
            => $"Track #{_number} \"{_title}\" — {_artist}, {_durationSeconds}с, прослушиваний: {_playCount}";
    }
}
