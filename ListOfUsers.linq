<Query Kind="Statements">
  <Connection>
    <ID>623e535d-fd44-41dc-a5c5-888da12d3d4e</ID>
    <Persist>true</Persist>
    <Server>SO-DB-AG1.live.ext,3344</Server>
    <Database>IXRS_Global</Database>
    <IsProduction>true</IsProduction>
  </Connection>
</Query>

var user = 
UserStudyAccesses
	.Join(UserContactInformation
		,
		.Where(u => u.LastName.Contains("smith"))
		.Select(e => new {Name = e.FirstName + " " + e.LastName,
							EMail = e.Email });
		
user.Dump();