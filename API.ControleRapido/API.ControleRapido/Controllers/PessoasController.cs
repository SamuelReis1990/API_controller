using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Http;
using API.ControleRapido.Models;
using API.DAL;
using Newtonsoft.Json;

namespace API.ControleRapido.Controllers
{
    /// <summary>
    /// Classe responsável pelos cruds das pessoas
    /// </summary>
    [AllowAnonymous]
    [RoutePrefix("api/pessoas")]
    public class PessoasController : ApiController
    {
        private Contexto contexto;
        private RetornoPessoas retorno;
        /// <summary>
        /// Construtor da classe responsável por instanciar o contexto do EF
        /// </summary>
        public PessoasController()
        {
            try
            {
                contexto = new Contexto();
                retorno = new RetornoPessoas();
            }
            catch (Exception e) { }
        }

        #region TABELA PESSOAS

        /// <summary>
        /// Método responsável por listar todos os dados da tabela Pessoas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("listarPessoas")]
        public object ListarPessoas()
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

                return getDadosPessoas ?? new List<GetDadosPessoas>();
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

        /// <summary>
        /// Método responsável por listar os dados de uma pessoas da tabela Pessoas
        /// </summary>
        /// <param name="nome"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{nome}/listarPessoas")]
        public object ListarPessoas(string nome)
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
                }).Where(p => p.nome == nome).ToList();

                return getDadosPessoas ?? new List<GetDadosPessoas>();
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

        /// <summary>
        /// Método responsável pelo autoComplete dos nomes das pessoas
        /// </summary>
        /// <param name="nome"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{nome}/autoCompletePessoas")]
        public object AutoCompletePessoas(string nome)
        {
            try
            {
                var getAutoCompleteNomePessoas = contexto.pessoas.Select(p => new GetAutoCompletePessoas()
                {
                    id = p.id_pessoa,
                    label = p.nome
                }).Where(p => p.label.Contains(nome)).ToList();

                getAutoCompleteNomePessoas = getAutoCompleteNomePessoas
               .GroupBy(i => i.label)
               .Select(j => new GetAutoCompletePessoas()
               {
                   label = j.First().label,
                   id = j.First().id
               }).ToList();

                return getAutoCompleteNomePessoas ?? new List<GetAutoCompletePessoas>();
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

        /// <summary>
        /// Método responsável por inserir os dados na tabela Pessoas
        /// </summary>
        /// <param name="dadosPessoas"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("cadastrarPessoas")]
        public RetornoPessoas CadastrarPessoas([FromBody]DadosPessoas dadosPessoas)
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
        [HttpPost]
        [Route("{idPessoa}/atualizarPessoas")]
        public RetornoPessoas AtualizarPessoas(long idPessoa, [FromBody]PostDadosPessoas dadosPessoas)
        {
            try
            {
                if (!String.IsNullOrEmpty(dadosPessoas.foto_string))
                {
                    dadosPessoas.foto = Convert.FromBase64String(dadosPessoas.foto_string);
                }                

                var retornoJson = JsonConvert.SerializeObject(dadosPessoas);
                var retornoPessoas = JsonConvert.DeserializeObject<pessoa>(retornoJson);

                retornoPessoas.id_pessoa = idPessoa;

                contexto.Entry(retornoPessoas).State = EntityState.Modified;
                contexto.SaveChanges();

                retorno.idPessoa = retornoPessoas.id_pessoa.ToString();
                retorno.mensagemRetorno = "Operação realizada com sucesso.";

                return retorno;
            }
            catch (DbEntityValidationException e)
            {
                if (e.Message.Contains("inner exception for details"))
                {
                    retorno.mensagemRetorno = e.InnerException.InnerException.Message;
                }
                else if (e.Message.Contains("EntityValidationErrors"))
                {
                    foreach (var lista in e.EntityValidationErrors)
                    {
                        foreach (var listaErro in lista.ValidationErrors)
                        {
                            retorno.mensagemRetorno += listaErro.ErrorMessage;
                        }
                    }
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
        [HttpPost]
        [Route("{idPessoa}/removerPessoas")]
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
            catch (DbEntityValidationException e)
            {
                var mensagemErro = string.Empty;

                if (e.Message.Contains("inner exception for details"))
                {
                    return e.InnerException.InnerException.Message;
                }
                else if (e.Message.Contains("EntityValidationErrors"))
                {
                    foreach (var lista in e.EntityValidationErrors)
                    {
                        foreach (var listaErro in lista.ValidationErrors)
                        {
                            mensagemErro += listaErro.ErrorMessage;
                        }
                    }

                    return mensagemErro;
                }
                else
                {
                    return e.Message;
                }
            }
        }

        #endregion

        #region TABELA TIPO_PESSOA

        /// <summary>
        /// Método responsável por listar todos os dados da tabela Tipo_Pessoa
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("listarTipoPessoa")]
        public object ListarTipoPessoa()
        {
            try
            {
                var dadosTipoPessoa = contexto.tipo_pessoa.Select(t => new DadosTipoPessoa()
                {
                    descricao = t.descricao,
                    id_tipo_pessoa = t.id_tipo_pessoa
                }).ToList();

                return dadosTipoPessoa ?? new List<DadosTipoPessoa>();
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
    }
}
