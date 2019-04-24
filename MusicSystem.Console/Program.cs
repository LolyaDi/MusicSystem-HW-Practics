using MusicSystem.DataAccess;
using MusicSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace MusicSystem.Console
{
    public class Program
    {
        private static Repository _repository;
        
        public static void Main(string[] args)
        {
            string userChoice;
            bool isParsed;

            System.Console.WriteLine("Выберите действие:");
            System.Console.WriteLine("1) Вывести музыкальные группы с описанием их песен");
            System.Console.WriteLine("2) Добавить музыкальную группу и песню с описанием");
            System.Console.WriteLine("3) Искать группу");
            System.Console.WriteLine("4) Искать песню");
            System.Console.WriteLine("5) Сортировать песни по рейтингу");
            System.Console.WriteLine("6) Сортировать песни по рейтингу(с конца)");

            userChoice = System.Console.ReadLine();

            isParsed = int.TryParse(userChoice, out int choice);

            if (!isParsed)
            {
                System.Console.WriteLine("Были введены некорректные символы!");
                System.Console.ReadLine();
                return;
            }

            var groups = new List<Group>();
            var songs = new List<Song>();

            using (_repository = new Repository())
            {
                var groupsData = _repository._context.Groups.Include(g => g.Songs);
                var songsData = _repository._context.Songs;

                groups.AddRange(groupsData);
                songs.AddRange(songsData);
            }

            var sortedSongs = songs.OrderBy(s => s.Rating).ToList();
            var sortedSongsBack = songs.OrderByDescending(s => s.Rating).ToList();

            switch (choice)
            {
                case 1:
                    GetAllGroups(groups);
                    break;
                case 2:
                    Add();
                    break;
                case 3:
                    FindGroup(groups);
                    break;
                case 4:
                    FindSong(songs, groups);
                    break;
                case 5:
                    GetAllSongs(sortedSongs);
                    break;
                case 6:
                    GetAllSongs(sortedSongsBack);
                    break;
                default:
                    System.Console.WriteLine("Введенное Вами число не соответствует ни одному из вышеперечисленных!");
                    break;
            }

            System.Console.ReadLine();
        }

        public static void GetAllSongs(List<Song> songs)
        {
            if (songs.Count == 0)
            {
                System.Console.WriteLine("Информации о песнях не существует!");
                return;
            }

            int i = 0;

            System.Console.WriteLine(string.Format("{0}) {1,-30} {2,-20} {3,-20} {4,-20}", "№", "Название песни", "Длительность", "Рейтинг", "Текст песни"));
            foreach (var song in songs)
            {
                System.Console.WriteLine(string.Format("{0}) {1,-30} {2,-20} {3,-20} {4,-20}", ++i, song.Name, song.Length, song.Rating, (song.Lyrics == null ? "Нет" : "Есть")));
            }
        }

        public static void FindSong(List<Song> songs, List<Group> groups)
        {
            if (songs.Count == 0)
            {
                System.Console.WriteLine("Список песен пуст! Найти песню не удастся!");
                return;
            }
            
            System.Console.WriteLine("Введите название песни:");
            string songName = System.Console.ReadLine();

            var song = songs.Find(s => s.Name == songName);

            if (song is null)
            {
                System.Console.WriteLine("Информации о песне не было найдено!");
                return;
            }

            string groupName = groups.Find(g => g.Songs.Contains(song)).Name;

            System.Console.WriteLine("Вывод всей информации об этой песне:");

            System.Console.WriteLine(string.Format("{0,-30} {1,-30} {2,-20} {3,-20}", "Название группы", "Длительность", "Рейтинг", "Текст песни"));
            System.Console.WriteLine(string.Format("{0,-30} {1,-30} {2,-20} {3,-20}", groupName ?? "Нет", song.Length, song.Rating, song.Lyrics == null ? "Нет" : "Есть"));
        }

        public static void FindGroup(List<Group> groups)
        {
            if (groups.Count == 0)
            {
                System.Console.WriteLine("Список групп пуст! Найти группу не удастся!");
                return;
            }

            System.Console.WriteLine("Введите название группы:");
            string groupName = System.Console.ReadLine();
            
            var group = groups.Find(g => g.Name == groupName);
            
            if (group is null)
            {
                System.Console.WriteLine("Информации об этой группе не найдено!");
                return;
            }

            System.Console.WriteLine("Вывод всех песен этой группы:");

            if (group.Songs.Count == 0)
            {
                System.Console.WriteLine("Песен не было найдено!");
                return;
            }

            int i = 0;

            System.Console.WriteLine(string.Format("{0}) {1,-30} {2,-20} {3,-20} {4,-20}", "№", "Название песни", "Длительность", "Рейтинг", "Текст песни"));
            foreach(var song in group.Songs)
            {
                System.Console.WriteLine(string.Format("{0}) {1,-30} {2,-20} {3,-20} {4,-20}", ++i, song.Name, song.Length, song.Rating, song.Lyrics == null ? "Нет" : "Есть"));
            }
        }

        public static void GetAllGroups(List<Group> groups)
        {
            if (groups.Count == 0)
            {
                System.Console.WriteLine("Информации о группах, их песнях не существует!");
                return;
            }

            int i = 0;

            System.Console.WriteLine(string.Format("{0}) {1,-20} {2,-30} {3,-20} {4,-20} {5,-20}", "№", "Название группы", "Название песни", "Длительность", "Рейтинг", "Текст песни"));
            foreach (var group in groups)
            {
                foreach (var song in group.Songs)
                {
                    System.Console.WriteLine(string.Format("{0}) {1,-20} {2,-30} {3,-20} {4,-20} {5,-20}", ++i, group.Name, song.Name, song.Length, song.Rating, song.Lyrics == null? "Нет": "Есть"));
                }
            }
        }

        public static void Add()
        {
            var group = new Group();
            var song = new Song();

            string musicInfo;
            bool isParsed;

            System.Console.WriteLine("Введите название группы:");
            musicInfo = System.Console.ReadLine();

            group.Name = musicInfo;

            System.Console.WriteLine("Введите название песни:");
            musicInfo = System.Console.ReadLine();

            song.Name = musicInfo;

            System.Console.WriteLine("Введите продолжительность песни:");
            musicInfo = System.Console.ReadLine();

            isParsed = TimeSpan.TryParse(musicInfo, out TimeSpan length);

            while (!isParsed)
            {
                System.Console.WriteLine("Неверный ввод! Повторите еще раз:");
                musicInfo = System.Console.ReadLine();

                isParsed = TimeSpan.TryParse(musicInfo, out length);
            }

            song.Length = length;

            System.Console.WriteLine("Введите рейтинг песни:");
            musicInfo = System.Console.ReadLine();

            isParsed = int.TryParse(musicInfo, out int rating);

            while (!isParsed)
            {
                System.Console.WriteLine("Неверный ввод! Повторите еще раз:");
                musicInfo = System.Console.ReadLine();

                isParsed = int.TryParse(musicInfo, out rating);
            }

            song.Rating = rating;

            System.Console.WriteLine("Хотите добавить текст песни? y/n");
            string userChoice = System.Console.ReadLine();

            switch(userChoice)
            {
                case "y":
                    {
                        System.Console.WriteLine("Введите текст песни:");
                        musicInfo = System.Console.ReadLine();

                        song.Lyrics = musicInfo;
                    }
                    break;
                case "n":
                    System.Console.WriteLine("Хорошо");
                    break;
                default:
                    System.Console.WriteLine("...");
                    break;
            }

            var list = new List<Song>{ song };
            group.Songs = list;

            using (_repository = new Repository())
            {
                _repository.Insert(group);
            }

            System.Console.WriteLine("Информация была успешно добавлена!");
        }
    }
}
