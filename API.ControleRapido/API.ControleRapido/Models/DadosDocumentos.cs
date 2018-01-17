using System;

namespace API.ControleRapido.Models
{
    public class DadosDocumentos
    {
        public long id_documento { get; set; }
        public long id_pessoa { get; set; }
        public long id_tipo_documento { get; set; }
        public string numero { get; set; }
        public string emissor { get; set; }
        public DateTime? dt_validade { get; set; }
        public DateTime? dt_emissao { get; set; }
    }
}