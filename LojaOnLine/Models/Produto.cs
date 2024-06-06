using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace LojaOnline.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        public decimal Preco { get; set; }
        public string? ImagemUrl { get; set; }

        // Propriedade para controlar a visibilidade do card na página inicial
        public bool VisivelNaPaginaInicial { get; set; } 

        [NotMapped]
        public IFormFile? Imagem { get; set; }
    }
}
