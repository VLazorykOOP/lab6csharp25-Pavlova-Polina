using System;
using System.Collections.Generic;

namespace Lab6CSharp
{
    // Інтерфейс, що успадковує інтерфейси .NET
    public interface ISoftware : IComparable<ISoftware>, ICloneable
    {
        string Name { get; }
        void ShowInfo();
        bool CanUse(DateTime date);
    }

    // Клас вільного ПЗ
    public class FreeSoftware : ISoftware
    {
        public string Name { get; }
        public string Vendor { get; }

        public FreeSoftware(string name, string vendor)
        {
            Name = name;
            Vendor = vendor;
        }

        public void ShowInfo()
        {
            Console.WriteLine($"[Вільне ПЗ] {Name}, Виробник: {Vendor}");
        }

        public bool CanUse(DateTime date) => true;

        public int CompareTo(ISoftware? other) =>
            string.Compare(Name, other?.Name, StringComparison.Ordinal);

        public object Clone() => new FreeSoftware(Name, Vendor);
    }

    // Клас умовно-безкоштовного ПЗ
    public class SharewareSoftware : ISoftware
    {
        public string Name { get; }
        public string Vendor { get; }
        public DateTime InstallDate { get; }
        public int TrialPeriodDays { get; }

        public SharewareSoftware(string name, string vendor, DateTime installDate, int trialPeriodDays)
        {
            Name = name;
            Vendor = vendor;
            InstallDate = installDate;
            TrialPeriodDays = trialPeriodDays;
        }

        public void ShowInfo()
        {
            Console.WriteLine($"[Умовно-безкоштовне ПЗ] {Name}, Виробник: {Vendor}");
            Console.WriteLine($"Дата встановлення: {InstallDate.ToShortDateString()}, Термін: {TrialPeriodDays} днів");
        }

        public bool CanUse(DateTime date) => (date - InstallDate).TotalDays <= TrialPeriodDays;

        public int CompareTo(ISoftware? other) =>
            other is SharewareSoftware shareware
                ? TrialPeriodDays.CompareTo(shareware.TrialPeriodDays)
                : 0;

        public object Clone() => new SharewareSoftware(Name, Vendor, InstallDate, TrialPeriodDays);
    }

    // Клас комерційного ПЗ
    public class CommercialSoftware : ISoftware
    {
        public string Name { get; }
        public string Vendor { get; }
        public decimal Price { get; }
        public DateTime InstallDate { get; }
        public int LicensePeriodDays { get; }

        public CommercialSoftware(string name, string vendor, decimal price, DateTime installDate, int licensePeriodDays)
        {
            Name = name;
            Vendor = vendor;
            Price = price;
            InstallDate = installDate;
            LicensePeriodDays = licensePeriodDays;
        }

        public void ShowInfo()
        {
            Console.WriteLine($"[Комерційне ПЗ] {Name}, Виробник: {Vendor}");
            Console.WriteLine($"Ціна: {Price:C}, Встановлено: {InstallDate.ToShortDateString()}, Ліцензія: {LicensePeriodDays} днів");
        }

        public bool CanUse(DateTime date) => (date - InstallDate).TotalDays <= LicensePeriodDays;

        public int CompareTo(ISoftware? other) =>
            other is CommercialSoftware commercial
                ? Price.CompareTo(commercial.Price)
                : 0;

        public object Clone() => new CommercialSoftware(Name, Vendor, Price, InstallDate, LicensePeriodDays);
    }

    // Клас для тестування завдання 2
    public class SoftwareTesting
    {
        public static void RunTask2()
        {
            Console.WriteLine("=== Завдання 2: Програмне забезпечення ===");

            ISoftware[] softwareList = {
                new FreeSoftware("Linux", "Linux Foundation"),
                new SharewareSoftware("WinRAR", "RARLAB", DateTime.Now.AddDays(-40), 30),
                new CommercialSoftware("Windows", "Microsoft", 199.99m, DateTime.Now.AddDays(-10), 365)
            };

            Console.WriteLine("\n--- Всі програми ---");
            foreach (var sw in softwareList)
            {
                sw.ShowInfo();
                Console.WriteLine();
            }

            Console.WriteLine("--- Доступні програми на поточну дату ---");
            DateTime today = DateTime.Now;
            foreach (var sw in softwareList)
            {
                if (sw.CanUse(today))
                {
                    sw.ShowInfo();
                    Console.WriteLine();
                }
            }

            // Демонстрація інтерфейсів
            Console.WriteLine("--- Використання інтерфейсів ---");

            // IComparable - сортування програм
            Console.WriteLine("\n• Сортування за допомогою IComparable:");
            Array.Sort(softwareList);
            foreach (var sw in softwareList)
            {
                Console.WriteLine($"{sw.Name}");
            }

            // ICloneable - клонування програм
            Console.WriteLine("\n• Клонування за допомогою ICloneable:");
            CommercialSoftware original = new CommercialSoftware("Photoshop", "Adobe", 299.99m, DateTime.Now, 365);
            CommercialSoftware clone = (CommercialSoftware)original.Clone();
            Console.WriteLine("Оригінал:");
            original.ShowInfo();
            Console.WriteLine("\nКлон:");
            clone.ShowInfo();
        }
    }
}