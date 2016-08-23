<Query Kind="Statements">
  <Connection>
    <ID>bfed86d1-0885-452b-8d52-e748a5c23776</ID>
    <Persist>true</Persist>
    <Server>(localDB)\MSSQLLocalDB</Server>
    <Database>IXRS_Almac123456</Database>
  </Connection>
</Query>

var auditTrailRecords = AuditTrails
	.Join(AuditTrailRootEntityIds,
		at => at.Id,
		root => root.AuditTrailId,
		(at, root) => new {at.Id, at.EntityModifications_EntityName, at.EntityModifications_NewValues, at.EntityModifications_OldValues, at.EntityModifications_OperationType,
							at.DateCreatedUtc, EntityID = root.Id, root.AuditTrailId, root.RootEntityId_Value   } )
	.Where(a => a.Id == new Guid("863297C7-7E55-4603-8275-A63A010F7428"));

auditTrailRecords.Dump();