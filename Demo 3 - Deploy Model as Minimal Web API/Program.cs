using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;

// Configure app
var builder = WebApplication.CreateBuilder(args);

// Register Time Series PredictionEngine service
builder.Services.AddTimeSeriesPredictionEngine("WeatherForecastModel.zip");

var app = builder.Build();

// Define handler
Func<Task<ModelOutput>> predictHandler = async () => 
{
    var engine = app.Services.GetRequiredService<TimeSeriesPredictionEngine<ModelInput,ModelOutput>>();
    return await Task.FromResult(engine.Predict());
};

// Define prediction route
app.MapGet("/predict", predictHandler);

// Run app
app.Run();