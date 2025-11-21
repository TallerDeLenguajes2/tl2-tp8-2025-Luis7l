using System.ComponentModel.DataAnnotations;

namespace ViewModels
{
    public class PresupuestoViewModel
    {
        public int Id_presupuesto {get;set;}
        [Display(Name ="Nombre del destinatario")]
        [StringLength(100,ErrorMessage ="El nombre es corto")]
        [EmailAddress(ErrorMessage ="el mail no es valido")]  
        [Required(ErrorMessage ="el nombre es obligatorio")]
        public string NombreDestinatario{get;set;}

        [Display (Name ="Fecha de creacion")]
        [Required(ErrorMessage =" La fecha es necesaria pa")]
        [DataType(DataType.Date)]
        public DateTime FechaCreacion{get;set;} 

    }
}