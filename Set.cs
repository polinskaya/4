using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

/*
*Класс  множество Set. Дополнительно перегрузить следующие операции:
* ++-добавление случайного элемента к множесту,
* +  объединение множеств; <=  сравнение множеств;
* неявный int() мощность множества;
* % - доступ к элементу в заданной позиции. 
Методы расширения: 
1) Шифрование строки  2) Проверка на упорядоченность множеств
*
*/
namespace Lab4
{
    public class Set
    {

        public OwnerSet Owner { get; set; }

        public Date CreationDate { get; }

        public List<int> MyList { get; set; } = new List<int>();

        public Set()
        {
            Owner = new OwnerSet(27022000, "Polinskaya YULIA", "BSTU");
            DateTime creationDate = DateTime.Today;//Для работы с датами и временем в .NET предназначена структура DateTime
            CreationDate = new Date(creationDate.Day, creationDate.Month, creationDate.Year);
        }

        public int this[int index]
        {
            get => MyList[index]; //считывает
            set => MyList[index] = value; //устанавливает свойство
        }

        public Set(params int[] args)
            : this()
        {
            MyList.AddRange(args);
        }
        public Set(IEnumerable<int> mas)//передаем в конструктор коллекцию IEnumerable для заполнения множеств
            : this()
        {
            MyList.AddRange(mas);
        }

        public void Add(int elem)//добавдение
        {
            MyList.Add(elem);
        }
        public void Delete(int elem)//удаление
        {
            MyList.Remove(elem);
        }

        public void Push(int element) => this.MyList.Add(element);// вставка элемента в начало списка
        public int Count => this.MyList.Count;

        public Set Intersect(Set Source)//пересечение
        {
            return new Set(MyList.Intersect(Source.MyList));
        }
        public Set Union(Set Source)//объединение
        {
            return new Set(MyList.Union(Source.MyList));
        }
        public Set Except(Set Source)//разность
        {
            return new Set(MyList.Except(Source.MyList));
        }
        public override string ToString() //вызов метода  ToString
        {
            return string.Join(",", MyList);
        }

        public string FullInfo()
        {
            return $"Владелец: {Owner.id} {Owner.name} {Owner.organization}"
                   + Environment.NewLine +
                   $"Дата создания: {CreationDate.day} {CreationDate.month} {CreationDate.year}"
                   + Environment.NewLine +
                   "Множество: " +
                   string.Join(",", MyList);
        }

        public class OwnerSet
        {
            public int id;
            public string name;
            public string organization;

            public OwnerSet(int id, string name, string organization) //конструктор с параметрами
            {
                this.id = id;
                this.name = name;
                this.organization = organization;
            }
        }

        public class Date
        {
            public int day;
            public int month;
            public int year;

            public Date(int day, int month, int year)
            {
                this.day = day;
                this.month = month;
                this.year = year;
            }
        }

        public static Set operator ++(Set set)
        {
            //создание нового обекта int и добавление во множество 
            Random rand = new Random();
            set.MyList.Add(rand.Next(0, 99999));

            return set;
        }
        public static int operator %(Set set, int index)//проверка
        {
            if (index < 0)
            {
                return 404;
            }

            if (index > set.Count - 1)
            {
                return 404;
            }

            return set[index];
        }
        public static Set operator +(Set set1, Set set2)
        {
            return set1.Intersect(set2);
        }

        public static bool operator <=(Set set1, Set set2)
        {
            return set1.Count <= set2.Count;
        }

        public static bool operator >=(Set set1, Set set2)
        {
            return set1.Count >= set2.Count;
        }

        //неявное преобразование
        public static implicit operator int(Set set)//мощность множества
        {
            return set.Count;
        } 
    }
    public static class MathOperation
    {
        
        public static int Max(Set set)//макс.
        {
            return set.MyList.Max();
        }

        public static int Min(Set set)//мин.
        {
            return set.MyList.Min();
        }

        public static int Count(Set set)//кол-во эл.
        {
            return set.Count;
        }

        public static bool ValidateSet(this Set set) // сортировка по возрастанию, так как это int то getHashCode вернет значение
        {
            //мы делаем копию массива и сортируем, потом сравниваем элементы оригинала и отсортированной копии если совпадают значит множество упорядочено и возвращаем true
           // если не совпадает хотя бы один элемент то false
            Set tempSet = new Set(set.MyList);
            if (tempSet.Count == 0)
            {
                return false;
            }

            if (tempSet.Count == 1)
            {
                return true;
            }

            IEnumerable<int> sorted = tempSet.MyList.OrderBy(arg => arg.GetHashCode());

            for (var i = 0; i < set.Count; i++)
            {
                if (!sorted.ElementAt(i).Equals(set[i]))
                {
                    return false;
                }
            }
           
            return true;
           
        }
       
        public static string Encrypt(this string str)
        {
            return EncryptionHelper.Encrypt(str);
        }

        public static string Decrypt(this string encryptedString)
        {
            return EncryptionHelper.Decrypt(encryptedString);
        }

        //класс шифрования строки
        private static class EncryptionHelper
        {
            public static string Encrypt(string clearText)
            {
                string EncryptionKey = "abc123";
                byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        clearText = Convert.ToBase64String(ms.ToArray());
                    }
                }
                return clearText;
            }
            public static string Decrypt(string cipherText)
            {
                string EncryptionKey = "abc123";
                cipherText = cipherText.Replace(" ", "+");
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
                return cipherText;
            }
        }
    }
}
