
### Project Overview

1. **Core Functionality:**
   - "This API handles complete library operations"
   - "Users can register and login with email verification"
   - "Librarians can manage books, authors, categories, and publishers"
   - "Students can borrow and return books"
   - "System tracks overdue books and loan history"

2. **Key Benefits:**
   - "RESTful API design for easy integration"
   - "Role-based authentication for security"
   - "Complete CRUD operations for all entities"
   - "Professional error handling and logging"

---

### Technology Stack

- "ASP.NET Core for the API framework"
- "Entity Framework Core for database operations"
- "SQLite for data storage"
- "Identity Framework for authentication"

---

### Architecture Pattern
Implemented a clean architecture with these layers:

1. **Controllers** - "Handle HTTP requests and responses"
2. **Services** - "Business logic and validation"
3. **Repository** - "Data access abstraction"
4. **Models** - "Data entities and DTOs"

---

### Key Technical Features

1. **Authentication & Authorization:**
   - "JWT token-based authentication"
   - "Email verification for new users"
   - "Role-based access control"

2. **Error Handling:**
    - "Handled manually in each controller using try-catch blocks"
    - "Structured logging with info, warning, and error levels"
    - "Returns proper HTTP status codes based on the error"

3. **Data Validation:**
   - "Model validation attributes"
   - "Business rule validation in services"
   - "Database constraints"


---

### Design Patterns Used
1. **Repository Pattern**: Separates data access from business logic
2. **Service Layer Pattern**: Contains all business rules and validation
3. **Dependency Injection**: Makes code testable and maintainable
4. **DTO Pattern**: Separates internal models from API responses

---

### Database Design
- **8 main entities**: Books, Authors, Categories, Publishers, Loans, Users, Roles
- **Proper relationships**: Foreign keys and navigation properties
- **Identity integration**: Built-in user management and authentication