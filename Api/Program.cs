using System.Text;
using System.Text.Json;

public class Program
{
    public static async Task Main()
    {
        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("https://plataforma.astenassinatura.com.br/api/");

        string filePath = "C:\\Users\\Raphael\\source\\repos\\Api\\Api\\teste.txt";
        byte[] fileBytes = File.ReadAllBytes(filePath);
        string base64Content = Convert.ToBase64String(fileBytes);

        //Inserir Envelope
        var requisicaoInserir = new
        {
            token = "",
            @params = new
                   {
                       Envelope = new
                       {
                           descricao = "Novo Envelope!",
                           Repositorio = new
                           {
                               id = 40
                           },
                           listaDocumentos = new[]
                           {
                               new
                               {
                                   nomeArquivo = "teste.txt",
                                   conteudo = base64Content
                               }
                           }
                       }
                   }
        };

        var jsonInserir = JsonSerializer.Serialize(requisicaoInserir);

        var contentInserir = new StringContent(jsonInserir, Encoding.UTF8, "application/json");

        var responseInserir = await httpClient.PostAsync("inserirEnvelope", contentInserir);

        var responseContentInserir = await responseInserir.Content.ReadAsStringAsync();
        Console.WriteLine(responseContentInserir);

        //Inserir Signatário Envelope
        var requisicaoSignatario = new
        {
            token = "",
            @params = new
            {
                SignatarioEnvelope = new
                {
                    Envelope = new
                    {
                        id = 1966
                    },
                    ordem = 1,
                    ConfigAssinatura = new
                    {
                        emailSignatario = "usuario2@avmb.com.br",
                        nomeSignatario = "Nome do usuario"
                    }
                }
            }
        };

        var jsonSignatario = JsonSerializer.Serialize(requisicaoSignatario);    

        var contentSignatario = new StringContent(jsonSignatario, Encoding.UTF8, "application/json");

        var responseSignatario = await httpClient.PostAsync("inserirSignatarioEnvelope", contentSignatario);

        var responseContentSignatario = await responseSignatario.Content.ReadAsStringAsync();
        Console.WriteLine(responseContentSignatario);

        //Encaminhar Envelope para Assinatura
        var requisicaoEncaminhar = new
        {
            token = "",
            @params = new
            {
                Envelope = new
                {
                    id = 1948
                },
                dataEnvioAgendado = DateTime.Now.ToString("yyyy-MM-dd"),
                horaEnvioAgendado = DateTime.Now.ToString("hh:mm:ss")
            }
        };

        var jsonEncaminhar = JsonSerializer.Serialize(requisicaoEncaminhar);

        var contentEncaminhar = new StringContent(jsonEncaminhar, Encoding.UTF8, "application/json");

        var responseEncaminhar = await httpClient.PostAsync("encaminharEnvelopeParaAssinaturas", contentEncaminhar);

        var responseContentEncaminhar = await responseEncaminhar.Content.ReadAsStringAsync();
        Console.WriteLine(responseContentEncaminhar);
    }
}