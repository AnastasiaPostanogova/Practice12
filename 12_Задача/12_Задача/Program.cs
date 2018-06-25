using System;
using System.Collections.Generic;

namespace _12_Задача
{
    class TreeElement // Класс дерево
    {
        public readonly int Data; // Инф. поле
        public TreeElement Left; // Адрес левого поддерева
        public TreeElement Right; // Адрес правого поддерева

        public TreeElement(int data = 0, TreeElement left = null, TreeElement right = null) //Конструктор
        {
            Data = data;
            Left = left;
            Right = right;
        }
    }

    class MyArray //Класс массива
    {
        private static int[] arr { get; set; } //массив
        private static TreeElement root; //Корень дерева
        private static readonly List<int> result = new List<int>(); // результат сортировки деревом

        public MyArray(int size = 0) //Конструктор с размером
        {
            arr = new int[size];
        }

        public MyArray(int[] Arr) // Конструктор с массивом
        {
            arr = Arr;
        }

        private void BucketSort(out int countEqual, out int countSwap) // Блочная сортировка 
        {
            // Предварительная проверка элементов исходного массива 
            // 
            countEqual = 0;
            countSwap = 0;

            // Поиск элементов с максимальным и минимальным значениями
            // 

            int maxValue = arr[0];
            int minValue = arr[0];

            for (int i = 1; i < arr.Length; i++)
            {
                if (arr[i] > maxValue)
                { maxValue = arr[i]; countEqual++; }

                if (arr[i] < minValue)
                { minValue = arr[i]; countEqual++; }
            }

            // Создание временного массива "карманов" в количестве, 
            // достаточном для хранения всех возможных элементов, 
            // чьи значения лежат в диапазоне между minValue и maxValue. 
            // Т.е. для каждого элемента массива выделяется "карман" List<int>. 
            // При заполнении данных "карманов" элементы исходного не отсортированного массива 
            // будут размещаться в порядке возрастания собственных значений "слева направо". 
            // 

            List<int>[] bucket = new List<int>[maxValue - minValue + 1];

            for (int i = 0; i < bucket.Length; i++)
            {
                bucket[i] = new List<int>();
            }

            // Занесение значений в пакеты 
            // 

            for (int i = 0; i < arr.Length; i++)
            {
                bucket[arr[i] - minValue].Add(arr[i]);
            }

            // Восстановление элементов в исходный массив 
            // из карманов в порядке возрастания значений 
            // 

            int position = 0;
            for (int i = 0; i < bucket.Length; i++)
            {
                if (bucket[i].Count > 0)
                {
                    for (int j = 0; j < bucket[i].Count; j++)
                    {
                        arr[position] = bucket[i][j];
                        position++;
                        countSwap++;
                    }
                }
            }
        }

        private void BinaryTreeSort(out int countCompare, out int countSwap) // Сортировка бинарным деревом
        {
            countCompare = 0;
            countSwap = 0;
            root = null;
            result.Clear();
            FormTree(arr, out countCompare, out countSwap);
            GetSortedNumRec(root);
            arr = result.ToArray();
        }

        private static void AddToTreeElement(int value, ref TreeElement localRoot, ref int countCompare, ref int countSwap) //Добавление элемента в дерево
        {
            if (localRoot == null)
            {
                countSwap++;
                localRoot = new TreeElement(value);
                return;
            }
            countCompare++;
            if (localRoot.Data < value)
            {
                countSwap++;
                AddToTreeElement(value, ref localRoot.Right, ref countCompare, ref countSwap);
            }
            else
            {
                countSwap++;
                AddToTreeElement(value, ref localRoot.Left, ref countCompare, ref countSwap);
            }
        }

        public static void FormTree(int[] arr, out int countCompare, out int countSwap) //Дерево из массива
        {
            countCompare = 0;
            countSwap = 0;
            foreach (int el in arr)
                AddToTreeElement(el, ref root, ref countCompare, ref countSwap);
        }

        private static void GetSortedNumRec(TreeElement node) // Обход дерева
        {
            if (node != null)
            {
                GetSortedNumRec(node.Left);
                result.Add(node.Data);
                GetSortedNumRec(node.Right);
            }
        }

        private MyArray CreateArrayIncrease() //Создание массива, упорядоченного по возрастанию
        {
            for (int i = 0; i < arr.Length; i++) arr[i] = i + 1;

            return new MyArray(arr);
        }

        private MyArray CreateArrayDecrease() //Создание массива, упорядоченного по убыванию
        {
            for (int i = 0; i < arr.Length; i++) arr[i] = arr.Length - i;

            return new MyArray(arr);
        }

        private MyArray CreateArrayRandom() //Создание не упорядоченного массива
        {
            Random rnd = new Random();
            for (int i = 0; i < arr.Length; i++) arr[i] = rnd.Next(-100, 101);

            return new MyArray(arr);
        }

        public void IncreaseSort() //Сортировка возрастающего массива
        {
            var array2 = CreateArrayIncrease();
            Console.WriteLine("Массив до сортировки:");
            Show();

            BucketSort(out var countSravn, out var countSwap);
            Console.WriteLine("\n\nБлочная сортировка:");
            Console.WriteLine("Кол-во сравнений: {0}", countSravn);
            Console.WriteLine("Кол-во перестановок: {0}", countSwap);
            Console.WriteLine("Отсортированный массив: ");
            Show();

            array2.BinaryTreeSort(out countSravn, out countSwap);
            Console.WriteLine("\n\nСортировка с помощью двоичного дерева");
            Console.WriteLine("Кол-во сравнений: {0}", countSravn);
            Console.WriteLine("Кол-во перестановок: {0}", countSwap);
            Console.WriteLine("Отсортированный массив: ");
            array2.Show();

            Console.WriteLine("\n\nДля продолжения нажмите на любую клавишу...");
            Console.ReadKey(true);
        }

