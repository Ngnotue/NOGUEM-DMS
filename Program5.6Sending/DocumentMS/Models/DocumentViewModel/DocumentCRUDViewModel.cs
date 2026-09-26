using DocumentMS.Models.CommentViewModel;
using DocumentMS.Models.DocumentHistoryViewModel;
using DocumentMS.Models.EmailConfigViewModel;
using DocumentMS.Models.UserProfileViewModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DocumentMS.Models.DocumentViewModel
{
    public class DocumentCRUDViewModel : EntityBase
    {
        [Display(Name = "SL"), Required]
        public Int64 Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Display(Name = "Categories")]
        public Int64 CategoriesId { get; set; }
        public string CategoriesDisplay { get; set; }
        public string Notes { get; set; }
        [Display(Name = "Assign Employee")]
        public Int64 AssignEmployeeId { get; set; }
        public string AssignEmployeeDisplay { get; set; }
        [Display(Name = "Document Status")]
        public int DocumentStatus { get; set; }
        public string DocumentStatusDisplay { get; set; }
        [Display(Name = "Save In DB")]
        public bool IsFileSaveInDB { get; set; }
        [Display(Name = "Files Dir Name")]
        public string FilesDirName { get; set; }
        [Display(Name = "Files Path")]
        public string FilesPath { get; set; }

        [Display(Name = "Tag 01")]
        public string Tag01 { get; set; }
        [Display(Name = "Tag 02")]
        public string Tag02 { get; set; }
        [Display(Name = "Tag 03")]
        public string Tag03 { get; set; }
        [Display(Name = "Tag 04")]
        public string Tag04 { get; set; }
        [Display(Name = "Tag 05")]
        public string Tag05 { get; set; }
        public string DocByteBase64 { get; set; }
        public string FileViewName { get; set; }
        public string UserName { get; set; }
        public IFormFile Files { get; set; }
        public IFormFileCollection lisFiles { get; set; }    
        public string CurrentUserId { get; set; }
        public SendEmailViewModel SendEmailViewModel { get; set; }
        public CommentCRUDViewModel CommentCRUDViewModel { get; set; }
        public UserProfileCRUDViewModel UserProfileCRUDViewModel { get; set; }       
        public List<Comment> listComment { get; set; }
        public List<DocumentHistoryCRUDViewModel> listDocumentHistoryCRUDViewModel { get; set; }
        public List<DocumentFileCRUDViewModel> listDocumentFileCRUDViewModel { get; set; }
        // Extension
        // List of users allowed to view this document
        public List<UserProfile> listUsersAllowedCRUDViewModel { get; set; }
        // Place holder for selected user in the select dropdownlist
        public List<UserProfile> SelectedUserProfiles { get; set; }
        public bool IsUserAllowed { get; set; }
        public bool IsUserAssigned{ get; set; }

        public static implicit operator DocumentCRUDViewModel(Document _Document)
        {
            return new DocumentCRUDViewModel
            {
                Id = _Document.Id,
                Name = _Document.Name,
                CategoriesId = _Document.CategoriesId,
                Notes = _Document.Notes,
                AssignEmployeeId = _Document.AssignEmployeeId,
                DocumentStatus = _Document.DocumentStatus,
                IsFileSaveInDB = _Document.IsFileSaveInDB,
                FilesDirName = _Document.FilesDirName,
                FilesPath = _Document.FilesPath,
                Tag01 = _Document.Tag01,
                Tag02 = _Document.Tag02,
                Tag03 = _Document.Tag03,
                Tag04 = _Document.Tag04,
                Tag05 = _Document.Tag05,
                // Extension
                // List of users allowed to view this document
                listUsersAllowedCRUDViewModel = _Document.SharedUsers.ToList(),
                        
                CreatedDate = _Document.CreatedDate,
                ModifiedDate = _Document.ModifiedDate,
                CreatedBy = _Document.CreatedBy,
                ModifiedBy = _Document.ModifiedBy,
                Cancelled = _Document.Cancelled,
            };
        }

        public static implicit operator Document(DocumentCRUDViewModel vm)
        {
            return new Document
            {
                Id = vm.Id,
                Name = vm.Name,
                CategoriesId = vm.CategoriesId,
                Notes = vm.Notes,
                AssignEmployeeId = vm.AssignEmployeeId,
                DocumentStatus = vm.DocumentStatus,
                IsFileSaveInDB = vm.IsFileSaveInDB,
                FilesDirName = vm.FilesDirName,
                Tag01 = vm.Tag01,
                Tag02 = vm.Tag02,
                Tag03 = vm.Tag03,
                Tag04 = vm.Tag04,
                Tag05 = vm.Tag05,
                // Extension
                SharedUsers = vm.listUsersAllowedCRUDViewModel,

                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }
    }
}
