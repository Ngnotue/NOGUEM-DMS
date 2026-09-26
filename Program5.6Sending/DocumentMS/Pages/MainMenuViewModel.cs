namespace DocumentMS.Pages
{
    public class MainMenuViewModel
    {
        public bool Admin { get; set; }
        public bool Settings { get; set; }


        public bool Dashboard { get; set; }
        public bool UserManagement { get; set; }
        public bool UserProfile { get; set; }
        public bool ManageUserRoles { get; set; }
        public bool SystemRole { get; set; }
        public bool EmailSetting { get; set; }
        public bool IdentitySetting { get; set; }
        public bool LoginHistory { get; set; }
        public bool RefreshToken { get; set; }
        public bool CompanyInfo { get; set; }


        //DocumentMS
        public bool Document { get; set; }
        public bool DocumentDeepSearch { get; set; }
        public bool DocumentGeneral { get; set; }
        public bool DocumentCategories { get; set; }
        public bool DocumentStatus { get; set; }
        public bool Comment { get; set; }
        public bool DocumentHistory { get; set; }
        public bool EmailConfig { get; set; }
        public bool EmailTemplate { get; set; }

    }
}