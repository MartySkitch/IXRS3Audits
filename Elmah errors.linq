<Query Kind="Statements">
  <Connection>
    <ID>9f303b25-1da2-4249-9e2d-8cf2a4fff3df</ID>
    <Persist>true</Persist>
    <Server>SO-DB-AG1.live.ext,3344</Server>
    <Database>IXRS_ROGNEO3008101</Database>
    <IsProduction>true</IsProduction>
  </Connection>
</Query>

/*
  This gives the same result as SQL DateAdd(minute, -(timeToLookBack), GetUTCDate() )

*/

var timeToLookBackMin = 39480;
// user double quotes "" to escape quote
var errorMessageToFind = @"A potentially dangerous Request.Form value was detected from the client (wresult=""<t:RequestSecurityTo..."").";

var elmahErrors = ELMAH_Errors
		.Where(s => s.StatusCode == 500 
			&& s.Message.Contains(errorMessageToFind)
			&& (s.TimeUtc > DateTime.UtcNow.AddMinutes( -1*(timeToLookBackMin) )))
		.Count();
		
elmahErrors.Dump();