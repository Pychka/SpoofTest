using DataTransferObjects;
using SpoofTest.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SpoofTest.Helpers;

internal class Networker
{
    HttpClientHandler handler = new()
    {
        ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
    };
    HttpClient client;
    public Networker()
    {
        client = new(handler);
    }
    private readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    };
    int count = 0;
    public T? Desirialize<T>(string content) where T : class
    {
        try
        {
            if (content == "") return null;
            return JsonSerializer.Deserialize<T>(content, Options);
        }
        catch
        {
            return null;
        }
    }

    public string Serialize<T>(T entity) where T : class
    {
        return JsonSerializer.Serialize(entity, Options);
    }

    public async Task<Test?> GetWorkAsync(string key)
    {
        try
        {
            if(count < 5)
            {
                var response = await client.GetAsync($"https://192.168.2.97:7007/api/Test?id={key}");
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var test = Desirialize<DataTransferObjects.Server.TestDTOAnswer>(await response.Content.ReadAsStringAsync());
                    return new()
                    {
                        Id = test.Id,
                        Title = test.Title,
                        Limit = TimeSpan.FromMinutes(test.LimitMinutes),
                        Questions = [..test.Questions.Select(x => new Question()
                        {
                            Title = x.Title,
                            Id = x.Id,
                            Answers = [..x.Answers.Select(x => new Answer()
                            {
                                Id = x.Id,
                                Title = x.Title,
                            })]
                        })]
                    };
                }
                count = 0;
            }
            return null;
        }
        catch
        {
            count++;
            return await GetWorkAsync(key);
        }
    }

    public async Task<string> Send(Test test)
    {
        try
        {
            if(count < 5)
            {
                TestDTO result = ConvertTest(test);
                var answer = await client.PostAsync("https://192.168.2.97:7007/api/Test", new StringContent(Serialize(result), Encoding.UTF8, "application/json"));
                count = 0;
                return answer.StatusCode == System.Net.HttpStatusCode.OK ? await answer.Content.ReadAsStringAsync() : "Сервер не отвечает";
            }
            return "Сервер не отвечает";
        }
        catch
        {
            count++;
            return await Send(test);
        }
    }
    private static TestDTO ConvertTest(Test test)
    {
        return new()
        {
            Group = test.Group,
            Name = test.Name,
            LastName = test.LastName,
            Patronymic = test.Patronymic,
            SessionId = test.SessionId,
            TestId = test.Id,
            Questions = [.. test.Questions.Select(
                x => new QuestionDTO()
                {
                    AnswerId = x.UserAnswer?.Id ?? 0,
                    QuestionId = x.Id
                })]
        };
    }
}
