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
	var subID = GetSubjectId("3");
	var siteID = GetSiteId("100");
	var kitID = GetKitId("1034");

	
	// Build the query
	var predicate = PredicateBuilder.False<AuditTrailRootEntityId>();
	
	predicate = predicate.Or(areId => areId.RootEntityId_RootEntityIdName == "SubjectId" && areId.RootEntityId_Value == subID);
//	predicate = predicate.Or(areId => areId.RootEntityId_RootEntityIdName == "SiteId" && areId.RootEntityId_Value == siteID);
	predicate = predicate.Or(areId => areId.RootEntityId_RootEntityIdName == "KitId" && areId.RootEntityId_Value == kitID);
	
	// Get the AuditTrial IDs for the query
	var auditTrialIds = AuditTrailRootEntityIds
		.Where (predicate)
		.Select(s =>  s.AuditTrailId );

	// Get the Audit Trail
	var auditTrail = AuditTrails
		.Where (at => auditTrialIds.Contains (at.Id));
		
	// Create a list of the audit trail IDs
	List<Guid> listAuditTrailIDs = auditTrail
		.Select( at => at.Id)
		.ToList();
		
	// Get all distinct Root Entity IDs
	IQueryable<RootEntity> rootEntity_ID = AuditTrailRootEntityIds
		.Where (rei =>  listAuditTrailIDs
						.Contains(rei.AuditTrailId) )
		.Select (s =>  new RootEntity(s.RootEntityId_RootEntityIdName, s.RootEntityId_IsPrimaryKey, s.RootEntityId_Value) )
		//.Select (s =>  new {s.RootEntityId_RootEntityIdName, s.RootEntityId_IsPrimaryKey, s.RootEntityId_Value} )
		.Distinct();
						
	rootEntity_ID.Dump();	
		
//	auditTrail.Dump();

}

    public class RootEntity
    {
		
		// Constructor
        public RootEntity(string rootEntityIdName, bool isPrimaryKey, Guid rootEntityId)
		{
			RootEntityIdName = rootEntityIdName;
			IsPrimaryKey = isPrimaryKey;
			RootEntityId = rootEntityId;
		}

        public bool IsPrimaryKey { get; set; }
        public string RootEntityIdName { get; set; }
        public Guid RootEntityId { get; set; }
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