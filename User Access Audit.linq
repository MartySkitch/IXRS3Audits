<Query Kind="Program">
  <Connection>
    <ID>d9bf114f-f9d8-4254-965e-d8ad644a4878</ID>
    <Persist>true</Persist>
    <Server>SO-DB-AG1.live.ext,3344</Server>
    <Database>IXRS_Global</Database>
    <IsProduction>true</IsProduction>
  </Connection>
  <NuGetReference>Newtonsoft.JsonResult</NuGetReference>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
</Query>

void Main()
{
	var studyCD = "500006";
	List<AuditTrailResult> auditTrailResults;
	
	// Get the Audit Trail
	var auditTrail = AuditTrails
		.Where (at => at.EntityModifications_EntityName == "UserStudyAccess" 
						&& at.EntityModifications_EntityIds.Contains(studyCD))
		.Select (at => new AuditTrail { DateCreatedUtc = at.DateCreatedUtc,
										EntityModificationsNewValue = at.EntityModifications_NewValues,
										EntityModificationsOldValue = at.EntityModifications_OldValues,
										ModUserId = at.TransactionInformation_User_UserId
										})
		.OrderBy (at => at.DateCreatedUtc);
	
	auditTrailResults = auditTrail.ToList()
		.Select (at => ConvertFromAuditTrail(at) ).ToList();
		
	auditTrailResults.Dump();		
}
	private AuditTrailResult ConvertFromAuditTrail(AuditTrail auditTrail)
	{
		var oldValue = new JObject();
		oldValue = auditTrail.EntityModificationsOldValue == null ? null : JObject.Parse(auditTrail.EntityModificationsOldValue);
		var newValue = JObject.Parse(auditTrail.EntityModificationsNewValue);
		var user = GetUserInfo( new Guid((string)newValue["UserId"]));
		var mod = GetUserInfo(auditTrail.ModUserId);

		var auditTrailResult = new AuditTrailResult
		{
			DateCreatedUtc = auditTrail.DateCreatedUtc,
			isActiveOldValue = oldValue == null ? string.Empty : (string)oldValue["IsActive"],
			isActiveNewValue = (string)newValue["IsActive"],
			userFirstName = user.FirstName,
			userLastName = user.LastName,
			userEmail = user.Email,
			modFirstName = mod == null ? string.Empty : mod.FirstName,
			modLastName = mod == null ? string.Empty : mod.LastName,
			modEmail = mod == null ? string.Empty : mod.Email
		};
		
		return auditTrailResult;
	}	

	private UserContactInformation GetUserInfo(Guid? userID)
	{
		if(userID == null) return null;
		
		var userName = UserContactInformation
			.SingleOrDefault (uci => uci.UserId == userID);
			
		return userName;
	}		

// Define other methods and classes here
    public class AuditTrail
    {
        //public AuditTrail();
        public DateTime DateCreatedUtc { get; set; }
        public string EntityModificationsNewValue { get; set; }
        public string EntityModificationsOldValue { get; set; }		
        public Guid? ModUserId { get; set; }
    }
	
	public class AuditTrailResult
	{
		public DateTime DateCreatedUtc { get; set; }
		public string userFirstName { get; set; }
		public string userLastName { get; set; }
		public string userEmail { get; set; }
		public string isActiveOldValue { get; set; }
		public string isActiveNewValue { get; set; }
		public string modFirstName { get; set; }
		public string modLastName { get; set; }
		public string modEmail { get; set; }		
	}
