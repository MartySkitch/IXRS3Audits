<Query Kind="Program">
  <Connection>
    <ID>bfed86d1-0885-452b-8d52-e748a5c23776</ID>
    <Persist>true</Persist>
    <Server>(localDB)\MSSQLLocalDB</Server>
    <Database>IXRS_Almac123456</Database>
  </Connection>
  <IncludePredicateBuilder>true</IncludePredicateBuilder>
</Query>

void Main()
{
	var subID = GetSubjectId("2");
	var siteID = GetSiteId("100");
	var kitID = GetKitId("1034");
	
	var auditTrailRecords = AuditTrails
		.Join(AuditTrailRootEntityIds,
			at => at.Id,
			root => root.AuditTrailId,
			(at, root) => new {at.Id, at.EntityModifications_EntityName, at.EntityModifications_NewValues, at.EntityModifications_OldValues, at.EntityModifications_OperationType,
							at.DateCreatedUtc, EntityID = root.Id, root.AuditTrailId, root.RootEntityId_Value, root.RootEntityId_RootEntityIdName   } )
//			(at, root) => { new AuditTrailRecord(at.Id, at.EntityModifications_EntityName, at.EntityModifications_NewValues, 
//								at.EntityModifications_OldValues, at.EntityModifications_OperationType,
//								at.DateCreatedUtc, root.Id, root.AuditTrailId, root.RootEntityId_Value, 
//								root.RootEntityId_RootEntityIdName); }    )
		.AsEnumerable()
		.Select(x =>  new  AuditTrailRecord(x.Id, x.EntityModifications_EntityName, x.EntityModifications_NewValues, 
								x.EntityModifications_OldValues, x.EntityModifications_OperationType,
								x.DateCreatedUtc, x.Id, x.AuditTrailId, x.RootEntityId_Value, 
								x.RootEntityId_RootEntityIdName)    )
		.Where(areId => (areId.RootEntityIdName == "SubjectId" && areId.RootEntityId == subID)
							//| (areId.RootEntityIdName == "SiteId" && areId.RootEntityId == siteID) 
							|| (areId.RootEntityIdName == "KitId" && areId.RootEntityId == kitID) 
							);

								
	

	subID.Dump();
	siteID.Dump();
	//auditTrailRecords.Where (predicate).Dump();
	auditTrailRecords.Dump();
	
}

// Define other methods and classes here
        private Guid? GetSubjectId(string externalId)
        {
            if (string.IsNullOrWhiteSpace(externalId))
                return null;
				
            var subject = ScreenedSubjects
				.SingleOrDefault(s => s.ExternalId == externalId);

            return subject == null ? Guid.Empty : subject.SubjectId;
        }
		
        private Guid? GetSiteId(string siteCode)
        {
            if (string.IsNullOrWhiteSpace(siteCode))
                return null;
				
            var site = SiteInformation
				.SingleOrDefault(s => s.SiteCode == siteCode);

            return site == null ? Guid.Empty : site.SiteId;
        }
		
        private Guid? GetKitId(string kitNumber)
        {
            if (string.IsNullOrWhiteSpace(kitNumber))
                return null;
				
            var kit = Fulfillment_NumberedKits
				.SingleOrDefault(k => k.Spec_Number == kitNumber);

            return kit == null ? Guid.Empty : kit.KitId;
        }		
		
public class AuditTrailRecord{
	public Guid AuditTrailRecordID { get; set; }
	public string EntityName { get; set; }
	public string NewValues { get; set; }
	public string OldValues { get; set; }
	public int OperationType { get; set; }
	public DateTime DateCreatedUtc { get; set; }
	public Guid RootID { get; set; }
	public Guid AuditTrailId { get; set; }
	public Guid RootEntityId { get; set; }
	public string RootEntityIdName { get; set; }
	
	// Constructor
	public AuditTrailRecord(Guid auditTrailRecordID, string entityName, string newValues, string oldValues
							,int operationType, DateTime dateCreatedUtc, Guid rootID, Guid auditTrailId
							,Guid rootEntityId, string rootEntityIdName)
	{
		AuditTrailRecordID = auditTrailRecordID;
		EntityName = entityName;
		NewValues = newValues;
		OldValues= oldValues;
		OperationType = operationType;
		DateCreatedUtc = dateCreatedUtc;
		RootID = rootID;
		AuditTrailId = auditTrailId;
		RootEntityId = rootEntityId;
		RootEntityIdName = rootEntityIdName;
	}
	
	
	
	
}