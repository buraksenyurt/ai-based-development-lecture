namespace ExcpHandling;
public class Program
{
    public static void Main()
    {
        try
        {
            var result = Divide(10, 2);
            Console.WriteLine($"10 / 2 = {result}");


            result = Divide(5, 0);
            Console.WriteLine($"5 /0 = {result}");
            Console.WriteLine("Devam eden kodlar");
        }
        catch (DivideByZeroException excp)
        {
            Console.WriteLine("Bir hata oluştu. {0}", excp.Message);
        }
        catch (Exception excp)
        {
            Console.WriteLine(excp.Message);
        }
        finally
        {
            Console.WriteLine("Try bloğunda hata olsa da olmasa da finally bloğu çalışır.\nGenelde kaynak iadelerinde kullanılır.");
        }

        // var lines = File.ReadAllLines("ThereIsNoFile.txt");
        var content = ReadFileContent("ThereIsNoFile.txt");
        if (content is null)
        {
            Console.WriteLine("Dosya içeriği okunamadı");
        }

        var result2 = Divide(Math.PI, 1.2);
        Console.WriteLine($"3.14 / 1.2 = {result2}");
    }
    public static double Divide(double x, double y)
    {
        if (y == 0) throw new DivideByZeroException("Sıfıra bölme hatası");
        return x / y;
    }
    public static string ReadFileContent(string path)
    {
        if (!File.Exists(path)) return null;

        return File.ReadAllText(path);
    }
}
