using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API.ControleRapido.Models;

namespace API.ControleRapido.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/visitantes")]
    public class VisitantesController : ApiController
    {
        #region CRUD TABELA PESSOAS

        /// <summary>
        /// Método responsável por listar todos os dados da tabela Pessoas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("pessoas")]
        public IList<DadosPessoas> ListarPessoas()
        {
            IList<DadosPessoas> dadosPessoas = new List<DadosPessoas>();

            return dadosPessoas;
        }

        /// <summary>
        /// Método responsável por listar os dados de uma pessoas da tabela Pessoas
        /// </summary>
        /// <param name="idPessoa"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{idPessoa}/pessoas")]
        public DadosPessoas ListarPessoas(string idPessoa)
        {
            DadosPessoas dadosPessoas = new DadosPessoas();

            return dadosPessoas;
        }

        /// <summary>
        /// Método responsável por inserir os dados na tabela Pessoas
        /// </summary>
        /// <param name="dadosPessoas"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("pessoas")]
        public string CadastrarPessoas([FromBody]DadosPessoas dadosPessoas)
        {
            return "Cadastrado";
        }

        /// <summary>
        /// Método responsável por atualizar os dados na tabela Pessoas
        /// </summary>
        /// <param name="idPessoa"></param>
        /// <param name="dadosPessoas"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{idPessoa}/pessoas")]
        public string AtualizarPessoas(string idPessoa, [FromBody]DadosPessoas dadosPessoas)
        {
            return "Atualizado";
        }

        /// <summary>
        /// Método responsável por remover os dados na tabela pessoas
        /// </summary>
        /// <param name="idPessoa"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{idPessoa}/pessoas")]
        public string RemoverPessoas(string idPessoa)
        {
            return "Removido";
        }

        #endregion

        #region CRUD TABELA DOCUMENTOS

        /// <summary>
        /// Método responsável por listar todos os dados da tabela Documentos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("documentos")]
        public IList<DadosDocumentos> ListarDocumentos()
        {
            IList<DadosDocumentos> dadosDocumento = new List<DadosDocumentos>();

            return dadosDocumento;
        }

        /// <summary>
        /// Método responsável por listar os dados de um documento da tabela Documentos
        /// </summary>
        /// <param name="idDocumento"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{idDocmento}/documentos")]
        public DadosDocumentos ListarDocumentos(string idDocumento)
        {
            DadosDocumentos dadosDocumentos = new DadosDocumentos();

            return dadosDocumentos;
        }

        /// <summary>
        /// Método responsável por inserir os dados na tabela Documentos
        /// </summary>
        /// <param name="dadosDocumentos"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("documentos")]
        public string CadastrarDocumentos([FromBody]DadosDocumentos dadosDocumentos)
        {
            return "Cadastrado";
        }

        /// <summary>
        /// Método responsável por atualizar os dados na tabela Documentos
        /// </summary>
        /// <param name="idDocumento"></param>
        /// <param name="dadosDocumentos"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{idDocumento}/documentos")]
        public string AtualizarDocumentos(string idDocumento, [FromBody]DadosDocumentos dadosDocumentos)
        {
            return "Atualizado";
        }

        /// <summary>
        /// Método responsável por remover os dados na tabela Documentos
        /// </summary>
        /// <param name="idDocumento"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{idDocumento}/documentos")]
        public string RemoverDocumentos(string idDocumento)
        {
            return "Removido";
        }

        #endregion
    }
}
