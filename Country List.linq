<Query Kind="Statements">
  <Reference>D:\Projects\IXRNext\Main\Global\packages\ExcelDataReader.2.1.2.3\lib\net45\Excel.dll</Reference>
  <Reference>D:\Projects\IXRNext\Main\Global\packages\SharpZipLib.0.86.0\lib\20\ICSharpCode.SharpZipLib.dll</Reference>
  <Reference>D:\Projects\IXRNext\Main\Global\packages\IXRS.CountryManagement.CountryList.1.0.0\lib\net45\IXRS.CountryManagement.CountryList.dll</Reference>
  <Namespace>IXRS.CountryManagement</Namespace>
</Query>


// Using IXRS.CountryManagement.CountryList.dll to get the country list.

CountryProvider countryProvide = new CountryProvider();

var countries = countryProvide.GetAllCountries();

countries.Dump();