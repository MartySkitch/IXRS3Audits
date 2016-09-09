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
	Date: 9-Sep-2016
	Purpose: Give a UserId return the user's contact info
	
	ToDo: This Email contains a CR/LF but it is not shown.  Figure out why

*/

var userId = new Guid("92cd15db-6b63-4621-a483-a67b011094b9");


var user = UserContactInformation
			.Where(u => u.UserId == userId)
			.Dump();