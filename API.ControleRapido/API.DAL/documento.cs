//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace API.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class documento
    {
        public long id_documento { get; set; }
        public long id_pessoa { get; set; }
        public long id_tipo_documento { get; set; }
        public int numero { get; set; }
        public string emissor { get; set; }
        public Nullable<System.DateTime> dt_validade { get; set; }
        public Nullable<System.DateTime> dt_emissao { get; set; }
    
        public virtual pessoa pessoa { get; set; }
        public virtual tipo_documento tipo_documento { get; set; }
    }
}