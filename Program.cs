using System;
using Instagram;
using Instagram.Controllers.Media;
using Instagram.Controllers.Login;
using System.Threading.Tasks;
using System.IO;

namespace InstaComment
{
    class Program
    {
        static async Task Main()
        {
            try
            {
                Console.WriteLine("Bot de Comentarios");
                Console.WriteLine("Creado por Dias para Jonn ♥");
                Console.WriteLine();
                Console.WriteLine();
                var dir = Directory.GetCurrentDirectory();
                if (!File.Exists($@"{dir}/Config/user.txt") && !File.Exists($@"{dir}/Config/useragent.txt") && !File.Exists($@"{dir}/Config/comentarios.txt") && !File.Exists($@"{dir}/Config/shortcode.txt"))
                {
                    Console.WriteLine("Erro na configuração !");
                    await Task.Delay(TimeSpan.FromSeconds(50));
                    return;
                }
                string[] comments = File.ReadAllLines($@"{dir}/Config/comentarios.txt");
                string useragent = File.ReadAllText($@"{dir}/Config/useragent.txt");
                string[] userData = File.ReadAllLines($@"{dir}/Config/user.txt");
                string shortcode = File.ReadAllText($@"{dir}/Config/shortcode.txt");
                if (File.Exists($@"{dir}/Cookie/cookie.arka") && File.Exists($@"{dir}/Cookie/claim.arka"))
                {
                    string cookie = File.ReadAllText($@"{dir}/Cookie/cookie.arka");
                    string claim = File.ReadAllText($@"{dir}/Cookie/claim.arka");
                    Insta i = new(userData[0], userData[1], cookie, claim);
                    bool rodar = true;
                    string comentario;
                    Random rand = new Random();
                    while (rodar)
                    {
                        comentario = comments[rand.Next(0, comments.Length)];
                        Console.WriteLine();
                        Console.WriteLine("Comentario : " + comentario);
                        var comment = await i.CommentMediaByShortCode(shortcode, comentario);
                        if (comment.Status == 1)
                        {
                            Console.WriteLine("Sucesso ao comentar");
                        } else
                        {
                            Console.WriteLine("Erro ao comentar, motivo: " + comment.Response);
                        }
                        int segundos = rand.Next(30, 180);
                        Console.WriteLine($"Aguardando {segundos} segundos para o próximo comentario.");
                        await Task.Delay(TimeSpan.FromSeconds(segundos));
                        Console.WriteLine();
                        Console.WriteLine();
                    }
                }
                else
                {
                    Insta i = new(userData[0], userData[1]);
                    var ret = await i.ILogin();
                    if (ret.Status == 1)
                    {
                        string cookies = i.CookieString();
                        string claim = i.GetClaim();
                        if (!Directory.Exists($@"{dir}/Cookie"))
                            Directory.CreateDirectory($@"{dir}/Cookie");
                        if (!File.Exists($@"{dir}/Cookie/cookie.arka"))
                            File.WriteAllText($@"{dir}/Cookie/cookie.arka", cookies);
                        else
                        {
                            File.Delete($@"{dir}/Cookie/cookie.arka");
                            File.WriteAllText($@"{dir}/Cookie/cookie.arka", cookies);
                        }
                        if (!File.Exists($@"{dir}/Cookie/claim.arka"))
                            File.WriteAllText($@"{dir}/Cookie/claim.arka", claim);
                        else
                        {
                            File.Delete($@"{dir}/Cookie/claim.arka");
                            File.WriteAllText($@"{dir}/Cookie/claim.arka", claim);
                        }
                        Console.WriteLine(ret.Response);
                        bool rodar = true;
                        string comentario;
                        Random rand = new Random();
                        while (rodar)
                        {
                            comentario = comments[rand.Next(0, comments.Length)];
                            Console.WriteLine();
                            Console.Write("Comentario : " + comentario);
                            var comment = await i.CommentMediaByShortCode(shortcode, comentario);
                            if (comment.Status == 1)
                            {
                                Console.WriteLine("Sucesso ao comentar");
                            }
                            else
                            {
                                Console.WriteLine("Erro ao comentar, motivo: " + comment.Response);
                            }
                            int segundos = rand.Next(30, 180);
                            Console.WriteLine($"Aguardando {segundos} segundos para o próximo comentario.");
                            await Task.Delay(TimeSpan.FromSeconds(segundos));
                            Console.WriteLine();
                            Console.WriteLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine(ret.Response);
                    }
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
            await Task.Delay(TimeSpan.FromHours(150));
        }
    }
}
