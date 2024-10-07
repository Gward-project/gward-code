using Gwards.Api.Services.Common;

namespace Gwards.Api.Utils;

public class MetricsUtil
{
    private readonly FileStorageService _fileStorage;

    public MetricsUtil(FileStorageService fileStorage)
    {
        _fileStorage = fileStorage;
    }

    public SortedDictionary<double, double> LoadMetric(string metricName)
    {
        var result = new SortedDictionary<double, double>();
        using var file = _fileStorage.Open($"Metrics/{metricName}.csv");
        using var streamReader = new StreamReader(file);
        
        while (!streamReader.EndOfStream)
        {
            var line = streamReader.ReadLine();
            if (line == null)
            {
                throw new ApplicationException("Invalid metrics file was provided");
            }
            
            var values = line.Split(';');
            if (values.Length != 2)
            {
                throw new ApplicationException("Invalid metrics file was provided");
            }

            if (!double.TryParse(values[0], out var coefficient))
            {
                throw new ApplicationException("Metrics coefficients must be valid decimal numbers");
            }
            
            if (!double.TryParse(values[1], out var value))
            {
                throw new ApplicationException("Metrics values must be valid decimal numbers");
            }

            result[value] = coefficient;
        }

        return result;
    }
}