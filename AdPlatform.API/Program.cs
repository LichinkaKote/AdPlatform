using AdPlatform.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//варианты поисковиков на выбор: дерево либо словарь
builder.Services.AddSingleton<IStorage, InMemoryTrieAdStorage>(); //поиск через префиксное дерево
//builder.Services.AddSingleton<IStorage, InMemoryAdStorage>(); //вариант поисковика через словарь Dictionary

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
