using System;
using Lab1Music.Models;

namespace Lab1Music
{
    internal static class Program
    {
        private static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== ЧАСТЬ 1. Корректные операции ===\n");

            var t1 = new Track(1, "Bohemian Rhapsody", "Queen", 355);
            var t2 = new Track(2, "Imagine", "John Lennon", 183);
            var t3 = new Track(3, "Smells Like Teen Spirit", "Nirvana", 301);
            var t4 = new Track("Billie Jean", "Michael Jackson"); // упрощённый конструктор

            // Вложенные объекты-значения (Задание 5)
            t1.AddGenre(new Genre("Rock", "классический рок"));
            t1.AddGenre(new Genre("Progressive", "прогрессивные элементы"));
            t4.AddGenre(new Genre("Pop", "поп-музыка"));

            var playlist = new Playlist("Мой плейлист", capacity: 10);
            playlist.Add(t1);
            playlist.Add(t2);
            playlist.Add(t3);
            playlist.Add(t4);

            t1.Play(5);          // перегрузка: короткая версия
            t2.Play();           // короткая → вызывает полную
            t3.Play(12);
            t4.Play(3);

            Console.WriteLine(playlist);
            Console.WriteLine($"Суммарная длительность: {playlist.TotalDurationSeconds()} с");
            Console.WriteLine($"Самый популярный: {playlist.MostPlayed()}");
            Console.WriteLine($"Всего создано треков (статическое поле): {Track.TracksCreated}");

            Console.WriteLine("\n--- Сортировка по прослушиваниям ---");
            foreach (var t in playlist.SortedByPlayCountDesc())
                Console.WriteLine("  " + t);

            Console.WriteLine("\n\n=== ЧАСТЬ 2. Некорректные попытки (должны быть отбиты) ===\n");

            // 1) длительность 0
            TryAction("Создать трек с длительностью 0",
                () => new Track(10, "Bad", "X", 0));

            // 2) уменьшить число прослушиваний (такого метода нет — покажем через некорректный Play)
            TryAction("Уменьшить число прослушиваний (Play(-3))",
                () => t3.Play(-3));
            Console.WriteLine($"   Состояние t3 после отбоя: {t3}");

            // 3) удалить несуществующий трек
            TryAction("Удалить трек с номером 999",
                () =>
                {
                    bool ok = playlist.RemoveByNumber(999);
                    if (!ok) throw new InvalidOperationException("Трек с номером 999 не найден.");
                });

            // 4) пустое название
            TryAction("Создать трек с пустым названием",
                () => new Track(11, "   ", "X", 120));

            // 5) длина больше MAX_DURATION
            TryAction($"Создать трек с длительностью {Track.MAX_DURATION + 1}",
                () => new Track(12, "TooLong", "X", Track.MAX_DURATION + 1));

            Console.WriteLine("\n\n=== ЧАСТЬ 3. Защита внутреннего массива ===\n");

            // 3.1. ДО исправления: небезопасный геттер отдаёт сам массив.
            var unsafeGenres = t1.GetGenresUnsafe();
            unsafeGenres[0] = new Genre("HACKED", "массив испорчен снаружи");
            Console.WriteLine("После порчи через GetGenresUnsafe():");
            Console.WriteLine($"  t1.GetGenres()[0] = {t1.GetGenres()[0]}");

            // 3.2. ПОСЛЕ: безопасный геттер возвращает копию.
            var safeGenres = t1.GetGenres();
            safeGenres[0] = new Genre("HACKED2", "не должно повлиять");
            safeGenres[1] = new Genre("HACKED3", "не должно повлиять");
            Console.WriteLine("После попытки порчи через GetGenres():");
            Console.WriteLine($"  t1.GetGenres()[0] = {t1.GetGenres()[0]}");
            Console.WriteLine($"  t1.GetGenres()[1] = {t1.GetGenres()[1]}");
            Console.WriteLine("  Внутреннее состояние t1 не изменилось ✔");

            // 3.3. Защита массива внутри контейнера.
            var tracksCopy = playlist.ToArray();
            tracksCopy[0] = null!;
            Console.WriteLine($"\nПосле порчи копии из playlist.ToArray():");
            Console.WriteLine($"  playlist.FindByNumber(1) = {playlist.FindByNumber(1)}");
            Console.WriteLine("  Контейнер по-прежнему хранит свой трек ✔");

            Console.WriteLine("\n\n=== ГОТОВО ===");
        }

        private static void TryAction(string description, Action action)
        {
            Console.WriteLine($"[ПОПЫТКА] {description}");
            try
            {
                action();
                Console.WriteLine("   !!! ОШИБКА: попытка не была отбита !!!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   Отбито: {ex.GetType().Name}: {ex.Message}");
            }
        }
    }
}
