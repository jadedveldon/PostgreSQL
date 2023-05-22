using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PostgreSQL;
using PostgreSQL.DataEntities.Models;
using PostgreSQL.DataLayer.Implementations;
using PostgreSQL.DataLayer.Interfaces;
using PostgreSQL.Repository.Implementations;
using PostgreSQL.Repository.Interfaces;


var builder = WebApplication.CreateBuilder(args);

//ADD SERVICES SECTION 

//Functionality: it adds for the controller, basic essential mvc services are added
//things related to data storage and retrivial, data validation, ui rendering, input handling, request handling;
//configures the builder to use api explorer which is used to access various end points;
//adds cors and data annotations for validation
//adds formatting mappers
//When removed: error is thrown at app.UseAuthorisation part
//when position changed: before the middleware pipeline anywhere the order doesn't matter but when added in the pipeline it gives an error
//saying that the servies is in readonly form, by that point the services cant be changed anymore so adding controlers when its built is not possible
builder.Services.AddControllers();



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//functionality: it is added to give additonal informaion for the swagger so that it can function smoothly, it was primarily added for minimal apis
//minimal apis are extremely lightweight apis that work without a controller and its basic endpoints can be mapped without a controller
//as we are having a proper controller and everything this is not necessarily required
//when removed: works normally as expected
builder.Services.AddEndpointsApiExplorer();

//functionality: it is used to add swagger to our application, swagger is used to get a ui which helps in interacting with the webapi and everything
//is configured and handled on its own and no additonal lines of code are required
//when removed: works normally but the ui is not generated anymore
//when position changed: in the servies section makes no difference but in middleware pipeline throws an error saying that servies are readonly 
//servies cant be added at this point
builder.Services.AddSwaggerGen();

//Dependency Injection
//functionality: registers our interfaaces and implementations and then it specifies what implementation is to be looked at when an interface is instantiated
//when removed: throws invalid operation error as the controller has dependency on datalayer interface which has a dependency on repository interface
//when position changed: no change in services section, when added to middleware layer again the same expected error of servies being readonly and cant be added 
builder.Services.AddScoped<IPostgreSQLRepository, PostgreSQLRepository>();
builder.Services.AddScoped<IPostgreSQLDataLayer, PostgreSQLDataLayer>();

// Read the connection string from configuration
//functionality: gets the connection string from appsettings.json
//when removed: cant connect to db as there is no connection string
//when position changed: throws error as the variable has been defined and its declaration needs to come before it is being used
string connectionString = builder.Configuration.GetConnectionString("connectionString");

// Service to connect to db
//functionality: it is used to connect to the data base through the connection string
//when removed: the db doesnt work
//when position changed: in the services section anywhere after variable decleration works, before that variable undefined
//in the middleware it says service is readonly and nothing can be added to it
builder.Services.AddDbContext<TestContext>(options => options.UseNpgsql(connectionString));

//automapper setup
//functionality: to setup automapper and use it in our code
//when removed: cant use automapper
//when position changed: no change in services section, when added to middleware layer again the same expected error of servies being readonly and cant be added 
builder.Services.AddAutoMapper(typeof(WeatherForecast));
//automapper takes a marker for the assembly in which the file which has the mapper configured in it is given, as in this project 
//i have setup the mapper.cs file then any other file in the same library as it can be given to the typeof function.


//MIDDLEWARE PIPELINE

//it contains 

//functionality: builds the application and it returns a built webapplication with all the services added to it
//when positon changed? if anywhere in the servies section then all the servies after this line are not registered
//in the middleware section it doesnt work as the defination app makes no sense for the sentances which come before it
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//functionality:it is to launch and use swagger
//when removed: swagger isnt launched but the api does work normally without it too
//when position changed: has to be used before build only, and anywhere in the pipeline before run works 
    app.UseSwagger();
    app.UseSwaggerUI();
//}

//functionality: it is used to redirect http requests to https This redirection helps enforce secure communication by
//ensuring that all requests are made over an encrypted HTTPS connection instead of an insecure HTTP connection.
//when removed: everything still works but it is now open to attacks from outside as it is on insecue http connection
//when position changed: doesnt matter as long as it is between build and run.
app.UseHttpsRedirection();


//functionality:it adds built in middleware authorization capabilites to the pipeline
app.UseAuthorization();


//functionality: to enable handling incoming requests and generate appropriate responses to those requests. basicslly helps our controller do its job
//when removed: controller doesn't have anything to serve from the response
app.MapControllers();


//functionality: after everything is added set and confiugured then the application is finally run
//when removed: the application isnt launched as its never run 
//when position changed: it has to be the last line only as anything after it is ignored and app is just run without it
app.Run();
