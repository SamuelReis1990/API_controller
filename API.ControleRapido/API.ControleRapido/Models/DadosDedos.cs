namespace API.ControleRapido.Models
{
    public class DadosDedos
    {
        public long id_dedos { get; set; }
        public byte[] imgdedo { get; set; }
        public long id_pessoa { get; set; }
        public long id_tipo_dedo { get; set; }
    }
}