        public void DecreaseSort() //Сортировка убывающего массива
        {
            var array2 = CreateArrayDecrease();
            Console.WriteLine("Массив до сортировкм:");
            Show();

            BucketSort(out var countSravn, out var countSwap);
            Console.WriteLine("\n\nБлочная сортировка:");
            Console.WriteLine("Кол-во сравнений: {0}", countSravn);
            Console.WriteLine("Кол-во перестановок: {0}", countSwap);
            Console.WriteLine("Отсортированный массив: ");
            Show();

            array2.BinaryTreeSort(out countSravn, out countSwap);
            Console.WriteLine("\n\nСортировка с помощью двоичного дерева");
            Console.WriteLine("Кол-во сравнений: {0}", countSravn);
            Console.WriteLine("Кол-во перестановок: {0}", countSwap);
            Console.WriteLine("Отсортированный массив: ");
            array2.Show();

            Console.WriteLine("\n\nДля продолжения нажмите на любую клавишу...");
            Console.ReadKey(true);
        }

        public void RandomSort() //Сортировка раномного массива
        {
            var array2 = CreateArrayRandom();
            Console.WriteLine("Массив до сортировкм:");
            Show();

            BucketSort(out var countSravn, out var countSwap);
            Console.WriteLine("\n\nБлочная сортировка:");
            Console.WriteLine("Кол-во сравнений: {0}", countSravn);
            Console.WriteLine("Кол-во перестановок: {0}", countSwap);
            Console.WriteLine("Отсортированный массив: ");
            Show();

            array2.BinaryTreeSort(out countSravn, out countSwap);
            Console.WriteLine("\n\nСортировка с помощью двоичного дерева");
            Console.WriteLine("Кол-во сравнений: {0}", countSravn);
            Console.WriteLine("Кол-во перестановок: {0}", countSwap);
            Console.WriteLine("Отсортированный массив: ");
            array2.Show();

            Console.WriteLine("\n\nДля продолжения нажмите на любую клавишу...");
            Console.ReadKey(true);
        }

        public void Show() // Вывод массива
        {
            foreach (var element in arr) Console.Write(element + " ");
        }
    }

    class Program
    {
        public static int Menu(string headLine, params string[] paragraphs) // Наикрасивейшее меню
        {
            Console.Clear();
            Console.Write(headLine);
            int paragraph = 0, x = 2, y = 5, oldParagraph = 0;
            Console.CursorVisible = false;
            for (int i = 0; i < paragraphs.Length; i++)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(x, y + i);
                Console.Write(paragraphs[i]);
            }
            bool choice = false;
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(x, y + oldParagraph);
                Console.Write(paragraphs[oldParagraph] + " ");

                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.White;
                Console.SetCursorPosition(x, y + paragraph);
                Console.Write(paragraphs[paragraph]);

                oldParagraph = paragraph;

                var key = Console.ReadKey();

                switch (key.Key)
                {
                    case ConsoleKey.DownArrow:
                        paragraph++;
                        break;
                    case ConsoleKey.UpArrow:
                        paragraph--;
                        break;
                    case ConsoleKey.Enter:
                        choice = true;
                        break;
                }
                if (paragraph >= paragraphs.Length)
                    paragraph = 0;
                else if (paragraph < 0)
                    paragraph = paragraphs.Length - 1;
                if (choice)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.CursorVisible = true;
                    Console.Clear();
                    break;
                }
            }
            Console.Clear();
            Console.CursorVisible = true;
            return paragraph;
        }

        private static int Input(string task)
        //Ввод целых чисел
        {
            int number;
            bool ok = false;
            do
            {
                Console.WriteLine(task);
                ok = int.TryParse(Console.ReadLine(), out number);
                if (!ok)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ввод неправильный, нужно ввести целое число. Повторите попытку:\n");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            } while (!ok);
            return number;
        }

        //Ввод числа в гранях
        private static int ReadVGran(int min, int max, string task, string name = null)
        {
            int chislo;
            do
            {
                chislo = Input(task);
                if (chislo <= min || chislo >= max)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ошибка! " + name + " должен(-но) быть больше, чем {0} и меньше, чем {1}. Попробуйте ещё раз:\n", min, max);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            } while (chislo <= min || chislo >= max);
            return chislo;
        }

        private static void CreateArray(ref MyArray array)
        {
            while (true)
            {
                var size = ReadVGran(0, 101, "Введите размер массива:", "Размер массива");
                if (size == 0)
                {
                    Console.WriteLine("Размер массива не может быть равен 0! Повторите ввод...");
                }
                else
                {
                    array = new MyArray(size);
                    break;
                }
            }
        }

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            string[] mainMenu = {
                "Создать новый массив", "Отсортировать массив, упорядоченный по возрастанию",
                "Отсортировать массив, упорядоченный по убыванию",
                "Отсортировать неупорядоченный массив",
                "Выход"};
            MyArray array = new MyArray();
            CreateArray(ref array);
            while (true)
            {
                var sw = Menu("Реализация сортировок массива\nБлочная сортировка и сортировка с помощью двоичного дерева", mainMenu);
                switch (sw)
                {
                    case 0:
                        CreateArray(ref array);
                        break;
                    case 1:
                        array.IncreaseSort();
                        break;
                    case 2:
                        array.DecreaseSort();
                        break;
                    case 3:
                        array.RandomSort();
                        break;
                    case 4:
                        return;
                }
            }
        }
    }
}
