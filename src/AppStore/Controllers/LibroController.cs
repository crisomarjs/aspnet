using System.Net.Mail;
using AppStore.Models.Domain;
using AppStore.Repositories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AppStore.Controllers;


[Authorize]
public class LibroController : Controller
{
    private readonly ILibroService _libroService;
    private readonly IFileService _fileService;
    private readonly ICategoriaService _categoriaService;

    public LibroController(ILibroService libroService, IFileService fileService, ICategoriaService categoriaService)
    {
        _libroService = libroService;
        _fileService = fileService;
        _categoriaService = categoriaService;
    }

    [HttpPost]
    public IActionResult Add(Libro libro)
    {
        libro.CategoriasList = _categoriaService.List().Select(a => new SelectListItem {Text = a.Nombre, Value = a.Id.ToString()});
        if(!ModelState.IsValid)
        {
            return View(libro);
        }

        if(libro.ImageFile != null)
        {
            var res = _fileService.SaveImage(libro.ImageFile);
            if(res.Item1 == 0)
            {
                TempData["msg"] = "La imagen no se pudo guardar";
                return View(libro);
            }

            var imageName = res.Item2;
            libro.Imagen = imageName;
        }

        var resultadoLibro = _libroService.Add(libro);
        if(resultadoLibro)
        {
            TempData["msg"] = "Libro guardado correctamente";
            return RedirectToAction(nameof(Add));
        }

        TempData["msg"] = "Error, No se pudo guardar el libro";
        return View(libro);
    }

    [HttpPost]
    public IActionResult Edit(Libro libro)
    {
       var categoriasDeLibro = _libroService.GetCategoryByLibroId(libro.Id);
       var multiSelectListCategory = new MultiSelectList(_categoriaService.List(), "Id", "Nombre", categoriasDeLibro);
        libro.MultiCategoriasList = multiSelectListCategory;

        if(!ModelState.IsValid)
        {
            return View(libro);
        }

        if(libro.ImageFile != null)
        {
            var fileResult = _fileService.SaveImage(libro.ImageFile);
            if(fileResult.Item1 == 0)
            {
                TempData["msg"] = "La imagen no se pudo guardar";
                return View(libro);
            }

            var imageName = fileResult.Item2;
            libro.Imagen = imageName;
        }

        var resultLibro = _libroService.Update(libro);
        if(!resultLibro)
        {
            TempData["msg"] = "Error, No se pudo actualizar el libro";
            return View(libro);
        }

        TempData["msg"] = "Libro actualizado correctamente";
        return View(libro);
    }

    public IActionResult Add()
    {
        var libro = new Libro();
        libro.CategoriasList = _categoriaService.List().Select(a => new SelectListItem {Text = a.Nombre, Value = a.Id.ToString()});

        return View(libro);
    }

    public IActionResult Edit(int id)
    {
        var libro = _libroService.GetById(id);
        var categoriasDeLibro = _libroService.GetCategoryByLibroId(id);
        var multiSelectListCategory = new MultiSelectList(_categoriaService.List(), "Id", "Nombre", categoriasDeLibro);
        libro.MultiCategoriasList = multiSelectListCategory;

        return View(libro);
    }

    public IActionResult LibroList()
    {
        var libros = _libroService.List();
        return View(libros);
    }

    public IActionResult Delete(int id)
    {
        _libroService.Delete(id);
        return RedirectToAction(nameof(LibroList));
    }
}