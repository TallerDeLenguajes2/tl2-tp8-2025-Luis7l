
using System.ComponentModel.DataAnnotations;

namespace ViewModels
{
    public class ProductoViewModel
    {
        public int Id_producto {get;set;}
        [Display(Name ="descripcion producto bro")]
        [StringLength(250,ErrorMessage ="la descripcion no puede superar estos caracteres")]
        public string Descripcion{get;set;}

        [Display(Name ="Precio del producto")]
        [Required(ErrorMessage ="El precio debe ser obligatorio")]
        [Range(1,double.MaxValue,ErrorMessage ="el precio debe ser mayor a 0")]
        public float Precio {get;set;}
    }
}