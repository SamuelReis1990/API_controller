using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using API.ControleRapido.Models;
using API.DAL;
using Newtonsoft.Json;

namespace API.ControleRapido.Controllers
{
    /// <summary>
    /// Classe responsável pelos cruds de visitantes
    /// </summary>
    [AllowAnonymous]
    [RoutePrefix("api/visitantes")]
    public class VisitantesController : ApiController
    {
        private Contexto contexto;
        private Retorno retorno;
        /// <summary>
        /// Construtor da classe responsável por instanciar o contexto do EF
        /// </summary>
        public VisitantesController()
        {
            try
            {
                contexto = new Contexto();
                retorno = new Retorno();

            }
            catch (Exception e) { }
        }

        #region CRUD TABELA PESSOAS

        /// <summary>
        /// Método responsável por listar todos os dados da tabela Pessoas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("pessoas")]
        public IList<GetDadosPessoas> ListarPessoas()
        {
            try
            {
                var getDadosPessoas = contexto.pessoas.Select(p => new GetDadosPessoas()
                {
                    id_pessoa = p.id_pessoa,
                    id_tipo_pessoa = p.id_tipo_pessoa,
                    nome = p.nome,
                    dt_fim_val = p.dt_fim_val,
                    dt_ini_val = p.dt_ini_val,
                    old_id = p.old_id,
                    sexo = p.sexo,
                    foto = p.foto
                }).ToList();

                return getDadosPessoas;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Método responsável por listar os dados de uma pessoas da tabela Pessoas
        /// </summary>
        /// <param name="idPessoa"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{idPessoa}/pessoas")]
        public GetDadosPessoas ListarPessoas(long idPessoa)
        {
            var getDadosPessoas = contexto.pessoas.Select(p => new GetDadosPessoas()
            {
                id_pessoa = p.id_pessoa,
                id_tipo_pessoa = p.id_tipo_pessoa,
                nome = p.nome,
                dt_fim_val = p.dt_fim_val,
                dt_ini_val = p.dt_ini_val,
                old_id = p.old_id,
                sexo = p.sexo,
                foto = p.foto
            }).Where(p => p.id_pessoa == idPessoa).SingleOrDefault();

            return getDadosPessoas;
        }

        /// <summary>
        /// Método responsável por inserir os dados na tabela Pessoas
        /// </summary>
        /// <param name="dadosPessoas"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("pessoas")]
        public Retorno CadastrarPessoas([FromBody]DadosPessoas dadosPessoas)
        {
            try
            {
                var retornoJson = JsonConvert.SerializeObject(dadosPessoas);
                var retornoPessoas = JsonConvert.DeserializeObject<pessoa>(retornoJson);

                contexto.pessoas.Add(retornoPessoas);
                contexto.SaveChanges();

                retorno.idPessoa = retornoPessoas.id_pessoa.ToString();
                retorno.mensagemRetorno = "Operação realizada com sucesso.";

                return retorno;
            }
            catch (Exception e)
            {
                if (e.Message.Contains("inner exception for details"))
                {
                    retorno.mensagemRetorno = e.InnerException.InnerException.Message;
                }
                else
                {
                    retorno.mensagemRetorno = e.Message;
                }

                return retorno;
            }
        }

        /// <summary>
        /// Método responsável por atualizar os dados na tabela Pessoas
        /// </summary>
        /// <param name="idPessoa"></param>
        /// <param name="dadosPessoas"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{idPessoa}/pessoas")]
        public Retorno AtualizarPessoas(long idPessoa, [FromBody]DadosPessoas dadosPessoas)
        {
            try
            {
                var retornoJson = JsonConvert.SerializeObject(dadosPessoas);
                var retornoPessoas = JsonConvert.DeserializeObject<pessoa>(retornoJson);

                retornoPessoas.id_pessoa = idPessoa;

                contexto.Entry(retornoPessoas).State = EntityState.Modified;
                contexto.SaveChanges();

                retorno.idPessoa = retornoPessoas.id_pessoa.ToString();
                retorno.mensagemRetorno = "Operação realizada com sucesso.";

                return retorno;
            }
            catch (Exception e)
            {
                if (e.Message.Contains("inner exception for details"))
                {
                    retorno.mensagemRetorno = e.InnerException.InnerException.Message;
                }
                else
                {
                    retorno.mensagemRetorno = e.Message;
                }

                return retorno;
            }
        }

        /// <summary>
        /// Método responsável por remover os dados na tabela pessoas
        /// </summary>
        /// <param name="idPessoa"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{idPessoa}/pessoas")]
        public string RemoverPessoas(long idPessoa)
        {
            try
            {
                pessoa retornoPessoas = new pessoa();
                retornoPessoas.id_pessoa = idPessoa;

                contexto.Entry(retornoPessoas).State = EntityState.Deleted;
                contexto.SaveChanges();

                return "Operação realizada com sucesso.";
            }
            catch (Exception e)
            {
                if (e.Message.Contains("inner exception for details"))
                {
                    return e.InnerException.InnerException.Message;
                }
                else
                {
                    return e.Message;
                }
            }
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
