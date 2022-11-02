using OneVariableFunction = System.Func<double, double>;
using FunctionName = System.String;
using System.Security.Cryptography.X509Certificates;

namespace Task2
{
    public class Task2
    {

        /*
         * В этом задании необходимо написать программу, способную табулировать сразу несколько
         * функций одной вещественной переменной на одном заданном отрезке.
         */


        // Сформируйте набор как минимум из десяти вещественных функций одной переменной
        internal static Dictionary<FunctionName, OneVariableFunction> AvailableFunctions =
                    new Dictionary<FunctionName, OneVariableFunction>
                    {
                { "square", x => x * x },
                { "sin", Math.Sin },
                { "cos", Math.Cos },
                { "tg", Math.Tan },
                { "abs", Math.Abs },
                { "exp", Math.Exp },
                { "arcsin", Math.Asin },
                { "arccos", Math.Acos },
                { "sqrt", x => Math.Pow(x,0.5) },
                { "cube", x => x * x * x },
                    };

        // Тип данных для представления входных данных
        internal record InputData(double FromX, double ToX, int NumberOfPoints, List<string> FunctionNames);

        // Чтение входных данных из параметров командной строки
        private static InputData? prepareData(string[] args)
        {
            double FromX = double.Parse(args[0]);
            double ToX = double.Parse(args[1]);
            int NumberOfPoints = int.Parse(args[2]);
            List<string> FunctionNames = args[3..].ToList();
            return new InputData(FromX, ToX, NumberOfPoints, FunctionNames);
        }

        // Тип данных для представления таблицы значений функций
        // с заголовками столбцов и строками (первый столбец --- значение x,
        // остальные столбцы --- значения функций). Одно из полей --- количество знаков
        // после десятичной точки.
        internal record FunctionTable(int DigitCount, double steps, double start, int count, List<string> FunctionNames)
        {
            // Код, возвращающий строковое представление таблицы (с использованием StringBuilder)
            // Столбец x выравнивается по левому краю, все остальные столбцы по правому.
            // Для форматирования можно использовать функцию String.Format.
            public override string ToString()
            {
                int max_len = 0;
                foreach (var name in FunctionNames)
                    max_len = Math.Max(max_len, name.Length + 3);
                string result = string.Empty;
                result += "x".PadLeft(max_len);
                foreach (var name in FunctionNames)
                    result += $" {name}".PadLeft(max_len);
                for (int i = 0; i < count; ++i)
                {
                    result += '\n';
                    double point = start + steps * i;
                    result += $"{point}".PadLeft(max_len);

                    foreach (var name in FunctionNames)
                    {
                        double f = Math.Round(AvailableFunctions[name](point));
                        result += $" {f}".PadLeft(max_len);
                    }
                }
                return result;
            }
        }

        /*
         * Возвращает таблицу значений заданных функций на заданном отрезке [fromX, toX]
         * с заданным количеством точек.
         */
        internal static FunctionTable tabulate(InputData input)
        {
            double step = (input.FromX + input.ToX) / (input.NumberOfPoints - 1);
            return new FunctionTable(1, step, input.FromX, input.NumberOfPoints, input.FunctionNames);
        }

        public static void Main(string[] args)
        {
            // Входные данные принимаются в аргументах командной строки
            // fromX fromY numberOfPoints function1 function2 function3 ...

            var input = prepareData(args);
            int count = 0;
            foreach (var line in tabulate(input).ToString().Split('\n')) count++;
            // Собственно табулирование и печать результата (что надо поменять в этой строке?):
            Console.WriteLine(tabulate(input));
        }
    }
}