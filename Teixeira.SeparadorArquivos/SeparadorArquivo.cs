using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Teixeira.ArquivosHelper.SeparadorArquivos
{
    public class SeparadorArquivo
    {
        public Dictionary<string, InformacaoArquivo> Marcadores { get; }

        public string NomeArquivoUnicoASerSeparado { get; }

        public SeparadorArquivo(Dictionary<string, InformacaoArquivo> marcadores, string nomeArquivoUnicoASerSeparado)
        {
            if (marcadores == null || marcadores.Count <= 0)
                throw new ArgumentException("Configuração dos marcadores para a separação do arquivo não informados.", nameof(marcadores));

            if (string.IsNullOrWhiteSpace(nomeArquivoUnicoASerSeparado))
                throw new ArgumentException("Nome do arquivo a ser separado não informado.", nameof(nomeArquivoUnicoASerSeparado));

            Marcadores = marcadores;
            NomeArquivoUnicoASerSeparado = nomeArquivoUnicoASerSeparado;
        }

        public SeparadorArquivo(string nomeArquivoConfiguracoesMarcadores, string nomeArquivoUnicoASerSeparado)
        {
            if (string.IsNullOrWhiteSpace(nomeArquivoConfiguracoesMarcadores))
                throw new ArgumentException("Nome do arquivo de configuração dos marcadores não informado.", nameof(nomeArquivoConfiguracoesMarcadores));

            if (string.IsNullOrWhiteSpace(nomeArquivoUnicoASerSeparado))
                throw new ArgumentException("Nome do arquivo a ser separado não informado.", nameof(nomeArquivoUnicoASerSeparado));

            Marcadores = CriarMarcadoresComBaseEmArquivoDeConfiguracao(nomeArquivoConfiguracoesMarcadores);

            if (Marcadores == null || Marcadores.Count <= 0)
                throw new ArgumentException("não foi possível carregar as configuração dos marcadores do arquivo informado.");

            NomeArquivoUnicoASerSeparado = nomeArquivoUnicoASerSeparado;
        }

        public void RealizarSeparacaoDeUmArquivoEmVarios()
        {
            string linha;
            var texto = new StringBuilder();

            if (!File.Exists(NomeArquivoUnicoASerSeparado))
                throw new FileNotFoundException("Arquivo a ser separado não encontrado.", NomeArquivoUnicoASerSeparado);

            using (System.IO.StreamReader arquivo = new System.IO.StreamReader(NomeArquivoUnicoASerSeparado))
            {
                while ((linha = arquivo.ReadLine()) != null)
                {
                    VerificarSeLinhaEhUmNovoArquivo(Marcadores, texto, linha);
                }
            }

            // escreve último arquivo
            GravarArquivo(Marcadores, texto);

        }

        void VerificarSeLinhaEhUmNovoArquivo(Dictionary<string, InformacaoArquivo> marcadores, StringBuilder texto, string linha)
        {
            if (string.IsNullOrWhiteSpace(linha))
                return;

            var primeiraPalavra = linha.Split(' ')[0];
            if (marcadores.ContainsKey(primeiraPalavra))
            {
                // escreve arquivo anterior
                GravarArquivo(marcadores, texto);

                // marcar arquivo atual
                marcadores[primeiraPalavra].Selecionado = true;
            }

            // coloca linha no buffer
            texto.AppendLine(linha);
        }

        void GravarArquivo(Dictionary<string, InformacaoArquivo> marcadores, StringBuilder texto)
        {
            if (texto.Length <= 0)
                return;

            // pega a chave do arquivo selecionado
            var chaveDoUltimoSelecionado = marcadores.FirstOrDefault(x => x.Value.Selecionado == true).Key;

            // escreve arquivo
            var nomeArquivo = marcadores[chaveDoUltimoSelecionado].NomeArquivo;
            File.WriteAllText(nomeArquivo, texto.ToString());
            Console.WriteLine(string.Format("Arquivo '{0}' salvo.", nomeArquivo));

            // desmarca arquivo  
            marcadores[chaveDoUltimoSelecionado].Selecionado = false;

            // limpa buffer
            texto.Clear();
        }

        Dictionary<string, InformacaoArquivo> CriarMarcadoresComBaseEmArquivoDeConfiguracao(string nomeArquivoConfiguracoesMarcadores)
        {
            string linha;
            var retorno = new Dictionary<string, InformacaoArquivo>();

            using (System.IO.StreamReader arquivo = new System.IO.StreamReader(nomeArquivoConfiguracoesMarcadores))
            {
                while ((linha = arquivo.ReadLine()) != null)
                {
                    if (linha.StartsWith("##"))
                        continue;

                    var marcadores = linha.Split(';');
                    if (marcadores != null && marcadores.Count() >= 2)
                    {
                        var informacaoArquivo = new InformacaoArquivo(marcadores[1]);
                        retorno.Add(marcadores[0], informacaoArquivo);
                    }
                }
            }

            return retorno;
        }
    }

}
