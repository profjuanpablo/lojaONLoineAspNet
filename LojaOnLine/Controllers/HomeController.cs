using Microsoft.AspNetCore.Mvc;
using LojaOnline.Data;
using LojaOnline.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace LojaOnline.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var produtos = _context.Produtos.Where(p => p.VisivelNaPaginaInicial).ToList();
            return View(produtos);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Produto produto)
        {
            _logger.LogInformation("Entrando no método Create");

            if (ModelState.IsValid)
            {
               // produto.VisivelNaPaginaInicial = true;
                _logger.LogInformation("ModelState é válido");

                if (produto.Imagem != null)
                {
                    _logger.LogInformation("Imagem recebida: " + produto.Imagem.FileName);

                    var imgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img");
                    if (!Directory.Exists(imgPath))
                    {
                        _logger.LogInformation("Diretório não existe, criando: " + imgPath);
                        Directory.CreateDirectory(imgPath);
                    }

                    var fileName = Path.GetFileNameWithoutExtension(produto.Imagem.FileName);
                    var extension = Path.GetExtension(produto.Imagem.FileName);
                    var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{extension}";
                    var filePath = Path.Combine(imgPath, uniqueFileName);

                    _logger.LogInformation("Caminho completo do arquivo: " + filePath);

                    try
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await produto.Imagem.CopyToAsync(stream);
                        }

                        _logger.LogInformation("Imagem salva com sucesso");
                        produto.ImagemUrl = "/img/" + uniqueFileName;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Erro ao salvar a imagem: " + ex.Message);
                    }
                }

                try
                {
                    _context.Add(produto);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Produto salvo com sucesso no banco de dados");
                }
                catch (Exception ex)
                {
                    _logger.LogError("Erro ao salvar o produto no banco de dados: " + ex.Message);
                }

                return RedirectToAction(nameof(Index));
            }

            _logger.LogInformation("ModelState é inválido");
            return View(produto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Descricao,Preco,ImagemUrl,VisivelNaPaginaInicial")] Produto produto)
        {
            if (id != produto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var produtoAtual = await _context.Produtos.FindAsync(id);
                    if (produtoAtual == null)
                    {
                        return NotFound();
                    }

                    // Atualizar apenas a propriedade VisivelNaPaginaInicial
                    produtoAtual.VisivelNaPaginaInicial = produto.VisivelNaPaginaInicial;

                    _context.Update(produtoAtual);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProdutoExists(produto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(produto);
        }

        private bool ProdutoExists(int id)
        {
            return _context.Produtos.Any(e => e.Id == id);
        }

    }
}
