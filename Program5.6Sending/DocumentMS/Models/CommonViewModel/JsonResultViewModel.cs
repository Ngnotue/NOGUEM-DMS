using System;

namespace DocumentMS.Models.CommonViewModel
{
    public class JsonResultViewModel
    {
        public Int64 Id { get; set; }
        public string AlertMessage { get; set; }
        public string CurrentURL { get; set; }
        public bool IsSuccess { get; set; }
        public int OperationTyep { get; set; }
    }
}
