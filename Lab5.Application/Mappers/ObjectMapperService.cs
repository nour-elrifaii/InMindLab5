using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Lab5.Application.Mappers
{

    public class ObjectMapperService
    {
        public TDestination Map<Tsource, TDestination>(Tsource source) where TDestination : new()
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var destination = new TDestination();
            Type sourceType = source.GetType();
            Type destinationType = typeof(TDestination);

            PropertyInfo[] destinationProperties = destinationType.GetProperties();
            PropertyInfo[] sourceProperties = sourceType.GetProperties();

            foreach (var sourceProperty in sourceProperties)
            {
                var destinationProperty = destinationProperties.FirstOrDefault(p =>
                    p.Name.Equals(sourceProperty.Name, StringComparison.OrdinalIgnoreCase)
                    && p.PropertyType.Equals(sourceProperty.PropertyType));
                if (destinationProperty != null && destinationProperty.CanWrite)
                {
                    destinationProperty.SetValue(destination, sourceProperty.GetValue(source));
                }
                else
                {
                    try
                    {
                        destinationProperty.SetValue(destination,
                            Convert.ChangeType(sourceProperty.GetValue(source), destinationProperty.PropertyType));
                    }
                    catch
                    {

                    }
                }
            }

            return destination;
        }

    }
}