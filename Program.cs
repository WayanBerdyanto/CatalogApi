using CatalogAPI.DAL;
using CatalogAPI.DAL.Interfaces;
using CatalogAPI.DTO;
using CatalogAPI.DTO.Product;
using CatalogAPI.Models;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure ProductDapper
builder.Services.AddScoped<IProduct, ProductDapper>();

// Configure DetailDapper
builder.Services.AddScoped<IJoinTable, DetailDapper>();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Version = "v1",
            Title = "Catalog API",
            Description = "Simple API documentation for OpenApi | DESAIN ARSITEKTUR MICROSERVICES",
            Contact = new OpenApiContact
            {
                Name = "Wayan Berdyanto",
                Url = new Uri("https://www.linkedin.com/in/wayanberdyanto/")
            },
            License = new OpenApiLicense
            {
                Name = "Github",
                Url = new Uri("https://www.linkedin.com/in/wayanberdyanto/")
            }
        }
    );
});
var app = builder.Build();

// Get Product
app.MapGet("/api/product", (IProduct product) =>
{
    List<ProductDto> productDto = new List<ProductDto>();
    var products = product.GetAll();
    if (!products.Any())
    {
        return Results.NotFound(new { error = true, message = "Data Kosong" });
    }
    foreach (var data in products)
    {
        productDto.Add(new ProductDto
        {
            ProductID = data.ProductID,
            CategoryID = data.CategoryID,
            Name = data.Name,
            Description = data.Description,
            Price = data.Price,
            Quantity = data.Quantity,
        });
    }
    return Results.Ok(new { success = true, message = "request data successful", data = productDto });
}).WithOpenApi();

// Get Product By Id
app.MapGet("/api/productById/{id}", (IProduct products, int id) =>
{
    ProductDto productDto = new ProductDto();
    var product = products.GetByID(id);
    if (product == null)
    {
        return Results.NotFound(new { error = true, message = "Id Tidak Ditemukan" });
    }
    productDto.ProductID = product.ProductID;
    productDto.CategoryID = product.CategoryID;
    productDto.Name = product.Name;
    productDto.Description = product.Description;
    productDto.Price = product.Price;
    productDto.Quantity = product.Quantity;
    return Results.Ok(new { success = true, message = "request data successful", data = productDto });
}).WithOpenApi();

// Get Product By name
app.MapGet("/api/product/search/{name}", (IProduct products, string name) =>
{
    List<ProductDto> productDto = new List<ProductDto>();
    var product = products.GetByName(name);
    if (!product.Any())
    {
        return Results.NotFound(new { error = true, message = "Nama Tidak Ditemukan" });
    }
    foreach (var data in product)
    {
        productDto.Add(new ProductDto
        {
            ProductID = data.ProductID,
            CategoryID = data.CategoryID,
            Name = data.Name,
            Description = data.Description,
            Price = data.Price,
            Quantity = data.Quantity,
        });
    }
    return Results.Ok(new { success = true, message = "request data successful", data = productDto });
}).WithOpenApi();

// Post Product
app.MapPost("/api/product", (IProduct productDal, CreateProductDto productDto) =>
{
    try
    {
        Product product = new Product
        {
            CategoryID = productDto.CategoryID,
            Name = productDto.Name,
            Description = productDto.Description,
            Price = productDto.Price,
            Quantity = productDto.Quantity,
        };
        productDal.Insert(product);

        //return 201 Created
        return Results.Created($"/api/product/{product.CategoryID}", product);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

// Put Product
app.MapPut("/api/product", (IProduct productDal, ProductDto productDto) =>
{
    try
    {
        var product = new Product
        {
            ProductID = productDto.ProductID,
            CategoryID = productDto.CategoryID,
            Name = productDto.Name,
            Description = productDto.Description,
            Price = productDto.Price,
            Quantity = productDto.Quantity,
        };
        productDal.Update(product);
        return Results.Ok(new { success = true, message = "request update successful", data = product });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

// Delete Product
app.MapDelete("/api/product/{id}", (IProduct productDal, int id) =>
{
    try
    {
        productDal.Delete(id);
        return Results.Ok(new { success = true, message = "request delete successful" });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

// Get Detail Product
app.MapGet("/api/detailsProduct/", (IJoinTable detail) =>
{
    List<DetailDto> detailsDto = new List<DetailDto>();

    var details = detail.GetDetailProducts();

    if (!details.Any())
    {
        return Results.NotFound(new { error = true, message = "Data Kosong" });
    }
    foreach (var data in details)
    {
        detailsDto.Add(new DetailDto
        {
            CategoryID = data.CategoryID,
            Name = data.Name,
            Description = data.Description,
            Price = data.Price,
            Quantity = data.Quantity,
            CategoryName = data.CategoryName,
        });
    }
    return Results.Ok(new { success = true, message = "request data successful", data = detailsDto });
}).WithOpenApi();

// Get Detail Search
app.MapGet("/api/detailsProduct/search/{name}", (IJoinTable details, string name) =>
{
    List<DetailDto> detailDto = new List<DetailDto>();
    var product = details.GetByName(name);
    if (!product.Any())
    {
        return Results.NotFound(new { error = true, message = "Nama Tidak Ditemukan" });
    }
    foreach (var data in product)
    {
        detailDto.Add(new DetailDto
        {
            CategoryID = data.CategoryID,
            Name = data.Name,
            Description = data.Description,
            Price = data.Price,
            Quantity = data.Quantity,
            CategoryName = data.CategoryName,
        });
    }
    return Results.Ok(new { success = true, message = "request data successful", data = detailDto });
}).WithOpenApi();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}