using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Teixeira.ArquivosHelper.SeparadorArquivos;

namespace Teixeira.ArquivosHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("ArquivosHelper.SeparadorArquivo");

            var marcadores = GetMarcadores();

            //var separadorArquivo = new SeparadorArquivo(marcadores, @"C:\Users\teixe\OneDrive\Área de Trabalho\ARQUIVO_INTEIRO.rpt");
            var separadorArquivo = new SeparadorArquivo("ConfiguracaoMarcadores.txt", @"C:\Users\teixe\OneDrive\Área de Trabalho\ARQUIVO_INTEIRO.rpt");

            separadorArquivo.RealizarSeparacaoDeUmArquivoEmVarios();

            Console.WriteLine("Fim!");

            Console.ReadLine();
        }

        static Dictionary<string, InformacaoArquivo> GetMarcadores()
        {
            var marcadores = new Dictionary<string, InformacaoArquivo>
            {
                { "CPF", new InformacaoArquivo("NOTAS_FISCAIS.rpt") },
                { "NUMERO", new InformacaoArquivo("ITENS_NOTAS_FISCAIS.rpt") }
            };

            return marcadores;
        }

    }
}
