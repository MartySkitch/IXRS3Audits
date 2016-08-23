<Query Kind="Statements">
  <Connection>
    <ID>bfed86d1-0885-452b-8d52-e748a5c23776</ID>
    <Persist>true</Persist>
    <Server>(localDB)\MSSQLLocalDB</Server>
    <Database>IXRS_Almac123456</Database>
  </Connection>
</Query>



var userID = ScreenedSubjects
			.Where(u => u.ExternalId == "3")
			.Select(u => new { UserID = u.SubjectId } );
			
userID.Dump();