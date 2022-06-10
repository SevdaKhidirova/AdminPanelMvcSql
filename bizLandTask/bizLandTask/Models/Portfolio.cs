using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace bizLandTask.Models
{
    public class Portfolio : BaseEntity
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public string Image { get; set; }

        [NotMapped]
        public IFormFile Img { get; set; }
    }
}
