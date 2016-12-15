<Query Kind="Statements">
  <Connection>
    <ID>15bd32a7-e5fe-4344-bd6b-1aa2fff9dd8c</ID>
    <Persist>true</Persist>
    <Server>US-SO-PRD-SPPS1</Server>
    <Database>Smart</Database>
    <IsProduction>true</IsProduction>
  </Connection>
</Query>

var studies = T_Projects
	.Where(c=> c.SolomonCode.Contains("ROCHEO3001301")
				//&& c.Active == true
				&& c.EndDate > DateTime.Now
				&& c.OnlineDate < DateTime.Now
				)
	.OrderBy(m => m.StudyCode)
	.Select(s=> new { s.Client, s.ProjectCode, s.ProtocolNo, DatabaseName = s.SolomonCode, s.StudyCode,
						s.ProjectManager, s.ProjectSpecialist, s.OnlineDate, s.EndDate, s.Active });
	
studies.Dump();