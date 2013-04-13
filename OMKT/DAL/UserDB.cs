namespace OMKT.DAL
{
    public class UserDB
    {
        //private readonly OMKTDB db;

        //public UserDB()
        //{
        //    db = new OMKTDB();
        //}

        //public MembershipUser CreateUser(string username, string password, string email)
        //{
        //    var oUser = new User
        //                    {
        //                        Name = "David",
        //                        Lastname = "Fernandez",
        //                        Email = email,
        //                        DateOfBirth = DateTime.Now,
        //                        Password = password,
        //                        Remember = true,
        //                        IsActivated = true,
        //                        CreationDate = DateTime.Now,
        //                        LastUpdated = DateTime.Now,
        //                        LastLoginDate = DateTime.Now,
        //                        LastLoginIp = ""
        //                    };


        //    db.Users.Add(oUser);
        //    db.SaveChanges();

        //    return GetUser(email);
        //}

        //public string GetUserNameByEmail(string email)
        //{
        //    var result = from u in db.Users where (u.Email == email) select u;

        //    if (result.Count() != 0)
        //    {
        //        var dbuser = result.FirstOrDefault();

        //        return dbuser.Name;
        //    }
        //    else
        //    {
        //        return "";
        //    }

        //}

        //public MembershipUser GetUser(string email)
        //{
        //    var result = from u in db.Users where (u.Email == email) select u;

        //    if (result.Count() != 0)
        //    {
        //        var dbuser = result.FirstOrDefault();

        //        string _username = dbuser.Email;
        //        int _providerUserKey = dbuser.UserID;
        //        string _email = dbuser.Email;
        //        string _passwordQuestion = dbuser.PasswordQuestion;
        //        string _comment = "";
        //        bool _isApproved = dbuser.IsActivated;
        //        bool _isLockedOut = dbuser.IsLockedOut;
        //        DateTime _creationDate = dbuser.CreationDate;
        //        DateTime _lastLoginDate = dbuser.LastLoginDate;
        //        DateTime _lastActivityDate = DateTime.Now;
        //        DateTime _lastPasswordChangedDate = DateTime.Now;
        //        DateTime _lastLockedOutDate = dbuser.LastLockedOutDate;

        //        var user = new MembershipUser("OpticalMarketingMembershipProvider",
        //                                      _username,
        //                                      _providerUserKey,
        //                                      _email,
        //                                      _passwordQuestion,
        //                                      _comment,
        //                                      _isApproved,
        //                                      _isLockedOut,
        //                                      _creationDate,
        //                                      _lastLoginDate,
        //                                      _lastActivityDate,
        //                                      _lastPasswordChangedDate,
        //                                      _lastLockedOutDate
        //            );

        //        return user;
        //    }
        //    return null;
        //}

        //public bool ValidateUser(string email, string password)
        //{
        //    var result = from u in db.Users where (u.Email == email) select u;

        //    if (result.Count() != 0)
        //    {
        //        var dbuser = result.First();

        //        if (dbuser.Password == password)
        //            return true;
        //        else
        //            return false;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //private static string CreateSalt()
        //{
        //    var rng = new RNGCryptoServiceProvider();
        //    var buff = new byte[32];
        //    rng.GetBytes(buff);

        //    return Convert.ToBase64String(buff);
        //}
    }
}




