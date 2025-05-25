using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fim.Models;
using PagedList;
using PagedList.Mvc;


namespace Fim.Controllers
{
    public class ClientesController : Controller
    {
        public ActionResult EditCliente(int? id)
        {
            using (BdFamelEntities db = new BdFamelEntities())
            {
                try
                {
                    cliente edite = db.clientes.Include("cliente1").Where(c => c.idcli == (id ?? -1)).First();
                    if (edite != null)
                    {
                        List<string> categoria = new List<string>() { "Alpha", "Beta", "Gama" };
                        ViewBag.categoria = new SelectList(categoria);
                        var tutores = db.clientes.Select(c => c.tutor).Distinct().ToList();
                        var lstclientes = db.clientes.Where(c => !tutores.Contains(c.idcli)).Select(c => new { idcli = c.idcli, nome = c.nome }).ToList();
                        ViewBag.lstclientes = new SelectList(lstclientes, "idcli", "nome");
                        return View(edite);
                    }
                    else
                    {
                        return RedirectToAction("ListaClientes", new { msg = "Id não existe" });
                    }
                }
                catch (Exception erro)
                {
                    return RedirectToAction("ListaClientes", new { msg = erro.Message });
                }
            }
        }


        [HttpGet]
        public ActionResult DeleteCliente(int? id)
        {
            using (BdFamelEntities db = new BdFamelEntities())
            {
                try
                {
                    cliente morto = db.clientes.Include("cliente1").Where(c => c.idcli == (id ?? -1)).First();
                    if (morto != null)
                    {
                        return View(morto);
                    }
                    else
                    {
                        return RedirectToAction("ListaClientes", new { msg = "Id não existe" });
                    }
                }
                catch (Exception erro)
                {

                    return RedirectToAction("ListaClientes", new { msg = erro.Message });
                }
            }
        }

        [HttpPost]
        [ActionName("DeleteCliente")]
        public ActionResult del(int? id)
        {
            try
            {
                using (BdFamelEntities db = new BdFamelEntities())
                {
                    cliente morto = db.clientes.Find(id);
                    if (morto != null)
                    {
                        db.clientes.Remove(morto);
                        db.SaveChanges();
                        return RedirectToAction("ListaClientes", new { msg = "Cliente apagado com sucesso" });
                    }
                    else
                    {
                        return RedirectToAction("ListaClientes", new { msg = "Id não existe" });
                    }
                }
            }
            catch (Exception erro)
            {

                return RedirectToAction("ListaClientes", new { msg = erro.Message });
            }
        }

        public ActionResult Detailcliente(int ? id)
        {
            try
            {
                using (BdFamelEntities db = new BdFamelEntities())
                {
                    cliente este = db.clientes.Include("cliente1").Where(c => c.idcli == (id ?? -1)).First();
                    if (este != null)
                    {
                        return View(este);
                    }
                    else
                    {
                        return RedirectToAction("ListaClientes","Clientes", new { msg = "Id não existe" });
                    }
                }
            }
            catch (Exception erro)
            {

                return RedirectToAction("ListaClientes", new { msg = erro.Message });
            }
        }


        public ActionResult Alugueres(int? id)
        {
            try
            {
                    List<aluguere> lstalugueres = null;
                using (BdFamelEntities db = new BdFamelEntities())
                {
                    if (id != null)
                    {
                        lstalugueres = db.alugueres.Include("carro").Where(a => a.idcli == id).ToList();
                    }
                    else
                    {
                        lstalugueres = db.alugueres.Include("carro").ToList();

                    }
                    return PartialView(lstalugueres);
                }
            }
            catch (Exception erro)
            {
                return RedirectToAction("ListaClientes", new { msg = erro.Message });
            }
        }

        // GET: Clientes
        public ActionResult ListaClientes(String msg, int ? page)
        {
            using (BdFamelEntities bd = new BdFamelEntities())
            {
                int pagesize = 3;
                int pagina = page ?? 1;
                ViewBag.msg = msg;
                List<cliente> clientes = bd.clientes.ToList();
                return View(clientes.ToPagedList(pagina,pagesize));
            }


        }

        [HttpGet]
        public ActionResult AddCliente()
        {
            using (BdFamelEntities db = new BdFamelEntities())
            {
                List<string> cats = new List<string>() { "Alpha", "Beta", "Gama" };
                ViewBag.cats = new SelectList(cats);

                var tutores = db.clientes.Select(c => c.tutor).Distinct().ToList();
                var lstclientes = db.clientes.Where(c => !tutores.Contains(c.idcli)).Select(c => new { idcli = c.idcli, nome = c.nome }).ToList();
                ViewBag.lstclientes = new SelectList(lstclientes, "idcli", "nome");

                cliente novo = new cliente();
                return View(novo);
            }
        }

        [HttpPost]
        public ActionResult AddCliente(cliente novo, HttpPostedFileBase fich)
        {
            using (BdFamelEntities db = new BdFamelEntities())
            {
                try
                {
                    db.clientes.Add(novo);
                    db.SaveChanges();
                    if (fich != null && fich.ContentLength > 0 && fich.ContentType.Contains("image"))
                    {
                        string fichnome = novo.idcli.ToString() + System.IO.Path.GetExtension(fich.FileName);
                        novo.fotopath = fichnome;
                        string caminhofisico = Server.MapPath(@"~/fotos/" + fichnome);
                        fich.SaveAs(caminhofisico);
                        db.SaveChanges();
                    }
                    return RedirectToAction("ListaClientes", "Clientes", new { msg = "insert ok" });
                }
                catch (Exception erro)
                {

                    return RedirectToAction("ListaClientes", "Clientes", new { msg = $"Erro:{erro.Message}" });
                }
            }
        }
    }
}