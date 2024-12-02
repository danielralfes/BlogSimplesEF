using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace BlogSimplesEF.Web.Models.Entities
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Summary { get; set; }
        public string? Content { get; set; }
        public DateTime? PublishedOn { get; set; }
        [Key, ForeignKey("UserBlog")]
        public string UserId { get; set; }
        public virtual AspNetUsers UserBlog { get; set; }
    }
    public class AspNetUsers : IdentityUser
    {    }
}