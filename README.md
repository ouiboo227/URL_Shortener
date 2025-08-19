# URL Shortener Project

A modern URL shortening application built with ASP.NET Core 8.0, following strict MVC architecture with Bootstrap and jQuery for the frontend.

## Features

### Core Functionality

- **URL Shortening**: Convert long URLs to short, manageable links
- **Custom Aliases**: Create custom short codes for your URLs
- **Click Tracking**: Monitor how many times each shortened URL is accessed
- **Active/Inactive Status**: Enable or disable URLs as needed
- **Automatic Redirect**: Clicking shortened links redirects users to original URLs in new tabs

### Technical Features

- **MVC Architecture**: Strict adherence to Model-View-Controller pattern
- **Entity Framework Core**: Modern ORM with SQL Server support
- **Async Operations**: All database operations are asynchronous for better performance
- **Input Validation**: Comprehensive client and server-side validation
- **Responsive Design**: Bootstrap 5 for mobile-friendly interface
- **Font Awesome Icons**: Beautiful and intuitive iconography

## Project Structure

```
URLvibing/
├── shortenerURL_vibing/          # Main MVC Application
│   ├── Controllers/              # MVC Controllers
│   ├── Models/                   # Data Models
│   ├── Views/                    # Razor Views
│   ├── Services/                 # Business Logic Services
│   ├── Data/                     # Database Context
│   └── wwwroot/                  # Static Files (CSS, JS, Images)
├── shortenerURL_API/             # REST API Project
│   ├── Controllers/              # API Controllers
│   └── Program.cs                # API Configuration
└── URLvibing.sln                 # Solution File
```

## Prerequisites

- .NET 8.0 SDK
- SQL Server (LocalDB recommended for development)
- Visual Studio 2022 or VS Code

## Installation & Setup

### 1. Clone the Repository

```bash
git clone <repository-url>
cd URLvibing
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Database Setup

The application uses Entity Framework Core with SQL Server LocalDB by default.

**Option A: Use LocalDB (Recommended for Development)**

- LocalDB is included with SQL Server Express
- Connection string is already configured in `appsettings.json`

**Option B: Use Full SQL Server**

- Update the connection string in `appsettings.json`
- Replace `(localdb)\mssqllocaldb` with your SQL Server instance

### 4. Create Database

```bash
cd shortenerURL_vibing
dotnet ef database update
```

### 5. Run the Application

```bash
# Run the main MVC application
dotnet run

# Run the API project (in a separate terminal)
cd ../shortenerURL_API
dotnet run
```

## Usage

### Web Interface

1. **Navigate to the application** - The home page automatically redirects to the URL shortener
2. **Create Short URLs**:
   - Click "Create New Short URL"
   - Enter the original URL
   - Optionally provide a custom alias
   - Click "Create Short URL"
3. **Manage URLs**:
   - View all URLs in the main table
   - Edit URL details and status
   - Delete URLs when no longer needed
   - View detailed statistics

### API Endpoints

The API provides REST endpoints for programmatic access:

#### Get All URLs

```
GET /api/UrlApi
```

#### Get URL by ID

```
GET /api/UrlApi/{id}
```

#### Create New URL

```
POST /api/UrlApi
Content-Type: application/json

{
  "originalUrl": "https://example.com",
  "customAlias": "optional-custom-alias"
}
```

#### Update URL

```
PUT /api/UrlApi/{id}
Content-Type: application/json

{
  "id": 1,
  "originalUrl": "https://updated-example.com",
  "customAlias": "new-alias",
  "isActive": true
}
```

#### Delete URL

```
DELETE /api/UrlApi/{id}
```

#### Get Redirect Information

```
GET /api/UrlApi/redirect/{shortCode}
```

#### Check Alias Availability

```
GET /api/UrlApi/check-alias/{alias}
```

#### Increment Click Count

```
POST /api/UrlApi/{id}/increment-clicks
```

## Database Schema

### UrlModel

- **Id**: Primary key (auto-increment)
- **OriginalUrl**: The full URL to be shortened (required)
- **ShortenedUrl**: The generated short code (auto-generated or custom)
- **CreatedDate**: When the URL was created
- **ClickCount**: Number of times the URL was accessed
- **CustomAlias**: Optional custom short code
- **IsActive**: Whether the URL is currently active

## Configuration

### Connection String

Update the connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your-server;Database=UrlShortenerDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### Environment Variables

For production, consider using environment variables for sensitive configuration:

```bash
setx ConnectionStrings__DefaultConnection "Server=prod-server;Database=UrlShortenerDb;User Id=username;Password=password"
```

## Development

### Adding New Features

1. **Models**: Add new properties to `UrlModel` or create new models
2. **Services**: Extend `IUrlService` and `UrlService` for new business logic
3. **Controllers**: Add new actions to `UrlController` or create new controllers
4. **Views**: Create corresponding Razor views for new functionality

### Database Migrations

When you modify the data model:

```bash
# Create a new migration
dotnet ef migrations add MigrationName

# Apply migrations to database
dotnet ef database update

# Remove last migration (if needed)
dotnet ef migrations remove
```

## Testing

### Manual Testing

1. Create various types of URLs (with and without custom aliases)
2. Test URL redirection functionality
3. Verify click counting works correctly
4. Test edit and delete operations
5. Verify validation messages

### API Testing

Use tools like Postman or Swagger UI to test API endpoints:

- Swagger UI is available at `/swagger` when running in development mode

## Deployment

### Production Considerations

1. **Database**: Use a production SQL Server instance
2. **Connection String**: Use secure connection strings with proper authentication
3. **HTTPS**: Ensure HTTPS is enabled in production
4. **Environment**: Set `ASPNETCORE_ENVIRONMENT=Production`
5. **Logging**: Configure appropriate logging levels

### Azure Deployment

1. Create an Azure App Service
2. Set up Azure SQL Database
3. Configure connection strings in App Service settings
4. Deploy using Azure DevOps or GitHub Actions

## Future Enhancements

### Planned Features

- **User Authentication**: Login system for managing personal URLs
- **Analytics Dashboard**: Detailed click analytics and charts
- **URL Expiration**: Set expiration dates for temporary URLs
- **Bulk Operations**: Import/export multiple URLs
- **QR Code Generation**: Generate QR codes for shortened URLs
- **Social Media Integration**: Share shortened URLs on social platforms

### API Enhancements

- **Rate Limiting**: Prevent API abuse
- **Authentication**: JWT token-based authentication
- **Webhooks**: Notify external systems of URL clicks
- **Bulk Operations**: Create multiple URLs in one request

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Support

For questions or issues:

1. Check the documentation
2. Search existing issues
3. Create a new issue with detailed information

## Acknowledgments

- ASP.NET Core team for the excellent framework
- Bootstrap team for the responsive CSS framework
- Font Awesome for the beautiful icons
- Entity Framework team for the powerful ORM
