namespace LabsTRVD.Entities
{
    public class Expenses
    {
        public int ExpenseId { get; set; }

        public decimal ExpenseAmount { get; set; }

        public DateTime ExpenseDate { get; set; }

        public Guid UserId { get; set; }
        public int CategoryId { get; set; }

        public string ExpenseDescription { get; set; }
    }
}
