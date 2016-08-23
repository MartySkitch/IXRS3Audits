<Query Kind="Statements">
  <Reference>&lt;RuntimeDirectory&gt;\System.dll</Reference>
  <Namespace>System</Namespace>
</Query>



var timeQuery = TimeZoneInfo.GetSystemTimeZones(); //.Select(zoneInfo => new { Value = zoneInfo.Id,  Text = zoneInfo.DisplayName });

timeQuery.Dump();