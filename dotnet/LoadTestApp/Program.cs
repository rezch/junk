// lab6_prev

using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.Text;

class Program
{
    private static ConcurrentBag<long> responseTimes = new ConcurrentBag<long>();
    private static int successCount = 0;
    private static int failureCount = 0;

    static async Task Main(string[] args)
    {
        int concurrentUsers = 1;           // число параллельных "пользователей"
        int requestsPerUser = 1;            // сколько запросов каждый пользователь делает
        string url = "http://localhost:5053/api/CreateProduct";  // адрес API

        Console.WriteLine($"Запуск нагрузки: {concurrentUsers} пользователей, по {requestsPerUser} запросов каждый.");
        // Создаём HttpClient, желательно один экземпляр на приложение
        using HttpClient client = new HttpClient();

        // Создаём задачи (Tasks) для параллельного запуска
        Task[] tasks = new Task[concurrentUsers];
        for (int i = 0; i < concurrentUsers; i++)
        {
            tasks[i] = RunUserScenario(client, url, requestsPerUser, i);
        }

        // Ждём окончания всех задач
        await Task.WhenAll(tasks);
        // Выводим результаты
        PrintStatistics();
    }

    private static void CheckRequest(bool result)
    {
        if (result)
        {
            Interlocked.Increment(ref successCount);
        }
        else
        {
            Interlocked.Increment(ref failureCount);
        }
    }

    private static async Task RunUserScenario(HttpClient client, string url, int requestsCount, int selfId)
    {
        for (int i = 0; i < requestsCount; i++)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                // POST
                var product = new
                {
                    name = "test product " + selfId,
                    price = 123123 + (selfId * 10)
                };
                var content = new StringContent(
                    JsonSerializer.Serialize(product), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);
                CheckRequest(response.IsSuccessStatusCode);
                string newId = await response.Content.ReadAsStringAsync();
                string urlWithId = url + "/" + newId;

                // PUT
                var productPut = new
                {
                    name = product.name,
                    price = product.price * 2
                };
                content = new StringContent(
                    JsonSerializer.Serialize(productPut), Encoding.UTF8, "application/json");
                response = await client.PutAsync(urlWithId, content);
                CheckRequest(response.IsSuccessStatusCode);

                // GET
                response = await client.GetAsync(urlWithId);
                CheckRequest(response.IsSuccessStatusCode);
                string json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);
                CheckRequest(
                    doc.RootElement.GetProperty("product").GetProperty("name").GetString() == productPut.name
                );
                CheckRequest(
                    doc.RootElement.GetProperty("product").GetProperty("price").GetInt32() == productPut.price
                );

                // DELETE
                response = await client.DeleteAsync(url + "/id?Id=" + newId);
                CheckRequest(response.IsSuccessStatusCode);

                sw.Stop();
                responseTimes.Add(sw.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                sw.Stop();
                Interlocked.Increment(ref failureCount);
                Console.WriteLine($"Ошибка запроса: {ex.Message}");
            }
            // Симуляция задержки в действиях пользователя
            await Task.Delay(10);
        }
    }

    private static void PrintStatistics()
    {
        int totalRequests = successCount + failureCount;
        Console.WriteLine("Тест завершён.");
        Console.WriteLine($"Всего запросов: {totalRequests}");
        Console.WriteLine($"Успешных: {successCount}");
        Console.WriteLine($"Ошибок: {failureCount}");

        if (responseTimes.Count > 0)
        {
            var timesArray = responseTimes.ToArray();
            Array.Sort(timesArray);
            long sum = 0;
            foreach (var t in timesArray) sum += t;
            double avg = sum / (double)timesArray.Length;

            Console.WriteLine($"Среднее время отклика: {avg:F2} ms");
            Console.WriteLine($"Минимальное время отклика: {timesArray[0]} ms");
            Console.WriteLine($"Максимальное время отклика: {timesArray[^1]} ms");
            Console.WriteLine($"Медианное время отклика: {CalculateMedian(timesArray)} ms");
        }
    }

    private static double CalculateMedian(long[] sortedTimes)
    {
        int n = sortedTimes.Length;
        if (n % 2 == 0)
            return (sortedTimes[n / 2 - 1] + sortedTimes[n / 2]) / 2.0;
        else
            return sortedTimes[n / 2];
    }
}
