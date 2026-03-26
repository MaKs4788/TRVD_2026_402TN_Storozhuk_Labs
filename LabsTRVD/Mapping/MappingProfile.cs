using AutoMapper;
using LabsTRVD.DTOs;
using LabsTRVD.Entities;

namespace LabsTRVD.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Income <-> IncomeDto
            CreateMap<Income, IncomeDto>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ReverseMap()
                .ForMember(dest => dest.Category, opt => opt.Ignore()); // не мапимо навігаційне поле

            // Expense <-> ExpenseDto
            CreateMap<Expense, ExpenseDto>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId ?? 0)) // null -> 0 або можна nullable у DTO
                .ReverseMap()
                .ForMember(dest => dest.Category, opt => opt.Ignore());

            // Category <-> CategoryDto
            CreateMap<Category, CategoryDto>().ReverseMap();

            // Budget <-> BudgetDto
            CreateMap<Budget, BudgetDto>().ReverseMap();

            // DashboardSummary (тільки з DTO, тож прямого мапінгу може і не треба)
            CreateMap<DashboardSummaryDto, DashboardSummaryDto>().ReverseMap();
        }
    }
}