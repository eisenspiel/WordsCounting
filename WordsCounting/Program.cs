using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using WordsCountingLib;
using System.Reflection;

namespace WordsCounting
{
    class Program
    {
        static void Main(string[] args)
        {
            //Запрашиваем путь к файлу, если путь не введён, по умолчанию путь к файлу в корне диска D:
             Console.Title = "Words Counting -- Test Task for Digital Design";
             Console.WriteLine("Please enter the path to your txt file\nor\nJust press Enter if you want to use the default path \"D:\\Vojnaimir.Tom1.utf8.txt\"");
             string path = Console.ReadLine();
             if (path == "") path = @"D:\Vojnaimir.Tom1.UTF8.txt";
            

            //Читаем файл
            Console.WriteLine($"Trying to open {path}...");
            string whole_text = "";
            try
            {
                whole_text = System.IO.File.ReadAllText(path);
            }
            catch (Exception)
            {
                Console.WriteLine("Something wrong with the path you entered. Please start again and enter a valid vath");
                Environment.Exit(0);
            }
            Console.WriteLine($"Done! There are {whole_text.Length} symbols.\nNow please wait, the book is big, so it's gonna take a while...");
            

            //Запускаем один из двух методов
            //Создаём словарь, куда будем записывать результат подсчта слов
            Dictionary<string, int> wordCount = new();

            Console.WriteLine("Please enter 1 if you want to use private metod with one thread\nor\nEnter anything other for public method with multiple threads");
            string ChosenMethod = Console.ReadLine();
            var WordsCountIstance = new WordsCountingClass();
            if (ChosenMethod == "1")
            {
                //Вызываем из библиотеки WordsCountLib приватный метод подсчёта слов из текста                
                var magicMethod = WordsCountIstance.GetType().GetMethod("CountWords", BindingFlags.NonPublic | BindingFlags.Instance);
                wordCount = (Dictionary<string, int>)magicMethod.Invoke(WordsCountIstance, new object[] { whole_text });
            }
            else
            {
                //Вызываем из библиотеки WordsCountLib публичный метод подсчёта слов из текста с многопоточностью
                var MTIstance = new WordsCountingClass();
                wordCount = WordsCountIstance.CountWordsMultithread(whole_text);
            }
            

            // Выводим топ-10 слов
            Console.WriteLine("Done! Here's top 10 most frequent words from the book:");
            int topCount = 0;
            foreach (var item in wordCount)
            {
                Console.WriteLine("{0}   {1}", item.Key, item.Value);
                topCount++;
                if (topCount == 10) break;
            }
            

            //Записываем результат в файл
            Console.WriteLine("Now please enter the path to the folder where you want to save the results.\nor\nJust press Enter if you want to use the default path \"D:\\\"");
            path = Console.ReadLine();
            if (path == "") path = @"D:\";
            Console.WriteLine($"Full results are available in a txt file here: {path}\\results.txt");
            File.WriteAllLines($"{path}\\results.txt", wordCount.Select(x => x.Key + " " + x.Value));
            

            // Ждём реакции пользователя
            Console.WriteLine("Press any key to close the console app");
            Console.ReadKey();

        }
    }
}
