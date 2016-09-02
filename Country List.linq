<Query Kind="Statements">
  <Reference Relative="DLLs\Excel.dll">D:\Skitch\Audits\IXRS 3.0 Audit\DLLs\Excel.dll</Reference>
  <Reference Relative="DLLs\ICSharpCode.SharpZipLib.dll">D:\Skitch\Audits\IXRS 3.0 Audit\DLLs\ICSharpCode.SharpZipLib.dll</Reference>
  <Reference Relative="DLLs\IXRS.CountryManagement.CountryList.dll">D:\Skitch\Audits\IXRS 3.0 Audit\DLLs\IXRS.CountryManagement.CountryList.dll</Reference>
  <Namespace>IXRS.CountryManagement</Namespace>
</Query>

// Using IXRS.CountryManagement.CountryList.dll to get the country list.

CountryProvider countryProvide = new CountryProvider();

var countries = countryProvide.GetAllCountries();

countries.Dump();