using System;
using Microsoft.ML.Data;

public class ModelInput
{
    [LoadColumn(6)]
    public DateTime Date { get; set; }

    [LoadColumn(7)]
    public float MaxTemp { get; set; }
}

public class ModelOutput
{
    // Maximum Temperature (Farenheit). Each element is a day in the future it's forecasting for
    public float[] ForecastTemp { get; set; }

    public float[] LowerBoundTemp { get; set; }

    public float[] UpperBoundTemp { get; set; }
}