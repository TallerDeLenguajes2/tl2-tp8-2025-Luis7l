using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
namespace ViewModels
{
    public class AgregarProductoViewModel
    {
        public int Id_presupuesto{get;set;}
        [Display(Name ="Camtidad")]
        public int Id_producto {get;set;}
        [Display(Name ="Cantidad de productos")]
        [Required(ErrorMessage ="este campo es obligatorio pa")]
        [Range(1,int.MaxValue,ErrorMessage ="La cantidad de productos debe ser mayor a 0")]
        public int Cantidad{get;set;}
        [ValidateNever]
        public SelectList ListaProductos{get;set;}
    }
}