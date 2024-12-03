# PaymentsAPI

A robust **Payment Gateway API** built using **.NET 8.0**, **C#**, and **MySQL**. This project demonstrates a clean architecture for managing payments efficiently with features like parameter validation, structured logging, and database operations using raw SQL queries.

## Features

- **CRUD Operations**:
  - Add, retrieve, update, and delete payment entries.
- **Validation**:
  - Ensures input data integrity with comprehensive validation logic.
- **Asynchronous Programming**:
  - Built with async/await for optimized performance.
- **Structured Logging**:
  - Logs all actions and errors for easy debugging.
- **Raw SQL Queries**:
  - Direct SQL queries for full control over database operations.
- **Try-Catch Everywhere**:
  - Handles exceptions gracefully to ensure system stability.

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [MySQL Server](https://dev.mysql.com/downloads/mysql/)
- [Postman](https://www.postman.com/) (or any API testing tool)

## Getting Started

1. **Clone the Repository**
   ```
   git clone https://github.com/USERNAME/REPO_NAME.git
   cd PaymentsAPI
   ```

2. **Set Up the Database**

3. **Create a MySQL database:**
```CREATE DATABASE PaymentsDB;```

4. **Use the provided SQL script to create the Payments table.**

5. **Update Configuration**

6. **Update the appsettings.json with your MySQL connection details:**
```
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=PaymentsDB;User=root;Password=yourpassword;"
}
```

7. **Run the Application**
```dotnet run```

8. Test the APIs

9. Access Swagger for API documentation: http://localhost:5167/swagger
    
## API Endpoints
```
GET /api/payments: Retrieve all payments.
GET /api/payments/{id}: Retrieve payment by Transaction ID.
POST /api/payments: Add a new payment.
PUT /api/payments/{id}: Update payment by Transaction ID.
DELETE /api/payments/{id}: Delete payment by Transaction ID.
```

## Project Structure
```
PaymentsAPI/
├── Controllers/
│   └── PaymentsController.cs
├── Services/
│   └── PaymentService.cs
├── Repositories/
│   └── PaymentRepository.cs
├── Models/
│   └── Payment.cs
├── appsettings.json
├── Program.cs
└── PaymentsAPI.csproj
```

## Technology Stack
1. Backend: .NET 8.0, C#
2. Database: MySQL
3. Testing Tools: Postman, Swagger

## Contributing
Contributions are welcome! Please fork the repository and create a pull request.

## License
This project is licensed under the MIT License.
Feel free to tweak the README content as per your style! Let me know if you need further help.
