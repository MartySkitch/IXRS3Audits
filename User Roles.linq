<Query Kind="Program">
  <Connection>
    <ID>b8c1db0b-25cc-4f90-b47d-d3d8a7ceb28b</ID>
    <Persist>true</Persist>
    <Server>SO-DB-AG1.live.ext,3344</Server>
    <Database>IXRS_Global</Database>
    <LinkedDb>IXRS_LEADISSS20101</LinkedDb>
  </Connection>
  <Output>DataGrids</Output>
  <NuGetReference>Newtonsoft.JsonResult</NuGetReference>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
</Query>

void Main()
{
	//List<AuditTrailResult> auditTrailResults;
	
	//Get the Audit Trail
	var auditTrail = IXRS_LEADISSS20101.AuditTrails
		.Where (at => at.EntityModifications_EntityName == "UserRole")
		.Select (at => new AuditTrail { DateCreatedUtc = at.DateCreatedUtc,
										EntityModificationsNewValue = at.EntityModifications_NewValues,
										EntityModificationsOldValue = at.EntityModifications_OldValues,
										ModUserId = at.TransactionInformation_User_UserId,
										OperationType = at.EntityModifications_OperationType
										})
		.OrderBy (at => at.DateCreatedUtc);
		
	auditTrail.Count().Dump();
	
	var auditTrailResults = auditTrail
		.Select (at => ConvertFromAuditTrail(at) );
				
	auditTrailResults.ToList().Where (tr => tr.UserEmail == "npsapps1690@gmail.com" ).Dump();
}

	private AuditTrailResult ConvertFromAuditTrail(AuditTrail auditTrail)
	{
		//var oldValue = new JObject();
		var oldValue = auditTrail.EntityModificationsOldValue == null ? null : JObject.Parse(auditTrail.EntityModificationsOldValue);
		var newValue = auditTrail.EntityModificationsNewValue == null ? null : JObject.Parse(auditTrail.EntityModificationsNewValue);
		var user =  oldValue == null ? GetUserInfo( new Guid((string)newValue["UserId"])) : GetUserInfo( new Guid((string)oldValue["UserId"])) ;
		var mod = GetUserInfo(auditTrail.ModUserId);
		var roleName =  oldValue == null ? GetRoleName( new Guid((string)newValue["RoleId"])) : GetRoleName( new Guid((string)oldValue["RoleId"])) ;
		var opType = "Insert";
		
		switch (auditTrail.OperationType)
		{
			case 0: 
				opType = "Insert";
				break;
			case 1: 
				opType = "Delete";
				break;
			case 2:
				opType = "Update";
				break;
			default:
				opType = "N/A";
				break;
		}

	// need to account for users in the new vs. old value
		var auditTrailResult = new AuditTrailResult
		{
			DateCreatedUtc = auditTrail.DateCreatedUtc,
			OperationType = opType,
			RoleName = roleName,
			UserFirstName = user == null ? string.Empty : user.FirstName,
			UserLastName = user == null ? string.Empty : user.LastName,
			UserEmail = user == null ? string.Empty : user.Email,
			ModFirstName = mod == null ? string.Empty : mod.FirstName,
			ModLastName = mod == null ? string.Empty : mod.LastName,
			ModEmail = mod == null ? string.Empty : mod.Email
		};
		
		return auditTrailResult;
	}	

	private UserContactInformation GetUserInfo(Guid? userID)
	{
		var userName = UserContactInformation
			.SingleOrDefault (uci => uci.UserId == userID);
			
		return userName;
	}
	
	private string GetRoleName(Guid roleId)
	{
		var roleName = IXRS_LEADISSS20101.Roles
			.SingleOrDefault (r => r.Id == roleId)
			.Name;
			
		return roleName;
	}

// Define other methods and classes here
    public class AuditTrail
    {
        //public AuditTrail();
        public DateTime DateCreatedUtc { get; set; }
        public string EntityModificationsNewValue { get; set; }
        public string EntityModificationsOldValue { get; set; }
		public int OperationType  { get; set; }
        public Guid? ModUserId { get; set; }
    }
	
	public class AuditTrailResult
	{
		public DateTime DateCreatedUtc { get; set; }
		public string OperationType  { get; set; }
		public string RoleName { get; set; }
		public string UserFirstName { get; set; }
		public string UserLastName { get; set; }
		public string UserEmail { get; set; }		
		public string ModFirstName { get; set; }	
		public string ModLastName { get; set; }
		public string ModEmail { get; set; }		
	}