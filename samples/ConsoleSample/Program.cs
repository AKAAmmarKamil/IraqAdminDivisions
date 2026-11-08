using System.Text;
using IraqAdminDivisions;

Console.OutputEncoding = Encoding.UTF8;

var subDistrict = IraqDivisions.Find("IQG07Q02N02")!;
var order = new Order(1001, subDistrict.Code);

Console.WriteLine($"Order #{order.Id} stores one column: SubDistrictCode = {order.SubDistrictCode}");
Console.WriteLine();

Console.WriteLine("Address in every language");
foreach (var language in Enum.GetValues<Language>())
{
    Console.WriteLine($"  {language,-8} {IraqDivisions.FormatAddress(order.SubDistrictCode, language)}");
}

Console.WriteLine();
var stored = IraqDivisions.Find(order.SubDistrictCode)!;
Console.WriteLine("Navigation up");
Console.WriteLine($"  Sub-district: {stored.Name.En} ({stored.Code})");
Console.WriteLine($"  District:     {stored.District!.Name.En} ({stored.District.Code})");
Console.WriteLine($"  Governorate:  {stored.Governorate.Name.En} ({stored.Governorate.Code})");
Console.WriteLine($"  Everything in the same governorate starts with '{stored.Governorate.Code}'");

Console.WriteLine();
Console.WriteLine("Search(\"البصره\")");
foreach (var result in IraqDivisions.Search("البصره", limit: 3))
{
    Console.WriteLine($"  {result.Score,3}  {result.Division.Code,-12} {result.Division.Name.Ar} / {result.Division.Name.En}");
}

Console.WriteLine();
var nearest = IraqDivisions.Nearest(30.5, 47.8, AdminLevel.District);
Console.WriteLine($"Nearest district to 30.5, 47.8: {nearest.FormatAddress(Language.English)} ({nearest.Code})");

internal sealed record Order(int Id, string SubDistrictCode);
