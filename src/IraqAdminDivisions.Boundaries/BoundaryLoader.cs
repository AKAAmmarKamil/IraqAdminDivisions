using System.Text.Json;

namespace IraqAdminDivisions.Boundaries;

internal static class BoundaryLoader
{
    private const string ResourceName = "IraqAdminDivisions.boundaries.json";
    private const string DivisionsProperty = "divisions";
    private const string CodeProperty = "code";
    private const string PolygonsProperty = "polygons";

    public static BoundaryData Load()
    {
        using var stream = typeof(BoundaryLoader).Assembly.GetManifestResourceStream(ResourceName)
            ?? throw new InvalidOperationException($"Embedded resource '{ResourceName}' is missing.");
        using var buffer = new MemoryStream();
        stream.CopyTo(buffer);
        return Parse(buffer.ToArray());
    }

    internal static BoundaryData Parse(byte[] json)
    {
        var builder = new Builder();
        var reader = new Utf8JsonReader(json, isFinalBlock: true, state: default);
        ExpectToken(ref reader, JsonTokenType.StartObject);
        while (reader.Read() && reader.TokenType == JsonTokenType.PropertyName)
        {
            if (reader.ValueTextEquals(DivisionsProperty))
            {
                ReadDivisions(ref reader, builder);
            }
            else
            {
                reader.Read();
                reader.Skip();
            }
        }

        return builder.Build();
    }

    private static void ReadDivisions(ref Utf8JsonReader reader, Builder builder)
    {
        ExpectToken(ref reader, JsonTokenType.StartArray);
        while (reader.Read() && reader.TokenType == JsonTokenType.StartObject)
        {
            var divisionIndex = -1;
            while (reader.Read() && reader.TokenType == JsonTokenType.PropertyName)
            {
                if (reader.ValueTextEquals(CodeProperty))
                {
                    reader.Read();
                    divisionIndex = builder.AddCode(reader.GetString()!);
                }
                else if (reader.ValueTextEquals(PolygonsProperty))
                {
                    ReadPolygons(ref reader, builder, divisionIndex);
                }
                else
                {
                    reader.Read();
                    reader.Skip();
                }
            }
        }
    }

    private static void ReadPolygons(ref Utf8JsonReader reader, Builder builder, int divisionIndex)
    {
        ExpectToken(ref reader, JsonTokenType.StartArray);
        while (reader.Read() && reader.TokenType == JsonTokenType.StartArray)
        {
            builder.BeginPolygon(divisionIndex);
            while (reader.Read() && reader.TokenType == JsonTokenType.StartArray)
            {
                builder.BeginRing();
                while (reader.Read() && reader.TokenType == JsonTokenType.StartArray)
                {
                    reader.Read();
                    var longitude = reader.GetDouble();
                    reader.Read();
                    var latitude = reader.GetDouble();
                    reader.Read();
                    builder.AddPoint(longitude, latitude);
                }

                builder.EndRing();
            }

            builder.EndPolygon();
        }
    }

    private static void ExpectToken(ref Utf8JsonReader reader, JsonTokenType expected)
    {
        if (!reader.Read() || reader.TokenType != expected)
        {
            throw new InvalidDataException($"Boundary data is malformed: expected {expected}.");
        }
    }

    private sealed class Builder
    {
        private readonly List<string> _codes = new();
        private readonly List<double> _coordinates = new(200_000);
        private readonly List<int> _ringStarts = new();
        private readonly List<int> _ringLengths = new();
        private readonly List<int> _polygonFirstRings = new();
        private readonly List<int> _polygonRingCounts = new();
        private readonly List<int> _polygonDivisions = new();
        private readonly List<double> _polygonBounds = new();
        private readonly List<double> _polygonAreas = new();
        private double _minLng, _minLat, _maxLng, _maxLat;
        private int _ringStartOffset;
        private int _polygonFirstRing;
        private double _outerArea;
        private double _holeArea;

        public int AddCode(string code)
        {
            _codes.Add(code);
            return _codes.Count - 1;
        }

        public void BeginPolygon(int divisionIndex)
        {
            _polygonDivisions.Add(divisionIndex);
            _polygonFirstRing = _ringStarts.Count;
            _polygonFirstRings.Add(_polygonFirstRing);
            _minLng = _minLat = double.MaxValue;
            _maxLng = _maxLat = double.MinValue;
            _outerArea = 0;
            _holeArea = 0;
        }

        public void BeginRing()
        {
            _ringStartOffset = _coordinates.Count;
        }

        public void AddPoint(double longitude, double latitude)
        {
            _coordinates.Add(longitude);
            _coordinates.Add(latitude);
            if (_ringStarts.Count == _polygonFirstRing)
            {
                _minLng = Math.Min(_minLng, longitude);
                _maxLng = Math.Max(_maxLng, longitude);
                _minLat = Math.Min(_minLat, latitude);
                _maxLat = Math.Max(_maxLat, latitude);
            }
        }

        public void EndRing()
        {
            var length = (_coordinates.Count - _ringStartOffset) / 2;
            var area = Math.Abs(SignedArea(_ringStartOffset, length));
            if (_ringStarts.Count == _polygonFirstRing)
            {
                _outerArea = area;
            }
            else
            {
                _holeArea += area;
            }

            _ringStarts.Add(_ringStartOffset);
            _ringLengths.Add(length);
        }

        public void EndPolygon()
        {
            _polygonRingCounts.Add(_ringStarts.Count - _polygonFirstRing);
            _polygonBounds.Add(_minLng);
            _polygonBounds.Add(_minLat);
            _polygonBounds.Add(_maxLng);
            _polygonBounds.Add(_maxLat);
            _polygonAreas.Add(_outerArea - _holeArea);
        }

        public BoundaryData Build() => new()
        {
            Codes = _codes.ToArray(),
            Coordinates = _coordinates.ToArray(),
            RingStarts = _ringStarts.ToArray(),
            RingLengths = _ringLengths.ToArray(),
            PolygonFirstRings = _polygonFirstRings.ToArray(),
            PolygonRingCounts = _polygonRingCounts.ToArray(),
            PolygonDivisions = _polygonDivisions.ToArray(),
            PolygonBounds = _polygonBounds.ToArray(),
            PolygonAreas = _polygonAreas.ToArray()
        };

        private double SignedArea(int start, int length)
        {
            var sum = 0.0;
            for (int i = 0, j = length - 1; i < length; j = i++)
            {
                var xi = _coordinates[start + 2 * i];
                var yi = _coordinates[start + 2 * i + 1];
                var xj = _coordinates[start + 2 * j];
                var yj = _coordinates[start + 2 * j + 1];
                sum += xj * yi - xi * yj;
            }

            return sum / 2;
        }
    }
}
