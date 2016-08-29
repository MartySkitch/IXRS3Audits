<Query Kind="Statements">
  <Namespace>System.Security.Cryptography</Namespace>
</Query>



	string value = "Viet-Nam";
	var provider = new MD5CryptoServiceProvider();
	byte[] inputBytes = Encoding.Default.GetBytes(value);
	byte[] hashBytes = provider.ComputeHash(inputBytes);
	var countryID = new Guid(hashBytes);
	
	countryID.Dump();