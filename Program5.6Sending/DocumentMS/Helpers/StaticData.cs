using DocumentMS.Pages;
using System;

namespace DocumentMS.Helpers
{
    public static class StaticData
    {
        public static string RandomDigits(int length)
        {
            var random = new Random();
            string s = string.Empty;
            for (int i = 0; i < length; i++)
                s = String.Concat(s, random.Next(10).ToString());
            return s;
        }
        public static string GetUniqueID(string Prefix)
        {
            Random _Random = new Random();
            var result = Prefix + DateTime.Now.ToString("yyyyMMddHHmmss") + _Random.Next(1, 1000);
            return result;
        }
        public static string GetUniqueIDOnlyDate(string Prefix)
        {
            Random _Random = new Random();
            var result = Prefix + DateTime.Now.ToString("yyyyMMdd") + "_" + RandomDigits(6);
            return result;
        }
    }
    public static class DocumentStatusValue
    {
        public const int New = 1;
        public const int Approved = 2;
        public const int Reviewing = 3;
    }

    public static class DefaultUserPage
    {
        public static readonly string[] PageCollection =
            {
                MainMenu.Dashboard.PageName,
                MainMenu.UserProfile.PageName,
            };
    }
    public static class PaymentStatus
    {
        public const string Paid = "Paid";
        public const string Unpaid = "Unpaid";
    }
    public static class CRUD
    {
        public const int Add = 1;
        public const int Edit = 2;
        public const int View = 3;
        public const int Delete = 4;
    }
    public static class ConnectionStrings
    {
        public const string connMSSQLNoCred = "connMSSQLNoCred";
        // For deployed remote database
        public const string connMSSQLDeployed = "connMSSQLDeployed";
        public const string connMSSQL = "connMSSQL";
        public const string connPostgreSQL = "connPostgreSQL";
        public const string connMySQL = "connMySQL";
        public const string connDockerBase = "connDockerBase";
        public const string connMSSQLProd = "connMSSQLProd";
        public const string connOthers = "connOthers";
    }
    public static class EmailContent
    {
        public const string Subject = "NOGUEM-Dokumentenmanagementsystem: ";
        public const string Body = "Hallo\nBitte holen Sie sich das Dokument aus dem E-Mail-Anhang.\n\nVielen Dank\nAdministrator, Dokumentenmanagementsystem\n";
    }
}
