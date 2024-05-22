using System.Net.Mail;

namespace prbd_2324_a01.Utils
{
    internal class Validations
    {
        public static bool Mail(string email) {
            var valid = true;

            try {
                var emailAddress = new MailAddress(email);
            } catch {
                valid = false;
            }
            return valid;
        }
    }
}
