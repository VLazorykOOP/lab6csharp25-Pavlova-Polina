using System;
using System.Collections;
using System.Collections.Generic;

namespace Lab6CSharp
{
    // Користувацькі інтерфейси
    public interface IIdentifiable
    {
        string Number { get; set; }
    }

    public interface ICalculatable
    {
        decimal CalculateTotal();
    }

    public interface IShowable
    {
        void Show();
    }

    // Базовий клас Документ
    public class Document : IIdentifiable, IShowable, ICalculatable, ICloneable
    {
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        public Document(string number, DateTime date, string description)
        {
            Number = number;
            Date = date;
            Description = description;
        }

        public virtual decimal CalculateTotal() => 0;

        public virtual void Show()
        {
            Console.WriteLine($"[Документ] Номер: {Number}, Дата: {Date.ToShortDateString()}");
            Console.WriteLine($"Опис: {Description}");
        }

        public virtual object Clone() => new Document(Number, Date, Description);
    }

    // Похідний клас - Квитанція
    public class Receipt : Document
    {
        public string PersonName { get; set; }
        public decimal Amount { get; set; }

        public Receipt(string number, DateTime date, string description, string personName, decimal amount)
            : base(number, date, description)
        {
            PersonName = personName;
            Amount = amount;
        }

        public override decimal CalculateTotal() => Amount;

        public override void Show()
        {
            base.Show();
            Console.WriteLine($"[Квитанція] Особа: {PersonName}, Сума: {Amount:C}");
        }

        public override object Clone() => new Receipt(Number, Date, Description, PersonName, Amount);
    }

    // Клас для елементів накладної
    public class InvoiceItem
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public InvoiceItem(string name, decimal price, int quantity)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
        }
    }

    // Похідний клас - Накладна
    public class Invoice : Document, IEnumerable<InvoiceItem>
    {
        public string Supplier { get; set; }
        public string Receiver { get; set; }
        private List<InvoiceItem> items = new List<InvoiceItem>();

        public Invoice(string number, DateTime date, string description, string supplier, string receiver)
            : base(number, date, description)
        {
            Supplier = supplier;
            Receiver = receiver;
        }

        public void AddItem(string name, decimal price, int quantity)
        {
            items.Add(new InvoiceItem(name, price, quantity));
        }

        public override decimal CalculateTotal()
        {
            decimal total = 0;
            foreach (var item in items)
            {
                total += item.Price * item.Quantity;
            }
            return total;
        }

        public override void Show()
        {
            base.Show();
            Console.WriteLine($"[Накладна] Постачальник: {Supplier}, Отримувач: {Receiver}");
            Console.WriteLine("Список товарів:");
            foreach (var item in items)
            {
                Console.WriteLine($"  - {item.Name}: {item.Price:C} x {item.Quantity}");
            }
        }

        public IEnumerator<InvoiceItem> GetEnumerator() => items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override object Clone()
        {
            var invoice = new Invoice(Number, Date, Description, Supplier, Receiver);
            foreach (var item in items)
            {
                invoice.AddItem(item.Name, item.Price, item.Quantity);
            }
            return invoice;
        }
    }

    // Похідний клас - Рахунок
    public class Bill : Document, IFormattable
    {
        public string ClientName { get; set; }
        public string ServiceDescription { get; set; }
        public decimal ServiceAmount { get; set; }
        public decimal TaxRate { get; set; }

        public Bill(string number, DateTime date, string description, string clientName,
                   string serviceDescription, decimal serviceAmount, decimal taxRate)
            : base(number, date, description)
        {
            ClientName = clientName;
            ServiceDescription = serviceDescription;
            ServiceAmount = serviceAmount;
            TaxRate = taxRate;
        }

        public override decimal CalculateTotal() => ServiceAmount + (ServiceAmount * TaxRate / 100);

        public override void Show()
        {
            base.Show();
            Console.WriteLine($"[Рахунок] Клієнт: {ClientName}");
            Console.WriteLine($"Послуга: {ServiceDescription}, Сума: {ServiceAmount:C}");
            Console.WriteLine($"Податок: {TaxRate}%, Загалом: {CalculateTotal():C}");
        }

        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            return $"Рахунок №{Number}, Клієнт: {ClientName}, Сума: {CalculateTotal():C}";
        }

        public override object Clone() =>
            new Bill(Number, Date, Description, ClientName, ServiceDescription, ServiceAmount, TaxRate);
    }

    // Тестовий клас для завдання 1
    public class DocumentTesting
    {
        public static void RunTask1()
        {
            Console.WriteLine("=== Завдання 1: Ієрархія класів з інтерфейсами ===");

            // Створення документів різних типів
            var receipt = new Receipt("R-001", DateTime.Now, "Оплата послуг", "Іванов І.І.", 1500.50m);

            var invoice = new Invoice("I-001", DateTime.Now, "Поставка товарів", "ТОВ Постач", "ТОВ Замовник");
            invoice.AddItem("Товар 1", 100, 5);
            invoice.AddItem("Товар 2", 200, 2);

            var bill = new Bill("B-001", DateTime.Now, "Послуги консультування",
                             "Петров П.П.", "Консультація з програмування", 3000, 20);

            // Виведення інформації
            Console.WriteLine("\n--- Документи ---");
            receipt.Show();
            Console.WriteLine();

            invoice.Show();
            Console.WriteLine();

            bill.Show();
            Console.WriteLine();

            // Демонстрація інтерфейсів
            Console.WriteLine("--- Використання інтерфейсів ---");

            // ICalculatable
            Console.WriteLine("\n• Розрахунок через ICalculatable:");
            ICalculatable[] calculatables = { receipt, invoice, bill };
            foreach (var calc in calculatables)
            {
                Console.WriteLine($"Сума: {calc.CalculateTotal():C}");
            }

            // ICloneable
            Console.WriteLine("\n• Клонування через ICloneable:");
            Receipt clonedReceipt = (Receipt)receipt.Clone();
            clonedReceipt.PersonName = "Клонований клієнт";
            Console.WriteLine("Оригінал:");
            receipt.Show();
            Console.WriteLine("\nКлон:");
            clonedReceipt.Show();

            // IEnumerable
            Console.WriteLine("\n• Перебір елементів через IEnumerable:");
            Console.WriteLine($"Елементи накладної {invoice.Number}:");
            foreach (var item in invoice)
            {
                Console.WriteLine($"  {item.Name} - {item.Price:C} x {item.Quantity}");
            }

            // IFormattable
            Console.WriteLine("\n• Форматування через IFormattable:");
            Console.WriteLine(((IFormattable)bill).ToString("G", null));
        }
    }
}