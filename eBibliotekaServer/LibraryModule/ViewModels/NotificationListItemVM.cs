using eBibliotekaServer.AuthModule.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace eBibliotekaServer.LibraryModule.ViewModels
{
    public class NotificationListItemVM
    {
        public int ID { get; set; }
        public string LibraryName { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
    }
}
