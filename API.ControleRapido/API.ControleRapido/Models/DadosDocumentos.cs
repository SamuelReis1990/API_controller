namespace API.ControleRapido.Models
{
    public class DadosDocumentos
    {
        public string ID_DOCUMENTO { get; set; }
        public string ID_PESSOA { get; set; }
        public string ID_TIPO_DOCUMENTO { get; set; }
        public string NUMERO { get; set; }
        public string EMISSOR { get; set; }
        public string DT_VALIDADE { get; set; }
        public string DT_EMISSAO { get; set; }
    }
}