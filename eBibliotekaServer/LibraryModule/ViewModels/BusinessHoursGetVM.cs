﻿namespace eBibliotekaServer.LibraryModule.ViewModels
{
    public class BusinessHoursGetVM
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string TimeFrom { get; set; }
        public string TimeTo { get; set; }
        public bool WorkingDay { get; set; }
    }
}
