using System;
using System.Collections.Generic;
using System.Text;

namespace Teixeira.ArquivosHelper.SeparadorArquivos
{
    public class InformacaoArquivo
    {
        public string NomeArquivo { get; }

        public bool Selecionado { get; set; }

        public InformacaoArquivo(string nomeArquivo)
        {
            NomeArquivo = nomeArquivo;
        }
    }
}
