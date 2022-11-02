using NUnit.Framework;
using static NUnit.Framework.Assert;
using static Task2.Task2;

namespace Task2;

public class Tests
{
    [Test]
    public void AvailableFunctionsAmountTest()
    {
        That(AvailableFunctions, Has.Count.GreaterThanOrEqualTo(10));
    }

    [Test]
    public void TabulateTest()
    {
        var funNames = new List<string> { "square", "exp" };
        var nOfPoints = 10;
        var res = tabulate(new InputData(0.0, 10.0, nOfPoints, funNames));
        var lines = res.ToString().Split('\n');

        That(lines, Has.Length.EqualTo(nOfPoints + 1));
        foreach (var line in lines)
        {
            foreach (var t in line.Trim('\n').Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                That(line.Trim('\n').Split(' ', StringSplitOptions.RemoveEmptyEntries), Has.Length.EqualTo(funNames.Count + 1));
            }
        }
    }
}