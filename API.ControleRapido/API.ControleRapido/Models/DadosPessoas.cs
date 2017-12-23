using System;

namespace API.ControleRapido.Models
{
    public class DadosPessoas
    {        
        public long id_tipo_pessoa { get; set; }
        public string nome { get; set; }
        public string sexo { get; set; }
        public byte[] foto { get; set; }
        public long? old_id { get; set; }
        public DateTime? dt_ini_val { get; set; }
        public DateTime? dt_fim_val { get; set; }
    }
}