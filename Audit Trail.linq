<Query Kind="Program">
  <Connection>
    <ID>bfed86d1-0885-452b-8d52-e748a5c23776</ID>
    <Persist>true</Persist>
    <Server>(localDB)\MSSQLLocalDB</Server>
    <Database>IXRS_Almac123456</Database>
  </Connection>
  <Reference Relative="DLLs\Excel.dll">D:\Skitch\Audits\IXRS 3.0 Audit\DLLs\Excel.dll</Reference>
  <Reference Relative="DLLs\ICSharpCode.SharpZipLib.dll">D:\Skitch\Audits\IXRS 3.0 Audit\DLLs\ICSharpCode.SharpZipLib.dll</Reference>
  <Reference Relative="DLLs\IXRS.CountryManagement.CountryList.dll">D:\Skitch\Audits\IXRS 3.0 Audit\DLLs\IXRS.CountryManagement.CountryList.dll</Reference>
  <Namespace>IXRS.CountryManagement</Namespace>
  <IncludePredicateBuilder>true</IncludePredicateBuilder>
</Query>

void Main()
{
	var subID = GetSubjectId("1");
	var siteID = GetSiteId("100");
	var kitID = GetKitId("1034");

	
	// Build the query
	var predicate = PredicateBuilder.False<AuditTrailRootEntityId>();
	
	predicate = predicate.Or(areId => areId.RootEntityId_RootEntityIdName == "SubjectId" && areId.RootEntityId_Value == subID);
//	predicate = predicate.Or(areId => areId.RootEntityId_RootEntityIdName == "SiteId" && areId.RootEntityId_Value == siteID);
//	predicate = predicate.Or(areId => areId.RootEntityId_RootEntityIdName == "KitId" && areId.RootEntityId_Value == kitID);
	
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
	IEnumerable<RootEntity> rootEntity_ID = AuditTrailRootEntityIds
		.Where (rei =>  listAuditTrailIDs
						.Contains(rei.AuditTrailId) )
		.Select (s =>  new RootEntity(s.RootEntityId_RootEntityIdName, s.RootEntityId_IsPrimaryKey, s.RootEntityId_Value) )
		//.Select (s =>  new {s.RootEntityId_RootEntityIdName, s.RootEntityId_IsPrimaryKey, s.RootEntityId_Value} )
		.Distinct();
		
	var rootEntityConversion = GetRootEntityConversion(rootEntity_ID);
	
	rootEntityConversion.Dump();
						
	//rootEntity_ID.Dump();	
		
//	auditTrail.Dump();

}

// Classes

    public class RootEntity
    {
        public bool IsPrimaryKey { get; set; }
        public string RootEntityIdName { get; set; }
        public Guid RootEntityId { get; set; }
		
		// Constructor
        public RootEntity(string rootEntityIdName, bool isPrimaryKey, Guid rootEntityId)
		{
			RootEntityIdName = rootEntityIdName;
			IsPrimaryKey = isPrimaryKey;
			RootEntityId = rootEntityId;
		}
    }
	
    public class RootEntityConversion
    {
        public Guid RootEntityId { get; set;} 
        public string Value { get; set; }
        public string RootEntityIdName { get; set; }
		
		// Constructor
		public RootEntityConversion(string rootEntityIdName, string value, Guid rootEntityId)
		{
			RootEntityIdName = rootEntityIdName;
			Value = value;
			RootEntityId = rootEntityId;
		}
    }
	
 //	Define other methods 
 
		private List<RootEntityConversion> GetRootEntityConversion(IEnumerable<RootEntity> rootEntity)
		{
			var rootEntityConversion = new List<RootEntityConversion>();
			CountryProvider countryProvider = new CountryProvider();
		
			var rootEntityIdGroupingByEntityName = rootEntity.GroupBy(rei => rei.RootEntityIdName);
			    foreach (var rootEntityIdsForEntityName in rootEntityIdGroupingByEntityName)
				{
					var distinctRootEntityIds = rootEntityIdsForEntityName.Select(rei => rei.RootEntityId).Distinct();
					switch (rootEntityIdsForEntityName.Key)
					{
                    	case "CountryId":
                            rootEntityConversion.AddRange(countryProvider.GetAllCountries().Where(c => distinctRootEntityIds.Contains(c.CountryId))
                                .Select(country => new RootEntityConversion( "CountryId", country.Name, country.CountryId )));
                        break;					
						case "KitId":
							rootEntityConversion.AddRange(Fulfillment_NumberedKits.Where(nk => distinctRootEntityIds.Contains(nk.KitId))
                                .Select(kit => new RootEntityConversion( "KitId", kit.Spec_Number.ToString(), kit.KitId ) ));
						break;
						case "KitTypeId":
							rootEntityConversion.AddRange(KitTypes.Where(kt => distinctRootEntityIds.Contains(kt.KitTypeId))
                                .Select(kitType => new RootEntityConversion( "KitTypeId", kitType.Code, kitType.KitTypeId ) ));
						break;
						case "LotId":  // need inner join
                            rootEntityConversion.AddRange(Fulfillment_Lots.Where(lot => distinctRootEntityIds.Contains(lot.LotId))
                                .Join(LotNumbers, l => l.OrderingLotNumberId, ln => ln.LotNumberId, (lot, lotNumber) => new {lot, lotNumber})
                                .Select(x => new RootEntityConversion( "LotId", x.lotNumber.Number.ToString(), x.lotNumber.LotNumberId )));
                        break;
						case "SiteId":
							rootEntityConversion.AddRange(SiteInformation.Where(si => distinctRootEntityIds.Contains(si.SiteId))
                                .Select(site => new RootEntityConversion( "SiteId", site.SiteCode, site.SiteId ) ));
						break;		
						case "SubjectId":
							rootEntityConversion.AddRange(ScreenedSubjects.Where(ss => distinctRootEntityIds.Contains(ss.SubjectId))
                                .Select(subject => new RootEntityConversion( "SubjectId", subject.ExternalId, subject.SubjectId ) ));
						break;		
						case "TreatmentArmId":
							rootEntityConversion.AddRange(TreatmentArms.Where(ta => distinctRootEntityIds.Contains(ta.Id))
                                .Select(treatmentArm => new RootEntityConversion( "TreatmentArmId", treatmentArm.Description, treatmentArm.Id ) ));
						break;								
//						case "UserId":  // need to figure out how to connect to second database
//                        var globalDB = new DataContext(@"Data Source=SO-DB-AG1.live.ext,3344;Integrated Security=SSPI;Initial Catalog=IXRS_Global;app=LINQPad [Get Studies]");
//                             rootEntityConversion.AddRange(globalDB.GetTable<UserAccount>().Where(ua => distinctRootEntityIds.Contains(ua.UserId))
//                                 .Select(user => new RootEntityIdConversion { Id = user.UserId, Value = user.UserName, RootEntityIdName = RootEntityIdNames.User }));
//						break;								
						case "VisitId":
							rootEntityConversion.AddRange(Visits.Where(v => distinctRootEntityIds.Contains(v.VisitId))
                                .Select(visit => new RootEntityConversion( "VisitId", visit.Name, visit.VisitId ) ));
						break;						
					}
				}
			return rootEntityConversion;
		}
		

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