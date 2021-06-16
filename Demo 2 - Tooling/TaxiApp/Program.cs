using System;
using System.Globalization;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace TaxiApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Taxi Price Prediction
            MLContext mlContext = new MLContext();

            // Load model
            ITransformer taxiFareModel = mlContext.Model.Load("taxi-fare-model.zip", out DataViewSchema modelSchema);

            // Create Prediction Engine
            var predEngine = mlContext.Model.CreatePredictionEngine<TaxiFareModelInput, TaxiFareModelOutput>(taxiFareModel);

            var loop = "Y";
            string printedString = "";
            
            TaxiFareModelInput input = new TaxiFareModelInput();
            ReviewSentimentModel.ModelInput reviewInput = new ReviewSentimentModel.ModelInput();

            // Consume model
            while (loop == "Y" || loop == "y")
            {
                Console.WriteLine("Enter in your ride details: ");
                Console.WriteLine("Trip distance (miles): ");
                input.Trip_distance = float.Parse(Console.ReadLine(), CultureInfo.InvariantCulture.NumberFormat);
                Console.WriteLine("Trip time (seconds): ");
                input.Trip_time_in_secs = float.Parse(Console.ReadLine(), CultureInfo.InvariantCulture.NumberFormat);
                Console.WriteLine("Number of passengers: ");
                input.Passenger_count = float.Parse(Console.ReadLine(), CultureInfo.InvariantCulture.NumberFormat);
                Console.WriteLine("Payment type (CSH or CRD): ");
                input.Payment_type = Console.ReadLine();

                TaxiFareModelOutput result = predEngine.Predict(input);

                Console.WriteLine($"\nYour taxi fare will be ~${result.Score}");

                Console.WriteLine("\nWrite a review of your taxi driver: ");
                
                reviewInput.Comment = Console.ReadLine();
                
                var sentimentResult = ReviewSentimentModel.Predict(reviewInput);

                if (sentimentResult.Prediction == "positive") { printedString = "Thanks for the great review! :)"; };

                if (sentimentResult.Prediction == "negative") { printedString = "We're sorry you had a bad experience. :("; }

                Console.WriteLine($"\n{printedString}");
                
                //Console.WriteLine($"\nML NOT IMPLEMENTED");

                Console.WriteLine("\nWould you like to start over (Y/N)?");
                loop = Console.ReadLine();
            };

        }

        public class TaxiFareModelInput
        {
            [ColumnName("vendor_id"), LoadColumn(0)]
            public string Vendor_id { get; set; }

            [ColumnName("rate_code"), LoadColumn(1)]
            public float Rate_code { get; set; }

            [ColumnName("passenger_count"), LoadColumn(2)]
            public float Passenger_count { get; set; }

            [ColumnName("trip_time_in_secs"), LoadColumn(3)]
            public float Trip_time_in_secs { get; set; }

            [ColumnName("trip_distance"), LoadColumn(4)]
            public float Trip_distance { get; set; }

            [ColumnName("payment_type"), LoadColumn(5)]
            public string Payment_type { get; set; }

            [ColumnName("fare_amount"), LoadColumn(6)]
            public float Fare_amount { get; set; }
        }

        // Define the model output schema
        public class TaxiFareModelOutput
        {
            public float Score { get; set; }
        }
    }
}