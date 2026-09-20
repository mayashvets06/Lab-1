using System;

namespace Lab1Music.Models
{
    /// <summary>
    /// Класс-значение «Жанр». Неизменяемый: поля readonly, сеттеров нет.
    /// </summary>
    public sealed class Genre
    {
        public string Name { get; }
        public string Description { get; }

        public Genre(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Название жанра не может быть пустым.");
            Name = name.Trim();
            Description = description?.Trim() ?? "";
        }

        // Изменение возвращает новый объект — исходный не трогаем.
        public Genre WithDescription(string newDescription)
            => new Genre(Name, newDescription);

        public override string ToString() => $"{Name} ({Description})";
    }
}
