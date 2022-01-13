using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace CandidateTesting.VictorHugoPortoFerreira.NoName
{
    class Program
    {
        static void Main(string[] args)
        {           

            WebClient wc = new WebClient();            
            var diretorio = "C:/CandidateTesting.VictorHugoPortoFerreira.NoName/App_Documents/Uploads/output";
            var caminhoArquivo = diretorio + "/input-01.txt";
            var caminhoArquivoOut = diretorio + "/minhaCdn1.txt";
            string url;
            var HttpMethod = "";
            var UriPath = "";


            File.Delete(caminhoArquivoOut);

            Console.WriteLine("Escreva a URL:");
            url = Console.ReadLine();

            if (!Directory.Exists(diretorio))
                Directory.CreateDirectory(diretorio);            

            wc.DownloadFile(url, caminhoArquivo);            

            List<Campos> dadosLidos = new List<Campos>();
            StreamReader arquivo = new StreamReader(caminhoArquivo);
            StreamWriter novoArquivo = new StreamWriter(caminhoArquivoOut, true, Encoding.ASCII);
            novoArquivo.WriteLine("# Versão: 1.0");
            novoArquivo.WriteLine("# Data: " + DateTime.Now);
            novoArquivo.WriteLine("# Campos: provedor http-method status-code uri-path time-taken response-size cachestatus");
            novoArquivo.WriteLine("");
            string linha = "";
            while (true)
            {
                linha = arquivo.ReadLine();
                if (linha != null)
                {
                    string[] DadosColetados = linha.Split('|');

                    var aux = DadosColetados[3].Split("/");
                    HttpMethod = aux[0].Substring(1);
                    UriPath = aux[1].Substring(0, aux[1].Length - 5);

                    dadosLidos.Add(new Campos
                    {
                        Provedor = "MINHA CDN",
                        HttpMethod = HttpMethod,
                        StatusCode = DadosColetados[1],
                        UriPath = UriPath,
                        TimeTaken = DadosColetados[4].Substring(0, DadosColetados[4].Length - 2),
                        ResponseSize = DadosColetados[0],
                        CacheStatus = DadosColetados[2]
                    });
                }
                else
                    break;
            }
            
            foreach (var linhaLida in dadosLidos)
            {
                novoArquivo.WriteLine(linhaLida.Provedor + " " + linhaLida.HttpMethod + " " + linhaLida.StatusCode + " " + linhaLida.UriPath + " " + linhaLida.TimeTaken + " " + linhaLida.ResponseSize + " " + linhaLida.CacheStatus);
            }
            arquivo.Close();
            novoArquivo.Close();
            File.Delete(caminhoArquivo);

        }
    }
}
