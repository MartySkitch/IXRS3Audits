<Query Kind="Statements">
  <Connection>
    <ID>623e535d-fd44-41dc-a5c5-888da12d3d4e</ID>
    <Persist>true</Persist>
    <Server>SO-DB-AG1.live.ext,3344</Server>
    <Database>IXRS_Global</Database>
    <IsProduction>true</IsProduction>
  </Connection>
</Query>

/* 	Programmer: MFS
	Date: 29-Jul-2016
	Purpose: Give a user's name return list of active studies
	
	ToDo: Add preicate to accept either first or last name or email

*/
var userName = "Rodriguez";

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
		{ 	UserFirstName = userstudy.user.FirstName
			,UserLastName = userstudy.user.LastName
			,StudyCode = userstudy.study.StudyCode
			,ProjectCode = studyInfo.ProjectCode
			,IsActive = userstudy.study.IsActive} )
     .Where(m => m.UserLastName.Contains(userName)  )
	 .OrderBy(m => m.StudyCode);

users.Dump();