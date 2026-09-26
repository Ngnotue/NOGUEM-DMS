using DocumentMS.Helpers;
using DocumentMS.Models;
using DocumentMS.Models.UserProfileViewModel;

namespace DocumentMS.Data
{
    public class SeedData
    {
        public IEnumerable<DocumentCategories> GetDocumentCategoriesList()
        {
            return new List<DocumentCategories>
            {
                new DocumentCategories { Name = "Software Engineering", Description = "Software Engineering"},
                new DocumentCategories { Name = "Approvals", Description = "Approvals"},
                new DocumentCategories { Name = "HR Policies", Description = "HR Policies"},
                new DocumentCategories { Name = "Confidential", Description = "Confidential"},
                new DocumentCategories { Name = "Quality Assurance Document", Description = "Quality Assurance Document"},


                new DocumentCategories { Name = "SOP Production", Description = "SOP Production"},
                new DocumentCategories { Name = "Govt Form", Description = "Govt Form"},
                new DocumentCategories { Name = "Resume", Description = "Resume"},
                new DocumentCategories { Name = "Terms and Conditions", Description = "Terms and Conditions"},
                new DocumentCategories { Name = "Legal Notice", Description = "Legal Notice"},

                new DocumentCategories { Name = "Offer Letter", Description = "Offer Letter"},
                new DocumentCategories { Name = "Holiday Notice", Description = "Holiday Notice"},
                new DocumentCategories { Name = "Welcome Notes", Description = "Welcome Notes"},
                new DocumentCategories { Name = "Company Policy", Description = "Company Policy"},
                new DocumentCategories { Name = "Team KPI", Description = "Team KPI"},

                new DocumentCategories { Name = "International", Description = "International"},
                new DocumentCategories { Name = "Signature", Description = "Employee Signature"},
            };
        }
        public IEnumerable<DocumentStatus> GetDocumentStatusList()
        {
            return new List<DocumentStatus>
            {
                new DocumentStatus { Name = "New", Description = "New"},
                new DocumentStatus { Name = "Draft", Description = "Draft"},
                new DocumentStatus { Name = "Reviewing", Description = "Reviewing"},
                new DocumentStatus { Name = "Signed", Description = "Signed"},
                new DocumentStatus { Name = "Approved", Description = "Approved"},

                new DocumentStatus { Name = "Rejected", Description = "Rejected"},
                new DocumentStatus { Name = "Pending", Description = "Pending"},
                new DocumentStatus { Name = "Invalid", Description = "Invalid"},
                new DocumentStatus { Name = "Obsolete", Description = "Obsolete"},
                new DocumentStatus { Name = "Completed", Description = "Completed"},
            };
        }
        public IEnumerable<Document> GetDocumentList()
        {
            return new List<Document>
            {
                new Document { Name = "adminlte.png", CategoriesId = 7, Tag01 = "tech1" },
                new Document { Name = "csharp.png", CategoriesId = 7, Tag01 = "tech2" },
                new Document { Name = "csharp2.png", CategoriesId = 7, Tag01 = "tech3" },
                new Document { Name = "dotnet.png", CategoriesId = 7, Tag01 = "tech4" },
                new Document { Name = "dotnet2.png", CategoriesId = 7, Tag01 = "tech5" },

                new Document { Name = "dotnet3.png", CategoriesId = 8, Tag01 = "tech6" },
                new Document { Name = "efcore.png", CategoriesId = 8, Tag01 = "tech7" },
                new Document { Name = "jQuery.png", CategoriesId = 8, Tag01 = "tech8" },
                new Document { Name = "MSSQL.png", CategoriesId = 8, Tag01 = "tech9" },
                new Document { Name = "MySQL.png", CategoriesId = 8, Tag01 = "tech10" },

                new Document { Name = "PostgraSQL.png", CategoriesId = 9, Tag01 = "tech11" },
                new Document { Name = "sweetalert2.png", CategoriesId = 9, Tag01 = "tech12" },

                new Document { Name = "Lorem Ipsum.docx", CategoriesId = 1, Tag01 = "Summer" },
                new Document { Name = "Lorem Ipsum.pdf", CategoriesId = 3, Tag01 = "Autumn" },
                new Document { Name = "Lorem Ipsum.txt", CategoriesId = 3, Tag01 = "Spring" },
                new Document { Name = "us-president.jpg", CategoriesId = 16, Tag01 = "Winter" },
                new Document { Name = "boris-johnson.jpg", CategoriesId = 16, Tag01 = "Rainy" },


                //Deep Search content
                new Document { Name = "Advance POS System.docx", CategoriesId = 16, Tag01 = "Deep Search" },
                new Document { Name = "ASP.NET Core MVC Web Starter Kit.pdf", CategoriesId = 16, Tag01 = "Deep Search" },
                new Document { Name = "Asset Management System.pdf", CategoriesId = 16, Tag01 = "Deep Search" },
                new Document { Name = "Business ERP Solution User Manual.pdf", CategoriesId = 16, Tag01 = "Deep Search" },
                new Document { Name = "C# Programming.docx", CategoriesId = 16, Tag01 = "Deep Search" },

                new Document { Name = "Complaint Management System.pdf", CategoriesId = 16, Tag01 = "Deep Search" },
                new Document { Name = "CRUD Operation Tutorials.pdf", CategoriesId = 16, Tag01 = "Deep Search" },
                new Document { Name = "Document Management System-User Manual.pdf", CategoriesId = 16, Tag01 = "Deep Search" },
                new Document { Name = "File Uploader User Manual.pdf", CategoriesId = 16, Tag01 = "Deep Search" },
                new Document { Name = "Full CRUD Operation Using .Net 6.0-User Manual.docx", CategoriesId = 16, Tag01 = "Deep Search" },

                new Document { Name = "Inventory Plus-User Manual.pdf", CategoriesId = 16, Tag01 = "Deep Search" },
                new Document { Name = "Leave Management System_User Manual.docx", CategoriesId = 16, Tag01 = "Deep Search" },
                new Document { Name = "ListCRUD-User Manual.docx", CategoriesId = 16, Tag01 = "Deep Search" },
                new Document { Name = "OOP.pdf", CategoriesId = 16, Tag01 = "Deep Search" },
                new Document { Name = "Pharmacy & Hospital Management System.pdf", CategoriesId = 16, Tag01 = "Deep Search" },

                new Document { Name = "Romeo and Juliet ( PDFDrive ).pdf", CategoriesId = 16, Tag01 = "Deep Search" },
                new Document { Name = "School Management ERP User Manual.docx", CategoriesId = 16, Tag01 = "Deep Search" },
                new Document { Name = "TD1.txt", CategoriesId = 16, Tag01 = "Deep Search" },
                new Document { Name = "TD2.txt", CategoriesId = 16, Tag01 = "Deep Search" },
                new Document { Name = "TD3.txt", CategoriesId = 16, Tag01 = "Deep Search" },

                new Document { Name = "User Management-Manual.docx", CategoriesId = 16, Tag01 = "Deep Search" },
                new Document { Name = "User Manual.pdf", CategoriesId = 16, Tag01 = "Deep Search" },
                new Document { Name = "User Manual-Library Management System.docx", CategoriesId = 16, Tag01 = "Deep Search" },
                new Document { Name = "ZeroByte Tech.docx", CategoriesId = 16, Tag01 = "Deep Search" },
            };
        }
        public IEnumerable<DocumentFile> GetDocumentFileList()
        {
            return new List<DocumentFile>
            {
                new DocumentFile { FilePath = "/upload/DefaultDoc/Buildwith/adminlte.png", ContentType = "image/png" },
                new DocumentFile { FilePath = "/upload/DefaultDoc/Buildwith/csharp.png", ContentType = "image/png" },
                new DocumentFile { FilePath = "/upload/DefaultDoc/Buildwith/csharp2.png", ContentType = "image/png" },
                new DocumentFile { FilePath = "/upload/DefaultDoc/Buildwith/dotnet.png", ContentType = "image/png" },
                new DocumentFile { FilePath = "/upload/DefaultDoc/Buildwith/dotnet2.png", ContentType = "image/png" },

                new DocumentFile { FilePath = "/upload/DefaultDoc/Buildwith/dotnet3.png", ContentType = "image/png" },
                new DocumentFile { FilePath = "/upload/DefaultDoc/Buildwith/efcore.png", ContentType = "image/png" },
                new DocumentFile { FilePath = "/upload/DefaultDoc/Buildwith/jQuery.png", ContentType = "image/png" },
                new DocumentFile { FilePath = "/upload/DefaultDoc/Buildwith/MSSQL.png", ContentType = "image/png" },
                new DocumentFile { FilePath = "/upload/DefaultDoc/Buildwith/MySQL.png", ContentType = "image/png" },

                new DocumentFile { FilePath = "/upload/DefaultDoc/Buildwith/PostgraSQL.png", ContentType = "image/png" },
                new DocumentFile { FilePath = "/upload/DefaultDoc/Buildwith/sweetalert2.png", ContentType = "image/png" },

                new DocumentFile { FilePath = "/upload/DefaultDoc/Lorem Ipsum.docx", ContentType = "content/docx" },
                new DocumentFile { FilePath = "/upload/DefaultDoc/Lorem Ipsum.pdf", ContentType = "content/pdf" },
                new DocumentFile { FilePath = "/upload/DefaultDoc/Lorem Ipsum.text", ContentType = "content/text" },
                new DocumentFile { FilePath = "/upload/DefaultDoc/us-president.jpg", ContentType = "image/jpg" },
                new DocumentFile { FilePath = "/upload/DefaultDoc/boris-johnson.jpg", ContentType = "image/png" },



                //Deep Search content
                new DocumentFile { FilePath = "/upload/DefaultDoc/DeepSearchTestFiles/Advance POS System.docx", ContentType = "content/docx" },
                new DocumentFile { FilePath = "/upload/DefaultDoc/DeepSearchTestFiles/ASP.NET Core MVC Web Starter Kit.pdf", ContentType = "content/pdf" },
                new DocumentFile { FilePath = "/upload/DefaultDoc/DeepSearchTestFiles/Asset Management System.pdf", ContentType = "content/pdf" },
                new DocumentFile { FilePath = "/upload/DefaultDoc/DeepSearchTestFiles/Business ERP Solution User Manual.pdf", ContentType = "content/pdf" },
                new DocumentFile { FilePath = "/upload/DefaultDoc/DeepSearchTestFiles/Charp Programming.docx", ContentType = "content/docx" },

                new DocumentFile { FilePath = "/upload/DefaultDoc/DeepSearchTestFiles/Complaint Management System.pdf", ContentType = "content/pdf" },
                new DocumentFile { FilePath = "/upload/DefaultDoc/DeepSearchTestFiles/CRUD Operation Tutorials.pdf", ContentType = "content/pdf" },
                new DocumentFile { FilePath = "/upload/DefaultDoc/DeepSearchTestFiles/Document Management System-User Manual.pdf", ContentType = "content/pdf" },
                new DocumentFile { FilePath = "/upload/DefaultDoc/DeepSearchTestFiles/File Uploader User Manual.pdf", ContentType = "content/pdf" },
                new DocumentFile { FilePath = "/upload/DefaultDoc/DeepSearchTestFiles/Full CRUD Operation Using .Net 6.0-User Manual.docx", ContentType = "content/docx" },

                new DocumentFile { FilePath = "/upload/DefaultDoc/DeepSearchTestFiles/Inventory Plus-User Manual.pdf", ContentType = "content/pdf" },
                new DocumentFile { FilePath = "/upload/DefaultDoc/DeepSearchTestFiles/Leave Management System_User Manual.docx", ContentType = "content/docx" },
                new DocumentFile { FilePath = "/upload/DefaultDoc/DeepSearchTestFiles/ListCRUD-User Manual.docx", ContentType = "content/docx" },
                new DocumentFile { FilePath = "/upload/DefaultDoc/DeepSearchTestFiles/OOP.pdf", ContentType = "content/pdf" },
                new DocumentFile { FilePath = "/upload/DefaultDoc/DeepSearchTestFiles/Pharmacy & Hospital Management System.pdf", ContentType = "content/pdf" },

                new DocumentFile { FilePath = "/upload/DefaultDoc/DeepSearchTestFiles/Romeo and Juliet ( PDFDrive ).pdf", ContentType = "content/pdf" },
                new DocumentFile { FilePath = "/upload/DefaultDoc/DeepSearchTestFiles/School Management ERP User Manual.docx", ContentType = "content/docx" },
                new DocumentFile { FilePath = "/upload/DefaultDoc/DeepSearchTestFiles/TD1.txt", ContentType = "content.txt/txt" },
                new DocumentFile { FilePath = "/upload/DefaultDoc/DeepSearchTestFiles/TD2.txt", ContentType = "content/txt" },
                new DocumentFile { FilePath = "/upload/DefaultDoc/DeepSearchTestFiles/TD3.txt", ContentType = "content/txt" },

                new DocumentFile { FilePath = "/upload/DefaultDoc/DeepSearchTestFiles/User Management-Manual.docx", ContentType = "content/docx" },
                new DocumentFile { FilePath = "/upload/DefaultDoc/DeepSearchTestFiles/User Manual.pdf", ContentType = "content/pdf" },
                new DocumentFile { FilePath = "/upload/DefaultDoc/DeepSearchTestFiles/User Manual-Library Management System.docx", ContentType = "content/docx" },
                new DocumentFile { FilePath = "/upload/DefaultDoc/DeepSearchTestFiles/ZeroByte Tech.docx", ContentType = "content/docx" },
            };
        }
        public IEnumerable<Comment> GetCommentList()
        {
            return new List<Comment>
            {
                new Comment { Message = "Auto Generated Comment at, " + string.Format("{0:f}", DateTime.Now.AddDays(-1)) },
                new Comment { Message = "Auto Generated Comment at, " + string.Format("{0:f}", DateTime.Now.AddDays(-2)) },
                new Comment { Message = "Auto Generated Comment at, " + string.Format("{0:f}", DateTime.Now.AddDays(-8)) },
                new Comment { Message = "Auto Generated Comment at, " + string.Format("{0:f}", DateTime.Now.AddDays(-15)) },
                new Comment { Message = "Auto Generated Comment at, " + string.Format("{0:f}", DateTime.Now.AddDays(-34)) }
            };
        }
        public IEnumerable<UserProfileCRUDViewModel> GetUserProfileList()
        {
            return new List<UserProfileCRUDViewModel>
            {
                new UserProfileCRUDViewModel { FirstName = "Employee 5", LastName = "User", Email = "Employee5@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/UserIcon/U1.png", Address = "California", Country = "USA", },
                new UserProfileCRUDViewModel { FirstName = "Employee 4", LastName = "User", Email = "Employee4@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/UserIcon/U2.png", Address = "California", Country = "USA", },
                new UserProfileCRUDViewModel { FirstName = "Employee 3", LastName = "User", Email = "Employee3@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/UserIcon/U3.png", Address = "California", Country = "USA", },
                new UserProfileCRUDViewModel { FirstName = "Employee 2", LastName = "User", Email = "Employee2@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/UserIcon/U4.png", Address = "California", Country = "USA", },
                new UserProfileCRUDViewModel { FirstName = "Employee 1", LastName = "User", Email = "Employee1@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/UserIcon/U5.png", Address = "California", Country = "USA", },

                new UserProfileCRUDViewModel { FirstName = "Regular", LastName = "User", Email = "regular@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/UserIcon/U6.png", Address = "California", Country = "USA", },
                new UserProfileCRUDViewModel { FirstName = "Technology", LastName = "User", Email = "tech@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/UserIcon/U7.png", Address = "California", Country = "USA", },
                new UserProfileCRUDViewModel { FirstName = "Finance", LastName = "User", Email = "finance@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/UserIcon/U8.png", Address = "California", Country = "USA", },
                new UserProfileCRUDViewModel { FirstName = "HR", LastName = "User", Email = "hr@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/UserIcon/U9.png", Address = "California", Country = "USA", },
                new UserProfileCRUDViewModel { FirstName = "Accountants", LastName = "User", Email = "accountants@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/UserIcon/U10.png", Address = "California", Country = "USA", },

                new UserProfileCRUDViewModel { FirstName = "Pharmacist 01", LastName = "User", Email = "pharmacist1@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/UserIcon/U11.png", Address = "California", Country = "USA", },
                new UserProfileCRUDViewModel { FirstName = "Pharmacist 02", LastName = "User", Email = "pharmacist2@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/UserIcon/U12.png", Address = "California", Country = "USA", },

            };
        }

        public IEnumerable<EmailConfig> GetEmailConfigList()
        {
            return new List<EmailConfig>
            {
                new EmailConfig { Email = "exmapl1@gmail.com", Password = "123", Hostname = "smtp.gmail.com", Port = 587 },
                new EmailConfig { Email = "exmapl2@gmail.com", Password = "123", Hostname = "smtp.gmail.com", Port = 587 },
                new EmailConfig { Email = "mlbddev@gmail.com", Password = "123", Hostname = "smtp.gmail.com", Port = 587 },
            };
        }
        public IEnumerable<ManageUserRoles> GetManageRoleList()
        {
            return new List<ManageUserRoles>
            {
                new ManageUserRoles { Name = "Admin", Description = "User Role: New"},
                new ManageUserRoles { Name = "General", Description = "User Role: General"},
            };
        }
        public CompanyInfo GetCompanyInfo()
        {
            return new CompanyInfo
            {
                Name = "XYZ Company Limited",
                Logo = "/upload/company_logo.png",
                Currency = "৳",
                Address = "Dhaka, Bangladesh",
                City = "Dhaka",
                Country = "Bangladesh",
                Phone = "132546789",
                Fax = "9999",
                Website = "www.wyx.com",
            };
        }
    }
}
