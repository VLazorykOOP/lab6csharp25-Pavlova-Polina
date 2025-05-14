using System;
using System.Collections.Generic;

namespace Lab6CSharp
{
    // Власні класи винятків
    public class InvalidDocumentNumberException : Exception
    {
        public InvalidDocumentNumberException(string message) : base(message) { }
    }

    public class NegativeAmountException : Exception
    {
        public NegativeAmountException(string message) : base(message) { }
    }

    public class DocumentNotFoundException : Exception
    {
        public DocumentNotFoundException(string message) : base(message) { }
    }

    public class InvalidInvoiceItemException : Exception
    {
        public InvalidInvoiceItemException(string message) : base(message) { }
    }

    // Інтерфейс для документів
    public interface IDocument
    {
        string Number { get; }
        void Show();
    }

    // Класи документів з обробкою помилок
    public class ExceptionReceipt : IDocument
    {
        public string Number { get; }
        public decimal Amount { get; }

        public ExceptionReceipt(string number, decimal amount)
        {
            if (string.IsNullOrWhiteSpace(number))
                throw new InvalidDocumentNumberException("Номер квитанції не може бути порожнім");

            if (amount < 0)
                throw new NegativeAmountException($"Сума не може бути від'ємною: {amount}");

            Number = number;
            Amount = amount;
        }

        public void Show()
        {
            Console.WriteLine($"Квитанція №{Number}, сума: {Amount:C}");
        }
    }

    public class ExceptionInvoice : IDocument
    {
        public string Number { get; }
        public List<string> Items { get; } = new List<string>();

        public ExceptionInvoice(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
                throw new InvalidDocumentNumberException("Номер накладної не може бути порожнім");

            Number = number;
        }

        public void AddItem(string item)
        {
            if (string.IsNullOrWhiteSpace(item))
                throw new InvalidInvoiceItemException("Елемент накладної не може бути порожнім");

            Items.Add(item);
        }

        public string GetItem(int index)
        {
            try
            {
                return Items[index];
            }
            catch (IndexOutOfRangeException)
            {
                throw new InvalidInvoiceItemException($"Помилка доступу до елемента накладної. Індекс {index} виходить за межі (0-{Items.Count - 1})");
            }
        }

        public void Show()
        {
            Console.WriteLine($"Накладна №{Number}, кількість елементів: {Items.Count}");
            for (int i = 0; i < Items.Count; i++)
            {
                Console.WriteLine($"  {i + 1}. {Items[i]}");
            }
        }
    }

    // Клас для тестування завдання 3
    public class DocumentExceptionTest
    {
        public static void RunTask3()
        {
            Console.WriteLine("=== Завдання 3: Обробка помилок ===");

            try
            {
                var documents = new List<IDocument>();

                // Додавання правильних документів
                Console.WriteLine("\n== Створення документів ==");
                var receipt = new ExceptionReceipt("R-001", 1500.50m);
                documents.Add(receipt);

                var invoice = new ExceptionInvoice("I-001");
                invoice.AddItem("Товар 1");
                invoice.AddItem("Товар 2");
                documents.Add(invoice);

                // Виведення всіх документів
                Console.WriteLine("\n== Список документів ==");
                PrintDocuments(documents);

                // Тестування різних виняткових ситуацій
                Console.WriteLine("\n== Тестування обробки помилок ==");

                // 1. Порожній номер документа
                try
                {
                    Console.WriteLine("1. Спроба створити квитанцію з порожнім номером:");
                    var badReceipt = new ExceptionReceipt("", 100);
                }
                catch (InvalidDocumentNumberException ex)
                {
                    Console.WriteLine($"[Помилка]: {ex.Message}");
                }

                // 2. Від'ємна сума
                try
                {
                    Console.WriteLine("\n2. Спроба створити квитанцію з від'ємною сумою:");
                    var badReceipt = new ExceptionReceipt("R-002", -50);
                }
                catch (NegativeAmountException ex)
                {
                    Console.WriteLine($"[Помилка]: {ex.Message}");
                }

                // 3. Доступ до елемента за межами діапазону (IndexOutOfRangeException)
                try
                {
                    Console.WriteLine("\n3. Спроба отримати неіснуючий елемент накладної:");
                    string item = invoice.GetItem(10); // Вихід за межі
                }
                catch (InvalidInvoiceItemException ex)
                {
                    Console.WriteLine($"[Помилка]: {ex.Message}");
                }

                // 4. Прямий IndexOutOfRangeException
                try
                {
                    Console.WriteLine("\n4. Спроба отримати документ за неіснуючим індексом:");
                    if (documents.Count > 0)
                    {
                        // Наприклад, спроба доступу до елемента [5] у списку з 2 елементів
                        IDocument doc = documents[5];
                    }
                }
                catch (IndexOutOfRangeException ex)
                {
                    Console.WriteLine($"[Стандартна помилка IndexOutOfRangeException]: {ex.Message}");
                    Console.WriteLine($"Тип винятку: {ex.GetType().Name}");
                }

                // 5. Неправильний елемент накладної
                try
                {
                    Console.WriteLine("\n5. Спроба додати порожній елемент до накладної:");
                    invoice.AddItem("");
                }
                catch (InvalidInvoiceItemException ex)
                {
                    Console.WriteLine($"[Помилка]: {ex.Message}");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Непередбачена помилка]: {ex.Message}");
            }
        }

        private static void PrintDocuments(List<IDocument> documents)
        {
            for (int i = 0; i < documents.Count; i++)
            {
                try
                {
                    Console.Write($"{i + 1}. ");
                    documents[i].Show();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка виведення документа #{i + 1}: {ex.Message}");
                }
            }
        }
    }
}