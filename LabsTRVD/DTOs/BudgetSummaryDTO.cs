namespace LabsTRVD.DTOs
{
    public class BudgetSummaryDto
    {
        public decimal Limit { get; set; }
        public decimal Used { get; set; }
        public decimal Remaining { get; set; }
        public double UsagePercentage { get; set; }
        public bool Exceeded { get; set; }
    }
}