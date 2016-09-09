<Query Kind="Statements">
  <Connection>
    <ID>623e535d-fd44-41dc-a5c5-888da12d3d4e</ID>
    <Persist>true</Persist>
    <Server>SO-DB-AG1.live.ext,3344</Server>
    <Database>IXRS_Global</Database>
    <IsProduction>true</IsProduction>
  </Connection>
</Query>

/* 	Programmer: MFS
	Date: ??-??-2016
	Purpose: Give a user's name return list of active studies
	
	ToDo: Clean up the display

*/

var user = 
UserStudyAccesses
	.Join(UserContactInformation
		,userAccess => userAccess.UserId
		,contactInfo => contactInfo.UserId,
		(userAccess, contactInfo) => new { userAccess, contactInfo } )
		.Where(u => u.contactInfo.LastName.Contains("smith"))
		.Select(e => new {Name = e.contactInfo.FirstName + " " + e.contactInfo.LastName,
							EMail = e.contactInfo.Email, e.userAccess });
		
user.Dump();