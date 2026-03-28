using System;
using System.ComponentModel.DataAnnotations;

namespace LabsTRVD.DTOs.ServicesDTOs
{
    public class BudgetDto
    {
        [Required(ErrorMessage = "UserId обов'язковий")]
        public Guid UserId { get; set; }

        [Range(1, 12, ErrorMessage = "Місяць повинен бути від 1 до 12")]
        public int Month { get; set; }

        [Range(2000, 3000, ErrorMessage = "Рік некоректний")]
        public int Year { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Ліміт повинен бути більше 0")]
        public decimal MonthlyLimit { get; set; }
    }
}