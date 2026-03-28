using System;
using System.ComponentModel.DataAnnotations;

namespace LabsTRVD.DTOs.ServicesDTOs
{
    public class IncomeDto
    {
        [Required(ErrorMessage = "Сума обов'язкова")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Сума повинна бути більше 0")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Дата обов'язкова")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "UserId обов'язковий")]
        public Guid UserId { get; set; }

        // CategoryId = 0 або null означає "без категорії"
        // CategoryId > 0 означає конкретну категорію
        public int? CategoryId { get; set; }

        [StringLength(250, ErrorMessage = "Опис не може бути довше 250 символів")]
        public string? Description { get; set; }
    }
}