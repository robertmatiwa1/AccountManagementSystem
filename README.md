# Account Management System

A comprehensive ASP.NET Core MVC application for managing people, their bank accounts, and financial transactions with full CRUD operations, search, sorting, and pagination.

## Features

### Core Functionality
- Person Management: Complete CRUD operations for individuals
- Account Management: Create and manage bank accounts with balance tracking
- Transaction Processing: Record financial transactions (deposits, withdrawals, transfers)
- Search & Filter: Advanced search across multiple fields (ID number, surname, account number)
- Sorting: Sort by any column in ascending/descending order
- Pagination: Efficient data browsing with configurable page sizes

### Business Rules & Validation
- ✅ Unique ID Number validation for persons
- ✅ No duplicate account numbers allowed
- ✅ Cannot close accounts with non-zero balances
- ✅ Cannot post transactions to closed accounts
- ✅ Transaction dates cannot be in the future
- ✅ Transaction amounts cannot be zero
- ✅ Balance automatically updates with transactions
- ✅ Persons with active accounts cannot be deleted

### User Interface
- Responsive Design: Bootstrap 5 styling
- Modern UI: Clean, professional interface
- Delete Confirmation: Modal dialogs for safe deletions
- Form Validation: Client and server-side validation
- Navigation: Consistent menu across all pages

## Technology Stack

- Backend: ASP.NET Core 6.0 MVC
- Database: SQL Server with Entity Framework Core
- Frontend: Bootstrap 5, HTML5, CSS3
- Pagination: X.PagedList
- Validation: DataAnnotations
- Architecture: Repository pattern with DbContext

## Database Schema

```sql
Persons (code, name, surname, id_number)
Accounts (code, person_code, account_number, outstanding_balance, is_closed)  
Transactions (code, account_code, transaction_date, capture_date, amount, description)

## Getting Started

### Prerequisites
- .NET 6.0 SDK
- SQL Server (Express or higher)
- Git

### Installation

1. Clone the repository
   ```bash
   git clone https://github.com/robertmatiwa1/AccountManagementSystem.git
   cd AccountManagementSystem
   ```

2. Database Setup
   - Update connection string in `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=your-server;Database=AccountManagementDB;Trusted_Connection=true;TrustServerCertificate=true;"
   }
   ```

3. Run the application
   ```bash
   dotnet run
   ```

4. Access the application
   - Open: https://localhost:7000 (or your configured port)

### Default Data
The application includes seed data with:
- 50 sample persons
- Multiple bank accounts
- Sample transactions
- Balanced account calculations

## Project Structure

```
AccountManagementSystem/
├── Controllers/           # MVC Controllers
│   ├── PersonsController.cs
│   ├── AccountsController.cs
│   └── TransactionsController.cs
├── Models/               # Data Models
│   ├── Person.cs
│   ├── Account.cs
│   └── Transaction.cs
├── Views/               # Razor Views
│   ├── Persons/
│   ├── Accounts/
│   └── Transactions/
├── Data/                # Data Access Layer
│   ├── ApplicationDbContext.cs
│   └── DbInitializer.cs
├── wwwroot/            # Static Files
├── Program.cs          # Application Entry
└── appsettings.json   # Configuration
```

## Usage Examples

### Adding a New Person
1. Navigate to Persons → Create New
2. Fill in required fields (Surname, ID Number)
3. Submit to create person record

### Creating an Account
1. Go to Person Details → Add New Account
2. Enter account number and initial balance
3. System validates uniqueness and business rules

### Processing Transactions
1. From Account Details → Add Transaction
2. Enter transaction details (date, amount, description)
3. System updates account balance automatically

## Search Features

- ID Number Search: Find persons by national ID
- Surname Search: Filter by last name
- Account Number Search: Locate specific accounts
- Combined Search: Use multiple filters simultaneously

## Configuration

### AppSettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Your-SQL-Server-Connection-String"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

## Testing

The application includes comprehensive validation:
- Model validation with DataAnnotations
- Business rule enforcement
- Database constraint validation
- User input sanitization

## Performance Features

- Efficient pagination (10 records per page)
- Database indexing on search fields
- Lazy loading for related data
- Optimized SQL queries

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit changes (`git commit -m 'Add amazing feature'`)
4. Push to branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.

## Author

Robert Matiwa
- GitHub: [@robertmatiwa1](https://github.com/robertmatiwa1)
- Project: [Account Management System](https://github.com/robertmatiwa1/AccountManagementSystem)

## Acknowledgments

- ASP.NET Core Team
- Bootstrap Team
- Entity Framework Core Team
- X.PagedList contributors

---

Note: This application demonstrates enterprise-level patterns for financial data management with proper validation and business rule enforcement.
```

## Add the README to Git and Push

```bash
# Create the README.md file with the content above
# Then add it to git:

git add README.md
git commit -m "Add comprehensive README documentation"
git push origin main
```

## Optional: Add a LICENSE file

Create `LICENSE.md` if you want:

```markdown
MIT License

Copyright (c) 2025 Robert Matiwa

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
```

```bash
# Add license if desired
git add LICENSE.md
git commit -m "Add MIT license"
git push origin main
```
