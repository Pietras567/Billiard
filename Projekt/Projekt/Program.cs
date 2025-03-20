using System;

namespace SimpleProgram {
    public class SimpleCalculator {
        public int Add(int a, int b) {
            return a + b;
        }
    }

    class Program {
        static void Main(string[] args) {
            SimpleCalculator calculator = new SimpleCalculator();

            // Test programu
            int result = calculator.Add(5, 3);
            Console.WriteLine("Wynik dodawania: " + result);
        }
    }
}
