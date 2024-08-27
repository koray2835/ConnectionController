using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {

        string host;
        string hostinput; 
        int durationInMinutes;
        int intervalInSeconds;
        float totalPings = 0;
        float failedPings = 0;

        Console.WriteLine("Default Host: google.com\nEnter ile devam edebilir veya değiştirmek isterseniz host girebilirsiniz.");
        Console.Write("Host girin: ");
        hostinput = Console.ReadLine();

        if (hostinput == null || hostinput=="") {
            host = "google.com";
        }
        else {
            host = hostinput; 
        }


        while (true)
        {
            Console.Write("Test süresi girin (dk): ");
            string durationInput = Console.ReadLine();

            if (int.TryParse(durationInput, out durationInMinutes) && durationInMinutes > 0)
            {
                break;
            }
            else
            {
                Console.WriteLine("Geçerli bir pozitif tam sayı girin.");
            }
        }


        while (true)
        {
            Console.Write("Ping atma aralığı girin (sn): ");
            string intervalInput = Console.ReadLine();

            if (int.TryParse(intervalInput, out intervalInSeconds) && intervalInSeconds > 0)
            {
                break;
            }
            else
            {
                Console.WriteLine("Geçerli bir pozitif tam sayı girin.");
            }
        }

        Console.WriteLine($"Bağlantı kontrolü başlatılıyor.\n");
        DateTime endTime = DateTime.Now.AddMinutes(durationInMinutes);

        while (DateTime.Now < endTime)
        {
            bool isSuccessful = await PingHostAsync(host);
            if (!isSuccessful)
            {
                failedPings++;
            }
            totalPings++;
            await Task.Delay(intervalInSeconds * 1000);
        }

        Console.WriteLine("\nBağlantı kontrolü tamamlandı.");
        Console.WriteLine($"Toplam atılan ping sayısı: {totalPings}");
        Console.WriteLine($"Başarısız olan ping sayısı: {failedPings}");

        float successPercent = ((totalPings - failedPings) / totalPings) *100;

        Console.WriteLine($"Başarı oranı: %{successPercent}");
        Console.ReadKey();
    }

    static async Task<bool> PingHostAsync(string host)
    {
        try
        {
            using Ping ping = new();
            PingReply reply = await ping.SendPingAsync(host);

            if (reply.Status == IPStatus.Success)
            {
                Console.WriteLine($"Ping başarılı: {reply.RoundtripTime} ms");
                return true;
            }
            else
            {
                Console.WriteLine($"Ping başarısız: {reply.Status}");
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ping hatası: {ex.Message}");
            return false;
        }
    }
}
