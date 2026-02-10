using System.Web.Mvc;
using ProductosAlimenticiosMVC.Data;
using ProductosAlimenticiosMVC.Models;

namespace ProductosAlimenticiosMVC.Controllers
{
    public class ProductoController : Controller
    {
        ProductoDAO dao = new ProductoDAO();

        public ActionResult Index()
        {
            return View(dao.Listar());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Producto p)
        {
            dao.EjecutarSP(2, p);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(Producto p)
        {
            return View(p);
        }

        [HttpPost]
        public ActionResult EditConfirm(Producto p)
        {
            dao.EjecutarSP(3, p);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(Producto p)
        {
            dao.EjecutarSP(4, p);
            return RedirectToAction("Index");
        }
    }
}