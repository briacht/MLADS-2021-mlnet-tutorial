# Weather forecast model

This is a sample application that shows how to use [.NET Interactive](https://github.com/dotnet/interactive) to train a weather forecasting model for maximum temperatures in Seattle, WA. Once trained, the model is exposed as a web service for predictions inside an ASP.NET Core Minimal Web API.

## Prerequisites

- Notebook
    - [.NET 5 SDK](https://dotnet.microsoft.com/download/dotnet/5.0)
    - [Visual Studio Code](https://code.visualstudio.com/)
    - [.NET Interactive Notebooks Visual Studio Code extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.dotnet-interactive-vscode)
- Web API
    - [.NET 6 SDK Preview 4](https://dotnet.microsoft.com/download/dotnet/6.0)

## About the data

The data used for this model is from [NOAA](https://www.noaa.gov/). It contains 10 years worth of weather data for the Seattle Boeing Field weather station in Seattle, WA. The data looks like the following.

|STATION|NAME|LATITUDE|LONGITUDE|ELEVATION|DATE|TMAX|TMIN|
|---|---|---|---|---|---|---|---|
|USW00024234|"SEATTLE BOEING FIELD, WA US"|47.53028|-122.30083|6.1|4/1/2010|51|41|
|USW00024234|"SEATTLE BOEING FIELD, WA US"|47.53028|-122.30083|6.1|4/2/2010|53|41|
|USW00024234|"SEATTLE BOEING FIELD, WA US"|47.53028|-122.30083|6.1|4/3/2010|50|39|

## Getting the data

1. Find a station https://www.ncdc.noaa.gov/cdo-web/datatools/findstation. The data contains the **Daily Summaries** dataset for the **Air Temperature** categories.

    ![image](https://user-images.githubusercontent.com/46974588/116326383-50f17f80-a792-11eb-9c66-3dabef398889.png)

1. View cart and export **Custom GHCN-Daily CSV** format.

    ![image](https://user-images.githubusercontent.com/46974588/116326449-78484c80-a792-11eb-8061-9c87bb6fc856.png)

1. Submit request for data with the following options.
    - [x] Station Name
    - Units: **Standard**
    - [x] Air Temperature
        - [ ] Average temperature (TAVG)
        - [x] Maximum temperature (TMAX)
        - [x] Minimum temperature (TMIN)

    ![image](https://user-images.githubusercontent.com/46974588/116326560-bd6c7e80-a792-11eb-8050-18f7f85193a5.png)
    
    After a few minutes, expect an e-mail with your dataset.

1. Save your dataset to the *Data* directory.

## Training the model

This sample uses the ML .NET [ForecastBySsa](https://docs.microsoft.com/en-us/dotnet/api/microsoft.ml.timeseriescatalog.forecastbyssa?view=ml-dotnet) trainer to train a univariate time-series forecasting model for maximum temperatures.

To train the model:

1. Open the *weather-mlnet-model-train.ipynb* notebook.
1. Update the `dataPath` variable with the name of your file if called something other than *seattle-10yr.csv*.
1. Run all the cells in the notebook.

Once all the cells are run, it should generate the model and save it to a file called *WeatherForecastModel.zip*.

## About the Web API

Once the model is trained, it's hosted inside a .NET 6 ASP.NET Core Minimal Web API which also leverages some preview C# language features. The Web API consists of 3 files:

- *Schema.cs*: Defines the `ModelInput` and `ModelOutput` classes specifying the schema of what the model expects for input and what it outputs as a prediction.  
- *TimeSeriesExtension.cs*: Helper `IServiceCollection` extension methods to load the trained time series forecasting model and register it using dependency injection.
- *Program.cs*: Web API configuration and routing logic.

## Making predictions

1. Start the Web API project.
2. Using a REST API client of your choice, make an HTTP `GET` request to `http://localhost:5000/predict`.

The response should look similar like the following:

```json
{
    "forecastTemp": [
        80.7973,
        82.15604,
        83.125435,
        83.9756,
        84.39549
    ],
    "lowerBoundTemp": [
        72.84004,
        71.66275,
        70.07037,
        68.60337,
        66.69504
    ],
    "upperBoundTemp": [
        88.75456,
        92.64932,
        96.1805,
        99.34783,
        102.09595
    ]
}
```

In this case, a forecast of the maximum temperature for the next 5 days is generated and stored in the `forecastTemp` property. Since no forecast is 100% accurate, the upper and lower bounds provide a range by which the actual forecast may vary.

Note that this forecast is generated at a point in time when the model was trained. As you get new weather measurements for subsequent days, you want to update your model with the actual values observed to generate a more reliable forecast.