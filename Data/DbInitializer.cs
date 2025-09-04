using AccountManagementSystem.Models;

namespace AccountManagementSystem.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            // Check if data already exists
            if (context.Persons.Any())
            {
                return; // DB has been seeded
            }

            var persons = new Person[]
            {
                new Person { name = "John", surname = "Doe", id_number = "8501015000089" },
                new Person { name = "Jane", surname = "Smith", id_number = "9002026000098" },
                new Person { name = "Bob", surname = "Johnson", id_number = "9503037000077" },
                new Person { name = "Alice", surname = "Brown", id_number = "8804048000066" },
                new Person { name = "Charlie", surname = "Wilson", id_number = "9205059000055" }
            };

            context.Persons.AddRange(persons);
            context.SaveChanges();

            var accounts = new Account[]
            {
                new Account { person_code = 1, account_number = "ACC10001", outstanding_balance = 1500.50m, is_closed = false },
                new Account { person_code = 2, account_number = "ACC10002", outstanding_balance = 2500.75m, is_closed = false },
                new Account { person_code = 3, account_number = "ACC10003", outstanding_balance = 500.25m, is_closed = false },
                new Account { person_code = 4, account_number = "ACC10004", outstanding_balance = 0.00m, is_closed = true },
                new Account { person_code = 5, account_number = "ACC10005", outstanding_balance = 3200.00m, is_closed = false }
            };

            context.Accounts.AddRange(accounts);
            context.SaveChanges();

            var transactions = new Transaction[]
            {
                new Transaction { account_code = 1, transaction_date = DateTime.Now.AddDays(-10), capture_date = DateTime.Now.AddDays(-10), amount = 100.00m, description = "Initial Deposit" },
                new Transaction { account_code = 1, transaction_date = DateTime.Now.AddDays(-5), capture_date = DateTime.Now.AddDays(-5), amount = -50.00m, description = "ATM Withdrawal" },
                new Transaction { account_code = 2, transaction_date = DateTime.Now.AddDays(-8), capture_date = DateTime.Now.AddDays(-8), amount = 200.00m, description = "Transfer Received" },
                new Transaction { account_code = 3, transaction_date = DateTime.Now.AddDays(-3), capture_date = DateTime.Now.AddDays(-3), amount = 150.00m, description = "Salary Deposit" },
                new Transaction { account_code = 5, transaction_date = DateTime.Now.AddDays(-1), capture_date = DateTime.Now.AddDays(-1), amount = -100.00m, description = "Online Payment" }
            };

            context.Transactions.AddRange(transactions);
            context.SaveChanges();
        }
    }
}