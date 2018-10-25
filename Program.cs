using System;

namespace Lab4
{
    class Program
    {
        static void Main(string[] args)
        {
            Set _mt1 = new Set(1, 2, 3, 4, 5);
            Console.WriteLine("Множество 1: {0}", _mt1);
            Set _mt2 = new Set(4, 5, 6, 7, 8);
            Console.WriteLine("Множество 2: {0}", _mt2);
            Console.WriteLine("Пересечение множеств: {0}", _mt1.Intersect(_mt2));
            _mt2.Add(9);
            Console.WriteLine("Множество 2 после добавления элемента: {0}", _mt2);
            Console.WriteLine("Объединение множеств: {0}", _mt1.Union(_mt2));
            _mt1.Delete(1);
            Console.WriteLine("Множество 1 после удаления элемента: {0}", _mt1);
            Console.WriteLine("Разность множеств: {0}", _mt1.Except(_mt2));

            var some = _mt1 % -1;

            Console.WriteLine("\n Информация: ");

            Console.WriteLine(_mt1.FullInfo());

            _mt1++;
           
            Console.WriteLine(_mt1.FullInfo());

            Console.WriteLine("Максимальный элемент множества: ");
            Console.WriteLine(MathOperation.Max(_mt1));
            Console.WriteLine("Минимальный элемент множества: ");
            Console.WriteLine(MathOperation.Min(_mt1));
            Console.WriteLine("Количество элементов множества: ");
            Console.WriteLine(MathOperation.Count(_mt1));
            Console.WriteLine("Множество упорядоченно: ");
            Console.WriteLine(_mt1.ValidateSet());
            Console.WriteLine("Шифрование: ");
            var str1= "Polinskaya";
            Console.WriteLine(str1);
            var str2 = str1.Encrypt();
            Console.WriteLine(str2);
            var str3 = str2.Decrypt();
            Console.WriteLine(str3);

            Console.ReadKey();
        }
    }
}
