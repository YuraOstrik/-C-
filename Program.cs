using System;
using System.Collections.Generic;

class Program
{
    static List<Dictionary> dictionaries = new List<Dictionary>();

    static void Main()
    {
        InitializeDictionaries();
        foreach (var dict in dictionaries)
        {
            dict.WordAdded += OnWordAdded;
            dict.WordRemoved += OnWordRemoved;
            dict.TranslationReplaced += OnTranslationReplaced;
        }

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Меню словарей:");
            Console.WriteLine("1. Создать словарь");
            Console.WriteLine("2. Добавить слово");
            Console.WriteLine("3. Заменить перевод");
            Console.WriteLine("4. Удалить слово или перевод");
            Console.WriteLine("5. Искать перевод");
            Console.WriteLine("0. Выход");

            Console.Write("Выберите пункт: ");
            string choice = Console.ReadLine();

            if (choice == "0") break;

            switch (choice)
            {
                case "1": CreateDictionary(); break;
                case "2": AddWord(); break;
                case "3": ReplaceTranslation(); break;
                case "4": DeleteWordOrTranslation(); break;
                case "5": SearchWord(); break;
                case "6": ExportWord(); break;
                default: Console.WriteLine("Неверный выбор!"); break;
            }

            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }

    static void InitializeDictionaries()
    {
        var dict1 = new Dictionary("Англо-Русский");
        dict1.AddWord("hello", "привет");
        dict1.AddWord("world", "мир");

        var dict2 = new Dictionary("Русско-Английский");
        dict2.AddWord("привет", "hello");
        dict2.AddWord("мир", "world");

        dictionaries.Add(dict1);
        dictionaries.Add(dict2);
    }

    static void CreateDictionary()
    {
        Console.Write("Введите название словаря (например, Англо-Русский): ");
        string name = Console.ReadLine();
        var dict = new Dictionary(name);
        dict.WordAdded += OnWordAdded;
        dict.WordRemoved += OnWordRemoved;
        dict.TranslationReplaced += OnTranslationReplaced;
        dictionaries.Add(dict);
    }

    static void AddWord()
    {
        var dict = SelectDictionary();
        if (dict != null)
        {
            Console.Write("Введите слово: ");
            string word = Console.ReadLine();
            Console.Write("Введите перевод: ");
            string translation = Console.ReadLine();
            dict.AddWord(word, translation);
        }
    }

    static void ReplaceTranslation()
    {
        var dict = SelectDictionary();
        if (dict != null)
        {
            Console.Write("Введите слово: ");
            string word = Console.ReadLine();
            Console.Write("Введите старый перевод: ");
            string oldTranslation = Console.ReadLine();
            Console.Write("Введите новый перевод: ");
            string newTranslation = Console.ReadLine();
            dict.ReplaceTranslation(word, oldTranslation, newTranslation);
        }
    }

    static void DeleteWordOrTranslation()
    {
        var dict = SelectDictionary();
        if (dict != null)
        {
            Console.Write("Введите слово: ");
            string word = Console.ReadLine();
            Console.Write("Удалить всё слово? (да/нет): ");
            if (Console.ReadLine().ToLower() == "да")
            {
                dict.RemoveWord(word);
            }
            else
            {
                Console.Write("Введите перевод для удаления: ");
                string translation = Console.ReadLine();
                dict.RemoveTranslation(word, translation);
            }
        }
    }

    static void SearchWord()
    {
        var dict = SelectDictionary();
        if (dict != null)
        {
            Console.Write("Введите слово для поиска: ");
            string word = Console.ReadLine();
            dict.SearchWord(word);
        }
    }
    static Dictionary SelectDictionary()
    {
        Console.WriteLine("Доступные словари:");
        for (int i = 0; i < dictionaries.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {dictionaries[i].Name}");
        }

        Console.Write("Выберите словарь: ");
        if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= dictionaries.Count)
        {
            return dictionaries[choice - 1];
        }

        Console.WriteLine("Неверный выбор!");
        return null;
    }
    static void OnWordAdded(object sender, WordEventArgs e)
    {
        Console.WriteLine($"Слово '{e.Word}' добавлено в словарь '{((Dictionary)sender).Name}' с переводом '{e.Translation}'.");
    }

    static void OnWordRemoved(object sender, WordEventArgs e)
    {
        Console.WriteLine($"Слово '{e.Word}' удалено из словаря '{((Dictionary)sender).Name}'.");
    }

    static void OnTranslationReplaced(object sender, TranslationEventArgs e)
    {
        Console.WriteLine($"Перевод для слова '{e.Word}' в словаре '{((Dictionary)sender).Name}' заменён с '{e.OldTranslation}' на '{e.NewTranslation}'.");
    }
}

class Dictionary
{
    public string Name { get; }
    private Dictionary<string, List<string>> words = new();
    public event EventHandler<WordEventArgs> WordAdded;
    public event EventHandler<WordEventArgs> WordRemoved;
    public event EventHandler<TranslationEventArgs> TranslationReplaced;

    public Dictionary(string name)
    {
        Name = name;
    }

    public void AddWord(string word, string translation)
    {
        if (!words.ContainsKey(word))
        {
            words[word] = new List<string>();
        }
        words[word].Add(translation);

        WordAdded?.Invoke(this, new WordEventArgs(word, translation));
    }

    public void ReplaceTranslation(string word, string oldTranslation, string newTranslation)
    {
        if (words.ContainsKey(word) && words[word].Remove(oldTranslation))
        {
            words[word].Add(newTranslation);
            TranslationReplaced?.Invoke(this, new TranslationEventArgs(word, oldTranslation, newTranslation));
        }
    }

    public void RemoveWord(string word)
    {
        if (words.ContainsKey(word))
        {
            words.Remove(word);
            WordRemoved?.Invoke(this, new WordEventArgs(word, null));
        }
    }

    public void RemoveTranslation(string word, string translation)
    {
        if (words.ContainsKey(word) && words[word].Count > 1)
        {
            words[word].Remove(translation);
            TranslationReplaced?.Invoke(this, new TranslationEventArgs(word, translation, null));
        }
    }

    public void SearchWord(string word)
    {
        if (words.ContainsKey(word))
        {
            Console.WriteLine($"{word}: {string.Join(", ", words[word])}");
        }
        else
        {
            Console.WriteLine("Слово не найдено.");
        }
    }
}

class WordEventArgs : EventArgs
{
    public string Word { get; }
    public string Translation { get; }

    public WordEventArgs(string word, string translation)
    {
        Word = word;
        Translation = translation;
    }
}

class TranslationEventArgs : EventArgs
{
    public string Word { get; }
    public string OldTranslation { get; }
    public string NewTranslation { get; }

    public TranslationEventArgs(string word, string oldTranslation, string newTranslation)
    {
        Word = word;
        OldTranslation = oldTranslation;
        NewTranslation = newTranslation;
    }
}
