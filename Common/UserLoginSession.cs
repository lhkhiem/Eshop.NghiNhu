using System;

namespace Common
{
    [Serializable]
    public class UserLoginSession
    {
        public long UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string UserGroupID { get; set; }
    }
}