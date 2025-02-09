using Services.Services;
using Services.strategies.PDFStrategies;

var builder = WebApplication.CreateBuilder(args);



var ollamaApiUri = new Uri("http://localhost:11434");


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IOllamaService>(provider =>
{
    return new OllamaService(ollamaApiUri);
});
builder.Services.AddScoped<IPdfStrategyContext, PdfStrategyContext>();
builder.Services.AddScoped<IPDFStrategy, AsposePDFSt>();
builder.Services.AddScoped<IAsposePDFService, AsposePDFService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
