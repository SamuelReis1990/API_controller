using API.ControleRapido.Models;
using API.DAL;
using Newtonsoft.Json;
using NITGEN.SDK.NBioBSP;
using System;
using System.Data.Entity.Validation;
using System.Web.Http;

namespace API.ControleRapido.Controllers
{
    /// <summary>
    /// Classe responsável pelos cruds das biometrias
    /// </summary>    
    [AllowAnonymous]
    [RoutePrefix("api/biometria")]
    public class BiometriaController : ApiController
    {
        private Contexto contexto;
        private RetornoBiometria retorno;
        private NBioAPI nBioAPI;
        private NBioAPI.NSearch m_NSearch;
        private string caminhoBdLeitoraDigital = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + @"Content\dbLeitoraDigital.ISDB";
        /// <summary>
        /// Construtor da classe responsável por instanciar o contexto do EF
        /// </summary>
        public BiometriaController()
        {
            try
            {
                contexto = new Contexto();
                retorno = new RetornoBiometria();
                nBioAPI = new NBioAPI();
                m_NSearch = new NBioAPI.NSearch(nBioAPI);
            }
            catch (Exception e) { }
        }

        private bool InicializarDispositivo()
        {
            uint retorno = nBioAPI.OpenDevice(NBioAPI.Type.DEVICE_ID.AUTO);
            if (retorno == NBioAPI.Error.NONE)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool FecharDispositivo()
        {
            uint retorno = nBioAPI.CloseDevice(NBioAPI.Type.DEVICE_ID.AUTO);
            if (retorno == NBioAPI.Error.NONE)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Método responsável por listar um dedos cadastrado na tabela Dedos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("listarDedo")]
        public RetornoBiometria ListarDedo()
        {
            try
            {
                NBioAPI.Type.HFIR hFIR;

                if (InicializarDispositivo())
                {
                    m_NSearch.InitEngine();

                    uint retornoEnroll = nBioAPI.Capture(out hFIR);

                    if (retornoEnroll != NBioAPI.Error.NONE)
                    {
                        if (!FecharDispositivo())
                        {
                            retorno.mensagemRetorno = "Dispositivo não foi encerrado.";
                            return retorno;
                        }
                    }

                    if (!FecharDispositivo())
                    {
                        retorno.mensagemRetorno = "Dispositivo não foi encerrado.";
                        return retorno;
                    }
                  
                    m_NSearch.LoadDBFromFile(caminhoBdLeitoraDigital);
                    NBioAPI.NSearch.FP_INFO fpInfo;
                    m_NSearch.IdentifyData(hFIR, NBioAPI.Type.FIR_SECURITY_LEVEL.NORMAL, out fpInfo);
                    m_NSearch.TerminateEngine();

                    if (!fpInfo.ID.ToString().Equals("0"))
                    {
                        retorno.idPessoa = fpInfo.ID.ToString();
                        return retorno;
                    }
                    else
                    {
                        retorno.mensagemRetorno = "Digital não encontrada, favor cadastrar essa digital!";
                        return retorno;
                    }
                   
                    //m_NSearch.SaveDBToFile(caminhoBdLeitoraDigital);
                }
                else
                {
                    retorno.mensagemRetorno = "Dispositivo não foi encontrado!";
                    return retorno;
                }

            }
            catch (DbEntityValidationException e)
            {
                var mensagemErro = string.Empty;

                if (e.Message.Contains("inner exception for details"))
                {
                    retorno.mensagemRetorno = e.InnerException.InnerException.Message;
                    return retorno;
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

                    retorno.mensagemRetorno = mensagemErro;
                    return retorno;
                }
                else
                {
                    retorno.mensagemRetorno = e.Message;
                    return retorno;
                }
            }
        }

        /// <summary>
        /// Método responsável por inserir os dados na tabela Dedos
        /// </summary>
        /// <param name="idPessoa"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{idPessoa}/cadastrarDedos")]
        public RetornoBiometria CadastrarDedos(string idPessoa)
        {
            try
            {
                NBioAPI.Type.HFIR hFIR;
                NBioAPI.Type.FIR byteDedo;
                uint nUserID = 0;
                if (InicializarDispositivo())
                {
                    m_NSearch.InitEngine();

                    nUserID = Convert.ToUInt32(idPessoa, 10);

                    uint retornoEnroll = nBioAPI.Enroll(out hFIR, null);          

                    if (retornoEnroll != NBioAPI.Error.NONE)
                    {
                        if (!FecharDispositivo())
                        {
                            retorno.mensagemRetorno = "Dispositivo não foi encerrado.";
                            return retorno;
                        }
                    }

                    if (!FecharDispositivo())
                    {
                        retorno.mensagemRetorno = "Dispositivo não foi encerrado.";
                        return retorno;
                    }

                    m_NSearch.LoadDBFromFile(caminhoBdLeitoraDigital);

                    NBioAPI.NSearch.FP_INFO[] fpInfo;
                    retornoEnroll = m_NSearch.AddFIR(hFIR, nUserID, out fpInfo);

                    if (fpInfo != null)
                    {

                        nBioAPI.GetFIRFromHandle(hFIR, out byteDedo);

                        DadosDedos dadosDedos = new DadosDedos();

                        foreach (var dedos in fpInfo)
                        {
                            if (dedos.SampleNumber.ToString().Equals("0"))
                            {
                                dadosDedos.id_pessoa = dedos.ID;
                                dadosDedos.id_tipo_dedo = dedos.FingerID;
                                dadosDedos.imgdedo = byteDedo.Data;

                                var retornoJson = JsonConvert.SerializeObject(dadosDedos);
                                var retornoDedos = JsonConvert.DeserializeObject<dedo>(retornoJson);

                                contexto.dedos.Add(retornoDedos);
                                contexto.SaveChanges();
                            }
                        }

                        m_NSearch.SaveDBToFile(caminhoBdLeitoraDigital);
                        retorno.mensagemRetorno = "Operação realizada com sucesso.";
                    }
                    else
                    {
                        retorno.mensagemRetorno = "Digital já cadastrada!";
                    }

                    m_NSearch.TerminateEngine();                   
                    return retorno;
                }
                else
                {
                    retorno.mensagemRetorno = "Dispositivo não encontrado.";
                    return retorno;
                }
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
    }
}
