using System;
using System.Linq;
using System.Text;

namespace Lab1Music.Models
{
    /// <summary>
    /// Класс-контейнер «Плейлист».
    /// Внутреннее хранилище (массив + счётчик) закрыто.
    /// </summary>
    public class Playlist
    {
        private readonly Track[] _tracks;
        private int _count;

        public string Name { get; }

        public Playlist(string name, int capacity = 10)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Название плейлиста не может быть пустым.");
            if (capacity <= 0)
                throw new ArgumentException("Ёмкость должна быть положительной.");

            Name = name.Trim();
            _tracks = new Track[capacity];
            _count = 0;
        }

        public int Count => _count;

        // ---------- Операции из варианта ----------

        /// <summary>Добавить трек.</summary>
        public void Add(Track track)
        {
            if (track == null) throw new ArgumentNullException(nameof(track));
            if (_count >= _tracks.Length)
                throw new InvalidOperationException("Плейлист заполнен.");
            if (FindByNumber(track.Number) != null)
                throw new InvalidOperationException(
                    $"Трек с номером {track.Number} уже в плейлисте.");

            _tracks[_count++] = track;
        }

        /// <summary>Удалить по номеру. Возвращает признак неудачи, а не падает.</summary>
        public bool RemoveByNumber(int number)
        {
            for (int i = 0; i < _count; i++)
            {
                if (_tracks[i].Number == number)
                {
                    for (int j = i; j < _count - 1; j++)
                        _tracks[j] = _tracks[j + 1];
                    _tracks[--_count] = null!;
                    return true;
                }
            }
            return false;
        }

        /// <summary>Найти трек по номеру или null.</summary>
        public Track? FindByNumber(int number)
        {
            for (int i = 0; i < _count; i++)
                if (_tracks[i].Number == number) return _tracks[i];
            return null;
        }

        /// <summary>Вычисляющая операция: суммарная длительность.</summary>
        public int TotalDurationSeconds()
        {
            int sum = 0;
            for (int i = 0; i < _count; i++) sum += _tracks[i].DurationSeconds;
            return sum;
        }

        /// <summary>Вычисляющая операция: самый популярный трек.</summary>
        public Track? MostPlayed()
        {
            Track? best = null;
            for (int i = 0; i < _count; i++)
                if (best == null || _tracks[i].PlayCount > best.PlayCount)
                    best = _tracks[i];
            return best;
        }

        /// <summary>Вычисляющая операция: сортировка по числу прослушиваний (по убыванию).</summary>
        public Track[] SortedByPlayCountDesc()
        {
            var copy = new Track[_count];
            Array.Copy(_tracks, copy, _count);
            Array.Sort(copy, (a, b) => b.PlayCount.CompareTo(a.PlayCount));
            return copy;
        }

        /// <summary>
        /// ПРАВИЛЬНО: наружу отдаём копию внутреннего массива.
        /// </summary>
        public Track[] ToArray()
        {
            var copy = new Track[_count];
            Array.Copy(_tracks, copy, _count);
            return copy;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Playlist \"{Name}\", треков: {_count}");
            for (int i = 0; i < _count; i++)
                sb.AppendLine("  " + _tracks[i]);
            return sb.ToString();
        }
    }
}
