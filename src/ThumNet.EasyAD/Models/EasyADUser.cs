namespace ThumNet.EasyAD.Models
{
    public class EasyADUser
    {
        public string Login { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }

        public override int GetHashCode()
        {
            return Login.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return Login == (obj as EasyADUser).Login;
        }
    }
}
