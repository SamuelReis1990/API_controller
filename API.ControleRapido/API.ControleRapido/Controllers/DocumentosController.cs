﻿using System;
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
    /// Classe responsável pelos cruds dos documentos
    /// </summary>
    [AllowAnonymous]
    [RoutePrefix("api/documentos")]
    public class DocumentosController : ApiController
    {
        private Contexto contexto;
        private RetornoDocumentos retorno;
        /// <summary>
        /// Construtor da classe responsável por instanciar o contexto do EF
        /// </summary>
        public DocumentosController()
        {
            try
            {
                contexto = new Contexto();
                retorno = new RetornoDocumentos();
            }
            catch (Exception e) { }
        }

        #region TABELA DOCUMENTOS

        /// <summary>
        /// Método responsável por listar todos os dados da tabela Documentos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("listarDocumentos")]
        public object ListarDocumentos()
        {
            try
            {
                var dadosDocumentos = contexto.documentos.Select(d => new DadosDocumentos()
                {
                    id_pessoa = d.id_pessoa,
                    id_documento = d.id_documento,
                    id_tipo_documento = d.id_tipo_documento,
                    numero = d.numero,
                    emissor = d.emissor,
                    dt_emissao = d.dt_emissao,
                    dt_validade = d.dt_validade
                }).ToList();

                return dadosDocumentos ?? new List<DadosDocumentos>();
            }
            catch(Exception e)
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
        /// Método responsável por listar os dados de um documento da tabela Documentos
        /// </summary>
        /// <param name="idDocumento"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{idDocmento}/listarDocumentos")]
        public object ListarDocumentos(long idDocumento)
        {
            try
            {
                var dadosDocumentos = contexto.documentos.Select(d => new DadosDocumentos()
                {
                    id_pessoa = d.id_pessoa,
                    id_documento = d.id_documento,
                    id_tipo_documento = d.id_tipo_documento,
                    numero = d.numero,
                    emissor = d.emissor,
                    dt_emissao = d.dt_emissao,
                    dt_validade = d.dt_validade
                }).Where(d => d.id_documento == idDocumento).ToList();

                return dadosDocumentos ?? new List<DadosDocumentos>();
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
        /// Método responsável por inserir os dados na tabela Documentos
        /// </summary>
        /// <param name="dadosDocumentos"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("cadastrarDocumentos")]
        public RetornoDocumentos CadastrarDocumentos([FromBody]DadosDocumentos dadosDocumentos)
        {
            try
            {
                var retornoJson = JsonConvert.SerializeObject(dadosDocumentos);
                var retornoDocumentos = JsonConvert.DeserializeObject<documento>(retornoJson);

                contexto.documentos.Add(retornoDocumentos);
                contexto.SaveChanges();

                retorno.idDocumento = retornoDocumentos.id_pessoa.ToString();
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
        /// Método responsável por atualizar os dados na tabela Documentos
        /// </summary>
        /// <param name="idDocumento"></param>
        /// <param name="dadosDocumentos"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{idDocumento}/atualizarDocumentos")]
        public RetornoDocumentos AtualizarDocumentos(long idDocumento, [FromBody]DadosDocumentos dadosDocumentos)
        {
            try
            {
                var retornoJson = JsonConvert.SerializeObject(dadosDocumentos);
                var retornoDocumentos = JsonConvert.DeserializeObject<documento>(retornoJson);

                retornoDocumentos.id_documento = idDocumento;

                contexto.Entry(retornoDocumentos).State = EntityState.Modified;
                contexto.SaveChanges();

                retorno.idDocumento = retornoDocumentos.id_pessoa.ToString();
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
        /// Método responsável por remover os dados na tabela Documentos
        /// </summary>
        /// <param name="idDocumento"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{idDocumento}/removerDocumentos")]
        public string RemoverDocumentos(long idDocumento)
        {
            try
            {
                documento retornoDocumentos = new documento();
                retornoDocumentos.id_documento = idDocumento;

                contexto.Entry(retornoDocumentos).State = EntityState.Deleted;
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

        #region TABELA TIPO_DOCUMENTO

        /// <summary>
        /// Método responsável por listar todos os dados da tabela Tipo_Documento
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("listarTipoDocumento")]
        public object ListarTipoDocumento()
        {
            try
            {
                var dadosTipoDocumento = contexto.tipo_documento.Select(t => new DadosTipoDocumento()
                {
                    descricao = t.descricao,
                    id_tipo_documento = t.id_tipo_documento
                }).ToList();

                return dadosTipoDocumento ?? new List<DadosTipoDocumento>();
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