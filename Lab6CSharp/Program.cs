using System;

namespace Lab6CSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            bool continueRunning = true;
            while (continueRunning)
            {
                Console.Clear(); // Очищаємо консоль перед виведенням меню

                Console.WriteLine("=== ЛАБОРАТОРНА РОБОТА №6 ===");
                Console.WriteLine("Введіть номер завдання:");
                Console.WriteLine("1 - Ієрархія класів з інтерфейсами (документи)");
                Console.WriteLine("2 - Програмне забезпечення");
                Console.WriteLine("3 - Обробка помилок з IndexOutOfRangeException");
                Console.WriteLine("0 - Вихід");
                Console.Write("Ваш вибір: ");

                string input = Console.ReadLine() ?? "0";
                Console.WriteLine();

                switch (input)
                {
                    case "0":
                        continueRunning = false;
                        Console.WriteLine("Програма завершує роботу...");
                        break;
                    case "1":
                        DocumentTesting.RunTask1();
                        break;
                    case "2":
                        SoftwareTesting.RunTask2();
                        break;
                    case "3":
                        DocumentExceptionTest.RunTask3();
                        break;
                    default:
                        Console.WriteLine("Невідомий вибір.");
                        break;
                }

                if (continueRunning) // Додаємо цю перевірку, щоб не показувати запит при виході
                {
                    Console.WriteLine("\nНатисніть будь-яку клавішу, щоб повернутися до меню...");
                    Console.ReadKey();
                }
            }
        }
    }
}