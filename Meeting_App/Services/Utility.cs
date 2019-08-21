using System;
using System.Linq;
using System.Text;

namespace Meeting_App.Services
{
    public static class Utility
    {
        public static string EncryptPass(string password)
        {
            var msg = "";
            var encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            msg = Convert.ToBase64String(encode);
            return msg;
        }

        public static string DecryptPass(string password)
        {
            var data = Convert.FromBase64String(password);
            var decodedString = Encoding.UTF8.GetString(data);
            return decodedString;
        }

        public static bool PasswordValidation(string passWord)
        {
            //   = { "aX2#", "sed2T", "*v3X", "Ae234&B", "fg234", "g1HL", "#1$23", "5a7%" };

            var b = ValidatePassword(passWord);


            return b;
        }

        private static bool ValidatePassword(string passWord)

        {
            var validConditions = 0;

            foreach (var c in passWord)

            {

                if (c >= 'a' && c <= 'z')

                {

                    validConditions++;

                    break;

                }

            }

            foreach (char c in passWord)

            {

                if (c >= 'A' && c <= 'Z')

                {

                    validConditions++;

                    break;

                }

            }

            if (validConditions == 0) return false;

            foreach (char c in passWord)

            {

                if (c >= '0' && c <= '9')

                {

                    validConditions++;

                    break;

                }

            }

            if (validConditions == 1) return false;

            if (validConditions == 2)

            {

                char[] special = { '@', '#', '$', '%', '^', '&', '+', '=' }; // or whatever

                if (passWord.IndexOfAny(special) == -1) return false;

            }

            return true;

        }

        public static bool BeAValidDate(DateTime date)
        {
            return !date.Equals(default(DateTime));
        }

        public static bool CheckAttendeeLength(string attendees)
        {
            var values = attendees.Split(';');

            return values.Length > 0 && values.Length <= 10;

        }

        public static bool IsAttendeeHaveValidData(string attendees)
        {
            var attendeeCollection = attendees.Split(';');

            var isIntValue = 0;

            return attendeeCollection.All(x => int.TryParse(x, out isIntValue));

        }

    }

    
    public static class PasswordExtensions
    {
        public static string Decrypt(this string passWord)
        {
            return Utility.DecryptPass(passWord);
        }

        public static string EnCrypt(this string passWord)
        {
            return Utility.EncryptPass(passWord);
        }
    }
}