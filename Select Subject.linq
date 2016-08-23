<Query Kind="Statements">
  <Connection>
    <ID>bfed86d1-0885-452b-8d52-e748a5c23776</ID>
    <Persist>true</Persist>
    <Server>(localDB)\MSSQLLocalDB</Server>
    <Database>IXRS_Almac123456</Database>
  </Connection>
</Query>



    var query = 
		ScreenedSubjects
			.Select(s => s);
			//.Where(s => s.ExternalId == "1" || s.ExternalId == "2");
	query.Dump();
	query = query.OrderByDescending(o => o.ExternalId);
	query.Dump();
	query = query.Where(s => s.ExternalId == "1");
	
	query.Dump();
	var query2 = query.Select(s => new { s.SubjectId, s.ExternalId } );
				
	query2.Dump();