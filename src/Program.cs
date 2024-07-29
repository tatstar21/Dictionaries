
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace C_Examination1
{
     public class Program
     {
        public List <MyDictionary> myDictionaries = new List<MyDictionary>();
       
        static void Main(string[] args)
        {
            Console.WriteLine("\t\t\tСЛОВАРИ");
            Program program = new Program();
            MyDictionary? englishDictionary = new MyDictionary();
            englishDictionary.Type = "Англо-русский";
            englishDictionary.GetWords("EnglishDic.txt");
            program.myDictionaries.Add(englishDictionary);
            MyDictionary? russianDictionary = new MyDictionary();
            russianDictionary.Type = "Русско - английский";
            russianDictionary.GetWords("RussianDic.txt");
            program.myDictionaries.Add (russianDictionary);

            while (true)
            {
                Console.WriteLine("Выберете пункт меню:");
                Console.WriteLine("1 - выбрать Англо-русский словарь");
                Console.WriteLine("2 - выбрать Русско-английский словарь");
                Console.WriteLine("Другая цифра - выход из программы");

                int menuItem = int.Parse(Console.ReadLine());

                if(menuItem == 1)
                {
                    program.HideMenu(englishDictionary, "EnglishDic.txt");
                }
                else if(menuItem == 2)
                {
                    program.HideMenu(russianDictionary, "RussianDic.txt");
                }
                else 
                {
                    Console.WriteLine("Выход из программы!");
                    Environment.Exit(0);
                }
            }
        }
        public void HideMenu(MyDictionary dictionary, string nameFileDictionary)
        {
            while (true)
            {
                dictionary.ShowMenu();
                int menuItem2 = int.Parse(Console.ReadLine());

                if (menuItem2 == 1)
                {
                    Console.WriteLine("Добавить слово и перевод в словарь");
                    dictionary.AddWord();
                    dictionary.SaveToAfile(nameFileDictionary);
                }
                else if (menuItem2 == 2)
                {
                    Console.WriteLine("Заменить слово или его перевод в словаре");
                    Console.WriteLine("Если заменить слово нажмите 1");
                    Console.WriteLine("Если заменить перевод нажмите 2");
                    Console.WriteLine("Другая цифра - вернуться в предыдущее меню нажмите");
                    int menuItem3 = int.Parse(Console.ReadLine());

                    if (menuItem3 == 1)
                    {
                        dictionary.ReplaseWord();
                        dictionary.SaveToAfile(nameFileDictionary);
                        break;
                    }
                    else if (menuItem3 == 2)
                    {
                        dictionary.ReplaseTranslation();
                        dictionary.SaveToAfile(nameFileDictionary);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Вернуться в предыдущее меню");
                    }
                }
                else if (menuItem2 == 3)
                {
                    Console.WriteLine("Удалить слово или перевод;");
                    Console.WriteLine("Если удалить слово нажмите 1");
                    Console.WriteLine("Если удалить перевод нажмите 2");
                    Console.WriteLine("Другая цифра - вернуться в предыдущее меню нажмите");
                    int menuItem4 = int.Parse(Console.ReadLine());

                    if (menuItem4 == 1)
                    {
                        dictionary.DeleteWord();
                        dictionary.SaveToAfile(nameFileDictionary);
                        break;
                    }
                    else if (menuItem4 == 2)
                    {
                        dictionary.DeleteTranslation();
                        dictionary.SaveToAfile(nameFileDictionary);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Вернуться в предыдущее меню");
                    }
                }
                else if (menuItem2 == 4)
                {
                    Console.WriteLine("Найти перевод слова ");
                    dictionary.SearchWord();
                }
                else if (menuItem2 == 5)
                {
                    Console.WriteLine("Экспортировать в отдельный файл перевод");
                    dictionary.ExportWordToFile("result.txt");
                }
                else if (menuItem2 == 6)
                {
                    Console.WriteLine("Pacпечатать словарь:");
                    dictionary.PrintDictionary();
                }
                else if (menuItem2 == 0)
                {
                    Console.WriteLine("Выход в предыдущее меню!");
                    break;
                }
                else
                {
                    Console.WriteLine("Выход из программы!");
                    Environment.Exit(0);
                }
            }
        }
     }

    public class MyDictionary
    {
        public string Type { get; set; }

        public List <Word> Words;
        public MyDictionary() { }
        public void ShowMenu()
        {
            Console.WriteLine($"Вы выбрали {Type} словарь!" +
                "\n МЕНЮ:" +
                "\n 1- добавить слово и перевод в словарь;" +
                "\n 2- заменить слово или его перевод в словаре;" +
                "\n 3- удалить слово или перевод;" +
                "\n 4- найти перевод слова (фразы);" +
                "\n 5- экспортировать в отдельный файл перевод;" +
                "\n 6- распечатать словарь;" +
                "\n 0- вернуться к выбору словаря;" +
                "\n Другое - выход из программы!");
        }
        public List <Word> GetWords(string fileName)//"EnglishDic.txt"
        {
            Words = new List<Word>();
            string temp;

            using (StreamReader sr = new StreamReader(fileName))
            {
                while ((temp = sr.ReadLine()) != null)
                {
                    Word word = new Word(temp);
                    Words.Add(word);
                }
            }
            return Words;
        } 
        public bool isExist(string serchWord)
        {
            foreach (Word word in Words)
            {
                if(word.SearchWord == serchWord)
                {
                    return true;
                }
            }
            return false;
        }
        public List<Word> AddWord()
        {
            Console.WriteLine("Введите слово, которое хотите добавить в словарь");
            string wordSearch = Console.ReadLine();
            Console.WriteLine("Введите перевод через запятую");
            string translation = Console.ReadLine();
            Word word = new Word();
            word.SearchWord = wordSearch;
            string[] translationTemp = translation.Split(',',',');
            word.Translation = translationTemp.ToList();
            Words.Add(word);
            Console.WriteLine("Вы успешно добавили слово!");
            return Words;
        }

        public void PrintDictionary()
        {
            foreach (Word word in Words)
            {
                word.PrintWord();
            }
        }
        public void ExportWordToFile(string pathFile) //pathFile = "result.txt";
        {
            Console.WriteLine("Введите слово, которое хотите перенести в отдельный файл");
            string wordSearch = Console.ReadLine();
            bool isExist = false;

            foreach (Word word in Words)
            {
                if (word.SearchWord == wordSearch)
                {
                    using (StreamWriter sw = new StreamWriter(pathFile))
                    { 
                        sw.WriteLine($"Слово: {word.SearchWord}");
                        sw.Write("Перевод:");

                        for (int i = 0; i < word.Translation.Count; i++)
                        {
                            sw.Write($"{word.Translation[i]}, ");
                        }
                    }
                    isExist = true;
                    break;
                }
            }
            if (!isExist)
            {
                Console.WriteLine("Слово в словаре не найдено!");
            }
        }
        public void SearchWord()
        {
            Console.WriteLine("Введите слово, которое хотите найти в словаре");
            string wordSearch = Console.ReadLine();
            bool isExist = false;

            foreach (Word word in Words)
            {
                if(word.SearchWord == wordSearch)
                {
                    word.PrintWord();
                    isExist = true;
                    break;
                }
            }
            if (!isExist)
            {
                Console.WriteLine("Слово в словаре не найдено!");
            }
        }
        public bool Find(List<string> list, string target)
        {
            return list.FindIndex(x => x.Equals(target)) != -1;
        }

        public List<Word> ReplaseWord()
        {
            Console.WriteLine("Введите слово, которое хотите заменить в словаре");
            string wordSearch = Console.ReadLine();

            bool isExist = false;

            foreach (Word word in Words)
            {
                if( word.SearchWord == wordSearch)
                {
                    Console.WriteLine("Введите слово, на которое хотите заменить слово");
                    string wordReplase = Console.ReadLine();
                    isExist = true;
                    word.SearchWord = wordReplase;
                    Console.WriteLine("Вы успешно заменили слово!");
                }
            }
            if (!isExist)
            {
                Console.WriteLine("Слово в словаре не найдено!");
            }
            return Words;
        }

        public List<Word> ReplaseTranslation()
        {
            Console.WriteLine("Введите слово, перевод которого хотите заменить в словаре");
            string wordSearch = Console.ReadLine();

            bool isExist = false;

            foreach (Word word in Words)
            {
                if(word.SearchWord == wordSearch)
                {
                    Console.WriteLine("Введите перевод через запятую");
                    string translationReplase = Console.ReadLine();
                    isExist = true;
                    word.Translation.Clear();
                    string[] translationTemp = translationReplase.Split(',', ',');
                    word.Translation = translationTemp.ToList();
                    Console.WriteLine("Вы успешно заменили перевод!");
                }
            }
            if (!isExist)
            {
                Console.WriteLine("Слово в словаре не найдено!");
            }
            return Words;
        }

        public List<Word> DeleteWord()
        {
            Console.WriteLine("Введите слово, которое хотите удалить в словаре");
            string wordSearch = Console.ReadLine();
            bool isExist = false;

            for(int i = 0; i < Words.Count; i++)
            {
                if (Words[i].SearchWord == wordSearch)
                {
                    isExist = true;
                    Words.RemoveAt(i);
                    Console.WriteLine("Вы успешно удалили слово!");
                    break;
                }
            }
            if (!isExist)
            {
                Console.WriteLine("Слово в словаре не найдено!");
            }
            return Words;
        }

        public List<Word> DeleteTranslation()
        {
            Console.WriteLine("Введите слово, перевод которого хотите удалить в словаре");
            string wordSearch = Console.ReadLine();
            bool isExist = false;
            
            foreach (Word word in Words)
            {
                int lastIndex = word.Translation.Count - 1;

                if (word.SearchWord == wordSearch && lastIndex != 0)
                {
                    isExist = true;
                    for (int i = 0; i < lastIndex; i++)
                    {
                        word.Translation.RemoveAt(i);
                    }
                    Console.WriteLine("Вы успешно удалили перевод!");
                }
                else if(word.SearchWord == wordSearch && lastIndex == 0)
                {
                    Console.WriteLine("Единственный перевод!");
                }
            }
            if (!isExist)
            {
                Console.WriteLine("Слово в словаре не найдено!");
            }
            return Words;
        }
        public void SaveToAfile(string fileName)
        {
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                foreach (Word word in Words)
                {
                    string s = $"{word.SearchWord}:";
                    for (int i = 0; i < word.Translation.Count; i++)
                    {
                        s += $"{word.Translation[i]},";
                    }
                    s = s.Remove(s.Length - 1);
                    s += '\n';
                    sw.Write(s);
                }
            }
        }
    }

    public class Word
    {
        public string? SearchWord { get; set; }
        public List <string> Translation { get; set; }

        public Word() { }
        public Word(string searchWord, List<string> translation) 
        {
            this.SearchWord = searchWord;
            this.Translation = translation;
        }

        public Word(string str)
        {
            createObjectWord(str);
        }

        public void PrintWord()
        {
            Console.WriteLine($"Слово: {SearchWord}");
            Console.WriteLine("Перевод:");

            foreach (var word in Translation)
            {
                Console.WriteLine($"{word}");
            }
        }

        public void createObjectWord(string str)
        {
            char[] delimiterChars = {',', ':',};
            string[] wordsTranslation = str.Split(delimiterChars);

            SearchWord = wordsTranslation[0];
            Translation = wordsTranslation.ToList();

            Translation.RemoveAt(0);
        }
    }
}
