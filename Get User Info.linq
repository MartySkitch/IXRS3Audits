<Query Kind="Program">
  <Connection>
    <ID>d9bf114f-f9d8-4254-965e-d8ad644a4878</ID>
    <Persist>true</Persist>
    <Server>SO-DB-AG1.live.ext,3344</Server>
    <Database>IXRS_Global</Database>
    <IsProduction>true</IsProduction>
  </Connection>
</Query>

void Main()
{
	Guid userID = new Guid("6e44f259-6002-4999-8360-a5c900ecf750");
	
	var userName = GetUserInfo(userID);
		
	(userName.FirstName + " " + userName.LastName).Dump();
}

	private UserContactInformation GetUserInfo(Guid userID)
	{
		var userName = UserContactInformation
			.SingleOrDefault (uci => uci.UserId == userID);
			
		return userName;
	}		

// Define other methods and classes here
