﻿// This file was auto-generated by ML.NET Model Builder. 
using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace TaxiApp
{
    public partial class TaxiFareModel
    {
        /// <summary>
        /// model input class for TaxiFareModel.
        /// </summary>
        #region model input class
        public class ModelInput
        {
            [ColumnName(@"vendor_id")]
            public string Vendor_id { get; set; }

            [ColumnName(@"rate_code")]
            public float Rate_code { get; set; }

            [ColumnName(@"passenger_count")]
            public float Passenger_count { get; set; }

            [ColumnName(@"trip_time_in_secs")]
            public float Trip_time_in_secs { get; set; }

            [ColumnName(@"trip_distance")]
            public float Trip_distance { get; set; }

            [ColumnName(@"payment_type")]
            public string Payment_type { get; set; }

            [ColumnName(@"fare_amount")]
            public float Fare_amount { get; set; }

        }

        #endregion

        /// <summary>
        /// model output class for TaxiFareModel.
        /// </summary>
        #region model output class
        public class ModelOutput
        {
            public float Score { get; set; }
        }
        #endregion

        private static string MLNetModelPath = Path.GetFullPath("TaxiFareModel.zip");

        /// <summary>
        /// Use this method to predict on <see cref="ModelInput"/>. You can use <see cref="GetSampleData"/> to create a sample <see cref="ModelInput"/>.
        /// </summary>
        /// <param name="input">model input.</param>
        /// <returns><seealso cref=" ModelOutput"/></returns>
        public static ModelOutput Predict(ModelInput input)
        {
            MLContext mlContext = new MLContext();

            // Load model & create prediction engine
            ITransformer mlModel = mlContext.Model.Load(MLNetModelPath, out var modelInputSchema);
            var predEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);

            ModelOutput result = predEngine.Predict(input);
            return result;
        }
    }
}
