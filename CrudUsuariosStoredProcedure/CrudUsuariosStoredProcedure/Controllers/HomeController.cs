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

        //página index
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
                TempData["MensagemErro"] = "Ocorreu um erro na criação do usuário!";
                return View();//retorno a view "index"
            }


            
        }

        //página Cadastrar
        public IActionResult Cadastrar()
        {
            return View();
        }

        //página Editar
        public IActionResult Editar(int id)
        {
            var usuario = _dataAccess.BuscarUsuarioPorId(id);

            return View(usuario);
        }

        //página Detalhes
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
                TempData["MensagemSucesso"] = "Erro na execução";
            }


            return RedirectToAction("index");
        }

        //o que a página executa
        //método que trabalha com o que foi recebido do formulário
        [HttpPost]
        public IActionResult Cadastrar(Usuario usuario)
        {
            if(ModelState.IsValid)
            {
                var result = _dataAccess.Cadastrar(usuario);

                if(result)
                {
                    TempData["MensagemSucesso"] = "Usuário criado com sucesso!";
                    return RedirectToAction("Index");//redirecionado para a página index
                }
                else
                {
                    TempData["MensagemErro"] = "Ocorreu um erro na criação do usuário";
                    return View(usuario);//permaneço na mesma página
                }
            }
            else
            {
                return View(usuario);//retorna Cadastrar com essa informações de usuario
            }
        }

        //o que a página executa
        //método que trabalha com o que foi recebido do formulário
        [HttpPost]
        public IActionResult Editar(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                var result = _dataAccess.Editar(usuario);

                if (result)
                {
                    TempData["MensagemSucesso"] = "Usuário editado com sucesso!";
                    return RedirectToAction("Index");//redirecionado para a página index
                }
                else
                {
                    TempData["MensagemErro"] = "Ocorreu um erro na criação do usuário";
                    return View(usuario);//permaneço na mesma página
                }
            }
            else
            {
                TempData["MensagemErro"] = "Ocorreu um erro na criação do usuário";
                return View(usuario);//permaneço na mesma página
            }

        }

    }
}
