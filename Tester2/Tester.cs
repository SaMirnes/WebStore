namespace Tester2
{
    using System.IO;  // include the System.IO namespace
    using static System.Net.Mime.MediaTypeNames;

    internal class Tester
    {
        public static void Main()
        {
            Console.WriteLine(Enum.Parse(typeof(Enums.Categories), "Computer", true));
            Console.WriteLine(typeof(Enums.Categories));
        }
    }

    public class Enums
    {
        public enum Categories
        {
            Computer,
            TV,
            Vehicle,
            Clothes,
            Other
        }
    }
}
