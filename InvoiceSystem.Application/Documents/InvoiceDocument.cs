using InvoiceSystem.Application.Dtos;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceSystem.Application.Documents
{
    public class InvoiceDocument : IDocument
    {
        private readonly OrderResponse _order;
        public InvoiceDocument(OrderResponse order)
        {
            _order = order;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
        public void Compose(IDocumentContainer container)
        {
            throw new NotImplementedException();
        }
    }
}
