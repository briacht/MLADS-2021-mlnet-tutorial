using System;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace TaxiApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Sentiment analysis
            var x = "Y";
            string userInput = "";
            string printedString = "";
            var sentimentInput = new SentimentModelInput();

            // Train sentiment model
            var sentimentModel = trainSentimentModel();

            // Consume sentiment model
            while (x == "Y" || x == "y")
            {
                Console.WriteLine("Write a review of your taxi driver: ");
                sentimentInput.Comment = Console.ReadLine();
                var result = consumeSentimentModel(sentimentModel, sentimentInput);
                if (result.Prediction == "positive") { printedString = "Thanks for the great review! :)"; };

                if (result.Prediction == "negative") { printedString = "We're sorry you had a bad experience. :("; }

                Console.WriteLine($"\n{printedString}");

                Console.WriteLine("Would you like to leave another review (Y/N)?");
            };
                        
        }

        public static ITransformer trainSentimentModel()
        {
            Console.WriteLine("Training model...");
            string taxiDataPath = "yelp_labelled.txt";

            var mlContext = new MLContext();

            IDataView trainingDataView = mlContext.Data.LoadFromTextFile<SentimentModelInput>(
                path: taxiDataPath,
                hasHeader: true,
                separatorChar: ',',
                allowQuoting: true,
                allowSparse: false);

            // Add data transformations to pipeline
            var dataProcessPipeline = mlContext.Transforms.Conversion.MapValueToKey(@"Sentiment", @"Sentiment")
                .Append(mlContext.Transforms.Text.FeaturizeText(@"Comment_tf", @"Comment"))
                .Append(mlContext.Transforms.CopyColumns(@"Features", @"Comment_tf"))
                .Append(mlContext.Transforms.NormalizeMinMax(@"Features", @"Features"));

            // Set the training algorithm 
            var trainer = mlContext.MulticlassClassification.Trainers.OneVersusAll(binaryEstimator: mlContext.BinaryClassification.Trainers.AveragedPerceptron(labelColumnName: @"Sentiment"), labelColumnName: @"Sentiment")
                .Append(mlContext.Transforms.Conversion.MapKeyToValue(@"PredictedLabel", @"PredictedLabel"));

            // Add trainer to pipeline
            var trainingPipeline = dataProcessPipeline.Append(trainer);

            // Train model
            ITransformer sentimentModel = trainingPipeline.Fit(trainingDataView);

            Console.WriteLine("Done training!");

            return sentimentModel;
        }

        public static SentimentModelOutput consumeSentimentModel(ITransformer sentimentModel, SentimentModelInput input)
        {
            MLContext mlContext = new MLContext();

            var predEngine = mlContext.Model.CreatePredictionEngine<SentimentModelInput, SentimentModelOutput>(sentimentModel);

            SentimentModelOutput result = predEngine.Predict(input);

            return result;
        }

        public class SentimentModelInput
        {
            [ColumnName(@"Comment"), LoadColumn(0)]
            public string Comment { get; set; }

            [ColumnName(@"Sentiment"), LoadColumn(0)]
            public string Sentiment { get; set; }
        }

        public class SentimentModelOutput
        {

            [ColumnName("PredictedLabel")]
            public string Prediction { get; set; }

            public float[] Score { get; set; }
        }
    }
}
