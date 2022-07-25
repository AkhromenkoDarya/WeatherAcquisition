using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WeatherAcquisition.DAL.Context;
using WeatherAcquisition.DAL.Entities;

namespace WeatherAcquisition.API.Data
{
    public class DataDbInitializer
    {
        private readonly DataDb _db;

        public DataDbInitializer(DataDb db) => _db = db;

        public void Initialize()
        {
            _db.Database.Migrate();

            if (_db.Sources.Any())
            {
                return;
            }

            var random = new Random();

            for (var i = 1; i <= 20; i++)
            {
                var source = new DataSource
                {
                    Name = $"Source {i}",
                    Description = $"Test data source № {i}"
                };

                _db.Sources.Add(source);

                var values = new DataValue[random.Next(10, 30)];

                for ((var j, int count) = (0, values.Length); j < count; j++)
                {
                    var value = new DataValue
                    {
                        Source = source,
                        Time = DateTimeOffset.Now.AddDays(-random.Next(0, 365)),
                        Value = $"{random.Next(0, 30)}"
                    };

                    //_db.Values.Add(value);

                    values[j] = value;
                }

                _db.Values.AddRange(values);
            }

            _db.SaveChanges();
        }
    }
}
