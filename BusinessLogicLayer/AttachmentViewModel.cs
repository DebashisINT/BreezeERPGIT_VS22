using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class AttachmentViewModel
    {
        public String id { get; set; }

        public String Type { get; set; }
    }

    public class AttachmentDocViewModel
    {
        public Int64 doc_id { get; set; }

        public Int64 doc_documentTypeId { get; set; }

        public String doc_documentName { get; set; }

        public String doc_source { get; set; }

        public String doc_Note1 { get; set; }

        public String doc_documentType { get; set; }

        public String doc_FileNo { get; set; }
        public String doc_receivedate { get; set; }
    }
}
