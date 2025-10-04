# Engagement Rings Product Listing Application

A beautiful product listing application built with ASP.NET MVC and Web API that displays engagement rings with dynamic pricing based on gold prices.

## Features

- **RESTful API**: Backend API that serves product data from JSON file
- **Dynamic Pricing**: Price calculation based on popularity score, weight, and real-time gold prices
- **Responsive Design**: Modern, mobile-friendly interface with gradient backgrounds
- **Image Carousel**: Swiper.js powered image carousel with navigation arrows and touch support
- **Color Picker**: Interactive color selection that changes product images
- **Star Rating**: Visual popularity score display (1-5 stars)
- **Filtering**: API supports filtering by price range and popularity score

## Technology Stack

- **Backend**: ASP.NET MVC 5, Web API 2
- **Frontend**: HTML5, CSS3, JavaScript (ES6+)
- **Styling**: Custom CSS with Avenir and Montserrat fonts
- **Carousel**: Swiper.js
- **Caching**: MemoryCache for gold price data
- **JSON**: Newtonsoft.Json for data serialization

## Project Structure

```
CaseStudy/
├── Controllers/
│   ├── Api/ProductsController.cs    # Web API controller
│   └── HomeController.cs            # MVC controller
├── Models/
│   └── Product.cs                   # Product data model
├── Services/
│   ├── GoldPriceService.cs         # Gold price fetching service
│   ├── IGoldPriceService.cs        # Gold service interface
│   ├── ProductService.cs           # Product data service
│   └── IProductService.cs          # Product service interface
├── Views/Home/
│   └── Index.cshtml                 # Main product listing page
├── Content/
│   └── styles.css                   # Custom styling
├── Data/
│   └── products.json                # Product data
├── Fonts/                           # Custom fonts (Avenir, Montserrat)
└── App_Start/
    └── WebApiConfig.cs              # Web API configuration
```

## API Endpoints

### GET /api/products

Returns all products with optional filtering and sorting.

**Query Parameters:**

- `minPrice` (double): Minimum price filter
- `maxPrice` (double): Maximum price filter
- `minPop` (double): Minimum popularity score filter
- `maxPop` (double): Maximum popularity score filter
- `sort` (string): Sort order (`price_asc`, `price_desc`)

**Response:**

```json
{
  "goldPricePerGram": 60.0,
  "products": [
    {
      "id": 1,
      "name": "Engagement Ring 1",
      "popularityScore": 0.85,
      "weight": 2.1,
      "images": {
        "yellow": "https://...",
        "rose": "https://...",
        "white": "https://..."
      },
      "priceUSD": 153.3
    }
  ]
}
```

### GET /api/products/{id}

Returns a specific product by ID.

## Price Calculation

Products are priced using the formula:

```
Price = (popularityScore + 1) * weight * goldPricePerGram
```

- `popularityScore`: 0-1 scale (converted to percentage if needed)
- `weight`: Weight in grams
- `goldPricePerGram`: Current gold price per gram in USD

## Gold Price Configuration

Configure gold price API in `Web.config`:

```xml
<appSettings>
  <add key="GoldApi:Url" value="https://api.example.com/gold-price" />
  <add key="GoldApi:CacheMinutes" value="10" />
  <add key="GoldApi:FallbackPerGram" value="60" />
</appSettings>
```

If no API URL is provided, the system uses a fallback price of $60/gram.

## Running the Application

1. **Build the project:**

   ```bash
   msbuild CaseStudy.csproj /p:Configuration=Debug
   ```

2. **Run in Visual Studio:**

   - Open `CaseStudy.sln` in Visual Studio
   - Press F5 or click "Start Debugging"

3. **Run with IIS Express:**
   - Right-click project → "View in Browser"
   - Or navigate to `http://localhost:port/`

## Frontend Features

- **Responsive Grid**: Auto-adjusting product grid layout
- **Image Carousel**: Touch/swipe support for mobile devices
- **Color Selection**: Click color buttons to change product images
- **Star Ratings**: Visual popularity display with half-star support
- **Loading States**: Smooth loading animations
- **Error Handling**: User-friendly error messages

## Customization

### Adding New Products

Edit `Data/products.json` to add new products:

```json
{
  "name": "New Ring",
  "popularityScore": 0.75,
  "weight": 3.2,
  "images": {
    "yellow": "https://example.com/yellow.jpg",
    "rose": "https://example.com/rose.jpg",
    "white": "https://example.com/white.jpg"
  }
}
```

### Styling

Modify `Content/styles.css` to customize:

- Colors and gradients
- Typography (fonts are in `Fonts/` directory)
- Layout and spacing
- Responsive breakpoints

### Gold Price Source

Update `GoldPriceService.cs` to integrate with your preferred gold price API.

## Browser Support

- Chrome 60+
- Firefox 55+
- Safari 12+
- Edge 79+
- Mobile browsers with touch support

## License

This project is for educational/demonstration purposes.
