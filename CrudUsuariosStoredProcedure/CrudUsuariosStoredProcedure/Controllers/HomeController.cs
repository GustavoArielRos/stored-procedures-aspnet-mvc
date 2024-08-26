using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
using System.Diagnostics;
using CrudUsuariosStoredProcedure.Data;
using CrudUsuariosStoredProcedure.Models;

namespace CrudUsuariosStoredProcedure.Controllers
{
    public class HomeController : Controller
    {

        private readonly DataAccess _dataAccess;

        public HomeController(DataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        //p�gina index
        public IActionResult Index()
        {   
            try
            {   
                //variavel armazena a listagem de usuarios la do "DataAccess"
                var usuarios = _dataAccess.ListarUsuarios();
                return View(usuarios);//vai retorna essa view com a listagem

            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = "Ocorreu um erro na cria��o do usu�rio!";
                return View();//retorno a view "index"
            }


            
        }

        //p�gina Cadastrar
        public IActionResult Cadastrar()
        {
            return View();
        }

        //p�gina Editar
        public IActionResult Editar(int id)
        {
            var usuario = _dataAccess.BuscarUsuarioPorId(id);

            return View(usuario);
        }

        //p�gina Detalhes
        public IActionResult Detalhes(int id)
        {
            var usuario = _dataAccess.BuscarUsuarioPorId(id);

            return View(usuario);
        }

        public IActionResult Remover(int id)
        {
            var result = _dataAccess.Remover(id);

            if (result)
            {
                TempData["MensagemSucesso"] = "Usuario removido com sucesso";
            }
            else
            {
                TempData["MensagemSucesso"] = "Erro na execu��o";
            }


            return RedirectToAction("index");
        }

        //o que a p�gina executa
        //m�todo que trabalha com o que foi recebido do formul�rio
        [HttpPost]
        public IActionResult Cadastrar(Usuario usuario)
        {
            if(ModelState.IsValid)
            {
                var result = _dataAccess.Cadastrar(usuario);

                if(result)
                {
                    TempData["MensagemSucesso"] = "Usu�rio criado com sucesso!";
                    return RedirectToAction("Index");//redirecionado para a p�gina index
                }
                else
                {
                    TempData["MensagemErro"] = "Ocorreu um erro na cria��o do usu�rio";
                    return View(usuario);//permane�o na mesma p�gina
                }
            }
            else
            {
                return View(usuario);//retorna Cadastrar com essa informa��es de usuario
            }
        }

        //o que a p�gina executa
        //m�todo que trabalha com o que foi recebido do formul�rio
        [HttpPost]
        public IActionResult Editar(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                var result = _dataAccess.Editar(usuario);

                if (result)
                {
                    TempData["MensagemSucesso"] = "Usu�rio editado com sucesso!";
                    return RedirectToAction("Index");//redirecionado para a p�gina index
                }
                else
                {
                    TempData["MensagemErro"] = "Ocorreu um erro na cria��o do usu�rio";
                    return View(usuario);//permane�o na mesma p�gina
                }
            }
            else
            {
                TempData["MensagemErro"] = "Ocorreu um erro na cria��o do usu�rio";
                return View(usuario);//permane�o na mesma p�gina
            }

        }

    }
}
