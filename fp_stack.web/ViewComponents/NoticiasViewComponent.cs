using fp_stack.core.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fp_stack.web.ViewComponents
{
    public class NoticiasViewComponent : ViewComponent
    {
        core.Services.NoticiaService _noticiaService;
        public NoticiasViewComponent(core.Services.NoticiaService noticiaService)
        {
            _noticiaService = noticiaService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int total, bool noticiasUrgentes=false)
        {
            var view = noticiasUrgentes ? "noticiasUrgentes" : "noticias";
            var itens = _noticiaService.GetItens(total);

            return View(view, itens);
        }
        
    }
}
