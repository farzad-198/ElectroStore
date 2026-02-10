# ElectroStore - E-Commerce Platform

A full-featured e-commerce web application built with ASP.NET Core MVC, designed for selling electronic products.

## 🚀 Features

### Customer Features
- **User Authentication & Authorization**
  - User registration and login
  - Password reset via email
  - Profile management with address and phone number
  - Role-based access control (Customer/Admin)

- **Product Browsing**
  - Browse products by category
  - Product filtering by price range
  - Product sorting (price, name, newest)
  - Product details page
  - Featured products on homepage

- **Shopping Cart**
  - Add products to cart
  - Update quantities
  - Remove items from cart
  - Session-based cart for guests
  - Database-persisted cart for logged-in users
  - Automatic cart merge when user logs in

- **Checkout & Orders**
  - Secure checkout process
  - Order history in user profile
  - Order details view

### Admin Features
- **Dashboard**
  - Overview statistics (total products, orders, users, revenue)

- **Product Management**
  - Create, read, update, and delete products
  - Upload product images
  - Categorize products

- **Category Management**
  - Create, read, update, and delete categories

- **Order Management**
  - View all orders
  - Order details

- **User Management**
  - View all users
  - User details with role information
  - Delete users

## 🛠️ Technologies Used

- **Framework**: ASP.NET Core MVC (.NET 10.0)
- **Database**: SQL Server with Entity Framework Core
- **Authentication**: ASP.NET Core Identity
- **UI**: Bootstrap 5, jQuery
- **Email**: SMTP email service for password reset
- **Session Management**: In-memory session for cart

## 📋 Prerequisites

- .NET 10.0 SDK or later
- SQL Server (LocalDB, Express, or Full)
- Visual Studio 2022 or VS Code (optional)

## 🔧 Installation & Setup

### 1. Clone the Repository
```bash
git clone <repository-url>
cd ElectroStore
```

### 2. Update Database Connection String

Edit `appsettings.json` and update the connection string to match your SQL Server instance:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=ElectroStoreDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"
  }
}
```

### 3. Configure Email Settings (Optional)

If you want to use password reset functionality, update SMTP settings in `appsettings.json`:

```json
{
  "Smtp": {
    "Host": "smtp.gmail.com",
    "Port": 587,
    "Username": "your-email@gmail.com",
    "Password": "your-app-password",
    "From": "ElectroStore<your-email@gmail.com>"
  }
}
```

**Note**: For Gmail, you'll need to use an App Password, not your regular password.

### 4. Run Database Migrations

Open a terminal in the project directory (`ElectroStore/ElectroStore/shop1/`) and run:

```bash
dotnet ef database update
```

Or if you prefer using Package Manager Console in Visual Studio:
```powershell
Update-Database
```

### 5. Run the Application

```bash
cd ElectroStore/ElectroStore/shop1/
dotnet run
```

Or press `F5` in Visual Studio.

The application will be available at:
- `https://localhost:5001` or
- `http://localhost:5000`

## 👤 Default Admin Account

The application automatically seeds an admin account on first run:

- **Email**: `admin@electrostore.com`
- **Password**: `Admin123!`

You can use this account to log in and access the admin panel at `/Admin`.

## 📁 Project Structure

```
ElectroStore/
├── Controllers/          # MVC Controllers
│   ├── AccountController.cs
│   ├── AdminController.cs
│   ├── CartController.cs
│   ├── CheckoutController.cs
│   ├── HomeController.cs
│   ├── ProductController.cs
│   ├── ProfileController.cs
│   └── StaticController.cs
├── Data/                 # Database context and seed data
│   ├── ApplicationDbContext.cs
│   └── SeedData.cs
├── Models/               # Entity models
│   ├── ApplicationUser.cs
│   ├── CartItem.cs
│   ├── Category.cs
│   ├── Order.cs
│   └── Product.cs
├── Views/                # Razor views
│   ├── Account/
│   ├── Admin/
│   ├── Cart/
│   ├── Checkout/
│   ├── Home/
│   ├── Product/
│   ├── Profile/
│   └── Shared/
├── ViewModels/           # View models for data transfer
├── Services/             # Business services (Email sender)
├── Extensions/           # Extension methods
└── wwwroot/              # Static files (CSS, JS, images)
```

## 🔐 Security Features

- ASP.NET Core Identity for authentication
- Role-based authorization (Admin/Customer)
- Password hashing and validation
- CSRF protection
- SQL injection prevention via EF Core
- HTTPS redirection in production

## 🎨 UI/UX Features

- Responsive design with Bootstrap 5
- Modern, clean interface
- Product cards with images
- Shopping cart icon with item count
- User-friendly navigation


## 📝 License

This project is created for portfolio/resume purposes.

## 👨‍💻 Author

Developed as a showcase project demonstrating full-stack ASP.NET Core MVC development skills.

## 🚧 Future Enhancements

Potential improvements for future versions:
- Payment gateway integration
- Product reviews and ratings system
- Wishlist functionality
- Order status tracking
- Email notifications for order confirmations
- Inventory management
- Coupon/discount codes
- Multi-language support

---

**Note**: This is a portfolio project designed to showcase e-commerce development capabilities with ASP.NET Core MVC.


