using DocumentMS.Data;
using DocumentMS.Helpers;
using DocumentMS.Models;
using DocumentMS.Models.DocumentHistoryViewModel;
using DocumentMS.Models.DocumentViewModel;
using DocumentMS.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DocumentMS.ConHelper
{
    public class DBOperation : IDBOperation
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        public DBOperation(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }

        public async Task AddDocumentHistory(AddDocumentHistoryViewModel vm)
        {
            DocumentHistoryCRUDViewModel _AssetHistoryCRUDViewModel = vm;
            var result = await _iCommon.AddDocumentHistory(_AssetHistoryCRUDViewModel);
        }
        public async Task<List<DocumentFile>> AddMultipleFile(DocumentCRUDViewModel vm)
        {
            List<DocumentFile> listDocumentFile = new();
            try
            {
                string _FileName = string.Empty;
                if (vm.IsFileSaveInDB == false)
                {
                    var _GetServerFileDir = _iCommon.GetServerFileDir() + "/" + vm.FilesDirName;
                    Directory.CreateDirectory(_GetServerFileDir);
                }

                foreach (var _item in vm.lisFiles)
                {
                    DocumentFile _DocumentFile = new();
                    _DocumentFile.DocumentId = vm.Id;
                    _DocumentFile.IsFileSaveInDB = vm.IsFileSaveInDB;
                    if (vm.IsFileSaveInDB)
                    {
                        _FileName = _item.FileName;
                        using (var _MemoryStream = new MemoryStream())
                        {
                            _item.CopyTo(_MemoryStream);
                            _DocumentFile.DocByte = _MemoryStream.ToArray();
                            _DocumentFile.ContentType = _item.ContentType;
                            _DocumentFile.Length = Convert.ToDouble((_item.Length / 1000).ToString("0.0000"));
                        }
                        _DocumentFile.DocByte = StaticUtility.Encrypt(_DocumentFile.DocByte, _FileName);
                    }
                    else
                    {
                        _FileName = _iCommon.UploadedFile(_item, vm.FilesDirName);
                        _DocumentFile.ContentType = _item.ContentType;
                        _DocumentFile.Length = Convert.ToDouble((_item.Length / 1000).ToString("0.0000"));
                        _DocumentFile.FilePath = "/upload/" + vm.FilesDirName + "/" + _FileName;
                    }

                    _DocumentFile.Name = _FileName;
                    _DocumentFile.CreatedDate = DateTime.Now;
                    _DocumentFile.ModifiedDate = DateTime.Now;
                    _DocumentFile.CreatedBy = vm.UserName;
                    _DocumentFile.ModifiedBy = vm.UserName;
                    listDocumentFile.Add(_DocumentFile);
                }
                await _context.AddRangeAsync(listDocumentFile);
                await _context.SaveChangesAsync();

                return listDocumentFile;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DocumentFile> AddDefaultFile(DocumentCRUDViewModel vm)
        {
            DocumentFile _DocumentFile = new();
            try
            {
                _DocumentFile.Name = "Lorem Ipsum.txt";

                _DocumentFile.DocumentId = vm.Id;
                _DocumentFile.IsFileSaveInDB = vm.IsFileSaveInDB;

                if (vm.IsFileSaveInDB == true)
                {
                    var _GetServerFileDir = _iCommon.GetServerFileDir();
                    _DocumentFile.DocByte = System.IO.File.ReadAllBytes(_GetServerFileDir + "/DefaultDoc/Lorem Ipsum.txt");
                    _DocumentFile.ContentType = "content/txt";
                    _DocumentFile.Length = 3.21;
                    _DocumentFile.DocByte = StaticUtility.Encrypt(_DocumentFile.DocByte, _DocumentFile.Name);
                }
                else
                {
                    _DocumentFile.ContentType = "content/txt";
                    _DocumentFile.Length = 3.21;
                    _DocumentFile.FilePath = "/upload/DefaultDoc/" + _DocumentFile.Name;
                }

                _DocumentFile.CreatedDate = DateTime.Now;
                _DocumentFile.ModifiedDate = DateTime.Now;
                _DocumentFile.CreatedBy = vm.UserName;
                _DocumentFile.ModifiedBy = vm.UserName;
                _context.Add(_DocumentFile);
                await _context.SaveChangesAsync();

                return _DocumentFile;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> AddDocumentFileContent(List<DocumentFile> listDocumentFile, string UserName)
        {
            try
            {
                foreach (var item in listDocumentFile)
                {
                    DocumentFileContent _DocumentFileContent = new();
                    _DocumentFileContent.DocumentId = item.DocumentId;
                    _DocumentFileContent.DocumentFileId = item.Id;

                    var _GetWWWRootPath = _iCommon.GetWWWRootPath() + "/";
                    var _FilePath = Path.Combine(_GetWWWRootPath + item.FilePath);
                    string _Content = "";

                    string _GetExtension = Path.GetExtension(_FilePath);
                    if (_GetExtension == ".docx" || _GetExtension == ".doc")
                    {
                        _Content = SearchInFileService.GetTextFromWord(_FilePath);
                    }
                    else if (_GetExtension == ".pdf")
                    {
                        _Content = SearchInFileService.GetTextFromPDF(_FilePath);
                    }
                    else if (_GetExtension == ".txt")
                    {
                        _Content = SearchInFileService.GetTextFromText(_FilePath);
                    }

                    _DocumentFileContent.Content = _Content;
                    _DocumentFileContent.ContentType = item.ContentType;
                    _DocumentFileContent.CreatedDate = DateTime.Now;
                    _DocumentFileContent.ModifiedDate = DateTime.Now;
                    _DocumentFileContent.CreatedBy = UserName;
                    _DocumentFileContent.ModifiedBy = UserName;
                    _context.Add(_DocumentFileContent);
                    await _context.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
    }
}
