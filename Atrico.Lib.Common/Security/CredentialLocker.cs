using System.Windows.Forms;

namespace Atrico.Lib.Common.Security
{
    public class CredentialLocker
    {
        public void Add(string username, string password)
        {
            Application.UserAppDataRegistry.SetValue(username, password.Protect());
        }

        public string Retrieve(string username)
        {
            var password = Application.UserAppDataRegistry.GetValue(username);
            return password == null ? null : password.ToString().Unprotect();
        }
    }
}
