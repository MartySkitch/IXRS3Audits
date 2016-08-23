<Query Kind="Statements">
  <Connection>
    <ID>bfed86d1-0885-452b-8d52-e748a5c23776</ID>
    <Persist>true</Persist>
    <Server>(localDB)\MSSQLLocalDB</Server>
    <Database>IXRS_Almac123456</Database>
  </Connection>
</Query>

/* 	Programmer: MFS
	Date: 29-Jul-2016
	Purpose: Give a user's name return list of active studies

*/
var userName = "support";

var users = 
UserContactInformation
   .Join (
      UserStudyAccesses, 
      user => user.UserId, 
      study => study.UserId, 
      (user, study) => new { user, study })
   	.Join (GlobalStudyInformation, 
		userstudy => userstudy.study.StudyCode, 
		studyInfo => studyInfo.StudyCode,
		(userstudy, studyInfo) => new 
		{ 	UserName = userstudy.user.FirstName + " " + userstudy.user.LastName
			,StudyCode = userstudy.study.StudyCode
			,ProjectCode = studyInfo.ProjectCode
			,IsActive = userstudy.study.IsActive} )
     .Where(m => m.UserName.Contains(userName) )
	 .OrderBy(m => m.StudyCode);

users.Dump();